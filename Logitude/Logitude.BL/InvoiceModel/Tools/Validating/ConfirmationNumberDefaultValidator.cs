using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.CommonDataModel;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Globalization;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Logitude.BL.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Data.Entity.Core;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using System.Windows.Controls;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace Logitude.BL.InvoiceModel.Tools.Validating
{
    public class ConfirmationNumberDefaultValidator
    {
        public static void Validate(ConfirmationNumberDefaultPM entityPM, IInvoiceContext myContext)
        {
           var value = myContext.ConfirmationNumberDefaults .FirstOrDefault(a => DbFunctions.TruncateTime(a.FromDate)== DbFunctions.TruncateTime(entityPM.FromDate) && a.Tenant==entityPM.Tenant);
           
            if (value!=null)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "FromDate");
                throw new Exception(msg);
            }
        }


       

   
       
    }
}