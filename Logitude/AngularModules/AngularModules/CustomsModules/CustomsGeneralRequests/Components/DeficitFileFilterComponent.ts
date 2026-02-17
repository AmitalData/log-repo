import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { DeficitFileFilterRequestParams } from '../../../Customs/DataContract/RequestParams/DeficitFileFilterRequestParams';
import {
    DeficitFilesDetailResponseData,
    ExternalFilesDetailsResult,
    ExternalPaymentOrderResult,
    RequireDocumentsResult
} from '../../../Customs/DataContract/ResponseData/DeficitFilesDetailResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'DeficitFileFilterComponent',
    moduleId: module.id,
    templateUrl: './DeficitFileFilterComponent.html',
})

export class DeficitFileFilterComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: DeficitFileFilterComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    public OpenFilesList: ObservableCollection = new ObservableCollection([]);
    public CloseFileList: ObservableCollection = new ObservableCollection([]);
    public PaymentOrderList: ObservableCollection = new ObservableCollection([]);
    public RequireDocumentsList: ObservableCollection = new ObservableCollection([]);
        
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
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

    get StatusName() {
        return this.ResponseData.StatusName;
    }


    get ExternalName() {
        return this.ResponseData.ExternalName;
    }


    get CustomOfficeName() {
        return this.ResponseData.CustomOfficeName;
    }

    get AgentName() {
        return this.ResponseData.AgentName;
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new DeficitFileFilterRequestParams();
        }

        if (this.ResponseData) {
            ////
        }
        else {
            
            let myDeficitFilesDetailResponseData = new DeficitFilesDetailResponseData();
            this.ResponseData = myDeficitFilesDetailResponseData;

           
            
        }
       
        setTimeout(() => {
            if (this.ResponseData.OpenFilesList) {

                this.OpenFilesList.InsertCollection(this.ResponseData.OpenFilesList);
                
            }
            if (this.ResponseData.CloseFileList) {

                this.CloseFileList.InsertCollection(this.ResponseData.CloseFileList);

            }
            if (this.ResponseData.PaymentOrderList) {
                this.AmountSumTotal= 0;
                this.PaymentOrderList.InsertCollection(this.ResponseData.PaymentOrderList);
                this.PaymentOrderList.Collection.forEach(
                    (p) => {
                        this.AmountSumTotal = this.AmountSumTotal + Number(p.AmountSum);
                    }
                );

                let headerH = 27;
                let rowH = 26;
                let top: number = headerH + (this.PaymentOrderList.Length * 26) + 2;
                this.FooterMethods = top;
            }
            if (this.ResponseData.RequireDocumentsList) {
                this.RequireDocumentsList.InsertCollection(this.ResponseData.RequireDocumentsList);
            }
            
        },200);
    }
    FooterMethods: number = 0;
    AmountSumTotal: number = 0;
    BuildDummy() {
        let myDeficitFilesDetailResponseData = new DeficitFilesDetailResponseData();
        this.ResponseData = myDeficitFilesDetailResponseData;

        this.ResponseData.FileStatus = "FileStatus";
        this.ResponseData.StatusName = "StatusName ";
        this.ResponseData.ExternalID = "ExternalID ";
        this.ResponseData.ExternalName = "ExternalName ";
        this.ResponseData.CustomOfficeNumber = "CustomOfficeNumber ";
        this.ResponseData.CustomOfficeName = "CustomOfficeName ";
        this.ResponseData.FilingNumber = "FilingNumber ";
        this.ResponseData.OpenFileCounter = "OpenFileCounter ";
        this.ResponseData.CloseFileCounter = "CloseFileCounter ";
        this.ResponseData.AgentExternalID = "AgentExternalID";
        this.ResponseData.AgentName = "AgentName";


        let myExternalFilesDetailsResult: ExternalFilesDetailsResult;
        myExternalFilesDetailsResult = {
            "ExternalID": "ExternalID",
            "ExternalName": "ExternalName",
            "FileNumber": "FileNumber",
            "Numeral": "Numeral",
            "DisplayFileNumber": "DisplayFileNumber",
            "DeficitEntityType": "DeficitEntityType",
            "EntityTypeName": "EntityTypeName",
            "DeficitEntityID": "DeficitEntityID",
            "ProductionDate": "ProductionDate",
            "UnpaidBalance": "UnpaidBalance",
            "EstimatedBalance": "EstimatedBalance",
            "EstimatedDate": "EstimatedDate",
            "Status": "Status",
            "StatusName": "StatusName",
            "TotalComponentAmount": "TotalComponentAmount",
            "TotalRefundAmount": "TotalRefundAmount",
            "CloseDate": "CloseDate",
            "SecondaryStatus": "SecondaryStatus",
        };
        myDeficitFilesDetailResponseData.OpenFilesList = [];
        myDeficitFilesDetailResponseData.OpenFilesList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.OpenFilesList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.CloseFileList = [];
        myDeficitFilesDetailResponseData.CloseFileList.push(myExternalFilesDetailsResult);
        myDeficitFilesDetailResponseData.CloseFileList.push(myExternalFilesDetailsResult);

        let myExternalPaymentOrderResult: ExternalPaymentOrderResult;
        myExternalPaymentOrderResult = {
            "ExternalID": "ExternalID",
            "ExternalName": "ExternalName",


            "PaymentID": "PaymentID",
            "PaymentProcessType": "PaymentProcessType",
            "PaymentProcessName": "PaymentProcessName",
            "AmountSum": 100,
            "ValidityDateTo": "ValidityDateTo",
            "CreateDate": "CreateDate",
            "PaymentOrderPayDate": "PaymentOrderPayDate",
            "PaymentOrderStatus": "PaymentOrderStatus",
            "PaymentOrderStatusName": "PaymentOrderStatusName"
        };
        myDeficitFilesDetailResponseData.PaymentOrderList = [];
        myDeficitFilesDetailResponseData.PaymentOrderList.push(myExternalPaymentOrderResult);
        myDeficitFilesDetailResponseData.PaymentOrderList.push(myExternalPaymentOrderResult);

        myDeficitFilesDetailResponseData.RequireDocumentsList = [];
        let myRequireDocumentsResult: RequireDocumentsResult = {
            "DocumentCode": "DocumentCode",
            "DocumentTypeName": "DocumentTypeName",
            "FileNumber": "FileNumber",
            "Numeral": "Numeral",
            "DisplayFileNumber": "DisplayFileNumber",
            "DocumentID": "DocumentID"
        };
        myDeficitFilesDetailResponseData.RequireDocumentsList.push(myRequireDocumentsResult);
        myDeficitFilesDetailResponseData.RequireDocumentsList.push(myRequireDocumentsResult);
    }
  

    //#region Properties
    get FileNumber() { return this.RequestParams.FileNumber; }
    set FileNumber(value: string) {
        if (this.RequestParams.FileNumber != value) {
            this.RequestParams.FileNumber = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, false);
            }

        }
    }

    get Numeral() { return this.RequestParams.Numeral; }
    set Numeral(value: string) {
        if (this.RequestParams.Numeral != value) {
            this.RequestParams.Numeral = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.Numeral)) {
                this.UIProperties.SetRequired("Numeral", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("Numeral", this.ObjectTableName, false);
            }
        }
    }




    //#endregion Properties


    //#endregion Response Properties



    //#endregion Response Properties

    //#region Declaration Commands



    //#endregion


    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (AppTool.IsNullOrEmpty( this.FileNumber )) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeficitFileFilterQuery.O.FileNumberMandatory"));
        }
        if (AppTool.IsNullOrEmpty(this.Numeral)) {
        
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeficitFileFilterQuery.O.NumeralMandatory"));

        }
        

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new DeficitFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        


        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא לגרעונות", true)
            .then((res) => {
                
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });




        this._IIGGeneralMessagesService.PostDeficitFileFilterRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                this.ResponseData = myServiceResponse.Result;
                this.OnMassageDisplayMethod();
            });

    }

    //#endregion Commands
}
