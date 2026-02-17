using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class GLAccountTotalByMonthUpdateService : EntityUpdateService<GLAccountTotalByMonth, GLAccountTotalByMonthPM, EntityPM>
    {
        protected override void OnCreating(GLAccountTotalByMonthPM entitypm, EntityPM entityParentPM)
        {
            //entitypm.id = idcounter.getnumber("accounting.glaccounttotalbymonth", entitypm.tenant);
        }

        public void UpsertDelta(List<GLAccountTotalByMonthPM> deltaList)
        {
            try
            {


                var defaultRecord = deltaList.FirstOrDefault();
                if (deltaList.FirstOrDefault(rec => !rec.Tenant.Equals(defaultRecord.Tenant)) != null)
                {
                    throw new Exception("in 1 tenent only ");
                }
                var inshureNoDuplicateKeys_MayBeCrash = deltaList.ToDictionary(rec => string.Concat(rec.AccountId,rec.DateTypeCode, rec.Year, rec.Month, rec.CurrencyId));
                var orderDeltaList = deltaList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.DateTypeCode).ThenBy(rec => rec.Year).ThenBy(rec => rec.Month).ThenBy(rec => rec.CurrencyId);

                var myGLAccountTotalByMonthsQueryServices = new GLAccountTotalByMonthQueryService(defaultRecord.Tenant);
                var myGLAccountTotalByMonthsUpdateServices = new GLAccountTotalByMonthUpdateService(MainContext, new Dictionary<string, IContext>(), defaultRecord.Tenant);


                ///better/faster multi But no insert   myGLAccountTotalByMonthsUpdateServices.UpdateMulti(orderDeltaList, true);
                foreach (var entityPM in orderDeltaList)
                {


                    var pm = myGLAccountTotalByMonthsQueryServices.GetSingle(entityPM.AccountId, entityPM.DateTypeCode, entityPM.Year, entityPM.Month, entityPM.CurrencyId, false, false);
                    if (pm != null)
                    {
                        pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        pm.ForeignAmountCredit += entityPM.ForeignAmountCredit;
                        pm.ForeignAmountDebit += entityPM.ForeignAmountDebit;
                        pm.LocalAmountCredit += entityPM.LocalAmountCredit;
                        pm.LocalAmountDebit += entityPM.LocalAmountDebit;


                    }
                    else
                    {

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        pm = entityPM;

                    }
                    myGLAccountTotalByMonthsUpdateServices.Update(pm, true);

                }
                //do the same like update  ~~~ myGLAccountTotalByMonthsUpdateServices.UpdateMulti( = 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
