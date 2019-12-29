import { Component, EventEmitter, Output, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { OccasionInviteePM } from '../../../../CRM/EntityPMs/OccasionInviteePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CRMDomainService, OccasionContactSearchresult } from '../../../../CRM/Services/CRMDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ProductTypeListService } from '../../../../Common/Services/StandardLists/ProductTypeListService';
import { ProductTypeList } from '../../../../Common/EntityLists/ProductTypeList';
import { AdditionalServiceListService } from '../../../../Common/Services/StandardLists/AdditionalServiceListService';
import { AdditionalServiceList } from '../../../../Common/EntityLists/AdditionalServiceList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'AddEditOccasionContactComponent',
    moduleId: module.id,
    templateUrl: './AddEditOccasionContactComponent.html',
})

export class AddEditOccasionContactComponent extends BaseComponent implements OnDestroy {
    public EntityPM: OccasionPM;    
    public DataContext: AddEditOccasionContactComponent = this;
    public Items: any[] = [];
    private CurrentSession = SessionLocator.SelectedSession;    
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Occasion";
    private crmService: CRMDomainService;
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() SearchFieldChangeEvent = new EventEmitter();
    private selectedItems: ObservableCollection;
    private selectedItemsCount: number = 0;
    private timerToken: any;
    private IsSavedAll: boolean = false;
    private itemsCount;
    constructor() {
        super();
        this.crmService = new CRMDomainService();
        this.selectedItems = new ObservableCollection([]);

        this.RunComponentTimer();
        this.Listen();
    }

