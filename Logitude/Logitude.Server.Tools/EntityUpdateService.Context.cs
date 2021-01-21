using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public abstract partial class EntityUpdateService<TEntityPOCO, TEntityPM, TEntityParentPM>
    {




        //private void AddContext(TEntityPM myTEntityPM)
        protected virtual void AddContext(TEntityPM myTEntityPM)
        {
            if (EntityUpdateServiceContext.Current == null)
            {
                EntityUpdateServiceContext.Current = new CurrentDebug() { EntityUpdateService = this as object };
                ///EntityUpdateServiceContext.Current = this as object;
            }   
        }
        protected void AddExternalTrace(string Trace)
        {
            if (IsMainUpdateService())
            {
                EntityUpdateServiceContext.Current.AddExternalTrace(Trace);
            }
        }
        private void AddStepTrace(string stepName)
        {
            if (IsMainUpdateService())
            {
                EntityUpdateServiceContext.Current.AddStepTrace(stepName);
            }
        }
        public string GetDebugTrace()
        {
            if (IsMainUpdateService())
            {
                return EntityUpdateServiceContext.Current.GetDebugTrace();
            }
            return null;
        }

        private bool IsMainUpdateService()
        {
            return EntityUpdateServiceContext.Current != null && EntityUpdateServiceContext.Current.EntityUpdateService == this;
        }
        
        private void RemoveContext(TEntityPM myTEntityPM)
        {
            //if (EntityUpdateServiceContext.Current == this)
            if (EntityUpdateServiceContext.Current != null && EntityUpdateServiceContext.Current.EntityUpdateService == this)
            {
                EntityUpdateServiceContext.Current.EntityUpdateService = null;
                EntityUpdateServiceContext.Current = null;
            }
        }
        public void GetAncestorEntityUpdateService
            (out object AncestorEntityUpdateService)
        {
            AncestorEntityUpdateService = null;
            try
            {
                AncestorEntityUpdateService = EntityUpdateServiceContext.Current.EntityUpdateService;
            }
            catch (Exception)
            {

                //throw;
            }
            
        }
        public void GetAncestor
            (out object entityPOCO, out object entityPM, out object entityParentPM)
        
        {
            entityPOCO = null; entityPM = null; entityParentPM = null;
            try
            {
                var entityUpdateService = //myobject 
                EntityUpdateServiceContext.Current.EntityUpdateService as dynamic;
                entityPOCO = entityUpdateService.EntityPOCO;
                entityPM = entityUpdateService.EntityPM;
            }
            catch (Exception)
            {
                
                //throw;
            }
            
        }

    }
    class EntityUpdateServiceContext
    {
        //[ThreadStatic]
        //public static object Current = null;
        [ThreadStatic]
        public static CurrentDebug Current = null;
        
    }
    class CurrentDebug
    {
        public object EntityUpdateService = null;
        private StringBuilder _sb;
        private Stopwatch _sw;

        public CurrentDebug()
        {
            _sb = new StringBuilder();
            _sw = Stopwatch.StartNew();
        }
        

        internal void AddStepTrace(string stepName)
        {
            _sb.AppendLine(stepName + ":Took:" + _sw.ElapsedMilliseconds);
            _sw.Restart();
            
        }



        internal void AddExternalTrace(string Trace)
        {
            _sb.AppendLine("ExternalTrace:" + Trace);
        }

        internal string GetDebugTrace()
        {
            return _sb.ToString(); 
        }
    }
}
