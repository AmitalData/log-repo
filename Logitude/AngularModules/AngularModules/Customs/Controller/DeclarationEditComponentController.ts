import { AmitalGatewayUtil, UnifreightMessageM } from   '../../Infrastructure/Utilities/AmitalGatewayUtil';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { IEditComponentController } from '../../Infrastructure/Components/EditComponent/EditComponent';
import { DeclarationPM } from '../../Customs/EntityPMs/DeclarationPM';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../Infrastructure/Utilities/events/MenuButtonsEvents';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';

export class DeclarationEditComponentController implements IEditComponentController {
    FilterTabs(allTabs: any[]) {
        let currentEntity: DeclarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;
         var indexOfTab = allTabs.findIndex(t => t.Code == "DCCR");
        if (!currentEntity.IsAmendment && FeatureLocator.HasFeaturePermession("Customs.Declaration", "DECLARATIONAMENDMENT")) {
            if (indexOfTab > -1) {
                allTabs.splice(indexOfTab, 1);
            }
        }

       else if (!FeatureLocator.HasFeaturePermession("Customs.Declaration", "DECLARATIONAMENDMENT")) {
            allTabs[indexOfTab].IndexOrder = Math.max.apply(Math, allTabs.map(function (o) { return o.IndexOrder; })) + 1;
        }
       else if (currentEntity.IsAmendment && FeatureLocator.HasFeaturePermession("Customs.Declaration", "DECLARATIONAMENDMENT")) {
            allTabs[indexOfTab].IndexOrder = -1;
        }

        if (currentEntity.AmendmentDontDisplayInList) {
            var indexOfTab = allTabs.findIndex(t => t.Code == "DCDA");
            if (indexOfTab > -1) {
                allTabs.splice(indexOfTab, 1);
            }

        }
          if (currentEntity.Direction=="E") {
            var indexOfTab = allTabs.findIndex(t => t.Code == "DEIN");
             if (indexOfTab > -1) {
                 allTabs[indexOfTab].TabNameTextCodeCode = "Customs.Declaration.TH.ExporterInvoices";
             }
        

        }
         else {
             var indexOfTab = allTabs.findIndex(t => t.Code == "DEIN");
             if (indexOfTab > -1) {
                 allTabs[indexOfTab].TabNameTextCodeCode = "Customs.Declaration.TH.Invoices";
             }
               var indexOfTab = allTabs.findIndex(t => t.Code == "DCDI");
              if (indexOfTab > -1) {
                  allTabs.splice(indexOfTab, 1);

              }
         }

    }
    public MustRefresh: boolean = null;
    public MustRefreshMessage: string = null;
    public IsInBatchRequest: boolean = null;
    private CurrentSession = SessionLocator.SelectedSession;
    private _CurrentEntity: DeclarationPM

    private _ControllerOn: boolean = false;
    private _UnifaceExclusiveAlreadyLocked: boolean;    ////if true then force Check is loc in every reload
 
