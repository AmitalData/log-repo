using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class FullAccountingHelper
    {
        public void UpdateGLAccount(GLAccountData glaacount)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            CardRepository cardRepository = new CardRepository(glaacount.Tenant);
            Card card = cardRepository.GetSingleCard(glaacount.EntityId, glaacount.Tenant);
            if (card != null)
            {
                GLAccountPM gLAccountEntity = glAccountQuery.GetSingleGLAccountWithComposition(card.GLAccountId, glaacount.Tenant);
                if (gLAccountEntity != null)
                {
                    gLAccountEntity.EnglishName = glaacount.EnglishName;
                    gLAccountEntity.LocalName = glaacount.LocalName;
                    gLAccountEntity.ChangeSetOp = ChangeSetOperation.Update;
                    IGLAccountUpdateServiceExt glaccountUpdate = ContainerAccessor.Container.Resolve(typeof(IGLAccountUpdateServiceExt), "GLAccountUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountUpdateServiceExt;
                    glaccountUpdate.Update(gLAccountEntity);
                }
            }
        }

        public string CreateGLAccount(GLAccountPM account)
        {
            IGLAccountUpdateServiceExt glaccountCreate = ContainerAccessor.Container.Resolve(typeof(IGLAccountUpdateServiceExt), "GLAccountUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountUpdateServiceExt;
            glaccountCreate.Create(account);

            return account.Id;
        }
    }

    public class GLAccountData
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }

    }

}
