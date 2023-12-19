using System.Collections.Generic;
using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    [CustomEditor(typeof(CameraGmComponent))]
    public class CameraEditorGmComponent : UnityEditor.Editor
    {
        private CameraGmComponent _cameraGmComponent;
        
        private bool _isShowSource = false;
    
        private void OnEnable()
        {
            _cameraGmComponent = target as CameraGmComponent;
        }
    
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShowCameraListContent();
            if (GUILayout.Button("转换一下"))
            {
                
            }
        }

        private void ShowCameraListContent()
        {
            _isShowSource = EditorGUILayout.Foldout(_isShowSource, "展开源数据",true);
            if (_isShowSource)
            {
                List<CameraData> sourceCameraDataList = _cameraGmComponent.SourceCameraDataList;
                if (sourceCameraDataList != null)
                {
                    
                }
            }

            if (GUILayout.Button("保存相机数据"))
            {
                _cameraGmComponent.SaveCameraDataList();
                EditorUtility.SetDirty(_cameraGmComponent.CameraGmAsset);
                AssetDatabase.SaveAssetIfDirty(_cameraGmComponent.CameraGmAsset);
            }
        }
    }
}