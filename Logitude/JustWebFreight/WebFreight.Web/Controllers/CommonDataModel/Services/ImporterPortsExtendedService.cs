using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
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
using System;

namespace WebFreight.Web.Controllers.CommonDataModel.Services
{
    public class ImporterPortsExtendedService
    {
        private int tenant;
        public string correlationId;
        private ICommonDataContext commoncontext;
        private IWebFreightContext webFreightContext;
        private PortService portService;
        private PortPM importerPort = null;
        public ImporterPortsExtendedService(int tenant , string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;
            commoncontext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
            portService = new PortService(commoncontext, tenant);
        }

        public APIException GetPortAPIResult(PortAM portAM, bool isNew)
        {
            PortQuery portQuery = new PortQuery(portAM.Tenant);
            if (isNew)
            {
                importerPort = new PortPM();
            }
            else if (!string.IsNullOrEmpty(portAM.Code) && !string.IsNullOrEmpty(portAM.CountryCode))
            {
                importerPort = portQuery.GetSinglePortPMByCodeCountryCode(portAM.Code, portAM.CountryCode, portAM.Tenant);
            }
            
            APIException result = MapAndValidateFieldsIds(portAM);
            if(result == null)
                MapEntityAMToEntityPM(portAM);
            return result;
        }
        public void MapEntityAMToEntityPM(PortAM portAM)
        {
            importerPort.Tenant = portAM.Tenant;
            importerPort.Code = portAM.Code;
            importerPort.EnglishName = portAM.EnglishName;
            importerPort.LocalName = portAM.LocalName;
            importerPort.Notes = portAM.Notes;
            importerPort.InActive = portAM.InActive;
            importerPort.IsAir = portAM.IsAir;
            importerPort.IsOcean = portAM.IsOcean;
            importerPort.IsInland = portAM.IsInland;
            importerPort.CountryCode = portAM.CountryCode;
            importerPort.StateCode = portAM.StateCode;
            importerPort.PortTimeZoneCode = portAM.TimeZoneCode;
            importerPort.IsFromWorkerRole = true;
        }
        public APIException MapAndValidateFieldsIds(PortAM portAM)
        {
            APIException Responce = new APIException();
            if (portAM.CountryCode != null)
            {
                var countryId = GetCountryIdFromCountryCode(portAM.CountryCode, portAM.Tenant);
                if (!string.IsNullOrEmpty(countryId))
                {
                    importerPort.CountryId = countryId;
                }
                else
                {
                    return ThrowDataBaseValidationError(Responce, "CountryId");
                }
            }
            else
            {
                return ThrowPortAMValidationError(Responce, "CountryCode");
            }

            if (portAM.StateCode != null)
            {
                var stateId = GetStateIdFromStateCode(portAM.StateCode, portAM.Tenant);
                if (!string.IsNullOrEmpty(stateId))
                {
                    importerPort.StateId = stateId;
                }
                else
                {
                    return ThrowDataBaseValidationError(Responce, "StateId");
                }
            }

            if (portAM.TimeZoneCode != null)
            {
                var portTimeZone = GetPortTimeZoneFromPortTimeZoneCode(portAM.TimeZoneCode, portAM.Tenant);
                if (portTimeZone == null)
                {
                    return ThrowDataBaseValidationError(Responce, "PortTimeZoneCode");
                }
            }
            if (string.IsNullOrEmpty(portAM.IsAir.ToString()))
            {
                return ThrowPortAMValidationError(Responce, "IsAir");
            }
            if (string.IsNullOrEmpty(portAM.IsOcean.ToString()))
            {
                return ThrowPortAMValidationError(Responce, "IsOcean");
            }
            if (string.IsNullOrEmpty(portAM.IsInland.ToString()))
            {
                return ThrowPortAMValidationError(Responce, "IsInland");
            }
            return null;
        }

        private static APIException ThrowPortAMValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName +" field is required.";
            return Responce;
        }

        private static APIException ThrowDataBaseValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName + " field doesn't exist in the database, insert this entity before using it.";
            return Responce;
        }

        public void UpdateImporterPort()
        {
            portService.Update(importerPort);
        }
        public void CreateImporterPort()
        {
            portService.Create(importerPort);
        }
        public string GetCountryIdFromCountryCode(string countryCode, int tenant)
        {
            Country country = new CountryRepository(commoncontext).GetSingleCountryByCode(countryCode, tenant);
            if (country != null)
            {
                return country.Id;
            }
            return "";
        }
        public string GetStateIdFromStateCode(string stateCode, int tenant)
        {
            State state = new StateRepository(commoncontext).GetSingleStateByCode(stateCode, tenant);
            if (state != null)
            {
                return state.Id;
            }
            return "";
        }
        public PortTimeZone GetPortTimeZoneFromPortTimeZoneCode(string portTimeZoneCode, int tenant)
        {
            PortTimeZone portTimeZone = new PortTimeZoneRepository(commoncontext).GetSinglePortTimeZone(portTimeZoneCode);

            return portTimeZone;
        }

        public APILogsPM GetLogPM(string subject, string reference)
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            if (Log != null)
            {
                return MapLogPMFromPoco(Log);
            }
            APILogsPM LogPM = GetNewLog(subject, reference);
            new APILogsService(webFreightContext, tenant).Create(LogPM);
            return LogPM;
        }
        private static APILogsPM MapLogPMFromPoco(APILogs Log)
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
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId,
                QueueType = "Port",
                Subject = Log.Subject
            };
        }
        private APILogsPM GetNewLog(string subject, string reference)
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
                QueueMessageMoreDetailsId = correlationId,
                QueueType = "Port",
                Subject = subject,
                Refrence = reference,
            };
        }
    }
}