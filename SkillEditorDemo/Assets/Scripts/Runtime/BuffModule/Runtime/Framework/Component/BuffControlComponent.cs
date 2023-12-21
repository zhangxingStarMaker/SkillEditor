using System.Collections.Generic;
using Module.FrameBase;

namespace BuffModule.Runtime.Component
{
    public class BuffControlComponent : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate,ICoreEntityLateUpdate
    {
        private LinkedList<BuffInfoEntity> _buffInfoEntityLinkedList = new LinkedList<BuffInfoEntity>();
        public void OnAwake()
        {
            
        }

        public void OnUpdate()
        {
            
        }

        public void OnLateUpdate()
        {
            
        }
    }
}