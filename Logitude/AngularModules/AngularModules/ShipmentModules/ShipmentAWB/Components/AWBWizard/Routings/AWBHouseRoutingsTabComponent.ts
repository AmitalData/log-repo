import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {ShipmentTool, RoutingHelper} from '../../../../../Shipment/Tools';
import {PortList} from '../../../../../Common/EntityLists/PortList';

@Component({
    moduleId: module.id,

    selector: 'AWBHouseRoutingsTabComponent',
    templateUrl: './AWBHouseRoutingsTabComponent.html',
})

export class AWBHouseRoutingsTabComponent extends BaseComponent implements AfterViewInit {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: AWBHouseRoutingsTabComponent = this;
    public ObjectTableName: string;
    constructor() {
        super();
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

        this.UIProperties.SetEnabled("HasOnCarriage", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("HasOnCarriage", this.ObjectTableName, this.IsEditingEnabled);       
        this.UIProperties.SetEnabled("PreCarriageFromPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("OnCarriageToPortId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
        this.SetUIProperties_PreCarriage();
        this.SetUIProperties_OnCarriage();
    }

    SetUIProperties_PreCarriage() {
        var isPreCarriagePortRequired = false;

        if (this.HasPreCarriage && AppTool.IsNullOrEmpty(this.PreCarriageFromPortId)) {
            isPreCarriagePortRequired = true;
        }

        this.UIProperties.SetRequired("PreCarriageFromPortId", this.ObjectTableName, isPreCarriagePortRequired);
        this.FireWizardEvent();
    }

    SetUIProperties_OnCarriage() {
        var isOnCarriagePortRequired = false;

        if (this.HasOnCarriage && AppTool.IsNullOrEmpty(this.OnCarriageToPortId)) {
            isOnCarriagePortRequired = true;
        }

        this.UIProperties.SetRequired("OnCarriageToPortId", this.ObjectTableName, isOnCarriagePortRequired);
        this.FireWizardEvent();
    }

    public ShowWarning_House: boolean = false;
    private FireWizardEvent() {
        this.Wizard.ValidateScreen_ROU();
        this.Wizard.ValidateScreen_GEN();
        this.Wizard.ValidateScreen_PAC();
    }
    private Validate() {
        this.ShowWarning_House = AppTool.IsNullOrEmpty(this.House) ? true : false;
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

    get HasPreCarriage() { return this.EntityPM.HasPreCarriage; }
    set HasPreCarriage(newValue: boolean) {
        if (this.EntityPM.HasPreCarriage != newValue) {
            this.EntityPM.HasPreCarriage = newValue;
            this.SetPreCarriage();
            this.SetUIProperties_PreCarriage();
        }
    }

    get PreCarriageFromPortId() { return this.EntityPM.PreCarriageFromPortId; }
    set PreCarriageFromPortId(newValue: string) {
        if (this.EntityPM.PreCarriageFromPortId != newValue) {
            this.EntityPM.PreCarriageFromPortId = newValue;
            this.SetUIProperties_PreCarriage();
        }
    }

    private preCarriageFromPort: PortList;
    get PreCarriageFromPort() { return this.preCarriageFromPort; }
    set PreCarriageFromPort(list: PortList) {
        if (this.preCarriageFromPort != list) {
            this.preCarriageFromPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.PreCarriageFromPortCode) {
                if (list == null) {
                    this.EntityPM.PreCarriageFromPortCode = null;
                    this.EntityPM.PreCarriageFromPortName = null;
                    this.EntityPM.PreCarriageFromPortCountryCode = null;
                    this.EntityPM.PreCarriageFromPortCountryName = null;
                }

                else {
                    this.EntityPM.PreCarriageFromPortCode = list.Code;
                    this.EntityPM.PreCarriageFromPortName = list.EnglishName;
                    this.EntityPM.PreCarriageFromPortCountryCode = list.CountryCode;
                    this.EntityPM.PreCarriageFromPortCountryName = list.CountryName;
                }
            }
        }
    }

    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFromPortId != newValue) {
            this.EntityPM.FromPortId = newValue;
            this.EntityPM.MainCarriageFromPortId = newValue;
            this.SetPreCarriage();
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
            this.SetOnCarriage();
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

            //var Code = list == null ? null : list.Code;
            //if (Code != this.EntityPM.MainCarriageToPortCode) {
            //    if (list == null) {
            //        this.EntityPM.MainCarriageToPortCode = null;
            //        this.EntityPM.MainCarriageToPortName = null;
            //        this.EntityPM.MainCarriageToPortCountryCode = null;
            //        this.EntityPM.MainCarriageToPortCountryName = null;
            //        //this.BuildLegs();
            //    }

            //    else {
            //        this.EntityPM.MainCarriageToPortCode = list.Code;
            //        this.EntityPM.MainCarriageToPortName = list.EnglishName;
            //        this.EntityPM.MainCarriageToPortCountryCode = list.CountryCode;
            //        this.EntityPM.MainCarriageToPortCountryName = list.CountryName;
            //        //this.BuildLegs();
            //    }
            //}
        }
    }

