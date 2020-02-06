import {Component, OnInit, ViewChild, ViewContainerRef,ChangeDetectorRef} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
import {FilterField, FilterFieldsClass, FieldsValues} from '../../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/FilterField';
import {DateTool} from '../../../../Infrastructure/Tools';
import {CustomerAdditionalServicePM} from '../../../../Common/EntityPMs/CustomerAdditionalServicePM';
import {CustomerProductPM} from '../../../../Common/EntityPMs/CustomerProductPM';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
import {RankList} from '../../../../Common/EntityLists/RankList';
import {RankListService} from '../../../../Common/Services/StandardLists/RankListService';
import {AdditionalServiceListService} from '../../../../Common/Services/StandardLists/AdditionalServiceListService'; 
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../../Infrastructure/Services/EntityPMService';
import {TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CustomerCompetitorPM} from '../../../../Common/EntityPMs/CustomerCompetitorPM';
import {CompetitorList} from '../../../../Common/EntityLists/CompetitorList';
import {CompetitorListService} from '../../../../Common/Services/StandardLists/CompetitorListService';
import {CustomerAccountManagerByProductSplitComponentARGS} from '../../../../Common/Args';
import {CustomerSalesmanByProductPM}  from '../../../../Common/EntityPMs/CustomerSalesmanByProductPM';
import {CustomerAccountManagerByProductPM}  from '../../../../Common/EntityPMs/CustomerAccountManagerByProductPM';
import {CustomerMediatorByProductPM}  from '../../../../Common/EntityPMs/CustomerMediatorByProductPM';
import {CustomerForwarderByProductPM}  from '../../../../Common/EntityPMs/CustomerForwarderByProductPM';
import {CustomerCustomsAgentByProductPM}  from '../../../../Common/EntityPMs/CustomerCustomsAgentByProductPM';
import {GroupByPipe} from '../../../../Infrastructure/Pipes/GroupByPipe';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {LeadSourceListService} from '../../../../Common/Services/StandardLists/LeadSourceListService';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
declare var UploadLogoFile, HideImage, SetImage, ArrayBufferToBase64: any;
import {CustomerFieldsUpdateSettingListService} from '../../../../Common/Services/StandardLists/CustomerFieldsUpdateSettingListService';
import {CustomerFieldsUpdateSettingList} from '../../../../Common/EntityLists/CustomerFieldsUpdateSettingList';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerGeneralTabComponent.html',
    providers: [ImageLibraryService, EntityPMService]
})

export class CustomerGeneralTabComponent extends BaseComponent   {    
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public TenantPM: TenantPM;
    public ServicePM: CustomerAdditionalServicePM;
    public LabelColumnWidth: number = 115;
    public ControlColumnWidth: number = 200;
    public EntityId: string = "";
    public EntityName: string = "";
    public AllProductTypes: any[] = [];
    DataImage: any;
    public ImageId: string = "";
    customerFieldsUpdateSettingListService: CustomerFieldsUpdateSettingListService = new CustomerFieldsUpdateSettingListService();
    customerFieldsUpdateSettingList: Array<CustomerFieldsUpdateSettingList> = [];
    public IsUnifreightEditable: boolean = false;
    public rankListService: RankListService;
    public DataContext: CustomerGeneralTabComponent = this;
    LogoInput: string = Guid.NewRandomString();
    IsShowMessageComplate: boolean = false;
    IsShowProgressLoading: boolean = false;
    IsCustomer: boolean = false;
    public ScreenCode: string = "Customer.AdditionalFields";
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public _imageLibraryService: ImageLibraryService, private CD: ChangeDetectorRef, private entityPMService: EntityPMService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ImageId = this.EntityPM.ImageDetailId;
        this.TenantPM = SessionLocator.TenantPM;
        this.IndustryId = this.EntityPM.IndustryId;
        this.EntityName = "Customer";
        this.EntityId = this.EntityPM.Id;
        this.IsCustomer = this.EntityPM.IsCustomer;
        this.LeadSourceId = this.EntityPM.LeadSourceId;

        this.Listen();

