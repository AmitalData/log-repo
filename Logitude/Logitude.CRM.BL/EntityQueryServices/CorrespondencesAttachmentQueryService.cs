using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class CorrespondencesAttachmentQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, CorrespondencesAttachmentPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            CorrespondencesAttachmentKeys ticketKeys = entityKeys as CorrespondencesAttachmentKeys;
        }

        public List<CorrespondencesAttachmentPM> GetCorrespondencesAttachmentsListByTenant(int tenant)
        {
            List<CorrespondencesAttachment> entities = repository.GetAll(tenant).ToList();
            List<CorrespondencesAttachmentPM> myEntities = new List<CorrespondencesAttachmentPM>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    EntityPM = new CorrespondencesAttachmentPM();
                    mapping.CustomPOCOToPM(EntityPM, item);
                    mapping.POCOToPM(EntityPM, item);
                    myEntities.Add(EntityPM);
                }
            }

            return myEntities;
        }

        public List<CorrespondencesAttachmentPM> GetCorrespondencesAttachmentsListByCorrespondenceIdAndTenant(string id, int tenant)
        {
            List<CorrespondencesAttachment> entities = repository.GetAllCorrespondencesAttachmentByCorrespondenceIdAndTenant(id,tenant).ToList();
            List<CorrespondencesAttachmentPM> myEntities = new List<CorrespondencesAttachmentPM>();
           
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    EntityPM = new CorrespondencesAttachmentPM();
                    mapping.CustomPOCOToPM(EntityPM, item);
                    mapping.POCOToPM(EntityPM, item);
                    myEntities.Add(EntityPM);
                }
            }

            return myEntities;
        }
    }
}
