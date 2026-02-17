import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { SharedLogisticsSettingPM } from '../../Infrastructure/EntityPMs/SharedLogisticsSettingPM';
import { SharedLogisticsSettingPMService } from '../../Infrastructure/Services/StandardPMs/SharedLogisticsSettingPMService';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './SharedLogisticsPartnersPermissiosComponent.html',
})

export class SharedLogisticsPartnersPermissiosComponent implements OnInit {
    public PartnersList: PartnerItem[];
    public CarriersList: PartnerItem[];

    public EntityPM: SharedLogisticsSettingPM;
    public TenantZeroEntity: SharedLogisticsSettingPM;
    private myService: SharedLogisticsSettingPMService;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityResourceService: EntityResourceService) {
        this.myService = new SharedLogisticsSettingPMService();
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("SharedLogisticsSetting").subscribe((res: any) => {
            var tenantZeroId: number = 0;
            this.myService.get(tenantZeroId.toString()).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    if (myResponse.Result) {
                        this.TenantZeroEntity = myResponse.Result;
                    }

                    else {
                        this.TenantZeroEntity = new SharedLogisticsSettingPM();
                    }

                    this.LoadMyTenantData();
                }
            });           
        });
    }

    private LoadMyTenantData() {
        this.myService.get(SessionLocator.Tenant.toString()).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                if (myResponse.Result) {
                    this.EntityPM = myResponse.Result;
                }

                else {
                    this.EntityPM = new SharedLogisticsSettingPM();
                    this.EntityPM.IsDirty = false;
                }

                if (this.EntityPM != null && this.TenantZeroEntity != null) {
                    this.BuildData();
                }

                this.IsResourcesReady = true;
            }
        });
    }

    private BuildData() {
        this.PartnersList = [];
        this.CarriersList = [];

        var item1: PartnerItem = new PartnerItem(this);
        item1.Code = "SH";
        item1.PartnerName = "Shipper";
        item1.SuggestedIsChecked = this.TenantZeroEntity.IsShipperShared;
        item1.ChooseIsChecked = this.EntityPM.IsShipperShared;

        var item2: PartnerItem = new PartnerItem(this);
        item2.Code = "CO";
        item2.PartnerName = "Consignee";
        item2.SuggestedIsChecked = this.TenantZeroEntity.IsConsigneeShared;
        item2.ChooseIsChecked = this.EntityPM.IsConsigneeShared;

        var item3: PartnerItem = new PartnerItem(this);
        item3.Code = "AG";
        item3.PartnerName = "Agent";
        item3.SuggestedIsChecked = this.TenantZeroEntity.IsAgentShared;
        item3.ChooseIsChecked = this.EntityPM.IsAgentShared;

        var item4: PartnerItem = new PartnerItem(this);
        item4.Code = "SN";
        item4.PartnerName = "Shipper Not Exporter";
        item4.SuggestedIsChecked = this.TenantZeroEntity.IsShipperNotExporterShared;
        item4.ChooseIsChecked = this.EntityPM.IsShipperNotExporterShared;

        var item5: PartnerItem = new PartnerItem(this);
        item5.Code = "CN";
        item5.PartnerName = "Consignee Not Importer";
        item5.SuggestedIsChecked = this.TenantZeroEntity.IsConsigneeNotImporterShared;
        item5.ChooseIsChecked = this.EntityPM.IsConsigneeNotImporterShared;

        var item6: PartnerItem = new PartnerItem(this);
        item6.Code = "N1";
        item6.PartnerName = "Notify 1";
        item6.SuggestedIsChecked = this.TenantZeroEntity.IsNotify1Shared;
        item6.ChooseIsChecked = this.EntityPM.IsNotify1Shared;

        var item7: PartnerItem = new PartnerItem(this);
        item7.Code = "N2";
        item7.PartnerName = "Notify 2";
        item7.SuggestedIsChecked = this.TenantZeroEntity.IsNotify2Shared;
        item7.ChooseIsChecked = this.EntityPM.IsNotify2Shared;

        var item8: PartnerItem = new PartnerItem(this);
        item8.Code = "FF";
        item8.PartnerName = "Freight Forwarder";
        item8.SuggestedIsChecked = this.TenantZeroEntity.IsFreightForwarderShared;
        item8.ChooseIsChecked = this.EntityPM.IsFreightForwarderShared;

        var item9: PartnerItem = new PartnerItem(this);
        item9.Code = "CL";
        item9.PartnerName = "Coloader";
        item9.SuggestedIsChecked = this.TenantZeroEntity.IsColoaderShared;
        item9.ChooseIsChecked = this.EntityPM.IsColoaderShared;
        
        var item10: PartnerItem = new PartnerItem(this);
        item10.Code = "CE";
        item10.PartnerName = "Custom Agent Export";
        item10.SuggestedIsChecked = this.TenantZeroEntity.IsCustomsAgentExportShared;
        item10.ChooseIsChecked = this.EntityPM.IsCustomsAgentExportShared;

        var item11: PartnerItem = new PartnerItem(this);
        item11.Code = "CI";
        item11.PartnerName = "Custom Agent Import";
        item11.SuggestedIsChecked = this.TenantZeroEntity.IsCustomsAgentImportShared;
        item11.ChooseIsChecked = this.EntityPM.IsCustomsAgentImportShared;

        var item12: PartnerItem = new PartnerItem(this);
        item12.Code = "CC";
        item12.PartnerName = "Custom Clearance Point";
        item12.SuggestedIsChecked = this.TenantZeroEntity.IsCustomClearancePoinShared;
        item12.ChooseIsChecked = this.EntityPM.IsCustomClearancePoinShared;

        var item13: PartnerItem = new PartnerItem(this);
        item13.Code = "CD";
        item13.PartnerName = "Consolidator";
        item13.SuggestedIsChecked = this.TenantZeroEntity.IsConsolidatorShared;
        item13.ChooseIsChecked = this.EntityPM.IsConsolidatorShared;

        var item14: PartnerItem = new PartnerItem(this);
        item14.Code = "RL";
        item14.PartnerName = "Releasing Agent";
        item14.SuggestedIsChecked = this.TenantZeroEntity.IsReleasingAgentShared;
        item14.ChooseIsChecked = this.EntityPM.IsReleasingAgentShared;

        var item15: PartnerItem = new PartnerItem(this);
        item15.Code = "IS";
        item15.PartnerName = "Issuing Carrier Agent";
        item15.SuggestedIsChecked = this.TenantZeroEntity.IsIssuingCarrierAgentShared;
        item15.ChooseIsChecked = this.EntityPM.IsIssuingCarrierAgentShared;
        
        var item16: PartnerItem = new PartnerItem(this);
        item16.Code = "PI";
        item16.PartnerName = "Pickup and Deliveries Carriers";
        item16.SuggestedIsChecked = this.TenantZeroEntity.IsPickDelivCarriesShared;
        item16.ChooseIsChecked = this.EntityPM.IsPickDelivCarriesShared;

        var item17: PartnerItem = new PartnerItem(this);
        item17.Code = "MC";
        item17.PartnerName = "Main Carriage Carrier";
        item17.SuggestedIsChecked = this.TenantZeroEntity.IsMainCarrierShared;
        item17.ChooseIsChecked = this.EntityPM.IsMainCarrierShared;

        this.PartnersList.push(item1);
        this.PartnersList.push(item2);
        this.PartnersList.push(item3);
        this.PartnersList.push(item4);
        this.PartnersList.push(item5);
        this.PartnersList.push(item6);
        this.PartnersList.push(item7);
        this.PartnersList.push(item8);
        this.PartnersList.push(item9);
        this.PartnersList.push(item10);
        this.PartnersList.push(item11);
        this.PartnersList.push(item12);
        this.PartnersList.push(item13);
        this.PartnersList.push(item14);
        this.PartnersList.push(item15);

        this.CarriersList.push(item16);
        this.CarriersList.push(item17);
    }

    private mySearchText: string;
    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";

        this.mySearchText = searchText;
        this.BuildData();
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();

                if (this.EntityPM.Tenant == null) {
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();

                        if (!myRespone.HasError) {
                            ObjectsLocator.SharedLogisticsSettingPM = this.EntityPM;
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
                            ObjectsLocator.SharedLogisticsSettingPM = this.EntityPM;
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
}

export class PartnerItem {

    constructor(public father: SharedLogisticsPartnersPermissiosComponent) {

    }

    public Code: string;
    public PartnerName: string;
    public SuggestedIsChecked: boolean;
    public IsEnabled: boolean = true;

    private chooseIsChecked: boolean;
    public get ChooseIsChecked() { return this.chooseIsChecked; }
    public set ChooseIsChecked(value: boolean) {
        if (this.chooseIsChecked != value) {
            this.chooseIsChecked = value;

            this.SetIsChecked();
        }
    }

    private SetIsChecked() {
        switch (this.Code) {
            case "SH": {
                this.father.EntityPM.IsShipperShared = this.ChooseIsChecked;
                break;
            }

            case "CO": {
                this.father.EntityPM.IsConsigneeShared = this.ChooseIsChecked;
                break;
            }

            case "AG": {
                this.father.EntityPM.IsAgentShared = this.ChooseIsChecked;
                break;
            }

            case "SN": {
                this.father.EntityPM.IsShipperNotExporterShared = this.ChooseIsChecked;
                break;
            }

            case "CN": {
                this.father.EntityPM.IsConsigneeNotImporterShared = this.ChooseIsChecked;
                break;
            }

            case "N1": {
                this.father.EntityPM.IsNotify1Shared = this.ChooseIsChecked;
                break;
            }

            case "N2": {
                this.father.EntityPM.IsNotify2Shared = this.ChooseIsChecked;
                break;
            }

            case "FF": {
                this.father.EntityPM.IsFreightForwarderShared = this.ChooseIsChecked;
                break;
            }

            case "CL": {
                this.father.EntityPM.IsColoaderShared = this.ChooseIsChecked;
                break;
            }

            case "CE": {
                this.father.EntityPM.IsCustomsAgentExportShared = this.ChooseIsChecked;
                break;
            }

            case "CI": {
                this.father.EntityPM.IsCustomsAgentImportShared = this.ChooseIsChecked;
                break;
            }

            case "CC": {
                this.father.EntityPM.IsCustomClearancePoinShared = this.ChooseIsChecked;
                break;
            }

            case "CD": {
                this.father.EntityPM.IsConsolidatorShared = this.ChooseIsChecked;
                break;
            }

            case "RL": {
                this.father.EntityPM.IsReleasingAgentShared = this.ChooseIsChecked;
                break;
            }

            case "IS": {
                this.father.EntityPM.IsIssuingCarrierAgentShared = this.ChooseIsChecked;
                break;
            }

            case "PI": {
                this.father.EntityPM.IsPickDelivCarriesShared = this.ChooseIsChecked;
                break;
            }

            case "MC": {
                this.father.EntityPM.IsMainCarrierShared = this.ChooseIsChecked;
                break;
            }
        }
    }
}
