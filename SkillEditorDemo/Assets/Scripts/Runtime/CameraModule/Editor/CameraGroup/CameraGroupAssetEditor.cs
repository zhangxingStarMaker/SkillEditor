using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;

namespace CameraModule.Editor
{
    // [CustomEditor(typeof(CameraGroupAsset))]
    // public class CameraGroupAssetEditor : UnityEditor.Editor
    // {
    //     private CameraGroupAsset _cameraGroupAsset;
    //     private void OnEnable()
    //     {
    //         _cameraGroupAsset = (CameraGroupAsset)target;
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         // ShowButtonList();
    //     }
    //
    //     /// <summary>
    //     /// 按钮合集
    //     /// </summary>
    //     private void ShowButtonList()
    //     {
    //         if (GUILayout.Button("添加相机组", GUILayout.Width(0)))
    //         {
    //             OnClickAddCameraGroup();
    //         }
    //     }
    //
    //     /// <summary>
    //     /// 添加相机组
    //     /// </summary>
    //     private void OnClickAddCameraGroup()
    //     {
    //         CameraGroupItemAsset cameraGroupItemAsset = new CameraGroupItemAsset();
    //         _cameraGroupAsset.CameraGroupItemAssetList.Add(cameraGroupItemAsset);
    //         OnRefreshButton();
    //     }
    //
    //     /// <summary>
    //     /// 按钮事件刷新
    //     /// </summary>
    //     private void OnRefreshButton()
    //     {
    //         serializedObject.Update();
    //     }
    //     
    //     
    // }
}