
using Microsoft.Practices.Unity;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Validation;
using Logitude.Customs.BL.BL;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class UniCourierBatchUpdateDeclarationCourierStatusMasterChanged_MsgResponseService : ResponseServiceBase
        //<TResponseData, TCustomResponse, TRequestParams>
        <INF_MSG_GenericResponseData, DCAInUCBUpdateDeclarationCourierStatusMasterChangedResponse, GenericRequestParams>
    {

        private ICustomContext _context;

        public override void Update(DCAInUCBUpdateDeclarationCourierStatusMasterChangedResponse customResponse, GenericRequestParams requestParams)
        {
            var mess = new StringBuilder();
            _context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(_context);
            var myDeclarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var repo = new DeclarationCourierStatusRepository(_context);
            List<DeclarationCourierStatus> listPoco = new List<DeclarationCourierStatus>();
            if (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count > 0)
            {
                mess.AppendLine($"מפוצל כבר !!!");

                UpdateDeclarationCourierStatus(customResponse.ServerSplitDeclarationsList);
            }
            else
            {
                mess.AppendLine($"ראשי - מפצל");
                mess.AppendLine($"כל ההצהרות יפוצלו.....");
                if (customResponse.DeclarationsList != null && customResponse.DeclarationsList.Count > 0)
                {
                    listPoco = repo.GetDeclarationsByIds(customResponse.DeclarationsList, requestParams.Tenant);
                }
                
                if (listPoco.Count == 0)
                {
                    mess.AppendLine($"There ARE  NOT any Declarations");
                }
                else
                {
                    listPoco.Select(r=>r.DeclarationId).ToList().ChunkBy(100).ForEach(list100 =>
                    {
                        customResponse.ServerSplitDeclarationsList = list100;
                        customResponse.LoggingUserId = requestParams.LoggingUserId;
                        var CreateDCAInUCBUCBUDCSMC_MsgMessagingService = new CRSUtil();
                        CreateDCAInUCBUCBUDCSMC_MsgMessagingService
                        .CreateCRS_DCAIn<DCAInUCBUpdateDeclarationCourierStatusMasterChangedResponse>(customResponse, (requestParams as RequestParamsBase),out string list);
                    });
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBUpdateDeclarationCourierStatusMasterChangedResponse customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public void UpdateDeclarationCourierStatus(List<string> list)
        {
            foreach (var decId in list)
            {
                string prevVal = null;
                string currvVal = null;
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(decId, true, false);
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(_context);
                DeclarationPM currentDeclarationPM = declarationQueryService.GetSingle(decId, true, false);
                
                if (currentDeclarationCourierStatusPM != null)
                {
                    CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(currentDeclarationPM, decId, currentDeclarationPM.Tenant);
                    prevVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                    if (prevVal == "V")
                    {
                        currvVal = "R";
                    }
                    else
                    {
                        calculateDeclarationCourierStatus.CalcCourierManifestStatusCode(currentDeclarationCourierStatusPM);
                        currvVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                    }
                    if (prevVal != currvVal)
                    {
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), currentDeclarationPM.Tenant);
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        AppendLogLine("try to update declarationCourierStatus for DeclarationPM.Id: " + decId);
                        try
                        {
                            declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                        }
                        catch (DbEntityValidationException ex)
                        {
                            var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                            AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                            return;
                        }
                        catch (System.Exception e)
                        {
                            AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                            return;
                        }
                    }
                }
            }
        }

        private void AppendLogLine(string mess)
        {
            LogMessagingUtil.Instance.AppendLine(mess);
        }
    }
}
