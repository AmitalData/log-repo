import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'

import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { ExportDeclarationDataRequestParams } from '../../../../Customs/DataContract/RequestParams/ExportDeclarationDataRequestParams';
import { ExportDeclarationDataResponseData } from '../../../../Customs/DataContract/ResponseData/ExportDeclarationDataResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'ExportDeclarationDataComponent',
    moduleId: module.id,
    templateUrl: './ExportDeclarationDataComponent.html',
})

export class ExportDeclarationDataComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: ExportDeclarationDataComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    public InvoiceList: ObservableCollection;
    public RequestList: ObservableCollection;
    public GovernmentProcedureList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.InvoiceList = new ObservableCollection([]);
        this.RequestList = new ObservableCollection([]);
    }



    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }



    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new ExportDeclarationDataRequestParams();
        }

        if (this.ResponseData) {
            if (this.ResponseData.InvoiceList) {
                this.InvoiceList.InsertCollection(this.ResponseData.InvoiceList);
            }

            if (this.ResponseData.RequestList) {
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
            if (this.ResponseData.RequestList) {
                for (let item of this.ResponseData.RequestList) {
                    item.GovernmentProcedureList.forEach((itemLine) => {
                        this.GovernmentProcedureList.push(itemLine.ItemGovernmentProcedureType);
                    });
                    item.VehicleList = new ObservableCollection(item.VehicleList);
                }
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
            if (this.ResponseData.RequestList) {
                for (let item of this.ResponseData.RequestList) {
                    item.GovernmentProcedureList.forEach((itemLine) => {
                        this.GovernmentProcedureList.push(itemLine.ItemGovernmentProcedureType);
                    });
                    item.VehicleList = new ObservableCollection(item.VehicleList);
                }
                this.RequestList.InsertCollection(this.ResponseData.RequestList);
            }
        }
        else {
            this.ResponseData = new ExportDeclarationDataResponseData();
        }
    }

    //#region Properties
    
    get ReshimonNubmer() { return this.RequestParams ? this.RequestParams.ReshimonNubmer : null; }
    set ReshimonNubmer(value: string) {
        if (this.RequestParams.ReshimonNubmer != value) {
            this.RequestParams.ReshimonNubmer = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.ReshimonNubmer)) {
                this.UIProperties.SetRequired("ReshimonNubmer", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("ReshimonNubmer", this.ObjectTableName, false);
            }
        }
    }

    get Title() { return this.ResponseData ? this.ResponseData.Title : null; }
    set Title(value: string) {
        if (this.ResponseData.Title != value) {
            this.ResponseData.Title = value;
        }
    }

    get FOBNetoNISAmount() { return this.ResponseData ? this.ResponseData.FOBNetoNISAmount : null; }
    set FOBNetoNISAmount(value: string) {
        if (this.ResponseData.FOBNetoNISAmount != value) {
            this.ResponseData.FOBNetoNISAmount = value;
        }
    }

    get AgentCustomerExternalID() { return this.ResponseData ? this.ResponseData.AgentCustomerExternalID : null; }
    set AgentCustomerExternalID(value: string) {
        if (this.ResponseData.AgentCustomerExternalID != value) {
            this.ResponseData.AgentCustomerExternalID = value;
        }
    }

    get FOBNISAmount() { return this.ResponseData ? this.ResponseData.FOBNISAmount : null; }
    set FOBNISAmount(value: string) {
        if (this.ResponseData.FOBNISAmount != value) {
            this.ResponseData.FOBNISAmount = value;
        }
    }

    get CalculationDate() { return this.ResponseData ? this.ResponseData.CalculationDate : null; }
    set CalculationDate(value: string) {
        if (this.ResponseData.CalculationDate != value) {
            this.ResponseData.CalculationDate = value;
        }
    }

    get LoadingDate() { return this.ResponseData ? this.ResponseData.LoadingDate : null; }
    set LoadingDate(value: string) {
        if (this.ResponseData.LoadingDate != value) {
            this.ResponseData.LoadingDate = value;
        }
    }
    
    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty(this.ReshimonNubmer)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.ExportDeclarationDataQuery.O.ReshimonNubmerMandatory"));
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new ExportDeclarationDataRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ReshimonNubmer = this.ReshimonNubmer;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא להצהרה יצוא", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._DeclarationMessagesService.PostExportDeclarationDataRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    EditButtonClicked(item) {
        if (this.GovernmentProcedureList != null && this.GovernmentProcedureList.length > 0) {      

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 300;
            logitudeWindow.Height = 380;
            logitudeWindow.IsShowCloseButton = true;
            logitudeWindow.Title = "תהליכים לסחורה";
            logitudeWindow.WindowArgs = this.GovernmentProcedureList;
            //logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnCustomFilesScreenWindowClosed($event));
          logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/General/AccountingCustomFilesComponent');

        }
    }

    VehicleButtonClicked(item) {
        if (item.VehicleList != null && item.VehicleList.Collection.length > 0) {

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 380;
            logitudeWindow.Height = 380;
            logitudeWindow.IsShowCloseButton = true;
            logitudeWindow.Title = "רכבים לסחורה";
            logitudeWindow.WindowArgs = item.VehicleList;
            //logitudeWindow.Show('./Customs/Components/CustomsRequests/DeclarationRequests/VehicleForGoodsItemComponent');
            logitudeWindow.Show('./CustomsModules/CustomsRequests/Components/DeclarationRequests/VehicleForGoodsItemComponent');

        }
    }


    //#endregion Commands
}
