import { AmitalGatewayUtil, UnifreightMessageM } from   '../../Infrastructure/Utilities/AmitalGatewayUtil';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { IEditComponentController } from '../../Infrastructure/Components/EditComponent/EditComponent';
import { VehiclePM } from '../../Customs/EntityPMs/VehiclePM';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../Infrastructure/Utilities/events/MenuButtonsEvents';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
export class VehicleEditComponentController implements IEditComponentController {
    FilterTabs(allTabs: any[]) {
         
    }
    public MustRefresh: boolean = null;
    public MustRefreshMessage: string = null;
    public IsInBatchRequest: boolean = null;

    private _CurrentEntity: VehiclePM
    private CurrentSession = SessionLocator.SelectedSession;
    private _ControllerOn: boolean = false;
    private _UnifaceExclusiveAlreadyLocked: boolean;
    OnFirstTimeAfterSingleDataLoaded(CurrentEntity): Promise<boolean> {
        this._CurrentEntity = CurrentEntity as VehiclePM;
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
            //if (!(this._CurrentEntity.IsConvertedVehicle || this._CurrentEntity.IsConnectedToUnifreight)) {
           
            this.RaiseLockIIGEntReturnEntityAlreadyLock(resolve)


        });
    }
    private RaiseLockIIGEntReturnEntityAlreadyLock(resolve) {
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(

            (myUnifreightMessageM: UnifreightMessageM) => {
                if (
                    myUnifreightMessageM.LogitudeViewModel == "VehicleEditComponentController" &&
                    (myUnifreightMessageM.LogitudeEntity == "Customs.Vehicle" || myUnifreightMessageM.LogitudeEntity == "Vehicle") &&
                    myUnifreightMessageM.LogitudeEntityNumber == this._CurrentEntity.Id) {
                    sub.unsubscribe();

                  
                    //let ResponseInstructionCancel = this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil.Instance.VehicleMessaging.ResponseInstructionCancel)
                    //if (ResponseInstructionCancel) {
                    //    this.ToCancell = true;
                    //    resolve(this._ControllerOn);
                    //    //this.CurrentSession.RealCloseCurrentEditComponent();
                    //    return;
                    //}
                    this._InDisplayModeCFIFILMLockMMessage = "";
                    let IsAlreadyLock = this.GetBoolean(myUnifreightMessageM, AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockKey)
                    if (IsAlreadyLock) {
                        this._InDisplayModeCFIFILMLockMMessage = "הרכב בשימוש במסוף אחר";
                        this._UnifaceExclusiveAlreadyLocked = this.InDisplayMode = true;
                        
                        
                        
                        

                        let responseLockMessgae = UnifreightMessageM.GetStringValue(myUnifreightMessageM, AmitalGatewayUtil.Instance.GeneralMessaging.ResponseEntityAlreadyLockMessage);
                        if (!AppTool.IsNullOrEmpty(responseLockMessgae)) {
                            this._InDisplayModeCFIFILMLockMMessage = responseLockMessgae;
                        }
                        //this.ToCancell = true;///messageAndExit

                       

                       
                        //**********to lock menu buttons and save button --- mohammad bug 30289***************************//
                        var args: MenuButtonsStateChangedEventArgs = new MenuButtonsStateChangedEventArgs();
                        args.MenuButtonsStates = {};
                        args.MenuButtonsStates["SendVehicle"] = true;
                        args.MenuButtonsStates["DeleteVehicle"] = true;
                        MenuButtonsEvents.MenuButtonsStateChanged.emit(args);

                        if (this.CurrentSession.CurrentEditComponent) {
                            this.CurrentSession.CurrentEditComponent.IsSaveBtnDisable = true;
                        }
                        //**************************************************************************//

                        this.CurrentSession.StopBusyIndicator();
                        var message: MessageWindow = new MessageWindow();
                        message.Width = 350;
                        message.Height = 180;
                        message.Title = "הרכב בשימוש במסוף אחר";
                        message.Show(this._InDisplayModeCFIFILMLockMMessage);     
                        message.WindowClosed.subscribe((res) => {
                            this.ToCancell = true;
                            resolve(this._ControllerOn);
                        });

                        //resolve(this._ControllerOn); 
                        return;
                    }
                  
                    this._UnifaceExclusiveAlreadyLocked = this.InDisplayMode = false;
                    this._InDisplayModeCFIFILMLockMMessage = "";
                    resolve(this._ControllerOn);
                }
            }
            );

        AmitalGatewayUtil.Instance.GeneralMessaging
            .RaiseLockIIGEntityReturnEntityAlreadyLock(
            "Vehicle",
            this._CurrentEntity.Id,
            "VehicleEditComponentController"
            );
    }
    OnReloadEntityPM(): Promise<any> {
        return new Promise((resolve, reject) => {
            if (!this._ControllerOn) {
                resolve();
                return;
            }
            if (!this._UnifaceExclusiveAlreadyLocked) {
                resolve();
                return;
            }
            this.RaiseLockIIGEntReturnEntityAlreadyLock(resolve);
        });
    }
    OnCloseEditControl() {
        if (!this._ControllerOn) {
            return;
        }
        if (!this._UnifaceExclusiveAlreadyLocked) 
        {
            AmitalGatewayUtil.Instance.GeneralMessaging.RaiseUnlockIIGEntity(this._CurrentEntity.Id, this._CurrentEntity.Id, this.HaveSaved);
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
    IsDisabled(itemTabCode: string): boolean {
        return false;
    }
}
