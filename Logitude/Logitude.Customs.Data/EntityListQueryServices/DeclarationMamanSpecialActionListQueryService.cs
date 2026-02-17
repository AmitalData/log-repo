	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class DeclarationMamanSpecialActionListQueryService
    {
	    private IQueryable<DeclarationMamanSpecialActionList> GetIqueryableList(IQueryable<DeclarationMamanSpecialAction> iQueryable)
        {
		IQueryable<DeclarationMamanSpecialActionList> query = (from a in iQueryable
                                            select new DeclarationMamanSpecialActionList()
											{
					                          DeclarationId = a.DeclarationId,
					                          Tenant = a.Tenant,				
					                          MamanSpecialActionCode = a.MamanSpecialActionCode,
					                          MamanSpecialActionName = a.MamanSpecialAction != null ? a.MamanSpecialAction.LocalName : null,
                                              MamanLabelText1 = a.MamanLabelText1,
					                          MamanLabelText2 = a.MamanLabelText2,
					                          MamanLabelText3 = a.MamanLabelText3,
					                          MamanLabelText4 = a.MamanLabelText4,
					                          MamanLabelText5 = a.MamanLabelText5,
					                          MamanSpecialActionStatusCode = a.MamanSpecialActionStatusCode,
                                              MamanSpecialActionStatusName = a.MamanSpecialActionStatus != null ? a.MamanSpecialActionStatus.LocalName : null,
                                              MamanSpecialActionsErrorXml = a.MamanSpecialActionsErrorXml,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationMamanSpecialAction> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationMamanSpecialAction> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	