using Module.ObjectPool;

namespace BuffModule.Runtime
{
    public class BuffData : IClearObj
    {
        public BuffItem BuffItem;
        public BuffExecute OnCreate;
        public BuffExecute OnHit;
        public BuffExecute OnBeHurt;
        public BuffExecute OnKill;
        public BuffExecute OnBeKill;
        public BuffExecute OnTick;
        
        public void Clear()
        {
            OnCreate = null;
            OnHit = null;
            OnBeHurt = null;
            OnKill = null;
            OnBeKill = null;
            OnTick = null;
        }
    }
}