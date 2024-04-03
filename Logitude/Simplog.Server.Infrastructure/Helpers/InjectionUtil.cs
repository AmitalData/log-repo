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
        private Func<IHtmlEditorHelper> _HtmlEditorHelper;
        private Func<IEntityUpdateReflectorService> _EntityUpdateReflectorService;
        private Func<IEntityGetReflectorService> entityGetReflectorService;
        private Func<ITreeFilterQueryService> treeFilterQueryService;

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

        public static Func<string, int, bool> GetRequiredFieldErrorsForCourierDeclarationIsValid { get; set; }

        public static void Init(
            Func<IAmitalRestrictOwnerService> CreateAmitalRestrictOwnerModelService,
            Func<int> getTenantFromToken,
            Action<string, string, int, string> checkContactFeature,
            Func<IByteCompressorUtil> iByteCompressorUtilProvider,
            I_IISManager myIISManager,
            Func<IHtmlEditorHelper> myIHtmlEditorHelper,
            Func<IEntityUpdateReflectorService> myEntityUpdateReflectorService,
            Func<IEntityGetReflectorService> myEntityGetReflectorService,
            Func<ITreeFilterQueryService> treeFilterQueryService
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
            _Instance._HtmlEditorHelper = myIHtmlEditorHelper;
            _Instance._IISManager = myIISManager;
            _Instance._EntityUpdateReflectorService = myEntityUpdateReflectorService;
            _Instance.entityGetReflectorService = myEntityGetReflectorService;
            _Instance.treeFilterQueryService = treeFilterQueryService;

            
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

        //public string SendEmailOutActivityForEntity(byte[] htmlData, byte[] textData, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string myEntityId, string customerId, string objectTableId, string attachments, string entityReference, string documentTypeCode, string eventTypeCode)

        //{
        //    string res= _HtmlEditorHelper().SendEmailOutActivityForEntity(htmlData, textData, tenant,
        //        toEmail, subject, cc, bcc, userId,
        //        myEntityId, customerId, objectTableId, attachments, entityReference, documentTypeCode, eventTypeCode);
        //    return res;
        //}

        public string SendHtmlDocument(byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference, string from, string replyTo)
        {
            string res = _HtmlEditorHelper().SendHtmlDocument(htmlData, internalDocumentId, externalDocumentId, tenant, toEmail, subject, cc, bcc, userId, entityId, objectTableId, attachments, entityReference, from, replyTo);
            return res;

        }

        public object GetEntityByObjectTableNameAndEntityId(string entityName, string entityId, int tenant)
        {
            return _HtmlEditorHelper().GetEntity(entityName, entityId, tenant);
        }

        public void UpdateEntity(object entityPM, string entityName, int tenant)
        {
             _EntityUpdateReflectorService().UpdateEntity(entityPM, entityName, tenant);
        }
        public void UpdateEntity(UpdateEntityArgs updateEntityArgs)
        {
            _EntityUpdateReflectorService().UpdateEntity(updateEntityArgs);
        }
        public object GetEntity(EntityGetReflector entityGetReflector)
        {
            return entityGetReflectorService().GetEntity(entityGetReflector);
        }


        public IQueryable<T> ApplyTreeFilter<T>(IQueryable<T> queryable, TreeFilterQueryArgs treeFilterQueryArgs)
        {
            return treeFilterQueryService().Apply<T>(queryable , treeFilterQueryArgs);

        }
    }


    public interface ITreeFilterQueryService
    {
        IQueryable<T> Apply<T>(IQueryable<T> queryable, TreeFilterQueryArgs treeFilterQueryArgs);
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

    public interface IHtmlEditorHelper
    {
        //string SendEmailOutActivityForEntity(byte[] htmlData, byte[] textData, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string myEntityId, string customerId, string objectTableId, string attachments, string entityReference, string documentTypeCode, string eventTypeCode);
        string SendHtmlDocument(byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference, string from, string replyTo);
        object GetEntity(string entityName, string entityId, int tenant);

    }
    public class UpdateEntityArgs
    {
        public object EntityPM { get; set; }
        public string EntityName { get; set; }
        public int Tenant { get; set; }
        public string LoggedUserEmail { get; set; }
    }

    public interface IEntityUpdateReflectorService
    {
        void UpdateEntity(object entityPM, string entityName, int tenant);
        void UpdateEntity(UpdateEntityArgs updateEntityArgs);
    }

    public interface IEntityGetReflectorService
    {
        object GetEntity(EntityGetReflector entityGetReflector);
    }
}


