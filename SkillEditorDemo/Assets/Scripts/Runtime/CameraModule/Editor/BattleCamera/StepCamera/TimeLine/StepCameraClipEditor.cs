using System.Reflection;
using CameraModule.Runtime;
using Cinemachine;
using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    [CustomEditor(typeof(StepCameraClip))]
    public class StepCameraClipEditor : UnityEditor.Editor
    {
        private StepCameraClip _clip;
        private SerializedProperty _cameraAsset;
        private UnityEditor.Editor _cameraAssetEditor;
        private UnityEditor.Editor _virtualCameraEditor;
        private bool _hasChange = false;
        
        private void OnEnable()
        {
            _clip = target as StepCameraClip;
            _cameraAsset = serializedObject.FindProperty("cameraAsset");
            if (_cameraAsset.objectReferenceValue != null)
            {
                _cameraAssetEditor= CreateEditor(_cameraAsset.objectReferenceValue);
                var cameraAsset = _cameraAsset.objectReferenceValue as CameraAsset;
                var virtualCameraComponet = cameraAsset?.VirtualCamera?.GetComponent<CinemachineVirtualCameraBase>();
                if (virtualCameraComponet != null)
                {
                    _virtualCameraEditor = CreateEditor(virtualCameraComponet);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (_cameraAsset == null) return;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_cameraAsset, new GUIContent("CameraAsset"));
            if (_cameraAsset.objectReferenceValue != null)
            {
                if (_cameraAssetEditor == null||_hasChange)
                {
                    _cameraAssetEditor= CreateEditor(_cameraAsset.objectReferenceValue);
                    _hasChange = false;
                }
                var cameraAsset = _cameraAsset.objectReferenceValue as CameraAsset;
                var virtualCameraComponet = cameraAsset?.VirtualCamera?.GetComponent<CinemachineVirtualCameraBase>();
                if (virtualCameraComponet != null&&(_virtualCameraEditor==null||_virtualCameraEditor.target!=virtualCameraComponet))
                {
                    _virtualCameraEditor = CreateEditor(virtualCameraComponet);
                }
                // 调用CB的OnInspectorGUI方法
                _cameraAssetEditor.OnInspectorGUI();
                if (_virtualCameraEditor != null)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    _virtualCameraEditor.OnInspectorGUI();
                }
            }
            if (serializedObject.hasModifiedProperties)
            {
                _hasChange = true;
            }
            // 应用修改
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                // var cameraRoot = GameObject.Find("PreviewCameraEditorManager");
                // PreviewCameraEditorManager manager;
                // if (cameraRoot == null)
                // {
                //     cameraRoot = new GameObject("PreviewCameraEditorManager");
                //     manager = cameraRoot.AddComponent<PreviewCameraEditorManager>();
                // }
                // else
                // {
                //     manager = cameraRoot.GetComponent<PreviewCameraEditorManager>();
                //     if (manager == null)
                //     {
                //         manager = cameraRoot.AddComponent<PreviewCameraEditorManager>();
                //     }
                // }
                // manager.RefreshCameraPreview();
            }
        }

        public void OnDisable()
        {
            var t = _virtualCameraEditor.GetType();
            var methodInfo =t.GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo?.Invoke(_virtualCameraEditor,new object[]{});
            _cameraAssetEditor = null;
            _virtualCameraEditor = null;
        }
    }
}