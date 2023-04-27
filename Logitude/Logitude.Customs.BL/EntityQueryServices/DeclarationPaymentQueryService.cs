using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationPaymentQueryService
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, DeclarationPaymentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationPaymentKeys declarationPaymentKeys = entityKeys as DeclarationPaymentKeys;

            DeclarationPaymentMethodQueryService declarationPaymentMethodQueryService = new DeclarationPaymentMethodQueryService(context);
            entityPM.DeclarationPaymentMethods = declarationPaymentMethodQueryService.GetMulti(declarationPaymentKeys, false);


            DeclarationPaymentProtestQueryService declarationPaymentProtestQueryService = new DeclarationPaymentProtestQueryService(context);
            entityPM.DeclarationPaymentProtests = declarationPaymentProtestQueryService.GetMulti(declarationPaymentKeys, false);

            if (entityPM.DeclarationPaymentMethods.Count > 0)
            {
                entityPM.PaymentMethodLastLineNumber = entityPM.DeclarationPaymentMethods.Max(m => m.Line);
            }

            if (entityPM.DeclarationPaymentProtests.Count > 0)
            {
                entityPM.PaymentProtestLastLineNumber = entityPM.DeclarationPaymentProtests.Max(m => m.Line);
            }

            base.GetComposition(entityKeys, entityPM);
        }

        public int  GetAutomaticPayment(string declarationid,string direction)
        {
            int AutomaticPayment;
            if (direction == "E")
            {
                string entityName = "GetAutomaticPayment" + declarationid;

                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    AutomaticPayment = this.repository.GetAutomaticPayment(declarationid);

                    if (CacheManager.CacheWrapper.Get(AutomaticPayment.ToString()) == null)
                    {
                        if (AutomaticPayment.ToString() != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, AutomaticPayment.ToString(), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    AutomaticPayment = int.Parse((string)CacheManager.CacheWrapper.Get(entityName));

                }
            }
            else
            {
                AutomaticPayment= this.repository.GetAutomaticPayment(declarationid);
            }
            return AutomaticPayment;
        }
    }
}
