import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ClientPM} from '../../../../Customs/EntityPMs/ClientPM';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ClientSearchRequestParams} from '../../../../Customs/DataContract/RequestParams/ClientSearchRequestParams';
import {ClientSearchResponseData} from '../../../../Customs/DataContract/ResponseData/ClientSearchResponseData';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;
import { CustomMessageProgressComponent } from '../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ClientMessagesService} from '../../../../Customs/Services/WebServices/ClientMessagesService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'NewClientComponent',
    
    templateUrl: './NewClientComponent.html',
})

export class NewClientComponent
    extends BaseComponent {

    public ObjectTableName: string = "Customs.Client";
    public DataContext: any = this;
    _MyResponseObjectToShow: any = null;
    clientPM: ClientPM;
    public ValidationErrorsList: string[] = [];
    requestParams: ClientSearchRequestParams;
    ResponseData: ClientSearchResponseData;
    clientMessagesService: ClientMessagesService = new ClientMessagesService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.clientPM = new ClientPM();
        this.clientPM.Tenant = SessionLocator.Tenant;
        this.IsExternalId = true;
        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((response:any) => {
            });
        });
    }


    SetWindowArgs(menuArg: any) {

        this.OnMassageDisplayMethod();

        if (menuArg.Mode == "DeclarationGeneralComponent") {
            this.IsFromDeclaration = true;
            this.Code = menuArg.ImporterCode;
            this.IsExternalId = menuArg.IsExternalId;
            this.IsPassport = menuArg.IsPassport;
            this.PassportNumber = menuArg.PassportNumber;
            this.PassportTypeCode = menuArg.PassportTypeCode;
            this.PassportCountryCode = menuArg.PassportCountryCode;

            //if (!AppTool.IsNullOrEmpty(this.Code) && AppTool.IsNullOrEmpty(this.PassportNumber)) {
            //    this.UIProperties.SetEnabled("Code", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("FullName", "Customs.Client", false);

            //    this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
            //}

            let customSendOptionsArgs: CustomSendOptionsArgs = new CustomSendOptionsArgs();
            customSendOptionsArgs.ForcePersonalSign = false;
            customSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceInteractive;
            this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
        }

    }

    // #region properties
    private _IsFromDeclaration: boolean = false;
    get IsFromDeclaration() { return this._IsFromDeclaration; }
    set IsFromDeclaration(value: boolean) {
        if (this._IsFromDeclaration != value) {
            this._IsFromDeclaration = value;
        }
    }

    private isPassport: boolean;
    get IsPassport() { return this.isPassport; }
    set IsPassport(value: boolean) {
        if (this.isPassport != value) {
            this.isPassport = value;
        }
    }

    get Code() { return this.clientPM.Code; }
    set Code(value: string) {
        if (this.clientPM.Code != value) {
            this.clientPM.Code = value;
        }
    }

    get PassportNumber() { return this.clientPM.PassportNumber; }
    set PassportNumber(value: string) {
        if (this.clientPM.PassportNumber != value) {
            this.clientPM.PassportNumber = value;
        }
    }
    
    get PassportTypeCode() { return this.clientPM.PassportTypeCode; }
    set PassportTypeCode(value: string) {
        if (this.clientPM.PassportTypeCode != value) {
            this.clientPM.PassportTypeCode = value;
        }
    }

    get PassportCountryCode() { return this.clientPM.PassportCountryCode; }
    set PassportCountryCode(value: string) {
        if (this.clientPM.PassportCountryCode != value) {
            this.clientPM.PassportCountryCode = value;
        }
    }

    get FullName() { return this.clientPM.FullName; }
    set FullName(value: string) {
        if (this.clientPM.FullName != value) {
            this.clientPM.FullName = value;
        }
    }


    private isExternalId: boolean;
    get IsExternalId() { return this.isExternalId; }
    set IsExternalId(value: boolean) {
        if (this.isExternalId != value) {
            this.isExternalId = value;
        }
    }

   

    //#endregion



