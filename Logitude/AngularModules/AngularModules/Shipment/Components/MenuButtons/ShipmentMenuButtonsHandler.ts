declare var window: any;
import {OnDestroy} from '@angular/core';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {NewShipmentComponentArgs} from '../../Args';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {EntityWarningsValidator} from '../../../Infrastructure/Validators/EntityWarningsValidator';
import {RulesValidator} from '../../../Infrastructure/Validators/RulesValidator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import { MenuButtonsTemplateArgs} from './MenuButtonsTemplateComponent';
import {ShipmentTool, RoutingHelper} from '../../Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentValidator} from '../../Validators/ShipmentValidator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTableRuleFieldPM} from '../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ConvertDirectionArgs } from './ShipmenDirectionConvertComponent';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList';
import { ShipmentContainersWebService } from 'Shipment/Services/ShipmentContainersWebService';
import { $ } from 'protractor';
import { GeneralContainerTrackingArgs } from 'Shipment/DataContract/GeneralContainerTrackingArgs';
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';
import { ReportGroupList } from 'Report/EntityLists/ReportGroupList';
import { ReportsTemplateListExtendedService } from 'Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ReportList } from 'Report/EntityLists/ReportList';
import { QueryFilterItem } from 'Report/Components/Filters/QueryFilterItem';

export class ShipmentMenuButtonsHandler implements OnDestroy {
    public EntityPM: ShipmentPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    public ReportsPreview: ReportsPreviewComponent;
    private reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    private reportService: ReportService;

    constructor() {
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
        this.reportService = new ReportService();
    }

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Shipment')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {

                    var button = menuButtons[i];

                    if (button.EventCode == "ShowAWB") {
                        button.IsDisabled = buttonEnabled ? (this.EntityPM.TransportModeId != "A") : true;
                    }

                    if (button.EventCode == "ShipmentForm" || button.EventCode == "Forms") {
                        button.IsHidden = !this.EntityPM.IsCustomShipment;
                        // button.IsHidden = (this.entityArgs.EditComponent["SelectedQueryCode"] != "CustomsShipments");
                        // button.IsDisabled = true;
                    }

