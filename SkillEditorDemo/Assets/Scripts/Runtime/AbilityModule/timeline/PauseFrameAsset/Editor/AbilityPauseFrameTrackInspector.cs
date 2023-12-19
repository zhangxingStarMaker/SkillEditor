using UnityEditor;

namespace AbilitySystem
{
    [CustomEditor(typeof(AbilityPauseFrameTrack))]
    public class AbilityPauseFrameTrackInspector : AbilityTrackInspector
    {
        private AbilityPauseFrameTrack _currentTrack;
        protected override void OnSelectTrack()
        {
            base.OnSelectTrack();
            if (SelectTrackAsset!=null)
            {
                _currentTrack = (AbilityPauseFrameTrack) CurrentTrackAsset;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_currentTrack!=null)
            {
                _currentTrack.CurrentTrackRelyOnTrackAsset = SelectTrackAsset;
            }
        }
    }
}