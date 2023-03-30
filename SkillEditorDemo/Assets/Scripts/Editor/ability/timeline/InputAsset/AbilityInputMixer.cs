using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace AbilitySystem
{
    public class AbilityInputMixer : PlayableBehaviour
    {
        private float[] _weightRecords;
        private bool _during;
        private AbilityInputBehavior _curBehavior;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            var length = playable.GetInputCount();
            _weightRecords = _weightRecords ?? (_weightRecords = new float[length]);
            for (int index = 0; index < length; index++)
            {
                var weight = playable.GetInputWeight(index);
                _weightRecords[index] = weight;
            }
        }

        private void OnEnter(AbilityInputBehavior behavior)
        {
            _during = true;
            _curBehavior = behavior;
            if (!_curBehavior.IsFrameEvent) return;

            switch (_curBehavior.eventType)
            {
                case AbilityInputBehavior.TrackEventType.NextNode:
                    // AbilityEventManager.Ins.OnEventType(_curBehavior.branchList[0], 0,false);
                    break;
                case AbilityInputBehavior.TrackEventType.Skill:
                    // AbilityEventManager.Ins.OnIndependenceSkill(_curBehavior.skillData);
                    break;
            }

        }

        private void OnExit()
        {
            _during = false;
            _curBehavior = null;
        }

        private void OnProcess()
        {
            if (!_during) return;
            if (_curBehavior.IsFrameEvent) return;
            //
            switch (_curBehavior.eventType)
            {
                case AbilityInputBehavior.TrackEventType.InputListen:
                    OnSelectInputListen();
                    break;
                case AbilityInputBehavior.TrackEventType.Collision:
                    OnSelectInputCollision();
                    break;
            }
        }

        private void OnSelectInputListen()
        {
            // var branchList = _curBehavior.branchList;
            var keyCurrent = Keyboard.current;
            if (keyCurrent.anyKey.wasPressedThisFrame)
            {
                // var map = AbilityEventManager.Ins.InputMap();
                // foreach (var kv in map)
                // {
                //     if (keyCurrent[kv.Key].isPressed)
                //     {
                //         var branchIndex = branchList.IndexOf(kv.Value);
                //         if (branchIndex >= 0)
                //         {
                //             // 触发响应
                //             AbilityEventManager.Ins.OnEventType(kv.Value, branchIndex, false);
                //             _during = false;
                //             return;
                //         }
                //     }
                // }
            }
        }

        private void OnSelectInputCollision()
        {
            Debug.LogError("触发索敌接口逻辑");

            // 索敌成功释放buff
            // AbilityEventManager.Ins.OnIndependenceBuff(_curBehavior.buffTarget);
            _during = false;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            for (var index = 0; index < playable.GetInputCount(); index++)
            {
                var clip = (ScriptPlayable<AbilityInputBehavior>) playable.GetInput(index);
                var weight = playable.GetInputWeight(index);

                if (weight != _weightRecords[index])
                {
                    _weightRecords[index] = weight;
                    if (weight == 1)
                    {
                        var behavior = clip.GetBehaviour();
                        // onenter
                        OnEnter(behavior);
                        return;
                    }

                    // onexit
                    OnExit();
                    return;
                }

                OnProcess();
            }
        }
    }
}
