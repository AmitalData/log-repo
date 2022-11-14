import { OnInit, Component, ChangeDetectorRef } from '@angular/core';
import { ExportDeclarationClosingDataPM } from '../../../../../Customs/EntityPMs/ExportDeclarationClosingDataPM';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ExportDeclarationClosingDataPMService } from '../../../../../Customs/Services/StandardPMs/ExportDeclarationClosingDataPMService';
import { Time } from '@angular/common';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { GenericRequestParams } from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { AmendmentRequestParams } from '../../../../../Customs/DataContract/RequestParams/AmendmentRequestParams';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';
import { ExportDeclarationClosingDatasExtendPMService } from 'Customs/Services/ExtendedPMs/ExportDeclarationClosingDatasExtendPMService';
import { CustomsSettingListService } from 'Customs/Services/StandardLists/CustomsSettingListService';
import { ExportDeclarationClosingWebService } from 'Customs/Services/WebServices/ExportDeclarationClosingWebService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { CargoIdentifireTypeListService } from 'Customs/Services/StandardLists/CargoIdentifireTypeListService';

@Component({
    selector: 'ExportDeclarationClosingDataComponent',
    templateUrl: './ExportDeclarationClosingDataComponent.html',
})

export class ExportDeclarationClosingDataComponent extends BaseComponent {
    public DataContext: any = this;
    public EntityPM: ExportDeclarationClosingDataPM;
    public DecPM: DeclarationPM;
    public DeclarationIsClosed: boolean = false;
    public ObjectTableName: string = "Customs.ExportDeclarationClosingData";
    public IsReady: boolean = false;
    ValidationErrors: string[] = [];
    DeclarationService: DeclarationWebService = new DeclarationWebService();;
    exportDeclarationClosingDataPMService: ExportDeclarationClosingDataPMService = new ExportDeclarationClosingDataPMService();
    exportDeclarationClosingDatasExtendPMService: ExportDeclarationClosingDatasExtendPMService = new ExportDeclarationClosingDatasExtendPMService();
    _CargoIdentifireTypeListService: CargoIdentifireTypeListService = new CargoIdentifireTypeListService();
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;
    private exportDeclarationClosingWebService: ExportDeclarationClosingWebService = new ExportDeclarationClosingWebService();
    public ActualSailingDate: string = "תאריך הפלגה בפועל";
    public ActualTakeOffDate: string = "תאריך המראה בפועל";

    ManifestNumberPlaceholder: string = '';
    SecondCargoIdPlaceholder: string = '';
    ThirdCargoIdPlaceholder :string = '';

    constructor(
        private EntityResourceService: EntityResourceService,
        private readonly cdr: ChangeDetectorRef, public entityArgs: EntityArgs,
    ) {
        super();
    }

    SetUIProperty() {

        this.UIProperties.SetWarning("LoadingDateTime", this.ObjectTableName, true);

        if (this.DecPM.TransportModeId != "O") {
            this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Smp", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FlightDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("MainAWB", this.ObjectTableName, false);

        }
    }


    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.DecPM = args.EntityPM;
            this.GetExportDeclarationClosingData(this.DecPM.Id);
            this.SetUIProperty();
            if (this.DecPM.IsExportClosed) {
                this.DeclarationIsClosed = true
                this.setInputsReadOnly();
            }

