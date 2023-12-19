using Moudule.ETTaskCore;

namespace Module.FrameBase
{
    //通知所有单例创建完成
    public interface IAllSingletonCreateComplete
    {
        void OnAllSingletonCreateComplete();
    }
}