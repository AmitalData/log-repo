using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;   
using System.Web;

namespace Logitude.Customs.BL.Infrastructure
{

    internal static class AutoMsgScope
    {
        private const string Slot = "AutoMsgDeclarationIdSet";

        public static void Stamp(string declarationId)
        {
            GetSet().Add(declarationId);
        }
        public static bool FirstTime(string declarationId)
        {
            var set = GetSet();
            if (set.Contains(declarationId)) return false;   
            set.Add(declarationId);
            return true;
        }
        public static void Unstamp(string declarationId) => GetSet().Remove(declarationId);
        private static HashSet<string> GetSet()
        {
            if (HttpContext.Current != null)
            {
                var items = HttpContext.Current.Items;
                if (!(items[Slot] is HashSet<string> hs))
                {
                    hs = new HashSet<string>();
                    items[Slot] = hs;
                }
                return hs;
            }

            // In a background thread / Windows service – use LogicalCallContext
            if (!(CallContext.LogicalGetData(Slot) is HashSet<string> ls))
            {
                ls = new HashSet<string>();
                CallContext.LogicalSetData(Slot, ls);
            }
            return ls;
        }
    }
}
