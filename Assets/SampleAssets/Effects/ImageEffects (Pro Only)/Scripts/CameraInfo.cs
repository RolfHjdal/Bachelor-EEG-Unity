using UnityEngine;

namespace UnitySampleAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof (Camera))]
    [AddComponentMenu("Image Effects/Camera Info")]
    public class CameraInfo : MonoBehaviour
    {

        // display current depth texture mode
        public DepthTextureMode currentDepthMode;
        // render path
        public RenderingPath currentRenderPath;
        // number of official image fx used
        public int recognizedPostFxCount = 0;

#if UNITY_EDITOR
        private void Start()
        {
            UpdateInfo();
        }

        private void Update()
        {
            if (currentDepthMode != camera.depthTextureMode)
                camera.depthTextureMode = currentDepthMode;
            if (currentRenderPath != camera.actualRenderingPath)
                camera.renderingPath = currentRenderPath;

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            currentDepthMode = camera.depthTextureMode;
            currentRenderPath = camera.actualRenderingPath;
            PostEffectsBase[] fx = gameObject.GetComponents<PostEffectsBase>();
            int fxCount = 0;
            foreach (var post in fx)
                if (post.enabled)
                    fxCount++;
            recognizedPostFxCount = fxCount;
        }
#endif
    }
}