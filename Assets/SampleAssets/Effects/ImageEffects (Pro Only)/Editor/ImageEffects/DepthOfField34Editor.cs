using UnityEditor;
using UnityEngine;

namespace UnitySampleAssets.ImageEffects.Inspector
{
    [CustomEditor(typeof(DepthOfField34))]
    public class DepthOfField34Editor : Editor
    {
        private SerializedObject serObj;

        private SerializedProperty simpleTweakMode;

        private SerializedProperty focalPoint;
        private SerializedProperty smoothness;

        private SerializedProperty focalSize;

        private SerializedProperty focalZDistance;
        private SerializedProperty focalStartCurve;
        private SerializedProperty focalEndCurve;

        private SerializedProperty visualizeCoc;

        private SerializedProperty resolution;
        private SerializedProperty quality;

        private SerializedProperty objectFocus;

        private SerializedProperty bokeh;
        private SerializedProperty bokehScale;
        private SerializedProperty bokehIntensity;
        private SerializedProperty bokehThreshholdLuminance;
        private SerializedProperty bokehThreshholdContrast;
        private SerializedProperty bokehDownsample;
        private SerializedProperty bokehTexture;
        private SerializedProperty bokehDestination;

        private SerializedProperty bluriness;
        private SerializedProperty maxBlurSpread;
        private SerializedProperty foregroundBlurExtrude;

        private void OnEnable()
        {
            serObj = new SerializedObject(target);

            simpleTweakMode = serObj.FindProperty("simpleTweakMode");

            // simple tweak mode
            focalPoint = serObj.FindProperty("focalPoint");
            smoothness = serObj.FindProperty("smoothness");

            // complex tweak mode
            focalZDistance = serObj.FindProperty("focalZDistance");
            focalStartCurve = serObj.FindProperty("focalZStartCurve");
            focalEndCurve = serObj.FindProperty("focalZEndCurve");
            focalSize = serObj.FindProperty("focalSize");

            visualizeCoc = serObj.FindProperty("visualize");

            objectFocus = serObj.FindProperty("objectFocus");

            resolution = serObj.FindProperty("resolution");
            quality = serObj.FindProperty("quality");
            bokehThreshholdContrast = serObj.FindProperty("bokehThreshholdContrast");
            bokehThreshholdLuminance = serObj.FindProperty("bokehThreshholdLuminance");

            bokeh = serObj.FindProperty("bokeh");
            bokehScale = serObj.FindProperty("bokehScale");
            bokehIntensity = serObj.FindProperty("bokehIntensity");
            bokehDownsample = serObj.FindProperty("bokehDownsample");
            bokehTexture = serObj.FindProperty("bokehTexture");
            bokehDestination = serObj.FindProperty("bokehDestination");

            bluriness = serObj.FindProperty("bluriness");
            maxBlurSpread = serObj.FindProperty("maxBlurSpread");
            foregroundBlurExtrude = serObj.FindProperty("foregroundBlurExtrude");
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            GameObject go = (target as DepthOfField34).gameObject;

            if (!go)
                return;

            if (!go.camera)
                return;

            if (simpleTweakMode.boolValue)
                GUILayout.Label(
                    "Current: " + go.camera.name + ", near " + go.camera.nearClipPlane + ", far: " +
                    go.camera.farClipPlane + ", focal: " + focalPoint.floatValue, EditorStyles.miniBoldLabel);
            else
                GUILayout.Label(
                    "Current: " + go.camera.name + ", near " + go.camera.nearClipPlane + ", far: " +
                    go.camera.farClipPlane + ", focal: " + focalZDistance.floatValue, EditorStyles.miniBoldLabel);

            EditorGUILayout.PropertyField(resolution, new GUIContent("Resolution"));
            EditorGUILayout.PropertyField(quality, new GUIContent("Quality"));

            EditorGUILayout.PropertyField(simpleTweakMode, new GUIContent("Simple tweak"));
            EditorGUILayout.PropertyField(visualizeCoc, new GUIContent("Visualize focus"));
            EditorGUILayout.PropertyField(bokeh, new GUIContent("Enable bokeh"));


            EditorGUILayout.Separator();

            GUILayout.Label("Focal Settings", EditorStyles.boldLabel);

            if (simpleTweakMode.boolValue)
            {
                focalPoint.floatValue = EditorGUILayout.Slider("Focal distance", focalPoint.floatValue,
                                                               go.camera.nearClipPlane, go.camera.farClipPlane);
                EditorGUILayout.PropertyField(objectFocus, new GUIContent("Transform"));
                EditorGUILayout.PropertyField(smoothness, new GUIContent("Smoothness"));
                focalSize.floatValue = EditorGUILayout.Slider("Focal size", focalSize.floatValue, 0.0f,
                                                              (go.camera.farClipPlane - go.camera.nearClipPlane));
            }
            else
            {
                focalZDistance.floatValue = EditorGUILayout.Slider("Distance", focalZDistance.floatValue,
                                                                   go.camera.nearClipPlane, go.camera.farClipPlane);
                EditorGUILayout.PropertyField(objectFocus, new GUIContent("Transform"));
                focalSize.floatValue = EditorGUILayout.Slider("Size", focalSize.floatValue, 0.0f,
                                                              (go.camera.farClipPlane - go.camera.nearClipPlane));
                focalStartCurve.floatValue = EditorGUILayout.Slider("Start curve", focalStartCurve.floatValue, 0.05f,
                                                                    20.0f);
                focalEndCurve.floatValue = EditorGUILayout.Slider("End curve", focalEndCurve.floatValue, 0.05f, 20.0f);
            }

            EditorGUILayout.Separator();

            GUILayout.Label("Blur (Fore- and Background)", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(bluriness, new GUIContent("Blurriness"));
            EditorGUILayout.PropertyField(maxBlurSpread, new GUIContent("Blur spread"));

            if (quality.enumValueIndex > 0)
            {
                EditorGUILayout.PropertyField(foregroundBlurExtrude, new GUIContent("Foreground size"));
            }

            EditorGUILayout.Separator();

            if (bokeh.boolValue)
            {
                EditorGUILayout.Separator();
                GUILayout.Label("Bokeh Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(bokehDestination, new GUIContent("Destination"));
                bokehIntensity.floatValue = EditorGUILayout.Slider("Intensity", bokehIntensity.floatValue, 0.0f, 1.0f);
                bokehThreshholdLuminance.floatValue = EditorGUILayout.Slider("Min luminance",
                                                                             bokehThreshholdLuminance.floatValue, 0.0f,
                                                                             0.99f);
                bokehThreshholdContrast.floatValue = EditorGUILayout.Slider("Min contrast",
                                                                            bokehThreshholdContrast.floatValue, 0.0f,
                                                                            0.25f);
                bokehDownsample.intValue = EditorGUILayout.IntSlider("Downsample", bokehDownsample.intValue, 1, 3);
                bokehScale.floatValue = EditorGUILayout.Slider("Size scale", bokehScale.floatValue, 0.0f, 20.0f);
                EditorGUILayout.PropertyField(bokehTexture, new GUIContent("Texture mask"));
            }

            serObj.ApplyModifiedProperties();
        }
    }
}