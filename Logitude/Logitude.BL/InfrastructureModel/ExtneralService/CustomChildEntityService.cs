//using Logitude.BL.InfrastructureModel.EntityQueries;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Logitude.BL.InfrastructureModel.ExtneralService
//{
//   public class CustomChildEntityService
//    {
//        private CustomChildEntityArgs customChildEntityArgs;
//        public CustomChildEntityService(CustomChildEntityArgs customChildEntityArgs)
//        {
//            this.customChildEntityArgs = customChildEntityArgs;

//        }
//        public void Update()
//        {
//            List<CustomChildEntityPM> customChildEntities = GetCustomChildEntities();
//            //var objectTableId = new ObjectTableQuery(customChildEntityArgs.Tenant).GetObjectTableIdByName(customChildEntityArgs.ParentObjectTableName);
//            //if (string.IsNullOrEmpty(objectTableId)) return;

//        }

//        private List<CustomChildEntityPM> GetCustomChildEntities()
//        {
//            throw new NotImplementedException();
//        }
//    }


//    public class CustomChildEntityArgs
//    {
//        public int Tenant { get; set; }

//        public string ParentEntityId { get; set; }

//        public string ParentObjectTableName { get; set; }

//        public object ParentEntity { get; set; }

//    }

//}
