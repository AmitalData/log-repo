using Logitude.BL.DataContracts;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class EraseTenantDataHelper : BatchTaskExecutionsService
    {
        public EraseTenantDataHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(EraseTenantDataArgs));
            EraseTenantDataArgs parameterArgs = serializer.Deserialize(stringReader) as EraseTenantDataArgs;

            string procedureName = this.GetProcedureName(parameterArgs.Type);

            RunStoredProcedureClass.RunEreaseTenantData(parameterArgs.EntityId, procedureName);
        }

        private string GetProcedureName(string type)
        {
            string procedureName = "";
            switch (type)
            {
                case "B":
                    {
                        procedureName = "dbo.usp_DeleteBusinessRecords";
                        break;
                    }

                case "P":
                    {
                        procedureName = "dbo.usp_DeleteCustomerRecords";
                        break;
                    }

                case "T":
                    {
                        procedureName = "dbo.usp_DeleteTicketsRecords";
                        break;
                    }

                case "C":
                    {
                        procedureName = "dbo.usp_DeleteCRMRecords";
                        break;
                    }
            }

            return procedureName;
        }
    }

    public class EraseTenantDataArgs
    {
        public string Type { get; set; }
        public int EntityId { get; set; }
    }
}