    OnFirstTimeAfterSingleDataLoaded(CurrentEntity): Promise<boolean> {
        this._CurrentEntity = CurrentEntity as DeclarationPM;
        this._ControllerOn = true;
        return new Promise((resolve, reject) => {
            if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                this._ControllerOn = false;
                resolve(this._ControllerOn);
                return;
            }
            //alert(SessionLocator.AllSessions.length);
            //let myEditTab: SessionTabItem = this.Tabs[1];
            if (SessionLocator.AllSessions.length == 2 &&
                SessionLocator.AllSessions[0] != this.CurrentSession) {
                //myEditTab no need to Check !!!
                this._ControllerOn = false;
                resolve(this._ControllerOn);
                return;
            }
            //if (!(this._CurrentEntity.IsConvertedDeclaration || this._CurrentEntity.IsConnectedToUnifreight)) {
            if (!(this._CurrentEntity.IsConnectedToUnifreight)) {
                this._ControllerOn = false;
                resolve(this._ControllerOn);
                return;
            }
            this.RaiseCFIFILMLockReturnCFIFILMAlreadyLock(resolve)


        });
    }
    private RaiseCFIFILMLockReturnCFIFILMAlreadyLock(resolve) {
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(

            (myUnifreightMessageM: UnifreightMessageM) => {
                if (
                    myUnifreightMessageM.LogitudeViewModel == "DeclarationEditComponentController" &&
                    (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                    myUnifreightMessageM.LogitudeEntityNumber == this._CurrentEntity.Id) {
                    sub.unsubscribe();

                    //let ResponseInstructionCancel = false;
                    //let listRes = myUnifreightMessageM.Response.filter(itm =>
                    //    itm[0] == AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseInstructionCancel);
                    //if (listRes.length > -1 && !AppTool.IsNullOrEmpty(listRes[0])) {
                    //    if (!AppTool.IsNullOrEmpty(listRes[0][1])) {
                    //        ResponseInstructionCancel = (listRes[0][1].toLowerCase() == 'true');
                    //    }
                    //}
                    let ResponseInstructionCancel = this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseInstructionCancel)
                    if (ResponseInstructionCancel) {
                        this.ToCancell = true;
                        resolve(this._ControllerOn);
                        //this.CurrentSession.RealCloseCurrentEditComponent();
                        return;
                    }
                    this._InDisplayModeCFIFILMLockMMessage = "";
                    let IsAlreadyLock = this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockKey)
                    if (IsAlreadyLock) {
                        this._InDisplayModeCFIFILMLockMMessage = "ההצהרה נעולה";
                        this._UnifaceExclusiveAlreadyLocked = this.InDisplayMode = true;
                        let responseCFIFILMAlreadyLockMessgae = UnifreightMessageM.GetStringValue(myUnifreightMessageM, AmitalGatewayUtil.Instance.DeclarationMessaging.ResponseCFIFILMAlreadyLockMessgae);
                        if (!AppTool.IsNullOrEmpty(responseCFIFILMAlreadyLockMessgae)) {
                            this._InDisplayModeCFIFILMLockMMessage = responseCFIFILMAlreadyLockMessgae;
                        }

                        resolve(this._ControllerOn);
                        //**********to lock menu buttons and save button --- mohammad bug 30289***************************//
                        var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                        args.MenuButtonsStates = {};
                        args.MenuButtonsStates["SendDeclaration"] = true;
                        MenuButtonsEvents.MenuButtonsStateChanged.emit(args);

                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                        }
                        //**************************************************************************//

                        //this.CurrentSession.RealCloseCurrentEditComponent();
                        return;
                    }
                  
                    this._UnifaceExclusiveAlreadyLocked = this.InDisplayMode = false;
                    this._InDisplayModeCFIFILMLockMMessage = "";
                    resolve(this._ControllerOn);
                }
            }
            );

        AmitalGatewayUtil.Instance.DeclarationMessaging
            .RaiseCFIFILMLockReturnCFIFILMAlreadyLock(
            this._CurrentEntity.CustomFileNo, this._CurrentEntity.Id,
            //this.GetType().Name
            "DeclarationEditComponentController"
            );
    }
    ForceCheckIfLockWhileReload() {
        this._UnifaceExclusiveAlreadyLocked = true;
    }
    OnReloadEntityPM(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (!this._ControllerOn) {
                resolve();
                return;
            }
            if (!this._UnifaceExclusiveAlreadyLocked) {  ////if true then force Check is loc in every reload
                resolve();
                return;
            }
            setTimeout(() => { this.RaiseCFIFILMLockReturnCFIFILMAlreadyLock(resolve);},500)
            
        });
    }
    OnCloseEditControl(onCallBack?: () => void) {
        if (!this._ControllerOn) {
            return;
        }
        if (
            !this._UnifaceExclusiveAlreadyLocked
            ///&& this._CurrentEntity.IsConvertedDeclaration != true /// yuval +im - not need 
        ) //Yuval Chalup 25.11.2015 TASK-17450 (Add _CurrentEntity.IsConvertedDeclaration != true)
        {
          


            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            //mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            
                            mess.LogitudeEntityNumber == this._CurrentEntity.Id &&
                            mess.UnifreightEntityNumber == this._CurrentEntity.CustomFileNo);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            if (!AppTool.IsNullOrEmpty(onCallBack)) {
                                onCallBack();
                            }



                        }
                    }
            );

            if (!AppTool.IsNullOrEmpty(onCallBack)) {
                setTimeout(() => {
                    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockCFIFILEM(this._CurrentEntity.CustomFileNo, this._CurrentEntity.Id, this.HaveSaved);

                },300);
            } else {
                AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseUnlockCFIFILEM(this._CurrentEntity.CustomFileNo, this._CurrentEntity.Id, this.HaveSaved);

            }
            

        } else {
            if (!AppTool.IsNullOrEmpty(onCallBack)) {
                onCallBack();
            }

        }

    }
    private GetBoolean(myUnifreightMessageM: UnifreightMessageM, theKey: string): boolean {
        let myBool = false;
        myBool = (UnifreightMessageM.GetStringValue(myUnifreightMessageM, theKey).toLowerCase() == 'true');
        return myBool;
    }
    HaveSaved: boolean = false;
    
    ToCancell: boolean = false;

    private _InDisplayModeCFIFILMLockMMessage: string;
    public get InDisplayModeMessage() { return this._InDisplayModeCFIFILMLockMMessage; }
    public set InDisplayModeMessage(value: string) { this._InDisplayModeCFIFILMLockMMessage = value; }
    public InDisplayMode: boolean = false;

    
    public UnifaceStartAsLock(lockMess) {
        this.InDisplayMode = true;
        this.InDisplayModeMessage = lockMess;
        this._ControllerOn = true;
        this._UnifaceExclusiveAlreadyLocked = true;
    }
    public ResetMustRefresh() {
        this.MustRefresh = null;
        this.MustRefreshMessage = null;
    }
    public CustomsAnswersShowManifest: boolean = false;
    public ShowDeclarationClassificationComponentTAB: boolean = false;

    IsDisabled(itemTabCode: string): boolean {
        if (itemTabCode != "DCCF") {
            return false; 
        }
        if (this.ShowDeclarationClassificationComponentTAB) {
            return false;
        }
        return false; 
        
    }

    private _TapagId: string;
    public get TapagId() { return this._TapagId; }
    public set TapagId(value: string) { this._TapagId = value; }

    private _CargoSplitId: string;
    public get CargoSplitId() { return this._CargoSplitId; }
    public set CargoSplitId(value: string) { this._CargoSplitId = value; }
}
