using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteSettingService
    {
        private int tenant;
        private bool isNewEntity;
        private QuoteSettingPM entityPM;
        public QuoteSetting entityPoco { get; set; }
        private IQuotesContext objectContext;
        private QuoteSettingRepository entityRepository;
        public QuoteSettingService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new QuoteSettingRepository(objectContext);
        }

        public void Create(QuoteSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("QuoteSetting", tenant).ToString();
            this.entityPoco = new QuoteSetting() { Id = this.entityPM.Id };

            QuoteSettingMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPoco = entityRepository.GetSingleQuoteSetting(tenant);

            QuoteSettingMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }
    }
}
