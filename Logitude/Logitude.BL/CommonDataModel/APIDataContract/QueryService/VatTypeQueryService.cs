using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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

           
            VatTypePercentageRepository VatTypePercentageRepository = new VatTypePercentageRepository(tenant);
            VatTypeRepository VatTyperepository = new VatTypeRepository(tenant);
            VatTypeService service = new VatTypeService(context,  tenatToCopy);
            VatTypePercentageService VatTypePercentageService = new VatTypePercentageService(context, tenatToCopy);
            List<VatTypePercentage> pocos = VatTypePercentageRepository.GetVatTypePercentagesByTenant(tenant).Where(t=>t.VatType.InActive==false).ToList();

            foreach (var item in pocos)
            {
                VatTypePM vatType = new VatTypePM()
                {
                    Code = item.VatType.Code   ,
                    LocalName = item.VatType.LocalName,
                    Tenant = tenatToCopy,
                    EnglishName = item.VatType.EnglishName,
                    AddedManually = item.VatType.AddedManually,
                    InActive = item.VatType.InActive,
                    Description = item.VatType.Description,
                    LocalDescription = item.VatType.LocalDescription,
                    SearchFields = item.VatType.SearchFields,
                    ExternalVATCard = item.VatType.ExternalVATCard,
                    ExternalTAXItemId = item.VatType.ExternalTAXItemId,
                    IsMultiPercentage = item.VatType.IsMultiPercentage,
                    RecognizedPercentage = item.VatType.RecognizedPercentage,
                    PayablesExternalId = item.VatType.PayablesExternalId,
                    ReceivablesExternalId = item.VatType.ReceivablesExternalId,
                    IsRegionalTax = item.VatType.IsRegionalTax,




                };
                service.Create(vatType);
                string vatTypeId = VatTyperepository.GetSingleVatTypeByCode(vatType.Code, tenatToCopy).Id;
                VatTypePercentagePM vatTypePercentagePM = new VatTypePercentagePM()
                {
                    Tenant = tenatToCopy,
                    FromDate = item.FromDate,
                    Percentage = item.Percentage,
                    VatTypeId= vatTypeId
                };



                VatTypePercentageService.Create(vatTypePercentagePM, vatType);

            }
            this.context.SaveChanges();

        }
    }
}
