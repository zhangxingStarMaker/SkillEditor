using System;
using Module.Utility;
using UnityEngine;

namespace Module.ConstDefine
{
    public static class PathDefine
    {
        private const string DataRootFolder = "Data";
        private const string LuaRootFolder = "XLua";
        private const string AbRootFolder = "AssetBundles";
        
#if UNITY_EDITOR
        private const string Platform = "Editor";
#elif UNITY_ANDROID
        private const string Platform = "Android";
#elif UNITY_IPHONE
        private const string Platform = "Ios";
#else
        private const string Platform = "Windows";
#endif
        
        // 首包中assetbundle的加载位置
        private static readonly string StreamAssetAbRootPath = Application.streamingAssetsPath;
        
        // 首包中非assetbundle的加载位置
#if UNITY_EDITOR
        private static readonly string StreamAssetRootPath = Environment.CurrentDirectory + "/StreamingAssets";
#elif UNITY_STANDALONE
        private static readonly string StreamAssetRootPath = Application.streamingAssetsPath;
#elif UNITY_ANDROID
        // android平台非ab资源需copy到persistant路径下在加载
        private static readonly string StreamAssetRootPath = Application.persistentDataPath + "/StreamAssets";
#elif UNITY_IPHONE
        private static readonly string StreamAssetRootPath = Application.streamingAssetsPath;
#endif
        private static readonly string LuaRootPath = $"{StreamAssetRootPath}/{LuaRootFolder}/";
        
#if UNITY_EDITOR       
        private static readonly string AssetBundleRootPath =  $"{StreamAssetRootPath}/{AbRootFolder}/";
#else
        private static readonly string AssetBundleRootPath =  $"{StreamAssetAbRootPath}/{AbRootFolder}/";
#endif
        
#if UNITY_EDITOR
        #if UNITY_ANDROID
            private static readonly string AssetBundleResRootPath = AssetBundleRootPath + "Android";
        #elif UNITY_IPHONE
            private static readonly string AssetBundleResRootPath = AssetBundleRootPath + "Ios";
        #else
            private static readonly string AssetBundleResRootPath = AssetBundleRootPath + "Windows";
        #endif
#else
        private static readonly string AssetBundleResRootPath = AssetBundleRootPath;
#endif

        private static readonly string DataRootPath = $"{StreamAssetRootPath}/{DataRootFolder}/";
        private static readonly string CsExcelDataPathFormat = DataRootPath + "Config/Excel/{0}.bytes";
        private static readonly string LuBanExcelRelativePathFormat = "Config/LuBanDataDir/{0}/{1}.{2}";
        
        public const string NormalAssetRootPath = "Assets/ArtRes/"; //资源在策划工程的根目录
        private static string DialogueAssetPath = NormalAssetRootPath + "art_online/story/story_line/";

#region HotLoad
        private const string HotLoadFolder = "HotLoad";
        private const string HotLoadPatchFileName = "PatchInfoList.bytes";
        private static readonly string CsHotLoadRootPath = "HotUpdateCSharpCode";
        private static readonly string CsHotLoadConfigPath = $"{CsHotLoadRootPath}/CsDllInfo.bytes";
        
#endregion

#region CustomDefine

        public const string UrpRendererDataPath = "art_online/common/urp/g19_renderer/g29_renderer.asset";

#endregion

        public static string GetCSharpExcelPathFormat()
        {
            return CsExcelDataPathFormat;
        }

        public static string GetLuBanRelativeExcelPath(string relativeDir,string fileName,string suffix)
        {
            return string.Format(LuBanExcelRelativePathFormat, relativeDir, fileName, suffix);
        }
        
        public static string GetDialogueExcelPath()
        {
            return DialogueAssetPath;
        }
        public static string GetCsHotLoadRootPath()
        {
            return CsHotLoadRootPath;
        }
        
        public static string GetCsHotLoadConfigPath(bool relativePath = true)
        {
            if (relativePath)
            {
                return CsHotLoadConfigPath;
            }

            // string absolutePath;
            // using (zstring.Block())
            // {
            //     absolutePath = zstring.Concat(DataRootPath, CsHotLoadConfigPath).Intern();
            // }
            return "";
        }
        
        public static string GetLuaRootPath()
        {
            return LuaRootPath;
        }
        
        public static string GetDataRootPath()
        {
            return DataRootPath;
        }

        public static string GetStreamAssetPath()
        {
            return StreamAssetRootPath;
        }
        
        public static string GetStreamAssetAbRootPath()
        {
            return StreamAssetAbRootPath;
        }
        
        public static string GetAssetBundleRootPath()
        {
            return AssetBundleRootPath;
        }

        public static string GetAssetBundleResRootPath()
        {
            return AssetBundleResRootPath;
        }

        public static string GetDataRootFolder()
        {
            return DataRootFolder;
        }
        
        public static string GetLuaRootFolder()
        {
            return LuaRootFolder;
        }

        public static string GetCurrentPlatform()
        {
            return Platform;
        }

        public static string GetAssetBundleRootFolder()
        {
            return AbRootFolder;
        }
        
        
        public static string GetHotLoadRootFolder()
        {
            return HotLoadFolder;
        }
        public static string GetNormalAssetRootPath()
        {
            return NormalAssetRootPath;
        }
        
        public static string GetHotLoadPatchFileName()
        {
            return HotLoadPatchFileName;
        }
        
        
    }
}
