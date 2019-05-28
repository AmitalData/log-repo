import {Component, OnInit,Output,EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomerProductExtendedService} from '../../Common/Services/ExtendedPMs/CustomerProductExtendedService';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {CustomerProductPM} from '../../Common/EntityPMs/CustomerProductPM';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomerTenantAccessPM} from '../../Common/EntityPMs/CustomerTenantAccessPM';
import {CustomerTenantAccessCardPM} from '../../Common/EntityPMs/CustomerTenantAccessCardPM';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import {CardList} from '../../Common/EntityLists/CardList';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {PartnersDomainService} from '../../Common/Services/PartnersDomainService';
import {CustomerTenantAccessCardsBatchPM} from  '../../Common/EntityPMs/CustomerTenantAccessCardsBatchPM';
import {DateTool} from '../../Infrastructure/Tools';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {QueueMessageMoreDetailsListService} from '../../Infrastructure/Services/StandardLists/QueueMessageMoreDetailsListService';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {APILogsListService} from '../../Infrastructure/Services/StandardLists/APILogsListService';
import {UserListService} from '../../Common/Services/StandardLists/UserListService';
import {UserList} from '../../Common/EntityLists/UserList';
declare var window;
@Component({
    moduleId: module.id,
    selector: 'RelatedCustomerComponent',
    templateUrl: './RelatedCustomerComponent.html',
})

export class RelatedCustomerComponent extends BaseComponent{

    public EntityPM: CustomerTenantAccessPM;
    public customerPM: CustomerPM;
    public ObjectTableName: string;
    public SelectedCardPM: CustomerTenantAccessCardPM;
    public DataContext: RelatedCustomerComponent = this;
    public ObsList: Array<AddEditCustomerTenantAccessCardViewModel>=[];
    public BatchObsList: Array<CustomerTenantAccessCardsBatchDataViewModel> = [];
    public QueryObsList: Array<any> = [];
    public APILogsObsList: Array<any> = [];
    public SelectedQueryItem: BatchQueriesData;
    public BatchTitle: string;
    public RealCustomerTenantAccessPM: CustomerTenantAccessPM;
    public QueueNoDataTextBlockVisibility: boolean = false;
    public QueriesList: Array<BatchQueriesData> = [];
    private selectedLogItem: BatchQueriesData;
    public IsShowTipIcon: boolean = false;
    public get SelectedLogItem() { return this.selectedLogItem; }
    public set SelectedLogItem(value: BatchQueriesData) {
        if (this.selectedLogItem != value)
            this.selectedLogItem = value;
    }

