namespace CameraModule.Runtime
{
    public enum CameraPriority
    {
        None = 0,
        DefaultCamera = 1,
        ScriptCamera = 2,
        MoveCamera = 3,
        LevelCamera = 4,
        SkillCamera = 5,
        PerformanceCamera = 6,
    }

    
    public enum CameraTarget
    {
        None,
        World,
        Player_Root,
        Head,
        Chest,
        LeftFoot,
        RightFoot,
        Path,
        BgCenter,
    }

    public enum CameraPlayerMarkOffsetType
    {
        World,
        Local,
        Spline,
    }
    
    public enum CameraPathMarkOffsetType
    {
        World,
        Local,
    }
}
