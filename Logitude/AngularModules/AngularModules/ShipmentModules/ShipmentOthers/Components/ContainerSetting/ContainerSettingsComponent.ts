import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ContainerSettingPMService } from 'Infrastructure/Services/StandardPMs/ContainerSettingPMService';
import { ContainerSettingPM } from 'Infrastructure/EntityPMs/ContainerSettingPM';
import { ContainerSettingExtendedService } from 'Infrastructure/Services/ExtendedPMs/ContainerSettingExtendedService';
import { ShippingLineExtendedPMService } from 'Common/Services/ExtendedPMs/ShippingLineExtendedPMService';
import { ShippingLinePM } from 'Common/EntityPMs/ShippingLinePM';
import { ServiceLocator } from 'Infrastructure/Locators/ServiceLocator';

@Component({
    templateUrl: './ContainerSettingsComponent.html',
    styleUrls: ['./ContainerSettingsComponent.scss']
})

export class ContainerSettingsComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "ContainerSetting";
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];
    public reloadingTranslation: boolean;
    public ClosingContainerToolTipMessage: string = "How many days after the Actual Empty Return Date to wait before automatically closing the container.";
    public ShipmentATADateComboList: Array<CodeNameClass>;
    public SelectedTabCode: string = "G";
    public ContainerSettingPMService: ContainerSettingPMService;
    public ContainerSettingExtendedService: ContainerSettingExtendedService;
    private ShippingLineExtendedPMService: ShippingLineExtendedPMService;
    public EntityPM: ContainerSettingPM = new ContainerSettingPM();
    public DataContext: ContainerSettingsComponent = this;
    private IsNew: boolean = false;
    public Loaded: boolean = false;
    private TenantZeroShippingLines: ShippingLinePM[] = [];
    private UserTenantShippingLines: ShippingLinePM[] = [];
    public ShippingLines: ShippingLineItem[];
    private ShowDefaults: boolean;
    public IsContainerTrackingPrepaid: boolean;
    public CurrentTenant;
    constructor() {
        super();
        this.ContainerSettingPMService = new ContainerSettingPMService();
        this.ContainerSettingExtendedService = new ContainerSettingExtendedService();
        this.ShippingLineExtendedPMService = new ShippingLineExtendedPMService();
        this.IsContainerTrackingPrepaid = SessionLocator.TenantManagementJS.IsContainerTrackingPrepaid
    }

    ngOnInit() {
        this.CurrentTenant = SessionLocator.TenantPM;
        this.FillShipmentATADateComboList();
        this.GetSettings();
    }

    FillShipmentATADateComboList() {
        this.ShipmentATADateComboList = [];
        this.ShipmentATADateComboList.push(new CodeNameClass("Vessel", "Vessel Arrival"));
        this.ShipmentATADateComboList.push(new CodeNameClass("Container", "First Container Discharged"));
    }

    private GetSettings() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ContainerSettingExtendedService.GetSingleSetting().subscribe((response: ServiceResponse) => {
            if (!response || response.HasError) return this.StopIndicator();
            if (!response.Result) this.IsNew = true;
            else this.EntityPM = response.Result;
            this.ShowDefaults = SessionLocator.Tenant != 0 && (this.IsNew || !this.EntityPM?.AddedManually);
            this.SetDefaultGeneralItem();
            this.LoadTenantZeroShippingLines();
            this.SetDefaultData();
        });
    }

    SetDefaultData() {
        if (!this.ShowDefaults) return;
        this.IsExport = true;
        this.IsImport = true;
        this.IsDrop = true;
        this.IsDomestic = true;
    }

    private SetDefaultGeneralItem() {
        if (!AppTool.IsNullOrEmpty(this.DataContext.ShipmentATADateIndicator)) this.SelectedShipmentATADateItem = this.ShipmentATADateComboList.filter(d => d.Code == this.DataContext.ShipmentATADateIndicator)[0];
        else this.SelectedShipmentATADateItem = this.ShipmentATADateComboList.filter(d => d.Code == "Vessel")[0];
        if (!this.ShowDefaults) return;
        this.EmptyReturnClosingDays = this.CurrentTenant.EmptyReturnClosingDays != null ? this.CurrentTenant.EmptyReturnClosingDays : 5;
        this.ShipmentATAClosingDays = this.CurrentTenant.ShipmentATAClosingDays != null ? this.CurrentTenant.ShipmentATAClosingDays : 90;
    }


    private LoadTenantZeroShippingLines() {
        this.TenantZeroShippingLines = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ShippingLineExtendedPMService.GetShippingLinesForTenant(0).subscribe((response: ServiceResponse) => {
            if (response.HasError) return this.StopIndicator();
            this.TenantZeroShippingLines = response.Result;
            if (SessionLocator.Tenant == 0) {
                this.BuildShippingLines();
                return this.StopIndicator();
            }
            this.LoadCurrenctTenantShippingLines();
        });
    }

    private LoadCurrenctTenantShippingLines() {
        this.UserTenantShippingLines = [];
        this.ShippingLineExtendedPMService.GetShippingLinesForTenant(SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.UserTenantShippingLines = response.Result;
                this.BuildShippingLines();
            }
            this.StopIndicator();
        });
    }

    BuildShippingLines() {
        this.ShippingLines = [];
        let itemSource: ShippingLinePM[] = SessionLocator.Tenant == 0 ? this.TenantZeroShippingLines : this.UserTenantShippingLines;
        itemSource = this.SortItemSource(itemSource);
        itemSource.forEach((item) => {
            var shippingLineItem = this.BuildShippingLineItem(item);
            if (shippingLineItem) this.ShippingLines.push(shippingLineItem);
        });
    }

    private BuildShippingLineItem(shippingLine: ShippingLinePM): ShippingLineItem {
        var tenantZeroItem = this.TenantZeroShippingLines.filter(t => t.SCACCode == shippingLine.SCACCode)[0];
        if (tenantZeroItem != null) {
            if (this.ShowDefaults) this.SetShippingLineDefaultVaues(shippingLine, tenantZeroItem);
            return new ShippingLineItem(tenantZeroItem, shippingLine);
        }
        else if (shippingLine.AddedManually) return new ShippingLineItem(null, shippingLine);
        return null;
    }

    private SetShippingLineDefaultVaues(shippingLine: ShippingLinePM, tenantZeroItem: ShippingLinePM) {
        shippingLine.IsAutomaticRequestsSent = tenantZeroItem.IsAutomaticRequestsSent;
    }

    private SortItemSource(items: ShippingLinePM[]) {
        items.sort((a, b) => {
            if (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) return -1;
            if (a.EnglishName.toLowerCase() > b.EnglishName.toLowerCase()) return 1;
            else return 0;

        });
        return items;
    }

    private StopIndicator() {
        this.CurrentSession.StopBusyIndicator();
        this.Loaded = true;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        this.CustomValidation();
        if (this.ValidationErrorsList.length == 0) this.SubmitSave();
    }

    CustomValidation() {
        if (this.IsContainerTrackingPrepaid && AppTool.IsNullOrEmpty(this.ActivationDate))
            this.ValidationErrorsList.push("Activation Date is Required");
    }

    SubmitSave() {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.AddedManually = true;
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.IsNew) this.InsertEntity();
        else this.UpdateEntity()
    }

    private InsertEntity() {
        this.ContainerSettingPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.HandleSaveResponse(myResponse);
        });
    }

    private UpdateEntity() {
        this.ContainerSettingPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.HandleSaveResponse(myResponse);
        });
    }

    private HandleSaveResponse(myResponse: ServiceResponse) {
        if (!myResponse) return;
        if (!myResponse.HasError) return this.SaveShippingLines();
        this.ValidationErrorsList = myResponse.ErrorsArray;
        this.CurrentSession.StopBusyIndicator();
    }

    SaveShippingLines() {
        var changedShippingLines: ShippingLinePM[] = [];
        this.BuildChangedShippingLines(changedShippingLines);

        if (changedShippingLines.length == 0) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
            return;
        }

        this.ShippingLineExtendedPMService.Update(changedShippingLines).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError) this.ValidationErrorsList = response.ErrorsArray;
            else this.CurrentSession.CloseCurrentWindow();
        });
    }

    private BuildChangedShippingLines(savedList: ShippingLinePM[]) {
        this.ShippingLines.forEach((item) => {
            if (SessionLocator.Tenant == 0) {
                if (item.EntityZero.IsDirty) {
                    savedList.push(item.EntityZero);
                }
            }
            else {
                if (item.Entity.IsDirty) {
                    savedList.push(item.Entity);
                }
            }
        });
    }
    AutomaticRequestsTabClicked(){
        this.SelectedTabCode = 'R';
        ServiceLocator.SendTotangoUserActivity("Container Settings", "Automatic Request Tab View");
    }

    private selectedShipmentATADateItem: CodeNameClass;
    get SelectedShipmentATADateItem() { return this.selectedShipmentATADateItem; }
    set SelectedShipmentATADateItem(value: CodeNameClass) {
        if (this.selectedShipmentATADateItem != value) {
            this.selectedShipmentATADateItem = value;
            this.DataContext.ShipmentATADateIndicator = value?.Code;
        }
    }

    get EmptyReturnClosingDays() { return this.EntityPM.EmptyReturnClosingDays; }
    set EmptyReturnClosingDays(value: number) {
        if (this.EntityPM.EmptyReturnClosingDays != value) {
            this.EntityPM.EmptyReturnClosingDays = value;
        }
    }

    get ShipmentATAClosingDays() { return this.EntityPM.ShipmentATAClosingDays; }
    set ShipmentATAClosingDays(value: number) {
        if (this.EntityPM.ShipmentATAClosingDays != value) {
            this.EntityPM.ShipmentATAClosingDays = value;
        }
    }

    get ShipmentATADateIndicator() { return this.EntityPM.ShipmentATADateIndicator; }
    set ShipmentATADateIndicator(value: string) {
        if (this.EntityPM.ShipmentATADateIndicator != value) {
            this.EntityPM.ShipmentATADateIndicator = value;
        }
    }

    get IsImport() { return this.EntityPM.IsImport; }
    set IsImport(value: boolean) {
        if (this.EntityPM.IsImport != value) {
            this.EntityPM.IsImport = value;
        }
    }

    get IsExport() { return this.EntityPM.IsExport; }
    set IsExport(value: boolean) {
        if (this.EntityPM.IsExport != value) {
            this.EntityPM.IsExport = value;
        }
    }

    get IsDomestic() { return this.EntityPM.IsDomestic; }
    set IsDomestic(value: boolean) {
        if (this.EntityPM.IsDomestic != value) {
            this.EntityPM.IsDomestic = value;
        }
    }

    get IsDrop() { return this.EntityPM.IsDrop; }
    set IsDrop(value: boolean) {
        if (this.EntityPM.IsDrop != value) {
            this.EntityPM.IsDrop = value;
        }
    }

    get ActivationDate() { return this.EntityPM.ActivationDate; }
    set ActivationDate(value: Date) {
        if (this.EntityPM.ActivationDate != value) {
            this.EntityPM.ActivationDate = value;
        }
    }
}

