using System.Collections.Generic;

namespace CameraModule.Runtime
{
    [System.Serializable]
    public class CameraGroupInfo
    {
        public string AssetName;
        public List<CameraGroupItemInfo> CameraGroupItemList = new List<CameraGroupItemInfo>();
    }
}