    private ListenEvent: any = null;
    Listen() {
        this.ListenEvent = this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {            
            if (res.Name == "AddAll") {
                this.IsAllChecked = true;
            }

            else if (res.Name == "RemoveAll") {
                this.IsAllChecked = false;
            }
            this.OnLinesSelected();
        });
    }

    private RunComponentTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null
    }

    OnSetRemoved(event) {
        if (!AppTool.IsNullOrEmpty(event)) {
            this.RemovedItems = event;
        }
    }

    SetWindowArgs(entityPM: OccasionPM) {
        this.EntityPM = entityPM;

        this.BuildProductTypesFilters();
        this.BuildAdditionalServicesFilters();
        this.BuildColumns();
    }
    
    public ProductTypeComboList: ProductTypeItem[];
    public SelectedProductTypeFilter: any = "";
    private BuildProductTypesFilters() {
        this.ProductTypeComboList = [];

        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            var list: ProductTypeList[] = response.Result;

            list.filter(d => !d.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach((item) => {
                this.ProductTypeComboList.push(new ProductTypeItem(item));
            });
        });
    }

    public AdditionalServiceComboList: AdditionalServiceItem[];
    public SelectedAdditionalServiceFilter: any = "";
    public SelectedAdditionalServiceFilterIds: any = "";
    private BuildAdditionalServicesFilters() {
        this.AdditionalServiceComboList = [];

        var additionalServiceListService: AdditionalServiceListService = new AdditionalServiceListService();
        additionalServiceListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            var list: AdditionalServiceList[] = response.Result;

            list.filter(d => !d.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach((item) => {
                this.AdditionalServiceComboList.push(new AdditionalServiceItem(item));
            });
        });
    }

    private customerSizeId: string;
    get CustomerSizeId() { return this.customerSizeId; }
    set CustomerSizeId(value: string) {
        if (this.customerSizeId != value) {
            this.customerSizeId = value;
        }
    }

    private regionId: string;
    get RegionId() { return this.regionId; }
    set RegionId(value: string) {
        if (this.regionId != value) {
            this.regionId = value;
        }
    }

    private industryId: string;
    get IndustryId() { return this.industryId; }
    set IndustryId(value: string) {
        if (this.industryId != value) {
            this.industryId = value;
        }
    }

    private occasionId: string;
    get OccasionId() { return this.occasionId; }
    set OccasionId(value: string) {
        if (this.occasionId != value) {
            this.occasionId = value;
        }
    }

    get IsAllChecked() { return this.EntityPM.IsAllAdded; }
    set IsAllChecked(value: boolean) {
        if (this.EntityPM.IsAllAdded != value) {
            this.EntityPM.IsAllAdded = value;

        }
    }


    private removedItems: any[] = [];
    get RemovedItems() { return this.removedItems; }
    set RemovedItems(value: any[]) {
        if (this.removedItems != value) {
            this.removedItems = value;

        }
    }
    
    onCheckBoxChecked($event) {
        if ($event.IsChecked) {
            if (!this.selectedItems.Collection.includes($event)) {
                this.selectedItems.Insert($event);               
                this.selectedItemsCount += 1; 
            }            
        }

        else {
            var removedIndex = null;
            for (var i = 0; i < this.selectedItems.Collection.length; i++) {
                if ($event.rowIndex == this.selectedItems.Collection[i].rowIndex) {
                    removedIndex = i;
                    break;
                }
            }
            
            if (removedIndex != null) {
                this.selectedItems.RemoveFromIndex(removedIndex);
            }
            
            this.selectedItemsCount -= 1;
        }

        this.OnLinesSelected();
    }
    
    public OkButtonIsEnabled: boolean = false;    
    public OnLinesSelected() {
        this.OkButtonIsEnabled = (this.selectedItemsCount > 0 || this.IsAllChecked) ? true : false;
    }

    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    Columns: any;
    BuildColumns() {
        this.Columns = [];
        this.Columns.push({
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: 'Checked',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            IsCheckBox: true,
            ColumnHeaderTemplateName: 'CheckAllInviteeCheckBoxComponent',
            ColumnHeaderTemplateUrl: './CRMModules/CRMOccasion/Components/AddEdit/CheckAllInviteeCheckBoxComponent',
        });

        this.Columns.push({
            FieldName: "Email",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Email',
            Styles: { width: '150px' },
        });

        this.Columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '150px' },
        });

        this.Columns.push({
            FieldName: "Company",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Company',
            Styles: { width: '200px' },
        });

        this.Columns.push({
            FieldName: "Region",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Region',
            Styles: { width: '120px' },
        });

        this.Columns.push({
            FieldName: "Industry",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Industry',
            Styles: { width: '120px' },
        });

        this.Columns.push({
            FieldName: "Product",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Product',
            Styles: { width: '200px' },
        });

        this.Columns.push({
            FieldName: "CustomerSize",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Customer Size',
            Styles: { width: '120px' },
        });
    }
    
    private filters: ApiQueryFilters;
    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = 100;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = SessionLocator.Tenant;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.Filter1Name = "SearchText";
            filters.Filter1Value = this.SearchText;
            filters.Filter1Operator = "Contains";
        }

        if (!AppTool.IsNullOrEmpty(this.CustomerSizeId)) {
            filters.Filter2Name = "CustomerSizeId";
            filters.Filter2Value = this.CustomerSizeId;
            filters.Filter2Operator = "Equals";
        }

        if (!AppTool.IsNullOrEmpty(this.RegionId)) {
            filters.Filter3Name = "RegionId";
            filters.Filter3Value = this.RegionId;
            filters.Filter3Operator = "Equals";
        }

        if (!AppTool.IsNullOrEmpty(this.IndustryId)) {
            filters.Filter4Name = "IndustryId";
            filters.Filter4Value = this.IndustryId;
            filters.Filter4Operator = "Equals";
        }

        if (!AppTool.IsNullOrEmpty(this.OccasionId)) {
            filters.Filter5Name = "OccasionId";
            filters.Filter5Value = this.OccasionId;
            filters.Filter5Operator = "Equals";
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedProductTypeFilter)) {
            filters.Filter6Name = "Products";
            filters.Filter6Value = this.SelectedProductTypeFilter;
            filters.Filter6Operator = "Equals";
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedAdditionalServiceFilterIds)) {
            filters.Filter7Name = "AdditionalServices";
            filters.Filter7Value = this.SelectedAdditionalServiceFilterIds;
            filters.Filter7Operator = "Equals";
        }


        if (!this.IsSavedAll) {
            this.filters = filters;
            return new Promise((resolve, reject) => { resolve(this.crmService.GetOccasionContactsByFilters(filters)) });
        }

        else {
            var ids = this.EntityPM.RemovedOccasionInvitees.map(function (item) {
                return item['ContactId'];
            });
            filters.Filter8Name = "DeletedContactIds";
            filters.Filter8Value = ids.join(",");
            filters.Filter9Name = "OccasionIds";
            filters.Filter9Value = this.EntityPM.Id;
            this.filters = filters;

            return new Promise((resolve, reject) => {
                resolve(this.crmService.GetOccasionContactsByFiltersAndUpdate(filters))
            });
           
 
        }
    }
    private searchText: string;
    get SearchText() { return this.searchText; }
    set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    OnSearchTextChangeEvent(text: string) {        
        this.SearchText = text;
        this.BrowseClicked();
    }
    
    BrowseClicked() {
        this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Descending",
            getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };

        this.onQueryChangeEvent.emit({ Filters: this.filters, Reload: true });        
    }

    onCountReady(event) {
        this.itemsCount = event;
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var test = this.RemovedItems;
        if (this.selectedItemsCount <= 0 && !this.IsAllChecked) {
            errors.push("You must select 1 line at least");
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedProductTypeFilter)) {
            if (this.ProductTypeComboList.filter(d => d.Checked).length == 0) {
                errors.push("Please select product type");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (!this.IsAllChecked) {
                this.selectedItems.Collection.forEach(item => {
                    var existContact: OccasionInviteePM = this.EntityPM.OccasionInvitees.filter(d => d.ContactId == item.rowData.ContactId)[0];

                    if (existContact == null) {
                        var invitee = new OccasionInviteePM(this.EntityPM);
                        invitee.Tenant = SessionLocator.Tenant;
                        invitee.AddedByUserId = SessionInfo.LoggedUserId;
                        invitee.AddedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                        invitee.AddedDate = DateTool.GetCurrentDateTimeAsUtc();
                        invitee.ContactId = item.rowData.ContactId;
                        invitee.ContactName = item.rowData.Name;
                        invitee.OccasionId = this.EntityPM.Id;
                        invitee.UpdatedByUserId = SessionInfo.LoggedUserId;
                        invitee.UpdatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                        invitee.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                        invitee.ContactEmail = item.rowData.Email;
                        invitee.ContactMobile = item.rowData.ContactMobile;
                        invitee.ContactPhone = item.rowData.ContactPhone;
                        invitee.ContactPosition = item.rowData.ContactPosition;
                        invitee.CustomerName = item.rowData.Company;
                        invitee.ContactTel = item.rowData.ContactTel;
                        this.EntityPM.AddOccasionInvitee(invitee);
                    }
                });

            }
            else {
                if ((this.itemsCount - this.RemovedItems.length) > 1000) {
                    var msg = new MessageWindow();
                    msg.Show("Selected contacts must be less than 1000");
                    return;
                }
                this.IsSavedAll = true;
                this.RemovedItems.forEach(item => {
                    var existContact: OccasionInviteePM = this.EntityPM.OccasionInvitees.filter(d => d.ContactId == item.ContactId)[0];

                    if (existContact == null) {
                        var invitee = new OccasionInviteePM(this.EntityPM);
                        invitee.Tenant = SessionLocator.Tenant;
                        invitee.AddedByUserId = SessionInfo.LoggedUserId;
                        invitee.AddedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                        invitee.AddedDate = DateTool.GetCurrentDateTimeAsUtc();
                        invitee.ContactId = item.ContactId;
                        invitee.ContactName = item.Name;
                        invitee.OccasionId = this.EntityPM.Id;
                        invitee.UpdatedByUserId = SessionInfo.LoggedUserId;
                        invitee.UpdatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                        invitee.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                        invitee.ContactEmail = item.Email;
                        invitee.ContactMobile = item.ContactMobile;
                        invitee.ContactPhone = item.ContactPhone;
                        invitee.ContactPosition = item.ContactPosition;
                        invitee.CustomerName = item.Company;
                        invitee.ContactTel = item.ContactTel;
                        this.EntityPM.RemovedOccasionInvitees.push(invitee);
                    }
                });
                this.DataSource = {
                    pageSize: 20,
                    rowCount: null,
                    sortingDir: "Descending",
                    getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
                        var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                        return tempo;
                    },
                };
                this.onQueryChangeEvent.emit({ Filters: this.filters, Reload: false });        
                this.IsSavedAll = false;
            }
            this.CurrentSession.CloseCurrentWindow();


        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class ProductTypeItem {
    public entityList: ProductTypeList;
    constructor(entityList: ProductTypeList) {
        this.entityList = entityList;
    }

    get Code() { return this.entityList.Code; }
    get Name() { return this.entityList.Name; }

    private checked: boolean;
    public get Checked() { return this.checked; }
    public set Checked(value: boolean) { this.checked = value; }
}

export class AdditionalServiceItem {
    public entityList: AdditionalServiceList;
    constructor(entityList: AdditionalServiceList) {
        this.entityList = entityList;
    }

    get Id() { return this.entityList.Id; }
    get Name() { return this.entityList.Name; }

    private checked: boolean;
    public get Checked() { return this.checked; }
    public set Checked(value: boolean) { this.checked = value; }
}