    ViewLog(itemComponent) {
        if (itemComponent.Id != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: itemComponent.Id, ObjectTableName: 'APILogs', BackButtonLabel: "Back" });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        
                    });
                });
        }
    }
    public  FillQueriesList() {
        var TodayBatch: BatchQueriesData = new BatchQueriesData(0,"Today");
        var LastWeekBatch: BatchQueriesData = new BatchQueriesData(1, "Last Week");
        var LastMonthBatch: BatchQueriesData = new BatchQueriesData(2, "Last Month");
        this.QueriesList.push(TodayBatch);
        this.QueriesList.push(LastWeekBatch);
        this.QueriesList.push(LastMonthBatch);
        this.SelectedQueryItem = this.QueriesList[1];
        this.SelectedLogItem = this.QueriesList[1];
    }
    public IsShowTipArea: boolean = false;
    public IsTipsOpened: boolean = false;
    public IsFirstTipLoad: boolean = true;
    TipVisibilityChanged() {
        this.IsTipsOpened = !this.IsTipsOpened;
    }
   
    public TenantAccessCard: CustomerTenantAccessCardPM;
    public EditRelatedCustomer(item: AddEditCustomerTenantAccessCardViewModel) {
        this.TenantAccessCard = item.EntityPM;
        var service: CommonDomainService = new CommonDomainService();
        this.CurrentSession.StartBusyIndicatorLoading();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(res => {
            if (!res.HasError) {
                this.RealCustomerTenantAccessPM = res.Result;
                var entityService: EntityResourceService = new EntityResourceService();
                entityService.getEntityResourceByTableName("CustomerTenantAccessCard", 0).subscribe(p => {
                    var logitudeWindow: LogitudeWindow = new LogitudeWindow();
                    logitudeWindow.WindowArgs = { EntityPM: this.EntityPM.CustomerTenantAccessCards.filter(a => a.CustomerId == this.TenantAccessCard.CustomerId)[0], Parent: this };
                    logitudeWindow.Title = "Edit Card" + " - " + this.TenantAccessCard.CustomerCode + " - " + this.TenantAccessCard.CustomerName;
                    logitudeWindow.Show('./SharedLogistics/Components/EditRelatedCustomerComponent');
                    logitudeWindow.ComponentLoaded.subscribe(p => {
                        this.CurrentSession.StopBusyIndicator();
                    });
                    logitudeWindow.WindowClosed.subscribe(p => {
                        if (p == "OK") {
                            this.BuildData();
                        }

                    });
                });
            }


        });


    }

    LogsSelectedChange($event) {
        this.SelectedLogItem = $event;
        var service: APILogsListService = new APILogsListService();
        var filters: ApiQueryFilters = new ApiQueryFilters();

        var TodayDate = DateTool.TruncateTime(new Date());
        var YesterdayDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -1);
        var LastSevenDaysDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -7)
        var LastThirtyDaysDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -30);
        var value: Date;
        if ($event.Name == "Today") {
            value = TodayDate;
        }      
        else if ($event.Name == "Last Week") {
            value = LastSevenDaysDate;
        }
        else if ($event.Name == "Last Month") {
            value = LastThirtyDaysDate;
        }
        filters.addAdditionalFilter("CreateDate", value, null, null, "GreaterThanOrEqual", false, true, false, "datetime");
        filters.PageSize = 100;
        filters.PageIndex = 0;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        service.getByFilters(filters).subscribe(result => {
            this.APILogsObsList = result.Result.sort((a, b) => { return (DateTool.GetDateFromDate(a.CreateDate) === DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (DateTool.GetDateFromDate(a.CreateDate) > DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1 });

        });
    }
    QueriesSelectedChange($event) {
        var service: QueueMessageMoreDetailsListService = new QueueMessageMoreDetailsListService();
        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageSize = 100;
        filters.PageIndex = 0;
        filters.SortBy = "CreateDateTime";
        filters.SortDirection = "Descending";
        var TodayDate = DateTool.TruncateTime(new Date());
        var YesterdayDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -1);
        var LastSevenDaysDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -7)
        var LastThirtyDaysDate = DateTool.AddDays(DateTool.GetDateParts(new Date()).DateObject, -30);
        var value: Date;
        if ($event.Name == "Today") {
            value = TodayDate;
        }
        else if ($event.Name == "Last Week") {
            value = LastSevenDaysDate;
        }
        else if ($event.Name == "Last Month") {
            value = LastThirtyDaysDate;
        }

        filters.addAdditionalFilter("CreateDateTime", value, null, null, "GreaterThanOrEqual", false, true, false, "datetime");




        service.getByFilters(filters).subscribe(result => {
            this.QueryObsList = result.Result;
        });
    }

    AddRelatedCustomer() {
        var service: CommonDomainService = new CommonDomainService();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(res => {
            if (!res.HasError) {
                this.RealCustomerTenantAccessPM = res.Result;
                var customerTenantAccessCardPM: CustomerTenantAccessCardPM = new CustomerTenantAccessCardPM(this.EntityPM);
               
                customerTenantAccessCardPM.CustomerTenantAccessId = this.RealCustomerTenantAccessPM.Id;
                customerTenantAccessCardPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                customerTenantAccessCardPM.UpdateDateTime = DateTool.GetCurrentDateTimeAsUtc();
                customerTenantAccessCardPM.CreateByUserId = SessionLocator.LoggedUserPM.EnglishName;

                var viewModel: AddEditCustomerTenantAccessCardViewModel = new AddEditCustomerTenantAccessCardViewModel(this.RealCustomerTenantAccessPM, customerTenantAccessCardPM, true, this);
                viewModel.DataLoaded.subscribe(output => {
                    if (output) {
                        var logitudeWindow: LogitudeWindow = new LogitudeWindow();
                        logitudeWindow.WindowArgs = viewModel;
                        logitudeWindow.Title = TextCodeTranslator.Translate("CustomerTenantAccess.O.RelatedCustomers.AddRelatedCustomer");
                        logitudeWindow.Show('./SharedLogistics/Components/AddEditCustomerTenantAccessCardComponent');
                    } 
                });
               
            }
        });


    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs,private _entityResourceService: EntityResourceService) {
        super();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccess", 0).subscribe(response => {
            this.EntityPM = entityArgs.EntityPM;
            this.ObjectTableName = "CustomerTenantAccess";
            if (this.EntityPM.CustomerTenantAccessCards.length == 0) {
                this.IsShowTipArea = true;
                var table = window.ObjectTables.filter(d => d.Name == "CustomerTenantAccessCard" && (d.Tenant == SessionInfo.LoggedUserTenant || d.Tenant == 0))[0];
                if (table) {
                    this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccessCard", 0).subscribe(response => {
                        var Tip = window.Tips.filter(d => d.Code == "NCDT" && d.ObjectTableId == table.Id)[0];
                        var TipsVisibility = window.TipsVisibilities.filter(d => d.TipCode == Tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];
                        if (TipsVisibility!=null)
                            this.IsTipsOpened = TipsVisibility.IsVisible;
                        else
                            this.IsTipsOpened = false;

                    });
                }
            }
            this.BuildData();
            this.FillQueriesList();   
        });
     
    }

    private isAddEnabled: boolean=true;
    public get IsAddEnabled() { return this.isAddEnabled; }
    public set IsAddEnabled(value: boolean) { if (this.isAddEnabled != value) this.isAddEnabled = value; }

    private batchVisibility: boolean = false;
    public get BatchVisibility() { return this.batchVisibility; }
    public set BatchVisibility(value: boolean) { if (this.batchVisibility != value) this.batchVisibility = value; }

    private selectedItem: AddEditCustomerTenantAccessCardViewModel;
    private selectedItemBatch: CustomerTenantAccessCardsBatchDataViewModel;
    public get SelectedItemBatch() { return this.selectedItemBatch; }
    public set SelectedItemBatch(value: CustomerTenantAccessCardsBatchDataViewModel) { if (this.selectedItemBatch != value) this.selectedItemBatch = value; }
    private selectedTabCode;
    public get SelectedTabCode() { return this.selectedTabCode; }
    public set SelectedTabCode(value: string) {
        if (this.selectedTabCode != value) {
            this.selectedTabCode = value; 
            this.SetBatchTitle();    
                  
        }
    }

    RefreshRelatedCustomers() {
        this.BuildData();
    }

    RefreshBatch() {
        this.getBatchData();
    }

    RefreshLogs() {
        this.LogsSelectedChange(this.SelectedLogItem);
    }

    RefreshQueues() {
        this.QueriesSelectedChange(this.SelectedQueryItem);
    }

    AddBatch() {

        if (this.SelectedCardPM != null) {
            var logitudeWindow: LogitudeWindow = new LogitudeWindow();
            logitudeWindow.WindowArgs = { AccessCardPM: this.SelectedCardPM, Parent: this };
            logitudeWindow.Title = "Add New Batch";
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Show('./SharedLogistics/Components/AddCustomerBatchComponent');
            logitudeWindow.WindowClosed.subscribe(p => {
                if (p == "OK") {
                    this.BuildData();
                }
            });         
        }
    }

    SetBatchTitle() {
        if (this.SelectedTabCode == "B") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Batch Build History (last 100)";
        }
        else if (this.SelectedTabCode == "L") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Logs History (last 100)";
            this.LogsSelectedChange(this.SelectedLogItem);
        }
        else if (this.SelectedTabCode == "Q") {
            this.BatchTitle = "Card " + this.SelectedItem.EntityPM.CustomerCode + " - " + this.SelectedItem.CustomerName + " Queues History (last 100) ";
            this.QueriesSelectedChange(this.SelectedQueryItem);
        }
    }
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(value: AddEditCustomerTenantAccessCardViewModel) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
            this.SelectedCardPM = value.EntityPM;
            this.getBatchData();
        }       
    }
    getBatchData() {
        this.BatchVisibility = true;
        var service: CommonDomainService = new CommonDomainService();
        this.BatchObsList = [];
        service.GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(this.SelectedItem.EntityPM.CustomerId, this.SelectedItem.EntityPM.CustomerTenantAccessId).subscribe(res => {
            if (!res.HasError) {
                var CustomerTenantAccessCardsBatchpms: Array<CustomerTenantAccessCardsBatchPM> = res.Result;
                CustomerTenantAccessCardsBatchpms.forEach(item => {
                    this.BatchObsList.push(new CustomerTenantAccessCardsBatchDataViewModel(item));
                });
            }
            this.SelectedTabCode = "B";
            this.SetBatchTitle();
        });
    }
    
    BuildData() {
        this.IsEnabled = false;
        var service: CommonDomainService = new CommonDomainService();
        service.GetSingleCustomerTenantAccess(this.EntityPM.Id).subscribe(res => {
            this.ObsList = [];
            if (!res.HasError) {
                var list: Array<CustomerTenantAccessCardPM> = res.Result.CustomerTenantAccessCards;
                var customerTenantAccesspm: CustomerTenantAccessPM = res.Result;               
                list.forEach(item => {
                    this.ObsList.push(new AddEditCustomerTenantAccessCardViewModel(res.Result, item, false, this.DataContext));
                });
                if (this.ObsList.length > 0)
                    this.SelectedItem = this.ObsList[0];

                var temp = this.ObsList.filter(a => a.StatusTypeCode != "IA");
                if (customerTenantAccesspm.IsPrivateLabelCustomer == true) {
                    if (temp.length > 0) {
                        this.IsAddEnabled = false;
                    }
                    else {
                        this.IsAddEnabled = true;
                    }
                }
                this.IsEnabled = true;
                if (this.ObsList == null || this.ObsList.length == 0) {
                    this.TipVisibility = true;
                }
                else {
                    this.TipVisibility = false;
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });

    }
    private tipVisibility: boolean = false;
    public get TipVisibility() { return this.tipVisibility; }
    public set TipVisibility(value: boolean) { if (this.tipVisibility != value) this.tipVisibility = value; }

    private isEnabled: boolean = true;
    public get IsEnabled() { return this.isEnabled; }
    public set IsEnabled(value: boolean) { if (this.isEnabled != value) this.isEnabled = value; }

}

export class AddEditCustomerTenantAccessCardViewModel   extends BaseComponent {
    public EntityPM: CustomerTenantAccessCardPM;
    public customertenantAccessPM: CustomerTenantAccessPM;
    public CardObsList: Array<CardListDataViewModel>=[];
    public isNew: boolean = false;
    public Parent: RelatedCustomerComponent;
    @Output() DataLoaded = new EventEmitter();
    public DataContext: AddEditCustomerTenantAccessCardViewModel = this;
    constructor(customertenantAccessPM: CustomerTenantAccessPM, entityPM: CustomerTenantAccessCardPM, isNew: boolean, Parent: RelatedCustomerComponent) {
        super();
        this.EntityPM = entityPM;
        this.customertenantAccessPM = customertenantAccessPM;
        this.isNew = isNew;
        this.Parent = Parent;
        this.setCreatedByName();
        if (isNew) {
            this.LoadCardList();
        }
    }    

    public setCreatedByName() {
        var service: UserListService = new UserListService();
        service.getSingleFromCache(this.EntityPM.CreateByUserId).subscribe(resp => {
            if (!resp.HasError) {
                var result: ServiceResponse = resp;
                var list: UserList = result.Result;
                if (list != null) {
                    this.createByUserId = list.EnglishName;
                }
            }
        });
    }
    LoadCardList() {
        var service: PartnersDomainService = new PartnersDomainService();
        this.CardObsList = [];
        service.GetCustomerCardListByTenantVatNumber(this.customertenantAccessPM.CompanyVat).subscribe(res => {
            if (!res.HasError) {
                var tempList: Array<CardListDataViewModel> = [];
                var list: Array<CardList> = res.Result;
                list.forEach(item => {
                    var AccessCardPM = this.customertenantAccessPM.CustomerTenantAccessCards.filter(a => a.Tenant == item.Tenant && a.CustomerId == item.Id)[0];
                    this.CardObsList.push(new CardListDataViewModel(item, this.customertenantAccessPM, this, AccessCardPM));
                });
                  this.DataLoaded.emit(true);
            }
        });
    }

    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(value: string) { if (value != this.EntityPM.CustomerId) this.EntityPM.CustomerId = value; }
    public get CreateDate() { return this.EntityPM.CreateDate; }
    public get UpdateDateTime() { return this.EntityPM.UpdateDateTime; }
    public get LastShipmentDateInQueue() { return this.EntityPM.LastShipmentDateInQueue; }
    public get StatusType() { return this.EntityPM.StatusType; }
    public set StatusType(value: string) { if (this.EntityPM.StatusType != value) this.EntityPM.StatusType = value; }
    public get StatusTypeCode() { return this.EntityPM.StatusTypeCode; }
    public set StatusTypeCode(value: string) { if (this.EntityPM.StatusTypeCode != value) this.EntityPM.StatusTypeCode = value; }
    private createByUserId: string;
    public get CreateByUserId() {
        return this.createByUserId;
    }

    public set CreateByUserId(value: string) {
        if (this.createByUserId != value)
            this.createByUserId = value;
    }
    public get CustomerCode() { return this.EntityPM.CustomerCode; }
    public set CustomerCode(value: string) { if (this.EntityPM.CustomerCode != value) this.EntityPM.CustomerCode = value; }
    public get CustomerName() { return this.EntityPM.CustomerName; }
    public set CustomerName(value: string) { if (this.CustomerName != value) this.EntityPM.CustomerName = value; }
    public get HybridStartDate() {
        if (this.EntityPM.HybridStartDate == null || this.EntityPM.HybridStartDate.getFullYear() == 1 || this.EntityPM.HybridStartDate == DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
                var date = DateTool.GetCurrentDateTimeAsUtc();
                date.setDate(date.getDate() - 14);
                this.EntityPM.HybridStartDate = date            
        }
        return this.EntityPM.HybridStartDate;

    }
    public set HybridStartDate(value: Date) { if (this.EntityPM.HybridStartDate != value) this.EntityPM.HybridStartDate = value; }

    private changeHybridStartDateEnable: boolean = false;
    public get ChangeHybridStartDateEnable() {
        if (this.EntityPM.StatusType == "Accepted") {
            return true;
        }
        else {
            return false;
        }
    }
    public set ChangeHybridStartDateEnable(value: boolean) {
        if (this.changeHybridStartDateEnable != value)
            this.changeHybridStartDateEnable = value;
    }


}


export class CardListDataViewModel {
    public entityList: CardList;
    public CustomerTenantAccessPM: CustomerTenantAccessPM;
    public Parent: AddEditCustomerTenantAccessCardViewModel;
    public AccessCardsPms: CustomerTenantAccessCardPM;
    public publicAccessCard: CustomerTenantAccessCardPM;

    constructor(entityList: CardList, customertenantAccessPM: CustomerTenantAccessPM, Parent: AddEditCustomerTenantAccessCardViewModel, AccessCard: CustomerTenantAccessCardPM) {
        this.entityList = entityList;
        this.CustomerTenantAccessPM = customertenantAccessPM;
        this.Parent = Parent;
        this.AccessCardsPms = AccessCard;
        var service: CommonDomainService = new CommonDomainService();
        service.GetCustomerTenantAccessCard(this.entityList.Id).subscribe(res => {
            if (!res.HasError) {
                var AccessCards = res.Result;
                if (AccessCards != null) {
                    this.publicAccessCard = AccessCards;
                } 
            }
        });
    }


    public get IsCustomerCanChecked() {
        var isCustomerCanChecked: boolean = true;
        if (this.publicAccessCard != null)
        {
            isCustomerCanChecked = false;
        }
        return isCustomerCanChecked;
    }
    private isCustomerCanChecked;

    public get IsCustomerCanCheckSubmiting() { return this.isCustomerCanChecked; }
    public set IsCustomerCanChecked(value: boolean) { if (value != this.isCustomerCanChecked) this.isCustomerCanChecked = value; }

    public get Id() { return this.entityList.Id; }

    public get IsCardsHasSelectedOpacity() {
        var isCardsHasSelectedOpacity: number = 1;
        if (this.CustomerTenantAccessPM.CustomerTenantAccessCards.filter(s => s.CustomerId == this.Id)[0]) {
            isCardsHasSelectedOpacity = 0.5;
        }
        return isCardsHasSelectedOpacity;
    }

    public get EnglishName() { return this.entityList.EnglishName; }
    public set EnglishName(value: string) { if (this.entityList.EnglishName != value) this.entityList.EnglishName = value; }
    public get VatNumber() { return this.entityList.VatNumber; }
    public set VatNumber(value: string) { if (this.entityList.VatNumber != value) this.entityList.VatNumber = value; }


    public get Code() { return this.entityList.Code; }
    private isSelected: boolean = false;
    public get IsSelected() {
        if (this.AccessCardsPms != null || !this.IsCustomerCanChecked) {
            return true;
        }
        return this.isSelected;
    }
    public get IsSelectedSubmited() { return this.isSelected; }
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        if (value) {
            this.UnckechOthers();
        }

    }

    UnckechOthers() {
        this.Parent.CardObsList.forEach(item => {
            if (item.IsCustomerCanChecked) {
                if (this.Id != item.Id) {
                    item.IsSelected = false;
                }
            }
        });
          
    }

    




}


