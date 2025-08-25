using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
namespace Logitude.Accounting.BL.Utils
{
    public class IntegrityCheckBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public IntegrityCheckBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void CreateBatchAccountingIntegrityCheck(int tenant)
        {
            try
            {

                int year = DateTime.Now.Year;
                var repo = new GLAccountTotalByMonthRepository(0);
                var activeTenants = repo.GetActiveTenantPerYear(year);
                var myTenantRepository = new TenantRepository(0);
                var prodTenant = myTenantRepository.GetTenants()
                    .Where(r => r.IsTestTenant == false && (tenant == 0 || r.Id == tenant))
                    .ToList();
                int iCount = 0;
                foreach (int t in activeTenants)
                {
                    if (prodTenant.FirstOrDefault(r => r.Id == t) == null)
                    {
                        continue;
                    }
                    using (var scope = TransactionFactory.GetNewTransaction())
                    {
                        IAccountingContext MyContext = AccountingContext.GetContext(t);
                        AccountingIntegrityCheckUpdateService service = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), t);

                        var paramsObj = new AccountingIntegrityInParam()
                        {
                            Tenant = tenant,
                            FromMonthInclusive = new DateTime(year, 1, 1),
                            ToMonthInclusive = DateTime.Now,
                        };
                        string xmlString = LogitudeXmlSerializer.SerializeObjectToXmlElementString<AccountingIntegrityInParam>(paramsObj);

                        service.DelayQueueInMinutes = iCount * 2;
                        service.Update(new AccountingIntegrityCheckPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            Tenant = t,
                            CreateDateTimeUTC = DateTime.UtcNow,
                            FromMonthInclusive = new DateTime(year, 1, 1),
                            ToMonthInclusive = DateTime.Now,
                            StatusCode = "1",
                            SendEmailWhileError = true,
                            ParametersXML = xmlString,

                        }
                        , true);
                        scope.Complete();
                        iCount++;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

