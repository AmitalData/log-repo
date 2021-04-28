using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ObjectFieldPropertyGetter : IObjectFieldPropertyGetter
    {
        Dictionary<string, string> fieldDataTypes;
        public ObjectFieldPropertyGetter()
        {
            FillFieldDataTypes();
        }

        private void FillFieldDataTypes()
        {
            fieldDataTypes = new Dictionary<string, string>();
            fieldDataTypes.Add("Integer", "System.Int32");
            fieldDataTypes.Add("BigInteger", "System.Int64");
            fieldDataTypes.Add("Text", "System.String");
            fieldDataTypes.Add("nText", "System.String");
            fieldDataTypes.Add("LookUp", "System.String");
        }

        public string GetObjectFieldType(string fieldName, string objectTableName, int tenant)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(objectTableName, tenant, true);
           
            ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(0);
            var objectfield = objectFieldRepository.GetObjectFieldByName(fieldName, objectTable.Id, tenant);

            return fieldDataTypes[objectfield.DataTypeCode];
        }
    }
}
