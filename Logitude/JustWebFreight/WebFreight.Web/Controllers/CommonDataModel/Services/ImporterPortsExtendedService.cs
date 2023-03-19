using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Transactions;

namespace WebFreight.Web.Controllers.CommonDataModel.Services
{
    public class ImporterPortsExtendedService
    {
        private int tenant;
        public string correlationId;
        ICommonDataContext commoncontext;
        IWebFreightContext webFreightContext;
        public ImporterPortsExtendedService(int tenant , string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;
            commoncontext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public APIException MapEntityAMToEntityPM(PortAM entityAM, PortPM entityPM)
        {
            APIException Responce = new APIException();
            //TenantPM currentTenant = TenantQuery.GetSingleTenantPM(entityAM.Tenant, false);
            //ICommonDataContext commoncontext = CommonDataContext.GetContext(entityAM.Tenant);
            //HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
            //HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
            //HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(entityAM.Tenant);
            //TenantManagmentPrivateLabelsPM privatelabel = null;
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
            //    privatelabel = query.GetSinglePM(currentTenant.PrivateLabelId);
            //}
            entityPM.Tenant = entityAM.Tenant;
            entityPM.StateCode = entityAM.StateCode;
            entityPM.PortTimeZoneCode = entityAM.PortTimeZoneCode;
            entityPM.LocalName = entityAM.LocalName;
            entityPM.EnglishName = entityAM.EnglishName;

            if (entityAM.CountryCode != null)
            {
                var countryId = GetCountryIdFromCountryCode(entityAM.CountryCode, entityAM.Tenant);
                if (!string.IsNullOrEmpty(countryId))
                {
                    entityPM.CountryId = countryId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "CountryId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "CountryCode field is required.";
                return Responce;
            }

            if (entityAM.StateCode != null)
            {
                var stateId = GetStateIdFromStateCode(entityAM.StateCode, entityAM.Tenant);
                if (!string.IsNullOrEmpty(stateId))
                {
                    entityPM.StateId = stateId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "StateId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "StateCode field is required.";
                return Responce;
            }
            if (entityAM.PortTimeZoneCode != null)
            {
                var portTimeZoneCode = GetPortTimeZoneFromPortTimeZoneCode(entityAM.PortTimeZoneCode, entityAM.Tenant);
                if (!string.IsNullOrEmpty(portTimeZoneCode))
                {
                    entityPM.PortTimeZoneCode = portTimeZoneCode;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "PortTimeZoneCode field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "PortTimeZoneCode field is required.";
                return Responce;
            }
            return null;
        }
        public string GetCountryIdFromCountryCode(string countryCode, int tenant)
        {
            CountryRepository countryRepository = new CountryRepository(commoncontext);
            Country country = countryRepository.GetSingleCountryByCode(countryCode, tenant);
            if (country != null)
            {
                return country.Id;
            }
            return "";
        }
        public string GetStateIdFromStateCode(string stateCode, int tenant)
        {
            StateRepository stateRepository = new StateRepository(commoncontext);
            State state = stateRepository.GetSingleStateByCode(stateCode, tenant);
            if (state != null)
            {
                return state.Id;
            }
            return "";
        }
        public string GetPortTimeZoneFromPortTimeZoneCode(string portTimeZoneCode, int tenant)
        {
            PortTimeZoneRepository portTimeZoneRepository = new PortTimeZoneRepository(commoncontext);
            PortTimeZone portTimeZone = portTimeZoneRepository.GetSinglePortTimeZone(portTimeZoneCode);
            if (portTimeZone != null)
            {
                return portTimeZone.Code;
            }
            return "";
        }

        public APILogsPM GetLogPM()
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogsService apiLogsService = new APILogsService(webFreightContext, tenant);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            APILogsPM LogPM;
            bool IsNewLog = false;
            if (Log == null)
            {
                IsNewLog = true;
                LogPM = CreateNewLog();
            }

            IsNewLog = false;
            LogPM = UpdateLog(Log);
            if (IsNewLog)
            {
                //LogPM.QueueMessage = DictionaryJsonConverter.FromDictionaryToJson((Dictionary<string, string>)response.MessageValues);
                LogPM.QueueType = "Port";
                apiLogsService.Create(LogPM);
            }
            return LogPM;
        }

        private static APILogsPM UpdateLog(APILogs Log)
        {
            return new APILogsPM()
            {
                Id = Log.Id,
                CorrelationId = Log.CorrelationId,
                CreateDate = Log.CreateDate,
                CreateDateUTC = Log.CreateDateUTC,
                Direction = Log.Direction,
                EntityId = Log.EntityId,
                LastUpdateDate = Log.LastUpdateDate,
                LastUpdateDateUTC = Log.LastUpdateDateUTC,
                NumberOfRetries = Log.NumberOfRetries++,
                ObjectTableId = Log.ObjectTableId,
                ExpirationDate = Log.ExpirationDate,
                Refrence = Log.Refrence,
                Status = "I",
                Tenant = Log.Tenant,
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId
            };
        }

        private APILogsPM CreateNewLog()
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = correlationId
            };
        }
    }
}