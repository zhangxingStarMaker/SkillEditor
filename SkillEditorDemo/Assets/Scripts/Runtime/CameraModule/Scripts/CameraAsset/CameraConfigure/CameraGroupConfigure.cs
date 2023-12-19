using System.Collections.Generic;

namespace CameraModule.Runtime
{
    [System.Serializable]
    public class CameraGroupConfigure : CameraConfigure
    {
        public int CameraGroupId;
        public int TagId;
        public int RuleId;
        public List<string> CameraAssetPathItemList = new List<string>();
        public List<float> CameraFixTimeList = new List<float>();
    }
}