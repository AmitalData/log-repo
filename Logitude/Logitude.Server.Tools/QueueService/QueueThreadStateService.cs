using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.QueueService
{
    public static class QueueThreadStateService
    {

        private static readonly ConcurrentDictionary<string, string> _ConcurrentQueueState ;
        static QueueThreadStateService()
        {
            _ConcurrentQueueState= new ConcurrentDictionary<string, string>();
        }

        public static void Upsert(string queueThreadCode ,string queueThreadState)
        {
            try
            {
                var state = $"{DateTime.Now.ToString()}:{queueThreadState}";
                _ConcurrentQueueState.AddOrUpdate(queueThreadCode, state
                     , 
                    (keyToUpdate, existingValue) =>
                    {
                        return state;
                    });

            }
            catch (Exception)
            {

                //throw;
            }

        }
        public static string GetState()
        {
            var sb= new StringBuilder();
            try
            {
                var keys = _ConcurrentQueueState.Keys.ToList();
                foreach (var key in keys)
                {
                    var value = _ConcurrentQueueState[key];
                    sb
                        //.Append(JsonConvert.SerializeObject(key, Formatting.None, new JsonSerializerSettings { }))
                        .Append(key.ToString())
                        .Append(":")
                        .AppendLine(value);
                }

            }
            catch (Exception e)
            {

                sb.Append("QueueSThreadStateService:ERRRR:").AppendLine(e.Message);
            }
            return sb.ToString();
        }
    }
    public class QueueThreadKey
    {

        public QueueThreadKey(string workerClass, string currentThreadName) 
        {

            WorkerClass = workerClass;
            CurrentThreadName = currentThreadName;
        }
        public string WorkerClass { get; private set; }
        public string CurrentThreadName { get; private set; }

        public override string ToString()
        {
            return $"WRClass{WorkerClass},ThreadName:{CurrentThreadName}";
        }
    }
}
