using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ComputingPartnerTranslationHelper
    {
        ICommonDataContext context;
        ObjectTableRepository myObjectTabelRepository;
        ComputingPartnerQuery computingPartnerQuery;
        ComputingPartnerTranslationQuery computingPartnerTranslationQuery;
        int tenant = 0;
        public ComputingPartnerTranslationHelper(int tenant)
        {
            this.tenant = tenant;
            context = CommonDataContext.GetContext(tenant);
            myObjectTabelRepository = new ObjectTableRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(new ComputingPartnerRepository(context));
            computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(new ComputingPartnerTranslationRepository(context));

        }

        public string GetComputingPartnerCodeTranslation(string logitudeCode, string computingPartner, string objectTableName)
        {
          
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            ComputingPartnerPM partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, tenant);
            if (partner == null)
            {
                partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, 0);
            }

            string partnerCode = null;
            if (partner != null && objectTable != null)
            {
                partnerCode = computingPartnerTranslationQuery.GetPartnerCodeTranslation(logitudeCode, partner.Id, objectTable.Id, tenant);
            }


            return partnerCode;
        }

        public List<ComputingPartnerTranslationPM> GetComputingPartnerCodeTranslations( string computingPartner, int tenant)
        {
            List<ComputingPartnerTranslationPM> partnerTranslationPMs = null;
        
            ComputingPartnerPM partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, tenant);
            if (partner == null)
            {
                partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, 0);
            }

         
            if (partner != null )
            {
                partnerTranslationPMs = computingPartnerTranslationQuery.GetAllByComputingPartner(partner.Id,  tenant).ToList();
            }


            return partnerTranslationPMs;
        }
        public string GetLogitudeCodeTranslation(string PartnerCode, string computingPartner, string objectTableName)
        {
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            ComputingPartnerPM partner = null;

            partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, tenant);

            if (partner == null)
            {
                partner = computingPartnerQuery.GetSinglePMByCode(computingPartner, 0);
            }

            string MyCode = null;
            if (partner != null && objectTable != null)
            {
                MyCode = computingPartnerTranslationQuery.GetLogitudeCodeTranslation(PartnerCode, partner.Id, objectTable.Id, tenant);
            }

            return MyCode;
        }
    }
}
