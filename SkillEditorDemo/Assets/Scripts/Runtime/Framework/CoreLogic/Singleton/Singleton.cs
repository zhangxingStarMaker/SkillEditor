using System;
using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    public interface ISingleton: IDisposable
    {
        void Register();
        void Destroy();
        bool IsDisposed();
    }
    
    public abstract class Singleton<T>: ISingleton where T: Singleton<T>, new()
    {
        private bool isDisposed;
        
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    FrameworkLog.LogError($"{typeof(T)}的实例为null!如果是非运行时，单例可能未完成初始化流程，可以在合适时机使用GameCenterEditor完成初始化！");
                }
                return instance;
            }
        }

        void ISingleton.Register()
        {
            if (instance != null)
            {
                throw new Exception($"singleton register twice! {typeof (T).Name}");
            }
            instance = (T)this;
        }

        void ISingleton.Destroy()
        {
            if (this.isDisposed)
            {
                return;
            }
            this.isDisposed = true;
            
            instance.Dispose();
            instance = null;
        }

        bool ISingleton.IsDisposed()
        {
            return this.isDisposed;
        }

        public virtual void Dispose()
        {
        }
    }
}