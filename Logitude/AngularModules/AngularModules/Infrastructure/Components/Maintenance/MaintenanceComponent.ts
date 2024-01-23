declare var window: any;
import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import { AppTool, DateTool} from '../../../Infrastructure/Tools';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
//import {CustomsSettingExtendedListService} from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import { DeclarationRemarksService } from '../../../Common/Services/ExtendedPMs/DeclarationRemarksService';
import { DeclarationRemarks } from '../../../Customs/EntityPMs/Extended/DeclarationRemarks';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { AmitalGatewayUtil } from '../../Utilities/AmitalGatewayUtil';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
//import {RecallClientsForCutoms} from '../../../Customs/Components/CustomsRequests/GeneralRequests/RecallClientsForCutoms';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { CustomizationPermissionService } from '../../../InfrastructureModules/InfrastructureCustomization/ExternalService/CustomizationPermissionService';
import { HostScreenService } from 'Common/Components/HostScreen/HostScreenService';
import { CustomsCloudComponentArgs } from 'InfrastructureModules/InfrastructureOthers/Components/CustomsCloud/CustomsCloudComponent';
import { HomeScreenEvent, HomeScreenEventTypes, HostScreenComponent } from 'Common/Components/HostScreen/HostScreenComponent';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
    

    templateUrl: './MaintenanceComponent.html',
})

export class MaintenanceComponent {
    public ItemsSource: MaintenanceMenuItem[];
    LayoutDirection: string = 'ltr';
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    private textCodeTranslationPipe: TextCodeTranslationPipe;
    private readonly invoiceConfirmationNumber = "InvoiceConfirmationNumber";

    public EntityStatusToggle: boolean = false;
    constructor() {
        this.SetEntityStatusToggle();
        this.ItemsSource = [];
        this.BuildPagesMenu();
        this.BuildMaintenanceMenu();
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.textCodeTranslationPipe = new TextCodeTranslationPipe();
    }


    private SetEntityStatusToggle() {
        let entityStatusFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EST")[0];
        if (entityStatusFeatureToggle) {
            this.EntityStatusToggle = true;

        }
    }
    private CustomObjectTables: ObjectTablePM[] = window.ObjectTables.filter(obj => (obj.ParentObjectTableId == null) && (obj.IsCustom == true));
    private IsCustomObjectsAdded(): boolean {
        if (this.CustomObjectTables.length > 0)
            return true;
        return false;
    }

    // Pages Menu
    public PagesMenu: Menu[];
    public SelectedMenu: Menu;
    private isTransmissionsPageVisible: boolean = false;
    private BuildPagesMenu() {
        this.PagesMenu = [];

        this.PagesMenu.push(new Menu("PAR", TextCodeTranslator.Translate("General.MC.Partners.Partners")));
        this.PagesMenu.push(new Menu("BIL", TextCodeTranslator.Translate("General.MC.Billings.Billings")));
        this.PagesMenu.push(new Menu("LOC", TextCodeTranslator.Translate("General.MC.Locations.Locations")));
        this.PagesMenu.push(new Menu("OTH", TextCodeTranslator.Translate("General.MC.Others.Others")));
        this.PagesMenu.push(new Menu("PRS", TextCodeTranslator.Translate("General.MC.PersonalSettings.PersonalSettings")));
        this.PagesMenu.push(new Menu("CMS", TextCodeTranslator.Translate("General.MC.SystemSettings.SystemSettings")));
         
        if (FeatureLocator.HasFeaturePermession("General", this.invoiceConfirmationNumber))
            this.PagesMenu.push(new Menu("SHA", TextCodeTranslator.Translate("General.MC.ShaamTokenManagement")));
        
        if (SessionLocator.Tenant == 0) {
            this.PagesMenu.push(new Menu("MNG", TextCodeTranslator.Translate("General.MC.Management.Management")));
        }

        if (FeatureLocator.HasFeaturePermession("General", "LEADSOURCES") ||
            FeatureLocator.HasFeaturePermession("General", "STAGES") ||
            FeatureLocator.HasFeaturePermession("General", "ADDITIONALSERVICES") ||
            FeatureLocator.HasFeaturePermession("General", "CLOSINGREASONS") ||
            FeatureLocator.HasFeaturePermession("General", "COMPETITORS") ||
            FeatureLocator.HasFeaturePermession("General", "OPPORTUNITYTYPES") ||
            FeatureLocator.HasFeaturePermession("General", "INDUSTRIES") ||
            FeatureLocator.HasFeaturePermession("General", "PRODUCTTYPES") ||
            FeatureLocator.HasFeaturePermession("General", "EMAILALERTSETTINGS")) {
            this.PagesMenu.push(new Menu("CRM", TextCodeTranslator.Translate("General.MC.CRM.CRM")));
        }

        if (FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            this.PagesMenu.push(new Menu("TKT", "Tickets"));
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Occasion.OccasionType")) {
            this.PagesMenu.push(new Menu("OCS", "Occasions"));
        }

        if (FeatureLocator.HasFeaturePermession("CustomsGeneral", "CUSTOMS")) {
            this.PagesMenu.push(new Menu("CSM", TextCodeTranslator.Translate("General.MC.Custom.Customs")));
        }

        if (FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGS")) {
            this.PagesMenu.push(new Menu("ACC", TextCodeTranslator.Translate("General.MC.Accounting.Accounting")));
        }

        if (FeatureLocator.HasFeaturePermession("General", "QUOTES")) {
            this.PagesMenu.push(new Menu("QUO", "Quotation"));
        }

        this.isTransmissionsPageVisible = false;
        if (SessionInfo.LoggedUserPM.IsCustomerCare && FeatureLocator.HasFeaturePermession("General", "General.Features.InttraSettings")) {
            this.isTransmissionsPageVisible = true;
        }

        else if (SessionLocator.Tenant == 0) {
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.InttraCommunicationSettings")) {
                this.isTransmissionsPageVisible = true;
            }
        }

