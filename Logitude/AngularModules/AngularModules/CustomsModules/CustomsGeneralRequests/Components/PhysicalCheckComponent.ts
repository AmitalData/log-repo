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
import { PhysicalCheckExtendedPMService } from "../../../Common/Services/ExtendedPMs/PhysicalCheckExtendedPMService";
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { DateTimeFormat } from "../../../Infrastructure/Utilities/DateTimeZone";
import { DateTool, AppTool } from "../../../Infrastructure/Tools";


 

@Component({
    selector: 'PhysicalCheckComponent',     
    templateUrl: './PhysicalCheckComponent.html',
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
    _IsReady: boolean = false;

    _PhysicalCheckPMService: PhysicalCheckExtendedPMService = new PhysicalCheckExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession; 
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe(response => {
            this._IsReady = true;
        });
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

  
    OnMassageDisplayMethod() {

        if (this.MyCommunicationLogId != null) {
            this.getData(this.MyCommunicationLogId, this.MyCustomsMenuItem.MainInterfaceCode);
        }
    }

    getData(id: string, InterfaceTypeCode: string) {
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
            });  
    }

    get operationCode() { return this.EntityPM ? this.EntityPM.OperationCode : null; }
    set operationCode(value: string) {  
        if (this.EntityPM.OperationCode != value) {
            this.EntityPM.OperationCode = value;
        }
    }
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
    get OperationName() { return this.EntityPM ? this.EntityPM.OperationName : null; }
    set OperationName(value: string) {
        if (this.EntityPM.OperationName != value) {
            this.EntityPM.OperationName = value;
        } 
    }
    get StatusMessageName() { return this.EntityPM ? this.EntityPM.StatusMessageName : null; }
    set StatusMessageName(value: string) {
        if (this.EntityPM.StatusMessageName != value) {
            this.EntityPM.StatusMessageName = value;
        }
    }
    get CheckSiteName() { return this.EntityPM ? this.EntityPM.CheckSiteName : null; }
    set CheckSiteName(value: string) {
        if (this.EntityPM.CheckSiteName != value) {
            this.EntityPM.CheckSiteName = value;
        }
    }
    get CheckTypeName() { return this.EntityPM ? this.EntityPM.CheckTypeName : null; }
    set CheckTypeName(value: string) {
        if (this.EntityPM.CheckTypeName != value) {
            this.EntityPM.CheckTypeName = value;
        }
    }
    get LimitDate() { return this.limitDate ? this.limitDate : null; }

    get OpenDate() { return this.openDate ? this.openDate : null; }
   
    get StorageSiteName() { return this.EntityPM ? this.EntityPM.StorageSiteName : null; }
    set StorageSiteName(value: string) {
        if (this.EntityPM.StorageSiteName != value) {
            this.EntityPM.StorageSiteName = value;
        }
    }
    public get PhysicalCheckNumberHeader() {
        if (!AppTool.IsNullOrEmpty(this.CheckId)) {
            return "בדיקה פיזית מספר" + " " + this.CheckId;
        }
        return "בדיקה פיזית";
    }
    get QueueTypeName() { return this.EntityPM ? this.EntityPM.QueueTypeName : null; }
    set QueueTypeName(value: string) {
        if (this.EntityPM.QueueTypeName != value) {
            this.EntityPM.QueueTypeName = value;
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {  
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    

}
