using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data;
using Logitude.Customs.BL.NotificationBL;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.Models;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using System.Data.Entity.Core;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.CustomsMessaging.Common.Gen;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common;
using System.IO;
using System.Xml.Serialization;
using Logitude.Customs.BL.Messaging.Customs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.BL.Utils;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.ILOVS;
using System.Diagnostics;
using Logitude.Customs.BL.CloseTables;
using System.Text.RegularExpressions;
using Logitude.Customs.BL.BL;
using System.Configuration;
using System.Globalization;
using Logitude.Customs.BL.Messaging.ILSWS;
using System.Xml;
using Microsoft.Practices.ObjectBuilder2;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CertificateOfOriginUpdateService
	{

        protected override void OnCreating(CertificateOfOriginPM entityPM, EntityPM entityParentPM)
        {

            // update user data:
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            entityPM.OpenByUser = contact.Id;
            //update counter:
            entityPM.Counter = CodeCounter.GetNumber("Counter", entityPM.Tenant).ToString();

        }

        protected override void UpdateComposition(CertificateOfOriginPM entityPM)
        {
            if(entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                for (int i = entityPM.CertificateOriginInvoiceItems.Count - 1; i >= 0; i--)
                {
                    var item = entityPM.CertificateOriginInvoiceItems[i];
                    if (item.ChangeSetOp == ChangeSetOperation.Delete)
                    {
                        entityPM.DeletedCertificateOriginInvoiceItems.Add(item);
                        entityPM.CertificateOriginInvoiceItems.RemoveAt(i);
                    }
                    else if (!string.IsNullOrEmpty(item.Id))
                    {
                        item.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }

                for (int i = entityPM.CertificateOriginItemItems.Count - 1; i >= 0; i--)
                {
                    var item = entityPM.CertificateOriginItemItems[i];
                    if (item.ChangeSetOp == ChangeSetOperation.Delete)
                    {
                        entityPM.DeletedCertificateOriginItemItems.Add(item);
                        entityPM.CertificateOriginItemItems.RemoveAt(i);
                    }
                    else if (!string.IsNullOrEmpty(item.Id))
                    {
                        item.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
            CertificateOfOriginInvoiceUpdateService consignmentUpdateService = new CertificateOfOriginInvoiceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            consignmentUpdateService.UpdateMulti(entityPM.CertificateOriginInvoiceItems, entityPM.DeletedCertificateOriginInvoiceItems, entityPM, false);

			CertificateOfOriginItemUpdateService certificateOfOriginItemUpdateService = new CertificateOfOriginItemUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
			certificateOfOriginItemUpdateService.UpdateMulti(entityPM.CertificateOriginItemItems, entityPM.DeletedCertificateOriginItemItems, entityPM, false);
			base.UpdateComposition(entityPM);
        }

    }
}
