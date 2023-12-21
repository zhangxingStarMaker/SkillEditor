using System;
using System.Collections;

namespace Module.Utility
{
    public interface IEventArg
    {
        
    }
    
    public delegate void EventListenerDelegate(IEventArg arg);//定义委托用于传事件基类
    
    public class EventDispatcher
    {
        private readonly Hashtable _listeners = new Hashtable(); //掌控所有类型的委托事件

        public void AddEventListener(uint eventType,EventListenerDelegate listener)
        {
            EventListenerDelegate eventListenerDelegate = _listeners[eventType] as EventListenerDelegate;//获得之前这个类型的委托 如果第一次等于Null 
            eventListenerDelegate = (EventListenerDelegate) Delegate.Combine(eventListenerDelegate, listener);//将两个委托的调用列表连接在一起,成为一个新的委托
            _listeners[eventType] = eventListenerDelegate;//赋值给哈希表中的这个类型
        }

        public void RemoveEventListener(uint eventType, EventListenerDelegate listener)
        {
            EventListenerDelegate eventListener = _listeners[eventType] as EventListenerDelegate;//获得之前这个类型的委托 如果第一次等于Null
            if (eventListener != null)
            {
                eventListener =(EventListenerDelegate)Delegate.Remove(eventListener, listener);//从hEventListener的调用列表中移除listener
                _listeners[eventType] = eventListener;//赋值给哈希表中的这个类型
            }
        }

        public void DispatchEvent(uint eventType,IEventArg arg)
        {
            EventListenerDelegate eventListener = _listeners[eventType] as EventListenerDelegate;
            if (eventListener != null)
            {
                try
                {
                    eventListener(arg);//执行委托
                }
                catch (Exception e)
                {
                    throw new Exception(string.Concat(new string[] { "Error Dispatch event", eventType.ToString(), ":", e.Message, " ", e.StackTrace }), e);
                }
            }
        }

        public void RemoveAll()
        {
            _listeners.Clear();
        }
    }
}