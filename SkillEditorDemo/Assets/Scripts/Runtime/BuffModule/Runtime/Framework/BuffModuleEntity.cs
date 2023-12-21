using Module.FrameBase;

namespace BuffModule.Runtime
{
    public class BuffModuleEntity : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate
    {
        public void OnAwake()
        {
            AddComponent<BuffPoolComponent>();
        }

        public void OnUpdate()
        {
            
        }
    }
}