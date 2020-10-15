using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
    public partial class ObjectTableQueryService
    {

        IWebFreightContext context;
        //ObjectTableService service; 

        ObjectTableQuery query;

        public ObjectTableQueryService(int tenant)
        {
            context = WebFreightContext.GetContext(tenant);
            //service = new ObjectTableService(context, tenant); 
            query = new ObjectTableQuery(tenant);
        }


        public ObjectTable GetObjectTableById(string Id, int Tenant)
        {
            try
            {


                var temp = query.GetSinglePM(Id, Tenant);
                return ObjectTableDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ObjectTable ObjectTableDataMapping(ObjectTablePM MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var temp = new ObjectTable();
                temp.Id = MyEntityPM.Id;
                temp.Name = MyEntityPM.Name;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ObjectTablePM ObjectTableDataMappingAndValidatin(ObjectTable MyEntity, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {
                var temp = new ObjectTablePM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                if (!string.IsNullOrEmpty(MyEntity.Name))
                {
                    temp = query.GetObjectTableByName(MyEntity.Name, Tenant);
                }
                if (temp == null)
                {
                    throw new ApplicationException("ObjectTable with Name " + MyEntity.Name + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Name))
                {
                    temp.Name = MyEntity.Name;
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