
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsVendorDataMapping: IMapping<CustomsVendorPM, CustomsVendor>
   {

        public void CustomPMToPOCO(CustomsVendorPM entityPM, CustomsVendor entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.Id = entityPM.Id;             
                entityPOCO.Tenant = entityPM.Tenant;



            }


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;       
        }

        public void CustomPOCOToPM(CustomsVendorPM entityPM, CustomsVendor entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);
            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        private static void BuildSearchFields(CustomsVendorPM entityPM, CustomsVendor poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.VendorNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.VendorNumber : result + "," + entityPM.VendorNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.VendorName))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.VendorName : result + "," + entityPM.VendorName;
            }

            if (!string.IsNullOrEmpty(entityPM.DunsNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.DunsNumber : result + "," + entityPM.DunsNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.VATNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.VATNumber : result + "," + entityPM.VATNumber;
            }


            //ClientRepository clientRepository = new ClientRepository(entityPM.Tenant);
            //ClientKeys clientKeys = new ClientKeys() { Id = entityPM.ImporterId };
            //Client client = clientRepository.GetSingle(clientKeys);
            //if (client != null)
            //{
            //    result = string.IsNullOrEmpty(result) ? client.EnglishName : result + "," + client.EnglishName;
            //}

            //if (isNewEntity)
            //{
            //    foreach (VendorCommunicationPM item in entityPM.VendorCommunications)
            //    {
            //        if (!string.IsNullOrEmpty(item.CommunicationAddress))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.CommunicationAddress : result + "," + item.CommunicationAddress;
            //        }


            //    }
            //}

            //else
            //{
            //    VendorCommunicationRepository communicationRepository = new VendorCommunicationRepository(entityPM.Tenant);
            //    CustomsVendorKeys entityKeys = new CustomsVendorKeys() { Id = entityPM.Id };
            //    List<VendorCommunication> communications = communicationRepository.GetMulti(entityKeys);
            //    foreach (VendorCommunication item in communications)
            //    {
            //        if (!string.IsNullOrEmpty(item.CommunicationAddress))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.CommunicationAddress : result + "," + item.CommunicationAddress;
            //        }


            //    }
            //}

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
   }


}
   