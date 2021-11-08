import { Component, OnInit} from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { GlobalDomainService, OceanInsightGlobalSetting } from '../../../../Common/Services/GlobalDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShippingLinePM } from '../../../../Common/EntityPMs/ShippingLinePM';
import { ShippingLineExtendedPMService } from '../../../../Common/Services/ExtendedPMs/ShippingLineExtendedPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    templateUrl: './OceanInsightsSettingsComponent.html',
})

export class OceanInsightsSettingsComponent extends BaseComponent implements OnInit {
    public EntityPM: OceanInsightGlobalSetting;
    public DataContext: OceanInsightsSettingsComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private globalDomainService: GlobalDomainService;
    public IsVisible = false;
    public SelectedTabCode: string = SessionLocator.Tenant == 0 ? "G" : "P";
    public ShippingLinesLists: ShippingLineItem[];
    private shippingLineExtendedPMService: ShippingLineExtendedPMService;
    public IsVisibleForTenantZero: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        super();

        if (SessionLocator.Tenant == 0) {
            this.IsVisibleForTenantZero = true;
        }
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("ShippingLine").subscribe((res: any) => {
            this.globalDomainService = new GlobalDomainService();
            this.shippingLineExtendedPMService = new ShippingLineExtendedPMService();
            this.GetOceanInsightGlobalSetting();
        });
    }

    GetOceanInsightGlobalSetting() {
        this.globalDomainService.GetOceanInsightGlobalSetting().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.EntityPM = response.Result;
                this.IsVisible = true;
                this.LoadTenantZeroData();
            }
        });
    }

    private myTenantZeroList: ShippingLinePM[];
    private myTenantList: ShippingLinePM[];

    private LoadTenantZeroData() {
        this.myTenantZeroList = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.shippingLineExtendedPMService.GetShippingLinesForTenant(0).subscribe((res: ServiceResponse) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantZeroList = pmResponse.Result;

                if (SessionLocator.Tenant == 0) {
                    this.BuildData();
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.LoadCurrenctTenantData();
                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private LoadCurrenctTenantData() {
        this.myTenantList = [];
        this.shippingLineExtendedPMService.GetShippingLinesForTenant(SessionLocator.Tenant).subscribe((res: ServiceResponse) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantList = pmResponse.Result;
                this.BuildData();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    OnSearchTextChangeEvent(searchText) {
        if (!searchText) {
            searchText = "";
        }

        this.mySearchText = searchText;
        this.BuildData();
    }

    mySearchText: string;
    private BuildData() {
        this.ShippingLinesLists = [];
        var mySourceList: ShippingLinePM[] = [];
        var myList: ShippingLinePM[] = [];

        if (SessionLocator.Tenant == 0) {
            mySourceList = this.myTenantZeroList;
        }

        else {
            mySourceList = this.myTenantList;
        }

        if (!this.mySearchText) {
            myList = mySourceList;
        }
        else {

            myList = mySourceList.filter(d => (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }

        myList = this.SortItemSource(myList);
        myList.forEach((item) => {
            var tenantZeroItem = this.myTenantZeroList.filter(t => t.Code == item.Code)[0];
            if (tenantZeroItem != null) {
                this.ShippingLinesLists.push(new ShippingLineItem(tenantZeroItem, item));
            }
            else {
                if (item.AddedManually) {
                    this.ShippingLinesLists.push(new ShippingLineItem(null, item));
                }
            }
        });
    }
    private SortItemSource(items: ShippingLinePM[]) {
        items.sort((a, b) => {
            if (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) {
                return -1;
            }
            else if (a.EnglishName.toLowerCase() > b.EnglishName.toLowerCase()) {
                return 1;
            }

            else {
                return 0;
            }
        });

        return items;
    }
    get OITenantNumber() { return this.EntityPM.OITenantNumber; }
    set OITenantNumber(value: number) {
        if (this.EntityPM.OITenantNumber != value) {
            this.EntityPM.OITenantNumber = value;
        }
    }

    get AmitalCloudLogitudeTenantPrimaryKey() { return this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey; }
    set AmitalCloudLogitudeTenantPrimaryKey(value: string) {
        if (this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey != value) {
            this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey = value;
        }
    }

    get AmitalCloudEnvironmentURL() { return this.EntityPM.AmitalCloudEnvironmentURL; }
    set AmitalCloudEnvironmentURL(value: string) {
        if (this.EntityPM.AmitalCloudEnvironmentURL != value) {
            this.EntityPM.AmitalCloudEnvironmentURL = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        if (AppTool.IsNullOrZero(this.OITenantNumber)) {
            errors.push("Ocean Insight Tenant is required");
        }
        if (AppTool.IsNullOrEmpty(this.AmitalCloudEnvironmentURL)) {
            errors.push("Amital Cloud Environment URL is required");
        }
        if (AppTool.IsNullOrEmpty(this.AmitalCloudLogitudeTenantPrimaryKey)) {
            errors.push("Amital Primary Key is required");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            var savedList: ShippingLinePM[] = [];
            this.ShippingLinesLists.forEach((item) => {
                if (SessionLocator.Tenant == 0) {
                    if (item.entityPM_TenantZero.IsDirty) {
                        savedList.push(item.entityPM_TenantZero);
                    }
                }
                else {
                    if (item.entityPM.IsDirty) {
                        savedList.push(item.entityPM);
                    }
                }
            });

            this.CurrentSession.StartBusyIndicatorSaving();
            this.globalDomainService.UpdateOceanInsightGlobalSetting(this.EntityPM.OITenantNumber, this.EntityPM.AmitalCloudEnvironmentURL, this.EntityPM.AmitalCloudLogitudeTenantPrimaryKey).subscribe((response: ServiceResponse) => {
                if (response.HasError) {
                    this.ValidationErrorsList = response.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    if (savedList.length > 0) {
                        this.SaveShippingLines(savedList);
                    }

                    else {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    }
                }
            });
        }
    }
    private SaveShippingLines(savedList: ShippingLinePM[]) {
        this.shippingLineExtendedPMService.Update(savedList).subscribe((response: ServiceResponse) => {
            if (response.HasError) {
                this.ValidationErrorsList = response.ErrorsArray;                
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
}

export class ShippingLineItem extends BaseComponent {
    public entityPM: ShippingLinePM;
    public entityPM_TenantZero: ShippingLinePM;
    public DataContext: ShippingLineItem = this;
    constructor(zeroEntity: ShippingLinePM, currentEntity: ShippingLinePM) {
        super();

        this.entityPM_TenantZero = zeroEntity;
        this.entityPM = currentEntity;

        this.SetUIProperties();
    }

    private SetUIProperties() {
        if (SessionLocator.Tenant != 0) {
            //this.UIProperties.SetEnabled("IsSendingByContainer", "ShippingLine", false);
            //this.UIProperties.SetEnabled("IsSendingByBillOfLading", "ShippingLine", false);
        }
    }

    public get Code() {
        if (SessionLocator.Tenant == 0) {
            return this.entityPM_TenantZero.Code;
        }

        else {
            return this.entityPM.Code;
        }
    }

    public get Name() {
        if (SessionLocator.Tenant == 0) {
            return this.entityPM_TenantZero.EnglishName;
        }

        else {
            return this.entityPM.EnglishName;
        }
    }

    public get IsSendingByContainer() {
        if (this.entityPM) {
            return this.entityPM.IsSendingByContainer;
        }

        else return false;
    }
    public set IsSendingByContainer(value: boolean) {
        if (this.entityPM != null) {
            this.entityPM.IsSendingByContainer = value;
        }
    }

    public get IsSendingByBillOfLading() {
        if (this.entityPM) {
            return this.entityPM.IsSendingByBillOfLading;
        }

        else return false;
    }
    public set IsSendingByBillOfLading(value: boolean) {
        if (this.entityPM != null) {
            this.entityPM.IsSendingByBillOfLading = value;
        }
    }

    public get TenantZeroIsSendingByContainer() {
        if (this.entityPM_TenantZero) {
            return this.entityPM_TenantZero.IsSendingByContainer;
        }

        else return false;
    }
    public set TenantZeroIsSendingByContainer(value: boolean) {
        if (this.entityPM_TenantZero != null) {
            this.entityPM_TenantZero.IsSendingByContainer = value;
        }
    }

    public get TenantZeroIsSendingByBillOfLading() {
        if (this.entityPM_TenantZero) {
            return this.entityPM_TenantZero.IsSendingByBillOfLading;
        }

        else return false;
    }
    public set TenantZeroIsSendingByBillOfLading(value: boolean) {
        if (this.entityPM_TenantZero != null) {
            this.entityPM_TenantZero.IsSendingByBillOfLading = value;
        }
    }

    public get IsSendingByContainerEnabled() {
        if (this.entityPM_TenantZero != null) {
            return this.entityPM_TenantZero.IsSendingByContainer;
        }

        else {
            return false;
        }
    }

    public get IsSendingByBillOfLadingEnabled() {
        if (this.entityPM_TenantZero != null) {
            return this.entityPM_TenantZero.IsSendingByBillOfLading;
        }

        else {
            return false;
        }
    }
}
