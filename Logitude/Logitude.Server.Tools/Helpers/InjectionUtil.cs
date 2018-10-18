//using Logitude.Server.Tools.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Logitude.Server.Tools.Helpers
//{
//    public class InjectionUtil
//    {
//        static InjectionUtil _Instance;

//        readonly Func<IAmitalRestrictOwnerService> _CreateAmitalRestrictOwnerModelService;
//        private InjectionUtil(Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService)
//        {
//            _CreateAmitalRestrictOwnerModelService = CreateAmitalRestrictOwnerModelService;
//        }


//        public AmitalRestrictOwnerModel GetAmitalRestrictOwnerModel(bool getFromCache, int tenant = 1, string UnifreightUserId = null)
//        {
//            if (_CreateAmitalRestrictOwnerModelService == null)
//            {
//                throw new Exception("Please Init Method with a Reference to Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService");
//            }
//            var service = _CreateAmitalRestrictOwnerModelService();
//            var res = service.GetAmitalRestrictOwnerModel(getFromCache, tenant, UnifreightUserId);
//            return res;
//        }


//        public static InjectionUtil Instance
//        {
//            get
//            {
//                if (_Instance == null)
//                {
//                    throw new Exception("InjectionUtil Instance not Init (Please create it @ Global.asax Or ThreadInit )");
//                }
//                return _Instance;
//            }

//        }
//        public static void Init(Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService)
//        {
//            if (_Instance != null)
//            {
//                throw new Exception(@"already created 
//                    InjectionUtil Instance not Init (Please create it @ Global.asax Or ThreadInit )");
//            }
//            _Instance = new InjectionUtil(CreateAmitalRestrictOwnerModelService);

//        }

//    }
//}
