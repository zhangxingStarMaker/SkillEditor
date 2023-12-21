using Module.FrameBase;

namespace BuffModule.Runtime
{
    public class BuffInfoEntity : CoreEntity
    {
        private BuffData _buffData;
        private float _durationTimer;//运行总时长
        private float _tickTimer;//单次执行的时间
        private uint _currentStack;//当前层数

        public void OnInit(BuffData buffData)
        {
            _buffData = buffData;
        }
        
        public void OnUpdate(float frameTime)
        {
            _durationTimer += frameTime;
            _tickTimer += frameTime;
        }

        private void CheckState()
        {
            
        }
        
        // public 
        
    }
}