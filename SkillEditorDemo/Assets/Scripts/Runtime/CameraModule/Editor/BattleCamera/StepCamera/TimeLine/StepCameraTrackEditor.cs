using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    [CustomEditor(typeof(StepCameraTrack))]
    public class StepCameraTrackEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            if (GUILayout.Button("转换"))
            {
                StepCameraTrack stepCameraTrack = target as  StepCameraTrack;
                if (stepCameraTrack?.CameraGroupAssetList!=null&&stepCameraTrack.CameraConfigureAsset!=null)
                {
                    foreach (var groupAsset in stepCameraTrack.CameraGroupAssetList)
                    {
                        CameraGroupAsset cameraGroupAsset = groupAsset;
                        CameraGroupInfo cameraGroupInfo = new CameraGroupInfo();
                        string assetName = AssetDatabase.GetAssetPath(cameraGroupAsset);
                        cameraGroupInfo.AssetName = assetName.Replace("Assets/ArtRes/","");
                        foreach (var cameraGroupItemAsset in cameraGroupAsset.CameraGroupItemAssetList)
                        {
                            CameraGroupItemInfo cameraGroupItemInfo = new CameraGroupItemInfo();
                            cameraGroupItemInfo.AssetName = AssetDatabase.GetAssetPath(cameraGroupItemAsset.CameraAsset).Replace("Assets/ArtRes/","");
                            cameraGroupItemInfo.StartTime = (float)cameraGroupItemAsset.StartTime;
                            cameraGroupItemInfo.EndTime = (float) cameraGroupItemAsset.EndTime;
                            cameraGroupInfo.CameraGroupItemList.Add(cameraGroupItemInfo);
                        }
                        CameraConfigureAsset cameraConfigureAsset = stepCameraTrack.CameraConfigureAsset;
                        cameraConfigureAsset.AddCameraGroupInfo(cameraGroupInfo);
                        EditorUtility.SetDirty(cameraConfigureAsset);
                        AssetDatabase.SaveAssetIfDirty(cameraConfigureAsset);
                    }
                }
            }
            
            if (GUILayout.Button("另存为"))
            {
                // string name = ChoreographyPreviewEditorWindow.GetWindow<ChoreographyPreviewEditorWindow>().CurrentAnimName();
                // CameraEditorDebug.LogError(name);
            }
            
            if (GUILayout.Button("刷新预览"))
            {
                CameraEditorFacade.Instance.CameraEditorEntity.PreviewCameraEditorComponent?.InitPreviewCameraData();
            }
            
            if (GUILayout.Button("从文件夹加载步伐相机组"))
            {
                // var path = EditorUtility.OpenFilePanel("请选择本地文件", PathDefine.NormalAssetRootPath+CameraEditorDefine.CameraGroupPath, "asset");
                string path = "";
                path = path.Substring(path.IndexOf("Assets"));
                path = path.Replace('\\', '/');
                
                var cameraTrack = target as StepCameraTrack;
                CameraGroupAsset cameraGroupAsset = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(path);
                if (cameraGroupAsset!=null)
                {
                    string newPath = CameraEditorFacade.Instance.CameraEditorEntity.CopyCameraGroup(cameraGroupAsset);
                    
                    CameraEditorFacade.Instance.CameraEditorEntity.StepCameraEditorComponent?.LoadCameraGroup(cameraTrack, newPath);
                }
                else
                {
                    CameraEditorDebug.LogError("CameraGroup Is Null");
                }
            }

            if (GUILayout.Button("保存步伐相机"))
            {
                SaveAsset();
            }
            
            if (GUILayout.Button("另存为"))
            {
                var cameraTrack = target as StepCameraTrack;
                CameraEditorFacade.Instance.CameraEditorEntity.StepCameraEditorComponent?.SaveAs(cameraTrack);
            }
            
            if (GUILayout.Button("加载默认并生成新的相机组配置"))
            {
                var cameraTrack = target as StepCameraTrack;
                // CameraGroupAsset cameraGroupAsset = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(PathDefine.NormalAssetRootPath+CameraEditorDefine.CameraGroupPath +
                //     CameraEditorDefine.CameraDefaultGroupName);
                // if (cameraGroupAsset!=null)
                // {
                //     string newPath = CameraEditorFacade.Instance.CameraEditorEntity.CopyCameraGroup(cameraGroupAsset);
                //     
                //     CameraEditorFacade.Instance.CameraEditorEntity.StepCameraEditorComponent?.LoadCameraGroup(cameraTrack, newPath);
                // }
                // else
                // {
                //     CameraEditorDebug.LogError("CameraGroup Is Null");
                // }
            }
        }

        /// <summary>
        /// 自动生成对应的步伐相机
        /// </summary>
        private void AutoGenStepCamera()
        {
            //todo 根据某种规则生成
        }
        

        private void SaveAsset()
        {
            var cameraTrack = target as StepCameraTrack;
            CameraEditorFacade.Instance.CameraEditorEntity.StepCameraEditorComponent.SaveTrackAsset(cameraTrack);
        }
    }
}