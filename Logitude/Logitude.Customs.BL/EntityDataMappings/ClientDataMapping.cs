
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClientDataMapping: IMapping<ClientPM, Client>
   {

        public void CustomPMToPOCO(ClientPM entityPM, Client entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                
                entityPOCO.Id = entityPM.Id;               
                entityPOCO.Tenant = entityPM.Tenant;
                if (entityPM.FullName == null)
                {
                    entityPM.FullName = entityPM.LocalFirstName +" " + entityPM.LocalLastName;
                }
            } 




            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;       
            
        }
        private static void BuildSearchFields(ClientPM entityPM, Client poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.ClientTypeSpecificCode + "," + entityPM.Code + "," + entityPM.DunsNumber + "," + entityPM.EnglishBirthPlace + "," + entityPM.EnglishCorporationName + "," + entityPM.EnglishFatherName + "," + entityPM.EnglishFirstName + "," + entityPM.EnglishLastName + "," + entityPM.FullName + "," + entityPM.GenderCode + "," + entityPM.LocalCorporationName + "," + entityPM.LocalFirstName + "," + entityPM.LocalLastName + "," + entityPM.PassportCountryCode + "," + entityPM.PassportFirstName + "," + entityPM.PassportLastName + "," + entityPM.PassportNumber + "," + entityPM.PassportTypeCode;

            if (isNewEntity)
            {

            }

            else
            {

            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
        public void CustomPOCOToPM(ClientPM entityPM, Client entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ClientTypeSpecificName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PassportCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GenderName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PassportTypeName);


            if (entityPOCO.ClientTypeSpecificCode != null)
            {
                CustomerTypeGeneralQueryService customerTypeGeneralQueryService = new CustomerTypeGeneralQueryService(entityPOCO.Tenant);
                CustomerTypeGeneralPM customerTypeGeneral = customerTypeGeneralQueryService.GetSingle(entityPOCO.ClientTypeSpecificCode, false, true);
                entityPM.ClientTypeSpecificName = customerTypeGeneral.LocalName;
            }

            if (entityPOCO.PassportCountryCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(entityPOCO.PassportCountryCode, false, true);
                entityPM.PassportCountryName = customsCountry.LocalName;
            }
            if (entityPOCO.GenderCode != null)
            {
                GenderQueryService genderQueryService = new GenderQueryService(entityPOCO.Tenant);
                GenderPM gender = genderQueryService.GetSingle(entityPOCO.GenderCode, false, true);
                entityPM.GenderName = gender.LocalName;
            }

            if (entityPOCO.PassportTypeCode != null)
            {
                PassportTypeQueryService passportTypeQueryService = new PassportTypeQueryService(entityPOCO.Tenant);
                PassportTypePM passportType = passportTypeQueryService.GetSingle(entityPOCO.PassportTypeCode, false, true);
                entityPM.PassportTypeName = passportType.LocalName;
            }
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);
            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();


        }
   }


}
   