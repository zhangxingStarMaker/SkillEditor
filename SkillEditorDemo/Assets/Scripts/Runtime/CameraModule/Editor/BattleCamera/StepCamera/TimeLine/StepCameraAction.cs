using UnityEditor;
using UnityEditor.Timeline.Actions;

namespace CameraModule.Editor
{
    [MenuEntry("步伐相机/创建新的步伐相机")]
    public class StepCameraAction : TimelineAction
    {
        public override bool Execute(ActionContext context)
        {
            foreach (var trackAsset in context.tracks)
            {
                if (trackAsset is StepCameraTrack stepCameraTrack)
                {
                    double startTime = 0;
                    foreach (var clip in stepCameraTrack.GetClips())
                    {
                        startTime = clip.end;
                    }
                    var timelineClip = stepCameraTrack.CreateClip<StepCameraClip>();
                    StepCameraClip stepCameraClip = timelineClip.asset as StepCameraClip;
                    if (stepCameraClip!=null)
                    {
                        stepCameraClip.cameraAsset = CameraEditorFacade.Instance.CameraEditorEntity.CreateDefaultCameraAsset();
                        timelineClip.start = startTime;
                        timelineClip.duration = 2;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            return true;
        }
        
        public override ActionValidity Validate(ActionContext context)
        {
            return ActionValidity.Valid;
        }
    }
}