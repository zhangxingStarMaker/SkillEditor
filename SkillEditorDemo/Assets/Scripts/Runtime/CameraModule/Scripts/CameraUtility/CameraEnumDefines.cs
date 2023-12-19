namespace CameraModule.Runtime
{
    /// <summary>
    /// 相机组资源类型
    /// </summary>
    [System.Serializable]
    public enum CameraGroupJumpType
    {
        Pre=1,
        LinkBase=2,
        LinkAll=3,
        JumpNormalPre=4,//正常跳跃准备(未出现标记 MarkType == NotHappen 或者Not)
        JumpNormalAir=5,//正常跳跃腾空
        JumpCircle=6,//跳跃存周
        JumpMarkPre=7,//出现标记（MarkType）后 跳跃准备
        JumpMarkAir=8,//出现标记后 跳跃腾空
        JumpFall=9, //摔倒
    }
    
    public enum CameraTag
    {
        Pre=1,
        LinkBase=2,
        LinkAll=3,
        JumpNormalPre=4,
        JumpNormalAir=5,
        JumpCircle=6,
        JumpBladePre=7,
        JumpBladeAir=8,
        JumpFall=9
    }
    
    
}