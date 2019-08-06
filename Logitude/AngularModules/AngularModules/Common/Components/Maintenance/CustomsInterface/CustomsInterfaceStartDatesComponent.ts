import { Component } from '@angular/core';
import { CustomsInterfaceSettingPM } from '../../../EntityPMs/CustomsInterfaceSettingPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsInterfaceStartDatesComponent.html',
})

export class CustomsInterfaceStartDatesComponent extends BaseComponent {
    public EntityPM: CustomsInterfaceSettingPM;
    public ObjectTableName: string = "CustomsInterfaceSetting";
    public DataContext: CustomsInterfaceStartDatesComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public Code: string = null;    
    private shipmentDomainService: ShipmentDomainService;
    constructor() {
        super();
        this.shipmentDomainService = new ShipmentDomainService();
    }

    SetWindowArgs(args: any) {
        this.Code = args['Code'];
        this.EntityPM = args['EntityPM'];
        
        this.Clone();
    }
    
    get AMCAirStartDate() { return this.EntityPM.AMCAirStartDate; }
    set AMCAirStartDate(value: Date) {
        if (this.EntityPM.AMCAirStartDate != value) {
            this.EntityPM.AMCAirStartDate = value;
        }
    }

    get AMCOceanStartDate() { return this.EntityPM.AMCOceanStartDate; }
    set AMCOceanStartDate(value: Date) {
        if (this.EntityPM.AMCOceanStartDate != value) {
            this.EntityPM.AMCOceanStartDate = value;
        }
    }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private isOkButtonClicked: boolean = false;
    OkButtonClicked() {
        if (!this.isOkButtonClicked) {
            this.isOkButtonClicked = true;

            var errors: string[] = [];
            var myStartDate: Date = null;

            switch (this.Code) {
                case "Air": {
                    myStartDate = this.AMCAirStartDate;
                    break;
                }

                case "Ocean": {
                    myStartDate = this.AMCOceanStartDate;
                    break;
                }
            }

            if (AppTool.IsNullOrEmpty(myStartDate)) {
                var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                errors.push(msg.replace("%FieldName", "Start date"));
            }

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {
                var myConfirmWindow = new ConfirmWindow();
                myConfirmWindow.Width = 400;
                myConfirmWindow.Show(this.GetConfirmMessage(myStartDate));

                myConfirmWindow.WindowClosed.subscribe((event: any) => {
                    if (myConfirmWindow.Yes) {
                        this.UpdateSystemStartDate(myStartDate);
                    }

                    else {
                        this.ReApplyOkButton();
                    }
                });
            }

            else {
                this.ReApplyOkButton();
            }
        }       
    }
    ReApplyOkButton() {
        this.isOkButtonClicked = false;
        this.CurrentSession.StopBusyIndicator();
    }
    GetConfirmMessage(myStartDate: Date) {
        var myResult: string = "";
        var myDateString: string = "";

        if (myStartDate != null) {
            myDateString = DateTool.GetDateFormats(myStartDate).ShortDateString;
        }

        myResult = "Please note that all the shipments with a create date smaller than " + myDateString + " will be updated and marked as blocked for transfer";

        return myResult;
    }

    private stepCount: number = 10;
    private sentCount: number = 0;
    private allEntitiesCount: number = 0;
    private entitiesIdsList: string[] = [];
    public NoDataText: string = null;
    public UpdatedDataText: string = null;
    public IsNoDataTextVisible: boolean = false;
    public IsUpdateTextVisible: boolean = false;
    UpdateSystemStartDate(myStartDate: Date) {
        this.CurrentSession.StartBusyIndicator("Calculating data...");

        this.shipmentDomainService.SetAMANACStartDate(this.Code, myStartDate).subscribe((myResponse1: ServiceResponse) => {
            if (myResponse1.HasError) {
                this.ValidationErrorsList = myResponse1.ErrorsArray;
                this.ReApplyOkButton();
            }

            else {
                this.Clone();

                this.shipmentDomainService.GetOnStartDateEntitiesIds(this.Code, myStartDate).subscribe((myResponse2: ServiceResponse) => {
                    this.sentCount = 0;
                    this.entitiesIdsList = [];
                    this.UpdatedDataText = "";
                    this.IsNoDataTextVisible = false;
                    this.IsUpdateTextVisible = false;

                    if (!myResponse2.HasError) {
                        this.entitiesIdsList = myResponse2.Result;
                    }

                    this.allEntitiesCount = this.entitiesIdsList.length;

                    if (this.allEntitiesCount == 0) {
                        this.NoDataText = "No shipments smaller than this date";
                        this.IsNoDataTextVisible = true;
                        this.ReApplyOkButton();
                    }

                    else {
                        this.IsUpdateTextVisible = true;
                        this.Blocking();
                    }
                });
            }
        });
    }

    Blocking() {
        if (this.entitiesIdsList.length == 0) {
            if (this.sentCount == 1) {
                this.UpdatedDataText = "1 shipment has been blocked";
            }

            else {
                this.UpdatedDataText = this.sentCount + " shipments have been blocked";
            }

            this.ReApplyOkButton();
        }

        else {
            var idsList: string[] = [];

            this.entitiesIdsList.forEach(id => {
                if (idsList.length < this.stepCount) {
                    idsList.push(id);
                }
            });

            idsList.forEach(id => {
                var indexOfId: number = this.entitiesIdsList.indexOf(id);
                if (indexOfId > -1) {
                    this.entitiesIdsList.splice(indexOfId, 1);
                }
            });

            this.sentCount = this.sentCount + idsList.length;

            this.CurrentSession.StartBusyIndicator("Blocking " + this.sentCount + " from " + this.allEntitiesCount + " shipments");

            this.shipmentDomainService.BlockTransferEntities(idsList, this.Code).subscribe((myResponse: ServiceResponse) => {
                this.Blocking();
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('AMCAirStartDate');
        this.myCloner.AddField('AMCOceanStartDate');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
