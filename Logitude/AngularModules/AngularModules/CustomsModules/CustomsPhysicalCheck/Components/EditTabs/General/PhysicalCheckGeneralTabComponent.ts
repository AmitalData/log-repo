declare var window: any;
import { Component, Input, AfterContentInit, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DateTool, AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PhysicalCheckPM } from '../../../../../Customs/EntityPMs/PhysicalCheckPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { PaymentOrderConnectionTableExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogDatePickerComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/LogDatePickerComponent';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { CH_NG_191_MSG2_ChangingTimeRequestParams } from    '../../../../../Customs/DataContract/RequestParams/CH_NG_191_MSG2_ChangingTimeRequestParams';
import { SendRequestVIA } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CH_NG_192_MSG3_ApproveChangeTimeResponseData } from '../../../../../Customs/DataContract/ResponseData/CH_NG_192_MSG3_ApproveChangeTimeResponseData';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector:'PhysicalCheckAvailableTimes',
    
    templateUrl: './PhysicalCheckGeneralTabComponent.html',
})

export class PhysicalCheckGeneralTabComponent
    extends BaseComponent
    implements AfterViewInit, AfterContentInit{
    public DataContext: PhysicalCheckGeneralTabComponent = this;
    public EntityPM: PhysicalCheckPM;
    public ObjectTableName: string = "Customs.PhysicalCheck";

    private currentEditComponentId: string;

    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private paymentOrderConnectionTableExtendedPMService: PaymentOrderConnectionTableExtendedPMService = new PaymentOrderConnectionTableExtendedPMService;
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService();

    public AskForAnEarlierDate: boolean = false;
    public AskForAnLaterDate: boolean = false;
    public AvailableTimeChecked: boolean = false;
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    private _FromDate: Date;
    get FromDate() { return this._FromDate; }
    set FromDate(value: Date) {
        this._FromDate = value;
        if (value) {
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
        }
    }
    private _ToDate: Date;
    get ToDate() { return this._ToDate; }
    set ToDate(value: Date) {

        this._ToDate = value;
        if (value) {
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
        }
    }

    
    public XrayItems: XRayAvailableItem[]=[];

    ValidationErrorsList: string[] = [];
    public get ErrorsList() { return this.ValidationErrorsList; }
    public set ErrorsList(val: string[]) {
        this.ValidationErrorsList = val;
    }


    SelectedDateTime: Date;
    IsSpotlightMode: boolean = false;


    _InputParam: EntityArgs;
    @Input()
    set PhysicalCheckParam(val: EntityArgs) {
        this.entityArgs = this._InputParam = val;
        this.Init();
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe((response:any) => {
                this.Init();
                //this.EntityPM = this.entityArgs.EntityPM;
                //this.ObjectTableName = this.entityArgs.ObjectTableName;
                ////this.ToDate = this.FromDate = new Date();
                //this.Listen();
                               
            });
        });
       

    }

    Init() {
        if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) return;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
    }

    ngAfterViewInit() {
        // viewChildren is set
       // this.SetByAvailableTimeChecked();
    }
    ngAfterContentInit()
    {
        this.FromDate = DateTool.GetDateByDay(+0);
        this.ToDate = DateTool.GetDateByDay(+7);
        this.SetByAvailableTimeChecked()
        //alert(this.AllDates);
        //this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.BuildAccountingCustomFilesList();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            //this.DisplayOnlyCheck();
                        }
                    }
                })
            );

            // todo: BringQueueForwardIndicatorStatus=004  יש להציג הודעה “הקדמת תור הועברה לבחינה של עובד מכס”
            this.ShowAlertBringQueueForwardIndicatorStatus();
        }
    }

    ShowAlertBringQueueForwardIndicatorStatus() {
        debugger
        // if(this.EntityPM.BringQueueForwardIndicatorStatus== "4"){
        //     var msg= "יש להציג הודעה “הקדמת תור הועברה לבחינה של עובד מכס";
        // }
    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }
    SetWindowArgs(winArg) {
        this.EntityPM = winArg.EntityPM;
        //this.EntityPM = winArg.declarationPM;
    }
    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    SetByAskForAnEarlierDate() {
        this.ResetAll();
        this.AskForAnEarlierDate = true;
        
        
    }
    SetByAskForAnLaterDate() {
        this.ResetAll();
        this.AskForAnLaterDate = true;
        
        
    }
    SetByAvailableTimeChecked() {
        this.ResetAll();
        this.AvailableTimeChecked = true;
        this.SetDateEnable(true);
        this.FromDate = this.FromDate;
        this.ToDate = this.ToDate;
    }
    ResetAll() {
        this.AskForAnEarlierDate = this.AskForAnLaterDate = this.AvailableTimeChecked = false;
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
        this.SetDateEnable(false);
        
    }

    checkEarlierDateFeature() {
        debugger
        return FeatureLocator.HasFeaturePermession("PhysicalCheck", "EarlierDateFeature");
    }

    SetEarlierDateFieldsEnable(enable: boolean) {
        this.UIProperties.SetEnabled("RequestToAdvanceAQueue", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled(" RequestDetails", this.ObjectTableName, enable);
        debugger
    }
    
    SetDateEnable(enable: boolean) {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, enable);
        //let from: LogDatePickerComponent = this.AllDates[0];
        //from.IsDisabled = !enable;

        //let to: LogDatePickerComponent = this.AllDates[1];
        //to.IsDisabled = !enable;

    }
    _ChooseDate: boolean = false;
    Choose(itemDate: Date) {
        this.SelectedDateTime = itemDate;
        this._ChooseDate= true;
        this.SendCheckRequest();
    }
    public get CheckId() { return this.EntityPM.CheckId; }
    public get CheckSiteCode() { return this.EntityPM.CheckSiteCode; }
    public get QueueTypeCode() { return this.EntityPM.QueueTypeCode; }
    public get CargoIdentifierKey1() { return this.EntityPM.CargoIdentifierKey1; }
    public get CargoIdentifierKey2() { return this.EntityPM.CargoIdentifierKey2; }
    public get CargoIdentifierKey3() { return this.EntityPM.CargoIdentifierKey3; }
    public get CargoIdentifierTypeCode() { return this.EntityPM.CargoIdentifierTypeCode; }
    public get CargoIdentifierTypeName() { return this.EntityPM.CargoIdentifierTypeName; }
    
    RequestStatusMessage: string;
    private SendCheckRequest() {
        this.RequestStatusMessage = null;
        this.ErrorsList = [];
        this.ValidateRequest();

        if (this.ErrorsList.length == 0) {
            this.SendRequest();

        }

    }
    ResponseData: CH_NG_192_MSG3_ApproveChangeTimeResponseData;
    public SendRequest() {


        let requestTypeparam: string = "";

        let checkParams = new CH_NG_191_MSG2_ChangingTimeRequestParams();
        if (this._ChooseDate) {
            requestTypeparam = "2";
            checkParams.QueueDate = this.SelectedDateTime;
            checkParams.QueueDateSpecified = true;
            //case 2: // In case of approval / deny of a requested date
        } else {
            if (this.AvailableTimeChecked) {
                requestTypeparam = "1";
                //case 1: // In .Case of list of available dates & times 
                checkParams.DateSearchFrom = this.FromDate;
                checkParams.DateSearchTo = this.ToDate;
                checkParams.DateSearchFromSpecified = true;
                checkParams.DateSearchToSpecified = true;
            } else {
                //if (this.AskForAnEarlierDate) {
                    
                //    this.bringQueueForwardIndicator = true
                //}
                requestTypeparam = "3";
                //case 3: // In Case of automatic update
            }
        }
        let objecttable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == "Customs.PhysicalCheck")[0];






        checkParams.RequestType = requestTypeparam;

        checkParams.PhysicalCheckId = this.EntityPM.Id;
        checkParams.Tenant = SessionLocator.Tenant;
        checkParams.LoggingEnabled = true;
        checkParams.LoggingEntityId = this.EntityPM.Id;
        if (this.AskForAnEarlierDate) {
            checkParams.BringQueueForwardIndicator = true;
        }
        checkParams.LoggingObjectTableId = objecttable.Id;
        checkParams.LoggingUserId = SessionLocator.LoggedUserId;
        checkParams.RequestName = "Changing Time Request";
        checkParams.ResponseName = "Approve Changing Time";
        checkParams.CheckTypeCode = this.EntityPM.CheckTypeCode;



        //if (sendOption == null) {
        checkParams.RequestVIA = SendRequestVIA.Default;
        //}
        //else if (sendOption == "WI") {
        //    checkParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

        //}
        //else if (sendOption == "WB") {
        //    checkParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //}
        //else if (sendOption == "D") {
        //    checkParams.RequestVIA = SendRequestVIA.DCABatch;
        //}

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,checkParams.PBId, "שליחת בקשה- בדיקה פיזית", true)
            .then((res) => {
                this.ResponseData = res;
                this.AnalyzeResponseMessage(this.ResponseData);

            }
            ).catch((err) => {
                this.ErrorsList.push(err);

            });

        this._IIGGeneralMessagesService.PostChangingTimeRequestParams(checkParams)
            .subscribe(() => { }
            );


        //customServiceReference.SendCheckRequestCompleted += customServiceReference_SendCheckRequestCompleted;
        //customServiceReference.SendCheckRequestAsync(bytearray);
        //SelectedTest2Index = 0;
        //SelectedTestIndex = 0;
        //SelectedTest = null;
    }
    PiscalCheckItems: Date[];
    private AnalyzeResponseMessage(customCheckData: CH_NG_192_MSG3_ApproveChangeTimeResponseData): string {
        let userMessage = "";
        if (!customCheckData.HasException) {
            let RequestStatusMessage = "";
            //switch (requestTypeIndex) {
            //    case 0:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.AutomaticDateMessage";
            //            break;
            //        }
            //    case 1:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.AvailableTimesMessage";
            //            break;
            //        }
            //    case 2:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.ChooseDateMessage";
            //            break;
            //        }
            //}
            //userMessage = TextCodeTranslator.Translate(RequestStatusMessage);
            this.PiscalCheckItems = customCheckData.PiscalCheckItems;
            customCheckData.XrayItems.forEach(
                (date) => {
                    this.XrayItems.push(date);
                }
            );
            if (!AppTool.IsNullOrEmpty(customCheckData.XrayItems)) {
                if (customCheckData.XrayItems.length > 0)
                {
                    try {
                        if (CustomMessageProgressComponent.CurrCustomMessageProgressHelper) {
                            CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
                        }
                    } catch (err) {

                    }
                }
            }

            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
            //myEvent.Publish(new RefreshScreenEventArgs("") { ScreenCode = "PhysicalCheckSpotlight" });
        }
        else {
            /*ValidationResult error = new ValidationResult(customCheckData.UserMessage);
            ErrorsList.Add(error);
            FillErrors(ErrorsList);*/
            userMessage = customCheckData.UserMessage;
            if (AppTool.IsNullOrEmpty(userMessage)) {
                userMessage = "שליחה נכשלה";
            }
        }



        //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = false, });
        if (this.IsSpotlightMode) {
            //spotlightSaveCompletedEvent.Publish(new SpotlightSaveCompletedEventArgs() { entityId = entityPM.Id });
        }
        //FirePropertyChanged("XrayItems");

        return userMessage;
    }

    private ValidateRequest() {


        if (this.AvailableTimeChecked) {
            if (this._ChooseDate) {
                if (this.SelectedDateTime == null) {
                    this.ErrorsList.push(TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
                }
            } 
        }
        if (AppTool.IsNullOrEmpty(this.CheckId)) {
            this.ErrorsList.push(TextCodeTranslator.Translate("Customs.PhysicalCheck.O.CheckIdRequierd"));
        }
        if (AppTool.IsNullOrEmpty(this.CheckSiteCode)) {
            this.ErrorsList.push(TextCodeTranslator.Translate("Customs.PhysicalCheck.O.CheckSiteRequierd"));
        }

    }
    Validate1stRequest() {
        if (this.FromDate == null) {
            this.ErrorsList.push(TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
        } else if (this.ToDate == null) {
            this.ErrorsList.push(TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
        }

    }
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        this._ChooseDate = false;
        this.XrayItems = [];
        this.ErrorsList = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //errors.forEach((err) => { this.ValidationErrorsList.push(err); });
        this.Validate1stRequest();
        if (this.ErrorsList.length > 0) {
            return;
        }
        this.SendRequest();
        //if (this.MorningMessageObservableList.Length > 0) {
        //    this.MorningMessageObservableList.Clear();
        //}



        





        
    }


}
export class XRayAvailableItem {

}
