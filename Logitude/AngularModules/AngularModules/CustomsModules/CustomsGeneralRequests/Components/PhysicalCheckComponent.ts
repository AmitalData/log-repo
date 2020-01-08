import { Component, AfterViewInit, ViewChild, Input } from "@angular/core";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { PhysicalCheckPM } from "../../../Customs/EntityPMs/PhysicalCheckPM";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { IIGGeneralMessagesService } from "../../../Customs/Services/WebServices/IIGGeneralMessagesService";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { CommunicationLogStepListService } from "../../../Common/Services/ExtendedLists/CommunicationLogStepListService";
import { PhysicalCheckMenuButtonsHandler } from "../../../Customs/Components/MenuButtons/PhysicalCheckMenuButtonsHandler";
import { PhysicalCheckWebService } from "../../../Customs/Services/WebServices/PhysicalCheckWebService";
import { PhysicalCheckExtendedPMService } from "../../../Common/Services/ExtendedPMs/PhysicalCheckExtendedPMService";


 

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
    public ObjectTableName: string = "Customs.Declaration";
 
    public id: string;

    _PhysicalCheckPMService: PhysicalCheckExtendedPMService = new PhysicalCheckExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession; 
    constructor() {
        super();
    }

   // ngOnInit() {
      //  this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsDocument").subscribe(response => {
   //         

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }
  
    OnMassageDisplayMethod() {
       
    if (this.MyCommunicationLogId != null) { 
            this.showModal(this.MyCommunicationLogId, this.MyCustomsMenuItem.MainInterfaceCode);
        }
        this.UIProperties.SetRequired("checkId", this.ObjectTableName, true);

     
        
    }

    showModal(id: string, InterfaceTypeCode: string) {
        let reqJson = "";
        let resJson = "";
        this.CurrentSession.StartBusyIndicator("");
        var ary = [20, 30];
        var suppressHugeDataFeature: boolean = true;
        debugger;
        this._PhysicalCheckPMService.GetPhysicalCheckRequest(
            InterfaceTypeCode,
            id, SessionLocator.Tenant,
            ary,
            suppressHugeDataFeature
        )
            .subscribe((response: any) => {
                this.EntityPM = response.Result;
                debugger;
                this.CurrentSession.StopBusyIndicator();

                
            });


    }

    get operationCode() { return this.EntityPM ? this.EntityPM.OperationCode : null; }
    set operationCode(value: string) {
        if (this.EntityPM.OperationCode != value) {
            this.EntityPM.OperationCode = value;
        }
    }
    get checkId() { return this.EntityPM ? this.EntityPM.CheckId : null; }
    set checkId(value: string) { 
        if (this.EntityPM.CheckId != value) {
            this.EntityPM.CheckId = value; 
        }
    }
    get CustomFileNo() { return this.EntityPM ? this.EntityPM.CustomFileNo : null; }
    set CustomFileNo(value: string) {
        if (this.EntityPM.CustomFileNo != value) {
            this.EntityPM.CustomFileNo = value;
        }
    } 
    get DeclarationNo() { return this.EntityPM ? this.EntityPM.DeclarationId : null; }
    set DeclarationNo(value: string) {
        if (this.EntityPM.DeclarationId != value) {
            this.EntityPM.DeclarationId = value;
        }  
    }
    get CargoIdentifierKey1() { return this.EntityPM ? this.EntityPM.CargoIdentifierKey1 : null; }
    set CargoIdentifierKey1(value: string) {
        if (this.EntityPM.CargoIdentifierKey1 != value) {
            this.EntityPM.CargoIdentifierKey1 = value;
        }
    } 
    get CargoIdentifierKey2() { return this.EntityPM ? this.EntityPM.CargoIdentifierKey2 : null; }
    set CargoIdentifierKey2(value: string) {
        if (this.EntityPM.CargoIdentifierKey2 != value) {
            this.EntityPM.CargoIdentifierKey2 = value;
        }
    }
    get CargoIdentifierTypeName() { return this.EntityPM ? this.EntityPM.CargoIdentifierTypeName : null; }
    set CargoIdentifierTypeName(value: string) {
        if (this.EntityPM.CargoIdentifierTypeName != value) {
            this.EntityPM.CargoIdentifierTypeName = value;
        }
    }
    get ContainerNubmer() { return this.EntityPM ? this.EntityPM.ContainerNubmer : null; }
    set ContainerNubmer(value: string) {
        if (this.EntityPM.ContainerNubmer != value) {
            this.EntityPM.ContainerNubmer = value;
        }
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {  
    }
    

}
