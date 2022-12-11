using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalTextCodeQuery
    {
        public DigitalTextCodeList GetDigitalTextCodesQuery(int tenant, string objectTableId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);

            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodes(tenant, objectTableId)
                                                            .Select(x => new DigitalTextCodeList
                                                            {
                                                                Id = x.Id,
                                                                ObjectTableId = x.ObjectTableId,
                                                                Tenant = x.Tenant,
                                                                Labels = x.Labels,
                                                                CreateDate = x.CreateDate,
                                                                UpdateDate = x.UpdateDate
                                                            })
                                                            .FirstOrDefault();
            return defaultTextCode;
        }

        public bool CheckTenantTranslation(int tenant, string objectTableId = "")
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);
            return digitalTextCodeRepository.CheckTenantTranslation(tenant, objectTableId);
        }
        
        public void UpdateDigitalTextCodes(DigitalTextCodeList digitalTextCodeList)
        {
            if (string.IsNullOrEmpty(digitalTextCodeList.Id))
            {
                var entityPm = new DigitalTextCodePM
                {
                    ObjectTableId = digitalTextCodeList.ObjectTableId,
                    Tenant = digitalTextCodeList.Tenant,
                    Labels = digitalTextCodeList.Labels,
                    CreateDate = digitalTextCodeList.CreateDate,
                    UpdateDate = digitalTextCodeList.UpdateDate
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalTextCodeUpdateService service = new DigitalTextCodeUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalTextCodePM
                {
                    Id = digitalTextCodeList.Id,
                    ObjectTableId = digitalTextCodeList.ObjectTableId,
                    Tenant = digitalTextCodeList.Tenant,
                    Labels = digitalTextCodeList.Labels,
                    CreateDate = digitalTextCodeList.CreateDate,
                    UpdateDate = digitalTextCodeList.UpdateDate
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalTextCodeUpdateService service = new DigitalTextCodeUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}