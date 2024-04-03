using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    class EventRemarkMapping
    {
        public static void MapEntity(EventRemarkPM eventRemarkPM, EventRemark eventRemark, bool isNewState)
        {
            eventRemark.EventTypeId = eventRemarkPM.EventTypeId;
            eventRemark.PartnerTypeId = eventRemarkPM.PartnerTypeId;
            eventRemark.Tenant = eventRemarkPM.Tenant;
            eventRemark.CreateDate = eventRemarkPM.CreateDate;
            eventRemark.CreatedByUserId = eventRemarkPM.CreatedByUserId;
            eventRemark.SearchFields = eventRemarkPM.SearchFields;
            eventRemark.IsChoose = eventRemarkPM.IsChoose;
        }
    }
}
