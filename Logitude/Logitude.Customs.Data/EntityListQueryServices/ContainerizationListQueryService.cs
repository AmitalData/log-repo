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

    public partial class ContainerizationListQueryService
    {
	    private IQueryable<ContainerizationList> GetIqueryableList(IQueryable<Containerization> iQueryable)
        {


			var declarations =
	   from dec in context.Declarations.Include("Card")
	   group dec by new { dec.ExportFile, dec.ExportContainerizationID , dec.TransportModeId , ImporterName = dec.CustomerCard.LocalName}
			into newgroup
	   select newgroup;// new Declaration {ExportFile = newgroup.Key.ExportFile };


			IQueryable<ContainerizationList> query = (from a in iQueryable.Include("ContainerizationStatusCode").Include("DeclarationStatusType")
													  join d in declarations
													  on a.Id equals d.Key.ExportContainerizationID into EmpCont
													  from ed in EmpCont.DefaultIfEmpty()
						


													  select new ContainerizationList()
													  {

														  Id = a.Id,

														  Tenant = a.Tenant,

														  SearchFields = a.SearchFields,

														  AgentDeclaration = a.AgentDeclaration,

														  ContainerizationDate = a.ContainerizationDate,

														  ContainerizationNumber = a.ContainerizationNumber,

														  ContainerizationStatus = a.ContainerizationStatusCode.Code,

														  HataraStatus = a.HataraStatus,

														  OperationMode = a.OperationMode,

														  ContainerizationStatusName = a.ContainerizationStatusCode != null ? a.ContainerizationStatusCode.Name :null,
											              ExportFile = ed.Key.ExportFile,
														  HataraStatusName = a.DeclarationStatusType != null? a.DeclarationStatusType.LocalName:null,
														  ImporterName = ed.Key.ImporterName,
														  TransportModeForExport = ed.Key.TransportModeId ,
														  HataraStatusIsNull = a.HataraStatus != null ? false :true
													  }); ;
            return query;
		}

		private IQueryable<Containerization> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Containerization> iQueryable, int tenant)
        {
			return iQueryable;
		}
			}


}
	