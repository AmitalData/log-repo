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
    public EntityPM: ContainerSettingPM = new ContainerSettingPM();
    public DataContext: ContainerSettingsComponent = this;
    private IsNew: boolean = false;
    public Loaded: boolean = false;

    constructor() {
        super();
        this.ContainerSettingPMService = new ContainerSettingPMService();
        this.ContainerSettingExtendedService = new ContainerSettingExtendedService();
    }

    ngOnInit() {
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
            if (!response || response.HasError) return;
            if (!response.Result) this.IsNew = true;
            else this.EntityPM = response.Result;
            this.SetDefaultATADateItem();
            this.CurrentSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }

    private SetDefaultATADateItem() {
        if (!AppTool.IsNullOrEmpty(this.DataContext.ShipmentATADateIndicator)) this.SelectedShipmentATADateItem = this.ShipmentATADateComboList.filter(d => d.Code == this.DataContext.ShipmentATADateIndicator)[0];
        else this.SelectedShipmentATADateItem = this.ShipmentATADateComboList.filter(d => d.Code == "Vessel")[0];
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SaveTenant();
        }
    }

    SaveTenant() {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.CurrentSession.StartBusyIndicatorSaving;
        if (this.IsNew) this.InsertEntity();
        else this.UpdateEntity()
    }

    private InsertEntity() {
        this.ContainerSettingPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.HandleResponse(myResponse);
        });
    }

    private UpdateEntity() {
        this.ContainerSettingPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.HandleResponse(myResponse);
        });
    }

    private HandleResponse(myResponse: ServiceResponse) {
        if (!myResponse) return;
        if (!myResponse.HasError) this.CurrentSession.CloseCurrentWindowEmit("ok");
        else {
            this.ValidationErrorsList = myResponse.ErrorsArray;
            this.CurrentSession.StopBusyIndicator();
        }
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
}
