import { Component, OnInit } from '@angular/core';
import { CreditLimitSettingPM } from '../../../EntityPMs/CreditLimitSettingPM';
import { CreditLimitSettingPMService } from '../../../Services/StandardPMs/CreditLimitSettingPMService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CreditLimitSettingsComponent.html',
})

export class CreditLimitSettingsComponent extends BaseComponent implements OnInit {
    public EntityPM: CreditLimitSettingPM;
    public ObjectTableName: string = "CreditLimitSetting";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public SelectedTabCode: string = "A";
    private myService: CreditLimitSettingPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsPartnersRestrictionsTabVisible: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new CreditLimitSettingPMService();

        if (FeatureLocator.HasFeaturePermession("CreditLimitSetting", "PartnersRestrictions")) {
            this.IsPartnersRestrictionsTabVisible = true;
        }
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.myService.get(SessionLocator.Tenant + "").subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    if (myResponse.Result) {
                        this.EntityPM = myResponse.Result;
                    }

                    else {
                        this.EntityPM = new CreditLimitSettingPM();
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.IsDirty = false;
                    }

                    this.SetUIProperties();
                    this.IsResourcesReady = true;
                }
            });
        });
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled('InvoiceCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('InvoiceCreationWarning', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShipmentCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);

        this.UIProperties.SetEnabled('CustomersShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('AgentsShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShipperConsigneeShipmentBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('CustomsAgentsShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShippingAgentsShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('AirlinesShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShippingLinesShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('TruckersShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('VendorsShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('WarehousesShipmentsBlock', this.ObjectTableName, this.IsCreditLimitEnabled);

        this.UIProperties.SetEnabled('CustomersInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('AgentsInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShipperConsigneeInvoiceBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('CustomsAgentsInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShippingAgentsInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('AirlinesInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShippingLinesInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('TruckersInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('VendorsInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('WarehousesInvoicesBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
    }

    get IsCreditLimitEnabled() { return this.EntityPM.IsCreditLimitEnabled; }
    set IsCreditLimitEnabled(value: boolean) {
        if (this.EntityPM.IsCreditLimitEnabled != value) {
            this.EntityPM.IsCreditLimitEnabled = value;
            this.SetUIProperties();
        }
    }

    get InvoiceCreationBlock() { return this.EntityPM.InvoiceCreationBlock; }
    set InvoiceCreationBlock(value: boolean) {
        if (this.EntityPM.InvoiceCreationBlock != value) {
            this.EntityPM.InvoiceCreationBlock = value;
        }
    }

    get InvoiceCreationWarning() { return this.EntityPM.InvoiceCreationWarning; }
    set InvoiceCreationWarning(value: boolean) {
        if (this.EntityPM.InvoiceCreationWarning != value) {
            this.EntityPM.InvoiceCreationWarning = value;
        }
    }

    get ShipmentCreationBlock() { return this.EntityPM.ShipmentCreationBlock; }
    set ShipmentCreationBlock(value: boolean) {
        if (this.EntityPM.ShipmentCreationBlock != value) {
            this.EntityPM.ShipmentCreationBlock = value;
        }
    }

    get CustomersShipmentsBlock() { return this.EntityPM.CustomersShipmentsBlock; }
    set CustomersShipmentsBlock(value: boolean) {
        if (this.EntityPM.CustomersShipmentsBlock != value) {
            this.EntityPM.CustomersShipmentsBlock = value;
        }
    }

    get CustomersInvoicesBlock() { return this.EntityPM.CustomersInvoicesBlock; }
    set CustomersInvoicesBlock(value: boolean) {
        if (this.EntityPM.CustomersInvoicesBlock != value) {
            this.EntityPM.CustomersInvoicesBlock = value;
        }
    }

    get AgentsShipmentsBlock() { return this.EntityPM.AgentsShipmentsBlock; }
    set AgentsShipmentsBlock(value: boolean) {
        if (this.EntityPM.AgentsShipmentsBlock != value) {
            this.EntityPM.AgentsShipmentsBlock = value;
        }
    }

    get AgentsInvoicesBlock() { return this.EntityPM.AgentsInvoicesBlock; }
    set AgentsInvoicesBlock(value: boolean) {
        if (this.EntityPM.AgentsInvoicesBlock != value) {
            this.EntityPM.AgentsInvoicesBlock = value;
        }
    }

    get ShipperConsigneeShipmentBlock() { return this.EntityPM.ShipperConsigneeShipmentBlock; }
    set ShipperConsigneeShipmentBlock(value: boolean) {
        if (this.EntityPM.ShipperConsigneeShipmentBlock != value) {
            this.EntityPM.ShipperConsigneeShipmentBlock = value;
        }
    }

    get ShipperConsigneeInvoiceBlock() { return this.EntityPM.ShipperConsigneeInvoiceBlock; }
    set ShipperConsigneeInvoiceBlock(value: boolean) {
        if (this.EntityPM.ShipperConsigneeInvoiceBlock != value) {
            this.EntityPM.ShipperConsigneeInvoiceBlock = value;
        }
    }

    get CustomsAgentsShipmentsBlock() { return this.EntityPM.CustomsAgentsShipmentsBlock; }
    set CustomsAgentsShipmentsBlock(value: boolean) {
        if (this.EntityPM.CustomsAgentsShipmentsBlock != value) {
            this.EntityPM.CustomsAgentsShipmentsBlock = value;
        }
    }

    get CustomsAgentsInvoicesBlock() { return this.EntityPM.CustomsAgentsInvoicesBlock; }
    set CustomsAgentsInvoicesBlock(value: boolean) {
        if (this.EntityPM.CustomsAgentsInvoicesBlock != value) {
            this.EntityPM.CustomsAgentsInvoicesBlock = value;
        }
    }

    get ShippingAgentsShipmentsBlock() { return this.EntityPM.ShippingAgentsShipmentsBlock; }
    set ShippingAgentsShipmentsBlock(value: boolean) {
        if (this.EntityPM.ShippingAgentsShipmentsBlock != value) {
            this.EntityPM.ShippingAgentsShipmentsBlock = value;
        }
    }

    get ShippingAgentsInvoicesBlock() { return this.EntityPM.ShippingAgentsInvoicesBlock; }
    set ShippingAgentsInvoicesBlock(value: boolean) {
        if (this.EntityPM.ShippingAgentsInvoicesBlock != value) {
            this.EntityPM.ShippingAgentsInvoicesBlock = value;
        }
    }

    get AirlinesShipmentsBlock() { return this.EntityPM.AirlinesShipmentsBlock; }
    set AirlinesShipmentsBlock(value: boolean) {
        if (this.EntityPM.AirlinesShipmentsBlock != value) {
            this.EntityPM.AirlinesShipmentsBlock = value;
        }
    }

    get AirlinesInvoicesBlock() { return this.EntityPM.AirlinesInvoicesBlock; }
    set AirlinesInvoicesBlock(value: boolean) {
        if (this.EntityPM.AirlinesInvoicesBlock != value) {
            this.EntityPM.AirlinesInvoicesBlock = value;
        }
    }

    get ShippingLinesShipmentsBlock() { return this.EntityPM.ShippingLinesShipmentsBlock; }
    set ShippingLinesShipmentsBlock(value: boolean) {
        if (this.EntityPM.ShippingLinesShipmentsBlock != value) {
            this.EntityPM.ShippingLinesShipmentsBlock = value;
        }
    }

    get ShippingLinesInvoicesBlock() { return this.EntityPM.ShippingLinesInvoicesBlock; }
    set ShippingLinesInvoicesBlock(value: boolean) {
        if (this.EntityPM.ShippingLinesInvoicesBlock != value) {
            this.EntityPM.ShippingLinesInvoicesBlock = value;
        }
    }

    get TruckersShipmentsBlock() { return this.EntityPM.TruckersShipmentsBlock; }
    set TruckersShipmentsBlock(value: boolean) {
        if (this.EntityPM.TruckersShipmentsBlock != value) {
            this.EntityPM.TruckersShipmentsBlock = value;
        }
    }

    get TruckersInvoicesBlock() { return this.EntityPM.TruckersInvoicesBlock; }
    set TruckersInvoicesBlock(value: boolean) {
        if (this.EntityPM.TruckersInvoicesBlock != value) {
            this.EntityPM.TruckersInvoicesBlock = value;
        }
    }

    get VendorsShipmentsBlock() { return this.EntityPM.VendorsShipmentsBlock; }
    set VendorsShipmentsBlock(value: boolean) {
        if (this.EntityPM.VendorsShipmentsBlock != value) {
            this.EntityPM.VendorsShipmentsBlock = value;
        }
    }

    get VendorsInvoicesBlock() { return this.EntityPM.VendorsInvoicesBlock; }
    set VendorsInvoicesBlock(value: boolean) {
        if (this.EntityPM.VendorsInvoicesBlock != value) {
            this.EntityPM.VendorsInvoicesBlock = value;
        }
    }

    get WarehousesShipmentsBlock() { return this.EntityPM.WarehousesShipmentsBlock; }
    set WarehousesShipmentsBlock(value: boolean) {
        if (this.EntityPM.WarehousesShipmentsBlock != value) {
            this.EntityPM.WarehousesShipmentsBlock = value;
        }
    }

    get WarehousesInvoicesBlock() { return this.EntityPM.WarehousesInvoicesBlock; }
    set WarehousesInvoicesBlock(value: boolean) {
        if (this.EntityPM.WarehousesInvoicesBlock != value) {
            this.EntityPM.WarehousesInvoicesBlock = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        if (this.EntityPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.EntityPM.Id == null) {
                this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();

                    if (!myRespone.HasError) {
                        ObjectsLocator.CreditLimitSettingPM = this.EntityPM;
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }

            else {
                this.myService.update(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();

                    if (!myRespone.HasError) {
                        ObjectsLocator.CreditLimitSettingPM = this.EntityPM;
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }
        }

        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }
}
