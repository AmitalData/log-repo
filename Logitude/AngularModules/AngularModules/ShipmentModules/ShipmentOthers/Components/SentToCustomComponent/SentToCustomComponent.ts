import {Component, Output, EventEmitter} from '@angular/core';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {CustomsWizardArgs, ArtemusWizardArgs} from '../../../../Shipment/Args';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from  '../../../../Controls/Windows/LogitudeWindow'; 
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {ArtemusWebService} from '../../../../Infrastructure/Services/WebServices/ArtemusWebService';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentCustomsTransmissionPM} from  '../../../../Shipment/EntityPMs/ShipmentCustomsTransmissionPM';
import {ABMWebService, ABMResult} from '../../../../Infrastructure/Services/WebServices/ABMWebService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {CommonDomainService} from'../../../../Common/Services/CommonDomainService'; 
import {CustomsInterfaceSettingList} from '../../../../Common/EntityLists/CustomsInterfaceSettingList'; 
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';

@Component({
    selector: 'SentToCustomComponent',
    moduleId: module.id,
    templateUrl: './SentToCustomComponent.html',
})

export class SentToCustomComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ValidationErrorsList: string[] = [];
    public DataContext: SentToCustomComponent = this;
    public ObjectTableName = "ShipmentCustomsTransmission";
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();

    public MessageText: string;
    public IsMessageValid: boolean;

    public IsABMVisible = false;
    public IsAESVisible = false;
    public IsATMSVisible_BOL = false;
    public IsATMSVisible_VOG = false;
    public IsAMANACVisible = false;


    public IsABMDisabled = false;
    public IsAESDisabled = false;
    public IsATMSDisabled = false;
    public IsAMANACDisabled = false;
    private ShipmentCustomsTransmissionList: ShipmentCustomsTransmissionPM[] = [];

    public LocalCustomsTransmissionsStatusName: string;
    public LocalCustomsTransmissionsStatusDate: Date;
    public LocalCustomsTransmissionsByUserName: string;
    public LocalCustomsTransmissionsStatusCode: string;
    public LocalCustomsTransmissionsError: string;

    public ArtemusVoyageStatus: string;
    public ArtemusVoyageLastSendDate: Date;
    public ArtemusVoyageByUserName: string;
    public ArtemusVoyageStatusCode: string;
    public ArtemusVoyageError: string;

    public ArtemusBOLStatus: string;
    public ArtemusBOLLastSendDate: Date;
    public ArtemusBOLByUserName: string;
    public ArtemusBOLStatusCode: string;
    public ArtemusBOLError: string;

    public CBPStatus: string;
    public CBPLastSendDate: Date;
    public CBPByUserName: string;
    public CBPStatusCode: string;
    public CBPError: string;

    public IsVisible = false;
    private notSent = "Not Sent";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.Initialize();
    }
    SetWindowArgs(windowArgs: ShipmentPM) {
        this.EntityPM = windowArgs;
        this.FillData();
    }
    FillData() {
        this.LoadShipmentData();
        this.LoadDataList();
        this.LoadCustomsInterfaceListMethod();
    }

    private myArtemusWebService: ArtemusWebService;
    private myShipmentDomainService: ShipmentDomainService;
    private myABMWebService: ABMWebService;
    private myCommonDomainService: CommonDomainService;
    private myShipmentPMService: ShipmentPMService;

    Initialize() {
        this.myArtemusWebService = new ArtemusWebService();
        this.myShipmentDomainService = new ShipmentDomainService();
        this.myABMWebService = new ABMWebService();
        this.myCommonDomainService = new CommonDomainService();
        this.myShipmentPMService = new ShipmentPMService();
    }

    CustomsInterfaceList: CustomsInterfaceSettingList[] = [];
    LoadCustomsInterfaceListMethod() {
        this.myCommonDomainService.GetCustomsInterfaceListByTenant().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.CustomsInterfaceList = response.Result;
            }
            this.IsVisible = true;
            this.CheckVisibility();
            this.CheckIfSendButtonsEnabled();
        });
    }
    LoadShipmentData() {
        this.LocalCustomsTransmissionsStatusName = !AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsTransmissionsStatusName) ? this.EntityPM.LocalCustomsTransmissionsStatusName : this.notSent;
        this.LocalCustomsTransmissionsStatusDate = this.EntityPM.LocalCustomsTransmissionsStatusDate;
        this.LocalCustomsTransmissionsStatusCode = this.EntityPM.LocalCustomsTransmissionsStatusCode;
        this.LocalCustomsTransmissionsByUserName = this.EntityPM.LocalCustomsSentByUserName;
        this.LocalCustomsTransmissionsError = this.EntityPM.LocalCustomsTransmissionsStatusError;
    }
    LoadDataList() {
        this.myShipmentDomainService.GetShipmentCustomsTransmissionByShipmnetId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ShipmentCustomsTransmissionList = myResponse.Result;
                this.ShipmentCustomsTransmissionList.forEach(item => {
                    if (item.MessageCode == 'ARBL') {
                        this.ArtemusBOLStatus = !AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : this.notSent;
                        this.ArtemusBOLLastSendDate = item.LastSendDate;
                        this.ArtemusBOLByUserName = item.ByUserName;
                        this.ArtemusBOLStatusCode = item.Status;
                        this.ArtemusBOLError = item.Error;
                    }
                    if (item.MessageCode == 'ASVO') {
                        this.ArtemusVoyageStatus = !AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : this.notSent;
                        this.ArtemusVoyageLastSendDate = item.LastSendDate;
                        this.ArtemusVoyageByUserName = item.ByUserName;
                        this.ArtemusVoyageStatusCode = item.Status;
                        this.ArtemusVoyageError = item.Error;
                    }
                    if (item.MessageCode == 'CBAS') {
                        this.CBPStatus = !AppTool.IsNullOrEmpty(item.StatusName) ? item.StatusName : this.notSent;
                        this.CBPLastSendDate = item.LastSendDate;
                        this.CBPByUserName = item.ByUserName;
                        this.CBPStatusCode = item.Status;
                        this.CBPError = item.Error;
                    }
                });

                if (this.ShipmentCustomsTransmissionList.length == 0) {
                    this.ArtemusBOLStatus = this.notSent;
                    this.ArtemusVoyageStatus = this.notSent;
                    this.CBPStatus = this.notSent;
                }

                this.FireEvent();
            }
        });
    }

    private CheckIfSendButtonsEnabled() {
        this.CheckAMANACSendButton();
    }
    private CheckAMANACSendButton() {
        this.IsAMANACDisabled = false;
        if (this.LocalCustomsTransmissionsStatusCode  == "NSEN") {
            this.IsAMANACDisabled = true;
        }
    }

    CheckVisibility() {
        if ((ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "NO")
            &&
            (ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == null || ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "NO")
        ) {
        }
        else {
            this.CheckArtemusVisibility_BOL();
            this.CheckArtemusVisibility_VOG();
            this.CheckABMVisibility();
            this.CheckAESVisibility();
            this.CheckAMANACVisibility();
        }
    }
    CheckABMVisibility() {
        if (this.EntityPM.ShipmentLevelCode != "C") {
            if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "ABM") {
                    this.IsABMVisible = true;
                }
            }
        }
        else {
            this.IsABMVisible = false;
        }
    }
    CheckArtemusVisibility_BOL() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {

            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    this.IsATMSVisible_BOL = true;
                }
            }
            else {
                this.IsATMSVisible_BOL = false;
            }

        }
    }
    CheckArtemusVisibility_VOG() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SendToArtemus")) {

            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "C")) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ImportToUSAInterfaceCode == "ART") {
                    this.IsATMSVisible_VOG = true;
                }
            }
            else {
                this.IsATMSVisible_VOG = false;
            }

        }
    }
    CheckAESVisibility() {
        if (FeatureLocator.HasFeaturePermession("Shipment", "SENDTOAES")) {
            if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                if (ObjectsLocator.CustomsInterfaceSettingPM.ExportFromUSAInterfaceCode == "CBP") {
                    if (this.EntityPM.DirectionId == "E") {
                        this.IsAESVisible = true;
                    }
                }
            }
        }
    }
    CheckAMANACVisibility() {
        if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
            if (ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "AMC") {
                this.IsAMANACVisible = true;
            }
        }
        else {
            this.IsAMANACVisible = false;
        }
    }

    CloseButtonClicked() {        
        this.CurrentSession.CloseCurrentWindow();
    }
    ViewCustomsSettings() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Customs Settings";
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
        logWindow.WindowClosed.subscribe(comp => {
            this.FillData();
        });
    }
    SendButtonClicked(arg: string) {
        switch (arg) {
            case "ABM":
                {
                    this.SendToCustoms();
                    break;
                }
            case "ASV":
                {
                    this.SendToArtemus_Voyage();
                    break;
                }
            case "BOL":
                {
                    if (this.ArtemusVoyageStatusCode != "SENT" && this.ArtemusVoyageStatusCode != "ACPT" && this.EntityPM.ShipmentLevelCode == "D") {
                        var messageWindow = new MessageWindow();
                        messageWindow.Show("Can't send Bill of Lading message before sending the voyage message");
                    }
                    else {
                        this.SendToArtemus_Bill();
                    }
                    break;
                }
            case "CBP":
                {
                    this.SendToAES();
                }
            case "AMC": {
                this.SendToAMANAC();
            }
        }
    }
    CheckInterfaceByCode(code: string) {
        return this.CustomsInterfaceList.filter(a => a.LocalCustomsInterfaceCode == code)[0];
    }

    private SendToCustoms() {
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending in Progress..");

            this.myABMWebService.Send(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse == null) {
                    this.CurrentSession.StopBusyIndicator();
                }
                else if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var myResult: ABMResult = myResponse.Result;

                    if (myResult == null) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.IsMessageValid = false;
                        this.CurrentSession.StopBusyIndicator();
                    }
                    else {
                        this.ReloadEntity();                        
                    }
                }
            });
        }
    }
    private SendToArtemus_Voyage() {
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.myArtemusWebService.SendAMS_Voyage(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray; this.MessageText = "Checking Required Fields in Shipment...";
                this.IsMessageValid = false;

            }
            else {
                this.MessageText = "Voyage message has been sent successfully";
                this.IsMessageValid = true;
                this.LoadDataList();
            }
        });
    }
    private SendToArtemus_Bill() {
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.myArtemusWebService.SendAMS_Bill(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray; this.MessageText = "Checking Required Fields in Shipment...";
                this.IsMessageValid = false;
            }
            else {
                this.MessageText = "BOL message has been sent successfully";
                this.IsMessageValid = true;
                this.LoadDataList();
            }
        });
    }
    private SendToAES() {
        this.ValidationErrorsList = [];
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Exporting AES File";
        logWindow.Width = 500;
        logWindow.Height = 200;
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/Customs/ExportFileComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            comp.Export(this.EntityPM.Id);
        });
        logWindow.WindowClosed.subscribe(comp => {
            this.LoadDataList();
        });
    }
    public ReloadEntity() {

        if (this.myShipmentPMService == null) {
            this.myShipmentPMService = new ShipmentPMService();
        }

        this.myShipmentPMService.get(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result;
                    this.CurrentSession.CurrentEditComponent.EntityPM = myResponse.Result;
                    this.MessageText = "The message has been sent successfully";
                    this.IsMessageValid = true;
                    this.LoadCompleted.emit(true);
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.LoadCompleted.emit(false);
                }

                this.CurrentSession.StopBusyIndicator();
                this.LoadShipmentData();
                this.CurrentSession.FireEvent("CustomsWizardClosed");
            }
        });
    }
    private SendToAMANAC() {

    }

    SetCellNotesWidth(text: string) {
        var myColumnWidth: number = 0;
        var widthOfLabel = 0;
        if (!AppTool.IsNullOrEmpty(text)) {
            var widthOfLabel = AppTool.GetTextWidth(text) + 10;
        }
        return widthOfLabel;
    }

    private FireEvent() {
        this.CurrentSession.FireEvent("RefreshCustomsSummary");
    }
}
