using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class BusinessUnitService
    {
        bool isNewEntity;
        private int tenant;
        public BusinessUnit Poco { get; set; }
        private BusinessUnitPM entityPM;
        private ICommonDataContext objectContext;
        private BusinessUnitRepository entityRepository;
        public BusinessUnitService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new BusinessUnitRepository(objectContext);
        }

        public void Create(BusinessUnitPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = this.GenerateNewId();

            this.Poco = new BusinessUnit()
            {
                Id = entityPM.Id,
                Tenant = tenant,
            };

            BusinessUnitTracing.Trace(entityPM, Poco, isNewEntity);
            BusinessUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(BusinessUnitPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleBusinessUnit(entityPM.Id, tenant);

            BusinessUnitTracing.Trace(entityPM, Poco, isNewEntity);
            BusinessUnitMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private string GenerateNewId()
        {
            string myResultId = null;

            if (string.IsNullOrEmpty(entityPM.ParentId))
            {
                myResultId = tenant.ToString();
            }

            else
            {
                int numberOfSplitChar = entityPM.ParentId.Count(d => d == '-');

                List<string> allIds = entityRepository.GetAllIds(tenant);
                List<string> matchedIds = allIds.Where(d => d.Count(c => c == '-') == numberOfSplitChar + 1).ToList();
                List<string> numerics = matchedIds.Select(d => d.Substring(d.LastIndexOf('-') + 1)).ToList();

                int maxIdNumber = 0;

                if (numerics.Count > 0)
                {
                    var maxValue = (from max in numerics select Convert.ToInt32(max)).Max();
                    maxIdNumber = maxValue + 1;
                }

                else
                {
                    Int32.TryParse(numerics.Max(), out maxIdNumber);
                }

                myResultId = entityPM.ParentId + '-' + maxIdNumber.ToString();
            }

            return myResultId;
        }
    }
}