        var myService = new ProductTypeListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllProductTypes = myResponse.Result.filter(o => !o.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            }
        });

        this.customerFieldsUpdateSettingListService.getAll().subscribe(response => {
            if (!response.HasError) {
                this.customerFieldsUpdateSettingList = response.Result;
            }

            this.SetUIProperties();
            this.CloseScreen();
        });

       
        this.RunComponent();
        this.AllCompetitors = new Array<CompetitorList>();

        this.rankListService = new RankListService();
        this.rankListService.getAllFromCache().subscribe(result => {
            this.RankListArr = result.Result;
        });
    }   

    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "EntityActivated") {
                    this.CloseScreen();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();

            this.GetProductsList();
            this.GetAdditionalSerivceList();
            this.GetCompetitorList();

            this.RankSource1();
            this.RankSource2();
            this.RankSource3();
            this.SetMoreButtonsVisibility();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {

                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
    }

    public ProducttypeList = [];
    public ProductsToggleButtonList: any = [];
    public ProductsObslist: Array<ProductObslistItemClass> = [];
    GetProductsList() {
        var service = new CommonDomainService();
        service.GetProductTypesByTenant(this.TenantPM.Id).subscribe((myResult: any) => {
            this.ProductsToggleButtonList = [];
            this.ProducttypeList = myResult;

            myResult.forEach((i) => {
                if (!i.InActive) {
                    var item = new ProductTypeList();
                    item.Code = i.Code;
                    item.Name = i.Name;
                    item.InActive = i.InActive;
                    item.Id = i.Id;
                    item.SearchFields = i.SearchFields;
                    this.ProductsToggleButtonList.push(i);
                }
                else {


                }
            });
            this.ProductsToggleButtonList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            this.BuildProductsToggleButtonList();
            this.BuildProductsObsList();
        });
    }
    BuildProductsObsList() {
        this.ProductsObslist = [];

        this.EntityPM.CustomerProducts.forEach((item: CustomerProductPM) => {
            var newItem = new ProductObslistItemClass(item, this);
            this.ProductsObslist.push(newItem);
        });

        this.NoProductsVisibility = this.ProductsObslist.length == 0 ? true : false;
    }

    public ToggleButtonList: Array<ServiceItemClass> = [];
    public ToggleButtonListService: any = [];
    public Services: Array<ServiceViewModelData> = [];
    GetAdditionalSerivceList() {
        var AddtionalService: AdditionalServiceListService = new AdditionalServiceListService();
        AddtionalService.getAllFromCache().subscribe(result => {
            this.ToggleButtonListService = [];
            this.ToggleButtonListService = result.Result.filter(s => !s.InActive);
            this.ToggleButtonListService.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });

            this.BuildToggleButtonList();
            this.BuildObsList();
        });
    }
    BuildToggleButtonList() {
        this.ToggleButtonList = [];
        var data: Array<any> = this.ToggleButtonListService;

        if (!AppTool.IsNullOrEmpty(this.SearchTextAdditionalService)) {
            data = this.ToggleButtonListService.filter(f => f.Name.toLowerCase().indexOf(this.SearchTextAdditionalService.toLowerCase()) > -1);
        }

        data.forEach(item => {
            this.ToggleButtonList.push(new ServiceItemClass(item, this.EntityPM, this));
        });
    }
    BuildObsList() {
        this.Services = [];

        this.EntityPM.CustomerAdditionalServices.forEach(item => {
            this.Services.push(new ServiceViewModelData(item, this));
        });

        this.NoServicesVisibility = this.Services.length == 0 ? true : false;
        this.ServicesVisibility = this.Services.length == 0 ? false : true;       
    }

    public CompetitorToggleButtonList: any = [];
    public Competitors: Array<CompetitorViewModelData> = [];
    public AllCompetitors: Array<CompetitorList> = [];
    GetCompetitorList() {
        var competitorListService: CompetitorListService = new CompetitorListService();
        competitorListService.getAll().subscribe(result => {
            this.AllCompetitors = result.Result;

            this.BuildCompetitorToggleButtonList();
            this.BuildCompetitorsObsList();
        });
    }

    BuildCompetitorToggleButtonList() {
        this.CompetitorToggleButtonList = [];
        var data: Array<any> = this.AllCompetitors;

        if (!AppTool.IsNullOrEmpty(this.SearchTextCompetitor)) {
            data = this.AllCompetitors.filter(f => f.Name.toLowerCase().indexOf(this.SearchTextCompetitor.toLowerCase()) > -1);
        }

        data.forEach(item => {
            this.CompetitorToggleButtonList.push(new CompetitorItemClass(item, this.EntityPM, this));
        });
    }
    BuildCompetitorsObsList() {
        this.Competitors = [];
        this.EntityPM.CustomerCompetitors.forEach(item => {
            this.Competitors.push(new CompetitorViewModelData(item, this));
        });

        if (this.Competitors.length == 0)
            this.NoCompetitorVisibility = true;

        else {
            this.NoCompetitorVisibility = false;
        }
    }

    RankSource1(rank = null) {
        if (rank != null) {
            this.RankSourceText1 = "./Images/Icons/StarOrange.png";
        }

        else {
            var myResult: string = null;
            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;

                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }

                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    }
    RankSource2(rank = null) {
        if (rank != null) {
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }

        else {
            var myResult: string = "./Images/Icons/StarOrange.png";

            if (this.EntityPM != null) {
                var RankCode = this.EntityPM.RankCode;

                switch (RankCode) {
                    case "1": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "2": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                        break;
                    }

                    case "3": {
                        this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                        this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                        break;
                    }

                    default: {
                        this.RankSourceText1 = "./Images/Icons/StarGray.png";
                        this.RankSourceText2 = "./Images/Icons/StarGray.png";
                        this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    }
                }
            }
        }
    }
    RankSource3(rank = null) {
        if (rank != null) {
            this.RankSourceText3 = "./Images/Icons/StarOrange.png";
            this.RankSourceText2 = "./Images/Icons/StarOrange.png";
        }

        else {
            var RankCode = this.EntityPM.RankCode;

            switch (RankCode) {
                case "1": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "2": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                    break;
                }

                case "3": {
                    this.RankSourceText1 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText2 = "./Images/Icons/StarOrange.png";
                    this.RankSourceText3 = "./Images/Icons/StarOrange.png";
                    break;
                }

                default: {
                    this.RankSourceText1 = "./Images/Icons/StarGray.png";
                    this.RankSourceText2 = "./Images/Icons/StarGray.png";
                    this.RankSourceText3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    }

    public isRAFieldsVisibile: boolean = false;
    public IsBlockMessageVisible: boolean = false;
    SetUIProperties() {
        if (this.TenantPM.RegulatedAgentRegimeActivated) {
            this.isRAFieldsVisibile = true;
        }
        
        this.UIProperties.SetVisibility("KnownConsignor", this.ObjectTableName, this.isRAFieldsVisibile);
        this.UIProperties.SetVisibility("KCExpirationDate", this.ObjectTableName, this.isRAFieldsVisibile);

        this.SearchTextCompetitorsDropButtonCustomerId += this.CurrentSession.GetNewId("SearchTextCompetitorsDropButtonId_1");
        this.SearchTextCompetitorsCustomerId += this.CurrentSession.GetNewId("SearchTextCompetitorsId_1");
        this.SearchTextAdditionalServiceModeDropButtonCustomerId += this.CurrentSession.GetNewId("SearchTextAdditionalServiceModeDropButtonId_1");
        this.SearchTextAdditionalServiceCustomerId += this.CurrentSession.GetNewId("SearchTextAdditionalServiceId_1");
        this.SearchProductDropButtonCustomerId += this.CurrentSession.GetNewId("SearchProductDropButtonId_1");
        this.SearchProductsModeCustomerId += this.CurrentSession.GetNewId("SearchProductsModeId_1");

        this.SetUIProperties_Partners();
    }

    public IsSplitted_AccountManager: boolean = false;
    public IsSplitted_SalesmanUser: boolean = false;
    public IsSplitted_Forwarder: boolean = false;
    public IsSplitted_CustomsAgent: boolean = false;
    public IsSplitted_Mediator: boolean = false;
    SetUIProperties_Partners() {
        this.SetUIProperties_AccountManager();
        this.SetUIProperties_Salesman();
        this.SetUIProperties_Forwarder();
        this.SetUIProperties_CustomsAgent();
        this.SetUIProperties_Mediator();

    }
    SetUIProperties_AccountManager() {
        var isEnabled = false;
        var isSplitted = false;

        var myPipe = new GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerAccountManagerByProducts.filter(f => f.AccountManagerId != null), "AccountManagerId");

        if (Forwarders.length == 0) {
            isEnabled = true;
        }

        else if (Forwarders.length > 1) {
            isSplitted = true;
        }

        this.IsSplitted_AccountManager = isSplitted;
        this.UIProperties.SetEnabled("AccountManagerUserId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_Salesman() {
        var isEnabled = false;
        var isSplitted = false;

        var myPipe = new GroupByPipe();
        var Salesmens = myPipe.transform(this.EntityPM.CustomerSalesmanByProducts.filter(f => f.SalesmanUserId != null), "SalesmanUserId");

        if (Salesmens.length == 0) {
            isEnabled = true;
        }

        else if (Salesmens.length > 1) {
            isSplitted = true;
        }

        this.IsSplitted_SalesmanUser = isSplitted;
        //this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, isEnabled);

        var salesmanSettings: CustomerFieldsUpdateSettingList = this.customerFieldsUpdateSettingList.filter(f => f.ObjectFieldName == "SalesmanUserId")[0];
        if (salesmanSettings != null) {
            if (salesmanSettings.UpdateDirection == "UNFU" && this.EntityPM.CustomerStatusCode != "POT") {
                this.IsUnifreightEditable = true;
                this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            }
        }
    }
    SetUIProperties_Forwarder() {
        var isEnabled = false;
        var isSplitted = false;

        var myPipe = new GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerForwarderByProducts.filter(f => f.ForwarderId != null), "ForwarderId");

        if (Forwarders.length == 0) {
            isEnabled = true;
        }

        else if (Forwarders.length > 1) {
            isSplitted = true;
        }

        this.IsSplitted_Forwarder = isSplitted;
        this.UIProperties.SetEnabled("ForwarderId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_CustomsAgent() {
        var isEnabled = false;
        var isSplitted = false;

        var myPipe = new GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerCustomsAgentByProducts.filter(f => f.CustomsAgentId != null), "CustomsAgentId");

        if (Forwarders.length == 0) {
            isEnabled = true;
        }

        else if (Forwarders.length > 1) {
            isSplitted = true;
        }

        this.IsSplitted_CustomsAgent = isSplitted;
        this.UIProperties.SetEnabled("CustomsAgentId", this.ObjectTableName, isEnabled);
    }
    SetUIProperties_Mediator() {
        var isEnabled = false;
        var isSplitted = false;

        var myPipe = new GroupByPipe();
        var Forwarders = myPipe.transform(this.EntityPM.CustomerMediatorByProducts.filter(f => f.MediatorId != null), "MediatorId");

        if (Forwarders.length == 0) {
            isEnabled = true;
        }

        else if (Forwarders.length > 1) {
            isSplitted = true;
        }

        this.IsSplitted_Mediator = isSplitted;
        this.UIProperties.SetEnabled("MediatorId", this.ObjectTableName, isEnabled);
    }

    private CloseScreen() {
        var enabled: boolean = true;
        if (this.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
            enabled = false;
            this.IsBlockMessageVisible = true;
        }

        this.UIProperties.SetEnabled("EnglishName", "Customer", enabled);
        this.UIProperties.SetEnabled("LocalName", "Customer", enabled);
        this.UIProperties.SetEnabled("VatNumber", "Customer", enabled);
        this.UIProperties.SetEnabled("PaymentTermId", "Customer", enabled);
        this.UIProperties.SetEnabled("AccountManagerUserId", "Customer", enabled);
        this.UIProperties.SetEnabled("ClassifierId", "Customer", enabled);
        this.UIProperties.SetEnabled("CollectorId", "Customer", enabled);
        this.UIProperties.SetEnabled("ForwarderId", "Customer", enabled);
        this.UIProperties.SetEnabled("CustomsAgentId", "Customer", enabled);
        this.UIProperties.SetEnabled("MediatorId", "Customer", enabled);
        this.UIProperties.SetEnabled("KnownConsignor", "Customer", enabled);
        this.UIProperties.SetEnabled("KCExpirationDate", "Customer", enabled);
    }

    public get StartWorkingDate() { return this.EntityPM.StartWorkingDate; }
    public set StartWorkingDate(value: Date) { this.EntityPM.StartWorkingDate = value; }

    public get LeadDescription() { return this.EntityPM.LeadDescription; }
    public set LeadDescription(value: string) { this.EntityPM.LeadDescription = value; }

    public get CustomerSizeId() { return this.EntityPM.CustomerSizeId; }
    public set CustomerSizeId(value) { this.EntityPM.CustomerSizeId = value; }

    public get RegionId() { return this.EntityPM.RegionId; }
    public set RegionId(value: string) { this.EntityPM.RegionId = value; }

    public get IndustryId() { return this.EntityPM.IndustryId; }
    public set IndustryId(value: string) { this.EntityPM.IndustryId = value; }

    public get LeadSourceId() { return this.EntityPM.LeadSourceId; }
    public set LeadSourceId(value: string) {
        if (this.EntityPM.LeadSourceId != value) {
            this.EntityPM.LeadSourceId = value;
            this.SetLeadSourceName(value)
        }
    }

    private SetLeadSourceName(leadSourceId) {
        var service: LeadSourceListService = new LeadSourceListService();
        service.getSingleFromCache(leadSourceId).subscribe((result: ServiceResponse) => {
            if (!result.HasError) {
                if (result.Result != null)
                    this.EntityPM.LeadSourceName = result.Result.Name;
                else
                    this.EntityPM.LeadSourceName = null;
            }
        });        
    }
    
    ProductsToggleButtonClicked(item: ProductTypeItemClass, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerProducts.filter(d => d.ProductTypeCode == item.Code)) {
            this.BuildProductsToggleButtonList();
        }

        this.BuildProductsObsList();
    }

    ServicesToggleButtonClicked(item: ServiceItemClass, i) {
        if (item.IsChecked == true && !this.EntityPM.CustomerAdditionalServices.filter(d => d.AdditionalServiceId == item.Id)[0]) {
            this.BuildToggleButtonList();
        }

        this.BuildObsList();
    }

    public ProductsToggleButtonListFilterd: Array<ProductTypeItemClass> = [];
    public checkedProducts: Array<any> = [];
    public get CheckedProducts() {
        this.checkedProducts = [];
        for (var i = 0; i < this.ProductsToggleButtonListFilterd.length; i++) {
            if (this.ProductsToggleButtonListFilterd[i].IsChecked == true) {
                this.checkedProducts.push(this.ProductsToggleButtonListFilterd[i]);
            }
        }

        return this.checkedProducts;
    }

    public displayDelete: boolean = false;
    getProductsTitle(Item: any) {
        return "Last shipment date: " + Item.LastShipmentDate;
    }

    ExistingItemNotes(Item: ServiceViewModelData) {
        return AppTool.IsNullOrEmpty(Item.Notes);
    }
    
    DeleteItemServiceObsList(item: ServiceViewModelData) {
        if (this.EntityPM.CustomerAdditionalServices.filter(p => p.AdditionalServiceId == item.Id)[0] != null)
            this.EntityPM.RemoveCustomerAdditionalServicePM(this.EntityPM.CustomerAdditionalServices.filter(d => d.AdditionalServiceId == item.Id)[0]);      
        this.BuildToggleButtonList();
        this.BuildObsList();
    }

    deleteItemProductsObsList(item: ProductObslistItemClass) {
        if (this.EntityPM.CustomerProducts.filter(p => p.ProductTypeCode == item.ProductTypeCode)[0] != null)
            this.EntityPM.RemoveCustomerProductPM(this.EntityPM.CustomerProducts.filter(d => d.ProductTypeCode == item.ProductTypeCode)[0]);

        this.BuildProductsToggleButtonList();
        this.BuildProductsObsList();
    }

    EditItemServiceObsList(item: ServiceViewModelData) {
        var editWindow: LogitudeWindow = new LogitudeWindow();
        editWindow.Title = "Edit " + item.AdditionalServiceName + " Additional Service";
        editWindow.Width = 500;
        editWindow.Height = 350;
        editWindow.WindowArgs = item;
        this.Clone(item);
        editWindow.WindowClosed.subscribe(result => {
            if (result == "Cancel") {
                this.RejectChanges();
            }
            else {

            }

        });
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("CustomerAdditionalService", 0).subscribe(p => {
            editWindow.Show('./CommonModules/CommonCustomer/Components/EditTabs/EditCustomerAdditionalServiceComponent');
        });
    }

    EditCompetitor(Item: CompetitorViewModelData) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: Item.CompetitorId, ObjectTableName: 'Competitor', BackButtonLabel: "CRM Details" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.GetCompetitorList();
                });
            });
    }
    ImageUploadedCompleted(code) {
        this.ImageId = code;
        this.EntityPM.ImageDetailId = code;
    }
   
    Checked(code) {
        return code;
    }
    
    AddAdditionalService() {

        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity("AdditionalService").then(response => {

            var args = new EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = "AdditionalService";
            var logWindow = new LogitudeWindow();
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("AdditionalService"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });

    }

    AddCompetitor() {
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("Competitor", 0).subscribe(p => {
            var componentPath = "./Common/Components/Maintenance/CompetitorComponent";
            var args = new EntityArgs();
            args.ObjectTableName = "Competitor";
            var logWindow = new LogitudeWindow();
            logWindow.Height = 568;
            logWindow.Width = 958;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate("Competitor"));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
          
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosedCompetitor($event));
            logWindow.Show(componentPath);
        });

    }

    private myCloner: Cloner;
    private Clone(EntityPM: any) {
        this.myCloner = new Cloner(EntityPM);
        this.myCloner.AddField('InUse');
        this.myCloner.AddField('Potential');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);

    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    SetActivity(value: boolean) {
        this.EntityPM.ActivityWatch = value;
    }

    OnNewEntityWindowClosedCompetitor(event) {
        if (event == "OK") {
            this.GetCompetitorList();
        }
    }

    OnNewEntityWindowClosed(event) {
        this.GetAdditionalSerivceList();
    }
    
    private noServicesVisibility: boolean = false;
    public get NoServicesVisibility() { return this.noServicesVisibility; }
    public set NoServicesVisibility(value: boolean) { this.noServicesVisibility = value; }

    private noCompetitorVisibility: boolean = false;
    public get NoCompetitorVisibility() { return this.noCompetitorVisibility; }
    public set NoCompetitorVisibility(value: boolean) { this.noCompetitorVisibility = value; }

    private noProductsVisibility: boolean = false;
    public get NoProductsVisibility() { return this.noProductsVisibility; }
    public set NoProductsVisibility(value: boolean) { this.noProductsVisibility = value; }

    private servicesVisibility: boolean = false;
    public get ServicesVisibility() { return this.servicesVisibility; }
    public set ServicesVisibility(value: boolean) { this.servicesVisibility = value; }
    
    public RankName: string;
    public Customer: any;
    
    public RankListArr: Array<RankList> = [];

    ChangeRank(code) {

        var filteredData = this.RankListArr.filter(a => a.Code === code && a.Tenant == SessionLocator.TenantPM.Id)[0];
        this.EntityPM.RankCode = filteredData.Code;
        this.EntityPM.RankName = filteredData.Name;
        this.EntityPM.RankId = filteredData.Id;
        this.RankSource1();
        this.RankSource2();
        this.RankSource3();

    }
    
    clickItem(item) {         }
    
    isWindowViewMode: boolean;

    public searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        this.BuildProductsToggleButtonList();
        this.CD.detectChanges();
    }

    public searchTextAdditionalService: string = null;
    public get SearchTextAdditionalService() { return this.searchTextAdditionalService; }
    public set SearchTextAdditionalService(newValue: string) {
        this.searchTextAdditionalService = newValue;
        this.BuildToggleButtonList();
        this.CD.detectChanges();
    }

    public SearchTextAdditionalServiceCustomerId: string = "SearchTextAdditionalServiceId";
    public SearchTextCompetitorsCustomerId: string = "SearchTextCompetitorsId";

    public searchTextCompetitor: string = null;
    public get SearchTextCompetitor() { return this.searchTextCompetitor; }
    public set SearchTextCompetitor(newValue: string) {
        this.searchTextCompetitor = newValue;
        this.BuildCompetitorToggleButtonList();
        this.CD.detectChanges();
    }
    
    setToggleButtonMenuTemp() {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    } 
    
    setToggleButtonMenuAdditionalServicesTemp() {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    } 
    setToggleButtonMenuCompetitorTemp() {
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    } 
    
    setToggleButtonMenu() {
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    setToggleButtonMenuAdditionalServices() {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    setToggleButtonMenuCompetitor() {
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    
     public get ActivityWatch() { return this.EntityPM.ActivityWatch; }
     public set ActivityWatch(value: boolean) {
         if (this.EntityPM.ActivityWatch != value)
         this.EntityPM.ActivityWatch = value;
     }
    
     public SearchProductsModeCustomerId: string = "SearchProductsModeId";
     public SearchProductDropButtonCustomerId: string = "SearchProductDropButtonId";
     public SearchTextCompetitorsDropButtonCustomerId: string = "SearchTextCompetitorsDropButtonId";

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchProductsModeCustomerId) as HTMLInputElement;
        temp.placeholder = "";
        //this.SearchText = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }
    
    ClearPlaceHolderAdditionalService() {
        var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }
    
    ClearPlaceHolderCompetitor() {
        var temp = document.getElementById(this.SearchTextCompetitorsCustomerId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.SearchProductsModeCustomerId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    }
    OnDeleteValueAddtionalService() {
        var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId) as HTMLInputElement;
        temp.value = null;
        this.SearchTextAdditionalService = null;
        temp.focus();
    }

    OnDeleteValueCompetitor() {
        var temp = document.getElementById(this.SearchTextCompetitorsCustomerId) as HTMLInputElement;
        temp.value = null;
        this.SearchTextCompetitor = null;
        temp.focus();
    }

    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchProductsModeCustomerId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
   
    public SearchTextAdditionalServiceModeDropButtonCustomerId: string = "SearchTextAdditionalServiceModeDropButtonId";
    
    FillPlaceHolderAdditionalService() {
        if (!this.SearchTextAdditionalService) {
            var temp = document.getElementById(this.SearchTextAdditionalServiceCustomerId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    FillPlaceHoldeCompetitor() {
        if (!this.SearchTextCompetitor) {
            var temp = document.getElementById(this.SearchTextCompetitorsCustomerId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonCustomerId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    
    public RankSourceText2: string = "./Images/Icons/StarGray.png";
    public RankSourceText1: string = "./Images/Icons/StarGray.png";
    public RankSourceText3: string = "./Images/Icons/StarGray.png";
    
    DeleteItemCompetitorList(Item: CompetitorViewModelData) {
        if (this.EntityPM.CustomerCompetitors.filter(p => p.CompetitorId == Item.CompetitorId)[0] != null)
            this.EntityPM.RemoveCustomerCompetitorPM(this.EntityPM.CustomerCompetitors.filter(d => d.CompetitorId == Item.CompetitorId)[0]);
        this.BuildCompetitorToggleButtonList();
        this.BuildCompetitorsObsList();

    }
    ngOnInit() {
       

    }

    SearchTextChanged(text: string) {
        this.SearchText = text;
    }

    TextChanged(text: string) {
        this.SearchTextAdditionalService = text;
    }
    
    BuildProductsToggleButtonList() {
        var data: Array<ProductTypeList> = null;
        if (this.SearchText == null || this.SearchText == "") {
            data = this.ProductsToggleButtonList;
        }

        else {
         data = this.ProductsToggleButtonList.filter(f => f.Name.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1);

        }
        
        this.ProductsToggleButtonListFilterd = [];

        data.forEach((i) => {
            var itemTogleButton: ProductTypeItemClass = new ProductTypeItemClass(i, this.EntityPM, this, this.ProducttypeList);
            this.ProductsToggleButtonListFilterd.push(itemTogleButton);
        });
      


    }
    
    //Props
    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(newValue: string) {
        if (this.EntityPM.VatNumber != newValue) {
            this.EntityPM.VatNumber = newValue;
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(newValue: string) {
        if (this.EntityPM.PaymentTermId != newValue) {
            this.EntityPM.PaymentTermId = newValue;
        }
    }

    get Website() { return this.EntityPM.Website; }
    set Website(newValue: string) {
        if (this.EntityPM.Website != newValue) {
            this.EntityPM.Website = newValue;
        }
    }

    get KnownConsignor() { return this.EntityPM.KnownConsignor; }
    set KnownConsignor(newValue: string) {
        if (this.EntityPM.KnownConsignor != newValue) {
            this.EntityPM.KnownConsignor = newValue;
        }
    }

    get KCExpirationDate() { return this.EntityPM.KCExpirationDate; }
    set KCExpirationDate(newValue: Date) {
        if (this.EntityPM.KCExpirationDate != newValue) {
            this.EntityPM.KCExpirationDate = newValue;
        }
    }

    // Responsibilities
    public get AccountManagerUserId() { return this.EntityPM.AccountManagerUserId; }
    public set AccountManagerUserId(value: string) {
        if (this.EntityPM.AccountManagerUserId != value) {
            this.EntityPM.AccountManagerUserId = value;
        }
    }

    public get SalesmanUserId() { return this.EntityPM.SalesmanUserId; }
    public set SalesmanUserId(value: string) {
        if (this.EntityPM.SalesmanUserId != value) {
            this.EntityPM.SalesmanUserId = value;
            this.setSalesmanName(value);
        }
    }
    private setSalesmanName(salesmanId) {
        var service: UserListService = new UserListService();
        service.getSingleFromCache(salesmanId).subscribe((result: ServiceResponse) => {
            if (!result.HasError) {
                if (result.Result != null)
                    this.EntityPM.SalesmanUserEnglishName = result.Result.EnglishName;
                else
                    this.EntityPM.SalesmanUserEnglishName = null;
            }
        });    }

    public get ClassifierId() { return this.EntityPM.ClassifierId; }
    public set ClassifierId(value: string) {
        if (this.EntityPM.ClassifierId != value) {
            this.EntityPM.ClassifierId = value;
        }
    }

    public get CollectorId() { return this.EntityPM.CollectorId; }
    public set CollectorId(value: string) {
        if (this.EntityPM.CollectorId != value) {
            this.EntityPM.CollectorId = value;
        }
    }

    // Partners
    public get ForwarderId() { return this.EntityPM.ForwarderId; }
    public set ForwarderId(value: string) {
        if (this.EntityPM.ForwarderId != value) {
            this.EntityPM.ForwarderId = value;
        }
    }

    public get CustomsAgentId() { return this.EntityPM.CustomsAgentId; }
    public set CustomsAgentId(value: string) {
        if (this.EntityPM.CustomsAgentId != value) {
            this.EntityPM.CustomsAgentId = value;
        }
    }

    public get MediatorId() { return this.EntityPM.MediatorId; }
    public set MediatorId(value: string) {
        if (this.EntityPM.MediatorId != value) {
            this.EntityPM.MediatorId = value;
        }
    }

    get StorageFreeDays() { return this.EntityPM.StorageFreeDays; }
    set StorageFreeDays(newValue: number) {
        if (this.EntityPM.StorageFreeDays != newValue) {
            this.EntityPM.StorageFreeDays = newValue;
        }
    }

    // More Button
    public IsMoreButtonVisible_AccountManager: boolean = false;
    public IsMoreButtonVisible_Salesman: boolean = false;
    public IsMoreButtonVisible_Forwarder: boolean = false;
    public IsMoreButtonVisible_CustomsAgent: boolean = false;
    public IsMoreButtonVisible_Mediator: boolean = false;
    SetMoreButtonsVisibility() {

        if (FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERACCOUNTMANAGERBYPRODUCT")) {
            this.IsMoreButtonVisible_AccountManager = true;
        }

        if (FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERSALESMANBYPRODUCT")) {
            this.IsMoreButtonVisible_Salesman = true;
        }

        if (FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERFORWARDERBYPRODUCT")) {
            this.IsMoreButtonVisible_Forwarder = true;
        }

        if (FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERCUSTOMSAGENTBYPRODUCT")) {
            this.IsMoreButtonVisible_CustomsAgent = true;
        }

        if (FeatureLocator.HasFeaturePermession("Customer", "CUSTOMERMEDIATORBYPRODUCT")) {
            this.IsMoreButtonVisible_Mediator = true;
        }
    }
    MoreButtonClicked(field: string) {

        var windowTitle: string = null;
        var windowComponent: string = null;

        switch (field) {
            case "AccountManagerUserId": {
                windowTitle = "Customer Account Manager By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerAccountManagerByProductSplitComponent";
                break;
            }

            case "SalesmanUserId": {
                windowTitle = "Customer Salesman By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerSalesmanByProductSplitComponent";
                break;
            }

            case "ForwarderId": {
                windowTitle = "Customer Forwarder By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerForwarderByProductSplitComponent";
                break;
            }

            case "CustomsAgentId": {
                windowTitle = "Customer Customs Agent By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerCustomsAgentByProductSplitComponent";
                break;
            }

            case "MediatorId": {
                windowTitle = "Customer Mediator By Product";
                windowComponent = "./CommonModules/CommonCustomer/Components/EditTabs/MoreButtons/CustomerMediatorByProductSplitComponent";
                break;
            }
        }

        if (windowComponent != null) {
            var window = new LogitudeWindow();
            window.Title = windowTitle;
            window.WindowArgs = { EntityPM: this.EntityPM, ProductTypes: this.AllProductTypes, IsUnifreightEditable :this.IsUnifreightEditable }
            window.Show(windowComponent);
            window.WindowClosed.subscribe(s => {
                if (s == "OK") {
                    this.SetUIProperties_Partners();
                    this.CloseScreen();
                }
            });
        }
    }
}

export class ProductTypeItemClass {
    private entityPM: CustomerPM;
    private entityList: ProductTypeList;
    public TenantPM: TenantPM;
    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; } 

    public get DirectionId() { return this.entityList.Code.substr(1, 1); }
    public get TransportModeId() { return this.entityList.Code.substr(0, 1); }

    public ProductTypesByTenantList = [];

    constructor(itemList: ProductTypeList, itemPM: CustomerPM, private Parent: any, private productTypeList: Array<any>) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.ProductTypesByTenantList = productTypeList;
        this.entityList = itemList;
        var isCheckBoxEnabled: boolean = true;
        var isChecked = null;
        var productPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.entityList.Code)[0];
        this.isChecked = false;
        if (productPM != null) {
            isChecked = true;
            this.IsChecked = true;
        }
        if (isChecked) {
          
            if (productPM != null) {
                var lastShipmentDate: Date = productPM.LastShipmentDate;

                if (lastShipmentDate != null) {
                    isCheckBoxEnabled = false;
                }
            }
        }

        this.IsCheckBoxEnabled = isCheckBoxEnabled;

    }
    private isCheckBoxEnabled: boolean;
    public get IsCheckBoxEnabled() {
        return this.isCheckBoxEnabled;

    }
    public set IsCheckBoxEnabled(value: boolean) {
        if (this.isCheckBoxEnabled != value)
            this.isCheckBoxEnabled = value;

    }

    public get Code() {
        return this.entityList.Code;
    }   

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            if (value) {
                var newItem: CustomerProductPM = new CustomerProductPM(null);
                newItem.Tenant = this.TenantPM.Id;
                newItem.CustomerId = this.entityPM.Id;
                newItem.ProductTypeCode = this.Code;
                newItem.CommitmentChargeableWeight = 0;
                newItem.PotentialChargeableWeight = 0;
                newItem.CommitmentTEU = 0;
                newItem.PotentialTEU = 0;
                newItem.CommitmentNumberOfShipments = 0;
                newItem.PotentialNumberOfShipments = 0;
                newItem.CommitmentRevenue = 0;
                newItem.PotentialRevenue = 0;


                var type: string = null;

                var ProductsToggleButtonList = [];

                this.ProductTypesByTenantList.forEach((i) => {
                    if (!i.InActive) {
                        var item = new ProductTypeList();
                        item.Code = i.Code;
                        item.Name = i.Name;
                        item.InActive = i.InActive;
                        item.Id = i.Id;
                        item.SearchFields = i.SearchFields;

                        ProductsToggleButtonList.push(i);

                    }
                    else {


                    }
                });

                ProductsToggleButtonList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
                var typelist: ProductTypeList = ProductsToggleButtonList.filter(d => d.Code == this.Code)[0];
                if (typelist != null) {
                    type = typelist.Name;
                }
                newItem.ProductTypeName = type;
                var flag: boolean = true;
                for (var i = 0; i < this.entityPM.CustomerProducts.length; i++) {
                    if (this.entityPM.CustomerProducts[i].ProductTypeCode == newItem.ProductTypeCode) {
                        flag = false; break;
                    }
                }
                if (flag) {
                    this.entityPM.AddCustomerProductPM(newItem);
                }

                if (!this.entityPM.ActivityWatch)
                    this.entityPM.ActivityWatch = true;
            }
            else {
                var item: CustomerProductPM = this.entityPM.CustomerProducts.filter(d => d.ProductTypeCode == this.Code)[0];
                if (item != null) {
                    if (this.entityPM.CustomerProducts.includes(item)) {
                        var CustomerProdArr: Array<CustomerProductPM>= [];
                        this.entityPM.CustomerProducts.forEach(i => {
                            if (i.ProductTypeCode != item.ProductTypeCode) {
                                CustomerProdArr.push(i);
                            }
                        });
                        this.entityPM.RemoveCustomerProductPM(this.entityPM.CustomerProducts.filter(p => p.ProductTypeCode == this.Code)[0]);
                    }
                }
            }
        }
    }
}

class ProductTypeList {
    public id: string;
    public code: string;
    public name: string;
    public inActive: boolean
    public searchFields: string;

    public get Id() { return this.id; }
    public set Id(value: string) { this.id = value; }

    public get Code() { return this.code; }
    public set Code(value: string) { this.code = value; }


    public get Name() { return this.name; }
    public set Name(value: string) { this.name = value; }


    public get InActive() { return this.inActive; }
    public set InActive(value: boolean) { this.inActive = value; }

    public get SearchFields() { return this.searchFields; }
    public set SearchFields(value: string) { this.searchFields = value; }

}

class AdditionalServiceList {
    public id: string;
    public tenant: string;
    public name: string;
    public inActive: boolean
    public searchFields: string;

    public get Id() { return this.id; }
    public set Id(value: string) { this.id = value; }

    public get Tenant() { return this.tenant; }
    public set Tenant(value: string) { this.tenant = value; }


    public get Name() { return this.name; }
    public set Name(value: string) { this.name = value; }


    public get InActive() { return this.inActive; }
    public set InActive(value: boolean) { this.inActive = value; }

    public get SearchFields() { return this.searchFields; }
    public set SearchFields(value: string) { this.searchFields = value; }
}

export class ServiceViewModelData {
    private entityPM: CustomerAdditionalServicePM;
    private trigger: CustomerGeneralTabComponent;
    
    public get InUse() {
        return !this.entityPM.Potential;
    }
    public set InUse(value: boolean) {
            if (this.entityPM.Potential != !value)
            this.entityPM.Potential = !value;                    


    }

    private typeLabel: string;
    public get TypeLabel() {
        if (this.entityPM.Potential)
            return "Potential";
        return "In Use";


    }

    public get Potential() {

        return this.entityPM.Potential;
    }
    public set Potential(value: boolean) {
            if (this.entityPM.Potential !=value)
            this.entityPM.Potential = value;

        }    

    public get AdditionalServiceName() { return this.entityPM.AdditionalServiceName; }
    
    public get Notes() {
        return this.entityPM.Notes;

    }
    public set Notes(value: string) {
        if (this.entityPM.Notes != value) {
            this.entityPM.Notes = value;
            ServiceLocator.SendTotangoUserActivity("Customer", "Notes update");
        }

    }

    private brushedNotesIconVisibility: boolean;
    public get BrushedNotesIconVisibility() {
        if (this.entityPM != null && !(this.entityPM.Notes == null || this.entityPM.Notes==""))
        return true;
        return false;

    }

    private defaultNotesIconVisibility: boolean;
    public get DefaultNotesIconVisibility() {
        if (this.entityPM != null && (this.entityPM.Notes == null || this.entityPM.Notes == ""))
            return true;
        return false;

    }
    public get Id() { return this.entityPM.AdditionalServiceId }

    constructor(item: CustomerAdditionalServicePM, trigger: CustomerGeneralTabComponent) {
        this.entityPM = item;
        this.trigger = trigger;
        this.InUse = !this.Potential;
    }
}

export class CompetitorViewModelData {

    private entityPM: CustomerCompetitorPM;
    private trigger: CustomerGeneralTabComponent;



    public get CompetitorId() { return this.entityPM.CompetitorId; }

    constructor(item: CustomerCompetitorPM, trigger: CustomerGeneralTabComponent) {
        this.entityPM = item;
        this.trigger = trigger;
    }

    public get Name() {
        var result: string = "";
        var list: CompetitorList = this.trigger.AllCompetitors.filter(d => d.Id == this.entityPM.CompetitorId)[0];
        if (list != null) {
            result = list.Name;
        }

        return result;


    }
}

class ProductObslistItemClass {
    private entityPM: CustomerProductPM;
    private trigger: CustomerGeneralTabComponent;
    constructor(item: CustomerProductPM, trigger: CustomerGeneralTabComponent) {
        this.entityPM = item;
        this.trigger = trigger;
        this.GetProperties();
    }

    public get Name() { return this.entityPM.ProductTypeName; }
    public get ProductTypeCode() { return this.entityPM.ProductTypeCode; }

    public IsDeleteButtonEnabled: boolean = false;
    public PrepaidCollectTypeLabel: string = "";
    public TypeLabel: string = "Potential";
    public LastShipmentDate: string = "No Shipmnets";
    GetProperties() {
        this.IsDeleteButtonEnabled = this.entityPM.LastShipmentDate == null ? true : false;

        if (!AppTool.IsNullOrEmpty(this.entityPM.PrepaidCollectId)) {
            if (this.entityPM.PrepaidCollectId == "P") {
                this.PrepaidCollectTypeLabel = "Prepaid";
            }

            else if (this.entityPM.PrepaidCollectId == "C") {
                this.PrepaidCollectTypeLabel = "Collect";
            }
        }

        if (this.entityPM.LastShipmentDate != null) {
            var myDateFormats = DateTool.GetDateFormats(this.entityPM.LastShipmentDate);
            this.LastShipmentDate = myDateFormats.DateString;
            this.TypeLabel = myDateFormats.ShortDateString;
        }
    }
}

class CompetitorItemClass {

    private trigger: CustomerGeneralTabComponent;
    private entityPM: CustomerPM;
    private entityList: CompetitorList ;
    constructor(item: CompetitorList, entityPM: CustomerPM ,trigger: CustomerGeneralTabComponent) {
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.CustomerCompetitors.filter(d => d.CompetitorId == this.entityList.Id)[0] != null;

    }
    public get Name() { return this.entityList.Name; }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            if (value) {
                var newItem: CustomerCompetitorPM = new CustomerCompetitorPM(null)
                newItem.Tenant = SessionLocator.Tenant;
                newItem.CustomerId = this.entityPM.Id;
                newItem.CompetitorId =this.entityList.Id;
                newItem.CompetitorName = this.entityList.Name;
                if (!this.entityPM.CustomerCompetitors.includes(newItem)) {
                    this.entityPM.AddCustomerCompetitorPM(newItem);
                }                              
            }

            else {

                var item: CustomerCompetitorPM = this.entityPM.CustomerCompetitors.filter(d => d.CompetitorId == this.entityList.Id)[0];

                if (item != null) {
                    if (this.entityPM.CustomerCompetitors.includes(item)) {
                        this.entityPM.RemoveCustomerCompetitorPM(item);
                    }
                }
            }

            this.trigger.BuildCompetitorsObsList();
            this.trigger.BuildToggleButtonList();


            }

        }


    



}

class ServiceItemClass {    
    private entityPM: CustomerPM;
    private entityList: AdditionalServiceList;
    public TenantPM: TenantPM;
    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; }

    public get Id() { return this.entityList.Id; }


    constructor(itemList: AdditionalServiceList, itemPM: CustomerPM, private Parent: CustomerGeneralTabComponent) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.entityList = itemList;
        this.isChecked = this.entityPM.CustomerAdditionalServices.filter(d => d.AdditionalServiceId == this.entityList.Id)[0] != null;

    }
    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {

        if (this.isChecked != value) {
            this.isChecked = value;


            if (value) {

                var notesRightToLeft = false;
                if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                    notesRightToLeft = true;
                }

                var newItem: CustomerAdditionalServicePM = new CustomerAdditionalServicePM(null);

                newItem.Tenant = SessionLocator.Tenant;
                newItem.CustomerId = this.entityPM.Id;
                newItem.AdditionalServiceId = this.Id;
                newItem.Potential = true;
                newItem.NotesRightToLeft = notesRightToLeft;


                var type: string = null;

                var addtionalService: AdditionalServiceListService = new AdditionalServiceListService();
                addtionalService.getSingleFromCache(this.Id).subscribe(result => {
                    var typeList = result.Result;
                    if (typeList != null) {
                        type = typeList.Name;
                    }
                    newItem.AdditionalServiceName = type;

                    if (!this.entityPM.CustomerAdditionalServices.includes(newItem)) {
                        this.entityPM.AddCustomerAdditionalServicePM(newItem);
                    }

                });


                if (!this.entityPM.ActivityWatch) {
                    this.entityPM.ActivityWatch = true;
                }
            }

            else {
                var item: CustomerAdditionalServicePM = this.entityPM.CustomerAdditionalServices.filter(d => d.AdditionalServiceId == this.Id)[0];
                if (item != null) {
                    if (this.entityPM.CustomerAdditionalServices.includes(item)) {
                        this.entityPM.RemoveCustomerAdditionalServicePM(item);
                        }
                    }
                }

            this.Parent.BuildObsList();
            this.Parent.BuildToggleButtonList();            

        }
    }

    
}
