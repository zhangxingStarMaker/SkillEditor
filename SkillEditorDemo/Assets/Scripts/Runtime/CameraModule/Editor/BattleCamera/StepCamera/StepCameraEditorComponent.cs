using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CameraModule.Runtime;
using CameraModule.Runtime;
using Module.ConstDefine;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraModule.Editor
{
    public class StepCameraEditorComponent : EditorComponent
    {
        public override void OnInit(CameraEditorEntity cameraEditorEntity)
        {
            base.OnInit(cameraEditorEntity);
        }

        private bool IsEqualFloat(float value1,float value2)
        {
            return MathF.Abs(value1 - value2) < 0.01f;
        }

        /// <summary>
        /// 检测当前轨道是否合规
        /// </summary>
        /// <returns></returns>
        private bool CheckCompliance(StepCameraTrack stepCameraTrack)
        {
            CameraEditorDebug.LogError(Time.deltaTime);
            List<Vector2> stepCameraTimeList = new List<Vector2>();
            List<Vector2> stepActionTimeList = GetStepActionTime();
            foreach (var clip in stepCameraTrack.GetClips())
            {
                if (clip.asset is StepCameraClip stepCameraClip)
                {
                    stepCameraTimeList.Add(new Vector2((float)clip.start,(float)(clip.end)));
                }
            }

            if (stepCameraTimeList.Count == 0 || stepActionTimeList.Count == 0)
            {
                return false;
            }
            else
            {
                //相机轨道和步伐轨道的起始位置需要相同
                if (!IsEqualFloat(stepCameraTimeList[0].x,stepActionTimeList[0].x))
                {
                    CameraEditorDebug.LogError("步伐相机初始位置不对,相机资源保存失败");
                    return false;
                }
                
                //相机轨道的长度要大于或者等于步伐轨道
                if (!IsEqualFloat(stepCameraTimeList[^1].y,stepActionTimeList[^1].y))
                {
                    if (stepCameraTimeList[^1].y<stepActionTimeList[^1].y)
                    {
                        CameraEditorDebug.LogError("步伐相机长度不够,相机资源保存失败");
                        return false;
                    }
                }

                for (int i = 0; i < stepCameraTimeList.Count; i++)
                {
                    if (i+1 < stepCameraTimeList.Count)
                    {
                        if (!IsEqualFloat(stepCameraTimeList[i].y,stepCameraTimeList[i+1].x))
                        {
                            if (stepCameraTimeList[i].y < stepCameraTimeList[i+1].x)
                            {
                                CameraEditorDebug.LogError("步伐相机Clip之间有空余,相机资源保存失败");
                                return false;
                            }
                        }
                    }
                }
                
            }
            
            return true;
        }

        public string SaveTrackAssetByDefaultName(StepCameraTrack cameraTrack,string assetName)
        {
            return SaveTrackAssetAndRename(cameraTrack,assetName);
        }
        
        private string SaveTrackAssetAndRename(StepCameraTrack stepCameraTrack,string newName)
        {
            string path = stepCameraTrack.CameraGroupPath;
            var ca = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(path);
            if (ca == null)
            {
                string saveNewPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraGroupPath + newName + ".asset";
                ca = ScriptableObject.CreateInstance<CameraGroupAsset>();
                AssetDatabase.CreateAsset(ca, saveNewPath);
            }
            else
            {
                RemoveAssetByName(path);
                ca = ScriptableObject.CreateInstance<CameraGroupAsset>();
                string saveNewPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraGroupPath + newName + ".asset";
                AssetDatabase.CreateAsset(ca, saveNewPath);
            }

            List<Vector2> stepActionTime = GetStepActionTime();
            ca.StartTime = stepActionTime[0].x;
            ca.EndTime = stepActionTime[^1].y;

            List<CameraAsset> cameraAssetList = new List<CameraAsset>();
            
            foreach (var clip in stepCameraTrack.GetClips())
            {
                if (clip.asset is StepCameraClip stepCameraClip)
                {
                    if (stepCameraClip.cameraAsset!=null)
                    {
                        cameraAssetList.Add(stepCameraClip.cameraAsset);
                    }
                }
            }

            List<CameraAsset> newCameraAssetList = AgainCreateClip(cameraAssetList,newName);
            
            if (stepCameraTrack != null)
            {
                int count = 0;
                foreach (var clip in stepCameraTrack.GetClips())
                {
                    if (clip.asset is StepCameraClip stepCameraClip)
                    {
                        
                        if (count < newCameraAssetList.Count)
                        {
                            CameraGroupItemAsset cameraGroupItemAsset = new CameraGroupItemAsset();
                            cameraGroupItemAsset.StartTime = clip.start;
                            cameraGroupItemAsset.EndTime = clip.start + clip.duration;
                            cameraGroupItemAsset.CameraAsset = newCameraAssetList[count];
                            stepCameraClip.cameraAsset = newCameraAssetList[count];
                            count++;
                            if (stepCameraClip.cameraAsset!=null)
                            {
                                RenameAssetName(AssetDatabase.GetAssetPath(stepCameraClip.cameraAsset), newName+"_0"+count);
                                if (stepCameraClip.cameraAsset.VirtualCamera!=null)
                                {
                                    CameraEditorDebug.LogInfo("gamePath:"+AssetDatabase.GetAssetPath(stepCameraClip.cameraAsset.VirtualCamera));
                                    RenameAssetName(AssetDatabase.GetAssetPath(stepCameraClip.cameraAsset.VirtualCamera), newName+"_0"+count);
                                }

                                cameraAssetList.Add(stepCameraClip.cameraAsset);
                            }
                            ca.CameraGroupItemAssetList.Add(cameraGroupItemAsset);
                        }
                    }
                }
            }

            if (ca!=null)
            {
                CameraGroupAsset cameraGroupAsset = ca;
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
                CameraConfigureAsset cameraConfigureAsset = AssetDatabase.LoadAssetAtPath<CameraConfigureAsset>(CameraConstDefines.CameraConfigureEditorPath);
                if (cameraConfigureAsset!=null)
                {
                    cameraConfigureAsset.AddCameraGroupInfo(cameraGroupInfo);
                    EditorUtility.SetDirty(cameraConfigureAsset);
                    AssetDatabase.SaveAssetIfDirty(cameraConfigureAsset);
                }
            }
            
            stepCameraTrack.CameraGroupPath = AssetDatabase.GetAssetPath(ca);
            EditorUtility.SetDirty(ca);
            AssetDatabase.SaveAssets();
            return AssetDatabase.GetAssetPath(ca);
        }

        private List<CameraAsset> AgainCreateClip(List<CameraAsset> cameraAssetList,string newName)
        {
            List<CameraAsset> newCameraAssetList = new List<CameraAsset>();
            int count = 0;
            foreach (var cameraAsset in cameraAssetList)
            {
                CameraAsset newCameraAsset = CopyCameraAsset(cameraAsset,"copy"+count);
                newCameraAssetList.Add(newCameraAsset);
                count++;
            }
            LoadAndDeleteAssetsInFolder(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraAssetPath,newName);
            LoadAndDeleteAssetsInFolder(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraPrefabPath,newName);
            return newCameraAssetList;
        }
        
        


        public void LoadAndDeleteAssetsInFolder(string folderPath,string containName)
        {
            string[] files = Directory.GetFiles(folderPath);

            foreach (string file in files)
            {
                if (file.EndsWith(".meta")) continue; // 忽略.meta文件

                string assetPath = file.Substring(file.IndexOf("Assets"));
                assetPath = assetPath.Replace('\\', '/');

                CameraEditorDebug.LogError(assetPath);
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                CameraEditorDebug.LogError(asset.name);

                if (asset != null)
                {
                    // 这里可以根据资源的名字来判断是否删除
                    if (asset.name.Contains(containName))
                    {
                        if (!asset.name.Contains("copy"))
                        {
                            // 删除资源
                            AssetDatabase.DeleteAsset(assetPath);
                        }
                    }
                }
            }

            // 刷新AssetDatabase以便更新资源数据库
            AssetDatabase.Refresh();
        }

        private void RemoveAssetByName(string path)
        {
            var ca = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(path);
            if (ca!=null)
            {
                bool isDelect = AssetDatabase.DeleteAsset(path);
                if (isDelect)
                {
                    CameraEditorDebug.LogInfo("删除文件"+path);
                }
                
            }
        }
        
        private void RenameAssetName(string path,string name)
        {
            AssetDatabase.RenameAsset(path,name);
        }

        public void SaveTrackAsset(StepCameraTrack cameraTrack)
        {
            if (!CheckCompliance(cameraTrack))
            {
                return;
            }
            if (CameraEditorDefine.CameraDefaultGroupPath.Contains(cameraTrack.CameraGroupPath))
            {
                var path = EditorUtility.SaveFilePanelInProject("请选择本地文件夹保存", "camera_group1", "asset", "",PathDefine.NormalAssetRootPath+CameraEditorDefine.CameraGroupPath);
                if (path.Length != 0)
                {
                    string newPath = path.Substring(path.IndexOf("Assets"));
                    cameraTrack.CameraGroupPath = newPath.Replace('\\', '/');
                    CreateCameraGroup(cameraTrack,path);
                }
            }
            else
            {
                CreateCameraGroup(cameraTrack,cameraTrack.CameraGroupPath);
            }
        }

        public void SaveAs(StepCameraTrack cameraTrack)
        {
            var path = EditorUtility.SaveFilePanelInProject("请选择本地文件夹保存", "camera_group1", "asset", "",PathDefine.NormalAssetRootPath+CameraEditorDefine.CameraGroupPath);
            if (path.Length != 0)
            {
                string newPath = path.Substring(path.IndexOf("Assets"));
                cameraTrack.CameraGroupPath = newPath.Replace('\\', '/');
                CreateCameraGroup(cameraTrack,path);
            }
        }
        
        private List<Vector2> GetStepActionTime()
        {
            List<Vector2> clipTimeList = new List<Vector2>();
            double startTime = 0;
            double durTime = 15;
            // var allChoreographyFragment = ChoreographyPreviewEditorWindow.GetPreviewEditorWindow()?.GetFragments();
            // // List<EditorFragment> allChoreographyFragment = null;
            // if (allChoreographyFragment == null || allChoreographyFragment.Count == 0)
            // {
            //     CameraEditorDebug.LogError("拿到的动作步伐数据为空");
            // }
            // else
            // {
            //     for (int i = 0; i < allChoreographyFragment.Count; i++)
            //     {
            //         Vector2 time = new Vector2(allChoreographyFragment[i].StartTime,
            //             allChoreographyFragment[i].StartTime+allChoreographyFragment[i].Duration);
            //         clipTimeList.Add(time);
            //     }
            // }
            clipTimeList.Add(new Vector2(0,1.27f));
            clipTimeList.Add(new Vector2(1.27f,2.27f));
            clipTimeList.Add(new Vector2(2.27f,4.1f));
            clipTimeList.Add(new Vector2(4.1f,5.533f));
            return clipTimeList;
        }
        
        public void CreateCameraGroup(StepCameraTrack stepCameraTrack,string path)
        {
            var ca = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(path);
            if (ca == null)
            {
                ca = ScriptableObject.CreateInstance<CameraGroupAsset>();
                AssetDatabase.CreateAsset(ca, path);
            }
            else
            {
                ca.CameraGroupItemAssetList.Clear();
                ca.StartTime = 0;
                ca.EndTime = 0;
            }

            List<Vector2> stepActionTime = GetStepActionTime();
            ca.StartTime = stepActionTime[0].x;
            ca.EndTime = stepActionTime[^1].y;

            if (stepCameraTrack != null)
            {
                foreach (var clip in stepCameraTrack.GetClips())
                {
                    if (clip.asset is StepCameraClip stepCameraClip)
                    {
                        CameraGroupItemAsset cameraGroupItemAsset = new CameraGroupItemAsset();
                        cameraGroupItemAsset.StartTime = clip.start;
                        cameraGroupItemAsset.EndTime = clip.start + clip.duration;
                        cameraGroupItemAsset.CameraAsset = stepCameraClip.cameraAsset;
                        ca.CameraGroupItemAssetList.Add(cameraGroupItemAsset);
                    }
                }
            }

            EditorUtility.SetDirty(ca);
            AssetDatabase.SaveAssets();
        }
        

        public void LoadCameraGroup(StepCameraTrack stepCameraTrack,string path)
        {
            if (path.Length != 0)
            {
                stepCameraTrack.CameraGroupPath = path;
                var ca = AssetDatabase.LoadAssetAtPath<CameraGroupAsset>(path);
                if (ca == null || ca.CameraGroupItemAssetList == null || ca.CameraGroupItemAssetList.Count == 0)
                {
                    return;
                }

                if (stepCameraTrack == null)
                {
                    return;
                }
                
                var clipList = stepCameraTrack.GetClips().ToList();
                for (int i = 0; i < clipList.Count; i++)
                {
                    stepCameraTrack.DeleteClip(clipList[i]);
                }

                foreach (var cameraGroupItemAsset in ca.CameraGroupItemAssetList)
                {
                    var timelineClip = stepCameraTrack.CreateClip<StepCameraClip>();
                    var cameraPreviewClip = timelineClip.asset as StepCameraClip;
                    if (cameraPreviewClip == null)
                    {
                        continue;
                    }

                    cameraPreviewClip.cameraAsset = cameraGroupItemAsset.CameraAsset;
                    timelineClip.start = cameraGroupItemAsset.StartTime;
                    timelineClip.duration = cameraGroupItemAsset.EndTime - cameraGroupItemAsset.StartTime;
                }

                AssetDatabase.SaveAssets();
            }
        }
        
        public CameraAsset CopyCameraAsset(CameraAsset cameraAsset,string newName)
        {
            CameraAsset newAsset = ScriptableObject.CreateInstance<CameraAsset>();
            EditorUtility.CopySerialized(cameraAsset,newAsset);
            string newPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraAssetPath + "camera_step_" + newName + ".asset";
            AssetDatabase.CreateAsset(newAsset, newPath);
            string newGameObjectPath = CopyCameraPrefab(newAsset.VirtualCamera,newName);
            CameraEditorDebug.LogInfo("path:"+newGameObjectPath);
            newAsset.VirtualCamera = AssetDatabase.LoadAssetAtPath<GameObject>(newGameObjectPath);
            EditorUtility.SetDirty(newAsset);
            AssetDatabase.SaveAssets();
            return newAsset;
        }
        
        public string CopyCameraPrefab(GameObject prefab,string newName)
        {
            GameObject newAsset = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            string newPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraPrefabPath + "camera_step_" + newName + ".prefab";
            PrefabUtility.UnpackPrefabInstance(newAsset, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            PrefabUtility.SaveAsPrefabAsset(newAsset,newPath);
            Object.DestroyImmediate(newAsset);
            return newPath;
        }
    }
}