import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { OccasionInviteePM } from '../../../../CRM/EntityPMs/OccasionInviteePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CRMDomainService, OccasionContactArgs, OccasionContactSearchresult } from '../../../../CRM/Services/CRMDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ProductTypeListService } from '../../../../Common/Services/StandardLists/ProductTypeListService';
import { ProductTypeList } from '../../../../Common/EntityLists/ProductTypeList';

@Component({
    selector: 'AddEditOccasionContactComponent',
    moduleId: module.id,
    templateUrl: './AddEditOccasionContactComponent.html',
})

export class AddEditOccasionContactComponent extends BaseComponent {
    public EntityPM: OccasionPM;    
    public DataContext: AddEditOccasionContactComponent = this;
    public ItemsSource: OccasionContactItem[];
    private CurrentSession = SessionLocator.SelectedSession;
    public SelectedItem: OccasionContactItem = null;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Occasion";
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: OccasionPM) {
        this.EntityPM = entityPM;

        this.BuildProductTypesFilters();
    }

    public ProductTypeComboList: ProductTypeItem[];
    public SelectedProdustTypeFilter: any = "";
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
    
    private isAllChecked: boolean = false;
    get IsAllChecked() { return this.isAllChecked; }
    set IsAllChecked(value: boolean) {
        if (this.isAllChecked != value) {
            this.isAllChecked = value;

            this.ItemsSource.forEach(item => {
                item.IsChecked = value;
            });

            this.OnLinesSelected();
        }
    }

    public OnLinesSelected() {

    }

    private BuildItemsSource() {
        this.ItemsSource = [];

        var myList: OccasionContactSearchresult[] = [];
        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            myList = this.loadedContacts.filter(f => f.Email != null && f.Email.toLowerCase().startsWith(this.SearchText.toLowerCase())
                || f.Name != null && f.Name.toLowerCase().startsWith(this.SearchText.toLowerCase())
                || f.Company != null && f.Company.toLowerCase().startsWith(this.SearchText.toLowerCase()));
        }

        else {
            myList = this.loadedContacts;
        }

        myList.forEach(item => {
            var myResultItem = new OccasionContactItem(this);
            myResultItem.ContactId = item.ContactId;
            myResultItem.Email = item.Email;
            myResultItem.Name = item.Name;
            myResultItem.Company = item.Company;
            myResultItem.Region = item.Region;
            myResultItem.Industry = item.Industry;
            myResultItem.Product = item.Product;
            myResultItem.CustomerSize = item.CustomerSize;
            this.ItemsSource.push(myResultItem);
        });
    }

    private SearchText: string = null;
    SearchMethod(text: string) {
        if (AppTool.IsNullOrEmpty(text)) {
            this.SearchText = null;
        }

        else {
            this.SearchText = text;
        }

        this.BuildItemsSource();
    }

    private loadedContacts: OccasionContactSearchresult[];
    BrowseClicked() {
        this.CurrentSession.StartBusyIndicatorLoading();
        
        var args: OccasionContactArgs = new OccasionContactArgs();
        args.CustomerSizeId = this.CustomerSizeId;
        args.RegionId = this.RegionId;
        args.IndustryId = this.IndustryId;
        args.OccasionId = this.OccasionId;
        args.ProductTypes = this.SelectedProdustTypeFilter;
        args.AdditionalServices = "";

        var service = new CRMDomainService();
        service.BrowseOccasionContacts(args).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.loadedContacts = myResult.Result;
                this.BuildItemsSource();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    OkButtonClicked() {
        var errors: string[] = [];

        var selectedCount: number = this.ItemsSource.filter(f => f.IsChecked == true).length;
        if (selectedCount == 0) {
            errors.push("You must select 1 line at least");
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedProdustTypeFilter)) {
            if (this.ProductTypeComboList.filter(d => d.Checked).length == 0) {
                errors.push("Please select product type");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.ItemsSource.filter(f => f.IsChecked).forEach(item => {
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
                    this.EntityPM.AddOccasionInvitee(invitee);
                }
            });

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class OccasionContactItem {

    constructor(private fatherComponent: AddEditOccasionContactComponent) {
        
    }

    public ContactId: string;   
    public Email: string;
    public Name: string;
    public Company: string;
    public Region: string;    
    public Industry: string;
    public Product: string;
    public CustomerSize: string;

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            this.fatherComponent.OnLinesSelected();
        }
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
