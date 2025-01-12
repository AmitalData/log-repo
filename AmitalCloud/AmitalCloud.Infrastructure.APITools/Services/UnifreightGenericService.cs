using AmitalCloud.Infrastructure.APITools.DataContracts;
using AmitalCloud.Infrastructure.APITools.Interfaces;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using System;
namespace AmitalCloud.Infrastructure.APITools.Services
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

}
