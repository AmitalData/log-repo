import { Component, OnInit} from'@angular/core'
import { KeyCode } from '../../../Infrastructure/DataContracts/KeyCode';
import { KeyValuePair } from '../../CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CourierMasterService } from '../../../Customs/Services/Others/CourierMasterService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
 
@Component({
    templateUrl: './CourierSendStatusComponent.html',

})
export class CourierSendStatusComponent extends BaseComponent {

    _CourierMasterService: CourierMasterService = new CourierMasterService();

    
    public DataContext: CourierSendStatusComponent = this;
    public ObjectTableName: string = "Customs.DeclarationCourierStatus";
    ///DeclarationsList: DeclarationCourierStatusPM[] = [];
    ValidationErrorsList: any[] = [];

    ngOnInit(): void {
        //throw new Error("Method not implemented.");
    }
    _SendOptionList: KeyValuePair[];

    constructor() {
        super();
        this.CourierMasterId = '1-490';
        this._SendOptionList = [];
        this._SendOptionList.push(new KeyValuePair("Nothing".toUpperCase(), "Nothing"));
        this._SendOptionList.push(new KeyValuePair("Read".toUpperCase(), "Read"));
        this._SendOptionList.push(new KeyValuePair("Update".toUpperCase(), "Update"));

    }

    private _SelectedSendOption: KeyValuePair;
    public get SelectedSendOption(): KeyValuePair {
        return this._SelectedSendOption;
    }
    public set SelectedSendOption(value: KeyValuePair) {
        this._SelectedSendOption = value;
    }


    private _CourierMasterId: string;
    public get CourierMasterId(): string {
        return this._CourierMasterId;
    }
    public set CourierMasterId(value: string) {
        this._CourierMasterId = value;
    }


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CurrentWindow.Close("");
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this._CourierMasterId)) {
            this.ValidationErrorsList.push("CourierMasterId  is must");
            return;
        }
        if (AppTool.IsNullOrEmpty(this._SelectedSendOption)) {
            this.ValidationErrorsList.push("SendOption  is must");
            return;
        }

        //http://192.116.221.103:584/Courier58/api/CourierMaster/GetSendALLDeclarationsStatusRequest?CourierMasterId=1-490
        SessionLocator.SelectedSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendALLDeclarationsStatusRequest(this._CourierMasterId, this.SelectedSendOption.Key,false)
            .subscribe((res:any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                if(!AppTool.IsNullOrEmpty(res.RequestInProgressList)){
                    myMessageWindow.ShowEventButton=true;
                    TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                }  
                myMessageWindow.Show(res.Message);
                myMessageWindow.SendEvent.subscribe(s=>{
                    if(s){
                        this.LoadCustomsRequestSheetsScreen(res.RequestInProgressList)
                    }
                });
                this.CancelButtonClicked();
            });

    }
    LoadCustomsRequestSheetsScreen(RequestInProgressList:string){
       
        var entityArgs=new EntityArgs();
        entityArgs.EntityPM = this.EntityPM;
        entityArgs.ObjectTableName="Customs.CourierMaster";
        entityArgs.OriginEntity=RequestInProgressList;
        let windowTitle = TextCodeTranslator.Translate("TextCodeTranslator");
        let logWindow = new LogitudeWindow();
        logWindow.Width = 1300;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = entityArgs;
        
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent');


 
    }
}
