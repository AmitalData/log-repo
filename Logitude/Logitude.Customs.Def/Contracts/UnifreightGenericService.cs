
#if false
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.Contracts
{
    public abstract class UnifreightGenericService : UnifreightGatewayProxy, IUnifreightGenericService
    {
        private GenericResponseObj _GenericResponseObj = new GenericResponseObj();
        private CommunicationsParams _CommunicationsParams = new CommunicationsParams();
        private int _iTenanat;
        private string _contactEmail;

        public CommunicationsParams MyCommunicationsParams
        {
            get { return _CommunicationsParams; }
           
        }
        public GenericResponseObj MyGenericResponseObj
        {
            get { return _GenericResponseObj; }

        }
        protected virtual string GetLoggingObjectTableId(string objectTableName)
        {
            if (String.IsNullOrWhiteSpace(objectTableName)) return "";//not must 
            var objectTableRepository = new ObjectTableRepository(0); // ObjectTabelRepository tenant must be zero !!
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName,// "Customs.PhysicalCheck", 
                0, true);

            return objectTable.Id;
        }
        protected virtual int ResolvedTenant()
        {
            return this._iTenanat;
        }
        

        public UnifreightGenericService(string uniDescription, string uniVersion, bool uniProduction)
            : base(uniDescription, uniVersion, uniProduction)
        {
            _GenericResponseObj = new GenericResponseObj();
        }

        /// <summary>
        /// call from UnifreightGatewayService.ProccessGenericRequest
        /// can throw BusinessErrorException  == BusinessError Other TecinicalFailure
        /// 
        /// </summary>
        /// <param name="DataIn"></param>
        /// <param name="MoreParams"></param>
        /// <param name="MessageOut"></param>
        public abstract void ProccessGenericRequest(
                    string DataIn,
                    
                    ref string MoreParams,
                    out string MessageOut);



        public void SetTenant(int iTenanat)
        {
            _iTenanat = iTenanat;
        }

        public void SetIdentityName(string contactEmail)
        {
            _contactEmail = contactEmail;
        }
    }
    public interface IUnifreightGenericService
    {
        void ProccessGenericRequest(
                    string DataIn,
                    
                    ref string MoreParams,
                    out string MessageOut);
        GenericResponseObj MyGenericResponseObj { get; }
        CommunicationsParams MyCommunicationsParams { get; }
    }
}


#endif