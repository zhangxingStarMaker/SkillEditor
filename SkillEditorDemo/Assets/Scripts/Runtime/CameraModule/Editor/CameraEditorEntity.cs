using CameraModule.Editor.BattleCamera;
using CameraModule.Runtime;
using Module.Battle.Camera;
using Module.ConstDefine;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    public class CameraEditorEntity 
    {
        public StepCameraEditorComponent StepCameraEditorComponent;
        public PreviewCameraEditorComponent PreviewCameraEditorComponent;
        public BattleEditorComponent BattleEditorComponent;
        public CameraEditorManager CameraEditorManager;

        /// <summary>
        /// 初始化Component
        /// </summary>
        public void InitComponent()
        {
            CameraEditorManager = new CameraEditorManager();
            // CameraEditorManager.Init();
            StepCameraEditorComponent =  AddComponent<StepCameraEditorComponent>();
            PreviewCameraEditorComponent = AddComponent<PreviewCameraEditorComponent>();
            BattleEditorComponent = AddComponent<BattleEditorComponent>();
        }

        public void OnClear()
        {
            // CameraEditorManager?.EditorClear();
            CameraEditorManager = null;
            BattleEditorComponent = null;
        }

        private T AddComponent<T>() where T : EditorComponent, new()
        {
            T component = new T();
            component.OnInit(this);
            return component;
        }

        public string CopyCameraGroup(CameraGroupAsset cameraGroupAsset)
        {
            // string animName = ChoreographyPreviewEditorWindow.GetWindow<ChoreographyPreviewEditorWindow>().CurrentAnimName();
            // string name = GetNewName(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraGroupPath,animName);
            // CameraEditorDebug.LogInfo(name);
            // CameraGroupAsset newAsset = ScriptableObject.CreateInstance<CameraGroupAsset>();
            // EditorUtility.CopySerialized(cameraGroupAsset,newAsset);
            // int count = 0;
            // if (newAsset!=null)
            // {
            //     foreach (var cameraGroupItemAsset in newAsset.CameraGroupItemAssetList)
            //     {
            //         CameraAsset newCameraAssetAsset = CopyCameraAsset(cameraGroupItemAsset.CameraAsset,animName);
            //         cameraGroupItemAsset.CameraAsset = newCameraAssetAsset;
            //         count++;
            //     }
            // }
            // string newPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraGroupPath + animName + name + ".asset";
            // AssetDatabase.CreateAsset(newAsset, newPath);
            // EditorUtility.SetDirty(newAsset);
            // AssetDatabase.SaveAssets();
            string newPath = "";
            return newPath;
        }

        public CameraAsset CreateDefaultCameraAsset()
        {
            // var previewWindow = GetWindow<ChoreographyPreviewEditorWindow>();
            // string animName = ChoreographyPreviewEditorWindow.GetWindow<ChoreographyPreviewEditorWindow>().CurrentAnimName();
            // CameraAsset cameraAsset = AssetDatabase.LoadAssetAtPath<CameraAsset>(PathDefine.NormalAssetRootPath+CameraEditorDefine.CameraAssetDefaultPath);
            // if (cameraAsset!=null)
            // {
            //     CameraAsset newCameraAsset = CopyCameraAsset(cameraAsset,animName);
            //     return newCameraAsset;
            // }
            // else
            // {
            //     CameraEditorDebug.LogError("未找到默认步伐相机Asset");
            // }

            return null;
        }

        public CameraAsset CopyCameraAsset(CameraAsset cameraAsset,string animName)
        {
            string name = GetNewName(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraAssetPath,animName);
            CameraAsset newAsset = ScriptableObject.CreateInstance<CameraAsset>();
            EditorUtility.CopySerialized(cameraAsset,newAsset);
            string newPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraAssetPath + animName + name + ".asset";
            AssetDatabase.CreateAsset(newAsset, newPath);
            string newGameObjectPath = CopyCameraPrefab(newAsset.VirtualCamera,animName);
            CameraEditorDebug.LogInfo("path:"+newGameObjectPath);
            newAsset.VirtualCamera = AssetDatabase.LoadAssetAtPath<GameObject>(newGameObjectPath);
            EditorUtility.SetDirty(newAsset);
            AssetDatabase.SaveAssets();
            return newAsset;
        }
        
        public string CopyCameraPrefab(GameObject prefab,string animName)
        {
            string name = GetNewName(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraPrefabPath,animName);
            GameObject newAsset = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            string newPath = PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraPrefabPath + animName + name + ".prefab";
            PrefabUtility.UnpackPrefabInstance(newAsset, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            PrefabUtility.SaveAsPrefabAsset(newAsset,newPath);
            Object.DestroyImmediate(newAsset);
            return newPath;
        }

        public void TestCoed()
        {
            StepCameraEditorComponent.LoadAndDeleteAssetsInFolder(PathDefine.NormalAssetRootPath + CameraEditorDefine.CameraAssetPath,"llll");
        }

        public string GetNewName(string path,string containContent = "")
        {
            string[] assetPaths = AssetDatabase.FindAssets("", new string[] { path });
            int maxNum = 001;

            if (assetPaths!=null && assetPaths.Length > 0)
            {
                foreach (string assetPath in assetPaths)
                {
                    // 使用AssetDatabase加载资源
                    string loadPath = AssetDatabase.GUIDToAssetPath(assetPath);
                    Object asset = AssetDatabase.LoadAssetAtPath(loadPath, typeof(Object));

                    if (asset != null)
                    {
                        if (asset.name.Contains(containContent))
                        {
                            string[] names = asset.name.Split("_");
                            string numName = names[^1];
                            int currentNum = maxNum;
                            bool isNum = int.TryParse(numName, out currentNum);
                            if (isNum)
                            {
                                if (currentNum >= maxNum)
                                {
                                    maxNum = currentNum + 1;
                                }
                            }
                        }
                    }
                }
            }
            

            if (maxNum < 10)
            {
                return "00" + maxNum;
            }
            if (maxNum < 100)
            {
                return "0" + maxNum;
            }

            if (maxNum > 100)
            {
                return maxNum.ToString();
            }
            
            return "001";
        }
    }
}