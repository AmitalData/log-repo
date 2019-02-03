using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class InjectionUtil
    {
        static InjectionUtil _Instance;

        readonly Func<IAmitalRestrictOwnerService> _CreateAmitalRestrictOwnerModelService;
        readonly Func<int> _GetTenantFromToken;
        private Action<string, string, int, string> _checkContactFeature;
        private Func<IByteCompressorUtil> _ByteCompressorUtilProvider;
        private I_IISManager _IISManager;

        private InjectionUtil(
            Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService,
            Func<int> getTenantFromToken,
            Action<string, string, int, string> checkContactFeature
            )
        {
            _CreateAmitalRestrictOwnerModelService = CreateAmitalRestrictOwnerModelService;
            _GetTenantFromToken = getTenantFromToken;
            _checkContactFeature = checkContactFeature;
        }
        public void CheckContactFeature(string objectTableName, string featureCode, int tenant, string overrideEmail)
        {
            this._checkContactFeature(objectTableName, featureCode, tenant, overrideEmail);
        }
        public int GetTenantFromToken()
        {
            if (_GetTenantFromToken == null)
            {
                throw new Exception("Please Init Method with a Reference to Func<int> _GetTenantFromToken");
            }
            var tenant = _GetTenantFromToken();
            return tenant;
        }

        public AmitalRestrictOwnerModel GetAmitalRestrictOwnerModel(bool getFromCache, int tenant = 1, string UnifreightUserId = null)
        {
            if (_CreateAmitalRestrictOwnerModelService == null)
            {
                throw new Exception("Please Init Method with a Reference to Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService");
            }
            var service = _CreateAmitalRestrictOwnerModelService();
            var res = service.GetAmitalRestrictOwnerModel(getFromCache, tenant, UnifreightUserId);
            return res;
        }


        public static InjectionUtil Instance
        {
            get
            {
                if (_Instance == null)
                {
                    throw new Exception("InjectionUtil Instance not Init (Please create it @ Global.asax Or ThreadInit )");
                }
                return _Instance;
            }

        }

        //public I_IISManager IISManager { get => _IISManager; private set => _IISManager = value; }

        public I_IISManager IISManager {
            get
            {
                return _IISManager;
            }
            set
            {
                _IISManager = value;
            }
        }

        public static void Init(
            Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService,
            Func<int> getTenantFromToken,
            Action<string, string, int, string> checkContactFeature,
            Func<IByteCompressorUtil> iByteCompressorUtilProvider,
            I_IISManager myIISManager
            )
        {
            if (_Instance != null)
            {
                return;
                throw new Exception(@"already created 
                    InjectionUtil Instance not Init (Please create it @ Global.asax Or ThreadInit )");
            }

            _Instance = new InjectionUtil(CreateAmitalRestrictOwnerModelService, getTenantFromToken, checkContactFeature);
            _Instance._ByteCompressorUtilProvider = iByteCompressorUtilProvider;
            _Instance._IISManager = myIISManager;

        }

        public string CompressText(string text)
        {
            string compressText = _ByteCompressorUtilProvider().CompressText(text);
            return compressText;
        }

        public string DeCompressText(string compressText)
        {
            string deCompressText = _ByteCompressorUtilProvider().DeCompressText(compressText);
            return deCompressText;
        }
    }

    public interface IByteCompressorUtil
    {
        byte[] Compress(byte[] buffer);
        string CompressText(string text);
        byte[] Decompress(byte[] gzBuffer);
        string DeCompressText(string compressedText);
    }
    public interface I_IISManager
    {
        void RecycleMe();
    }
}


