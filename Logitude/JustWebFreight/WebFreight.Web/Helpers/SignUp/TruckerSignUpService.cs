using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System; 
using WebFreight.Web.InfrastructureModel;

namespace WebFreight.Web.Helpers.SignUp
{
    public class TruckerSignUpService
    { 
        private CountryRepository countryRepository;
        private int tenant;
        private int tenantZero = 0;
        private TruckerService truckerService;
        ICommonDataContext iCommonDataContext;
        TruckerPM tenantZeroTrucker;  
        private Contact systemContact; 
        ContactRepository contactRepository;
        SignUpInfoClass signUpInfoClass;
        private PaymentTermRepository paymentTermRepository;
        public TruckerSignUpService(SignUpInfoClass signUpInfoClass, int tenant)
        {
            this.tenant = tenant;
            this.iCommonDataContext = CommonDataContext.GetContext(tenant);

            this.contactRepository = new ContactRepository(this.iCommonDataContext);
            this.signUpInfoClass = signUpInfoClass;
            this.systemContact  = GetLoggedCountact(tenant);  
            this.countryRepository = new CountryRepository(tenant);
            this.paymentTermRepository = new PaymentTermRepository(tenant); 
            tenantZeroTrucker = GetTenantZeroTrucker(tenant);

        }

        private Contact GetLoggedCountact(int tenant)
        {
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            return contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
        }

        private TruckerPM GetTenantZeroTrucker(int tenant)
        {
            TruckerQuery truckerQuery = new TruckerQuery(tenant);
            return truckerQuery.GetSinglePMByCode("---", tenantZero);
        }

        internal void CopyFromTenantZero()
        {
            if (IsAllowCopyTrucker() && tenantZeroTrucker != null)
            { 
               CopyTrucker();  
            }
        } 
        public void CopyTrucker()
        {
             
            TruckerPM newTruckerPM = GetNewTruckerInstance(); 

            this.truckerService = new TruckerService(this.iCommonDataContext, newTruckerPM, this.systemContact.Id); 
            this.truckerService.Create(newTruckerPM);
        }

        private TruckerPM GetNewTruckerInstance()
        {
            Country country = countryRepository.GetSingleCountryByCode("--", tenant);
            PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTermByCode("--", tenant);

            return new TruckerPM()
            {
                Code = tenantZeroTrucker.Code,
                EnglishName = tenantZeroTrucker.EnglishName,
                LocalName = tenantZeroTrucker.LocalName,
                InActive = tenantZeroTrucker.InActive,
                VatNumber = tenantZeroTrucker.VatNumber,
                CountryId = country?.Id,
                CreateDate = DateTime.Today,
                UpdateDate = DateTime.Today,
                SearchFields = tenantZeroTrucker.SearchFields,
                PaymentTermId = paymentTerm?.Id,
                Notes = tenantZeroTrucker.Notes,
                Tenant = tenant,
                CountryCode = country?.Code,
                CountryName = country?.EnglishName,
                IsHybrid = true,
                CarrierTypeId = "TR",

            };
        }

        private bool IsAllowCopyTrucker()
        {
            return LogitudeSettings.DeploymentStage == "amitalstorage" || LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.DeploymentStage == "logboxwe1";
        }
          
    }
}