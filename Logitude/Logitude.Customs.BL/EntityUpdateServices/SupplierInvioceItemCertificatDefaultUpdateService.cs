 
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
   public partial class SupplierInvioceItemCertificatDefaultUpdateService
   {

		protected override void OnCreating(SupplierInvioceItemCertificatDefaultPM entityPM, SupplierInvioceExportDefaultPM entityParentPM)
		{
			entityPM.SupplierInvioceExportDefaultId = entityParentPM.Id;
			entityPM.Tenant = entityParentPM.Tenant;
			//entityPM.SequenceNumeric = entityParentPM.SequenceNumeric;

			base.OnCreating(entityPM, entityParentPM);
		}
		public void FastDeleteComposition(SupplierInvioceExportDefaultKeys entityKeyFields)
		{
			(Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatDefaultRepository).FastDeleteMulti(entityKeyFields);
		}

	}
   
}
	 