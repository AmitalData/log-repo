import { OnInit, Component } from '@angular/core';
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

@Component({
    selector: 'ExportDeclarationClosingDataComponent',
    templateUrl: './ExportDeclarationClosingDataComponent.html',
})

export class ExportDeclarationClosingDataComponent extends BaseComponent {
    public DataContext: any = this;
    public EntityPM: ExportDeclarationClosingDataPM;
    public DecPM: DeclarationPM;
    public DeclarationIsClosed:boolean=false;
    public ConPM: ConsignmentPM;
    public ObjectTableName: string = "Customs.ExportDeclarationClosingData";
    public IsReady: boolean = false;
    ValidationErrors: string[];
    DeclarationService: DeclarationWebService = new DeclarationWebService();;
    exportDeclarationClosingDataPMService: ExportDeclarationClosingDataPMService = new ExportDeclarationClosingDataPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;

    constructor(private EntityResourceService: EntityResourceService) {
        super();
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, true);
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
            if(this.DecPM.IsExportClosed){
                this.DeclarationIsClosed=true
                this.setInputsReadOnly();
            }
        });
    }

    setInputsReadOnly(){
        this.UIProperties.SetEnabled("FinalCargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalSecondCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalThirdCargoId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalManifestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("FinalLoadingSite", this.ObjectTableName, false);

    }

    GetExportDeclarationClosingData(id: string) {
        if (id != null) {
            this.exportDeclarationClosingDataPMService.get(id).subscribe((response: any) => {
                this.EntityPM = response.Result;
                if (this.EntityPM == null) {
                    this.EntityPM = new ExportDeclarationClosingDataPM();
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
                    this.IsNew = true;
                }
                this.IsReady = true;
                this.ConPM = this.DecPM.Consignments[0];
            });
        }
    }


    get LoadingDateTime() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
    set LoadingDateTime(value: Date) {
        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
        }
    }

    get FinalShipCode() { return this.EntityPM ? this.EntityPM.FinalShipCode : null; }
    set FinalShipCode(value: string) {
        if (this.EntityPM.FinalShipCode != value) {
            this.EntityPM.FinalShipCode = value;
        }
    }

    get FinalLoadingSite() { return this.EntityPM ? this.EntityPM.FinalLoadingSite : null; }
    set FinalLoadingSite(value: string) {
        if (this.EntityPM.FinalLoadingSite != value) {
            this.EntityPM.FinalLoadingSite = value;
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
            return myFormats.DateString + " " + myFormats.ShortTimeString;
        }
        return null;
    }

    get Smp() { return this.EntityPM ? this.EntityPM.SMP : null; }
    set Smp(value: string) {
        if (this.EntityPM.SMP != value) {
            this.EntityPM.SMP = value;
        }
    }

    get FinalCargoTypeCode() { return this.EntityPM ? this.EntityPM.FinalCargoTypeCode : null; }
    set FinalCargoTypeCode(value: string) {
        if (this.EntityPM.FinalCargoTypeCode != value) {
            this.EntityPM.FinalCargoTypeCode = value;
        }
    }

    get FinalManifestNumber() { return this.EntityPM ? this.EntityPM.FinalManifestNumber : null; }
    set FinalManifestNumber(value: string) {
        if (this.EntityPM.FinalManifestNumber != value) {
            this.EntityPM.FinalManifestNumber = value;
        }
    }
    get FinalSecondCargoId() { return this.EntityPM ? this.EntityPM.FinalSecondCargoId : null; }
    set FinalSecondCargoId(value: string) {
        if (this.EntityPM.FinalSecondCargoId != value) {
            this.EntityPM.FinalSecondCargoId = value;
        }
    }
    get FinalThirdCargoId() { return this.EntityPM ? this.EntityPM.FinalThirdCargoId : null; }
    set FinalThirdCargoId(value: string) {
        if (this.EntityPM.FinalThirdCargoId != value) {
            this.EntityPM.FinalThirdCargoId = value;
        }
    }


    SendButtonClicked(event: CustomSendOptionsArgs) {
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
             if (!AppTool.IsNullOrEmpty(response) && !AppTool.IsNullOrEmpty(response.Result) && !AppTool.IsNullOrEmpty(response.Result.UserMessage)) {
                this.ValidationErrors.push(response.Result.UserMessage);
                this.FillValidationErrors("Errors");
            }
            debugger;
            //DOTO

            //if success
            if (response.Result) {


                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "DEGC";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
            //if reject
            else {
                this.CurrentSession.CurrentEditComponent.PreSelectedTabCode = "CloD";
                this.CurrentSession.CurrentEditComponent.SetSelectedTab();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        

          //  SessionLocator.SelectedSession.CloseCurrentWindow();
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
}




