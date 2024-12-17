using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CardSearchService
    {
        bool isNewEntity;
        private int tenant;
        public CardSearch Poco { get; set; }
        private CardSearchPM entityPM;
        private ICommonDataContext objectContext;
        private CardSearchRepository entityRepository;
        public CardSearchService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CardSearchRepository(objectContext);

        }
        
  
        public void Create(CardSearchPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.entityPM.Id = IdCounter.GetNumber("CardSearch", tenant).ToString();
            this.Poco = new CardSearch();
            this.Poco.Id = this.entityPM.Id;

         
            CardSearchMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CardSearchPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCardSearch(entityPM.Id, entityPM.Tenant);

      


            CardSearchMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

       
    }
}
