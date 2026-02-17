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
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class InterfaceManagementListQueryService
    {
        

        private IQueryable<InterfaceManagementList> GetIqueryableList(IQueryable<InterfaceManagement> iQueryable)
        {
            int tenant = 1;
            try
            {
                
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                tenant = authToken.Tenant;
            }
            catch (Exception)
            {

               // throw;
            }
            

            InterfaceTenantDefinitionRepository definitionRep = new InterfaceTenantDefinitionRepository(context);
            InterfaceTenantDefinition definition = null;
            IQueryable<InterfaceTenantDefinition> interfaceManagementDefinitions = definitionRep.GetAll(tenant);


            IQueryable<InterfaceManagementList> query = (from a in iQueryable.Include("InterfaceSendOption").Include("SignatureType")

                                                         join d in interfaceManagementDefinitions.Include("InterfaceSendOption")
                                                         on a.Code equals d.Code into xy
                                                         from s in xy.DefaultIfEmpty()

                                                         select new InterfaceManagementList()
                                                         {
                                                             AllowRestore = a.AllowRestore,

                                                             Code = a.Code,
                                                             Active = a.Active,

                                                             DcaPrefixName = a.DcaPrefixName,
                                                             DefaultPriority = a.DefaultPriority,
                                                             DefaultSendOptionsCode = a.DefaultSendOptionsCode,
                                                             Description = a.Description,
                                                             InOut = a.InOut,

                                                             SearchFields = a.SearchFields,
                                                             DefaultSendOptionName = a.InterfaceSendOption != null ? a.InterfaceSendOption.LocalName : null,
                                                             SendAsDual = a.SendAsDual,
                                                             SignatureTypeCode = a.SignatureTypeCode,
                                                             ResponseInterfaceCode = a.ResponseInterfaceCode,
                                                             DcaPrefixName2 = a.DcaPrefixName2,
                                                             DcaPrefixName3 = a.DcaPrefixName3,
                                                             DcaPrefixName4 = a.DcaPrefixName4,

                                                             TenantPriority = s.TenantPriority,
                                                             TenantSendOptionsCode = s.TenantSendOptionsCode,

                                                             TenantSendOptionName = s.InterfaceSendOption != null ? s.InterfaceSendOption.LocalName : null,

                                                             HasDefinition = s.Id != null ? true : false,


                                                         });
            return query;
        }


        private IQueryable<InterfaceManagementList> GetIqueryableListWithOutTenantDef(IQueryable<InterfaceManagement> iQueryable)
        {
            IQueryable<InterfaceManagementList> query = (from a in iQueryable.Include("InterfaceSendOption")
                                                         select new InterfaceManagementList()
                                                         {
                                                             Code = a.Code,
                                                             Active = a.Active,
                                                             AllowRestore = a.AllowRestore,
                                                             DcaPrefixName = a.DcaPrefixName,
                                                             DefaultPriority = a.DefaultPriority,
                                                             DefaultSendOptionsCode = a.DefaultSendOptionsCode,
                                                             Description = a.Description,
                                                             InOut = a.InOut,

                                                             SearchFields = a.SearchFields,
                                                             DefaultSendOptionName = a.InterfaceSendOption != null ? a.InterfaceSendOption.LocalName : null,
                                                             SendAsDual = a.SendAsDual,
                                                             SignatureTypeCode = a.SignatureTypeCode,
                                                             ResponseInterfaceCode = a.ResponseInterfaceCode,
                                                             DcaPrefixName2 = a.DcaPrefixName2,
                                                             DcaPrefixName3 = a.DcaPrefixName3,
                                                             DcaPrefixName4 = a.DcaPrefixName4,

                                                         });
            return query;
        }

        private IQueryable<InterfaceManagement> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterfaceManagement> iQueryable)
        {
            return iQueryable;
		}
	}


}
	