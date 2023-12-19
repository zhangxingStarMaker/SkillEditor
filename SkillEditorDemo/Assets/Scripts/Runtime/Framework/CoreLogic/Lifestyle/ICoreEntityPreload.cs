using System;
using System.Collections.Generic;
using Moudule.ETTaskCore;
using Runtime.Framework.FrameUtility;
using UnityEngine;

namespace Module.FrameBase
{
    public enum PreloadOpportunity
    {
        None = 0,
        ChangeMainScene = 1,
        FirstEnterGame = 2,
    }
    
    public enum PreloadInfoType
    {
        Usual = 1, //通用类型，可以依赖其他类型，不能被其他类型依赖
        ChangeScene = 2, //切场景
    }
    
    public interface ICoreEntityPreload
    {
        void OnPreload(Action continueCallback,IPreloadComponent preloadComponent,PreloadOpportunity opportunity);
    }
    
    public interface IPreloadComponent
    {
        ETTask StartPreload(PreloadOpportunity opportunity);
        void ReleasePreload(PreloadOpportunity opportunity);
        void RegisterPreloadInfo(PreloadOpportunity opportunity, IPreloadInfo preloadInfo);
    }
    public interface IPreloadInfo
    {
        List<PreloadInfoType> DependPreloadInfoTypes { get; }
        PreloadInfoType PreloadType { get; }
        int Weight { get; }
        float Progress { get; }
        void ExecutePreload();
        void ExecuteUnPreload();
        void ReleaseToPool();
        void UpdatePreloadType(PreloadInfoType CurType, params PreloadInfoType[] dependPreloadInfoTypes);
    }
    public abstract class PreloadComponentBase:CoreEntity,IPreloadComponent
    {
        public async virtual ETTask StartPreload(PreloadOpportunity opportunity)
        {
            throw new NotImplementedException();
        }
        public abstract void ReleasePreload(PreloadOpportunity opportunity);
        public abstract void RegisterPreloadInfo(PreloadOpportunity opportunity, IPreloadInfo preloadInfo);
    }

    public struct PreloadQueueInfoArg:IExecuteQueueInfoArg
    {
        public IPreloadComponent PreloadCom;
        public PreloadOpportunity Opportunity;

        public PreloadQueueInfoArg(IPreloadComponent preloadCom, PreloadOpportunity opportunity)
        {
            PreloadCom = preloadCom;
            Opportunity = opportunity;
        }
    }
    
    public class ExecutePreloadQueueInfo:ExecuteQueueInfo
    {
        private ETTask _preloadTask = null;
        
        private void ContinuePreload()
        {
            _preloadTask.SetResult();
        }
        
        protected override async ETTask<bool> OnExecuteASync(CoreEntity tempCoreEntity, IExecuteQueueInfoArg executeQueueInfoArg)
        {
            if (_preloadTask != null)
            {
                FrameworkLog.LogError("OnExecuteASync error:_preloadTask != null!");
                return false;
            }
            if (executeQueueInfoArg is not PreloadQueueInfoArg preloadArg)
            {
                FrameworkLog.LogError("OnExecuteASync error:arg is not PreloadQueueInfoArg类型!");
                return false;
            }
            if (preloadArg.PreloadCom == null)
            {
                FrameworkLog.LogError("OnExecuteASync error:preloadArg.PreloadCom == null!");
                return false;
            }
            if (tempCoreEntity is ICoreEntityPreload obj)
            {
                _preloadTask = ETTask.Create(true);
                obj.OnPreload(ContinuePreload,preloadArg.PreloadCom,preloadArg.Opportunity);
                await _preloadTask;
                _preloadTask = null;
                return true;
            }
            return false;
        }
    }
}