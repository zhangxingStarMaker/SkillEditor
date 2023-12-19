using System;
using System.Collections.Generic;
using System.Globalization;
using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    /// <summary>
    /// 相机配置脚本
    /// </summary>
    [CustomEditor(typeof(CameraConfigureAsset))]
    public class CameraConfigureAssetEditor : UnityEditor.Editor
    {
        private CameraConfigureAsset _cameraConfigureAsset;
        private bool _isEditorCameraRule = false;
        private readonly List<bool> _cameraRuleItemShow = new List<bool>();
        private bool _isEditorCameraPath = false;
        private readonly List<bool> _cameraPathItemShow = new List<bool>();
        private bool _isEditorCameraGroup = false;
        private readonly List<bool> _cameraGroupItemShow = new List<bool>();
        private bool _isEditorJumpCamera = false;
        private readonly List<bool> _cameraJumpItemShow = new List<bool>();
        private readonly List<bool> _cameraJumpNormalPreCameraShow = new List<bool>();
        private readonly List<bool> _cameraJumpNormalAirCameraShow = new List<bool>();
        private readonly List<bool> _cameraJumpNormalMarkCameraShow = new List<bool>();
        private readonly List<bool> _cameraJumpNormalFallCameraShow = new List<bool>();
        private readonly List<bool> _cameraJumpBladeAirCameraShow = new List<bool>();
        private readonly List<bool> _cameraJumpBladePreCameraShow = new List<bool>();
        
        private bool _isEditorRotationCamera = false;
        private readonly List<bool> _cameraRotationItemShow = new List<bool>();
        private readonly List<bool> _cameraRotationNormalItemShow = new List<bool>();
        
        private SerializedProperty _cameraRuleAssetList;
        private SerializedProperty _cameraPathAssetList;
        private SerializedProperty _cameraGroupAssetList;

        //跳跃
        private SerializedProperty _cameraGroupJumpNormalPreTakeoffAssetList;
        private SerializedProperty _cameraGroupJumpNormalAirTakeoffAssetList;
        private SerializedProperty _cameraGroupJumpNormalMarkAssetList;
        private SerializedProperty _cameraGroupJumpBladePreAssetList;
        private SerializedProperty _cameraGroupJumpBladeAirAssetList;
        private SerializedProperty _cameraGroupJumpNormalFallAssetList;
        
        //旋转
        private SerializedProperty _cameraRotationNormalGroupList;

        //新增
        private SerializedProperty _cameraGroupJumpDic;
        private Dictionary<CameraGroupJumpType, bool> _cameraGroupJumpShow = new Dictionary<CameraGroupJumpType, bool>();

        private Dictionary<CameraGroupJumpType, List<bool>> _cameraGroupJumpItemShow =
            new Dictionary<CameraGroupJumpType, List<bool>>();
        
        
        private Color _defaultColor;

        private void OnEnable()
        {
            _defaultColor = GUI.color;
            _cameraConfigureAsset = (CameraConfigureAsset)target;

            _cameraGroupJumpShow.Clear();
            _cameraGroupJumpItemShow.Clear();

            _cameraRuleAssetList = serializedObject.FindProperty("CameraRuleAssetList");
            
            InitItemShow(_cameraRuleItemShow, _cameraRuleAssetList);
            _cameraPathAssetList = serializedObject.FindProperty("CameraPathAssetList");
            InitItemShow(_cameraPathItemShow, _cameraPathAssetList);
            _cameraGroupAssetList = serializedObject.FindProperty("CameraGroupAssetList");
            InitItemShow(_cameraGroupItemShow, _cameraGroupAssetList);
            
            _cameraGroupJumpNormalPreTakeoffAssetList = serializedObject.FindProperty("JumpNormalPreCameraGroupList");
            InitItemShow(_cameraJumpNormalPreCameraShow, _cameraGroupJumpNormalPreTakeoffAssetList);
            
            _cameraGroupJumpNormalAirTakeoffAssetList = serializedObject.FindProperty("JumpNormalAirCameraGroupList");
            InitItemShow(_cameraJumpNormalAirCameraShow, _cameraGroupJumpNormalAirTakeoffAssetList);
            
            _cameraGroupJumpNormalMarkAssetList = serializedObject.FindProperty("JumpNormalMarkCameraGroupList");
            InitItemShow(_cameraJumpNormalMarkCameraShow, _cameraGroupJumpNormalMarkAssetList);
            
            _cameraGroupJumpNormalFallAssetList = serializedObject.FindProperty("JumpNormalFallCameraGroupList");
            InitItemShow(_cameraJumpNormalFallCameraShow, _cameraGroupJumpNormalFallAssetList);
            
            _cameraRotationNormalGroupList = serializedObject.FindProperty("RotationCameraGroupList");
            InitItemShow(_cameraRotationNormalItemShow, _cameraRotationNormalGroupList);
            
            _cameraGroupJumpBladePreAssetList = serializedObject.FindProperty("JumpBladePreCameraGroupList");
            InitItemShow(_cameraJumpBladePreCameraShow, _cameraGroupJumpBladePreAssetList);
            
            _cameraGroupJumpBladeAirAssetList = serializedObject.FindProperty("JumpBladeAirCameraGroupList");
            InitItemShow(_cameraJumpBladeAirCameraShow, _cameraGroupJumpBladeAirAssetList);

            for (int i = 0; i < 6; i++)
            {
                _cameraJumpItemShow.Add(true);
            }
            
            for (int i = 0; i < 1; i++)
            {
                _cameraRotationItemShow.Add(true);
            }
        }
        

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShowRuleContent();
            // ShowPathContent();
            // ShowGroupContent();
            ShowJumpCameraContent();
            ShowRotationCameraContent();
        }

        
        private void ShowCameraGroupJumpContent()
        {
            // _isEditorJumpCamera = ShowOneTitle(_isEditorJumpCamera, "跳跃相机");
            // if (_isEditorCameraGroup)
            // {
            //     foreach (var key in _cameraConfigureAsset.CameraGroupJumpDic.Keys)
            //     {
            //         switch (key)
            //         {
            //             case CameraGroupJumpType.JumpNormalPre:
            //                 ShowGroupJumpContent(CameraGroupJumpType.JumpNormalPre,_cameraConfigureAsset.CameraGroupJumpDic[key],"正常起跳准备");
            //                 break;
            //             case CameraGroupJumpType.JumpNormalAir:
            //                 
            //                 break;
            //         }
            //     }
            // }
        }

        private void ShowGroupJumpContent(CameraGroupJumpType jumpType,List<CameraGroupConfigure> cameraGroupList,string title)
        {
            if (!_cameraGroupJumpShow.ContainsKey(jumpType))
            {
                _cameraGroupJumpShow.Add(jumpType, false);
            }
            bool jumpTakeOffShow = _cameraGroupJumpShow[jumpType];
            jumpTakeOffShow = ShowTwoTitle(jumpTakeOffShow, title);
            _cameraGroupJumpShow[jumpType] = jumpTakeOffShow;
            List<bool> isShowContent = _cameraGroupJumpItemShow[jumpType];
            if (jumpTakeOffShow)
            {
                for (int i = 0; i < cameraGroupList.Count; i++)
                {
                    GUI.color = Color.green;
                    bool isShow = true;
                    // isShow = ShowContentTitle(isShow, title + (i+1),24);
                    // isShowContent[i] = isShow;
                    GUI.color = _defaultColor;

                    if (isShow)
                    {

                        OnCameraGroupJumpItemInspector(cameraGroupList[i],36);

                        ShowDeleteButton(() =>
                        {
                            cameraGroupList.RemoveAt(i);
                            // isShowContent.RemoveAt(i);
                        });
                    }
                }
                
                ShowAddButton(() =>
                {
                    cameraGroupList.Add(new CameraGroupConfigure());
                    // isShowContent.Add(true);
                },8);
            }
        }
        
        private void OnCameraGroupJumpItemInspector(CameraGroupConfigure cameraGroupConfigure,int space = 24)
        {
            ShowPropertyField(cameraGroupConfigure.CameraAssetPathItemList, "相机路径",24);
            ShowSmallAddButton(() =>
            {
                cameraGroupConfigure.CameraAssetPathItemList.Add("");
            },12);
        }

        private void ShowSmallAddButton(Action action,int space = 6)
        {
            GUIStyle buttonStyle = GUI.skin.button;
            buttonStyle.normal.textColor = Color.blue;
            buttonStyle.hover.textColor = Color.blue;
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            if (GUILayout.Button("添加子节点",buttonStyle, GUILayout.Width(80),GUILayout.Height(30)))
            {
                action?.Invoke();
                serializedObject.Update();
            }
            GUILayout.EndHorizontal();
        }
        
        private void ShowSaveButton(int space = 6)
        {
            GUIStyle buttonStyle = GUI.skin.button;
            buttonStyle.normal.textColor = Color.blue;
            buttonStyle.hover.textColor = Color.blue;
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            if (GUILayout.Button("保存",buttonStyle, GUILayout.Width(80),GUILayout.Height(30)))
            {
                EditorUtility.SetDirty(_cameraConfigureAsset); // 标记对象为已修改

                // 保存修改
                AssetDatabase.SaveAssetIfDirty(_cameraConfigureAsset);
            }
            GUILayout.EndHorizontal();
        }
        
        private void ShowPropertyField(List<string> assetPathList,string content,int space = 12)
        {
            for (int i = 0; i < assetPathList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                assetPathList[i] = EditorGUILayout.TextField(content + i, assetPathList[i]);
                
                if (GUILayout.Button("删除",GUILayout.Width(40)))
                {
                    assetPathList.RemoveAt(i);
                    serializedObject.Update();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void ShowPropertyField(List<int> assetPathList,string content,int space = 12)
        {
            for (int i = 0; i < assetPathList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                assetPathList[i] = EditorGUILayout.IntField(content + i, assetPathList[i]);
                
                if (GUILayout.Button("删除",GUILayout.Width(40)))
                {
                    assetPathList.RemoveAt(i);
                    serializedObject.Update();
                }
                EditorGUILayout.EndHorizontal();
            }
            
            // EditorGUILayout.PropertyField(assetPathList.top, new GUIContent(content));
        }

        private void InitItemShow(List<bool> boolList,SerializedProperty propertyList,bool isShow = false)
        {
            for (int i = 0; i < propertyList.arraySize; i++)
            {
                boolList.Add(isShow);
            }
        }

        private GUIStyle GetFoldoutGUIStyle(int size = 16,FontStyle fontStyle = FontStyle.Bold)
        {
            GUIStyle titleGUIStyle = new GUIStyle(EditorStyles.foldout);
            titleGUIStyle.fontSize = size;
            titleGUIStyle.fontStyle = fontStyle;
            titleGUIStyle.normal.textColor = GUI.color;
            return titleGUIStyle;
        }
        
        private void ShowPropertyField(SerializedProperty property,string content,int space = 12)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUILayout.PropertyField(property, new GUIContent(content));
            EditorGUILayout.EndHorizontal();
        }

        private bool ShowOneTitle(bool editorShow,string title)
        {
            GUIStyle titleGUIStyle = GetFoldoutGUIStyle();
            return EditorGUILayout.Foldout(editorShow, title,true,titleGUIStyle);
        }

        private bool ShowContentTitle(bool editorShow,string title,int space = 12)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(space);
            bool isShow = EditorGUILayout.Foldout(editorShow, title,true);
            EditorGUILayout.EndHorizontal();
            return isShow;
        }
        
        private bool ShowTwoTitle(bool editorShow,string title)
        {
            GUIStyle titleGUIStyle = GetFoldoutGUIStyle(14,FontStyle.Normal);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            bool isShow = EditorGUILayout.Foldout(editorShow, title,true,titleGUIStyle);
            EditorGUILayout.EndHorizontal();
            return isShow;
        }

        private void ShowDeleteButton(Action action)
        {
            GUIStyle buttonStyle = GUI.skin.button;
            buttonStyle.normal.textColor = _defaultColor;
            buttonStyle.hover.textColor = Color.red;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            // GUILayout.Space(12);
            EditorGUILayout.LabelField(" ");
            if (GUILayout.Button("删除",buttonStyle, GUILayout.Width(80),GUILayout.Height(40)))
            {
                action?.Invoke();
                serializedObject.Update();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void ShowAddButton(Action action,int space = 6)
        {
            GUIStyle buttonStyle = GUI.skin.button;
            buttonStyle.normal.textColor = Color.yellow;
            buttonStyle.hover.textColor = Color.yellow;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            if (GUILayout.Button("添加",buttonStyle, GUILayout.Width(60),GUILayout.Height(30)))
            {
                action?.Invoke();
                serializedObject.Update();
            }
            GUILayout.EndHorizontal();
        }
        
        private void ShowRuleContent()
        {
            _isEditorCameraRule = ShowOneTitle(_isEditorCameraRule, "相机参数表");
            if (_isEditorCameraRule)
            {
                for (int i = 0; i < _cameraRuleAssetList.arraySize; i++)
                {
                    GUI.color = Color.green;
                    bool isShow = _cameraRuleItemShow[i];
                    isShow = ShowContentTitle(isShow, "规则参数" + (i+1));
                    _cameraRuleItemShow[i] = isShow;
                    GUI.color = _defaultColor;
                    
                    if (isShow)
                    {
                        
                        OnCameraRuleInspector(_cameraRuleAssetList.GetArrayElementAtIndex(i));
                        ShowDeleteButton(() =>
                        {
                            _cameraConfigureAsset.CameraRuleAssetList.RemoveAt(i);
                            _cameraRuleItemShow.RemoveAt(i);
                        });
                    }
                }
                
                GUI.color = _defaultColor;

                ShowAddButton(() =>
                {
                    _cameraConfigureAsset.CameraRuleAssetList.Add(new CameraRuleConfigure());
                    _cameraRuleItemShow.Add(true);
                });
                
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        private void OnCameraRuleInspector(SerializedProperty cameraRuleConfigure)
        {
            SerializedProperty paramDesc = cameraRuleConfigure.FindPropertyRelative("ParamDesc");
            ShowPropertyField(paramDesc, "参数描述");
            SerializedProperty paramName = cameraRuleConfigure.FindPropertyRelative("ParamName");
            ShowPropertyField(paramName, "参数名称");
            SerializedProperty paramValue = cameraRuleConfigure.FindPropertyRelative("ParamValue");
            ShowPropertyField(paramValue, "参数值");
        }

        private void ShowPathContent()
        {
            _isEditorCameraPath = ShowOneTitle(_isEditorCameraPath, "相机资源路径表");
            if (_isEditorCameraPath)
            {
                
                for (int i = 0; i < _cameraPathAssetList.arraySize; i++)
                {
                    GUI.color = Color.green;
                    bool isShow = _cameraPathItemShow[i];
                    isShow = ShowContentTitle(isShow, "相机资源" + (i+1));
                    _cameraPathItemShow[i] = isShow;
                    GUI.color = _defaultColor;

                    if (isShow)
                    {

                        OnCameraPathInspector(_cameraPathAssetList.GetArrayElementAtIndex(i));

                        ShowDeleteButton(() =>
                        {
                            _cameraConfigureAsset.CameraPathAssetList.RemoveAt(i);
                            _cameraPathItemShow.RemoveAt(i);
                            serializedObject.Update();
                        });
                    }
                }
                
                ShowAddButton(() =>
                {
                    _cameraConfigureAsset.CameraPathAssetList.Add(new CameraPathConfigure());
                    _cameraPathItemShow.Add(true);
                });
                
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        private void OnCameraPathInspector(SerializedProperty cameraPathConfigure)
        {
            SerializedProperty id = cameraPathConfigure.FindPropertyRelative("ID");
            ShowPropertyField(id, "相机ID");
            SerializedProperty cameraAssetPath = cameraPathConfigure.FindPropertyRelative("CameraAssetPath");
            ShowPropertyField(cameraAssetPath, "资源路径");
        }

        private void ShowGroupContent()
        {
            _isEditorCameraGroup = ShowOneTitle(_isEditorCameraGroup, "相机组表");
            if (_isEditorCameraGroup)
            {
                
                for (int i = 0; i < _cameraGroupAssetList.arraySize; i++)
                {
                    GUI.color = Color.green;
                    bool isShow = _cameraGroupItemShow[i];
                    isShow = ShowContentTitle(isShow, "相机组" + (i+1));
                    _cameraGroupItemShow[i] = isShow;
                    GUI.color = _defaultColor;

                    if (isShow)
                    {

                        OnCameraGroupInspector(_cameraGroupAssetList.GetArrayElementAtIndex(i));

                        ShowDeleteButton(() =>
                        {
                            _cameraConfigureAsset.CameraGroupAssetList.RemoveAt(i);
                            _cameraGroupItemShow.RemoveAt(i);
                        });
                    }
                }
                
                ShowAddButton(() =>
                {
                    _cameraConfigureAsset.CameraGroupAssetList.Add(new CameraGroupConfigure());
                    _cameraGroupItemShow.Add(true);
                });
                
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        private void OnCameraGroupInspector(SerializedProperty cameraGroupConfigure,int space = 24)
        {
            // SerializedProperty cameraGroupId = cameraGroupConfigure.FindPropertyRelative("CameraGroupId");
            // ShowPropertyField(cameraGroupId, "相机组ID",24);
            // SerializedProperty tagId = cameraGroupConfigure.FindPropertyRelative("TagId");
            // ShowPropertyField(tagId, "标签ID",24);
            // SerializedProperty ruleId = cameraGroupConfigure.FindPropertyRelative("RuleId");
            // ShowPropertyField(ruleId, "规则ID",24);
            
            SerializedProperty cameraAssetPathItemList = cameraGroupConfigure.FindPropertyRelative("CameraAssetPathItemList");
            ShowPropertyField(cameraAssetPathItemList, "相机资源路径组",space);
            // SerializedProperty cameraAssetItemList = cameraGroupConfigure.FindPropertyRelative("CameraAssetItemList");
            // ShowPropertyField(cameraAssetItemList, "相机Id组",space);
            SerializedProperty cameraFixTimeList = cameraGroupConfigure.FindPropertyRelative("CameraFixTimeList");
            ShowPropertyField(cameraFixTimeList, "过度时间组",space);
        }

        private void ShowCameraGroupContent(SerializedProperty serializedProperty,List<bool> isShowContent,List<CameraGroupConfigure> cameraGroupList,string content)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                GUI.color = Color.green;
                bool isShow = isShowContent[i];
                isShow = ShowContentTitle(isShow, content + (i+1),24);
                isShowContent[i] = isShow;
                GUI.color = _defaultColor;

                if (isShow)
                {

                    OnCameraGroupInspector(serializedProperty.GetArrayElementAtIndex(i),36);

                    ShowDeleteButton(() =>
                    {
                        cameraGroupList.RemoveAt(i);
                        isShowContent.RemoveAt(i);
                    });
                }
            }
                
            ShowAddButton(() =>
            {
                cameraGroupList.Add(new CameraGroupConfigure());
                isShowContent.Add(true);
            },12);
        }

        private void ShowJumpCameraContent()
        {
            _isEditorJumpCamera = ShowOneTitle(_isEditorJumpCamera, "跳跃相机");
            if (_isEditorJumpCamera)
            {
                bool jumpTakeOffPreShow = _cameraJumpItemShow[0];
                jumpTakeOffPreShow = ShowTwoTitle(jumpTakeOffPreShow, "跳跃正常起跳准备");
                _cameraJumpItemShow[0] = jumpTakeOffPreShow;
                if (jumpTakeOffPreShow)
                {
                    ShowCameraGroupContent(_cameraGroupJumpNormalPreTakeoffAssetList,_cameraJumpNormalPreCameraShow,_cameraConfigureAsset.JumpNormalPreCameraGroupList,"相机组");
                }
                
                bool jumpTakeOffAirShow = _cameraJumpItemShow[1];
                jumpTakeOffAirShow = ShowTwoTitle(jumpTakeOffAirShow, "跳跃正常起跳滞空");
                _cameraJumpItemShow[1] = jumpTakeOffAirShow;
                if (jumpTakeOffAirShow)
                {
                    ShowCameraGroupContent(_cameraGroupJumpNormalAirTakeoffAssetList,_cameraJumpNormalAirCameraShow,_cameraConfigureAsset.JumpNormalAirCameraGroupList,"相机组");
                }
                
                bool jumpMarkShow = _cameraJumpItemShow[2];
                jumpMarkShow = ShowTwoTitle(jumpMarkShow, "跳跃正常用刃");
                _cameraJumpItemShow[2] = jumpMarkShow;
                if (jumpMarkShow)
                {
                    ShowCameraGroupContent(_cameraGroupJumpNormalMarkAssetList,_cameraJumpNormalMarkCameraShow,_cameraConfigureAsset.JumpNormalMarkCameraGroupList,"相机组");
                }
                
                bool jumpFallShow = _cameraJumpItemShow[3];
                jumpFallShow = ShowTwoTitle(jumpFallShow, "跳跃摔倒");
                _cameraJumpItemShow[3] = jumpFallShow;
                if (jumpFallShow)
                {
                    ShowCameraGroupContent(_cameraGroupJumpNormalFallAssetList,_cameraJumpNormalFallCameraShow,_cameraConfigureAsset.JumpNormalFallCameraGroupList,"相机组");
                }
                
                bool jumpBladePre = _cameraJumpItemShow[4];
                jumpBladePre = ShowTwoTitle(jumpBladePre, "跳跃标记准备");
                _cameraJumpItemShow[4] = jumpBladePre;
                if (jumpBladePre)
                {
                    ShowCameraGroupContent(_cameraGroupJumpBladePreAssetList,_cameraJumpBladePreCameraShow,_cameraConfigureAsset.JumpBladePreCameraGroupList,"相机组");
                }
                
                bool jumpBladeAir = _cameraJumpItemShow[5];
                jumpBladeAir = ShowTwoTitle(jumpBladeAir, "跳跃标记滞空");
                _cameraJumpItemShow[5] = jumpBladeAir;
                if (jumpBladeAir)
                {
                    ShowCameraGroupContent(_cameraGroupJumpBladeAirAssetList,_cameraJumpBladeAirCameraShow,_cameraConfigureAsset.JumpBladeAirCameraGroupList,"相机组");
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowRotationCameraContent()
        {
            _isEditorRotationCamera = ShowOneTitle(_isEditorRotationCamera, "旋转相机");
            if (_isEditorRotationCamera)
            {
                bool rotationNormalShow = _cameraRotationItemShow[0];
                rotationNormalShow = ShowTwoTitle(rotationNormalShow, "旋转跳跃");
                _cameraRotationItemShow[0] = rotationNormalShow;
                if (rotationNormalShow)
                {
                    ShowCameraGroupContent(_cameraRotationNormalGroupList,_cameraRotationNormalItemShow,_cameraConfigureAsset.RotationCameraGroupList,"相机组");
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}