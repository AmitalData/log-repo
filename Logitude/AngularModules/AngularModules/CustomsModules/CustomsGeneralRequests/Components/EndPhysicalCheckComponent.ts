import { Component, AfterViewInit, ViewChild, Input, ChangeDetectorRef } from "@angular/core";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { PhysicalCheckPM } from "../../../Customs/EntityPMs/PhysicalCheckPM";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { IIGGeneralMessagesService } from "../../../Customs/Services/WebServices/IIGGeneralMessagesService";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { CommunicationLogStepListService } from "../../../Common/Services/ExtendedLists/CommunicationLogStepListService";
import { PhysicalCheckMenuButtonsHandler } from "../../../Customs/Components/MenuButtons/PhysicalCheckMenuButtonsHandler";
import { PhysicalCheckWebService } from "../../../Customs/Services/WebServices/PhysicalCheckWebService";
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { DateTimeFormat } from "../../../Infrastructure/Utilities/DateTimeZone";
import { DateTool } from "../../../Infrastructure/Tools";


 

@Component({
    selector: 'EndPhysicalCheckComponent',
    moduleId: module.id,
    templateUrl: './EndPhysicalCheckComponent.html',
})
export class PhysicalCheckComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {
    public DataContext: PhysicalCheckComponent = this;
    public EntityPM: PhysicalCheckPM;
    public ObjectTableName: string = "Customs.PhysicalCheck";
    public openDate: string;
    public limitDate: string;

 
    public id: string;

//    _PhysicalCheckPMService: PhysicalCheckExtendedPMService = new PhysicalCheckExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession; 
    constructor(private CD: ChangeDetectorRef) {
        super();
    }

    ngOnInit() {
 
    }


    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
            this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
            this.subscribeWrapperComponent()


 
    }
  
    OnMassageDisplayMethod() {

        if (this.MyCommunicationLogId != null) {
           // this.getData(this.MyCommunicationLogId, this.MyCustomsMenuItem.MainInterfaceCode);
        }
        /*
        this.UIProperties.SetEnabled("OpenDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("operationCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CheckId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DeclarationNo", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CheckTypeName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LimitDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("QueueTypeName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CheckSiteName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("StorageSiteName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("StatusMessageName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OperationName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ContainerNubmer", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoIdentifierKey2", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoIdentifierKey1", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoIdentifierTypeName", this.ObjectTableName, false);
        */



    }

    /*getData(id: string, InterfaceTypeCode: string) {
        this.CurrentSession.StartBusyIndicator("");
        var ary = [20, 30];
        var suppressHugeDataFeature: boolean = true;
        this._PhysicalCheckPMService.GetPhysicalCheckRequest(
            InterfaceTypeCode,
            id, SessionLocator.Tenant,
            ary,  
            suppressHugeDataFeature
        )
            .subscribe((response: any) => {
                this.EntityPM = response.Result;
                this.CurrentSession.StopBusyIndicator();
                if (this.EntityPM.OpenDate!= null) {
                    var myFormats = DateTool.GetDateFormats(this.EntityPM.OpenDate);
                    this.openDate = myFormats.DateString + " " + myFormats.ShortTimeString;
                }
                if (this.EntityPM.LimitDate!=null) {
                    var myFormats = DateTool.GetDateFormats(this.EntityPM.LimitDate);
                    this.limitDate = myFormats.DateString + " " + myFormats.ShortTimeString;
                }
              /*  if (this.CurrentSession != null && this.CurrentSession.CurrentWindow != null) {
                    setTimeout(() => {
                        this.CurrentSession.CurrentWindow.Title = "ddddddd";
                        this.CD.detectChanges();
                        console.log("CurrentSession.CurrentWindow");
                    }, 20000);
                
});
    }
    */
 
    get CheckId() { return this.EntityPM ? this.EntityPM.CheckId : null; }
    set CheckId(value: string) {
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
    get CargoTypeCode() { return this.EntityPM ? this.EntityPM.CargoTypeCode : null; }
    set CargoTypeCode(value: string) {
        if (this.EntityPM.CargoTypeCode != value) {
            this.EntityPM.CargoTypeCode = value;
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
    get CustomerName() { return this.EntityPM ? this.EntityPM.CustomerName : null; }
    set CustomerName(value: string) {
        if (this.EntityPM.CustomerName != value) {
            this.EntityPM.CustomerName = value;
        }
    }
    get CheckSiteName() { return this.EntityPM ? this.EntityPM.CheckSiteName : null; }
    set CheckSiteName(value: string) {
        if (this.EntityPM.CheckSiteName != value) {
            this.EntityPM.CheckSiteName = value;
        }
    }
    get endDate() { return this.openDate ? this.openDate : null; }
   
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {  
    }
    

}
