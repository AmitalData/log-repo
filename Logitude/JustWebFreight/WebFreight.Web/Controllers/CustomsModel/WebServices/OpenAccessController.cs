using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class OpenAccessController : ApiController
    {
        public HttpResponseMessage GetQuickSearch(string ObjectTableName, string SearchFields,int tenant, 
            int PageIndex,int PageSize)
        {
            try
            {
                QueryOperations myQueryOperations = new QueryOperations();
                myQueryOperations.PageIndex = PageIndex;
                myQueryOperations.PageSize = PageSize;

                if (!string.IsNullOrEmpty(SearchFields))
                {
                    myQueryOperations.SetFilter("SearchFields", SearchFields, false, "Contains", null, false);
                }
                FilterSerializer serializer = new FilterSerializer();
                byte[] arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                ICustomContext ctx = CustomContext.GetContext(tenant);

                switch (ObjectTableName)
                {
                    //http://localhost:9996/api/OpenAccess/GetQuickSearch?ObjectTableName=Customs.CustomsItems&SearchFields=-00740&Tenant=1&PageIndex=0&PageSize=10
                    case "Customs.CustomsItems":
                        {
                            
                            CustomsItemListQueryService query = new CustomsItemListQueryService(ctx);


                            myQueryOperations.SetFilter("FullClassification", SearchFields, false, "Contains", null, false);
                            myQueryOperations.SetFilter("CustomsItemCategoryID", "2,3", false, "InListInt", null, false);

                            List<CustomsItemList> myResult = query.GetList(myQueryOperations, tenant);

                            return Request.CreateResponse(HttpStatusCode.OK, myResult);
                        }
                        break;
                    default:
                        throw new Exception($"ObjectTableName={ObjectTableName} unControl");
                        break;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}