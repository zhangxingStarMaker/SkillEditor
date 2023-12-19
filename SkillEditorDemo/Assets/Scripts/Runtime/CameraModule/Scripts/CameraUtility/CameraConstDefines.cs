namespace CameraModule.Runtime
{

    /// <summary>
    /// 相机的状态
    /// </summary>
    public enum CameraWorkState
    {
        None = 0,
        Free = 1, //空闲状态
        Initialization = 2, //正在初始化
        InitializationCompleted = 3,//初始化完毕
        Working = 4,//工作中
        Ended = 5,//工作完成状态
        Error = 6,//异常状态
    }

    public enum CameraPlayMode
    {
        None = 0,
        Free = 1,
        Single = 2,//单个播放模式
        Link = 3,//连续播放模式
    }
    
    /// <summary>
    /// Mark类型
    /// </summary>
    public enum CameraMarkType
    {
        None = 0,
        Actor = 1, //坐标跟随角色
        World = 2, //坐标世界坐标变化
        Path = 3,//坐标路径变化
    }
    
    public enum VirtualCameraPriority
    {
        None = 0,
        DefaultCamera = 1,
        SceneCamera = 2,
        ScriptCamera = 3
    }
    
    /// <summary>
    /// Mark类型
    /// </summary>
    public enum CameraEntityType
    {
        None = 0,
        Normal = 1, //常规相机
        Art = 2, //美术相机
    }

    public enum CameraCtrlNames
    {
        Control,//相机控制，包含各种模式
        Asset,//资源相关，会包含对象的重复使用
        Entity,//实体
        PoolList,//对象池，包含已经实例化GameObject和加载的资源
        Data,//缓存数据
        MaxCount,
    }
    
    /// <summary>
    /// 存放相机的一些常量
    /// </summary>
    public class CameraConstDefines
    {
        public const string CameraConfigurePath = "art_online/battle/camera/configure_asset/camera_configure.asset";
        public const string CameraConfigureEditorPath = "Assets/ArtRes/" + CameraConfigurePath;
    }
}