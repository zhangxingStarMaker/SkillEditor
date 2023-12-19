
namespace CameraModule.Editor
{
    public class CameraEditorFacade
    {
        
        private static CameraEditorFacade _cameraEditorFacade;
        
        public static CameraEditorFacade Instance
        {
            get
            {
                if (_cameraEditorFacade == null)
                {
                    _cameraEditorFacade = new CameraEditorFacade();
                }

                return _cameraEditorFacade;
            }
        }

        public CameraEditorEntity CameraEditorEntity
        {
            get
            {
                if (_cameraEditorEntity == null)
                {
                    CreateCameraEditorEntity();
                }

                return _cameraEditorEntity;
            }
        }
        private CameraEditorEntity _cameraEditorEntity;
        
        

        /// <summary>
        /// 自己初始化
        /// </summary>
        public CameraEditorFacade()
        {
            CreateCameraEditorEntity();
        }

        private void CreateCameraEditorEntity()
        {
            _cameraEditorEntity = new CameraEditorEntity();
            _cameraEditorEntity.InitComponent();
        }

        public void OnClear()
        {
            _cameraEditorEntity.OnClear();
            _cameraEditorEntity = null;
            CreateCameraEditorEntity();
        }
    }
}