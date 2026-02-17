using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CardExternalCodeByCurrencyService
    {
          bool isNewEntity;
        private int tenant;
        public CardExternalCodeByCurrency Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CardExternalCodeByCurrencyPM entityPm;
        private ICommonDataContext objectContext;
        private CardExternalCodeByCurrencyRepository entityRepository;
        public CardExternalCodeByCurrencyService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CardExternalCodeByCurrencyRepository(objectContext);
        }

        public void Create(CardExternalCodeByCurrencyPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CardExternalCodeByCurrency", tenant).ToString();
            this.Poco = new CardExternalCodeByCurrency();
            this.Poco.Id = this.entityPm.Id;

          
          
            CardExternalCodeByCurrencyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CardExternalCodeByCurrencyPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCardExternalCodeByCurrency(entityPM.Id, entityPm.Tenant);

      
           
            CardExternalCodeByCurrencyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }   
    }
}
