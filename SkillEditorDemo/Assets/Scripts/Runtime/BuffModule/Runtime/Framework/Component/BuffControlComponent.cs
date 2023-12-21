using System.Collections.Generic;
using Module.FrameBase;
using UnityEngine;

namespace BuffModule.Runtime.Component
{
    public class BuffControlComponent : CoreEntity,ICoreEntityAwake,ICoreEntityUpdate,ICoreEntityLateUpdate,ICoreEntityDestroy
    {
        private BuffModuleEntity _buffModuleEntity;
        private LinkedList<BuffInfoEntity> _buffInfoEntityLinkedList;
        public void OnAwake()
        {
            _buffInfoEntityLinkedList = new LinkedList<BuffInfoEntity>();
        }

        public void OnInit(BuffModuleEntity buffModuleEntity)
        {
            _buffModuleEntity = buffModuleEntity;
        }

        /// <summary>
        /// 直接移除buff效果,需要判断当前层数
        /// </summary>
        /// <param name="buffInfoEntity"></param>
        public void RemoveBuff(BuffInfoEntity buffInfoEntity)
        {
            buffInfoEntity.ReductionBuff();
            CheckBuffInfoState(buffInfoEntity);
        }

        /// <summary>
        /// 添加buff
        /// </summary>
        /// <param name="buffInfoEntity"></param>
        public void AddBuff(BuffInfoEntity buffInfoEntity)
        {
            BuffInfoEntity findBuffInfoEntity = FindBuffInfoEntity(buffInfoEntity.BuffId);
            if (findBuffInfoEntity!=null)
            {
                if (buffInfoEntity.CanOverlay())
                {
                    buffInfoEntity.DirectOverlay();
                }
            }
            else
            {
                buffInfoEntity.DirectOverlay();
                _buffInfoEntityLinkedList.AddLast(buffInfoEntity);
                //进行排序,确定执行先后顺序
                InsertionSortLinkedList(_buffInfoEntityLinkedList);
            }
        }
        
        private void InsertionSortLinkedList(LinkedList<BuffInfoEntity> list)
        {
            if (list == null || list.Count < 2) return;
            
            LinkedList<BuffInfoEntity> sortedList = new LinkedList<BuffInfoEntity>();
            foreach (var value in list) {
                var node = sortedList.First;
                while (node != null && node.Value.BuffPriority > value.BuffPriority) {
                    node = node.Next;
                }
                if (node == null) {
                    sortedList.AddLast(value);
                } else {
                    sortedList.AddBefore(node, value);
                }
            }
        }

        private BuffInfoEntity FindBuffInfoEntity(uint buffId)
        {
            foreach (var buffInfoEntity in _buffInfoEntityLinkedList)
            {
                if (buffInfoEntity.BuffId == buffId)
                {
                    return buffInfoEntity;
                }
            }
            return default;
        }
        

        public void OnUpdate()
        {
            if (_buffInfoEntityLinkedList!=null)
            {
                for (LinkedListNode<BuffInfoEntity> node = _buffInfoEntityLinkedList.First; node != null; node = node.Next)
                {
                    node.Value.OnUpdate(Time.deltaTime);
                    CheckBuffInfoState(node.Value);
                }
            }
        }

        public void OnLateUpdate()
        {
            
        }

        private void CheckBuffInfoState(BuffInfoEntity buffInfoEntity)
        {
            switch (buffInfoEntity.BuffRunningState)
            {
                case BuffRunningState.Expired:
                    buffInfoEntity.DirectRemove();
                    _buffInfoEntityLinkedList.Remove(buffInfoEntity);
                    break;
            }
        }

        public void OnDestroy()
        {
            _buffModuleEntity = null;
            _buffInfoEntityLinkedList = null;
        }
    }
}