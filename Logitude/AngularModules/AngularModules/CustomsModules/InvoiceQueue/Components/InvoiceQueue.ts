import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { BaseComponent } from "../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
 import { DeclarationRemarks } from "../../../Customs/EntityPMs/Extended/DeclarationRemarks";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { ObservableCollection } from "../../../Infrastructure/Utilities/ObservableCollection";
import { DateTool } from "../../../Infrastructure/Tools";
import { InvoiceQueueWebService } from "../../../Customs/Services/WebServices/InvoiceQueueWebService";

@Component({
    selector: 'InvoiceQueue',
    moduleId: module.id,
    templateUrl: './InvoiceQueue.html',
})
export class InvoiceQueue
    extends BaseComponent{
    public DataContext:any=this;
    public InvoiceQueueList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    private Entity: DeclarationRemarks[] = [];

    _invoiceQueueWebService: InvoiceQueueWebService = new InvoiceQueueWebService();
    constructor() {
        super();
        this.InvoiceQueueList = new ObservableCollection([]);
    }
    SetWindowArgs(args: any) {



    }
}
 

 

