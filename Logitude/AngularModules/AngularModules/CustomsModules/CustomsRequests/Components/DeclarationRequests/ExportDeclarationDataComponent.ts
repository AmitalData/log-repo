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
        let toTest = false;
        if (toTest) {
            this.ResponseData = this.tester();
            this.OnMassageDisplayMethod();
        }
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
                this.InvoiceList.Collection.sort((a, b) => { return (a.SequenceNumber > b.SequenceNumber) ? 1 : -1 })
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
            if (this.ResponseData.RequestList) {
                this.RequestList.Collection.sort((a, b) => { return (+a.SequenceNumber > +b.SequenceNumber) ? 1 : -1 })
            }
        }
        else {
            this.ResponseData = new ExportDeclarationDataResponseData();
            
        }
    }
    tester() {
        this.InvoiceList = new ObservableCollection([]);
        this.RequestList = new ObservableCollection([]);

        this.GovernmentProcedureList= [];
        var b = { "$id": "1", "DeclarationID": null, "ReshimonNumber": "980002646", "Title": "19980080002645", "LoadingDate": "09.01.2019", "CalculationDate": "09.01.2019", "AgentCustomerExternalID": "513094649", "DeclarationNumber": null, "FOBNetoNISAmount": null, "FOBNISAmount": null, "InvoiceList": [{ "$id": "2", "SequenceNumber": "1", "ExternalID": "1006", "InvoiceAmountCurrency": "2904.7600", "InvoiceAmount": "2904.7600", "InvoiceCurrency": null }], "RequestList": [{ "$id": "3", "SequenceNumber": "3", "CustomsItem": "4821900000/6", "ValueQuantity": "120.000", "ForeignCurrencyAmount": "1234567891123456.12 (USD)", "ForeignAmount": "23.0000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "4", "ItemGovernmentProcedureType": "ABCDEFG", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "5", "SequenceNumber": "2", "CustomsItem": "4819100000/8", "ValueQuantity": "20.000", "ForeignCurrencyAmount": "5.0000 (10)", "ForeignAmount": "5.0000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "6", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "7", "SequenceNumber": "1", "CustomsItem": "5807900000/4", "ValueQuantity": "80.000", "ForeignCurrencyAmount": "22.0000 (10)", "ForeignAmount": "22.0000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "8", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "9", "SequenceNumber": "1", "CustomsItem": "6204630000/0", "ValueQuantity": "1001.000", "ForeignCurrencyAmount": "300.5000 (10)", "ForeignAmount": "300.5000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "10", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "11", "SequenceNumber": "12", "CustomsItem": "4823906000/6", "ValueQuantity": "80.000", "ForeignCurrencyAmount": "16.0000 (10)", "ForeignAmount": "16.0000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "12", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "13", "SequenceNumber": "3", "CustomsItem": "5804210000/6", "ValueQuantity": "119.000", "ForeignCurrencyAmount": "32.5000 (10)", "ForeignAmount": "32.5000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "14", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "15", "SequenceNumber": "2", "CustomsItem": "6206400000/3", "ValueQuantity": "9365.000", "ForeignCurrencyAmount": "1873.9000 (10)", "ForeignAmount": "1873.9000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "16", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "17", "SequenceNumber": "13", "CustomsItem": "7310291000/4", "ValueQuantity": "240.000", "ForeignCurrencyAmount": "600.0000 (10)", "ForeignAmount": "600.0000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "18", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "19", "SequenceNumber": "11", "CustomsItem": "3926909000/5", "ValueQuantity": "40.000", "ForeignCurrencyAmount": "6.2000 (10)", "ForeignAmount": "6.2000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "20", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "21", "SequenceNumber": "8", "CustomsItem": "9606100000/4", "ValueQuantity": "10.000", "ForeignCurrencyAmount": "4.8600 (10)", "ForeignAmount": "4.8600", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "22", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "23", "SequenceNumber": "7", "CustomsItem": "9606210000/1", "ValueQuantity": "40.000", "ForeignCurrencyAmount": "6.5000 (10)", "ForeignAmount": "6.5000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "24", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "25", "SequenceNumber": "5", "CustomsItem": "3923299000/0", "ValueQuantity": "125.000", "ForeignCurrencyAmount": "11.8000 (10)", "ForeignAmount": "11.8000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "26", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }, { "$id": "27", "SequenceNumber": "4", "CustomsItem": "4002990000/4", "ValueQuantity": "6.000", "ForeignCurrencyAmount": "2.5000 (10)", "ForeignAmount": "2.5000", "ForeignCurrency": "10", "OriginCountry": null, "OriginCountryName": null, "GovernmentProcedureList": [{ "$id": "28", "ItemGovernmentProcedureType": "10", "ItemGovernmentProcedureName": null }], "VehicleList": [] }], "HasException": false, "UserMessage": "ניתוח בוצע בהצלחה", "Succeeded": true, "ContinueProcessInBackground": false, "CustomsRequestsSheetId": "3dd0dc00-9e94-4e26-bd49-96292273d751", "CorrelationId": null };
        
        return b;
      
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
