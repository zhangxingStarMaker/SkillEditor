using System;
using System.Collections.Generic;
using Module.ObjectPool;
using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    [Flags]
    public enum EntityStatus: byte
    {
        None = 0,
        IsFromPool = 1,
        IsRegister = 1 << 1,
        IsComponent = 1 << 2,
    }

    public partial class CoreEntity: DisposeObject
    {
#if ENABLE_VIEW && UNITY_EDITOR
        private UnityEngine.GameObject viewGO;
#endif
        private EntityStatus status = EntityStatus.None;
        protected CoreEntity parent;
        protected CoreEntity domain;
        
        public long InstanceId
        {
            get;
            protected set;
        }
        
        private bool IsFromPool
        {
            get => (this.status & EntityStatus.IsFromPool) == EntityStatus.IsFromPool;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsFromPool;
                }
                else
                {
                    this.status &= ~EntityStatus.IsFromPool;
                }
            }
        }
        
        protected bool IsRegister
        {
            get => (this.status & EntityStatus.IsRegister) == EntityStatus.IsRegister;
            set
            {
                if (this.IsRegister == value)
                {
                    return;
                }

                if (value)
                {
                    this.status |= EntityStatus.IsRegister;
                }
                else
                {
                    this.status &= ~EntityStatus.IsRegister;
                }

                
                if (!value)
                {
                    LogicEntityCollector.Instance.Remove(this.InstanceId);
                }
                else
                {
                    LogicEntityCollector.Instance.Add(this);
                    // CoreEventSystem.Instance.NotifyAddNewEntity(this);
                }

#if ENABLE_VIEW && UNITY_EDITOR
                //将逻辑实体以视图的形式展示出来
                if (value)
                {
                    this.viewGO = new UnityEngine.GameObject(this.ViewName);
                    this.viewGO.AddComponent<ComponentView>().Component = this;
                    this.viewGO.transform.SetParent(this.Parent == null? 
                            UnityEngine.GameObject.Find("Global").transform : this.Parent.viewGO.transform);
                }
                else
                {
                    UnityEngine.Object.Destroy(this.viewGO);
                }
#endif
            }
        }
        
        protected virtual string ViewName
        {
            get
            {
                return this.GetType().Name;    
            }
        }
        
        private bool IsComponent
        {
            get => (this.status & EntityStatus.IsComponent) == EntityStatus.IsComponent;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsComponent;
                }
                else
                {
                    this.status &= ~EntityStatus.IsComponent;
                }
            }
        }
        
        public bool IsDisposed => this.InstanceId == 0;
        
        // 可以改变parent，但是不能设置为null
        public CoreEntity Parent
        {
            get => this.parent;
            private set
            {
                if (value == null)
                {
                    throw new Exception($"cant set parent null: {this.GetType().Name}");
                }
                
                if (value == this)
                {
                    throw new Exception($"cant set parent self: {this.GetType().Name}");
                }

                // 严格限制parent必须要有domain,也就是说parent必须在数据树上面
                if (value.Domain == null)
                {
                    throw new Exception($"cant set parent because parent domain is null: {this.GetType().Name} {value.GetType().Name}");
                }

                if (this.parent != null) // 之前有parent
                {
                    // parent相同，不设置
                    if (this.parent == value)
                    {
                        FrameworkLog.LogError($"重复设置了Parent: {this.GetType().Name} parent: {this.parent.GetType().Name}");
                        return;
                    }
                    this.parent.RemoveFromChildren(this);
                }
                
                this.parent = value;
                this.IsComponent = false;
                this.parent.AddToChildren(this);
                this.Domain = this.parent.domain;
            }
        }

        // 该方法只能在AddComponent中调用，其他人不允许调用
        private CoreEntity ComponentParent
        {
            set
            {
                if (value == null)
                {
                    throw new Exception($"cant set parent null: {this.GetType().Name}");
                }
                
                if (value == this)
                {
                    throw new Exception($"cant set parent self: {this.GetType().Name}");
                }
                
                // 严格限制parent必须要有domain,也就是说parent必须在数据树上面
                if (value.Domain == null)
                {
                    throw new Exception($"cant set parent because parent domain is null: {this.GetType().Name} {value.GetType().Name}");
                }
                
                if (this.parent != null) // 之前有parent
                {
                    // parent相同，不设置
                    if (this.parent == value)
                    {
                        FrameworkLog.LogError($"重复设置了Parent: {this.GetType().Name} parent: {this.parent.GetType().Name}");
                        return;
                    }
                    this.parent.RemoveFromComponents(this);
                }

                this.parent = value;
                this.IsComponent = true;
                this.parent.AddToComponents(this);
                this.Domain = this.parent.domain;
            }
        }

        public T GetParent<T>() where T : CoreEntity
        {
            return this.Parent as T;
        }
        
        public long Id
        {
            get;
            set;
        }
        
        public CoreEntity Domain
        {
            get
            {
                return this.domain;
            }
            private set
            {
                if (value == null)
                {
                    throw new Exception($"domain cant set null: {this.GetType().Name}");
                }
                
                if (this.domain == value)
                {
                    return;
                }
                
                CoreEntity preDomain = this.domain;
                this.domain = value;
                
                if (preDomain == null)
                {
                    this.InstanceId = IdGenerator.Instance.GenerateInstanceId();
                    this.IsRegister = true;
                }

                // 递归设置孩子的Domain
                if (this.children != null)
                {
                    foreach (CoreEntity entity in this.children.Values)
                    {
                        entity.Domain = this.domain;
                    }
                }

                if (this.components != null)
                {
                    foreach (CoreEntity component in this.components.Values)
                    {
                        component.Domain = this.domain;
                    }
                }
            }
        }
        
        private Dictionary<long, CoreEntity> children;
        
        public Dictionary<long, CoreEntity> Children
        {
            get
            {
                return this.children ??= UnityEngine.Pool.DictionaryPool<long, CoreEntity>.Get();
            }
        }

        private void AddToChildren(CoreEntity coreEntity)
        {
            this.Children.Add(coreEntity.Id, coreEntity);
        }

        private void RemoveFromChildren(CoreEntity coreEntity)
        {
            if (this.children == null)
            {
                return;
            }

            this.children.Remove(coreEntity.Id);

            if (this.children.Count == 0)
            {
                UnityEngine.Pool.DictionaryPool<long, CoreEntity>.Release(this.children);
                this.children = null;
            }
        }
        
        private Dictionary<Type, CoreEntity> components;
        
        public Dictionary<Type, CoreEntity> Components
        {
            get
            {
                return this.components ??= UnityEngine.Pool.DictionaryPool<Type, CoreEntity>.Get();
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.IsRegister = false;
            this.InstanceId = 0;

            // 清理Component
            if (this.components != null)
            {
                foreach (KeyValuePair<Type, CoreEntity> kv in this.components)
                {
                    kv.Value.Dispose();
                }

                this.components.Clear();
                UnityEngine.Pool.DictionaryPool<Type, CoreEntity>.Release(this.components);
                this.components = null;
            }

            // 清理Children
            if (this.children != null)
            {
                foreach (CoreEntity child in this.children.Values)
                {
                    child.Dispose();
                }

                this.children.Clear();
                UnityEngine.Pool.DictionaryPool<long,CoreEntity>.Release(this.children);
                this.children = null;
            }

            // 触发Destroy事件
            if (this is ICoreEntityDestroy destroyObj)
            {
                destroyObj.OnDestroy();
            }

            this.domain = null;

            if (this.parent != null && !this.parent.IsDisposed)
            {
                if (this.IsComponent)
                {
                    this.parent.RemoveComponent(this);
                }
                else
                {
                    this.parent.RemoveFromChildren(this);
                }
            }

            this.parent = null;

            base.Dispose();
            
            if (this.IsFromPool)
            {
                ReleaseInstance(this);
            }
            status = EntityStatus.None;
        }
        
        private void AddToComponents(CoreEntity component)
        {
            this.Components.Add(component.GetType(), component);
        }

        private void RemoveFromComponents(CoreEntity component)
        {
            if (this.components == null)
            {
                return;
            }

            this.components.Remove(component.GetType());

            if (this.components.Count == 0)
            {
                DictionaryPool<Type, CoreEntity>.Release(this.components);
                this.components = null;
            }
        }

        public K GetChild<K>(long id) where K: CoreEntity
        {
            if (this.children == null)
            {
                return null;
            }
            this.children.TryGetValue(id, out CoreEntity child);
            return child as K;
        }
        
        public void RemoveChild(long id)
        {
            if (this.children == null)
            {
                return;
            }

            if (!this.children.TryGetValue(id, out CoreEntity child))
            {
                return;
            }
            
            this.children.Remove(id);
            child.Dispose();
        }

        public void RemoveComponent<K>() where K : CoreEntity
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.components == null)
            {
                return;
            }

            Type type = typeof (K);
            CoreEntity c = this.GetComponent(type);
            if (c == null)
            {
                return;
            }

            this.RemoveFromComponents(c);
            c.Dispose();
        }

        public void RemoveComponent(CoreEntity component)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.components == null)
            {
                return;
            }

            CoreEntity c = this.GetComponent(component.GetType());
            if (c == null)
            {
                return;
            }

            if (c.InstanceId != component.InstanceId)
            {
                return;
            }

            this.RemoveFromComponents(c);
            c.Dispose();
        }

        public void RemoveComponent(Type type)
        {
            if (this.IsDisposed)
            {
                return;
            }

            CoreEntity c = this.GetComponent(type);
            if (c == null)
            {
                return;
            }

            RemoveFromComponents(c);
            c.Dispose();
        }

        public K GetComponent<K>() where K : CoreEntity
        {
            if (this.components == null)
            {
                return null;
            }

            CoreEntity component;
            if (!this.components.TryGetValue(typeof (K), out component))
            {
                return default;
            }

            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is ICoreEntityGetComponent getComObj)
            {
                getComObj.OnGetComponent();
            }

            return (K) component;
        }

        public CoreEntity GetComponent(Type type)
        {
            if (this.components == null)
            {
                return null;
            }

            CoreEntity component;
            if (!this.components.TryGetValue(type, out component))
            {
                return null;
            }
            
            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is ICoreEntityGetComponent getComObj)
            {
                getComObj.OnGetComponent();
            }

            return component;
        }
        
        private static CoreEntity Create(Type type, bool isFromPool)
        {
            CoreEntity component;
            if (isFromPool)
            {
                component = (CoreEntity)Module.FrameBase.ObjectPool.Instance.Fetch(type);
            }
            else
            {
                component = Activator.CreateInstance(type) as CoreEntity;
            }
            component.IsFromPool = isFromPool;
            component.Id = 0;
            return component;
        }

        public virtual CoreEntity GetEntityInstance()
        {
            return CoreEntity.GetInstance();
        }
        
        public CoreEntity AddComponent(CoreEntity component)
        {
            Type type = component.GetType();
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            component.ComponentParent = this;

            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component;
        }

        public CoreEntity AddComponent(Type type, bool isFromPool = false)
        {
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            CoreEntity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;

            if (component is ICoreEntityAwake awakeObj)
            {
                awakeObj.OnAwake();
            }
          
            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component;
        }

        public K AddComponent<K>(bool isFromPool = false) where K : CoreEntity, ICoreEntityAwake, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            CoreEntity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            if (component is ICoreEntityAwake awakeObj)
            {
                awakeObj.OnAwake();
            }
          
            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component as K;
        }

        public K AddComponent<K, P1>(P1 p1, bool isFromPool = false) where K : CoreEntity, ICoreEntityAwake<P1>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            CoreEntity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            if (component is ICoreEntityAwake<P1> awakeObj)
            {
                awakeObj.OnAwake(p1);
            }
          
            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component as K;
        }

        public K AddComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = false) where K : CoreEntity, ICoreEntityAwake<P1, P2>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            CoreEntity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            if (component is ICoreEntityAwake<P1,P2> awakeObj)
            {
                awakeObj.OnAwake(p1,p2);
            }
          
            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component as K;
        }

        public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = false) where K : CoreEntity, ICoreEntityAwake<P1, P2, P3>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            CoreEntity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            if (component is ICoreEntityAwake<P1,P2,P3> awakeObj)
            {
                awakeObj.OnAwake(p1,p2,p3);
            }
          
            if (this is ICoreEntityAddComponent addComObj)
            {
                addComObj.OnAddComponent();
            }
            return component as K;
        }
        
        public CoreEntity AddChild(CoreEntity coreEntity)
        {
            coreEntity.Parent = this;
            return coreEntity;
        }

        public T AddChild<T>(bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = IdGenerator.Instance.GenerateId();
            component.Parent = this;

            if (component is ICoreEntityAwake awakeObj)
            {
                awakeObj.OnAwake();
            }
            return component;
        }

        public T AddChild<T, A>(A a, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = IdGenerator.Instance.GenerateId();
            component.Parent = this;

            if (component is ICoreEntityAwake<A> awakeObj)
            {
                awakeObj.OnAwake(a);
            }
            return component;
        }

        public T AddChild<T, A, B>(A a, B b, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A, B>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = IdGenerator.Instance.GenerateId();
            component.Parent = this;

            if (component is ICoreEntityAwake<A,B> awakeObj)
            {
                awakeObj.OnAwake(a,b);
            }
            return component;
        }

        public T AddChild<T, A, B, C>(A a, B b, C c, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A, B, C>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = IdGenerator.Instance.GenerateId();
            component.Parent = this;

            if (component is ICoreEntityAwake<A,B,C> awakeObj)
            {
                awakeObj.OnAwake(a,b,c);
            }
            return component;
        }

        public T AddChild<T, A, B, C, D>(A a, B b, C c, D d, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A, B, C, D>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = IdGenerator.Instance.GenerateId();
            component.Parent = this;

            if (component is ICoreEntityAwake<A,B,C,D> awakeObj)
            {
                awakeObj.OnAwake(a,b,c,d);
            }
            return component;
        }

        public T AddChildWithId<T>(long id, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake, new()
        {
            Type type = typeof (T);
            T component = CoreEntity.Create(type, isFromPool) as T;
            component.Id = id;
            component.Parent = this;
            
            if (component is ICoreEntityAwake awakeObj)
            {
                awakeObj.OnAwake();
            }

            return component;
        }

        public T AddChildWithId<T, A>(long id, A a, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            if (component is ICoreEntityAwake<A> awakeObj)
            {
                awakeObj.OnAwake(a);
            }
            return component;
        }

        public T AddChildWithId<T, A, B>(long id, A a, B b, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A, B>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            if (component is ICoreEntityAwake<A,B> awakeObj)
            {
                awakeObj.OnAwake(a,b);
            }
            return component;
        }

        public T AddChildWithId<T, A, B, C>(long id, A a, B b, C c, bool isFromPool = false) where T : CoreEntity, ICoreEntityAwake<A, B, C>
        {
            Type type = typeof (T);
            T component = (T) CoreEntity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            if (component is ICoreEntityAwake<A,B,C> awakeObj)
            {
                awakeObj.OnAwake(a,b,c);
            }
            return component;
        }
    }

    public partial class CoreEntity
    {
        private static bool _isPoolInit = false;
        
        protected CoreEntity(){}
        
        private static CoreEntity GetInstance()
        {
            if (!_isPoolInit)
            {
                CommonObjectPool<CoreEntity>.Init(CreateInstance);
                _isPoolInit = true;
            }
            return CommonObjectPool<CoreEntity>.Get();
        }

        private static void ReleaseInstance(CoreEntity loadInfo)
        {
            if (loadInfo == null)
            {
                return;
            }
            CommonObjectPool<CoreEntity>.Release(loadInfo);
        }
        private static CoreEntity CreateInstance()
        {
            return new CoreEntity();
        }
    }
}