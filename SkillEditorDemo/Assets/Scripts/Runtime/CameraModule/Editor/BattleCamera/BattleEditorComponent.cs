
namespace CameraModule.Editor.BattleCamera
{
    public class BattleEditorComponent : EditorComponent
    {
        // public BattleViewContext BattleViewContext;

        public override void OnInit(CameraEditorEntity cameraEditorEntity)
        {
            base.OnInit(cameraEditorEntity);
            // BattleViewContext = new BattleViewContext();
            // BattleViewContext.EditorManualInit();
            // BattleViewService.RegisterService(cameraEditorEntity.CameraEditorManager);
        }
    }
}