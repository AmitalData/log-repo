using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.Tools.DataMapping;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AccountingSystemService
    {
        bool isNewEntity;
        private int tenant;
        public AccountingSystem Poco { get; set; }
        private AccountingSystemPM entityPM;
        private AccountingSystemRepository entityRepository;

        private ICommonDataContext objectContext;
        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        public AccountingSystemService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new AccountingSystemRepository(objectContext);
        }

        public void Update(AccountingSystemPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleAccountingSystem(theEntityPm.Code);

            AccountingSystemMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
