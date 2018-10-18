using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.FaultProceduralDetailsServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_NG_Web8332_FaultProceduralRequestService
        : RequestServiceBase<DF_NG_Web8332_FaultProceduralParam, FaultProceduralRequestParams>
    {
        public override DF_NG_Web8332_FaultProceduralParam GetRequest(FaultProceduralRequestParams requestParams)
        {
            var myDF_NG_Web8332_FaultProceduralParam = new DF_NG_Web8332_FaultProceduralParam();

            myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails = new DF_NG_Web8332_FaultProceduralParamFaultQueryDetails()
            {
                //ImporterExternalID_Exporter = importerExternalID,
                ImporterExternalID_ExporterSpecified = false,
                DeclarationID = requestParams.DeclarationNumber,
                //ProceduralFaultCode = proceduralFaultCode,
                ProceduralFaultCodeSpecified = false,
                //StartDate = requestParams.StartDate,
                //EndDate = requestParams.EndDate,
                //AgentExternalID = agentExternalID,
                AgentExternalIDSpecified = false
            };


            int importerExternalID;
            if (!string.IsNullOrWhiteSpace(requestParams.ImporterExternalID))
            {
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.ImporterExternalID_ExporterSpecified = true;
                if (!int.TryParse(requestParams.ImporterExternalID, out importerExternalID))
                {
                    return null;
                }
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.ImporterExternalID_Exporter = importerExternalID;
            }

            int proceduralFaultCode;
            if (!string.IsNullOrWhiteSpace(requestParams.ProceduralFaultCode))
            {
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.ProceduralFaultCodeSpecified = true;
                if (!int.TryParse(requestParams.ProceduralFaultCode, out proceduralFaultCode))
                {
                    return null;
                }
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.ProceduralFaultCode = proceduralFaultCode;
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.ProceduralFaultCodeSpecified = true;
            }

            int agentExternalID;
            if (!string.IsNullOrWhiteSpace(requestParams.AgentExternalID))
            {
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.AgentExternalIDSpecified = true;
                if (!int.TryParse(requestParams.AgentExternalID, out agentExternalID))
                {
                    return null;
                }
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.AgentExternalID = agentExternalID;
            }

            DateTime startDate;
            if(requestParams.StartDate.HasValue)
            {
                if (!DateTime.TryParse(requestParams.StartDate.ToString(), out startDate))
                {
                    return null;
                }
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.StartDate = startDate;
            }

            DateTime endDate;
            if (requestParams.EndDate.HasValue)
            {
                if (!DateTime.TryParse(requestParams.EndDate.ToString(), out endDate))
                {
                    return null;
                }
                myDF_NG_Web8332_FaultProceduralParam.FaultQueryDetails.EndDate = endDate;
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            this.MyRequestSheetParam.RequestDescription = "שאילתא לליקויים";

            return myDF_NG_Web8332_FaultProceduralParam;

        }
    }
}
