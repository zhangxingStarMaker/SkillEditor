using Module.FrameBase;
using Module.ObjectPool;

namespace BuffModule.Runtime
{
    public class BuffPoolComponent : CoreEntity,ICoreEntityAwake
    {
        public void OnAwake()
        {
            CommonObjectPool<BuffInfoEntity>.Init(CreateClass<BuffInfoEntity>);
            CommonObjectPool<BuffData>.Init(CreateClass<BuffData>);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T CreateClass<T>() where T : new()
        {
            return new T();
        }

        public BuffInfoEntity GetBuffInfoEntity()
        {
            return CommonObjectPool<BuffInfoEntity>.Get();
        }
        
        public BuffData GetBuffData()
        {
            return CommonObjectPool<BuffData>.Get();
        }

    }
}