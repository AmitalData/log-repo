
import {Component} from '@angular/core';
import {CustomsCollateralPM} from '../../../Customs/EntityPMs/CustomsCollateralPM';
import {CustomsCollateralsConditionPM} from '../../../Customs/EntityPMs/CustomsCollateralsConditionPM';
import {LogTab} from '../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool} from '../../../Infrastructure/Tools';
import {CustomsCollateralsAnswerPM} from '../../../Customs/EntityPMs/CustomsCollateralsAnswerPM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import { CustomsCollateralPMService } from '../../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CollateralRequestParams, CustomsCollateralsAnswerParams } from '../../../Customs/DataContract/RequestParams/CollateralRequestParams';
import { INF_MSG_GenericResponseData } from '../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
declare var window: any;
import { DeclarationMessagesService } from '../../../Customs/Services/WebServices/DeclarationMessagesService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    
    templateUrl: './CustomsCollateralComponent.html',
})

export class CustomsCollateralComponent extends BaseComponent {
    public CurrentEntity: CustomsCollateralPM;
    public ObjectTableName: string = "Customs.CustomsCollateral";
    public DataContext: any = this;
    public ItemsSource: CustomsCollateralsConditionPM[];
    public AnswersTabs: LogTab[] = [];
   customsCollateralPMService: CustomsCollateralPMService = new CustomsCollateralPMService();
   public ValidationErrorsList: string[] = [];
   requestParams: CollateralRequestParams = new CollateralRequestParams();
   responseData: INF_MSG_GenericResponseData = new INF_MSG_GenericResponseData();
    declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    private CurrentSession = SessionLocator.SelectedSession;
   constructor(public entityArgs: EntityArgs) {
        super();
    }
    SetWindowArgs(args: any) {
        this.CurrentEntity = args.CurrentEntity;
     
            this.ItemsSource = [];

            if (this.CurrentEntity.IsClosed) {
                this.IsClosedCollateral = true;
                this.IsClosedButtonEnabled = false;
                this.IsUnClosedButtonEnabled = true;
            }
            else {
                this.IsClosedCollateral = false;
                this.IsClosedButtonEnabled = true;
                this.IsUnClosedButtonEnabled = false;

            }
            this.BuildConditionsList();
            this.BuildAnswersTabs();
            this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    BuildConditionsList() {
        this.ItemsSource = [];

        for (var item of this.CurrentEntity.CustomsCollateralsConditions) {

            this.ItemsSource.push(item);
            this.Total = this.Total + item.RequestedAmount;
            this.GridHeight += 25;
        }




    }

    answerIndex: number;
  

    BuildAnswersTabs() {

        var tab;
        this.AnswersTabs = [];
       
        if (this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {

            var items: CustomsCollateralsAnswerPM[] = this.CurrentEntity.CustomsCollateralsAnswers.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });

            for (let item of items) {

                tab = new LogTab();
                tab.EntityPM = item;
                tab.Code = item.LineNumber;
                tab.Header = item.LineNumber;
                tab.Parent = this.CurrentEntity;
              tab.ComponentPath = "./CustomsModules/CustomsCollateral/Components/CustomsCollateralAnswerComponent";
                this.AnswersTabs.push(tab);
            }
        }
        else {
            this.AddAnswer(null);
        }
   
        this.SelectedTab = this.AnswersTabs[0];
    }

