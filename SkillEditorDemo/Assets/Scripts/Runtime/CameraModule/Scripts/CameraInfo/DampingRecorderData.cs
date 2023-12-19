namespace CameraModule.Runtime
{
    public class DampingRecorderData
    {
        public float XDamping;
        public float YDamping;
        public float ZDamping;
        public float YawDamping;
        public float HorizontalDamping;
        public float VerticalDamping;
        public SingleCameraComponent SingleCameraComponent;

        
        public void ResetData()
        {
            XDamping = 0;
            YDamping = 0;
            ZDamping = 0;
            YawDamping = 0;
            HorizontalDamping = 0;
            VerticalDamping = 0;
        }
    }
}