using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.QueryService
{
    public  class AutomaticReconcileQueryService
    {

        IAccountingContext context;
        //AutomaticReconcileService service; 

        Logitude.Accounting.BL.EntityQueryServices.AutomaticReconcileQueryService query;

        public AutomaticReconcileQueryService(int tenant)
        {
            context = AccountingContext.GetContext(tenant);
            //service = new AutomaticReconcileService(context, tenant); 
            query = new Logitude.Accounting.BL.EntityQueryServices.AutomaticReconcileQueryService(tenant);
        }


        public AutomaticReconcile GetAutomaticReconcileById(string Id, int Tenant)
        {
            try
            {


                var temp = query.GetSinglePM(Id, Tenant);
                if (temp == null)
                    throw new ApplicationException("AutomaticReconcile with Id " + Id + " doesn't exist");

                return AutomaticReconcileDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AutomaticReconcile AutomaticReconcileDataMapping(AutomaticReconcilePM MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var temp = new AutomaticReconcile();
             
                temp.Code = MyEntityPM.Code;
                temp.EnglishName = MyEntityPM.EnglishName;
                temp.LocalName = MyEntityPM.LocalName;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AutomaticReconcilePM AutomaticReconcileDataMappingAndValidatin(AutomaticReconcile MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new AutomaticReconcilePM();
                if (!string.IsNullOrEmpty(MyEntity.Code))
                {
                    temp = query.GetSinglePM(MyEntity.Code, Tenant);
                }

                if (temp == null)
                {
                    throw new ApplicationException("AutomaticReconcile with code " + MyEntity.Code + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                temp.EnglishName = MyEntity.EnglishName;
                temp.LocalName = MyEntity.LocalName;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
