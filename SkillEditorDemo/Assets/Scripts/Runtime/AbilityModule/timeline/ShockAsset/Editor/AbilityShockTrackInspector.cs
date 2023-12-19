using UnityEditor;

namespace AbilitySystem
{
    [CustomEditor(typeof(AbilityShockTrack))]
    public class AbilityShockTrackInspector : AbilityTrackInspector
    {
        private AbilityShockTrack _currentTrack;
        protected override void OnSelectTrack()
        {
            base.OnSelectTrack();
            if (SelectTrackAsset!=null)
            {
                _currentTrack = (AbilityShockTrack) CurrentTrackAsset;
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