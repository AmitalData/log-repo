using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class InterfaceManagementQueryService : EntityQueryService<InterfaceManagement, InterfaceManagementKeys, InterfaceManagementPM, object, InterfaceManagementKeys>
    {
        

        public List<InterfaceManagementList> GetInterfaceManagementwithDefinition(int tenant)
        {

            InterfaceTenantDefinitionRepository definitionRep = new InterfaceTenantDefinitionRepository(context);
            InterfaceTenantDefinition definition = null;
            IQueryable<InterfaceTenantDefinition> interfaceManagementDefinitions = definitionRep.GetAll(tenant);

            List<InterfaceManagementList> managements = (from a in context.InterfaceManagements.Include("InterfaceSendOption").Include("SignatureType")
                                                         join d in interfaceManagementDefinitions.Include("InterfaceSendOption")
                                                         on a.Code equals d.Code into xy
                                                         from s in xy.DefaultIfEmpty()
                                                         select new InterfaceManagementList()
                                                         {
                                                             AllowRestore = a.AllowRestore,
                                                             Code = a.Code,
                                                             Active = a.Active,
                                                             DcaPrefixName = a.DcaPrefixName,
                                                             DcaPrefixName2 = a.DcaPrefixName2,
                                                             DcaPrefixName3 = a.DcaPrefixName3,
                                                             DcaPrefixName4 = a.DcaPrefixName4,
                                                             DefaultPriority = a.DefaultPriority,
                                                             DefaultSendOptionsCode = a.DefaultSendOptionsCode,
                                                             Description = a.Description,
                                                             InOut = a.InOut,
                                                            
                                                             TenantPriority = s.TenantPriority,
                                                             TenantSendOptionsCode = s.TenantSendOptionsCode,
                                                             SearchFields = a.SearchFields,
                                                             TenantSendOptionName = s.InterfaceSendOption != null ? s.InterfaceSendOption.LocalName : null,
                                                             DefaultSendOptionName = a.InterfaceSendOption != null ? a.InterfaceSendOption.LocalName : null,
                                                             HasDefinition = s.Id != null ? true : false,
                                                             SignatureTypeCode = a.SignatureTypeCode,
                                                             SignatureTypeName = a.SignatureType != null? a.SignatureType.LocalName : null,

                                                         }).ToList();

            //foreach (InterfaceManagementList item in managements)
            //{
            //    definition = definitionRep.GetSingleDefinitionByCode(item.Code, tenant);
            //    if (definition != null)
            //    {
            //        item.HasDefinition = true;
            //    }
            //}

            return managements;


        }


        public InterfaceManagementPM GetSingleInterfaceManagementwithDefinition(string code, int tenant)
        {
            InterfaceManagementPM interfaceManagement = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                InterfaceManagement management = repository.GetSingleInterfaceManagement(new InterfaceManagementKeys() { Code = code });
                InterfaceTenantDefinitionRepository definitionRepository = new InterfaceTenantDefinitionRepository(context);
                InterfaceTenantDefinition definition = definitionRepository.GetSingleDefinitionByCode(code, tenant);
                if (management != null)
                {
                    interfaceManagement = new InterfaceManagementPM()
                                                     {
                                                         Code = management.Code,
                                                         AllowRestore = management.AllowRestore,
                                                         Active = management.Active,
                                                         DcaPrefixName = management.DcaPrefixName,
                                                         DcaPrefixName2 = management.DcaPrefixName2,
                                                         DcaPrefixName3 = management.DcaPrefixName3,
                                                         DcaPrefixName4 = management.DcaPrefixName4,
                                                         DefaultPriority = management.DefaultPriority,
                                                         DefaultSendOptionsCode = management.DefaultSendOptionsCode,
                                                         Description = management.Description,
                                                         InOut = management.InOut,
                                                        SignatureTypeCode = management.SignatureTypeCode,
                                                         DefaultSendOptionName = management.InterfaceSendOption != null ? management.InterfaceSendOption.LocalName :null,
                                                         SearchFields = management.SearchFields,
                                                         InterfaceType = management.InterfaceType,
                                                         Tenant = tenant,
                                                         UseRabbitMQ= management.UseRabbitMQ
                                                     };
                    if (definition != null)
                    {
                        interfaceManagement.TenantPriority = definition.TenantPriority;
                        interfaceManagement.TenantSendOptionsCode = definition.TenantSendOptionsCode;
                        interfaceManagement.TenantSendOptionName = definition.InterfaceSendOption != null ? definition.InterfaceSendOption.LocalName : null;

                        interfaceManagement.DcaRenameFileEnable = definition.DcaRenameFileEnable;
                        interfaceManagement.DcaRenameFilePrefix = definition.DcaRenameFilePrefix;


                    }
                }
            }
            return interfaceManagement;
        }

        public InterfaceManagementPM GetOutInterfaceType(string currentDCAInInterfaceTypeCode)
        {


#if true
            //List<InterfaceManagement> list=null;
            var poco = this.repository.GetAll().FirstOrDefault(rec => rec.ResponseInterfaceCode == currentDCAInInterfaceTypeCode);
            var newpm =this.GetEntityPM(poco);
            return newpm;
#else
            switch (currentDCAInInterfaceTypeCode)
            {
                case "103":
                    {
                        var pocos = repository.GetAll().Where(rec => rec.Code == "101").ToList();
                        if (pocos == null)
                        {
                            return null;
                        }
                        var poco = pocos.FirstOrDefault();
                        if (poco == null)
                        {
                            return null;
                        }
                        var newpm = new InterfaceManagementPM();
                        mapping.POCOToPM(newpm, poco);
                        return newpm;
                    }
                    break;
                case "190":
                    return null;
                case "3053":
                    {
                        var pocos = repository.GetAll().Where(rec => rec.Code == "3050").ToList();
                        if (pocos == null)
                        {
                            return null;
                        }
                        var poco = pocos.FirstOrDefault();
                        if (poco == null)
                        {
                            return null;
                        }
                        var newpm = new InterfaceManagementPM();
                        mapping.POCOToPM(newpm, poco);
                        return newpm;
                    }
                    break;
                default:
                    throw new Exception("Exept ony 1 item , GetOutInterfaceType(string currentDCAInInterfaceTypeCode):" + currentDCAInInterfaceTypeCode);
                    break;
            }
#endif


        }
        public InterfaceManagementPM GetCodeByDcaPrefixName(string DcaPrefixName)
        {
            InterfaceManagement poco=null;
            if (!string.IsNullOrWhiteSpace(DcaPrefixName))
            {


                poco = this.repository.GetAll().FirstOrDefault(rec =>
                    rec.DcaPrefixName == DcaPrefixName ||
                    rec.DcaPrefixName2 == DcaPrefixName ||
                    rec.DcaPrefixName3 == DcaPrefixName ||
                    rec.DcaPrefixName4 == DcaPrefixName
                    );
            }
            var newpm = this.GetEntityPM(poco);
            return newpm;
        }

        public List<InterfaceManagementPM> GetAll()
        {

            //mapping = new InterfaceTypeDataMapping();

            //var allPoco = repository.GetAll().ToList();
            
            var allPM = new List<InterfaceManagementPM>();
            allPM = repository.GetAll().ToList().Select(poco => GetEntityPM(poco)).ToList();

            return allPM;
        }
    }
}
