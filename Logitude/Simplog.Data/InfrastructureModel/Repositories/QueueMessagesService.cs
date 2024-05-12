using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueueMessagesService
    {
        public List<QueueMessage> FilteredQuery(QueueMessageRepository queueMessageRepository, List<FilterCriteria> filterCriteria, int tenant)
        {
            IQueryable<QueueMessage> entityPocos = queueMessageRepository.GetQueueMessages();
            foreach (var criteria in filterCriteria)
            {
                if (criteria.FieldName == "CreateDateTime" && criteria.Operator == "Between" &&
                    DateTime.TryParse(criteria.FieldValue, out DateTime fromDate) &&
                    DateTime.TryParse(criteria.FieldValue2, out DateTime toDate))
                {
                    entityPocos = entityPocos.Where(msg => msg.CreateDateTime >= fromDate && msg.CreateDateTime <= toDate);
                }
                if (criteria.FieldName == "Status" && int.TryParse(criteria.FieldValue, out int status))
                {
                    entityPocos = entityPocos.Where(msg => msg.Status == status);
                }
                if (tenant == 0 && criteria.FieldName == "Tenant" && int.TryParse(criteria.FieldValue, out int tenantFilter))
                {
                    entityPocos = entityPocos.Where(msg => msg.Tenant == tenantFilter);
                }
                if (criteria.FieldName == "RetryNumber" && int.TryParse(criteria.FieldValue, out int retryNumber))
                {
                    entityPocos = entityPocos.Where(msg => msg.RetryNumber == retryNumber);
                }
            }

            //var sqlQuery = entityPocos.ToString();

            return entityPocos.ToList();
        }
    }
}
