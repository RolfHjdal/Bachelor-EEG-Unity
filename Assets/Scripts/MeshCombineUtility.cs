using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshCombineUtility
{
	private readonly List<Color> _colors = new List<Color>();
	private readonly bool _generateStrips;
	private readonly List<Vector3> _normals = new List<Vector3>();
	private readonly Dictionary<int, List<int>> _strip = new Dictionary<int, List<int>>();
	private readonly List<Vector4> _tangents = new List<Vector4>();
	private readonly Dictionary<int, List<int>> _triangles = new Dictionary<int, List<int>>();
	private readonly List<Vector2> _uv = new List<Vector2>();
	private readonly List<Vector2> _uv1 = new List<Vector2>();
	private readonly List<Vector3> _vertices = new List<Vector3>();
	
	/// <summary>
	/// Creates a new, empty MeshCombineUtility object for combining meshes.
	/// </summary>
	/// <param name="generateStrips">true if the meshes you're going to combine are all triangle-strip based; false if you just want to use triangle lists.</param>
	public MeshCombineUtility(bool generateStrips)
	{
		_generateStrips = generateStrips;
	}
	
	/// <summary>
	/// Allocate space for adding a load more vertex data.
	/// </summary>
	/// <param name="numVertices">The number of vertices you're about to add.</param>
	public void PrepareForAddingVertices(int numVertices)
	{
		int shortfall = numVertices - (_vertices.Capacity - _vertices.Count);
		
		_vertices.Capacity += shortfall;
		_normals.Capacity += shortfall;
		_tangents.Capacity += shortfall;
		_uv.Capacity += shortfall;
		_uv1.Capacity += shortfall;
		_colors.Capacity += shortfall;
	}
	
	/// <summary>
	/// Allocate space for adding a load more triangles to the triangle list.
	/// </summary>
	/// <param name="targetSubmeshIndex">The index of the submesh that you're going to add triangles to.</param>
	/// <param name="numIndices">The number of triangle indicies (number of triangles * 3) that you want to reserve space for.</param>
	public void PrepareForAddingTriangles(int targetSubmeshIndex, int numIndices)
	{
		if (!_triangles.ContainsKey(targetSubmeshIndex))
			_triangles.Add(targetSubmeshIndex, new List<int>());
		
		int shortfall = numIndices - (_triangles[targetSubmeshIndex].Capacity - _triangles[targetSubmeshIndex].Count);
		_triangles[targetSubmeshIndex].Capacity += shortfall;
	}
	
	/// <summary>
	/// Allocate space for adding a load more triangle strips to the combiner.
	/// </summary>
	/// <param name="targetSubmeshIndex">The index of the submesh you're going to add strips to.</param>
	/// <param name="stripLengths">A sequence of strip lengths (in number-of-indices). These numbers will be used to automatically calculate how many bridging indices need to be added.</param>
	public void PrepareForAddingStrips(int targetSubmeshIndex, IEnumerable<int> stripLengths)
	{
		if (!_strip.ContainsKey(targetSubmeshIndex))
			_strip.Add(targetSubmeshIndex, new List<int>());
		
		int requiredCapacity = _strip[targetSubmeshIndex].Count;
		
		foreach (int srcStripLength in stripLengths)
		{
			int adjStripLength = srcStripLength;
			if (requiredCapacity > 0)
				if ((requiredCapacity & 1) == 1)
					adjStripLength += 3;
			else
				adjStripLength += 2;
			requiredCapacity += adjStripLength;
		}
		
		if (_strip[targetSubmeshIndex].Capacity < requiredCapacity)
			_strip[targetSubmeshIndex].Capacity = requiredCapacity;
	}
	
	/// <summary>
	/// Add a mesh instance to the combiner.
	/// </summary>
	/// <param name="instance">The mesh instance to add.</param>
	public void AddMeshInstance(MeshInstance instance)
	{
		int baseVertexIndex = _vertices.Count;
		
		PrepareForAddingVertices(instance.mesh.vertexCount);
		
		_vertices.AddRange(instance.mesh.vertices.Select(v => instance.transform.MultiplyPoint(v)));
		_normals.AddRange(
			instance.mesh.normals.Select(n => instance.transform.inverse.transpose.MultiplyVector(n).normalized));
		_tangents.AddRange(instance.mesh.tangents.Select(t =>
		                                                 {
			var p = new Vector3(t.x, t.y, t.z);
			p =
				instance.transform.inverse.transpose.
					MultiplyVector(p).normalized;
			return new Vector4(p.x, p.y, p.z, t.w);
		}));
		_uv.AddRange(instance.mesh.uv);
		_uv1.AddRange(instance.mesh.uv1);
		_colors.AddRange(instance.mesh.colors);
		
		if (_generateStrips)
		{
			int[] inputstrip = instance.mesh.GetTriangleStrip(instance.subMeshIndex);
			PrepareForAddingStrips(instance.targetSubMeshIndex, new[] {inputstrip.Length});
			List<int> outputstrip = _strip[instance.targetSubMeshIndex];
			if (outputstrip.Count != 0)
			{
				if ((outputstrip.Count & 1) == 1)
				{
					outputstrip.Add(outputstrip[outputstrip.Count - 1]);
					outputstrip.Add(inputstrip[0] + baseVertexIndex);
					outputstrip.Add(inputstrip[0] + baseVertexIndex);
				}
				else
				{
					outputstrip.Add(outputstrip[outputstrip.Count - 1]);
					outputstrip.Add(inputstrip[0] + baseVertexIndex);
				}
			}
			
			outputstrip.AddRange(inputstrip.Select(s => s + baseVertexIndex));
		}
		else
		{
			int[] inputtriangles = instance.mesh.GetTriangles(instance.subMeshIndex);
			PrepareForAddingTriangles(instance.targetSubMeshIndex, inputtriangles.Length);
			_triangles[instance.targetSubMeshIndex].AddRange(inputtriangles.Select(t => t + baseVertexIndex));
		}
	}
	
	/// <summary>
	/// Add multiple mesh instances to the combiner, allocating space for them all up-front.
	/// </summary>
	/// <param name="instances">The instances to add.</param>
	public void AddMeshInstances(IEnumerable<MeshInstance> instances)
	{
		instances = instances.Where(instance => instance.mesh);
		
		PrepareForAddingVertices(instances.Sum(instance => instance.mesh.vertexCount));
		
		foreach (var targetSubmesh in instances.GroupBy(instance => instance.targetSubMeshIndex))
		{
			if (_generateStrips)
			{
				PrepareForAddingStrips(targetSubmesh.Key,
				                       targetSubmesh.Select(instance => instance.mesh.GetTriangleStrip(instance.subMeshIndex).Length));
			}
			else
			{
				PrepareForAddingTriangles(targetSubmesh.Key,
				                          targetSubmesh.Sum(instance => instance.mesh.GetTriangles(instance.subMeshIndex).Length));
			}
		}
		
		foreach (MeshInstance instance in instances)
			AddMeshInstance(instance);
	}
	
	/// <summary>
	/// Generate a single mesh from the instances that have been added to the combiner so far.
	/// </summary>
	/// <returns>A combined mesh.</returns>
	public Mesh CreateCombinedMesh()
	{
		var mesh = new Mesh
		{
			name = "Combined Mesh",
			vertices = _vertices.ToArray(),
			normals = _normals.ToArray(),
			colors = _colors.ToArray(),
			uv = _uv.ToArray(),
			uv1 = _uv1.ToArray(),
			tangents = _tangents.ToArray(),
			subMeshCount = (_generateStrips) ? _strip.Count : _triangles.Count
		};
		
		if (_generateStrips)
		{
			foreach (var targetSubmesh in _strip)
				mesh.SetTriangleStrip(targetSubmesh.Value.ToArray(), targetSubmesh.Key);
		}
		else
		{
			foreach (var targetSubmesh in _triangles)
				mesh.SetTriangles(targetSubmesh.Value.ToArray(), targetSubmesh.Key);
		}
		
		return mesh;
	}
	
	/// <summary>
	/// Combine the given mesh instances into a single mesh and return it.
	/// </summary>
	/// <param name="instances">The mesh instances to combine.</param>
	/// <param name="generateStrips">true to use triangle strips, false to use triangle lists.</param>
	/// <returns>A combined mesh.</returns>
	public static Mesh Combine(IEnumerable<MeshInstance> instances, bool generateStrips)
	{
		var processor = new MeshCombineUtility(generateStrips);
		processor.AddMeshInstances(instances);
		return processor.CreateCombinedMesh();
	}
	
	#region Nested type: MeshInstance
	
	public class MeshInstance
	{
		/// <summary>
		/// The source mesh.
		/// </summary>
		public Mesh mesh;
		
		/// <summary>
		/// The submesh from the source mesh that you want to add to the combiner.
		/// </summary>
		public int subMeshIndex;
		
		/// <summary>
		/// The submesh that you want this instance to be combined into. Group instances that should
		/// share a material into the same target submesh index.
		/// </summary>
		public int targetSubMeshIndex;
		
		/// <summary>
		/// The instance transform.
		/// </summary>
		public Matrix4x4 transform;
	}
	
	#endregion
}