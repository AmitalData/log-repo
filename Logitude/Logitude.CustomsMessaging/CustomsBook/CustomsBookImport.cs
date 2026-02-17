using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.CustomsMessaging.CustomsBook
{
    public class CustomsBookImport
    {
        public void Upsert(CustomsBookInRequestParams requestParams, int tenant)
        {
            TransactionScope scope = null;
            try
            {
                var logContext = CustomContext.GetContext(tenant);
                var customsBookQueryService = new CustomsBookQueryService(logContext);
                var customsBookUpdateService = new CustomsBookUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                //CustomsBookPM dbCustomsBookPM = customsBookQueryService.GetCustomsBookByTenant(tenant);
                CustomsBookPM dbCustomsBookPM = customsBookQueryService.GetCustomsBookData();

                scope = TransactionFactory.GetNewTransaction();

                DateTime lastUpdateDateSent = (DateTime)requestParams.toDate == null ? DateTime.MinValue : requestParams.toDate.Value;
                if (DateTime.Compare(lastUpdateDateSent,DateTime.Today) > 0)
                {
                    lastUpdateDateSent = DateTime.Today;
                }
                if (dbCustomsBookPM == null)
                {
                    dbCustomsBookPM = new CustomsBookPM();
                    dbCustomsBookPM.LastUpdateDate = lastUpdateDateSent;
                    dbCustomsBookPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    //DateTime toDate = (DateTime)requestParams.toDate == null ? DateTime.MinValue : requestParams.toDate.Value;
                    DateTime lastUpdateDate = (DateTime)dbCustomsBookPM.LastUpdateDate == null ? DateTime.MinValue : dbCustomsBookPM.LastUpdateDate.Value;
                    if (DateTime.Compare(lastUpdateDateSent, lastUpdateDate) > 0)
                    {
                        dbCustomsBookPM.LastUpdateDate = lastUpdateDateSent;
                    }
                    else
                    {
                        dbCustomsBookPM.LastUpdateDate = lastUpdateDate;
                    }
                    dbCustomsBookPM.ChangeSetOp = ChangeSetOperation.Update;
                }

                //If the date is a future date - Make it NOW
                if (DateTime.Compare(dbCustomsBookPM.LastUpdateDate.Value, DateTime.Now) > 0)
                {
                    dbCustomsBookPM.LastUpdateDate = DateTime.Now;
                }

                dbCustomsBookPM.Tenant = tenant;

                var tk2 = true;
                if (tk2)
                {
                    ContactRepository contactRep = new ContactRepository(tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, tenant);
                    dbCustomsBookPM.LastUpdateByUserId = contact.Id;
                }
                else
                {


                    string useremail = SecurityUtility.GetAuthenticatedUser();
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(useremail, tenant, true);
                    if (loggedContact == null)
                    {
                        loggedContact = contactQuery.GetContactByEmailOnly(useremail, tenant);
                    }
                    dbCustomsBookPM.LastUpdateByUserId = loggedContact.Id;
                }
                customsBookUpdateService.Update(dbCustomsBookPM, false);
                logContext.SaveChanges();
            }
            finally
            {
                scope.Complete();
                scope.Dispose();
            }
        }
    }
}