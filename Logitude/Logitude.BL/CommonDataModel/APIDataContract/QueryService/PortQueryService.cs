using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;

 namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{ 
   public partial class PortQueryService
   {
        public Port GetCustomPortById(string Id, int Tenant)
        { 
            return PortCustomDataMapping(Id, Tenant);
        }

        public Port PortCustomDataMapping(string PortId,int Tenant)
        {
            var MyEntityPM = query.GetSinglePM(PortId,Tenant);

               var temp = new Port(); 
			   temp.Id = MyEntityPM.Id;
			   temp.Code = MyEntityPM.Code;
			   temp.LocalName = MyEntityPM.LocalName;
			   temp.EnglishName = MyEntityPM.EnglishName;
			   //temp.CombinedCode = MyEntityPM.Code + MyEntityPM.CountryCode;				
			   return temp;
        } 

		public PortPM PortCustomDataMappingAndValidatin(Port MyEntity,int Tenant)
        {
		   		    var temp = new PortPM();
			if (!string.IsNullOrEmpty(MyEntity.Id))
			{
				temp = query.GetSinglePM(MyEntity.Id, Tenant);
			} 
			if(string.IsNullOrEmpty(temp.Id))
			{
			    temp.Id = MyEntity.Id;
			}
			temp.Code = MyEntity.Code;
			temp.LocalName = MyEntity.LocalName;
			temp.EnglishName = MyEntity.EnglishName;
			//temp.Code = MyEntity.CombinedCode;               
			   return temp;
			     
        }
		 
   }
}