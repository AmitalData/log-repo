import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {ShipmentTool, RoutingHelper} from '../../../../../Shipment/Tools';
import {PortList} from '../../../../../Common/EntityLists/PortList';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { PortListService } from '../../../../../Common/Services/StandardLists/PortListService';

@Component({
    

    selector: 'AWBHouseRoutingsTabComponent',
    templateUrl: './AWBHouseRoutingsTabComponent.html',
})

export class AWBHouseRoutingsTabComponent extends BaseComponent implements AfterViewInit {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: AWBHouseRoutingsTabComponent = this;
    public ObjectTableName: string;
    private myPortListService: PortListService;

    constructor() {
        super();
        if (this.myPortListService == null) {
            this.myPortListService = new PortListService();
        }
    }

    ngAfterViewInit() {
        this.SetUIProperties();
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.Validate();
        this.SetUIProperties();
    }

    RefreshTab() {
        this.Validate();
        this.SetUIProperties();
    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                    this.FireWizardEvent();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.SetUIProperties();
                    this.FireWizardEvent();
                }
            });
        }
    }


    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        var isPortsEnabled = this.IsEditingEnabled;
        if (isPortsEnabled) {
            if (this.EntityPM.MasterShipmentDataId != null) {
                isPortsEnabled = false;
            }
        }

        if (isPortsEnabled) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.BookingId)) {
                isPortsEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("House", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isPortsEnabled);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, isPortsEnabled);

        this.UIProperties.SetEnabled("HasOnForwarding", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("HasOnForwarding", this.ObjectTableName, this.IsEditingEnabled);       
        this.UIProperties.SetEnabled("PreForwardingFromPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnForwardingToPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.SetUIProperties_PreForwarding();
        this.SetUIProperties_OnForwarding();
    }

    SetUIProperties_PreForwarding() {
        var isPreForwardingPortRequired = false;

        if (this.HasPreForwarding && AppTool.IsNullOrEmpty(this.PreForwardingFromPortId)) {
            isPreForwardingPortRequired = true;
        }

        this.UIProperties.SetRequired("PreForwardingFromPortId", this.ObjectTableName, isPreForwardingPortRequired);
        this.FireWizardEvent();
    }

    SetUIProperties_OnForwarding() {
        var isOnForwardingPortRequired = false;

        if (this.HasOnForwarding && AppTool.IsNullOrEmpty(this.OnForwardingToPortId)) {
            isOnForwardingPortRequired = true;
        }

        this.UIProperties.SetRequired("OnForwardingToPortId", this.ObjectTableName, isOnForwardingPortRequired);
        this.FireWizardEvent();
    }

    public ShowWarning_House: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_ROU();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
    }
    private Validate() {
        if (!this.Wizard.IsImportWizard) {
            this.ShowWarning_House = AppTool.IsNullOrEmpty(this.House) ? true : false;
        }
    }

    private AllPorts: PortList[];
    private AddPort(list: PortList) {
        if (this.AllPorts == null) {
            this.AllPorts = [];
        }

        if (list != null) {
            if (this.AllPorts.indexOf(list) == -1) {
                this.AllPorts.push(list);
            }
        }
    }

    get HasPreForwarding() { return this.EntityPM.HasPreForwarding; }
    set HasPreForwarding(newValue: boolean) {
        if (this.EntityPM.HasPreForwarding != newValue) {
            this.EntityPM.HasPreForwarding = newValue;
            this.SetPreForwarding();
            this.SetUIProperties_PreForwarding();
        }
    }

    get PreForwardingFromPortId() { return this.EntityPM.PreForwardingFromPortId; }
    set PreForwardingFromPortId(newValue: string) {
        if (this.EntityPM.PreForwardingFromPortId != newValue) {
            this.EntityPM.PreForwardingFromPortId = newValue;
            this.SetUIProperties_PreForwarding();
        }
    }

    private preForwardingFromPort: PortList;
    get PreForwardingFromPort() { return this.preForwardingFromPort; }
    set PreForwardingFromPort(list: PortList) {
        if (this.preForwardingFromPort != list) {
            this.preForwardingFromPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.PreForwardingFromPortCode) {
                if (list == null) {
                    this.EntityPM.PreForwardingFromPortCode = null;
                    this.EntityPM.PreForwardingFromPortName = null;
                    this.EntityPM.PreForwardingFromPortCountryCode = null;
                    this.EntityPM.PreForwardingFromPortCountryName = null;
                }

                else {
                    this.EntityPM.PreForwardingFromPortCode = list.Code;
                    this.EntityPM.PreForwardingFromPortName = list.EnglishName;
                    this.EntityPM.PreForwardingFromPortCountryCode = list.CountryCode;
                    this.EntityPM.PreForwardingFromPortCountryName = list.CountryName;
                }
            }
        }
    }

    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFromPortId != newValue) {
            this.EntityPM.FromPortId = newValue;
            this.EntityPM.MainCarriageFromPortId = newValue;
            this.SetPreForwarding();
            this.FireWizardEvent();
            this.SetUIProperties();
    
        }
    }

    private mainCarriageFromPort: PortList;
    get MainCarriageFromPort() { return this.mainCarriageFromPort; }
    set MainCarriageFromPort(list: PortList) {
        if (this.mainCarriageFromPort != list) {
            this.mainCarriageFromPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.MainCarriageFromPortCode) {
                if (list == null) {
                    this.EntityPM.FromCountryId = null;
                    this.EntityPM.FromCountryIsEC = false;
                    this.EntityPM.MainCarriageFromPortCode = null;
                    this.EntityPM.MainCarriageFromPortName = null;
                    this.EntityPM.MainCarriageFromPortCountryCode = null;
                    this.EntityPM.MainCarriageFromPortCountryName = null;
                    ShipmentTool.ComputeSCI(this.EntityPM);
                    ShipmentTool.BuildAWBPlaceField(this.EntityPM);                    
                }

                else {
                    this.EntityPM.FromCountryId = list.CountryId;
                    this.EntityPM.FromCountryIsEC = list.CountryEC;
                    this.EntityPM.MainCarriageFromPortCode = list.Code;
                    this.EntityPM.MainCarriageFromPortName = list.EnglishName;
                    this.EntityPM.MainCarriageFromPortCountryCode = list.CountryCode;
                    this.EntityPM.MainCarriageFromPortCountryName = list.CountryName;
                    ShipmentTool.ComputeSCI(this.EntityPM);
                    ShipmentTool.BuildAWBPlaceField(this.EntityPM);
                }
            }
        }
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.EntityPM.MainCarriageToPortId = value;
            this.EntityPM.MainCarriageFinalDestinationPortId = value;
           
            this.SetOnForwarding();
            this.FireWizardEvent();
            this.SetUIProperties();
        }
    }

    private toPort: PortList;
    get ToPort() { return this.toPort; }
    set ToPort(list: PortList) {
        if (this.toPort != list) {
            this.toPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.MainCarriageToPortCode) {
                if (list == null) {
                    this.EntityPM.ToCountryId = null;
                    this.EntityPM.ToCountryIsEC = false;
                    this.EntityPM.MainCarriageToPortCode = null;
                    this.EntityPM.FinalDistenationPortId = null;
                    ShipmentTool.ComputeSCI(this.EntityPM);
                }

                else {
                    this.EntityPM.ToCountryId = list.CountryId;
                    this.EntityPM.ToCountryIsEC = list.CountryEC;
                    this.EntityPM.MainCarriageToPortCode = list.Code;
                    this.EntityPM.FinalDistenationPortId = list.Id;
                    ShipmentTool.ComputeSCI(this.EntityPM);
                }
            }            
        }
    }

    get HasOnForwarding() { return this.EntityPM.HasOnForwarding; }
    set HasOnForwarding(newValue: boolean) {
        if (this.EntityPM.HasOnForwarding != newValue) {
            this.EntityPM.HasOnForwarding = newValue;
            this.SetOnForwarding();
            this.SetUIProperties_OnForwarding();
        }
    }

    get OnForwardingToPortId() { return this.EntityPM.OnForwardingToPortId; }
    set OnForwardingToPortId(newValue: string) {
        if (this.EntityPM.OnForwardingToPortId != newValue) {
            this.EntityPM.OnForwardingToPortId = newValue;
            this.SetUIProperties_OnForwarding();
        }
    }

    private onForwardingToPort: PortList;
    get OnForwardingToPort() { return this.onForwardingToPort; }
    set OnForwardingToPort(list: PortList) {
        if (this.onForwardingToPort != list) {
            this.onForwardingToPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.OnForwardingToPortCode) {
                if (list == null) {
                    this.EntityPM.OnForwardingToPortCode = null;
                    this.EntityPM.OnForwardingToPortName = null;
                    this.EntityPM.OnForwardingToPortCountryCode = null;
                    this.EntityPM.OnForwardingToPortCountryName = null;
                }

                else {
                    this.EntityPM.OnForwardingToPortCode = list.Code;
                    this.EntityPM.OnForwardingToPortName = list.EnglishName;
                    this.EntityPM.OnForwardingToPortCountryCode = list.CountryCode;
                    this.EntityPM.OnForwardingToPortCountryName = list.CountryName;
                }
            }
        }
    }

    private SetPreForwarding() {
        if (this.HasPreForwarding) {
            this.EntityPM.PreForwardingTransportModeId = "A";
            this.EntityPM.PreForwardingToPortId = this.EntityPM.FromPortId;
            this.EntityPM.PreForwardingToPortCode = this.EntityPM.MainCarriageFromPortCode;
            this.EntityPM.PreForwardingToPortName = this.EntityPM.MainCarriageFromPortName;
            this.EntityPM.PreForwardingToPortCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
            this.EntityPM.PreForwardingToPortCountryName = this.EntityPM.MainCarriageFromPortCountryName;
        }

        else {
            RoutingHelper.RemovePreForwardingLeg(this.EntityPM);
        }
    }
    private SetOnForwarding() {
        if (this.HasOnForwarding) {
            this.EntityPM.OnForwardingTransportModeId = "A";
            this.EntityPM.OnForwardingFromPortId = this.EntityPM.ToPortId;
            this.EntityPM.OnForwardingFromPortCode = this.EntityPM.MainCarriageToPortCode;
            this.EntityPM.OnForwardingFromPortName = this.EntityPM.MainCarriageToPortName;
            this.EntityPM.OnForwardingFromPortCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
            this.EntityPM.OnForwardingFromPortCountryName = this.EntityPM.MainCarriageToPortCountryName;
        }

        else {
            RoutingHelper.RemoveOnForwardingLeg(this.EntityPM);
        }
    }

    get House() { return this.EntityPM.House; }
    set House(newValue: string) {
        if (this.EntityPM.House != newValue) {
            this.EntityPM.House = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }
}
