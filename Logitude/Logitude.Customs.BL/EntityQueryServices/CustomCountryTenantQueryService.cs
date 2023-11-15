 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CustomsCountryTenantQueryService
	{


        public List<CustomsCountryTenantPM> GetByTenant( int tenant, string code)
        {
            var customDocumentTypes = repository.GetAll(tenant, code).ToList();

            return customDocumentTypes.Select(poco => this.GetEntityPM(poco)).ToList();

        }


    }

}
	 