export class BatchQueriesData{

    constructor(code: number, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: number;
    public get Code() {
        return this.code;
    }
    public set Code(value: number) { if (this.code != value) this.code = value; }

    private name: string;

    public get Name() { return this.name; }
    public set Name(value: string) { if (this.name != value) this.name = value; }


}


export class CustomerTenantAccessCardsBatchDataViewModel   {

    public EntityPM: CustomerTenantAccessCardsBatchPM;

    constructor(entityPM: CustomerTenantAccessCardsBatchPM) {
        this.EntityPM = entityPM;
    }

    public get BatchNumber() { return this.EntityPM.BatchNumber; }
    public set BatchNumber(value: string) { if (this.EntityPM.BatchNumber != value) this.EntityPM.BatchNumber = value; }

    public get DoneDate() { return this.EntityPM.DoneDate; }
    public set DoneDate(value: Date) { if (this.EntityPM.DoneDate != value) this.EntityPM.DoneDate = value; }


    public get CreateDateTime() { return this.EntityPM.CreateDateTime; }
    public set CreateDateTime(value: Date) { if (this.EntityPM.CreateDateTime != value) this.EntityPM.CreateDateTime = value; }



    public get FromDatetime() { return this.EntityPM.FromDatetime; }
    public set FromDatetime(value: Date) { if (this.EntityPM.FromDatetime != value) this.EntityPM.FromDatetime = value; }


    public get ToDatetime() { return this.EntityPM.ToDatetime; }
    public set ToDatetime(value: Date) { if (this.EntityPM.ToDatetime != value) this.EntityPM.ToDatetime = value; }

  

    public get Status() { return this.EntityPM.Status; }
    public set Status(value: string) { if (this.EntityPM.Status != value) this.EntityPM.Status = value; }

    public get TotalFailed() { return this.EntityPM.TotalFailed; }
    public set TotalFailed(value: number) { if (this.EntityPM.TotalFailed != value) this.EntityPM.TotalFailed = value; }


    public get TotalShipment() { return this.EntityPM.TotalShipment; }
    public set TotalShipment(value: number) { if (this.EntityPM.TotalShipment != value) this.EntityPM.TotalShipment = value; }


    public get Totalsucceeded() { return this.EntityPM.Totalsucceeded; }
    public set Totalsucceeded(value: number) { if (this.EntityPM.Totalsucceeded != value) this.EntityPM.Totalsucceeded = value; }




}
