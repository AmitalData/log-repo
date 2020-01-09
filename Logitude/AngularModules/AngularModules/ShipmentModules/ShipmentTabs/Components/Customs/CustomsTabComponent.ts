import { Component, OnInit, OnDestroy, ViewChild, ViewContainerRef}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentTool} from '../../../../Shipment/Tools';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentCustomsTransmissionPM} from  '../../../../Shipment/EntityPMs/ShipmentCustomsTransmissionPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsTabComponent.html',
})

export class CustomsTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: ShipmentPM = null;
    public ObjectTableName: string = null;
    public DataContext: CustomsTabComponent = this;
    public IsSendToAESButtonVisible: boolean = false;
    public DeclarationNumberLabel: string = TextCodeTranslator.Translate("Shipment.F.DeclarationNumber");
    public DeclarationDateLabel: string = TextCodeTranslator.Translate("Shipment.F.DeclarationDate");
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.SetFlags();
        this.Listen();
        this.LoadSummaryData();

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "SENDTOAES")) {
            this.IsSendToAESButtonVisible = true;
        }

        if (SessionLocator.TenantPM.CountryCode == "US") {
            this.DeclarationNumberLabel = "Entry Summary";
            this.DeclarationDateLabel = "Entry Summary Date";
        }

      this.BuildAdditionalFields();
    }

  // Additional Fields
  private Retries: number = 0;
  private timerToken: any;
  private GeneratedComponent: any;
  BuildAdditionalFields() {
    this.RunComponent();
  }
  RunComponent() {
    if (this.viewContainerRef) {
      this.LoadChildComponent();
    }

    else {
      this.RunComponentTimer();
    }
  }
  RunComponentTimer() {
    this.Retries++;

    if (this.timerToken) {
      clearTimeout(this.timerToken);
    }

    if (this.Retries < 3) {
      this.timerToken = setTimeout(() => this.RunComponent(), 1);
    }
  }

  LoadChildComponent() { 
    SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
      .then(cmpRef => {

        //this.GeneratedComponent = cmpRef.instance;

        cmpRef.instance.LoadCompleted.subscribe(s => {
          
        });

        var screenCode = "Shipment.CustomsAdditionalFields";
        //cmpRef.instance.LabelWidth = 110;
        cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);
      });
  }

    public IsTenantUS: boolean = false;
    public IsOceanOrAir: boolean = false;
    public IsOceanImport: boolean = false;
    public IsDirectOrHouse: boolean = false;
    public IsDirectOrMaster: boolean = false;
    public IsExport: boolean = false;
    SetFlags() {
        if (SessionLocator.TenantPM.CountryCode) {
            if (SessionLocator.TenantPM.CountryCode.toUpperCase() == "US") {
                this.IsTenantUS = true;
            }
        }

        this.IsOceanOrAir = this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "A" ? true : false;
        this.IsOceanImport = this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" ? true : false;
        this.IsExport = this.EntityPM.DirectionId == "E" ? true : false;

        this.SetSummaryAreaFlags();

        switch (this.EntityPM.ShipmentLevelCode) {
            case "H": {
                this.IsDirectOrHouse = true;
                break;
            }

            case "C": {
                this.IsDirectOrMaster = true;
                break;
            }

            case "D": {
                this.IsDirectOrHouse = true;
                this.IsDirectOrMaster = true;
                break;
            }
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "RefreshCustomsSummary") {
                    this.LoadSummaryData();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadSummaryData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadSummaryData();
                }
            });
        }
    }

    ngOnDestroy() {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }

        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }

        if (this.SessionEvent) {
            this.SessionEvent.unsubscribe();
            this.SessionEvent = null;
        }
    }
    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    }

    public IsCustomFollowUpEnabled: boolean = false;
    public IsEditingEnabled: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        var isIncludeFieldsEnabled = false;

        if (isEditingEnabled) {
            if (this.IncludesCustoms) {
                isIncludeFieldsEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("IncludesCustoms", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DeclarationNumber", this.ObjectTableName, isIncludeFieldsEnabled);
        this.UIProperties.SetEnabled("DeclarationDate", this.ObjectTableName, isIncludeFieldsEnabled);
        this.UIProperties.SetEnabled("CustomsClearanceDate", this.ObjectTableName, isIncludeFieldsEnabled);

        this.UIProperties.SetEnabled("FreightRelease", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("TerminalAvailable", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ISFNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ISFDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ITNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ITDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ENSNumber", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ENSDate", this.ObjectTableName, isEditingEnabled);

        if (!AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            this.UIProperties.SetRequired("DeclarationDate", this.ObjectTableName, this.DeclarationDate == null);
        }

        else {
            this.UIProperties.SetRequired("DeclarationDate", this.ObjectTableName, false);
        }

        this.IsCustomFollowUpEnabled = isIncludeFieldsEnabled;
        this.IsEditingEnabled = isEditingEnabled;
    }

    get IncludesCustoms() { return this.EntityPM.IncludesCustoms; }
    set IncludesCustoms(newValue: boolean) {
        if (this.EntityPM.IncludesCustoms != newValue) {
            this.EntityPM.IncludesCustoms = newValue;

            if (!newValue) {
                this.DeclarationNumber = null;
                this.DeclarationDate = null;
                this.CustomsClearanceDate = null;
            }

            this.SetUIProperties();
        }
    }

    get DeclarationNumber() { return this.EntityPM.DeclarationNumber; }
    set DeclarationNumber(newValue: string) {
        if (this.EntityPM.DeclarationNumber != newValue) {
            this.EntityPM.DeclarationNumber = newValue;

            this.SetUIProperties();
        }
    }

    get DeclarationDate() { return this.EntityPM.DeclarationDate; }
    set DeclarationDate(newValue: Date) {
        if (this.EntityPM.DeclarationDate != newValue) {
            this.EntityPM.DeclarationDate = newValue;

            this.SetUIProperties();
        }
    }

    get CustomsClearanceDate() { return this.EntityPM.CustomsClearanceDate; }
    set CustomsClearanceDate(newValue: Date) {
        if (this.EntityPM.CustomsClearanceDate != newValue) {
            this.EntityPM.CustomsClearanceDate = newValue;
        }
    }

    get FreightRelease() { return this.EntityPM.FreightRelease; }
    set FreightRelease(value: Date) {
        if (this.EntityPM.FreightRelease != value) {
            this.EntityPM.FreightRelease = value;
        }
    }

    get TerminalAvailable() { return this.EntityPM.TerminalAvailable; }
    set TerminalAvailable(value: Date) {
        if (this.EntityPM.TerminalAvailable != value) {
            this.EntityPM.TerminalAvailable = value;
        }
    }

    get ISFNumber() { return this.EntityPM.ISFNumber; }
    set ISFNumber(value: string) {
        if (this.EntityPM.ISFNumber != value) {
            this.EntityPM.ISFNumber = value;
        }
    }

    get ISFDate() { return this.EntityPM.ISFDate; }
    set ISFDate(value: Date) {
        if (this.EntityPM.ISFDate != value) {
            this.EntityPM.ISFDate = value;
        }
    }

    get ITNumber() { return this.EntityPM.ITNumber; }
    set ITNumber(value: string) {
        if (this.EntityPM.ITNumber != value) {
            this.EntityPM.ITNumber = value;
        }
    }

    get ITDate() { return this.EntityPM.ITDate; }
    set ITDate(value: Date) {
        if (this.EntityPM.ITDate != value) {
            this.EntityPM.ITDate = value;
        }
    }

    get ENSNumber() { return this.EntityPM.ENSNumber; }
    set ENSNumber(value: string) {
        if (this.EntityPM.ENSNumber != value) {
            this.EntityPM.ENSNumber = value;
        }
    }

    get ENSDate() { return this.EntityPM.ENSDate; }
    set ENSDate(value: Date) {
        if (this.EntityPM.ENSDate != value) {
            this.EntityPM.ENSDate = value;
        }
    }

    //Summary
    public SummatyAreaIsVisible: boolean = false;
    public SummaryItemsSource: SummaryItem[];

    private SetSummaryAreaFlags() {
        this.SummatyAreaIsVisible = this.CheckSummaryAreaVisibility();
    }
    private CheckSummaryAreaVisibility(): boolean {
        var visible = true;

        if ((ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")
        ) {
            visible = false;
        }

        return visible;
    }
    private CheckLocalVisibility(): boolean {
        var visible: boolean = false;

        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (!AppTool.IsNullOrEmpty(ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode)) {
                    if (ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "AMC") {
                        visible = true;
                    }

                    else {
                        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
                            visible = true;
                        }
                    }
                }
            }
        }


        //if (FeatureLocator.HasFeaturePermession("Shipment", "SendToCustoms")) {
        //    if (this.EntityPM.ShipmentLevelCode != "C") {
        //        if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
        //            if (!AppTool.IsNullOrEmpty(ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode)) {
        //                visible = true;
        //            }
        //        }
        //    }
        //}

        return visible;
    }
    private CheckArtemusVisibility_BOL(): boolean {
        var visible = false;

        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    visible = true;
                }
            }
        }

        return visible;
    }
    private CheckArtemusVisibility_VOG(): boolean {
        var visible: boolean = false;

        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {
            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    visible = true;
                }
            }
        }

        return visible;
    }
    private CheckAESVisibility(): boolean {
        var visible: boolean = false;

        if (FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "CBP") {
                    if (this.EntityPM.DirectionId == "E") {
                        visible = true;
                    }
                }
            }
        }

        return visible;
    }

    private ShipmentCustomsTransmissionList: ShipmentCustomsTransmissionPM[] = [];
    LoadSummaryData() {
        var myShipmentDomainService: ShipmentDomainService = new ShipmentDomainService();

        myShipmentDomainService.GetShipmentCustomsTransmissionByShipmnetId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ShipmentCustomsTransmissionList = myResponse.Result;

                this.FillSummaryItemsSource();

            }
        });
    }

    private FillSummaryItemsSource() {
        this.SummaryItemsSource = [];
        var notSent: string = "Not Sent";

        var isLocalVisible: boolean = this.CheckLocalVisibility();
        var isBOLVisible: boolean = this.CheckArtemusVisibility_BOL();
        var isVOGVisible: boolean = this.CheckArtemusVisibility_VOG();
        var isAESVisible: boolean = this.CheckAESVisibility();

        if (isLocalVisible) {
            var newItem: SummaryItem = new SummaryItem();
            newItem.CustomsInterfaceName = ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceName;
            newItem.Code = ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode;
            newItem.StatusName = !AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsTransmissionsStatusName) ? this.EntityPM.LocalCustomsTransmissionsStatusName : notSent;
            newItem.StatusDate = this.EntityPM.LocalCustomsTransmissionsStatusDate;
            newItem.StatusCode = this.EntityPM.LocalCustomsTransmissionsStatusCode;
            newItem.UserName = this.EntityPM.LocalCustomsSentByUserName;
            newItem.Error = this.EntityPM.LocalCustomsTransmissionsStatusError;
            newItem.IsSeparatorVisible = false;

            this.SummaryItemsSource.push(newItem);
        }

        if (isVOGVisible) {
            var newItem0: SummaryItem = new SummaryItem();
            newItem0.StatusName = notSent;
            newItem0.Code = "ASVO";
            newItem0.CustomsInterfaceName = "Artemus Voyage";
            newItem0.IsSeparatorVisible = true;

            this.SummaryItemsSource.push(newItem0);
        }

        if (isBOLVisible) {
            var newItem1: SummaryItem = new SummaryItem();
            newItem1.StatusName = notSent;
            newItem1.Code = "ARBL";
            newItem1.CustomsInterfaceName = "Artemus Bill of Lading";
            newItem1.IsSeparatorVisible = true;

            this.SummaryItemsSource.push(newItem1);
        }
        
        if (isAESVisible) {
            var newItem2: SummaryItem = new SummaryItem();
            newItem2.StatusName = notSent;
            newItem2.Code = "CBAS";
            newItem2.CustomsInterfaceName = "CBP AES";
            newItem2.IsSeparatorVisible = true;

            this.SummaryItemsSource.push(newItem2);
        }
        
        if (this.ShipmentCustomsTransmissionList.length > 0) {
            this.SummaryItemsSource.forEach(summaryItem => {
                this.ShipmentCustomsTransmissionList.forEach(item => {
                    if (item.MessageCode == summaryItem.Code) {
                        summaryItem.StatusName = !AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : notSent;
                        summaryItem.StatusDate = item.LastSendDate;
                        summaryItem.StatusCode = item.Status;
                        summaryItem.UserName = item.ByUserName;
                        summaryItem.Error = item.Error;
                    }
                });
            });
        }
    }
}

export class SummaryItem {
    constructor() {

    }

    public Code: string;
    public CustomsInterfaceName: string;
    public StatusCode: string;
    public StatusName: string;
    public Error: string;
    public UserName: string;
    public StatusDate: Date;
    public IsSeparatorVisible: boolean;
}