            if (this.DecPM.Direction === 'E'){
                this.setIdentifiersPlaceHolders();
            }           
        });
    }

    setInputsReadOnly() {

        this.UIProperties.SetEnabled("FinalCargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalSecondCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalThirdCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LoadingSite", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalManifestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalLoadingSite", this.ObjectTableName, false);

    }

    GetExportDeclarationClosingData(id: string) {
        if (id != null) {

            this.exportDeclarationClosingDatasExtendPMService.GetSingleWithEFIFILEMData(id).subscribe((response: any) => {
                this.EntityPM = response.Result;
                
                if (this.EntityPM)
                    if (response.Result.ChangeSetOp == "1") {
                        this.EntityPM.IsDirty = true;
                        this.IsNew = true;
                    } else {
                        this.EntityPM.IsDirty = false;
                    }

                if (this.DecPM.Direction === 'E' && this.DecPM.TransportModeId === 'O') {
 
                    if (this.FinalCargoTypeCode == null) {

                        this.FinalManifestNumber = this.FinalManifestNumber == null ? '' : this.FinalManifestNumber;
                        this.FinalSecondCargoId = this.FinalSecondCargoId == null ? '' : this.FinalSecondCargoId;
                        this.FinalThirdCargoId = this.FinalThirdCargoId == null ? '' : this.FinalThirdCargoId;

                        this.FinalCargoTypeCode = '37';
                                                          
                    }
                    else{
                        this.setIdentifiersPlaceHolders();
                    }

                }
                else {
                    this.ManifestNumberPlaceholder = 'XXX-XXXXXXXX';
                    this.setWarningValues();
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.FinalManifestNumber) && this.DecPM.Direction == 'E' && this.DecPM.TransportModeId == 'A' && !AppTool.IsNullOrEmpty(this.EntityPM.MAIN_AWB)) {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM ? this.EntityPM.FinalManifestNumber = this.EntityPM.MAIN_AWB : null;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.FinalCargoTypeCode) && this.DecPM.Direction == 'E' && this.DecPM.TransportModeId == 'A') {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM.FinalCargoTypeCode = "1";
                }

                if(AppTool.IsNullOrEmpty(this.EntityPM.FinalCargoTypeCode) && this.DecPM.TransportModeId == 'L' && !AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].CargoTypeCodeForExport))
                {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM.FinalCargoTypeCode = this.DecPM.Consignments[0].CargoTypeCodeForExport;
                    
                }

                if(AppTool.IsNullOrEmpty(this.EntityPM.FinalManifestNumber) && this.DecPM.TransportModeId == 'L' && !AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].ManifestNumber))
                {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM.FinalManifestNumber = this.DecPM.Consignments[0].ManifestNumber;
                    
                }

                if(AppTool.IsNullOrEmpty(this.EntityPM.FinalLoadingSite) && this.DecPM.TransportModeId == 'L' && !AppTool.IsNullOrEmpty(this.DecPM.Consignments[0].ExportLoadingPortCode))
                {
                    this.EntityPM.IsDirty = true;
                    this.EntityPM.FinalLoadingSite = this.DecPM.Consignments[0].ExportLoadingPortCode;
                    
                }

               
                /*this.EntityPM = new ExportDeclarationClosingDataPM();
                this.EntityPM.DeclarationId = id;
                this.EntityPM.Tenant = this.DecPM.Tenant;
                var consignments = this.DecPM.Consignments.filter(x => x.ConsignmentType == 'E');
                if (consignments.length == 1) {
                    this.EntityPM.FinalCargoTypeCode = consignments[0].CargoTypeCode;
                    this.EntityPM.FinalSecondCargoId = consignments[0].SecondCargoID;
                    this.EntityPM.FinalThirdCargoId = consignments[0].ThirdCargoID;
                    this.EntityPM.FinalManifestNumber = consignments[0].ManifestNumber;
                    this.EntityPM.FinalShipCode = consignments[0].ShipCode;
                    this.EntityPM.FinalLoadingSite = consignments[0].ExportLoadingPortCode;
                }
                this.IsNew = true;*/

                this.IsReady = true;

                this.initOceanExportData();
            });
        }
    }


    setWarningValues() {
        if (this.DecPM.Direction === 'E') {
            this.UIProperties.SetWarning("FinalManifestNumber", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalManifestNumber));
            this.UIProperties.SetWarning("FinalSecondCargoId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalSecondCargoId) && !AppTool.IsNullOrEmpty(this.SecondCargoIdPlaceholder) );
            this.UIProperties.SetWarning("FinalThirdCargoId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FinalThirdCargoId)  && !AppTool.IsNullOrEmpty(this.ThirdCargoIdPlaceholder));
        }
    }

    get LoadingDateTime() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
    set LoadingDateTime(value: Date) {

        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
            this.EntityPM.IsDirty = true;
        }
     
    }

    get FinalShipCode() { return this.EntityPM ? this.EntityPM.FinalShipCode : null; }
    set FinalShipCode(value: string) {
        if (this.EntityPM.FinalShipCode != value) {
            this.EntityPM.FinalShipCode = value;
            this.EntityPM.IsDirty = true;
        }
    }

    get LoadingSite() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
    set LoadingSite(value: Date) {
        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
            this.EntityPM.IsDirty = true;
        }
    }

    get FinalLoadingSite() { return this.EntityPM ? this.EntityPM.FinalLoadingSite : null; }
    set FinalLoadingSite(value: string) {
        if (this.EntityPM.FinalLoadingSite != value) {
            this.EntityPM.FinalLoadingSite = value;
            this.EntityPM.IsDirty = true;
        }
    }
    get MainAWB() { return this.EntityPM ? this.EntityPM.MAIN_AWB : null; }
    set MainAWB(value: string) {
        if (this.EntityPM.MAIN_AWB != value) {
            this.EntityPM.MAIN_AWB = value;
        }
    }
    public get FlightDate() {
        if (this.EntityPM != null && this.EntityPM.FLIGHT_DATE != null) {
            var myFormats = DateTool.GetDateFormats(this.EntityPM.FLIGHT_DATE);
            return myFormats.DateString as any;
            // + " " + myFormats.ShortTimeString;
        }
        return null;
    }
    public set FlightDate(value: Date) {
        if (this.EntityPM.FLIGHT_DATE != value)
            this.EntityPM.FLIGHT_DATE = value;
    }

    get Smp() { return this.EntityPM ? this.EntityPM.SMP : null; }
    set Smp(value: string) {
        if (this.EntityPM.SMP != value) {
            this.EntityPM.SMP = value;
        }
    }

    get ChargingSite() { return this.EntityPM ? this.EntityPM.ChargingSite : null; }
    set ChargingSite(value: string) {
        if (this.EntityPM.ChargingSite != value) {
            this.EntityPM.ChargingSite = value;
        }
    }

    get FinalCargoTypeCode() { return this.EntityPM ? this.EntityPM.FinalCargoTypeCode : null; }
    set FinalCargoTypeCode(value: string) {        
        if (this.EntityPM.FinalCargoTypeCode != value) {
            this.EntityPM.FinalCargoTypeCode = value;
            this.EntityPM.IsDirty = true;

            if (this.DecPM.Direction === 'E')
            {
                this.setIdentifiersPlaceHolders();
            }
        }
    }

    get FinalManifestNumber() { return this.EntityPM ? this.EntityPM.FinalManifestNumber : null; }
    set FinalManifestNumber(value: string) {        
        if (this.EntityPM.FinalManifestNumber != value) {
            this.EntityPM.FinalManifestNumber = value;
            this.EntityPM.IsDirty = true;

           
        }
    }
    get FinalSecondCargoId() { return this.EntityPM ? this.EntityPM.FinalSecondCargoId : null; }
    set FinalSecondCargoId(value: string) {
        if (this.EntityPM.FinalSecondCargoId != value) {
            this.EntityPM.FinalSecondCargoId = value;
            this.EntityPM.IsDirty = true;
        }
    }
    get FinalThirdCargoId() { return this.EntityPM ? this.EntityPM.FinalThirdCargoId : null; }
    set FinalThirdCargoId(value: string) {
        if (this.EntityPM.FinalThirdCargoId != value) {
            this.EntityPM.FinalThirdCargoId = value;
            this.EntityPM.IsDirty = true;
        }
    }

    ViewDocumentsComponent() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.DecPM;
        //windowArgs.ObjectTableName = "Customs.DeclarationCancellation";
        windowArgs.ObjectTableName = "Customs.Declaration";// this.ObjectTableName;
        windowArgs.EntityParentPM = "ExportDeclarationClosingData";
        //    windowArgs.SkipCtor = this.SkipCtor;
        windowArgs.IsFromStandAloneScreen = true;
        var windowTitle = "Customs.Declaration.TH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnDocumentsWindowClosed($event));
        this.entityArgs.SkipCtor = true;
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }
    OnDocumentsWindowClosed(event) {
        this.entityArgs.SkipCtor = false;
    }
    SendButtonClicked(event: CustomSendOptionsArgs) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.LoadingDateTime)) {
            var msg = " שדה תאריך טעינה שדה חובה";
            this.ValidationErrors.push(msg);
            this.FillValidationErrors("Errors");
        }
        else {
            if (this.EntityPM.IsDirty) {
                if (this.IsNew) {
                    this.exportDeclarationClosingDataPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {


                        if (!response.HasError) {
                            this.SendAmendmentCloseDeclaration(event);
                        }
                    });
                } else {
                    this.exportDeclarationClosingDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {

                        if (!response.HasError) {
                            this.SendAmendmentCloseDeclaration(event);
                        }
                    });
                }
            }
            else {
                this.SendAmendmentCloseDeclaration(event);
            }
        }
    }


    SendAmendmentCloseDeclaration(event: CustomSendOptionsArgs) {
        
        this.CurrentSession.CurrentEditComponent.StartBusyIndicator("שליחת מסר סגירת הצהרה");
        var searchParams: AmendmentRequestParams = new AmendmentRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.DeclarationId;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.DeclarationId;
        searchParams.LoggingEntityReference = this.DecPM.DeclarationNumber;
        //searchParams.LoggingObjectTableId = this.ObjectTable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Export Amendment Declaration Request";
        searchParams.ResponseName = "Amendment Declaration Response";
        searchParams.RequestVIA = event.RequestVIA;
        searchParams.ForcePersonalSign = event.ForcePersonalSign;
        searchParams.IsExportClose = true;
        //searchParams.TestCase = event.TestCase;
        let myShowProgressBarParams: ShowProgressBarParams = null;

        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession, searchParams.PBId, "שליחת מסר סגירה", false, myShowProgressBarParams)
            .then((res) => {




            }
            ).catch((err) => {

                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                this.ValidationErrors.push(err);
                this.FillValidationErrors("Errors");
            });

        this.DeclarationService.PostSendDeclarationClosingAmendment(searchParams).subscribe((response: ServiceResponse) => {

            if (!AppTool.IsNullOrEmpty(response) && !AppTool.IsNullOrEmpty(response.Result) && !AppTool.IsNullOrEmpty(response.Result.UserMessage) && response.Result.HasException) {
                //this.ValidationErrors.push(response.Result.UserMessage);
                //this.FillValidationErrors("Errors");
            }

            if (!AppTool.IsNullOrEmpty(response) && !AppTool.IsNullOrEmpty(response.Result) && (response.Result.IsExportCloseApprove || response.Result.HasException)) {


                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DEGC";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (!response.Result.HasException) {
                    var tab = this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(d => d.Code == "CloD")[0];
                    tab.IsDisabled = false;
                }

            }
            //if reject
            else {
                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "CloD";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                var tab = this.CurrentSession.CurrentEditComponent.TabsItemsSource.filter(d => d.Code == "CloD")[0];
                tab.IsDisabled = false;
            }


            SessionLocator.SelectedSession.CloseCurrentWindow();
        });

    }
    FillValidationErrors(title: string) {


        this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.Errors = this.ValidationErrors;
        windowArgs.ComponentHeight = '328px';
        var windowTitle = title;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
    }
    OnAddEditWindowClosed(event) {
        this.ValidationErrors = [];
    }
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }
    OkButtonClicked() {
        
        if (this.ValidationErrors.length > 0)
            this.FillValidationErrors("Errors");


        this.CurrentSession.CurrentEditComponent.StartBusyIndicator("שמירה");
        if (this.IsNew) {
            this.exportDeclarationClosingDataPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
            });
        } else {
            this.exportDeclarationClosingDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
            });
        }
    }


    setIdentifiersPlaceHolders()
    {
        this._CargoIdentifireTypeListService.getSingleFromCache(this.FinalCargoTypeCode)
        .subscribe((Response: ServiceResponse) => {
            if (Response.Result != null) {
                
                this.ManifestNumberPlaceholder = Response.Result.CargoIdentifierKey1Name;
                this.SecondCargoIdPlaceholder = Response.Result.CargoIdentifierKey2Name ?? '';
                this.ThirdCargoIdPlaceholder = Response.Result.CargoIdentifierKey3Name ?? '';

                this.FinalManifestNumber = '';
                this.FinalSecondCargoId = '';
                this.FinalThirdCargoId = '';
                            
                this.setWarningValues();
            }
        });
    }

    async initOceanExportData() {
        if (this.DecPM.Direction !== 'E' || this.DecPM.TransportModeId !== 'O' || !(await this.isConnectedToUniFreight())) return;

        const exportData = await this.exportDeclarationClosingWebService.getUnifreightData(this.DecPM.ExportFile);
        if (!exportData) return;

        this.FlightDate = exportData.flightDate;
        this.ChargingSite = exportData.loadingSite;
        this.Smp = exportData.HAWB;
        this.MainAWB = exportData.MAWB;

        this.cdr.detectChanges();
    }

    isConnectedToUniFreight(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) =>
            new CustomsSettingListService().getSingleFromCache(this.DecPM.Tenant.toString()).subscribe((response: ServiceResponse) =>
                resolve(response.Result.IsConnectedToUniFreight)))
    }
}