//        public string ResponseStatusXML
//{
//    get { return GetXml(); }
//    get { return ResponseData.ResponseStatusXML; }
//    set { FirePropertyChanged("ResponseStatusXML"); }
//}
    

    //#endregion


    UseExternalId() {

        this.UIProperties.SetEnabled("Code", "Customs.Client", true);
      //  this.UIProperties.SetEnabled("FullName", "Customs.Client", true);

        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
        this.IsExternalId = true;
        this.IsPassport = false;


    }

    UsePassportRadio() {

        this.UIProperties.SetEnabled("Code", "Customs.Client", false);
       // this.UIProperties.SetEnabled("FullName", "Customs.Client", false);

        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", true);
        this.IsExternalId = false;
        this.IsPassport = true;
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);

        var errors: string[] = [];
        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.clientPM, this.ObjectTableName, errors);

        if (this.IsExternalId && AppTool.IsNullOrEmpty(this.Code)) {
            var msg = TextCodeTranslator.Translate("Customs.Client.O.CodeRequired");
            this.ValidationErrorsList.push(msg);

          
        }

        if (this.IsPassport && AppTool.IsNullOrEmpty(this.PassportNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.Client.O.PassportRequired");
            this.ValidationErrorsList.push(msg);
         
        }

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        var currRequestParams = new ClientSearchRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.LoggingEntityId = this.clientPM.Id;
        currRequestParams.LoggingEntityReference = this.PassportNumber;
             currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.Client')[0].Id;
          
             currRequestParams.ExternalId = this.Code;
             currRequestParams.PassportCountryCode = this.PassportCountryCode;
             currRequestParams.PassportNumber = this.PassportNumber;
             currRequestParams.PassportTypeCode = this.PassportTypeCode;
             currRequestParams.Tenant = this.clientPM.Tenant;
             currRequestParams.RequestName = "Client Search";
             currRequestParams.ResponseName = "Client Search";
             
             currRequestParams.Tenant = SessionLocator.Tenant;
             currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לשליפת לקוח", false)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this.clientMessagesService.PostClientRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                if (myServiceResponse.Result) {
                    var response = myServiceResponse.Result;
                    if (!response.HasException && response.Succeeded || response.CanContinue) {
                        if (response.CanContinue) {
                            var confirmWindow = new ConfirmWindow();

                            confirmWindow.Width = 400;
                            confirmWindow.Title = TextCodeTranslator.Translate("Customs.Client.O.ClientScreen");
                            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
                            confirmWindow.Height = 180;
                            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Client.O.BuildClient");
                            confirmWindow.ShowNoButton = true;
                            confirmWindow.Show(response.UserMessage);

                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    this.CreateNewClient();
                                    confirmWindow.Close();
                                }
                            });
                        }
                       
                    }
                }
            });


        }

    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (this.IsFromDeclaration) {
            if (this._MyResponseObjectToShow == null) {
                this.CurrentSession.CloseCurrentWindowEmit("");
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit(this._MyResponseObjectToShow.FullName);
            }
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }

  
    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new ClientSearchRequestParams();
        }
        if (this.ResponseData == null) {
            this.ResponseData = new ClientSearchResponseData();
        }

        this.RefreshScreen();
    }

    RefreshScreen() {
        this._MyResponseObjectToShow = null;
   //     this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
      //  this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;


        if (!AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
    }
    CreateNewClient() {
        //var item = new ClientPM();
     //   item.Tenant = SessionLocator.Tenant;
      
        var windowArgs: any = {};
        
        windowArgs.isNewClient = true;
        windowArgs.CurrentEntity = this.clientPM;
        windowArgs.IsExternalId = this.IsExternalId;
        var windowTitle = TextCodeTranslator.Translate("Customs.Client.O.BuildClient"); 
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;

        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');
   

    }

}