                    if (button.EventCode == "CopyShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "C") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                            if (this.IsStandAloneFeatureShipment()) {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "OperationalCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "AccountingCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || !this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "OperationalReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsOperationalClosed || this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "AccountedReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "CancelShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || (this.EntityPM.MasterShipmentDataId != null && this.EntityPM.MasterShipmentDataId != this.EntityPM.Id)) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                            if (this.IsStandAloneFeatureShipment() && this.SetIsStandaloneWithPickupDeliveryOnlyVisible()) {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ReactivateShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsCancelled || (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId != null)) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ExceptionResolved") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.HasException) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentFromHouseToDirect") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId == null) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentFromDirectToHouse") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "D" && !this.EntityPM.IsOperationalClosed) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                            if (this.IsStandAloneFeatureShipment()) {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }

                    }
                    if (button.EventCode == "SendRequest") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "A" && this.EntityPM.DirectionId == "E") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SendResponse") {
                        if (buttonEnabled) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertToCustomFile") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode != "D") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SplitShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                else {
                                    button.IsHidden = true;
                                }

                                if (this.IsStandAloneFeatureShipment() || this.IsForwarderShipmentConnectedWithStandAlone()) {
                                    button.IsDisabled = true;
                                }
                            }                            
                        }
                    }

                    if (button.EventCode == "ConvertShipmentToLTL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "I" && this.EntityPM.ShipmentTypeId == "FTL") {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
     
                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                if (this.IsStandAloneFeatureShipment()) {
                                    button.IsDisabled = true;
                                }
                            }       
                            else {
                                button.IsHidden = true;
                            }                  
                        }

                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "ConvertShipmentToFTL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "I" && this.EntityPM.ShipmentTypeId == "LTL") {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                if (this.IsStandAloneFeatureShipment()) {
                                    button.IsDisabled = true;
                                }
                            }
                 
                            else {
                                button.IsHidden = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "ConvertShipmentToLCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.ShipmentTypeId == "FCLD") {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                if (this.IsStandAloneFeatureShipment()) {
                                    button.IsDisabled = true;
                                }
                            }
                  
                            else {
                                button.IsHidden = true;
                            }
                        }

                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "ConvertShipmentToFCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "O" && this.EntityPM.ShipmentTypeId == "LCLD") {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsHidden = false;
                                    button.IsDisabled = false;
                                }
                                if (this.IsStandAloneFeatureShipment()) {
                                    button.IsDisabled = true;
                                }
                            }

                            else {
                                button.IsHidden = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "ConvertShipmentDirection") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }

                            else {
                                button.IsHidden = false;
                                button.IsDisabled = false;
                            }
                            if (this.IsStandAloneFeatureShipment()) {
                                button.IsDisabled = true;
                            }

                            button.IsHidden = false;
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "SendToAMANAC") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsCancelled || this.EntityPM.IsOperationalClosed || this.EntityPM.IsAccountingClosed) {
                                button.IsDisabled = true;
                            }

                            else {
                                if (this.EntityPM.TransportModeId == 'I' || this.EntityPM.ShipmentLevelCode == "C" || (this.EntityPM.ShipmentLevelCode == "H" && AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId))) {
                                    button.IsHidden = true;
                                }
                            }                            
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "ViziionUnsubscribe") {
                        if (!buttonEnabled) button.IsHidden = true;
                        if(this.EntityPM.ShipmentTypeId != "FCLD") button.IsHidden = true;
                        if(!SessionLocator.TenantManagementJS.IsContainerTrackingPrepaid) button.IsHidden = true;
                    }


                    if (button.EventCode == "SendCartaPorte") {
                        button.IsHidden = SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE" || !SessionLocator.SATInterfaceSettings.IsCartaPorteTransferEnabled;

                    }
                }

                return menuButtons;
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {

        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            this.Validate();

            if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "ShowEvents": {
                        this.ShowEvents();
                        break;
                    }
                    case "CopyShipment": {
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                    case "OperationalCloseShipment": {
                        this.OperationalCloseShipment();
                        break;
                    }
                    case "OperationalReopenShipment": {
                        this.OperationalReopenShipment();
                        break;
                    }
                    case "AccountingCloseShipment": {
                        this.AccountingCloseShipment();
                        break;
                    }
                    case "AccountedReopenShipment": {
                        this.AccountedReopenShipment();
                        break;
                    }
                    case "CancelShipment": {
                        this.CancelShipment();
                        break;
                    }
                    case "ReactivateShipment": {
                        this.ReactivateShipment();
                        break;
                    }
                    case "ExceptionResolved": {
                        this.ExceptionResolvedShipment();
                        break;
                    }
                    case "ConvertShipmentFromHouseToDirect": {
                        this.ConvertShipmentFromHouseToDirect();
                        break;
                    }
                    case "ConvertShipmentFromDirectToHouse": {
                        this.ConvertShipmentFromDirectToHouse();
                        break;
                    }
                    case "SendRequest": {
                        this.StopFlags();
                        break;
                    }
                    case "SendResponse": {
                        this.StopFlags();
                        break;
                    }
                    case "ConvertToCustomFile": {
                        this.ConvertShipmentToCustomFile();
                        break;
                    }
                    case "SplitShipment": {
                        this.SplitShipmentClicked();
                        break;
                    }

                    case "ConvertShipmentToLTL": {
                        this.ConvertShipmentToLTLClicked();
                        break;
                    }

                    case "ConvertShipmentToFTL": {
                        this.ConvertShipmentToFTLClicked();
                        break;
                    }

                    case "ConvertShipmentToLCL": {
                        this.ConvertShipmentToLCLClicked();
                        break;
                    }

                    case "ConvertShipmentToFCL": {
                        this.ConvertShipmentToFCLClicked();
                        break;
                    }

                    case "ConvertShipmentDirection": {
                        this.ConvertShipmentDirectionClicked();
                        break;
                    }

                    case "SendToAMANAC":
                        {
                            this.SendToAMANACClicked();
                            break;
                        }

                    case "ViziionUnsubscribe":
                        {
                            this.ViziionUnsubscribe();
                            break;
                        }

                    case "ShipmentForm": {
                        this.ShipmentFormClicked();
                        break;
                    }
    
                    default: {
                        this.isButtonClicked = false;
                        break;
                    }
                }
            }
        }
    }

    ShipmentFormClicked() {
        this.ResetButtonClicked();

        this.reportService.GetReportByCode("SHTO").subscribe((myResponse: ServiceResponse) => {
            this.LoadReportTemplate(myResponse.Result);
        });
    }

    ReportTemplates: any[] = [];
    LoadReportTemplate(reportList: ReportList) {
     
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id,"R").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;
            }

            SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                this.ReportsPreview = cmpRef.instance;
                this.ReportsPreview.Report = reportList;

                this.BuildReport();

                this.ReportsPreview.OnDone.subscribe(response => {
                    
                    // close report page
                    cmpRef.instance.ComponentRef.destroy();

                    if (!response.HasError) {
                        // download report
                        const templateDescription = this.ReportTemplates.filter(d => d.Id == this.ReportsPreview.Report.DefaultTemplateId)[0].Description;
                        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadReportPage.aspx?fileName=" + response.Result.ReportKey + "@" + templateDescription + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&type=PrintToPDF";
                        window.open(url);
                    }
                });
            });
        });
    }

    GetNewQueryFilterItem(FieldName: string, FieldValue: any, FieldValue2: any = null, FieldDataType: string = null, Operator: string = "Equals") {
        var queryFilterItem = new QueryFilterItem();
        queryFilterItem.DisplayInList = false;
        queryFilterItem.FieldName = FieldName;
        queryFilterItem.FieldValue = FieldValue;
        queryFilterItem.FieldValue2 = FieldValue2;
        queryFilterItem.Operator = Operator;
        queryFilterItem.FieldDataType = FieldDataType;

        return queryFilterItem;
    }

    BuildReport() {

        let queryFilterItems = new Array<QueryFilterItem>();
        queryFilterItems.push(this.GetNewQueryFilterItem("Id", this.EntityPM.Id, null, "string"));

        let reportFliter = new ReportFliter();
        reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        reportFliter.QueryFilterItemLists = queryFilterItems;
        reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        reportFliter.NumberOfPage = 1;
        reportFliter.DisablePreview = true;
        reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(reportFliter, true);
    }

    ViziionUnsubscribe() {
        var shipmentContainersWebService = new ShipmentContainersWebService();
        this.CurrentSession.StartBusyIndicator("Unsubscribe...");
        var args: GeneralContainerTrackingArgs = <GeneralContainerTrackingArgs>  {
            ContainerId:null,
            ShipmentId:this.EntityPM.Id,
            IsFromContainer:false,
            IsSimulator:false,
            SourceCode:'VZN'
        }
        shipmentContainersWebService.ViziionUnsubscribe(args).subscribe(e=>{
            this.CurrentSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            if(e.HasError){
                messageWindow.Show(e.ErrorsArray.join(', '));
            }else{
                var messageWindow = new MessageWindow();
                messageWindow.Show(e.Result.message);
            }
        })
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        switch (this.MenuButtonCode) {
                            case "CopyShipment": {
                                this.CopyShipment();
                                break;
                            }

                            case "SplitShipment": {
                                this.SplitShipmentApply();
                                break;
                            }
                        }

                        if (this.IsConvertToLCLClicked) {
                            this.DoConvertShipmentType("ToLCL");
                        }

                        if (this.IsConvertToFCLClicked) {
                            this.DoConvertShipmentType("ToFCL");
                        }

                        if (this.IsConvertToLTLClicked) {
                            this.DoConvertShipmentType("ToLTL");
                        }

                        if (this.IsConvertToFTLClicked) {
                            this.DoConvertShipmentType("ToFTL");
                        }

                        if (this.IsConvertDirectionClicked) {
                            this.DoConvertShipmentDirection();
                        }

                        if (this.IsSendToAMANACClicked) {
                            this.DoSendToAMANA();
                        }

                        if (this.Reload) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }

                        if (this.isConvertFromHouseToDirect) {
                            this.StartConvertingShipmentFromHouseToDirect();
                        }

                        if (this.isConvertFromDirectToHouse) {
                            this.StartConvertingShipmentFromDirectToHouse();
                        }
                    }

                    this.StopFlags();
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        this.CurrentSession.SessionEvent.emit("ReloadHouses");
                    }

                    this.StopFlags();
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    Reload: boolean = false;
    StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.Reload = false;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
        this.IsConvertToLTLClicked = false;
        this.IsConvertToFTLClicked = false;
        this.IsConvertDirectionClicked = false;
        this.IsSendToAMANACClicked = false;
        this.isConvertFromHouseToDirect = false;
        this.isConvertFromDirectToHouse = false;
    }
    Validate() {
        var validator = new ShipmentValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }

    public ActionStepsStateList: Array<ActionsStepsState> = [];
    private shipmentService: ShipmentDomainService = new ShipmentDomainService();
    private warningsList: Array<ValidationErrorInfo>;
    public get WarningList() { if (this.warningsList == null) this.warningsList = new Array<ValidationErrorInfo>(); return this.warningsList; }
    public set WarningList(value: Array<ValidationErrorInfo>) { this.warningsList = value; }
    private hasWarnings: boolean = false;
    public get HasWarnings() { return this.hasWarnings; }
    public set HasWarnings(value: boolean) { this.hasWarnings = value; }

    private CopyShipment() {
        var args = new NewShipmentComponentArgs();
        args.IsCopyFromShipment = true;
        args.Shipment = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            logWindow.Title = "Copy Master";
            logWindow.Show('./Shipment/Components/NewEntity/NewMasterComponent');
        }

        else {
            logWindow.Title = "Copy Shipment";
            logWindow.Show('./Shipment/Components/NewEntity/NewShipmentComponent');
        }

        logWindow.ComponentLoaded.subscribe(cmp => {
            this.StopFlags();

            cmp.MainCarriageFromPortId = this.EntityPM.MainCarriageFromPortId;
            cmp.MainCarriageToPortId = this.EntityPM.MainCarriageToPortId;
        });
    }
    private AccountedReopenShipment() {
        this.currentActionName = "AccountedReopen";        
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounted Shipment Reopen";
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
            });
        });

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsAccountingClosed = false;
                this.OkButton();
            }
        });
    }
    private AccountingCloseShipment() {
        this.currentActionName = "AccountingClose";
        var hasOpenPayables: boolean = false;
        var hasOpenReceivables: boolean = false;

        if (SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "TSV")[0] == null) {
            if (this.EntityPM.ShipmentReceivables.length > 0) {
                this.EntityPM.ShipmentReceivables.forEach(p => {
                    if (p.ShipmentReceivableLineStatusCode != "ACCT" && p.ShipmentReceivableLineStatusCode != "EMPT")
                        if (p.TotalAmount != null && p.TotalAmount != 0) hasOpenReceivables = true;
                });
            }

            if (!SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables) {
                if (this.EntityPM.ShipmentPayables.length > 0)
                    this.EntityPM.ShipmentPayables.forEach(p => {
                        if (p.ShipmentPayableLineStatusCode != "ACCT" && p.ShipmentPayableLineStatusCode != "EMPT")
                            if (p.ShipmentPayableAmountTypeCode == "NEXP") {
                                p.AccountedAmount != null && p.AccountedAmount != null ? hasOpenPayables = true : p.ExpectedAmount != null && p.ExpectedAmount != 0 ? hasOpenPayables = true : -1;

                            }
                            else {
                                if (p.ExpectedAmount != null && p.ExpectedAmount != 0) {
                                    hasOpenPayables = true;
                                }
                            }
                    });
            }

            if (this.EntityPM.ShipmentLevelCode == "C") {
                if (hasOpenPayables && hasOpenReceivables) {
                    this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);

                }
                else {
                    this.shipmentService.CheckHousesOpenAmounts(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse != null) {
                            if (myResponse.Result != null && myResponse.Result != "") {
                                var Result: string = myResponse.Result;

                                if (Result.includes('R'))
                                    hasOpenReceivables = true;

                                if (!SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables && Result.includes('P'))
                                    hasOpenPayables = true;                                

                                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
                            }

                            else
                                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
                        }

                        else
                            this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
                    });                    
                }
            }
            else {
                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
            }
        }

        else {
            this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
        }
    }
    private RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables) {
        this.currentActionName = "OperationalReopen";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = false;
        args.EventNote = null;
        var ErrorsList: Array<string> = [];
        var success: boolean = true;
        if (hasOpenPayables || hasOpenReceivables) {
            success = false;
            var error: string = "can’t close for accounting if there are any open payables/receivables";
            if (this.EntityPM.ShipmentLevelCode == "C") {
                error = "can’t close for accounting if there are any open payables/receivables in the Master or one \nof the connected shipments. Please check and fix this issue and try again";
            }

            ErrorsList.push(error);
        }

        else if (!this.EntityPM.IsOperationalClosed) {
            success = false;
        }
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        success == true ? state.State = "Succeeded" : state.State = "Failed";
        this.ActionStepsStateList.push(state);

        var hasInvoice = null;
        hasInvoice = this.EntityPM.ShipmentARInvoices.filter(p => p.StatusCode != 'AC')[0];
        if (!hasInvoice) {
            state = new ActionsStepsState();
            state.Message = TextCodeTranslator.Translate("Shipment.M.DoesntContainInvoice");
            state.State = "Warning";
            this.ActionStepsStateList.push(state);

        }
        if (!success) args.EnabledOkButton = false;
        args.ValidationErrorsList = ErrorsList;
        args.ActionStepsStateList = this.ActionStepsStateList;
        args.IsNotesStackPanelVisible = true;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Shipment Accounting Close";
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
            });
        });
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsAccountingClosed = true;                
                this.EntityPM.AccountingCloseDate = DateTool.GetCurrentDateTimeAsUtc();
                this.OkButton();
            }
        });
    }
    private OperationalReopenShipment() {
        this.currentActionName = "OperationalReopen";
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;        
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = "Shipment Operational Reopen";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
                this.EntityPM.IsOperationalClosed = false;
                this.EntityPM.OperationalCloseDate = null;
                this.OkButton();
            });
        });

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();           
        });
    }
    private ShowEvents() {


    }
    private currentActionName: string = null;
    private ExceptionResolvedShipment() {
        this.currentActionName = "ExceptionResolved";

        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Exception Resolved Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var state2: ActionsStepsState = new ActionsStepsState();
        state2.Message = TextCodeTranslator.Translate("Shipment.M.ExceptionResolved");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Exception Resolved";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsExceptionResolved = true;
                    this.EntityPM.HasException = false;
                    this.OkButton();
                }

                this.ResetButtonClicked();
            });
        });
    }

    private isConvertFromHouseToDirect: boolean = false;
    private isConvertFromDirectToHouse: boolean = false;
    private ConvertShipmentFromHouseToDirect() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.isConvertFromHouseToDirect = true;
            this.OkButton();
        }
    }
    private ConvertShipmentFromDirectToHouse() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.isConvertFromDirectToHouse = true;
            this.OkButton();
        }
    }
    private StartConvertingShipmentFromHouseToDirect() {
        this.currentActionName = "ConvertShipmentFromHouseToDirect";

        this.ActionStepsStateList = this.CreateConvertingFromHouseToDirectActionSteps();
        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.IsNotesStackPanelVisible = true;
        args.ActionStepsStateList = this.ActionStepsStateList;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = "Convert Shipment From House To Direct";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                this.EntityPM.EventNote = s.EventNotes;
                if (d == "confirm") {
                    this.EntityPM.ConvertFromHouseToDirect = true;
                    this.EntityPM.ConvertFromDirectToHouse = false;
                    RoutingHelper.RemovePreForwardingLeg(this.EntityPM);
                    RoutingHelper.RemoveOnForwardingLeg(this.EntityPM);
                    this.OkButton();
                }
                this.ResetButtonClicked();
            });
        });
    }    
    private StartConvertingShipmentFromDirectToHouse() {
        if (ShipmentTool.IsInlandDomestic(this.EntityPM)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Converting Shipment";
            messageWindow.Show("Converting inland domestic direct to house is not allowed");
        }

        else {
            this.currentActionName = "ConvertShipmentFromDirectToHouse";
            this.ActionStepsStateList = this.CreateConvertingFromDirectToHouseActionSteps();

            var args = new MenuButtonsTemplateArgs();
            args.ObjectTableName = "Shipment";
            args.EntityPM = this.EntityPM;
            args.IsNotesStackPanelVisible = true;
            args.ActionStepsStateList = this.ActionStepsStateList;

            var logWindow = new LogitudeWindow();
            if (this.EntityPM.MainCarriageIsFromStack) {                
                args.EnabledOkButton = false;
                args.ValidationErrorsList.push(TextCodeTranslator.Translate("Shipment.M.MasterAWBNumberTakenFromStack"));
                logWindow.Width = 700;
                logWindow.Height = 400;
            }

            else {
                logWindow.Width = 450;
                logWindow.Height = 300;
            }
            
            logWindow.WindowArgs = args;
            logWindow.Title = "Convert Shipment From Direct To House";
            logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    this.EntityPM.EventNote = s.EventNotes;
                    if (d == "confirm") {
                        if (!this.EntityPM.MainCarriageIsFromStack) {
                            this.EntityPM.ConvertFromDirectToHouse = true;
                            this.EntityPM.ConvertFromHouseToDirect = false;
                            RoutingHelper.RemovePreCarriageLeg(this.EntityPM);
                            RoutingHelper.RemoveOnCarriageLeg(this.EntityPM);
                        }

                        this.OkButton();
                    }

                    this.ResetButtonClicked();
                });
            });
        }
    }
    private CreateConvertingFromHouseToDirectActionSteps(): ActionsStepsState[] {
        var actionStepsStateList = new Array<ActionsStepsState>();
        var state = new ActionsStepsState();

        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            var settingCode = "HAWBCounter" + this.EntityPM.TransportModeId + "_E_D";
            var tenantSettingPM = SessionLocator.TenantSettings.filter(p => p.SettingCode == settingCode)[0];
            if (tenantSettingPM != null) {
                if (tenantSettingPM.DontIncludeDirects) {
                    state = new ActionsStepsState();
                    state.Message = "The HAWB field will be removed";
                    state.State = "Warning";
                    actionStepsStateList.push(state);
                }
            }
        }

        if (this.EntityPM.HasPreForwarding) {
            state = new ActionsStepsState();
            state.Message = "Pre Forwarding data will be removed";
            state.State = "Warning";
            actionStepsStateList.push(state);
        }

        if (this.EntityPM.HasOnForwarding) {
            state = new ActionsStepsState();
            state.Message = "On Forwarding data will be removed";
            state.State = "Warning";
            actionStepsStateList.push(state);
        }

        return actionStepsStateList;
    }
    private CreateConvertingFromDirectToHouseActionSteps(): ActionsStepsState[] {
        var actionStepsStateList = new Array<ActionsStepsState>();
        var state = new ActionsStepsState();

        if (this.EntityPM.MainCarriageIsFromStack) {
            state = new ActionsStepsState();
            state.Message = TextCodeTranslator.Translate("Shipment.M.MasterAWBNumberTakenFromStack");
            state.State = "Error";
            actionStepsStateList.push(state);
        }

        if (this.EntityPM.HasPreCarriage) {
            state = new ActionsStepsState();
            state.Message = "Pre Carriage data will be removed";
            state.State = "Warning";
            actionStepsStateList.push(state);
        }

        if (this.EntityPM.HasOnCarriage) {
            state = new ActionsStepsState();
            state.Message = "On Carriage data will be removed";
            state.State = "Warning";
            actionStepsStateList.push(state);
        }

        return actionStepsStateList;
    }

    private ConvertShipmentToCustomFile() {
        this.currentActionName = "ConvertToCustomFile";
        this.EntityPM.ShipmentLevelCode = "A";
        this.EntityPM.DirectionId = "C";
        this.EntityPM.ConvertToCustomFile = true;
        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;

    }
    private ReactivateShipment() {
        this.currentActionName = "ReactivateShipment";

        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";       
        
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = "Reactivate Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsCancelled = false;
                    this.OkButton();
                }

                this.ResetButtonClicked();
            });
        });
    }

    private CancelShipment() {
        this.currentActionName = "CancelShipment";

        if (this.EntityPM.ShipmentReceivables.filter(p => p.ARInvoiceId != null)[0]) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Cancelling Shipment";
            messageWindow.Show("This shipment can't be canceled because it has one or more invoices. all invoices must be disconnect to cancel this shipment");
        }

        else if (this.EntityPM.TransportModeId == "A" && this.EntityPM.DirectionId == "E" && !AppTool.IsNullOrEmpty(this.EntityPM.Master)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Cancelling Shipment";
            messageWindow.Show("Can't cancel shipments that have a MAWB number, please remove it");
        }

        else if (this.EntityPM.BookingId != null && this.EntityPM.BookingId != "") {
            var confirmWindow: ConfirmWindow = new ConfirmWindow();
            confirmWindow.Title = "Cancel Shipment";
            confirmWindow.Width = 400;
            confirmWindow.Show("Cancelling this shipment will disconnect it from the Booking , are you sure you want to cancel ?");
            confirmWindow.YesButtonText = "Yes";
            confirmWindow.NoButtonText = "No";

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.ConfirmCanceling();
                }

                else if (confirmWindow.No) {

                }

                this.ResetButtonClicked();
            });
        }

        else {
            this.ConfirmCanceling();
        }

    }
    private ResetButtonClicked() {
        this.isButtonClicked = false;
    }
    private ConfirmCanceling() {
        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = "Cancel Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsCancelled = true;

                    if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
                        this.EntityPM.MAWBReturnedToStack = true;
                        this.EntityPM.MAWBReturnedToStackWithCancel = true;
                        this.EntityPM.MAWBStackNumber = this.EntityPM.Master;
                    }

                    this.OkButton();

                }
                this.ResetButtonClicked();
            });
        });
    }
    private OkButton() {
        if (this.currentActionName == "OperationalClose" || this.currentActionName == "AccountingClose") {
            ServiceLocator.SendTotangoUserActivity("Shipment", this.currentActionName);
        }

        else if (this.currentActionName == "ExceptionResolved") {
            this.EntityPM.ExceptionResolvedDescription = this.EntityPM.EventNote;
        }


        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;

        this.isButtonClicked = false;
    }
    private OperationalCloseShipment() {
        this.currentActionName = "OperationalClose";
        var WarningsList: Array<string> = [];
        var ErrorsList: Array<string> = [];

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.myCloner = new Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            var args = this;
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Master Operational Close";
            logWindow.WindowArgs = args;
            logWindow.IsFillScreen = true;
            logWindow.Show('./Shipment/Components/MenuButtons/MasterActionConfirmationComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.ResetButtonClicked();
                if ($event == "confirm") {
                    this.EntityPM.OperationalCloseDate = DateTool.GetCurrentDateTimeAsUtc();
                    this.EntityPM.EventNote = null;
                    this.OkButton();
                }
                else {
                    this.RejectChanges();
                }
            });
        }

        else {
            this.myCloner = new Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            this.ActionStepsStateList = new Array<ActionsStepsState>();
            this.EntityPM.IsOperationalClosed = true;
            this.ValidateShipmentRules(WarningsList, ErrorsList, this.EntityPM);
            var tableId = window.ObjectTables.filter(t => t.Name == "Shipment")[0].Id;

            if (SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "TSV")[0] == null) {
                ServiceLocator.RulesValidator.ValidateAllRequiredFieldRules(this.EntityPM, tableId, ErrorsList);
            }

            this.DisplayErrorsWindow(WarningsList, ErrorsList);
        }
    }
    private SplitShipmentClicked() {
        this.Validate();

        if (this.isValid) {
            if (this.EntityPM.IsOperationalClosed) {
                this.StopFlags();
                var messageWindow = new MessageWindow();
                messageWindow.Show("Can't split an operationally closed shipment");                
            }

            else {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }
    private SplitShipmentApply() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: this.EntityPM }
        logWindow.IsFillScreen = true;
        logWindow.Title = "Split Shipment";
        logWindow.Show('./Shipment/Components/SplitShipment/SplitShipmentComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            this.StopFlags();

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    }

    private IsConvertToLTLClicked: boolean = false;
    private IsConvertToFTLClicked: boolean = false;
    private ConvertShipmentToLTLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToLTLClicked = true;
            this.OkButton();
        }
    }
    private ConvertShipmentToFTLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToFTLClicked = true;
            this.OkButton();
        }
    }

    private IsConvertToLCLClicked: boolean = false;
    private IsConvertToFCLClicked: boolean = false;
    private ConvertShipmentToLCLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToLCLClicked = true;
            this.OkButton();
        }
    }
    private ConvertShipmentToFCLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToFCLClicked = true;
            this.OkButton();
        }
    }

    private DoConvertShipmentType(type: string) {
        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            errors.push("Cannot change shipment type when connected to a quote");
        }

        else if (this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.DeliveryId)).length > 0
            || this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.EmptyContainerReturnId)).length > 0) {
            errors.push("Cannot change shipment type when shipment packages are \nconnected to a delivery or empty container return");
        }

        else if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            errors.push("Cannot change shipment type when connected to a Master shipment");
        }

        else if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            errors.push("Cannot change shipment type when connected to house shipments ");
        }

        else if (this.EntityPM.IsOperationalClosed) {
            errors.push("Cannot change shipment type when shipment is operationally closed");
        }

        else if (this.EntityPM.MainCarriageATD != null || this.EntityPM.Transshipment1ATD != null || this.EntityPM.Transshipment2ATD != null
            || this.EntityPM.Transshipment3ATD != null || this.EntityPM.MainCarriageATA != null || this.EntityPM.Transshipment1ATA != null
            || this.EntityPM.Transshipment2ATA != null || this.EntityPM.Transshipment3ATA != null) {
            errors.push("Cannot change shipment type when shipment contains \nactual departure/arrival dates");
        }

        if (errors.length == 0) {
            this.shipmentService.CheckIfConnectedEntryOrRelease(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var result: boolean = myResponse.Result;

                    if (result) {
                        errors.push("Cannot change shipment type when shipment is connected \nto Cross Docks Entries / Releases");
                    }

                    this.ShowNotesWindow(errors, type);
                }
            });
        }

        else {
            this.ShowNotesWindow(errors, type);
        }
    }
    private ShowNotesWindow(errors: string[], type: string) {
        var args = new MenuButtonsTemplateArgs();
        var windowTitle: string;

        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.EventNote = null;
        args.IsConvertShipmentType = true;
        args.ValidationErrorsList = errors;
        args.EnabledOkButton = false;
        args.IsNotesStackPanelVisible = true;
        args.NotesHeader = "Notes";

        if (errors.length == 0) {
            args.EnabledOkButton = true;
        }

        switch (type) {
            case "ToLCL":
                {
                    this.currentActionName = "ConvertShipmentToLCL";
                    windowTitle = "Convert Shipment From FCL To LCL";
                    break;
                }

            case "ToFCL":
                {
                    this.currentActionName = "ConvertShipmentToFCL";
                    windowTitle = "Convert Shipment From LCL To FCL";
                    break;
                }

            case "ToLTL":
                {
                    this.currentActionName = "ConvertShipmentToLTL";
                    windowTitle = "Convert Shipment From FTL To LTL";
                    break;
                }

            case "ToFTL":
                {
                    this.currentActionName = "ConvertShipmentToFTL";
                    windowTitle = "Convert Shipment From LTL To FTL";
                    break;
                }
        }

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;

        logWindow.Title = windowTitle;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    this.EntityPM.EventNote = notes;

                    switch (type) {
                        case "ToLCL":
                            {
                                this.EntityPM.ConvertShipmentToLCL = true;
                                this.EntityPM.ConvertShipmentToFCL = false;
                                break;
                            }

                        case "ToFCL":
                            {
                                this.EntityPM.ConvertShipmentToLCL = false;
                                this.EntityPM.ConvertShipmentToFCL = true;
                                break;
                            }

                        case "ToLTL":
                            {
                                this.EntityPM.ConvertShipmentToLTL = true;
                                this.EntityPM.ConvertShipmentToFTL = false;
                                break;
                            }

                        case "ToFTL":
                            {
                                this.EntityPM.ConvertShipmentToLTL = false;
                                this.EntityPM.ConvertShipmentToFTL = true;
                                break;
                            }
                    }

                    this.Reload = true;
                    this.OkButton();
                    this.ResetButtonClicked();
                }
            });
        });
    }

    private IsConvertDirectionClicked: boolean = false;
    private ConvertShipmentDirectionClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertDirectionClicked = true;
            this.OkButton();
        }
    }
    private DoConvertShipmentDirection() {
        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            errors.push("Shipment is connected to a quote, can't change direction");
        }

        else if (this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.DeliveryId)).length > 0
            || this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.EmptyContainerReturnId)).length > 0) {
            errors.push("Shipment packages are connected to a delivery or empty container return, can't change direction");
        }

        else if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            errors.push("Shipment is connected to other shipment/s, can't change direction");
        }

        else if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            errors.push("Shipment is connected to other shipment/s, can't change direction");
        }

        else if (this.EntityPM.IsOperationalClosed) {
            errors.push("Shipment is closed operationally, can't change direction");
        }

        else if (this.EntityPM.MainCarriageATD != null || this.EntityPM.Transshipment1ATD != null || this.EntityPM.Transshipment2ATD != null
            || this.EntityPM.Transshipment3ATD != null || this.EntityPM.MainCarriageATA != null || this.EntityPM.Transshipment1ATA != null
            || this.EntityPM.Transshipment2ATA != null || this.EntityPM.Transshipment3ATA != null) {
            errors.push("Shipment has departed/arrived, can't change direction");
        }
        else if (this.HasPayablesAmounts() && this.HasReceivablesAmounts()) {
            errors.push("Shipment has Payables and Receivables amounts, can't change direction");
        }

        else if (this.HasPayablesAmounts()) {
            errors.push("Shipment has Payables amounts, can't change direction");
        }

        else if (this.HasReceivablesAmounts()) {
            errors.push("Shipment has Receivables amounts, can't change direction");
        }

        if (errors.length == 0) {
            this.shipmentService.CheckIfConnectedEntryOrRelease(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    var result: boolean = myResponse.Result;

                    if (result) {
                        errors.push("Shipment has connected Cross DocKs Entries/Releases, can't change direction");
                    }

                    this.ShowConvertShipmentDirectionWindow(errors);
                }
            });
        }

        else {
            this.ShowConvertShipmentDirectionWindow(errors);
        }
    }

    private HasPayablesAmounts() {
        var hasAnyPayablesAmount: boolean = false;
        if (this.EntityPM.ShipmentPayables != null) {
            var filterdPayableLineWithAmounts = this.EntityPM.ShipmentPayables.filter(payable => payable.ExpectedAmount != null && payable.ExpectedAmount != 0.0)[0];
            if (filterdPayableLineWithAmounts != null) {
                hasAnyPayablesAmount = true;
            }            
        }       
        return hasAnyPayablesAmount;
    }

    private HasReceivablesAmounts() {
        var hasAnyReceivablesAmount: boolean = false;
        if (this.EntityPM.ShipmentReceivables != null) {
            var filterdReceivableLineWithAmounts = this.EntityPM.ShipmentReceivables.filter(payable => payable.TotalAmount != null && payable.TotalAmount != 0.0)[0];
            if (filterdReceivableLineWithAmounts != null) {
                hasAnyReceivablesAmount = true;
            }   
        }
        return hasAnyReceivablesAmount;
    }


    private ShowConvertShipmentDirectionWindow(errors: string[]) {
        this.currentActionName = "ConvertShipmentDirection";

        var args = new ConvertDirectionArgs();        
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.ValidationErrorsList = errors;
        args.EnabledOkButton = true;

        if (errors.length > 0) {
            args.EnabledOkButton = false;
        }
        
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 960;
        logWindow.Height = 570;

        logWindow.Title = "Convert Shipment Direction";
        logWindow.Show('./Shipment/Components/MenuButtons/ShipmenDirectionConvertComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d == "ok") {
                    this.ResetButtonClicked();
                }
            });
        });
    }

    private IsSendToAMANACClicked: boolean = false;
    private SendToAMANACClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsSendToAMANACClicked = true;
            this.OkButton();
        }
    }
    private DoSendToAMANA() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.shipmentService.SendToAMANAC(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName: string = myResponse.Result;               

                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + fileName;
                {
                    window.open(url);
                }
            }

            else {
                
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private myCloner: Cloner;
    private Clone(EntityPM: ShipmentPM) {
        this.myCloner.AddField('IsOperationalClosed');
        this.myCloner.AddField('IsMaster');
        this.myCloner.AddEntity(EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
    private DisplayErrorsWindow(WarningsList: Array<string>, ErrorsList: Array<string>) {
        console.log(WarningsList);
        console.log(ErrorsList);

        var args = new MenuButtonsTemplateArgs();
        args.ValidationErrorsList = ErrorsList;
        args.ValidationWarningsList = WarningsList;
        args.IsNotesStackPanelVisible = true;
        args.ActionStepsStateList = this.ActionStepsStateList;

        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        if (ErrorsList.length != 0) {
            state.State = "Failed";
        }
        else {
            state.State = "Succeeded";
        }

        this.ActionStepsStateList.push(state);

        if (ErrorsList.length > 0) {
            args.EnabledOkButton = false;
        }

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Shipment Operational Close";
        logWindow.WindowArgs = args;
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
            });
        });
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsOperationalClosed = true;
                this.EntityPM.OperationalCloseDate = DateTool.GetCurrentDateTimeAsUtc();                
                this.OkButton();
            }
            else {
                this.EntityPM.RejectChanges();
                this.RejectChanges();
            }
        });
    }
    private ValidateShipmentRules(WarningsList: Array<string>, ErrorsList: Array<string>, entityPM: any) {
        if (SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "TSV")[0] == null) {
            var warningValidator: EntityWarningsValidator = new EntityWarningsValidator();
            var ruleValidator: RulesValidator = new RulesValidator();
            var requiredFields: Array<ObjectTableRuleFieldPM> = [];

            if (entityPM.ShipmentLevelCode == "H") {
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AI", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OI", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_IE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_II", entityPM, requiredFields);

                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AI", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OI", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_IE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_II", entityPM, WarningsList);
            }

            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") {
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II", entityPM, requiredFields);

                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);

                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
                ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);

                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II", entityPM, WarningsList);

                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE_D", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI_D", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE_D", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI_D", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE_D", entityPM, WarningsList);
                warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II_D", entityPM, WarningsList);
            }

            var _tenantObjectFields = window.ObjectFields;
            if (requiredFields.length != 0) {
                for (var k in requiredFields) {
                    var field = requiredFields[k];
                    var obField = _tenantObjectFields.filter(x => x.FieldCode === field.ObjectFieldCode)[0];
                    var requiredError = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                    var fieldTrans = TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                    requiredError = requiredError.replace("%FieldName", fieldTrans);
                    ErrorsList.push(requiredError);
                }

            }
        }
    }

    private IsStandAloneFeatureShipment() {
        return this.EntityPM.IsStandalonePickupDelivery;
    }

    private IsForwarderShipmentConnectedWithStandAlone() {
        var result = false;
        if (this.EntityPM.ShipmentPickUps != null && this.EntityPM.ShipmentPickUps.length != 0) {
            result = (this.EntityPM.ShipmentPickUps.filter(pickup =>
                !AppTool.IsNullOrEmpty(pickup.StandaloneShipmentId)).length != 0 ? true : result
            );
        }
        if (this.EntityPM.ShipmentDeliveries != null && this.EntityPM.ShipmentDeliveries.length != 0) {
            result = (this.EntityPM.ShipmentDeliveries.filter(delivery =>
                !AppTool.IsNullOrEmpty(delivery.StandaloneShipmentId)).length != 0 ? true : result
            );
        }
        return result;
    }

    SetIsStandaloneWithPickupDeliveryOnlyVisible() {
        var featureToggle: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "OPD")[0];
        if (featureToggle) {
            return true;
        }
        return false;
    }
}
export class ActionValidationArgs {
    public EnttiyPM: any;
    public WarningsList: Array<string> = [];
    public ErrorsList: Array<string> = [];
}
export class ActionsStepsState {
    constructor() { }
    private message: string;
    private state: string;
    public set Message(value: string) { this.message = value; }
    public get Message() { return this.message; }
    public set State(value: string) { this.state = value; }
    public get State() { return this.state; }
}
export class ValidationErrorInfo {
    private messageType: string;
    private errorCode: number;
    private errorMessage: string;
    public get MessageType() { return this.messageType; }
    public set MessageType(value: string) { this.messageType = value; }
    public set ErrorCode(value: number) { this.errorCode = value; }
    public get ErrorCode() { return this.errorCode; }
    public get ErrorMessage() { return this.errorMessage; }
    public set ErrorMessage(value: string) { this.errorMessage = value; }
    public ToString() {
        return this.ErrorMessage;
    }
}
