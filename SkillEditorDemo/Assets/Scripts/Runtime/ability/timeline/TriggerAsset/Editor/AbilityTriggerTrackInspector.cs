using UnityEditor;

namespace AbilitySystem
{
    [CustomEditor(typeof(AbilityTriggerTrack))]
    public class AbilityTriggerTrackInspector : AbilityTrackInspector
    {
        private AbilityTriggerTrack _currentTrack;
        protected override void OnSelectTrack()
        {
            base.OnSelectTrack();
            if (SelectTrackAsset!=null)
            {
                _currentTrack = (AbilityTriggerTrack) CurrentTrackAsset;
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