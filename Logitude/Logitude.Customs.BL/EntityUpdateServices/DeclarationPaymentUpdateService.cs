using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.Helpers;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPaymentUpdateService
    {
        protected override void UpdateComposition(DeclarationPaymentPM entityPM)
        {
            DeclarationPaymentMethodUpdateService declarationPaymentMethodUpdateService = new DeclarationPaymentMethodUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            declarationPaymentMethodUpdateService.UpdateMulti(entityPM.DeclarationPaymentMethods, entityPM.DeletedDeclarationPaymentMethods, entityPM, false);

            DeclarationPaymentProtestUpdateService declarationPaymentProtestUpdateService = new DeclarationPaymentProtestUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            declarationPaymentProtestUpdateService.UpdateMulti(entityPM.DeclarationPaymentProtests, entityPM.DeletedDeclarationPaymentProtests, entityPM, false);

            base.UpdateComposition(entityPM);
        }
        protected override void OnUpdating(DeclarationPaymentPM entityPM)
        {
            //CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting.IsConnectedToUniFreight)
            {
                var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                declarationQueryService.LoadSupplierInvoicesWithItems = false;
                var declaration = declarationQueryService.GetSingle(entityPM.DeclarationId, true, true);
                //UpdateUnifreight(entityPM, declaration);
                var unifreightDeclarationPaymentUpdateService = new UnifreightDeclarationPaymentUpdateService(entityPM, declaration);
                unifreightDeclarationPaymentUpdateService.Update();
            }
            base.OnUpdating(entityPM);
        }

        protected override void CheckConcurrency(DeclarationPaymentPM entityPM, DeclarationPayment entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }

        }


        protected override void AfterUpdating(DeclarationPaymentPM entityPM, Server.Tools.EntityPM entityParentPM)
        {

            bool methodAdded = entityPM.DeclarationPaymentMethods.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert).Any();
            bool methodDeleted = entityPM.DeletedDeclarationPaymentMethods.Any();


            //----- DeclarationPaymentMethod 
            bool isDeclarationPaymentMethodInsert = (from a in entityPM.DeclarationPaymentMethods
                                                     where a.ChangeSetOp == ChangeSetOperation.Insert
                                                     select a).Any();

            bool isDeclarationPaymentMethodDelete = (from a in entityPM.DeletedDeclarationPaymentMethods
                                                     select a).Any();
            bool theCountOf_DeclarationPaymentProtest_Changed =
                entityPM.DeletedDeclarationPaymentProtests.Any() ||
                entityPM.DeclarationPaymentProtests.Any(r => r.ChangeSetOp == ChangeSetOperation.Delete) ||
                entityPM.DeclarationPaymentProtests.Any(r => r.ChangeSetOp == ChangeSetOperation.Insert)
                ;
            bool theCountOf_DeclarationPaymentMethod_Changed = (isDeclarationPaymentMethodInsert || isDeclarationPaymentMethodDelete);
            if (theCountOf_DeclarationPaymentMethod_Changed || theCountOf_DeclarationPaymentProtest_Changed)
            {
                SubmitChanges();
            }
            if (theCountOf_DeclarationPaymentProtest_Changed)
            {
                bool fake_until_you_make_it = false;
                if (fake_until_you_make_it)
                {
                    (new Declaration()).IsPaymentProtested = true;//HOW IS CHANGING IsPaymentProtested>LOOK DOWN
                }
                //55160	עדכון סימון הצהרה כהוגשה אגב מחאה
                //please do not set the ischanged>DUE THAT IS SP 
                var repoFast = new DeclarationPaymentProtestRepository(entityPM.Tenant);
                bool anyDeclarationPaymentProtest = repoFast.AnyDeclarationPaymentProtest(entityPM.DeclarationId, entityPM.Tenant);
                CustomsStoredProcedures.Declaration_SetIsPaymentProtested(entityPM.DeclarationId, entityPM.Tenant, anyDeclarationPaymentProtest);

            }
            //if (isDeclarationPaymentMethodInsert || isDeclarationPaymentMethodDelete)

            if (theCountOf_DeclarationPaymentMethod_Changed)
            {
                ///SubmitChanges();//moveup 
                ICustomContext context = MainContext as CustomContext;
                DeclarationPaymentMethodRepository declarationPaymentMethodRepository = new DeclarationPaymentMethodRepository(context);
                List<DeclarationPaymentMethod> declarationPaymentMethods = declarationPaymentMethodRepository.GetMulti(new DeclarationPaymentKeys() { DeclarationId = entityPM.DeclarationId });

                int index = 0;
                bool dirty = false;
                foreach (DeclarationPaymentMethod item in declarationPaymentMethods)
                {
                    index += 1;
                    if (item.SequenceNumeric == index) continue;
                    dirty = true;
                    item.SequenceNumeric = index;
                    declarationPaymentMethodRepository.Update(item);
                    DeclarationPaymentMethodPM itemPM = (from a in entityPM.DeclarationPaymentMethods
                                                         where a.DeclarationId == item.DeclarationId && a.Line == item.Line
                                                         select a).FirstOrDefault();
                    itemPM.SequenceNumeric = item.SequenceNumeric;
                }
                if (dirty)
                {
                    declarationPaymentMethodRepository.SubmitChanges();
                }
            }



        }

    }
}