    //get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
    //set MainCarriageToPortId(newValue: string) {
    //    if (this.EntityPM.MainCarriageToPortId != newValue) {
    //        this.EntityPM.ToPortId = newValue;
    //        this.EntityPM.MainCarriageToPortId = newValue;
    //        this.EntityPM.MainCarriageFinalDestinationPortId = newValue;
    //        this.SetOnCarriage();            
    //        this.FireWizardEvent();
    //        this.SetUIProperties();
    //    }
    //}

    //private mainCarriageToPort: PortList;
    //get MainCarriageToPort() { return this.mainCarriageToPort; }
    //set MainCarriageToPort(list: PortList) {
    //    if (this.mainCarriageToPort != list) {
    //        this.mainCarriageToPort = list;
    //        this.AddPort(list);

    //        var Code = list == null ? null : list.Code;
    //        if (Code != this.EntityPM.MainCarriageToPortCode) {
    //            if (list == null) {
    //                this.EntityPM.MainCarriageToPortCode = null;
    //                this.EntityPM.MainCarriageToPortName = null;
    //                this.EntityPM.MainCarriageToPortCountryCode = null;
    //                this.EntityPM.MainCarriageToPortCountryName = null;
    //                //this.BuildLegs();
    //            }

    //            else {
    //                this.EntityPM.MainCarriageToPortCode = list.Code;
    //                this.EntityPM.MainCarriageToPortName = list.EnglishName;
    //                this.EntityPM.MainCarriageToPortCountryCode = list.CountryCode;
    //                this.EntityPM.MainCarriageToPortCountryName = list.CountryName;
    //                //this.BuildLegs();
    //            }
    //        }
    //    }
    //}


    get HasOnCarriage() { return this.EntityPM.HasOnCarriage; }
    set HasOnCarriage(newValue: boolean) {
        if (this.EntityPM.HasOnCarriage != newValue) {
            this.EntityPM.HasOnCarriage = newValue;
            this.SetOnCarriage();
            this.SetUIProperties_OnCarriage();
        }
    }

    get OnCarriageToPortId() { return this.EntityPM.OnCarriageToPortId; }
    set OnCarriageToPortId(newValue: string) {
        if (this.EntityPM.OnCarriageToPortId != newValue) {
            this.EntityPM.OnCarriageToPortId = newValue;
            this.SetUIProperties_OnCarriage();
        }
    }

    private onCarriageToPort: PortList;
    get OnCarriageToPort() { return this.onCarriageToPort; }
    set OnCarriageToPort(list: PortList) {
        if (this.onCarriageToPort != list) {
            this.onCarriageToPort = list;
            this.AddPort(list);

            var Code = list == null ? null : list.Code;
            if (Code != this.EntityPM.OnCarriageToPortCode) {
                if (list == null) {
                    this.EntityPM.OnCarriageToPortCode = null;
                    this.EntityPM.OnCarriageToPortName = null;
                    this.EntityPM.OnCarriageToPortCountryCode = null;
                    this.EntityPM.OnCarriageToPortCountryName = null;
                }

                else {
                    this.EntityPM.OnCarriageToPortCode = list.Code;
                    this.EntityPM.OnCarriageToPortName = list.EnglishName;
                    this.EntityPM.OnCarriageToPortCountryCode = list.CountryCode;
                    this.EntityPM.OnCarriageToPortCountryName = list.CountryName;
                }
            }
        }
    }

    private SetPreCarriage() {
        if (this.HasPreCarriage) {
            this.EntityPM.PreCarriageTransportModeId = "A";
            this.EntityPM.PreCarriageToPortId = this.EntityPM.FromPortId;
            this.EntityPM.PreCarriageToPortCode = this.EntityPM.MainCarriageFromPortCode;
            this.EntityPM.PreCarriageToPortName = this.EntityPM.MainCarriageFromPortName;
            this.EntityPM.PreCarriageToPortCountryCode = this.EntityPM.MainCarriageFromPortCountryCode;
            this.EntityPM.PreCarriageToPortCountryName = this.EntityPM.MainCarriageFromPortCountryName;
        }

        else {
            RoutingHelper.RemovePreCarriageLeg(this.EntityPM);
        }
    }
    private SetOnCarriage() {
        if (this.HasOnCarriage) {
            this.EntityPM.OnCarriageTransportModeId = "A";
            this.EntityPM.OnCarriageFromPortId = this.EntityPM.ToPortId;
            this.EntityPM.OnCarriageFromPortCode = this.EntityPM.MainCarriageToPortCode;
            this.EntityPM.OnCarriageFromPortName = this.EntityPM.MainCarriageToPortName;
            this.EntityPM.OnCarriageFromPortCountryCode = this.EntityPM.MainCarriageToPortCountryCode;
            this.EntityPM.OnCarriageFromPortCountryName = this.EntityPM.MainCarriageToPortCountryName;
        }

        else {
            RoutingHelper.RemoveOnCarriageLeg(this.EntityPM);
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