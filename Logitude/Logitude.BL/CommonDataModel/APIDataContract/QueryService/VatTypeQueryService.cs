using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class VatTypeQueryService
    {
        public VatTypePM VatTypeCustomDataMappingAndValidatin(VatType MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new VatTypePM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }
                else
                {
                    temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
                }
                if (temp == null)
                {
                    throw new ApplicationException("VatType with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }

                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public VatType VatTypeCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                VatTypeQueryService VatTypeService0 = new VatTypeQueryService(Tenant);
                var VatType = VatTypeService0.GetVatTypeById(Id, Tenant,ComputingPartnerName);
                return VatType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {


           
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(tenant);
            VatTypeRepository vatTyperepository = new VatTypeRepository(tenant);
            VatTypeService service = new VatTypeService(context,  tenatToCopy);
            VatTypePercentageService vatTypePercentageService = new VatTypePercentageService(context, tenatToCopy);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(tenatToCopy);
            List<Simplog.Data.CommonDataModel.EntityPOCOs.VatType> pocos = vatTypeRepository.All().Where(t => t.InActive == false && t.Tenant== tenant).ToList();



            foreach (var item in pocos)
            {
                VatTypePM vatType = this.query.GetSinglePMByCode(item.Code, tenatToCopy);
                string vatTypeId = vatType.Id;
                if (vatTypeId == null)
                {
                     vatType = new VatTypePM()
                    {
                        Code = item.Code,
                        LocalName = item.LocalName,
                        Tenant = tenatToCopy,
                        EnglishName = item.EnglishName,
                        AddedManually = item.AddedManually,
                        InActive = item.InActive,
                        Description = item.Description,
                        LocalDescription = item.LocalDescription,
                        SearchFields = item.SearchFields,
                        ExternalVATCard = item.ExternalVATCard,
                        ExternalTAXItemId = item.ExternalTAXItemId,
                        IsMultiPercentage = item.IsMultiPercentage,
                        RecognizedPercentage = item.RecognizedPercentage,
                        PayablesExternalId = item.PayablesExternalId,
                        ReceivablesExternalId = item.ReceivablesExternalId,
                        IsRegionalTax = item.IsRegionalTax,




                    };
                    service.Create(vatType);
                    vatTypeId = vatTyperepository.GetSingleVatTypeByCode(vatType.Code, tenatToCopy).Id;
                  
                }
                VatTypePercentage vatTypePercentage = vatTypePercentageRepository.GetVatTypePercentageByVatTypeId(item.Id,tenant);
                if (vatTypePercentage != null)
                {
                    VatTypePercentagePM vatTypePercentagePM = new VatTypePercentagePM()
                    {
                        Tenant = tenatToCopy,
                        FromDate = vatTypePercentage.FromDate,
                        Percentage = vatTypePercentage.Percentage,
                        VatTypeId = vatTypeId
                    };


                    vatTypePercentageService.Create(vatTypePercentagePM, vatType);
                }
            }
            this.context.SaveChanges();

        }
    }
}
