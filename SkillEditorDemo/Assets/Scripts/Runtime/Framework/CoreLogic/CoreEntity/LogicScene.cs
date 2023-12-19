using System.Diagnostics;
using Runtime.Framework.FrameUtility;

namespace Module.FrameBase
{
    [DebuggerDisplay("ViewName,nq")]
    public sealed class LogicScene: CoreEntity
    {
        public int Zone
        {
            get;
        }

        public SceneType SceneType
        {
            get;
        }

        public string Name
        {
            get;
        }

        public LogicScene(long instanceId, int zone, SceneType sceneType, string name, CoreEntity parent)
        {
            this.Id = instanceId;
            this.InstanceId = instanceId;
            this.Zone = zone;
            this.SceneType = sceneType;
            this.Name = name;
            
            this.Parent = parent;
            this.Domain = this;
            this.IsRegister = true;
            FrameworkLog.LogInfo($"scene create: {this.SceneType} {this.Name} {this.Id} {this.InstanceId} {this.Zone}");
        }

        public LogicScene(long id, long instanceId, int zone, SceneType sceneType, string name, CoreEntity parent)
        {
            this.Id = id;
            this.InstanceId = instanceId;
            this.Zone = zone;
            this.SceneType = sceneType;
            this.Name = name;
            this.Parent = parent;
            this.Domain = this;
            this.IsRegister = true;
            FrameworkLog.LogInfo($"scene create: {this.SceneType} {this.Name} {this.Id} {this.InstanceId} {this.Zone}");
        }

        public override void Dispose()
        {
            base.Dispose();
            
            FrameworkLog.LogInfo($"scene dispose: {this.SceneType} {this.Name} {this.Id} {this.InstanceId} {this.Zone}");
        }

        public new CoreEntity Domain
        {
            get => this.domain;
            private set => this.domain = value;
        }

        public new CoreEntity Parent
        {
            get
            {
                return this.parent;
            }
            private set
            {
                if (value == null)
                {
                    //this.parent = this;
                    return;
                }

                this.parent = value;
                this.parent.Children.Add(this.Id, this);
            }
        }
        
        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().Name} ({this.SceneType})";    
            }
        }
    }
}