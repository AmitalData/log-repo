using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using Logitude.CustomsMessaging;


namespace Logitude.Customs.BL.BL
{
    public class ExportDeclarationsBatchActionsService
    {
        private readonly ICustomContext _ctx;

        public ExportDeclarationsBatchActionsService(ICustomContext ctx)
        {
            _ctx = ctx;
        }

        public string RunCheckStatus(int tenant, IEnumerable<string> declarationIds, out string requestList)
        {
            int ok = 0, fail = 0;
            requestList = string.Empty;

            var ids = (declarationIds ?? Enumerable.Empty<string>())
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .Distinct()
                      .ToList();

            var declQuery = new DeclarationQueryService(tenant);
            var declTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var loggingUserId = AuthenticationUtil.ResolveUserId(tenant);

            foreach (var id in ids)
            {
                try
                {
                    var decl = declQuery.GetSingle(id, true, false);

                    if (decl == null)
                    {
                        fail++;
                        continue;
                    }

                    var p = new DeclarationStatusRequestParams
                    {
                        LoggingEnabled = true,
                        CustomFileNo = decl.CustomFileNo,
                        DeclarationNumber = decl.DeclarationNumber,
                        Tenant = tenant,
                        RequestName = "Declaration Status " + decl.DeclarationNumber,
                        ResponseName = "Declaration Status " + decl.DeclarationNumber,
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                        InterfaceTypeCode = "8250",
                        LoggingEntityId = decl.Id,
                        LoggingObjectTableId = declTableId,
                        LoggingUserId = loggingUserId,
                        SuppressSplitWR = true
                    };

                    SBQMessageService.CreateSheetSBQMessage<DeclarationStatusRequestParams>(p, false);
                    ok++;
                }
                catch (CustomsRequestsSheetDomainModelServiceException ex)
                {
                    if (ex.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                        ok++;
                    else
                        fail++;
                }
                catch
                {
                    fail++;
                }
            }

            requestList = fail.ToString();
            return ok.ToString();
        }

        public string RunOperationalClose(int tenant, IEnumerable<string> declarationIds, out string requestList)
        {
            int ok = 0, fail = 0;
            requestList = string.Empty;

            var ids = (declarationIds ?? Enumerable.Empty<string>())
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .Distinct();

            var declQuery = new DeclarationQueryService(tenant);

            foreach (var id in ids)
            {
                try
                {
                    var decl = declQuery.GetSingle(id, true, false);
                    if (decl == null)
                    {
                        fail++;
                        continue;
                    }

                    DeclarationUpdateService.DeclarationClosure(id, tenant);
                    ok++;
                }
                catch
                {
                    fail++;
                }
            }

            requestList = fail.ToString();
            return ok.ToString();
        }
        public IList<DeclarationRestoreRequestParams> BuildDeclarationRestoreRequests(
                    int tenant,
                    IEnumerable<string> declarationIds,
                    out string requestList)
        {
            int fail = 0;
            requestList = string.Empty;

            var ids = (declarationIds ?? Enumerable.Empty<string>())
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .Distinct()
                      .ToList();

            var loggingUserId = AuthenticationUtil.ResolveUserId(tenant);
            var declQuery = new DeclarationQueryService(tenant);

            var result = new List<DeclarationRestoreRequestParams>();

            foreach (var id in ids)
            {
                try
                {
                    var decl = declQuery.GetSingle(id, true, false);
                    if (decl == null)
                    {
                        fail++;
                        continue;
                    }

                    var request = BuildDeclarationRestoreRequestParams(tenant, loggingUserId, decl);
                    result.Add(request);
                }
                catch
                {
                    fail++;
                }
            }

            requestList = fail.ToString();
            return result;
        }
        private DeclarationRestoreRequestParams BuildDeclarationRestoreRequestParams(
           int tenant,
           string loggingUserId,
           DeclarationPM decl)
        {
            return new DeclarationRestoreRequestParams
            {
                LoggingEnabled = true,
                LoggingUserId = loggingUserId,
                Tenant = tenant,
                LoggingEntityReference = decl.Direction,
                AppicationId = decl.Id,
                DeclarationId = decl.Id,
                DeclarationNumber = decl.DeclarationNumber,
                CustomsFile = decl.CustomFileNo,
                RequestVIA = SendRequestVIA.WebServiceInteractive,
                ResponseName = "9079",
                RequestName = "Declaration Restore From BatchAction"
            };
        }


    }

}
