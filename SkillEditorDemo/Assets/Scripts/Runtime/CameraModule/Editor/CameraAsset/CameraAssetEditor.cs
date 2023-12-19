using UnityEngine;
using UnityEditor;
using CameraModule.Runtime;
using CameraType = CameraModule.Runtime.CameraType;


namespace Module.Battle.Editor
{
    [CustomEditor(typeof(CameraAsset))]
    public class CameraAssetEditor : UnityEditor.Editor
    {
        private CameraAsset _cameraAsset;

        private SerializedProperty _cameraName;
        private SerializedProperty _cameraType;
        private SerializedProperty _virtualCameraAsset;

        private SerializedProperty _cameraFollowMarks;
        private SerializedProperty _cameraLookMarks;
        
        private SerializedProperty _cameraMarkTarget;
        
        private SerializedProperty _weight;
        private SerializedProperty _isSlow;
        private SerializedProperty _speed;

        private SerializedProperty _timeOffsetControl;
        private SerializedProperty _timeOffset;

        private SerializedProperty _worldPosition;
        private SerializedProperty _worldRotation;

        private SerializedProperty _actorOffsetControl;
        private SerializedProperty _actorOffsetType;
        private SerializedProperty _actorOffset;

        private SerializedProperty _pathOffsetControl;
        private SerializedProperty _pathOffsetType;
        private SerializedProperty _pathOffset;

        private SerializedProperty _rangeControl;
        private SerializedProperty _rangeX;
        private SerializedProperty _rangeY;

        private SerializedProperty _animtionClip;
        private SerializedProperty _transitionTime;

        private bool _isEditorCamera = true;
        private bool _isEditorCameraFollow = true;
        private bool _isEditorCameraLook = true;

        private bool _isEditorCameraNew = false;
        private bool _isEditorCameraFollowNew = true;
        private bool _isEditorCameraLookNew = true;

        private Color _defaultColor;


