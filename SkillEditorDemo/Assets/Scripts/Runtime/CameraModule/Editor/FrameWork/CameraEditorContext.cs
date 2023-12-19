using System;
using System.Collections.Generic;
using CameraModule.Runtime;
using CameraModule.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CameraModule.Editor
{
    public class CameraEditorContext
    {
        // protected override void InitFramework()
        // {
        //     base.InitFramework();
        //     CameraAsset = AddCameraCtrl(CameraCtrlNames.Asset, new CameraEditorAssetCtrl());
        //     // CameraPrepareEntity 
        //     CameraDataCtrl.InitCameraConfigure();
        // }
        //
        // public void EditorClear()
        // {
        //     GameObject root = GameObject.Find("CameraRootNode");
        //     if (root!=null)
        //     {
        //         CameraEditorDebug.LogError("删除根节点");
        //         Object.DestroyImmediate(root);
        //     }
        //     // if (CameraRootNode!=null&& CameraRootNode.CameraRootTransform!=null)
        //     // {
        //     //     Object.DestroyImmediate(CameraRootNode.CameraRootTransform.gameObject);
        //     // }
        // }
        //
        // public override void OnPrepareLinkCameraControl(List<CameraData> cameraDataList, Action loadCompleted)
        // {
        //     foreach (var cameraData in cameraDataList)
        //     {
        //         string assetName = PathDefine.NormalAssetRootPath + cameraData.AssetName;
        //         cameraData.CameraAsset = AssetDatabase.LoadAssetAtPath<CameraAsset>(assetName);
        //     }
        // }
    }
}