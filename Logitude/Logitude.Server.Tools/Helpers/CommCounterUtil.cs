using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class CommCounterUtil
    {
        readonly static string _basic;
        readonly static string _filler19;

        
        //[ThreadStatic]
        static int threadStaticCounter;

        static CommCounterUtil()
        {
            _basic = new StringBuilder()
                .Append(Process.GetCurrentProcess().Id)
                .Append(Environment.UserName)
                .Append(Environment.MachineName)
                .ToString();


            _filler19 = new string('0', 19);
            //G_TIME //UNIFREIGHT //SERVER ORACLE TIME
        }
        private static readonly object _SyncRoot = new object();

        public static string GetUnique30(DateTime? seed = null)
        {
            lock (_SyncRoot)
            {
                var now = (seed.HasValue ? seed.Value : DateTime.Now)
                    .ToUniversalTime()
                    //.ToUffset
                    //.G_TIME
                    //.ToString("u");
                    .ToString("o");
                var datetime19 = new string(now.ToList().Where(arg => arg >= '0' && arg <= '9').Where((c, i) => i < 19).ToArray());
                var unique = new StringBuilder().Append(_basic).Append(threadStaticCounter++).ToString();
                var uniqueHashCode = unique.GetHashCode().ToString().Replace('-', '9');
                var uniqe11 =
                    //string.Format("{0:11}", uniqueHashCode);
                    //(_filler19 + uniqueHashCode)
                   uniqueHashCode.PadLeft(11, '0');

                if (datetime19.Length != 19)
                {
                    throw new Exception("datetime19.Length != 19");
                }
                if (uniqe11.Length != 11)
                {
                    throw new Exception("uniqe11.Length != 11");
                }


                return datetime19 + uniqe11; 
            }
            
        }
        private string PrintO(DateTime now)
        {
            var s = now
                //.ToUniversalTime()
                .ToString("o");
            Debug.WriteLine(s);
            var f = new string(s.ToList().Where(arg => arg >= '0' && arg <= '9').ToArray());
            Debug.WriteLine(f);
            Debug.WriteLine(f.Length);
            return f;
        }

       
    }
}
