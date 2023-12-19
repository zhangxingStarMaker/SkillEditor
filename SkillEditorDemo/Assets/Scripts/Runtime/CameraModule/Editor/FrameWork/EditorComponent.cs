namespace CameraModule.Editor
{
    public class EditorComponent
    {
        public CameraEditorEntity CameraEditorEntity;

        public virtual void OnInit(CameraEditorEntity cameraEditorEntity)
        {
            CameraEditorEntity = cameraEditorEntity;
        }
    }
}