using System;
using System.Collections.Generic;

namespace Moudule.ETTaskCore
{
    public class ETCancellationToken
    {
        private readonly HashSet<Action> actions = new HashSet<Action>();
        private bool _isCanceled = false;
        public void Add(Action callback)
        {
            // 如果action是null，绝对不能添加,要抛异常，说明有协程泄漏
            this.actions.Add(callback);
        }
        
        // 为了Token的复用，支持了恢复数据操作，但使用要注意，确保没有在使用此token的情况下在重置复用
        public void ResetToken()
        {
            if (!_isCanceled)
            {
                EtTaskDebugger.LogError($"尝试重置一个尚未取消的token!");
                return;
            }
            _isCanceled = false;
        }
        
        public void Remove(Action callback)
        {
            this.actions.Remove(callback);
        }

        public bool IsDispose()
        {
            return _isCanceled;
        }

        public void Cancel()
        {
            if (_isCanceled)
            {
                return;
            }

            this.Invoke();
        }

        private void Invoke()
        {
            _isCanceled = true;
            try
            {
                foreach (Action action in this.actions)
                {
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
                ETTask.ExceptionHandler.Invoke(e);
            }
        }
    }
}