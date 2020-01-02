import { Component, AfterViewInit, ViewChild } from "@angular/core";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { PhysicalCheckPM } from "../../../Customs/EntityPMs/PhysicalCheckPM";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { IIGGeneralMessagesService } from "../../../Customs/Services/WebServices/IIGGeneralMessagesService";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { CommunicationLogStepListService } from "../../../Common/Services/ExtendedLists/CommunicationLogStepListService";
import { PhysicalCheckMenuButtonsHandler } from "../../../Customs/Components/MenuButtons/PhysicalCheckMenuButtonsHandler";
import { PhysicalCheckWebService } from "../../../Customs/Services/WebServices/PhysicalCheckWebService";


@Component({
    selector: 'PhysicalCheckComponent',
    moduleId: module.id,
    templateUrl: './PhysicalCheckComponent.html',
})
export class PhysicalCheckComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: PhysicalCheckComponent = this;
    public EntityPM: PhysicalCheckPM;   
    public ObjectTableName: string = "Customs.PhysicalCheck"
    public RequestDescription: string = "בדיקה פיזית חדשה 2094775";
    public id: string;

    _CommunicationLogStepListService: CommunicationLogStepListService = new CommunicationLogStepListService();
    private physicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService;
 
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }
    OnMassageDisplayMethod() {
       
        if (this.MyCommunicationLogId != null) { 
            this.showModal(this.MyCommunicationLogId, this.MyCustomsMenuItem.MainInterfaceCode, this.RequestDescription);
             
        }

        
    }

    showModal(id: string, InterfaceTypeCode: string, RequestDescription: string) {
        let reqJson = "";
        let resJson = "";
        this.CurrentSession.StartBusyIndicator("");
        var ary = [20, 30];
        var suppressHugeDataFeature: boolean = true;
        debugger;
        this._CommunicationLogStepListService.GetCommunicationLogStepsDocumentDataBystringStepFilter(
            InterfaceTypeCode,
            id, SessionLocator.Tenant,
            ary,
            suppressHugeDataFeature
        )
            .subscribe((response: any) => {
                var myData = response.Result;
                var req: any = myData[0];
                var res: any = myData[1];
                reqJson = req.DocumentData;

                resJson = res.DocumentData;
                this.CurrentSession.StopBusyIndicator();

                
            });


    }

    get operationCode() { return this.ResponseData ? this.ResponseData.operationCode : null; }
    set operationCode(value: string) {
        if (this.ResponseData.operationCode != value) {
            this.ResponseData.operationCode = value;
        }
    }
    get checkId() { return this.ResponseData ? this.ResponseData.checkId : null; }
    set checkId(value: string) {
        if (this.ResponseData.checkId != value) {
            this.ResponseData.checkId = value;
        }
    }
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {  
    }
    

}
