using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class EntityStatusHelper
    {
        public static string GetHighestStatusId(string oldStatusId, string newStatusId, int tenant)
        {
            string myHighestStatusId = null;

            if (string.IsNullOrEmpty(oldStatusId) && string.IsNullOrEmpty(newStatusId))
            {
                myHighestStatusId = null;
            }

            else if (string.IsNullOrEmpty(oldStatusId) || string.IsNullOrEmpty(newStatusId))
            {
                myHighestStatusId = oldStatusId;

                if (string.IsNullOrEmpty(myHighestStatusId))
                {
                    myHighestStatusId = newStatusId;
                }
            }

            else
            {
                EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(oldStatusId, tenant, true);
                EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);

                myHighestStatusId = oldStatusId;

                if (newStatus.StatusWeight > oldStatus.StatusWeight)
                {
                    myHighestStatusId = newStatusId;
                }
            }

            return myHighestStatusId;
        }

        public static string GetHighestStatusId(string oldStatusId, string newStatusId, int tenant, ref string statusName)
        {
            string myHighestStatusId = null;

            if (string.IsNullOrEmpty(oldStatusId) && string.IsNullOrEmpty(newStatusId))
            {
                myHighestStatusId = null;
                statusName = null;
            }

            else if (string.IsNullOrEmpty(oldStatusId) || string.IsNullOrEmpty(newStatusId))
            {
                myHighestStatusId = oldStatusId;

                if (string.IsNullOrEmpty(myHighestStatusId))
                {
                    myHighestStatusId = newStatusId;
                }

                EntityStatus myStatus = EntityStatusRepository.GetSingleEntityStatus(myHighestStatusId, tenant, true);
                statusName = myStatus.Name;
            }

            else
            {
                EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(oldStatusId, tenant, true);
                EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);

                myHighestStatusId = oldStatus.Id;
                statusName = oldStatus.Name;

                if (newStatus.StatusWeight > oldStatus.StatusWeight)
                {
                    myHighestStatusId = newStatus.Id;
                    statusName = newStatus.Name;
                }
            }

            return myHighestStatusId;
        }
    }
}