        private void OnEnable()
        {
            _defaultColor = GUI.color;

            _cameraAsset = (CameraAsset) this.target;
            _cameraName = serializedObject.FindProperty("CameraName");
            _cameraType = serializedObject.FindProperty("CurCameraType");
            _virtualCameraAsset = serializedObject.FindProperty("VirtualCamera");
            _cameraFollowMarks = serializedObject.FindProperty("CameraFollowMarks");
            _cameraLookMarks = serializedObject.FindProperty("CameraLookMarks");
            _animtionClip = serializedObject.FindProperty("AnimationClip");
            _transitionTime = serializedObject.FindProperty("TransitionTime");
        }
        public override void OnInspectorGUI()
        {
            GUI.skin.button.wordWrap = true;
            
            _isEditorCamera = EditorGUILayout.Foldout(_isEditorCamera, "相机编辑");
            if (!_isEditorCamera) return;

            EditorGUILayout.Space();
            
            if (_cameraType != null)
            {
                EditorGUILayout.PropertyField(_cameraType, new GUIContent("相机类型"));
            }
            
            if (_virtualCameraAsset != null)
            {
                EditorGUILayout.PropertyField(_virtualCameraAsset, new GUIContent("相机资源"));
            }

            if (_cameraType != null && _cameraType.enumValueIndex == (int) CameraType.AnimationCamera)
            {
                EditorGUILayout.PropertyField(_animtionClip, new GUIContent("相机动画资源"));
                EditorGUILayout.PropertyField(_transitionTime, new GUIContent("过渡时间"));
                serializedObject.ApplyModifiedProperties();
                return;
            }
            _isEditorCameraFollow = EditorGUILayout.Foldout(_isEditorCameraFollow, "跟随目标点组-----------------");
            if (_isEditorCameraFollow)
            {
                for (int i = 0; i < _cameraFollowMarks.arraySize; i++)
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("目标点" + i);
                    GUI.color = _defaultColor;

                    OnCameraMarkInspector(_cameraFollowMarks.GetArrayElementAtIndex(i));

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(" ");
                    if (GUILayout.Button("X", GUILayout.Width(0)))
                    {
                        _cameraAsset.CameraFollowMarks.RemoveAt(i);
                        serializedObject.Update();
                    }

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.Space();
            _isEditorCameraLook = EditorGUILayout.Foldout(_isEditorCameraLook, "看向目标点组---------------------");
            if (_isEditorCameraLook)
            {
                for (int i = 0; i < _cameraLookMarks.arraySize; i++)
                {
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField("目标点" + i);
                    GUI.color = _defaultColor;

                    OnCameraMarkInspector(_cameraLookMarks.GetArrayElementAtIndex(i));

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(" ");
                    if (GUILayout.Button("X", GUILayout.Width(0)))
                    {
                        _cameraAsset.CameraLookMarks.RemoveAt(i);
                        serializedObject.Update();
                    }

                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("增加跟随目标点", GUILayout.Width(0)))
            {
                _cameraAsset.CameraFollowMarks.Add(new CameraMarkData());
                serializedObject.Update();
            }

            if (GUILayout.Button("增加看向目标点", GUILayout.Width(0)))
            {
                _cameraAsset.CameraLookMarks.Add(new CameraMarkData());
                serializedObject.Update();
            }
            GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnCameraMarkInspector(SerializedProperty cameraMarkEditorEntity)
        {
            _cameraMarkTarget = cameraMarkEditorEntity.FindPropertyRelative("CameraMarkTarget");
            _weight = cameraMarkEditorEntity.FindPropertyRelative("ScaleWeight");
            _isSlow = cameraMarkEditorEntity.FindPropertyRelative("IsSlow");
            _speed = cameraMarkEditorEntity.FindPropertyRelative("Speed");

            EditorGUILayout.PropertyField(_isSlow, new GUIContent("缓动开关"));
            if (_isSlow.boolValue) EditorGUILayout.PropertyField(_speed, new GUIContent("缓动速度"));
            EditorGUILayout.PropertyField(_weight, new GUIContent("权重"));

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_cameraMarkTarget, new GUIContent("目标"));
            if (_cameraMarkTarget.enumValueIndex == (int) CameraTarget.World)
            {
                _worldPosition = cameraMarkEditorEntity.FindPropertyRelative("WorldPosition");
                _worldRotation = cameraMarkEditorEntity.FindPropertyRelative("WorldRotation");

                EditorGUILayout.PropertyField(_worldPosition, new GUIContent("世界目标点位置"));
                EditorGUILayout.PropertyField(_worldRotation, new GUIContent("世界目标点朝向"));
            }

            if (_cameraMarkTarget.enumValueIndex == (int) CameraTarget.Player_Root
                ||_cameraMarkTarget.enumValueIndex == (int) CameraTarget.Head
                ||_cameraMarkTarget.enumValueIndex == (int) CameraTarget.Chest
                ||_cameraMarkTarget.enumValueIndex == (int) CameraTarget.LeftFoot
                ||_cameraMarkTarget.enumValueIndex == (int) CameraTarget.RightFoot)
            {
                _actorOffsetControl = cameraMarkEditorEntity.FindPropertyRelative("ActorOffsetControl");
                _actorOffsetType = cameraMarkEditorEntity.FindPropertyRelative("ActorOffsetType");
                _actorOffset = cameraMarkEditorEntity.FindPropertyRelative("ActorOffset");

                EditorGUILayout.PropertyField(_actorOffsetControl, new GUIContent("角色目标点偏移控制"));
                if (_actorOffsetControl.boolValue)
                {
                    EditorGUILayout.PropertyField(_actorOffsetType, new GUIContent("角色目标点偏移类型"));
                    EditorGUILayout.PropertyField(_actorOffset, new GUIContent("角色目标点偏移"));
                }
            }

            if (_cameraMarkTarget.enumValueIndex == (int) CameraTarget.Path)
            {
                _timeOffsetControl = cameraMarkEditorEntity.FindPropertyRelative("TimeOffsetControl");
                _timeOffset = cameraMarkEditorEntity.FindPropertyRelative("TimeOffset");

                _pathOffsetControl = cameraMarkEditorEntity.FindPropertyRelative("PathOffsetControl");
                _pathOffsetType = cameraMarkEditorEntity.FindPropertyRelative("PathOffsetType");
                _pathOffset = cameraMarkEditorEntity.FindPropertyRelative("PathOffset");

                EditorGUILayout.PropertyField(_timeOffsetControl, new GUIContent("时间偏移控制"));
                if (_timeOffsetControl.boolValue) EditorGUILayout.PropertyField(_timeOffset, new GUIContent("时间偏移"));

                EditorGUILayout.PropertyField(_pathOffsetControl, new GUIContent("目标点偏移控制"));
                if (_pathOffsetControl.boolValue)
                {
                    EditorGUILayout.PropertyField(_pathOffsetType, new GUIContent("目标点偏移类型"));
                    EditorGUILayout.PropertyField(_pathOffset, new GUIContent("目标点偏移"));
                }
            }
        }
    }
}