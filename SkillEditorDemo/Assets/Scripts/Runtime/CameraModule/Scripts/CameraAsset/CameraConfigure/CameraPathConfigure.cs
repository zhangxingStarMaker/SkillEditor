using CameraModule.Runtime;
using UnityEngine;

namespace CameraModule.Runtime
{
    [System.Serializable]
    public class CameraPathConfigure : CameraConfigure
    {
        public int ID;
        public string CameraAssetPath;
    }
}