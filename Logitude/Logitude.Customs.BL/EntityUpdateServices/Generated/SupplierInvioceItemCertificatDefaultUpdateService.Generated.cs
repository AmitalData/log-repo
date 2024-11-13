 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class SupplierInvioceItemCertificatDefaultUpdateService:EntityUpdateService<SupplierInvioceItemCertificatDefault,SupplierInvioceItemCertificatDefaultPM,SupplierInvioceExportDefaultPM>
   {
   
        SupplierInvioceItemCertificatDefaultRepository entityRepository;
        public SupplierInvioceItemCertificatDefaultUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new SupplierInvioceItemCertificatDefaultDataMapping();
            Repository = new SupplierInvioceItemCertificatDefaultRepository(context);
        }

       
        private ICustomContext currentContext;
        public SupplierInvioceItemCertificatDefaultUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvioceItemCertificatDefaultUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(SupplierInvioceItemCertificatDefaultPM entityPM)
        {
            SupplierInvioceItemCertificatDefaultKeys entityKeys = new SupplierInvioceItemCertificatDefaultKeys() { SupplierInvioceExportDefaultId = entityPM.SupplierInvioceExportDefaultId, SequenceNumeric = entityPM.SequenceNumeric };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(SupplierInvioceItemCertificatDefaultPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(SupplierInvioceItemCertificatDefaultPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 