export class ShippingLineItem extends BaseComponent {
    public EntityZero: ShippingLinePM;
    public Entity: ShippingLinePM;
    public DataContext: ShippingLineItem = this;
    public Tenant: number = 0;

    constructor(zeroEntity: ShippingLinePM, currentEntity: ShippingLinePM) {
        super();
        this.EntityZero = zeroEntity;
        this.Entity = currentEntity;
        this.Tenant = SessionLocator.Tenant;
    }

    public get Code(): string {
        if (SessionLocator.Tenant == 0) return this.EntityZero.Code;
        return this.Entity.Code;
    }

    public get SCACCode(): string {
        if (SessionLocator.Tenant == 0) return this.EntityZero.SCACCode;
        return this.Entity.SCACCode;
    }

    public get Name(): string {
        if (SessionLocator.Tenant == 0) return this.EntityZero.EnglishName;
        return this.Entity.EnglishName;
    }

    public set IsSupportsContainerTracking(value: boolean) {
        if (this.Tenant == 0 && this.Entity) {
            this.Entity.IsSupportsContainerTracking = value;
            if (!value) this.IsAutomaticRequestsSent = false;
        }
    }
    public get IsSupportsContainerTracking() {
        if (this.Tenant == 0) return this.Entity?.IsSupportsContainerTracking ?? false;
        else return this.EntityZero?.IsSupportsContainerTracking ?? false;
    }

    public set IsAutomaticRequestsSent(value: boolean) { if (this.Entity) this.Entity.IsAutomaticRequestsSent = value; }
    public get IsAutomaticRequestsSent() {
        if (this.Entity) return this.Entity.IsAutomaticRequestsSent;
        else return false;
    }

    public get IsAutomaticRequestsSentEnabled() {
        return this.EntityZero?.IsSupportsContainerTracking;
    }
}
