using Logitude.Customs.Def.ClosedTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public class InterfaceTenantDefinitionManagementPM : InterfaceTenantDefinitionPM
    {
        private InterfaceTenantDefinitionManagementPM() { }

        public InterfaceTenantDefinitionManagementPM(int tenant, InterfaceTenantDefinitionPM baseEntityPM, InterfaceManagementPM interfaceManagement)
        {
            if (interfaceManagement == null)
            {
                throw new Exception("interfaceManagement==null");
            }

            

            
            this.InterfaceManagement = interfaceManagement;
            this.Tenant = tenant;

            this.Code = InterfaceManagement.Code;
            if (baseEntityPM == null)
            {
                Tenant = tenant;
                Code = InterfaceManagement.Code;
                return;
            }
            this.Id = baseEntityPM.Id;
            this.TenantSendOptionsCode = baseEntityPM.TenantSendOptionsCode;

            this.TenantPriority = baseEntityPM.TenantPriority;

            this.Active = baseEntityPM.Active;
            this.DcaRenameFileEnable = baseEntityPM.DcaRenameFileEnable;
            this.DcaRenameFilePrefix = baseEntityPM.DcaRenameFilePrefix;

            //this.InActive = baseEntityPM.InActive;


        }

        public InterfaceManagementPM InterfaceManagement { get; private set; }



        public InteractiveMode Interactive
        {
            get
            {
                if (!HaveInterfaceManagement())
                {
                    throw new Exception("Please Use InterfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition() to retrieve InterfaceManagement !!");
                }
                
                //this.OverrideTenantSendOptionsCode = this.OverrideTenantSendOptionsCode ?? "";//mohammad :this code must change to fit the new adjustments to interface,i put OverrideTenantSendOptionsCode instead of SendOptionsCode
                if (InterfaceManagement.INOUT == InOutType.Out)
                {
                    //InterfaceSendOptions
                    //D	DCA Out
                    //WB	Batch WS
                    //WI	Interactive WS


                    switch (this.OverrideInterfaceSendOption)
                    {
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D:
                            return InteractiveMode.DCABatchOutIn;
                            break;
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WB:
                            return InteractiveMode.WebServiceBatch;
                            break;
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI:
                        default:
                            return InteractiveMode.WebServiceInteractive;
                            break;
                    }
                    
                }
                else if (InterfaceManagement.INOUT == InOutType.In)
                {
                    switch (this.OverrideInterfaceSendOption)
                    {
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D:
                            return InteractiveMode.DCABatchIn;
                            break;
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WB:
                        case InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI:
                        default:
                            return InteractiveMode.WebServiceBatch;
                            break;
                    }
                }
                return InteractiveMode.WebServiceInteractive;
            }
        }


        bool IsDummy()
        {
            return String.IsNullOrWhiteSpace(this.Id);
        }
        bool HaveInterfaceManagement()
        {
            return InterfaceManagement != null;
        }

        public InterfaceSendOptionsDetails.InterfaceSendOptionEnum OverrideInterfaceSendOption
        {
            get
            {
                string val = "";
                if (!HaveInterfaceManagement())
                {
                    throw new Exception("Please Use InterfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition() to retrieve InterfaceManagement !!");
                }
                if (IsDummy())
                {

                    val = InterfaceManagement.DefaultSendOptionsCode;
                }
                else
                {
                    val = this.TenantSendOptionsCode;
                }
                if (string.IsNullOrWhiteSpace(val))
                {
                    if (InterfaceManagement.INOUT == InOutType.In)
                    {
                        return InterfaceSendOptionsDetails.InterfaceSendOptionEnum.D;
                    }
                }
                InterfaceSendOptionsDetails.InterfaceSendOptionEnum interfaceSendOptionEnum;
                if (Enum.TryParse<InterfaceSendOptionsDetails.InterfaceSendOptionEnum>(val, out interfaceSendOptionEnum))
                {
                    return interfaceSendOptionEnum;
                }
                
                return InterfaceSendOptionsDetails.InterfaceSendOptionEnum.WI;
            }
        }


        public int OverrideTenantPriority
        {
            get
            {
                if (!HaveInterfaceManagement())
                {
                    throw new Exception("Please Use InterfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition() to retrieve InterfaceManagement !!");
                }
                if (IsDummy())
                {
                    return InterfaceManagement.DefaultPriority ?? 5;
                }
                return this.TenantPriority ?? 5;
            }
        }

        public bool OverrideActive
        {
            get
            {
                if (!HaveInterfaceManagement())
                {
                    throw new Exception("Please Use InterfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition() to retrieve InterfaceManagement !!");
                }
                if (IsDummy())
                {
                    return InterfaceManagement.Active;
                }
                return this.Active;
            }
        }

        //public bool OverrideInActive
        //{
        //    get
        //    {
        //        if (!HaveInterfaceManagement())
        //        {
        //            throw new Exception("Please Use InterfaceTenantDefinitionQueryService.GetWithInterfaceManagementDefinition() to retrieve InterfaceManagement !!");
        //        }
        //        if (IsDummy())
        //        {
        //            return false;
        //        }
        //        return this.InActive;
        //    }
        //}

    }
}

    

