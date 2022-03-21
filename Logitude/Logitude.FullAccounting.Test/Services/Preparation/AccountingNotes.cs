using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class AccountingNotes
    {

        public void Prepare()
        {
            FullAccountingData.GLAccountNoteID = GetID();
        }

        private string GetID()
        {
            AccountingNotePM note = CreateInstance();
            var AddedNote = APICaller.CallPost<AccountingNotePM>(note, Urls.AccountingNotesController, UserTenant.Token).Data;
            return AddedNote.Id;
        }

        private AccountingNotePM CreateInstance()
        {
            return new AccountingNotePM()
            {
                CardId = FullAccountingData.CustomerCardId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                Notes="notes",
                Tenant = UserTenant.Tenant,

            };
        }
    }
}
