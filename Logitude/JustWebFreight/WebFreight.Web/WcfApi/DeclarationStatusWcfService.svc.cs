using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Activation;
using System.Transactions;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeclarationStatusWcfService : IDeclarationStatusWcfService
    {

        public Response Upsert(List<DeclarationStatusPM> entityPM, bool batch)
        {
            Response response = new Response();
            return response;
        }
        public Response BuildDeclarationStatusesList(int tenant, string customFileNo, List<DeclarationStatusPM> DeclarationStatusesList)
        {
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customs.DeclarationStatus", "UPDATE", tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    var incoming = (DeclarationStatusesList ?? new List<DeclarationStatusPM>())
                                               .OrderBy(e => e.StatusDate)
                                               .ToList();

                    ICustomContext objectContext = CustomContext.GetContext(tenant);


                    DeclarationRepository declarationRepository = new DeclarationRepository(objectContext);
                    DeclarationStatusRepository declarationStatusRepository = new DeclarationStatusRepository(objectContext);
                    DeclarationStatusQueryService declarationStatusQueryService = new DeclarationStatusQueryService(objectContext);
                    DeclarationStatusUpdateService declarationStatusUpdateService = new DeclarationStatusUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    StatusCodeRepository StatusCodeRepository = new StatusCodeRepository(objectContext);


                    Declaration entityPoco = declarationRepository.GetOriginalDeclarationByCustomFileNo(customFileNo, tenant);
                    if (entityPoco == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Declaration doesn't exist!";
                        return response;
                    }

                    var dbRows = declarationStatusRepository.GetByDeclarationIdAndTenant(tenant,entityPoco.Id);
                    var distinctCodes = incoming.Select(x => x.StatusID).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
                    var codeToId = new Dictionary<string, string>(StringComparer.Ordinal);
                    foreach (var code in distinctCodes)
                    {
                        var sc = StatusCodeRepository.GetSingleByCode(code, tenant);
                        if (sc != null) codeToId[code] = sc.Id;
                    }

                    foreach (var pm in incoming)
                    {
                        pm.Tenant = tenant;
                        pm.DeclarationId = entityPoco.Id;
                        pm.StatusUser = null;
                        if (!string.IsNullOrEmpty(pm.StatusID) && codeToId.TryGetValue(pm.StatusID, out var mapped))
                        {
                            pm.StatusID = mapped;
                        }
                    }

                    var dbByLine = dbRows.ToDictionary(
                r => r.LineNumber,
                r => r,
                comparer: EqualityComparer<int>.Default);

                    var incomingByLine = incoming.ToDictionary(
                        r => r.LineNumber,
                        r => r,
                        comparer: EqualityComparer<int>.Default);

                    bool Changed(DeclarationStatusPM inc, dynamic db)
                    {
                        if (!string.Equals(inc.StatusID, (string)db.StatusID, StringComparison.Ordinal)) return true;
                        if (inc.StatusDate != (DateTime)db.StatusDate) return true;
                        if (inc.UnfSequenceNumeric != (int)db.UnfSequenceNumeric) return true;
                        return false;
                    }

                    var toInsert = incoming.Where(x => !dbByLine.ContainsKey(x.LineNumber)).ToList();

                    var toUpdate = new List<DeclarationStatusPM>();
                    foreach (var kv in incomingByLine)
                    {
                        if (dbByLine.TryGetValue(kv.Key, out var db))
                        {
                            if (Changed(kv.Value, db))
                            {
                                var up = kv.Value;
                                up.ChangeSetOp = ChangeSetOperation.Update;
                                toUpdate.Add(up);
                            }
                        }
                    }
                    var toDelete = dbByLine.Keys
                                   .Where(dbLn => !incomingByLine.ContainsKey(dbLn))
                                   .Select(dbLn => new DeclarationStatusPM
                                   {
                                       Tenant = tenant,
                                       DeclarationId = entityPoco.Id,
                                       LineNumber = dbLn,
                                       ChangeSetOp = ChangeSetOperation.Delete
                                   })
                                   .ToList();

                    foreach (var ins in toInsert)
                    {
                        ins.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationStatusUpdateService.Update(ins, true);
                    }

                    foreach (var up in toUpdate)
                    {
                        declarationStatusUpdateService.Update(up, true);
                    }

                    foreach (var del in toDelete)
                    {
                        declarationStatusUpdateService.Update(del, true);
                    }

                    scope.Complete();
                    return response;
                }
            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null
                    ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message)
                    : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;

                return response; ;
            }


        }

    }
}
