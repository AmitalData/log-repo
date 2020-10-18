using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   public class ARInvoiceLineActionQueryService
    {
        ARInvoiceLineActionQuery query;

        public ARInvoiceLineActionQueryService(int tenant)
        {

            query = new ARInvoiceLineActionQuery(tenant);
        }


        public ARInvoiceLineAction GetARInvoiceLineActionByCode(string Code, int Tenant)
        {
            try
            {


                var temp = query.GetEntityList(Code);
                if (temp == null)
                    throw new ApplicationException("ARInvoiceLineAction with Code " + Code + " doesn't exist");

                return ARInvoiceLineActionDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ARInvoiceLineAction ARInvoiceLineActionDataMapping(ARInvoiceLineActionList MyEntityList, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var temp = new ARInvoiceLineAction();
                temp.Code = MyEntityList.Code;
                temp.Name = MyEntityList.Name;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ARInvoiceLineAction ARInvoiceLineActionDataMapping(string code, int Tenant)
        {
            try
            {

                ARInvoiceLineActionQueryService ARInvoiceLineActionQuery = new ARInvoiceLineActionQueryService(Tenant);
                var ARInvoiceLineAction = ARInvoiceLineActionQuery.GetARInvoiceLineActionByCode(code, Tenant);
                return ARInvoiceLineAction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ARInvoiceLineActionList ARInvoiceLineActionDataMappingAndValidatin(ARInvoiceLineAction MyEntity, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {
                var temp = new ARInvoiceLineActionList();
                if (!string.IsNullOrEmpty(MyEntity.Code))
                {
                    temp = query.GetEntityList(MyEntity.Code);
                }
                if (temp == null)
                {
                    throw new ApplicationException("ARInvoiceLineAction with Code " + MyEntity.Code + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                temp.Name = MyEntity.Name;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
