using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    
    public class LogtitudeDomainScope : IDisposable
    {

        [ThreadStatic]
        private static Dictionary<string, ILogitudeContextEntity> _Context = new Dictionary<string, ILogitudeContextEntity>();
        
        private readonly bool _IsParentContext;
        private readonly string _myEnum;

        public LogtitudeDomainScope(ILogitudeContextEntity myLogitudeContextEntity = null)
        {
            if (myLogitudeContextEntity == null)
            {
                throw new Exception("myLogitudeContextEntity is must");
            }
            var myType=myLogitudeContextEntity.GetType().Name;
            _myEnum = myType;
            if (Context.ContainsKey(myType))
            {
                throw new InvalidOperationException("Only a single Domain Context can be created!");
            }
            
            
            //myLogtitudeDomainScope.MyEnum = myType;
            _IsParentContext = true;
            Context.Add(myType, myLogitudeContextEntity);
        }
        public static TLogitudeContextEntity GetCurrent<TLogitudeContextEntity>()
            where TLogitudeContextEntity : class ,ILogitudeContextEntity
        {
            var myType = typeof(TLogitudeContextEntity).Name;
            return GetCurrent(myType) as TLogitudeContextEntity;

        }
        private static ILogitudeContextEntity GetCurrent(string my_type)
        {
            ILogitudeContextEntity myILogitudeContextEntity = null;
            try
            {
                if (Context == null) return null;
                if (!Context.ContainsKey(my_type)) return null;
                myILogitudeContextEntity= Context[my_type];
            }
            catch 
            {
                
                
            }
            return myILogitudeContextEntity;
            
        }
        public ILogitudeContextEntity Current
        {
            get { return LogtitudeDomainScope.GetCurrent(_myEnum); }
        }

        public static Dictionary<string, ILogitudeContextEntity> Context
        {
            get
            {
                return _Context = _Context ?? new Dictionary<string, ILogitudeContextEntity>();
            }

            set
            {
                _Context = value;
            }
        }

        public void Dispose()
        {
            if (!_IsParentContext) return;
            Context.Remove(_myEnum);
        }
    }
    
    public interface ILogitudeContextEntity
    {
        
    }
    public class ExceptionInErrorLog : Exception, ILogitudeContextEntity
    {
        public string ErrorlogId { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {

            return String.Format(
@"An unhandled exception has been caught (our ref :{0})  
{1}", ErrorlogId, Message);
        }
    }
}