    AddAnswer(event) {
     

        this.answerIndex = 0;

        if (this.AnswersTabs.length > 0){
            var maxObj = this.CurrentEntity.CustomsCollateralsAnswers.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current });
            if (maxObj != null) {
                if (this.answerIndex <= maxObj.LineNumber)
                    this.answerIndex = maxObj.LineNumber;
            }
        }
        

        var answer: CustomsCollateralsAnswerPM = new CustomsCollateralsAnswerPM(this.CurrentEntity);
        answer.CustomsCollateralId = this.CurrentEntity.Id,
            answer.Tenant = this.CurrentEntity.Tenant;
        answer.LineNumber = this.answerIndex + 1;
        this.CurrentEntity.AddCustomsCollateralsAnswer(answer);

        // new tab
        var tab = new LogTab();
        tab.EntityPM = answer;
        tab.Code = answer.LineNumber.toString();
        tab.Header = answer.LineNumber.toString();
        tab.Parent = this.CurrentEntity;
      tab.ComponentPath = "./CustomsModules/CustomsCollateral/Components/CustomsCollateralAnswerComponent";
        this.AnswersTabs.push(tab);

        // select the tab
        this.SelectedTab = tab;
    }
    DeleteAnswer(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {

            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateralAnswer");
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
            var t = tab;
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) { // YES
                    tab = t;
                    var index = this.AnswersTabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab); return;
                    }
                    this.CurrentEntity.RemoveCustomsCollateralsAnswer(tab.EntityPM);
                    this.AnswersTabs.splice(index, 1);


                    for (var i = 0; i < this.CurrentEntity.CustomsCollateralsAnswers.length; i++) {
                        var answer = this.CurrentEntity.CustomsCollateralsAnswers[i];
                        answer.LineNumber = i + 1;
                      
                    }
                    for (var i = 0; i < this.AnswersTabs.length; i++) {
                        var collateralAnswer: CustomsCollateralsAnswerPM = this.AnswersTabs[i].EntityPM;
                        collateralAnswer.LineNumber = i + 1;
                        this.AnswersTabs[i].Code = collateralAnswer.LineNumber.toString();
                        this.AnswersTabs[i].Header = collateralAnswer.LineNumber.toString();
                    }

                    // select the last tab
                    var tab = this.AnswersTabs[0];
                    this.SelectedTab = tab;
                }
            });

        }
    }

    OnSelectedChanged(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
        
        }
    }


    //#region Properties

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }
    public GridHeight = 55;

    public Total: number=0;

    public get FileNo() { return this.CurrentEntity.FileNo; }

    public get WorkerName() { return this.CurrentEntity.WorkerName; }

    public get EntityIdKey1() { return this.CurrentEntity.EntityIdKey1; }

    public get CollateralRequestNumber() { return this.CurrentEntity.CollateralRequestNumber; }

    public get RequestedCollateralTypeName() { return this.CurrentEntity.RequestedCollateralTypeName; }


    public get CollateralRequestStatusName() { return this.CurrentEntity.CollateralRequestStatusName; }

    public get RequestValidityDate() { return this.CurrentEntity.RequestValidityDate; }

    public get CollateralValidityDate() { return this.CurrentEntity.CollateralValidityDate; }

    public get IncludingThirdPartyGuarantee() { return this.CurrentEntity.IncludingThirdPartyGuarantee; }

    public get Remarks() { return this.CurrentEntity.Remarks; }

    public IsClosedCollateral: boolean;
    public IsClosedButtonEnabled: boolean = true;
    public IsUnClosedButtonEnabled: boolean = true;

    //#endregion



    CloseClicked() {
        this.CurrentEntity.IsClosed = true;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CloseCollateral"));

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
             
                this.customsCollateralPMService.update(this.CurrentEntity).subscribe((response:any) => {
                    var result = response.Result;
                    this.CurrentSession.CollateralAnswerRefreshEvent.emit({ IsClosed: this.CurrentEntity.IsClosed  });
            this.IsUnClosedButtonEnabled = true;
            this.IsClosedButtonEnabled = false;
            this.IsClosedCollateral = true;

        });
               
               
            }

        });

     



    }
    
    UnCloseClicked() {
      
        this.CurrentEntity.IsClosed = false;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsCollateral.O.ReOpenCollateral"));

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
            
             
                this.customsCollateralPMService.update(this.CurrentEntity).subscribe((response:any) => {
                    var result = response.Result;
                    this.CurrentSession.CollateralAnswerRefreshEvent.emit({ IsClosed: this.CurrentEntity.IsClosed });
                    this.IsUnClosedButtonEnabled = false;
                    this.IsClosedButtonEnabled = true;
                    this.IsClosedCollateral = false;

                });
               

            }

        });

    }

    FIELD_IS_REQUIERD: string;
    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);

        var errors: string[] = [];
        this.ValidationErrorsList = [];

        for (var item  of this.CurrentEntity.CustomsCollateralsAnswers)
        {
            if (!item.NewFileRequest) {
                if (AppTool.IsNullOrEmpty(item.AnswerEntityTypeCode)) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.AnswerEntityTypeCode"));
                }

                if (item.AllocatedAmount == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.AllocatedAmount"));

                }

                if (item.CustomsTapgFile == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.CustomsTapgFile"));
                }
            }

            else {
                var sum= ArrayTool.Sum(item.CollateralsRequestFileConds, "RequestedAmount");
                if (item.RequestFileAmount !=sum) {
                    errors.push("הסכום שהזנת שונה מסכום הדרישה יש לשנות במסך מענה לבטוחה");
                  
                }
                if (item.RequestFileAmount == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.RequestFileAmount"));
                }

                if (item.RequestFileTypeCode == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.RequestFileTypeCode"));
                }
            }
        }

        if (this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {
            var nullVM = this.CurrentEntity.CustomsCollateralsAnswers.filter(vm => vm.IsClosed != true);
            if (nullVM.length == 0) {
                this.ValidationErrorsList.push("כל המענים סגורים- לא ניתן לבצע שליחה");
                return;
            }
        }

        // this.ValidationErrorsList = errors;
        if (errors.length == 0) {

            this.customsCollateralPMService.update(this.CurrentEntity).subscribe((response:any) => {
       
       
            if (this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {
                this.CurrentSession.StartBusyIndicator("");
                var answerParams: CustomsCollateralsAnswerParams[] = [];

                for (var tab of this.CurrentEntity.CustomsCollateralsAnswers) {
                    var answerParam = new CustomsCollateralsAnswerParams();

                    answerParam.AllocatedAmount = tab.AllocatedAmount;
                    answerParam.AnswerEntityTypeCode = tab.AnswerEntityTypeCode;
                    answerParam.AnswerForCollateralStatusCode = tab.AnswerForCollateralStatusCode;
                    answerParam.CustomsCollateralId = this.CurrentEntity.Id;
                    answerParam.CustomsNumeral = tab.CustomsNumeral;
                    answerParam.CustomsTapgFile = tab.CustomsTapgFile;
                    answerParam.LineNumber = tab.LineNumber;
                    answerParam.Tenant = tab.Tenant;
                    answerParam.Remarks = tab.Remarks;
                    answerParams.push(new CustomsCollateralsAnswerParams());
                }
                var LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0].Id;
                var requestParams: CollateralRequestParams = new CollateralRequestParams();

                requestParams.LoggingEnabled = true;
                requestParams.LoggingUserId = SessionLocator.LoggedUserId;
                requestParams.CustomCollateralId = this.CurrentEntity.Id;
                requestParams.CustomsCollateralsAnswers = answerParams;
                requestParams.Tenant = this.CurrentEntity.Tenant;
                requestParams.RequestName = "Send Collateral Request";
                requestParams.ResponseName = "Send Collateral Response";
                requestParams.LoggingEntityId = this.CurrentEntity.Id;
                LoggingObjectTableId = LoggingObjectTableId;

                CustomMessageProgressComponent
                    .ShowProgressBar(requestParams.PBId,
                    "שליחת מענה לדרישת בטוחה", true)
                    .then((res) => {
                        this.responseData = res;
                        this.OnMassageDisplayMethod();
                    }
                    ).catch((err) => {
                        //this.ValidationErrorsList = [];
                        //     this.ValidationErrorsList.push(err);
                    });


                this.declarationMessagesService.PostSendCollateralAnswers(requestParams)
                    .subscribe((response: ServiceResponse) => {
                        if (response) {                          
                            if (!response.HasError) {
                                if (response.Result.Succeeded) {
                                    this.customsCollateralPMService.get(this.CurrentEntity.Id).subscribe((response: ServiceResponse) => {
                                        if (response) {
                                            if (!response.HasError) {
                                                this.CurrentEntity = response.Result;
                                                this.BuildAnswersTabs();
                                            }
                                        }

                                    });
                                   
                                }
                            }
                        }
                    });


            }
            else {
                var messageWindow = new MessageWindow();
                messageWindow.Width = 450;
                messageWindow.Height = 190;
             
                messageWindow.Show("Add at least an answer to send.");
               
            }

          

            });


         

        }

        else {
            this.ValidationErrorsList = errors;
        }



    }

    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new CollateralRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData();
        }
    }


    CancelButtonClicked() {

       
        this.CurrentSession.CloseCurrentWindow();
    }
 


    ViewDocumentsComponent() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.CurrentEntity;
        windowArgs.ObjectTableName = this.ObjectTableName;

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

}