       if (this.isTransmissionsPageVisible) {
            this.PagesMenu.push(new Menu("TRANS", "Transmissions"));
        }
  

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessQueue") || FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessTeam") ||
            FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessBusinessRole")) {
            this.PagesMenu.push(new Menu("BUP", TextCodeTranslator.Translate("General.MC.BusinessProcess")));
        }

        //check if want to connect to feature
        this.PagesMenu.push(new Menu("CUS", TextCodeTranslator.Translate("General.MC.Customization.Customization")));

        if (this.IsCustomObjectsAdded()) {
            this.PagesMenu.push(new Menu("CSO", TextCodeTranslator.Translate("General.MC.CustomObjects.CustomObjects")));
        }

        if (FeatureLocator.HasFeaturePermession("General", "TASKTYPES") ||
            FeatureLocator.HasFeaturePermession("General", "TASKPRIORITIES") ||
            FeatureLocator.HasFeaturePermession("General", "TASKSTATUSES")) {
            this.PagesMenu.push(new Menu("TSK", TextCodeTranslator.Translate("General.MC.Tasks.Tasks")));
        }
    }
    
    // Maintenance Menu
    private AllMaintenanceMenu: MaintenanceMenuItem[];
    private BuildMaintenanceMenu() {
        this.AllMaintenanceMenu = [];
        var allMenusTables: MenusTablePM[] = window.MenusTables.filter(x => x.MenuTypeCode === "MTC").sort((a, b) => { return a.IndexOfOrder - b.IndexOfOrder });

        allMenusTables.forEach(item => {
 
            if (FeatureLocator.IsFeatureGrantedByUniqeCode(item.FeatureUniqeCode)) {

                if (item.Code == "MTCB") {
                    //CustomsSettingList customsSetting = DataProvider.GetCachedList<CustomsSettingList>("Customs.CustomsSetting").FirstOrDefault();
                    //if (customsSetting != null) {
                    //    if (!customsSetting.IsConnectedToUniFreight) {
                    //        mainList.Add(new MenusTableViewModel(viewInjectionService, eventAggregator, regionManager, container, menu));
                    //    }
                    //}
                }

                else if (item.Code == "MTHR") {
                    if (SessionLocator.Tenant == 0) {
                        if (FeatureLocator.HasFeaturePermession("HelpResource", "HelpResource.M.HelpResources")) {
                            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                        }
                    }
                }

                else if (item.Code == "MTRP") {
                    if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor) {
                        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                    }
                }
                 
                else if (item.Code == "MTCO") {
                    this.PushEntityStatusMenu(item);
                }

                else {
                    if (item.Code != "MTHT" && item.Code != "POGP") {
                        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                    }
                }
            }

            if (item.Code == "MTHT" || item.Code == "POGP") {
                if (SessionLocator.Tenant == 0) {
                    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                }
            }

            else if (item.Code == "DEPA" && this.CheckDeploymentPackageFeatures()) {
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        });

        this.BuildPersonalSettings();
        this.BuildSystemSettings();
        this.BuildCustomsMenus();
        this.BuildAccountingMenus();
        this.BuildOtherMenus();
        this.BuildTransmissionsMenus();
        this.BuildCustomizationMenus();
        this.BuildCustomObjectsMenus();
        this.BuildShaamTokenManagementMenu();
        this.PageChanged(this.PagesMenu[0]);
    }
    CheckDeploymentPackageFeatures() {
        if (FeatureLocator.IsFeatureGrantedByUniqeCode("General.Customization.DeploymentPackage"))
            return false;
        return CustomizationPermissionService.HasFeaturePermession("General", "Customization.DeploymentPackage");
    }
    private PushEntityStatusMenu(item: MenusTablePM) {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || this.EntityStatusToggle) {
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
    }

    private BuildSystemSettings() {
        if (FeatureLocator.HasFeaturePermession("General", this.invoiceConfirmationNumber)) {
            var item2 = new MenusTablePM();
            item2.CategoryTypeCode = "CMS";
            item2.Icon = "Settings"
            item2.Code = "CUSC";
            item2.ObjectTableName = "Customs Cloud";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item2));
        }

        if (FeatureLocator.HasFeaturePermession("General", "TERMOFUSERFEATUE")) {
            var item2 = new MenusTablePM();
            item2.CategoryTypeCode = "CMS";
            item2.Icon = "Settings"
            item2.Code = "TOUS";
            item2.ObjectTableName = "Terms of Use Signature";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item2));
        }
        this.AddTermsOfUseMenuItem();

        if (FeatureLocator.HasFeaturePermession("General", "SYSTEMSETTINGS")) {

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "COAD";
                item.ObjectTableName = "Company Address Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.TenantAdditionalData")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "PAGD";
                item.ObjectTableName = "Payment Gateway Definition";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (SessionLocator.Tenant == 0) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "Private Labels";
                item.ObjectTableName = "TenantManagment Private Labels";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "TenantManagmentPrivateLabels")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "CODE";
                item.ObjectTableName = "System Defaults";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (SessionLocator.TenantPM.IsDocumentsArchive) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "LBSE";
                item.ObjectTableName = "LogBox Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "COCO";
                item.ObjectTableName = "Counters";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "COLO";
                item.ObjectTableName = "Company Logo";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "SYCR";
                item.ObjectTableName = "System Currencies";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "ACSE";
                item.ObjectTableName = "Accounting Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "LOSE";
                item.ObjectTableName = "Local Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "INVS";
                item.ObjectTableName = "Invoice Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.AirlineSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "AIRS";
                item.ObjectTableName = "Airline Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.CustomerActivation")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "CUSA";
                item.ObjectTableName = "Customer Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.VATSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "VATS";
                item.ObjectTableName = "VAT Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.SupportManagement")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "SUMN";
                item.ObjectTableName = "Support Management";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.OceanInsightsSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "OISG";
                item.ObjectTableName = "Ocean Insights Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "CONTAINERTRACKINGSETTINGS") && SessionLocator.Tenant == 0) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "CTPS";
                item.ObjectTableName = "ContainerTrackingProvider";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "ContainerTrackingProvider")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemUserPassword")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "SUPW";
                item.ObjectTableName = "System User Password";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.HybridTenantThreshold")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "HYTT";
                item.ObjectTableName = "Hybrid Tenant Threshold";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.Automations")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "AUTO";
                item.ObjectTableName = "Automations";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.TenantManagement")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List"
                item.Code = "TEMG";
                item.ObjectTableName = "TenantManagement";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "TenantManagement")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (SessionLocator.Tenant == 0 && FeatureLocator.HasFeaturePermession("General", "General.Features.BluesnapContract")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List"
                item.Code = "BCMT";
                item.ObjectTableName = "BluesnapContract";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "BluesnapContract")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "ApiCredintials")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List"
                item.Code = "APIC";
                item.ObjectTableName = "ApiCredintials";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "ApiCredintials")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("WebhookKeys", "WebhookKeys")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List"
                item.Code = "WHKS";
                item.ObjectTableName = "WebhookKeys";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "WebhookKeys")[0]?window.ObjectTables.filter(d => d.Name == "WebhookKeys")[0].Id:null;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("APILogs", "APILogs")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List"
                item.Code = "APLG";
                item.ObjectTableName = "APILogs";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "APILogs")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.CustomerFieldsUpdateSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "CFUS";
                item.ObjectTableName = "CustomerFieldsUpdateSetting";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "CustomerFieldsUpdateSetting")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.SecurityPolicySettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "SECP";
                item.ObjectTableName = "Login Policy";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.QuoteSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "QUO";
                item.Icon = "Settings"
                item.Code = "QuoteSettings";
                item.ObjectTableName = "Quote Settings";
                item.IndexOfOrder = 5;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.ContainerSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "ContainerSettings";
                item.ObjectTableName = "Container Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            var item1 = new MenusTablePM();
            item1.CategoryTypeCode = "CMS";
            item1.Icon = "Settings"
            item1.Code = "SYIN";
            item1.ObjectTableName = "System Info";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item1));

            var item3 = new MenusTablePM();
            item3.CategoryTypeCode = "CMS";
            item3.Icon = "List"
            item3.Code = "LOGS";
            item3.ObjectTableName = "CommunicationLog";
            item3.ObjectTableId = window.ObjectTables.filter(d => d.Name == "CommunicationLog")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item3));

            if (FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module")) {
                var item4 = new MenusTablePM();
                item4.CategoryTypeCode = "CMS";
                item4.Icon = "Settings"
                item4.Code = "CRLM";
                item4.ObjectTableName = "Credit Limit Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item4));
            }

            if (FeatureLocator.HasFeaturePermession("General", "CUSTOMSINTERFACESETTINGS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "CISE";
                item.ObjectTableName = "Customs Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.DocumentFilingEmailSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "DFES";
                item.ObjectTableName = "Document Filing Email Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "TICKET") && FeatureLocator.HasFeaturePermession("General", "TicketsSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "TKT";
                item.Icon = "Settings"
                item.Code = "MTSE";
                item.ObjectTableName = "Ticket Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "SupportMailBoxMenu")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "TKT";
                item.Icon = "Settings"
                item.Code = "SUPM";
                item.ObjectTableName = "Support Mail Boxes";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (SessionInfo.LoggedUserPM.IsCustomerCare && SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "VIP")[0] != null) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings"
                item.Code = "VIZN";
                item.ObjectTableName = "Container Tracking - Pilot Customer";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        }
    }
    private AddTermsOfUseMenuItem() {
        if (SessionLocator.Tenant != 0 || !SessionLocator.LoggedUserPM.IsCustomerCare) return;
        let menusTablePM = new MenusTablePM();
        menusTablePM.CategoryTypeCode = "CMS";
        menusTablePM.Icon = "Settings";
        menusTablePM.Code = "TOU";
        menusTablePM.ObjectTableName = "Terms of Use";
        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(menusTablePM));
    }

   
    private BuildPersonalSettings() {
        if (FeatureLocator.HasFeaturePermession("General", "PERSONALSETTINGS")) {

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "PRS";
                item.Icon = "Settings"
                item.Code = "SIGN";
                item.ObjectTableName = "Signature";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "PRS";
                item.Icon = "Settings"
                item.Code = "CHPA";
                item.ObjectTableName = "Change Password";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        }
    }
    private BuildCustomsMenus() {

        if (window.ObjectTables.filter(d => d.Name == "Customs.CustomsRequiredField")[0] != null) {

            if (FeatureLocator.HasFeaturePermession("Customs.CustomsRequiredField", "CSTMREQFIELDMTC")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CSM";
                item.Icon = "Settings"
                item.Code = "REFI";
                item.ObjectTableName = "Customs.CustomsRequiredField";
                item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Customs.CustomsRequiredField")[0].Id
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

        }

        let yaronWantWithoutLogiUpdate = false;// in "customs" branch do not use it !!

        if (yaronWantWithoutLogiUpdate ||
            (SessionLocator.Tenant == 0 && !SessionLocator.LoggedUserPM.IsDistributor && window.ObjectTables.filter(d => d.Name == "Customs.CustomsSetting")[0] != null)
        ) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CSM";
            item.Icon = "Settings"
            item.Code = "CSMN";
            item.ObjectTableName = "Customs.CustomsSetting";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Customs.CustomsSetting")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }

        //let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
        //LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
        //let allowed = false;
        //allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."));

        if (SessionLocator.LoggedUserPM.Email.includes("amital"))             
         {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CSM";
            item.Icon = "Settings"
             item.Code = "CSRA";
            item.ObjectTableName = TextCodeTranslator.Translate("General.MC.Customs.ReAnalysis") ;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

        }

    }
    private BuildAccountingMenus() {

        if (FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGS")) {

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.FullAccountingSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "ACC";
                item.Icon = "Settings"
                item.Code = "FACS";
                item.ObjectTableName = "Full Accounting Setting";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.ChartOfAccountsTypesOrder")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "ACC";
                item.Icon = "Settings"
                item.Code = "COATO";
                item.ObjectTableName = "Chart Of Accounts Types Order";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            //if (FeatureLocator.HasFeaturePermession("General", "General.Features.YearTransfer")) {
            //    var item = new MenusTablePM();
            //    item.CategoryTypeCode = "ACC";
            //    item.Icon = "Settings"
            //    item.Code = "ACYT";
            //    item.ObjectTableName = "Year Transfer";
            //    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            //}
            //if (FeatureLocator.HasFeaturePermession("General", "General.Features.AccountingPeriods")) {
            //    var item = new MenusTablePM();
            //    item.CategoryTypeCode = "ACC";
            //    item.Icon = "Settings"
            //    item.Code = "ACPD";
            //    item.ObjectTableName = "AccountingPeriod";
            //    var ObjectTable = window.ObjectTables.filter(d => d.Name == "AccountingPeriod")[0];
            //    item.ObjectTableId = ObjectTable.Id;
            //    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            //}



        }

    }
    private BuildOtherMenus() {
        if (SessionLocator.Tenant == 0){// && FeatureLocator.HasFeaturePermession("General", "HYBRIDPARTNERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "List"
            item.Code = "MTHP";
            item.ObjectTableName = "HybridPartner";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "HybridPartner")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));            
        }

        if (FeatureLocator.HasFeaturePermession("General", "SCHEDULERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List"
            item.Code = "MASC";
            item.ObjectTableName = "TasksScheduler";
            item.TextCode = "General.Features.Schedulers";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "TasksScheduler")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

        }

        if (FeatureLocator.HasFeaturePermession("General", "MAINCUSTOMERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "Par";
            item.Icon = "Customer"
            item.Code = "CUST";
            item.ObjectTableName = "Customer";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Customer")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (FeatureLocator.HasFeaturePermession("General", "DROPBOX")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "Settings"
            item.Code = "DRBO";
            item.ObjectTableName = "DropBox Connection";
            //item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "General")[0].Idd
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }

        if (FeatureLocator.HasFeaturePermession("General", "UPLOADPARTNERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "Settings"
            item.Code = "PAUP";
            item.ObjectTableName = "Partners Upload";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        } 

        if (FeatureLocator.HasFeaturePermession("General", "CacheLogMenu")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "List"
            item.Code = "CCHL";
            item.ObjectTableName = "Cache Log";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }


        // if (FeatureLocator.HasFeaturePermession("UserDefinedReport", "Module")) {
        //     var item = new MenusTablePM();
        //     item.CategoryTypeCode = "OTH";
        //     item.Icon = "Settings"
        //     item.Code = "UDR";
        //     item.ObjectTableName = "UserDefinedReport";
        //     this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        // }
      

        if (SessionLocator.Tenant == 0) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "Settings"
            item.Code = "USPC";
            item.ObjectTableName = "Packages";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "List"
            item.Code = "ANQU";
            item.ObjectTableName = "AnalyzeQueue";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "AnalyzeQueue")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "List"
            item.Code = "ACCS";
            item.ObjectTableName = "AccountingSystem";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "AccountingSystem")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List"
            item.Code = "ERLG";
            item.ObjectTableName = "ErrorLog";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "ErrorLog")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

             var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "Settings"
            item.Code = "CARGO";
            item.ObjectTableName = "Cargo Tracking";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
       

           

            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List"
            item.Code = "BSLG";
            item.ObjectTableName = "BatchServicesLog";
            item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "BatchServicesLog")[0].Id
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));

           
        }

        else {
            this.AllMaintenanceMenu = this.AllMaintenanceMenu.filter(d => d.Code != "MTDS");
        }
    }
    private BuildTransmissionsMenus() {
        if (this.isTransmissionsPageVisible) {

            if (SessionInfo.LoggedUserPM.IsCustomerCare && FeatureLocator.HasFeaturePermession("General", "General.Features.InttraSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "TRANS";
                item.Icon = "Settings"
                item.Code = "INTTRA_S";
                item.ObjectTableName = "INTTRA Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }

            if (FeatureLocator.HasFeaturePermession("General", "General.Features.InttraCommunicationSettings")) {
                if (SessionLocator.Tenant == 0) {
                    var item = new MenusTablePM();
                    item.CategoryTypeCode = "TRANS";
                    item.Icon = "Settings"
                    item.Code = "INTTRA_CMS";
                    item.ObjectTableName = "INTTRA Communication Settings";
                    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                }
            }
        }
    }

    IsCustomizationMaintenanceMenuVisible(): boolean {

        if (SessionLocator.Tenant == 261) {
            return true;
        }
        if (CustomizationPermissionService.HasFeaturePermession("General", "General.Features.CustomizationSettings") && CustomizationPermissionService.HasToggleFeaturePermession("CUS")) {
            return true;
        }
        return false;
    }

    IsTranslationMaintenanceMenusVisible(): boolean {
        if (SessionLocator.Tenant == 261) {
            return true;
        }

        else if (CustomizationPermissionService.HasFeaturePermession("General", "General.Features.Customization")) {
            return true;
        }
        return false;
    }
    private BuildCustomizationMenus() {
        if (this.IsCustomizationMaintenanceMenuVisible()) {
            this.AddCustomizationMenu();
        }
        if (this.IsTranslationMaintenanceMenusVisible()) {
            this.AddTranslateLabelMenu();
            this.AddTranslationMenu();
        }
        this.AddCustomFieldsMenu();  
    }
    private AddCustomFieldsMenu() {
        let item = new MenusTablePM();
        item.CategoryTypeCode = "CUS";
        item.Icon = "Settings";
        item.Code = "CFMM";
        item.ObjectTableName = "Custom Fields";
        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
    }
    private AddCustomizationMenu() {
        let item = new MenusTablePM();
        item.CategoryTypeCode = "CUS";
        item.Icon = "list";
        item.Code = "CUMM";
        item.ObjectTableName = "Customization";
        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
    }
    private AddTranslationMenu() {
        let item = new MenusTablePM();
        item.CategoryTypeCode = "CUS";
        item.Icon = "list";
        item.Code = "TRMM";
        item.ObjectTableName = "Translation";
        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
    }
    private AddTranslateLabelMenu() {
        let item = new MenusTablePM();
        item.CategoryTypeCode = "CUS";
        item.Icon = "list";
        item.Code = "TLMM";
        item.ObjectTableName = "Translate Label";
        this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
    }
    private BuildCustomObjectsMenus() {
        this.CustomObjectTables.forEach(objectTable =>
            this.BuildSingleCustomObjectMenu(objectTable));
    }
    private BuildSingleCustomObjectMenu(objectTable: ObjectTablePM) {
        let item = new MenusTablePM();
        item.CategoryTypeCode = "CSO";
        item.Icon = "list";
        item.ObjectTableId = objectTable.Id;
        item.ObjectTableName = objectTable.Name;
        item.Code = "CustomObject"
        var maintenanceMenuItem = new MaintenanceMenuItem(item);
        maintenanceMenuItem.DescriptionText = objectTable.Description != null ? objectTable.Description : TextCodeTranslator.Translate(objectTable.DescriptionTextCodeCode);
        this.AllMaintenanceMenu.push(maintenanceMenuItem);
    }
     
    private BuildShaamTokenManagementMenu() {
        if (FeatureLocator.HasFeaturePermession("General", this.invoiceConfirmationNumber)) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "SHA";
            item.Icon = "List"
            item.Code = "SHAAM_LOGS";
            item.ObjectTableName = TextCodeTranslator.Translate('General.MC.Logs'),
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        
            var item = new MenusTablePM();
            item.CategoryTypeCode = "SHA";
            item.Icon = "Settings"
            item.Code = "SHAAM_TOKEN";
            item.ObjectTableName = TextCodeTranslator.Translate('General.MC.TokenManagement');
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
    }

    // Commands
    PageChanged(item: Menu) {
        this.SelectedMenu = item;

        var itemsSource: MaintenanceMenuItem[] = [];

        if (item != null) {
            itemsSource = this.AllMaintenanceMenu.filter(f => f.CategoryTypeCode.toUpperCase() == this.SelectedMenu.Code.toUpperCase());
        }

        //this.ItemsSource = itemsSource;

        itemsSource.forEach(item => {
            if (AppTool.IsNullOrEmpty(item.TranslatedName)) {
                item.TranslatedName = "";
            }
        });

        this.ItemsSource = itemsSource.sort((a, b) => a.TranslatedName.toLowerCase() !== b.TranslatedName.toLowerCase() ? a.TranslatedName.toLowerCase() < b.TranslatedName.toLowerCase() ? -1 : 1 : 0);
    }

    ItemClicked(item: MaintenanceMenuItem) {
        if (item) {
            switch (item.Code) {
                case "SHAAM_LOGS": {
                    HostScreenService.open(TextCodeTranslator.Translate('General.MC.Logs'),'ConfirmationNumberTokenLog');
                    break;
                }
                
                case "SHAAM_TOKEN": {        
                    this.openTokenManagment();                
                    break;
                }

                case "CUSC": {
                    const logWindow: LogitudeWindow = new LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 300;
                    logWindow.Title = "Customs Cloud";
                    logWindow.IsShowCloseButton = true;
                    logWindow.WindowArgs = { windowInstance: logWindow } as CustomsCloudComponentArgs;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomsCloud/CustomsCloudComponent');
                    break;
                }

                case "DFES": {
                    var windowTitle = "Document Filing Email Settings";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./Common/Components/Maintenance/DocumentFilingEmailSettings/DocumentFilingEmailSettingsComponent');
                    break;
                }
                case "CODE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "System Defaults ";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 850;
                        logWindow.Height = 630;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemDefaults/SystemDefaultsComponent');
                    });
                    break;
                }
                case "LBSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "LogBox Settings ";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 850;
                        logWindow.Height = 630;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/LogBoxSettings');
                    });
                    break;
                }
                case "LOSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "Local Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 750;
                        logWindow.Height = 500;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/LocalSettings/LocalSettingsComponent');
                    });
                    break;
                }
                case "CARGO": {
               
                        var windowTitle = "Cargo Tracking";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 950;
                        logWindow.Height = 500;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Accounting/Components/Others/CargoTrackingService/CargoTrackingServiceComponent');
                   
                    break;
                }
                case "MTCE": {
                    this.CustomsClosedTablesMethod(item);
                    break;
                }
                case "MTIM": {
                    this.InterfaceManageentMethod(item);
                    break;
                }
                ///case "MCPA": { this.CustomsAutonomyKeywordMethod(item); break; }
                case "CSMN": {
                    let test = true;
                    let strict = true;
                    if (test) {
                        if (DateTool.GetCurrentDateAsUtc().valueOf() < new Date(2017, 7, 20).valueOf()) {
                            strict = false;
                        }
                    }
                    let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
                    LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                    let allowed = false;
                    allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."));
                    if (strict && !SessionLocator.LoggedUserPM.IsCustomerCare && allowed) {

                        let messageWindow = new MessageWindow()
                        messageWindow.Show("Logged User Is not Customer Care ");
                        return;
                    }
                    let windowTitle = "הגדרות מכס"//"Customs Settings";
                    let logWindow = new LogitudeWindow();
                    logWindow.Width = 750;
                    logWindow.Height = 700;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsSettingsComponent');
                    break;
                }
                case "CSRA": {
                    let test = true;
                    let strict = true;
                    if (test) {
                        if (DateTool.GetCurrentDateAsUtc().valueOf() < new Date(2017, 7, 20).valueOf()) {
                            strict = false;
                        }
                    }
                    let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
                    LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                    let allowed = false;
                    allowed = SessionLocator.LoggedUserPM.IsCustomerCare || (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.")) ? true : false;
                    if (!allowed) {

                        let messageWindow = new MessageWindow()
                        messageWindow.Show("Logged User Is not Customer Care ");
                        return;
                    }
                    var windowArgs: any = {};;
                    windowArgs.isReAnAnalysis = true;

                    let windowTitle = TextCodeTranslator.Translate("General.MC.Customs.ReAnalysis");//"גליון בקשות - ניתוח מחדש"//"Customs Settings";
                    let logWindow = new LogitudeWindow();
                    logWindow.Width = 1300;
                    logWindow.Height = 700;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent');

                 
                        
                

 
                    break;
                }
                case "COAD": {
                    var windowTitle = "Company Address Settings";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 750;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/CompanyAddress/CompanyAddressSettingsComponent');
                    break;
                }
                case "PAGD": {
                    var windowTitle = "Payment Gateway Definition";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 250;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    this._entityResourceService.getEntityResourceByTableName("TenantAdditionalData").subscribe((response:any) => {

                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/PaymentGateway/PaymentGatewayComponent');
                    });
                    break;
                }
                case "SYCR": {
                    var windowTitle = "System Currencies";
                    var logWindow = new LogitudeWindow();
                    logWindow.Title = windowTitle;
                    logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/SystemCurrenciesComponent');
                    break;
                }
                case "CHPA": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 600;
                    logitudeWindow.Height = 400;
                    logitudeWindow.Title = "Change User Password";
                    this._entityResourceService.getEntityResourceByTableName("User").subscribe((response:any) => {
                        logitudeWindow.DataContext = this;
                        logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
                    });

                    break;
                }
                case "COLO": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 740;
                    logitudeWindow.Height = 585;
                    logitudeWindow.DataContext = this;
                    logitudeWindow.Title = "Logo Definition";
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent');
                    break;
                }
                case "AUTO": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 950;
                    logitudeWindow.Height = 640;
                    logitudeWindow.DataContext = this;
                    logitudeWindow.DataContext.IsMainteneceView = true;
                    logitudeWindow.Title = "Automations";
                    logitudeWindow.Show('./Infrastructure/Components/Maintenance/Automation/MainMenuAutomationComponent');
                    break;
                }

                case "CFMM": { 
                    var { logWindow, windowArgs }: { logWindow: LogitudeWindow; windowArgs: any; } = this.ShowCustomizationCustomFieldsWindow(logWindow, windowArgs);
                    break;
                }
                     

                case "SYIN": {
                    this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response:any) => {
                        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(tenantResp => {

                            var logitudeWindow = new LogitudeWindow();
                            logitudeWindow.Width = 800;
                            logitudeWindow.Height = 600;
                            logitudeWindow.DataContext = "SystemInfo";
                            logitudeWindow.Title = "System Info";
                            logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/SystemInfoComponent');
                        });
                    });
                    break;
                }
                case "SIGN": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {

                        var windowArgs: any = {};
                        windowArgs.DataViewModel = this;
                        windowArgs.PageType = "Signature";
                        windowArgs.TemplateId = SessionLocator.LoggedUserId;
                        windowArgs.Tenant = SessionLocator.Tenant;

                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = widthwindow - 100;
                        logWindow.Height = heighthwindow - 100;
                        logWindow.Title = "Edit Html Template";
                        logWindow.WindowArgs = windowArgs;
                        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
                    });
                    break;
                }
                case "TOUS": {
                    this._entityResourceService.getEntityResourceByTableName("TermsofUseSignature", 0).subscribe((response:any) => {
                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;

                        var data = SessionLocator.LoggedUserId + "@" + SessionLocator.Tenant + "@Signature";

                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.DataContext = data;
                        logitudeWindow.IsShowCloseButton = true;
                        logitudeWindow.Title = "Terms of Use Signature";
                        logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsofUseSignatureComponent');
                    });
                    break;
                }
                case "ACSE": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 820;
                    logitudeWindow.Height = 570;
                    logitudeWindow.Title = "Accounting Settings";
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
                    break;
                }
                case "FACS": {
                    this._entityResourceService.getEntityResourceByTableName("FullAccountingSetting", 0).subscribe((response:any) => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 900;
                        logitudeWindow.Height = 550;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.FullAccountingSettings"); // "Full Accounting Settings";
                        logitudeWindow.Show('./Accounting/Components/Maintenance/FullAccountingSettingsComponent');
                    });
                    break;
                }
                case "COATO": {
                    this._entityResourceService.getEntityResourceByTableName("ChartOfAccountsType", 0).subscribe((response:any) => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 400;
                        logitudeWindow.Title = 'Chart Of Accounts Types Order';//TextCodeTranslator.Translate("Accounting.General.O.ChartOfAccountsTypesOrder");
                        logitudeWindow.Show('./Accounting/Components/Maintenance/ChartOfAccountsTypesOrderComponent');
                    });
                    break;
                }
                case "ACYT": {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe((response:any) => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 300;
                        logitudeWindow.Title = "Year Transfer";
                        logitudeWindow.Show('./Accounting/Components/Maintenance/YearTransferComponent');
                    });
                    break;
                }
                case "ACPD": {
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe((response:any) => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.Width = 750;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
                        logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodsComponent');
                    });
                    break;
                }
                case "MTSV": {
                    this._entityResourceService.getEntityResourceByTableName("SLAHeader", 0).subscribe((response:any) => {
                        this._entityResourceService.getEntityResourceByTableName("SLALine", 0).subscribe((response:any) => {
                            var logitudeWindow = new LogitudeWindow();
                            logitudeWindow.Width = 950;
                            logitudeWindow.Height = 640;
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Title = "SLA/Result Setting";
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Show('./CRMModules/CRMOthers/Components/SLA/SLAMainWindowComponent');
                        });
                    });
                    break;
                }
                case "MTUS": {
                    this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe((resp: any) => {
                        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureUser/Components/UserWorkspaceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(null);
                                this.CurrentSession.AddMenuReference(cmpRef);
                            });
                    });
                    break;
                }
                case "CRLM": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Credit Limit Settings";
                    logitudeWindow.Show('./Common/Components/Maintenance/CreditLimit/CreditLimitSettingsComponent');
                    break;
                }
                case "MTBH": {
                    this._entityResourceService.getEntityResourceByTableName("BusinessHour", 0).subscribe((response:any) => {
                        this._entityResourceService.getEntityResourceByTableName("BusinessHoursHoliday", 0).subscribe((response:any) => {
                            var logitudeWindow = new LogitudeWindow();
                            logitudeWindow.Width = 950;
                            logitudeWindow.Height = 550;
                            logitudeWindow.Title = "Business Hours and Holidays";
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/NewBusinessHourAndHolidaysComponent');
                        });
                    });
                    break;
                }
                case "AIRS": {
                    this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response:any) => {
                        var windowTitle = "Airline Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 650;
                        logWindow.Height = 350;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AirlineSettings/AirlineSettingsComponent');
                    });
                    break;
                }
                case "DRBO": {
                    var windowTitle = "Dropbox Connection";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 350;
                    logWindow.Height = 225;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
                    break;
                }
                case "MTEA": {
                    var windowTitle = "E-mail Notifications Settings";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 800;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/EmailNotifications/EmailNotificationsSettingsComponent');
                    break;
                }
                case "MTIS": {
                    var windowTitle = "Integration Systems Setting";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 350;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/IntegrationSystemsSetting/IntegrationSystemsSetting');
                    break;
                }
                case "USPC": {
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 550;
                    logWindow.Title = "User Packages";
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/UserPackagesComponent');
                    break;
                }
                case "CUSA": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "Customer Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 700;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/CustomerActivationSettingsComponent');
                    });
                    break;
                }
                case "VATS": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "VAT Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 700;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/VATSettingsComponent');
                    });
                    break;
                }
                case "CISE": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Customs Settings";
                    logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
                    break;
                }
                case "CFTP": {
                    let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
                    LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                    let allowed = false;
                    allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."));
                    let LoggedUserIsnotCustomerCare: boolean = true;
                    if (SessionLocator.LoggedUserPM.IsCustomerCare ) {
                        LoggedUserIsnotCustomerCare = false;
                    }
                    if (allowed) {
                        LoggedUserIsnotCustomerCare = false;
                    }

                    //if (!SessionLocator.LoggedUserPM.IsCustomerCare && allowed) {
                    if (LoggedUserIsnotCustomerCare) {

                        let messageWindow = new MessageWindow()
                        messageWindow.Show("Logged User Is not Customer Care ");
                        return;
                    }
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "הגדרות תקשורת ";
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 530;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsPartnerFtpListComponent');
                    break;
                }
                case "MTTC": {
                    this._entityResourceService.getEntityResourceByTableName("TicketClassification", 0).subscribe((resp: any) => {
                        SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketClassificationMaintenanceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run();
                                this.CurrentSession.AddMenuReference(cmpRef);
                            });
                    });
                    break;
                }
                case "INVS": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "Invoice Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/InvoiceSettings/InvoiceSettingsComponent');
                    });
                    break;
                }
                case "MTFS": {
                    var windowTitle = "FBL Stock";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 800;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/FBLStockMainComponent');
                    break;
                }
                case "MTSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
                        var windowTitle = "Ticket Settings";
                        var logWindow = new LogitudeWindow();
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./CRMModules/CRMOthers/Components/TicketSettings/TicketSettingsComponent');
                    });
                    break;
                }
                case "REFI": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 525;
                    logitudeWindow.Width = 750;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/RequiredFields/RequiredFieldsComponent');
                    break;
                }
                case "SECP": {
                    this._entityResourceService.getEntityResourceByTableName("TenantLoginPolicy", 0).subscribe((response:any) => {
                        var windowTitle = TextCodeTranslator.Translate("TenantLoginPolicy");
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 550;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = false;
                        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/TenantSecurityPolicy/TenantLoginPolicyComponent');
                    });
                    break;
                }
                case "BSLG": {
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 1100;
                    logWindow.Height = 1000;
                    logWindow.Title = "Batch Services Log";
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/BatchService/BatchServicesComponent');
                    break;
                }
                case "COCO": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Counters";
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/CountersComponent');
                    break;
                }
                case "SUMN": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Support Management";
                    logitudeWindow.Height = 250;
                    logitudeWindow.Width = 300;
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/SupportManagementComponent');
                    break;
                }
                case "OISG": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Ocean Insights Settings";
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Width = 1400;
                    logitudeWindow.Height = 800;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/OceanInsightsSetting/OceanInsightsSettingsComponent');
                    break;
                }
                case "HYTT": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Hybrid Tenant Threshold";
                    logitudeWindow.Height = 250;
                    logitudeWindow.Width = 300;
                    logitudeWindow.ShowCloseButton = false;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/HybridTenantThresholdComponent');
                    break;
                }

                case "CRTE": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Create Tenant";
                    logitudeWindow.Height = 500;
                    logitudeWindow.Width = 750;
                    logitudeWindow.ShowCloseButton = false;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantComponent');
                    break;
                }

                case "MASC": {
                    this._entityResourceService.getEntityResourceByTableName("TasksScheduler", 0).subscribe((response:any) => {

                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1200;
                        logWindow.Height = 1000;
                        if (!FeatureLocator.HasFeaturePermession("TasksScheduler", "READ") || (!FeatureLocator.HasFeaturePermession("TasksScheduler", "TASK") && !FeatureLocator.HasFeaturePermession("TasksScheduler", "FTP"))) {
                            logWindow.Width = 800;
                            logWindow.Height = 500;
                        }

                        logWindow.Title = "Scheduler";
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/MainSchedulerComponent');
                    });
                    break;
                }
                case "MTHT": {
                    var windowTitle = "Hybrid Tenant State";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 1100;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.Show('./InfrastructureModules/InfrastructureHybrid/Components/HybridTenantState/HybridTenantStateComponent');
                    break;
                }
                case "MCSG": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "עמדות חתימה";//TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 525;
                    logitudeWindow.Width = 750;
                    logitudeWindow.Width = 1000;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/SignStationsComponent');
                    break;
                }
                case "MRSG": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("Customs.General.O.RecallSuppliersFromFile");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 400;
                    logitudeWindow.Width = 500;

                    //logitudeWindow.Show('./Customs/Components/CustomsRequests/GeneralRequests/RecallSuppliersFromFileComponent');
                    logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/RecallSuppliersFromFileComponent');
                    break;
                }
                case "MRCF": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "קליטת קובץ אישורים מאיקאה להצהרה";
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Width = 700;
                    logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/ReceiptCertificateFromFileComponent')
                    break;
                }
                case "MTDD": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator.Translate("General.MC.Customs.DocumentsDefinition");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 650;
                    logitudeWindow.Width = 750;

                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsDocumentsDefinitionComponent');
                    break;
                }
                case "MTRC":
                    {
                        let test = true;
                        let strict = true;
                        if (test) {
                            if (DateTool.GetCurrentDateAsUtc().valueOf() < new Date(2017, 7, 20).valueOf()) {
                                strict = false;
                            }
                        }
                        let LoggedUserPMCode = SessionLocator.LoggedUserPM.Code || "";
                        LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                        let allowed = false;
                        allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.") || SessionLocator.LoggedUserPM.IsCustomerCare);

                        if (strict && !allowed) {
                            let messageWindow = new MessageWindow()
                            messageWindow.Show("Logged User Is not Customer Care ");
                            return;
                        }

                        let confirmWindow = new ConfirmWindow();
                        confirmWindow.Title = TextCodeTranslator.Translate("General.MC.Customs.RecallClientsForCutoms");
                        confirmWindow.Width = 300;
                        confirmWindow.Height = 200;
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
                        confirmWindow.ShowNoButton
                        confirmWindow.Show("לעדכן נתוני יבואנים במערכת?");
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {

                                var servicelink = './Customs/CustomsGeneralRequests/Components/RecallClientsForCutoms';
                              //  servicelink = './Customs/Services/Others/CustomsRequestMenuService';

                                // SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                                //     service.SendRecallMessageToServer();
                                // });

                                SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                                    service.SendRecallMessageToServer();
                                });

                                // this will cause the customs to build every time......mohammad
                                //let _RecallClientsForCutoms: RecallClientsForCutoms = new RecallClientsForCutoms();
                                //_RecallClientsForCutoms.SendRecallMessageToServer();
                            }
                        });
                        break;
                    }
                case "QuoteSettings": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Height = 600;
                    logitudeWindow.Title = "Quote Settings";
                    logitudeWindow.Show('./QuoteModules/QuoteOthers/Components/Maintenance/QuoteSettingsComponent');
                    break;
                }

                case "ContainerSettings": {
                    this._entityResourceService.getEntityResourceByTableName("ContainerSetting").subscribe(() => {
                        var logitudeWindow = new LogitudeWindow();
                        logitudeWindow.ShowCloseButton = true;
                        logitudeWindow.Width = 1200;
                        logitudeWindow.Height = 600;
                        logitudeWindow.Title = "Container Settings";
                        logitudeWindow.Show('./ShipmentModules/ShipmentOthers/Components/ContainerSetting/ContainerSettingsComponent');
                    });
                    break;
                }

                case "INTTRA_S": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 960;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Title = "INTTRA Settings";
                    logitudeWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Maintenance/INTTRASettingsComponent');
                    break;
                }
                case "INTTRA_CMS": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "INTTRA Communication Settings";
                    logitudeWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Maintenance/INTTRACommunicationSettingsComponent');
                    break;
                }


                case "CCHL": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 800;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Title = 'Cache Log';
                    logitudeWindow.Show('./Infrastructure/Components/Maintenance/CacheLogComponent');
                    break;
                }

                case "SUPM": {
                    this._entityResourceService.getEntityResourceByTableName("SupportMailbox", 0).subscribe((response:any) => {
                        var windowTitle = "Support Mail Boxes";
                        var logWindow = new LogitudeWindow();
                        logWindow.Title = windowTitle;
                        logWindow.Show('./CRMModules/CRMOthers/Components/SupportMailBox/SupportMailBoxComponent');
                    });
                    break;
                }

                case "PAUP": {
                    var windowTitle = "Partners Upload";
                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./CommonModules/CommonPartners/Components/Maintenance/UploadPartnersComponent');
                    break;
                }

                case "VIZN": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Container Tracking - Pilot Customers";
                    logitudeWindow.Width = 1000;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/Vizion/VizionAutomaticRequestComponent');
                    break;
                }

                case "TOU": {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 800;
                    logitudeWindow.Height = 500;
                    logitudeWindow.IsShowCloseButton = true;
                    logitudeWindow.Title = "Terms of Use";
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsofUseComponent');
                    break;
                }

                case "CUMM": {
                    this.ShowCustomizationWindow();
                    break;
                }

                case "TRMM": {
                    this.ShowTranslationWindow();
                    break;
                }

                case "TLMM": {
                    this.ShowTranslateLabelsWindow();
                    break;
                }
                case "CustomObject":
                    this.ShowCustomObject(item);
                    break;
                default: {
                    
                    if (item.ObjectTableId) {
                        var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === item.ObjectTableId).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
                        if (allQueries.length == 0) {
                            var myConfirmWindow = new ConfirmWindow();
                            myConfirmWindow.Show("No Queries found for " + item.TranslatedName);
                        }

                        else {
                            var listArgs = new ListComponentArgs();

                            if (item.Code == "MTCL") {
                                listArgs.Perspective = "ShippersAndConsignees";
                                listArgs.NewButtonLabel = "New Shipper-Consignee";
                            }

                            var SelectedQuery = null;

                            if (listArgs.Perspective != null) {
                                if (item.ObjectTableName == "ErrorLog" || item.ObjectTableName == "AnalyzeQueue" || item.ObjectTableName == "CommunicationLog") {
                                    SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 })[0];

                                }
                                else {
                                    SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective)[0];
                                }
                            }
                            else {
                                if (item.ObjectTableName == "ErrorLog" || item.ObjectTableName == "AnalyzeQueue" || item.ObjectTableName == "CommunicationLog") {
                                    SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0)).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 })[0];

                                }
                                else {
                                    SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0))[0];
                                }
                            }

                            if(item.Code=="UDRM"){
                                this._entityResourceService.getEntityResourceByTableName("CalculatedChartsOfAccount", 0).subscribe((response:any) => {
                                    this._entityResourceService.getEntityResourceByTableName("CalculatedChartsOfAccountsLine", 0).subscribe((response:any) => {
                                    });
                                });
                              
                            }
                            //var SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective)[0];

                            var objectTablePM = window.ObjectTables.filter(d => d.Id == item.ObjectTableId)[0];

                            listArgs.QueryCode = SelectedQuery.Code;
                            listArgs.ObjectTableName = objectTablePM.Name;

                            listArgs.BackButtonTitle = "Maintenance";
                            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                                listArgs.DisplayTitle = TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
                                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        cmpRef.instance.ComponentRef = cmpRef;
                                        cmpRef.instance.Run(listArgs);
                                        //this.CurrentSession.AddMenuReference(cmpRef);
                                    });
                            });
                        }
                    }
                    else {
                        var myConfirmWindow = new ConfirmWindow();
                        myConfirmWindow.ShowNoButton = false;
                        myConfirmWindow.YesButtonText = "Ok";
                        myConfirmWindow.Show("Not Implemented");
                    }

                    break;
                }
            }
        }
    }

    private async openTokenManagment() {
        const logitudeWindow: LogitudeWindow = HostScreenService.open(TextCodeTranslator.Translate('General.MC.TokenManagement'),'CreateNewShaamToken');
        const hostScreenComponent: HostScreenComponent = await this.withWindowComponentLoaded(logitudeWindow);
        const subscription: Subscription  =  hostScreenComponent.$event
            .pipe(filter((event: HomeScreenEvent) => event.event === HomeScreenEventTypes.openNewBrowser))
            .subscribe((event: HomeScreenEvent) => {
                subscription.unsubscribe();
                logitudeWindow.Close('');
                this.openTokenManagment();
        })
    }
    
    private async withWindowComponentLoaded(logitudeWindow: LogitudeWindow) {
        return new Promise<any>(resolve => {
            const subscription: Subscription =  logitudeWindow.ComponentLoaded.subscribe(component => {
                subscription.unsubscribe();
                resolve(component);
            })            
        });
    }

    private ShowCustomObject(item: any) {
        let objectTable = window.ObjectTables.filter(d => d.Id == item.ObjectTableId)[0];
        let listArgs = new ListComponentArgs();
        listArgs.ObjectTableName = objectTable.Name;
        listArgs.BackButtonTitle = "Maintenance";
        listArgs.ShowViews = true;
        listArgs.DisplayTitle = this.textCodeTranslationPipe.transform(objectTable.Name);
        listArgs.QueryCode = this.GetQueryCode(item);
        listArgs.DontCheckQueryFeature = true;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.IsCustomEntity = true;
                cmpRef.instance.Run(listArgs);
            });
    }

    private GetQueryCode(item: any) {
        let allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === item.ObjectTableId).sort((a, b) => { return a.IndexOrder - b.IndexOrder; });
        let SelectedQuery = allQueries.filter(query => query.IsDefault)[0];
        if (allQueries.length > 0 && !SelectedQuery) {
            SelectedQuery = allQueries[0];
        }
        return SelectedQuery?.Code;
    }

    private ShowCustomizationCustomFieldsWindow(logWindow: LogitudeWindow, windowArgs: any) {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.IsCustomFieldsMenue = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "";
        logWindow.Width = 900;
        logWindow.Height = 550;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationMainComponent');
        return { logWindow, windowArgs };
    }
    private ShowCustomizationWindow() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "";
        logWindow.Width = 900;
        logWindow.Height = 550;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/CustomizationMainComponent');
    }
    private ShowTranslateLabelsWindow() {
        var windowTitle = "Select Translation Language";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 200;
        logWindow.Title = windowTitle;
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/TranslationLabels/SelectLanguagesComponent');
    }
    private ShowTranslationWindow() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.Title = "Translation";
        logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Translations/TranslationComponent');
    }
    DoJoker(text: string) {
        switch (text) {
            case "jokerinv":
                {

                        //$$GGG_IN = "UnifreightEntity=CFIFILEM;UnifreightEntityNumber=%%FILE_NO.CFIFILEM;LogitudeEntity=Customs.Declaration;LogitudeEntityNumber=%%LOGITUDE_FILE.CFIFILEM;LogitudeViewModel=UnifreightMassageHandler;LogitudeCommandId=CreateInvoiceCommand;formtitle=%%$text(IMP_DECLERATION)"
                    ///"UnifreightEntity=CFIFILEM·;
                    //UnifreightEntityNumber = 3000028·;
                    //LogitudeEntity = Customs.Declaration·;
                    //LogitudeEntityNumber = 1 - 211622·;
                    //LogitudeViewModel = UnifreightMassageHandler·;
                    //LogitudeCommandId = CreateInvoiceCommand·;
                    //formtitle = הצהרת יבוא"
                    var json = '{"UnifreightEntity"  :  "CFIFILEM" , "UnifreightEntityNumber"  :  "93320020" , "LogitudeEntity"  :  "Customs.Declaration" , "LogitudeEntityNumber"  :  "1-5415" , "LogitudeViewModel"  :  "UnifreightMassageHandler" , "LogitudeCommandId"  :  "CreateInvoiceCommand" , "formtitle"  :  "הצהרת יבוא"}';
                    
                    var objParams = JSON.parse(json);
                    objParams.Requset = new Array();
                    //objParams.Requset.push(["Requset.JumpTo", "Payment"]);
                    //objParams.Requset.push(["Requset.JumpTo", "RequestSheet"]);
                    objParams.Requset.push(["Requset.JumpTo", "Answer"]);

                    AmitalGatewayUtil.Instance.UnifaceRequest(objParams, null, null, null);
                    
                } break;
            case "jokeraccfunctionaltest": {


                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = 400;
                logitudeWindow.Height = 300;
                logitudeWindow.Title = "Accounting Load Test";
                logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingFunctionalTestComponent');

            } break;
            case "jokeracctest": {

                if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("nono NO only Customer Care ");
                    return;
                }
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = 1600;
                logitudeWindow.Height = 1200;
                logitudeWindow.Title = "Accounting main Tester";
                logitudeWindow.ShowCloseButton = true;
                logitudeWindow.Show('./Accounting/Components/Maintenance/Tester/AccountingMainTesterComponent');

            } break;
            case "jokeraccloadtest": {


                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = 750;
                logitudeWindow.Height = 500;
                logitudeWindow.Title = "Accounting Load Test";
                logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingLoadTestComponent');

            } break;
            case "jokersign": {
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = "עמדות חתימה";//TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                logitudeWindow.ShowCloseButton = true;
                logitudeWindow.Height = 525;
                logitudeWindow.Width = 750;
                logitudeWindow.Width = 1000;


                logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/SignStationsComponent');
                break;

            }
            case "jokerGetAmitalRestrictOwnerModel": {
                var servicelink = '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
                SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                    service.GetAmitalRestrictOwnerModel(false)
                        .subscribe(rsp => {
                            let AmitalRestrictOwnerModel = rsp.Result;
                        });
                });
                // this will cause the customs to build every time....mohammad
                //let customsSettingExtendedListService = new CustomsSettingExtendedListService();
                //customsSettingExtendedListService.GetAmitalRestrictOwnerModel(false)
                //    .subscribe(rsp => {
                //        let AmitalRestrictOwnerModel = rsp.Result;
                //    });

                break;
            }

            case "jokerremark": {
                var windowArgs: any = {};
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Height = 525;
                logitudeWindow.Width = 750;
                logitudeWindow.ShowCloseButton = true;
                windowArgs.QueType = 2;
                let EntityPM;
                var _declarationRemarksService: DeclarationRemarksService = new DeclarationRemarksService();
                if (windowArgs.QueType == 2) {
                    _declarationRemarksService.GetSVCOrSRVStatusList(1, "41100314")
                        .subscribe((response: any) => {
                            windowArgs.EntityPM = response.Result;
                            windowArgs.length = response.Result.length;
                            logitudeWindow.Title = windowArgs.length+ "  הערות מסווג  " ;
                            logitudeWindow.WindowArgs = windowArgs;
                            logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                        });
                } else {
                    _declarationRemarksService.GetINCorINAtatusList(1, "41100314")
                        .subscribe((response: any) => {
                            windowArgs.EntityPM = response.Result;
                            let counter = response.Result.length;
                            logitudeWindow.Title = counter + "  הערות מבקר  ";
                            logitudeWindow.WindowArgs = windowArgs;
                            logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/DeclarationRemarksComponent');
                        });
                }
                break;
            }
            default: {
                break;
            }

        }
    }
    SearchTextChanged(text: string) {
        if (text && text.startsWith("joker")) {
            this.DoJoker(text);
            return;
        }
        var itemsSource = this.AllMaintenanceMenu;
        if (AppTool.IsNullOrEmpty(text)) {
            itemsSource = this.AllMaintenanceMenu.filter(f => f.CategoryTypeCode.toUpperCase() == this.SelectedMenu.Code.toUpperCase());
        }
       else {
            itemsSource = itemsSource.filter(f => f.TranslatedName != null);
            itemsSource = itemsSource.filter(f => f.TranslatedName.toUpperCase().indexOf(text.toUpperCase()) > -1);
        }

        this.ItemsSource = itemsSource;
    }
    CustomsClosedTablesMethod(item: MaintenanceMenuItem) {
        //ObjectTableName                     :        "Customs.CustomsClosedTable"
        this._entityResourceService.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe((response:any) => {
            var listArgs = new ListComponentArgs();
            listArgs.DisplayTitle = item.TranslatedName;//TextCodeTranslator.Translate();
            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsClosedTablesComponent',
                this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    //cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }
    CustomsAutonomyKeywordMethod(item: MaintenanceMenuItem): any {
        
        this._entityResourceService.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = item.TranslatedName
            logitudeWindow.ShowCloseButton = true;
            logitudeWindow.Height = 400;
            logitudeWindow.Width = 500;
            //logitudeWindow.Show('./Customs/Components/CustomsRequests/GeneralRequests/RecallSuppliersFromFileComponent');
            logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/AutonomyKeywordComponent');

        });

    }

    InterfaceManageentMethod(item: MaintenanceMenuItem) {
        //ObjectTableName                     :        "Customs.CustomsClosedTable"
        this._entityResourceService.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe((response:any) => {
            var listArgs = new ListComponentArgs();
            listArgs.DisplayTitle = item.TranslatedName;//TextCodeTranslator.Translate();
            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsMaintenance/Components/Maintenance/InterfaceManagementComponent',
                this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    //cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }
}

class MaintenanceMenuItem {
    public Code: string;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public CategoryTypeCode: string;
    public TranslatedName: string;
    public DescriptionText: string;
    public ImageIconSource: string;
    constructor(private item: MenusTablePM) {
        this.Code = item.Code;
        this.ObjectTableId = item.ObjectTableId;
        this.ObjectTableName = item.ObjectTableName;
        this.CategoryTypeCode = item.CategoryTypeCode;
        this.SetTranslatedName();
        this.SetDescriptionText();
        this.SetImageIconSource();
    }

    private SetTranslatedName() {
        var myResult = "";

        if (this.Code == "AWMS") {
            var r = "";
        }

        if (this.Code == "MTCL" || this.Code == "MTIS" || this.Code == "MCSG" || this.Code == "MASC" || this.Code == "CRTE") {
            myResult = TextCodeTranslator.TranslateTable(this.item.TextCode);
        }

        else if (this.ObjectTableId == null) {
            myResult = this.item.ObjectTableName;
        }

        else {
            var ObjectTable = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
            if (ObjectTable.Name.indexOf("Customs.") != -1 || ObjectTable.ClientModuleName == "Accounting") {
                myResult = TextCodeTranslator.TranslateTable(this.item.ObjectTableName);
            }

            else {
                myResult = TextCodeTranslator.TranslateTablePlural(this.item.ObjectTableName);
            }
        }

        this.TranslatedName = myResult;
        if (this.Code == "MASC") {

            this.TranslatedName = "Scheduler";
        }
        if (AppTool.IsNullOrEmpty(this.TranslatedName)) {
            this.TranslatedName = this.Code;
        }
    }
    private SetDescriptionText() {
        var myResult = "";

        if (this.Code == "CFMM" || this.Code == "CUMM") {
            switch (this.Code) {
                case "CUMM": { myResult = "Managing standard and Custom objects such ad Custom Fields, Screen Layout, Tabs, Rules .."; break; }
                case "CFMM": { myResult = "Managing Custom fields with Pick-list type, defining new Pick-list and adjusting the existing ones"; break; }
            }
        }


        if (this.CategoryTypeCode == "PRS" || this.CategoryTypeCode == "CMS" || this.CategoryTypeCode == "MNG" || this.Code == "FACS" || this.Code == "COATO" || this.Code == "ACYT") {
            switch (this.Code) {
                case "SIGN": { myResult = "Set Signature Settings"; break; }
                case "CHPA": { myResult = "Change Password"; break; }
                case "CARGO": { myResult = "Cargo Tracking"; break; }
                case "COAD": { myResult = "Company Address Settings"; break; }
                case "PAGD": { myResult = "Payment Gateway Definition"; break; }
                case "CODE": { myResult = "System Defaults"; break; }
                case "LBSE": { myResult = "LogBox Settings"; break; }
                case "COCO": { myResult = "Counters"; break; }
                case "COLO": { myResult = "Company Logo"; break; }
                case "ACSE": { myResult = "Accounting Settings"; break; }
                case "LOSE": { myResult = "Local Settings"; break; }
                case "USPC": { myResult = "User Packages"; break; }
                case "TENT": { myResult = "List of Tenants"; break; }
                case "LOGS": { myResult = "Communication Logs"; break; }
                case "CTPS": { myResult = "Container Tracking Settings"; break; }
                case "MNGT": { myResult = "Error Logs"; break; }
                case "APLG": { myResult = "API Logs"; break; }
                case "FACS": { myResult = "Define your accounting settings"; break; }
                case "COATO": { myResult = "Define your Chart Of Accounts Types Order"; break; }
                //case "ACPD": { myResult = "Define your accounting periods"; break; }
                case "ACYT": { myResult = "Year Transfer"; break; }



            }
        }

        else {
            var ObjectTable = window.ObjectTables.filter(d => d.Id == this.item.ObjectTableId)[0];
            if (ObjectTable != null) {
                myResult = TextCodeTranslator.Translate(ObjectTable.DescriptionTextCodeCode);
            }
        }
        //if (this.Code == "MCSG") { myResult = "עמדות חתימה"; }

        this.DescriptionText = myResult;

    }
    private SetImageIconSource() {
        switch (this.item.Icon) {

            case "Customer":
            case "Customer.png": {
                this.ImageIconSource = "./Images/Maintenance/Customer.png";
                break;
            }

            case "PotentialCustomer.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippersConsignee.png";
                break;
            }

            case "Agent.png": {
                this.ImageIconSource = "./Images/Maintenance/Agent.png";
                break;
            }

            case "CustomAgent.png": {
                this.ImageIconSource = "./Images/Maintenance/CustomAgent.png";
                break;
            }

            case "ShippingAgent.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippingAgent.png";
                break;
            }

            case "Airline.png": {
                this.ImageIconSource = "./Images/Maintenance/Airline.png";
                break;
            }

            case "ShippingLine.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippingLine.png";
                break;
            }


            case "Trucker.png": {
                this.ImageIconSource = "./Images/Maintenance/Trucker.png";
                break;
            }

            case "Settings": {
                this.ImageIconSource = "./Images/Maintenance/Settings.png";
                break;
            }

            case "List":
            case "Table":
            default: {
                this.ImageIconSource = "./Images/Maintenance/Table.png";
                break;
            }
        }
    }
}

class Menu {
    Code: string;
    Name: string;
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }
}

class MenusTablePM {
    public Id: string;
    public Tenant: number;
    public MenuTypeCode: string;
    public CategoryTypeCode: string;
    public IndexOfOrder: number;
    public Icon: string;
    public TextCode: string;
    public UserControlName: string;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public Code: string;
    public FeatureId: string;
    public FeatureCode: string;
    public ShowMenuTable: boolean;
    public HtmlView: string;
    public FeatureUniqeCode: string;
}
