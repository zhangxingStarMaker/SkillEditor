using Module.FrameBase;
using Module.ObjectPool;
using Runtime.Framework.FrameUtility;

namespace BuffModule.Runtime
{
    public class BuffInfoEntity : CoreEntity,ICoreEntityDestroy
    {
        #region private

        private BuffData _buffData;
        private float _durationTimer;//运行总时长
        private float _tickTimer;//单次执行的时间
        private uint _currentStack;//当前层数
        private BuffRunningState _buffRunningState;

        #endregion

        #region public

        public CoreEntity TargetEntity;//目标者
        public CoreEntity SendEntity;//发起者
        public uint BuffId => _buffData.BuffItem.ID;
        public BuffPriority BuffPriority => _buffData.BuffItem.BuffPriority;

        public BuffRunningState BuffRunningState => _buffRunningState;

        #endregion

        public void OnInit(BuffData buffData)
        {
            _buffData = buffData;
            _buffRunningState = BuffRunningState.Active;
        }
        
        public void OnUpdate(float frameTime)
        {
            if (_tickTimer < 0)
            {
                OnExecute(_buffData.OnTick);
                _tickTimer = _buffData.BuffItem.TickTime;
            }
            else
            {
                _tickTimer -= frameTime;
            }

            if (_durationTimer < 0)
            {
                ChangeState(BuffRunningState.Expired);
            }
            else
            {
                _durationTimer -= frameTime;
            }
        }

        /// <summary>
        /// 是否可以叠加
        /// </summary>
        /// <returns></returns>
        public bool CanOverlay()
        {
            return _currentStack < _buffData.BuffItem.MaxStack;
        }

        /// <summary>
        /// 直接叠加
        /// </summary>
        public void DirectOverlay()
        {
            _currentStack++;
            switch (_buffData.BuffItem.BuffAddTimeUpdate)
            {
                case BuffAddTimeUpdate.Add:
                    _durationTimer += _buffData.BuffItem.DurationTime;
                    break;
                case BuffAddTimeUpdate.Replace:
                    _durationTimer = _buffData.BuffItem.DurationTime;
                    break;
            }
            OnExecute(_buffData.OnCreate);
        }

        /// <summary>
        /// 外部需要判断是否可移除
        /// </summary>
        /// <returns></returns>
        public void ReductionBuff()
        {
            switch (_buffData.BuffItem.BuffRemoveStackUpdate)
            {
                case BuffRemoveStackUpdate.Clear:
                    ChangeState(BuffRunningState.Expired);
                    break;
                case BuffRemoveStackUpdate.Reduce:
                    ReduceBuff();
                    break;
            }
        }

        private void ReduceBuff()
        {
            _currentStack--;
            if (_currentStack == 0)
            {
                ChangeState(BuffRunningState.Expired);
                return;
            }

            _durationTimer -= _buffData.BuffItem.DurationTime;
            OnExecute(_buffData.OnRemove);//删除叠层的时候也需要触发remove
            if (_durationTimer <= 0)
            {
                FrameworkLog.LogError("计算出错，出现减少层数后时间小于零的情况");
            }
        }

        /// <summary>
        /// 内部无权限直接释放需要外部调用
        /// </summary>
        public void DirectRemove()
        {
            OnExecute(_buffData.OnRemove);
            Dispose();
        }

        private void OnExecute(BuffExecute buffExecute)
        {
            buffExecute?.OnExecute(this);
        }
        
        private void ChangeState(BuffRunningState buffRunningState)
        {
            _buffRunningState = buffRunningState;
        }

        public void OnDestroy()
        {
            CommonObjectPool<BuffData>.Release(_buffData);
            _durationTimer = 0;
            _tickTimer = 0;
            _currentStack = 0;
            _buffRunningState = BuffRunningState.None;
        }
    }
}