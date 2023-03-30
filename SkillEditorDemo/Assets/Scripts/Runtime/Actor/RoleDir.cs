using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class RoleDir : MonoBehaviour
{
    [ExecuteAlways]
    public Transform headRig;

    //public Transform customLight;
    
    private static readonly int UpDir = Shader.PropertyToID("_RoleUpDir");
    private static readonly int ForwardDir = Shader.PropertyToID("_RoleForwardDir");
    private static readonly int RightDir = Shader.PropertyToID("_RoleRightDir");
    //private static readonly int LightDir = Shader.PropertyToID("_RolLightDir");
    
    private void Update()
    {
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {

        var Up = headRig.up;
        Shader.SetGlobalVector(UpDir,new Vector4(Up.x,0,Up.z,1));
        var forward = headRig.forward;
        Shader.SetGlobalVector(ForwardDir,new Vector4(-forward.x,0,-forward.z,1));
        var right = headRig.right;
        Shader.SetGlobalVector(RightDir,new Vector4(right.x,0,right.z,1));
        //var light = customLight.forward;
        //Shader.SetGlobalVector(LightDir,new Vector4(-light.x,0,-light.z,1));


    }
}
