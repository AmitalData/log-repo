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
import { Invoices } from "../../../Customs/EntityPMs/Extended/InvoiceQueue";
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { DeclarationPMService } from "../../../Customs/Services/StandardPMs/DeclarationPMService";
import { DeclarationPM } from "../../../Customs/EntityPMs/DeclarationPM";
import { DropdownMenuFilterComponent } from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/DropdownMenuFilterComponent';

@Component({
    selector: 'InvoiceQueueComponent',
    moduleId: module.id,
    templateUrl: './InvoiceQueueComponent.html',
    providers: [DeclarationPMService]

})
export class InvoiceQueueComponent
    extends BaseComponent{
    public DataContext:any=this;
    public InvoiceLineList: ObservableCollection;
    public IntegratedInvoiceList: ObservableCollection;
    public StatusList: ObservableCollection;
    public declaration: DeclarationPM;
    _invoiceQueueWebService: InvoiceQueueWebService = new InvoiceQueueWebService();
    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService) {
        super();
        this.InvoiceLineList = new ObservableCollection([]);
        this.IntegratedInvoiceList = new ObservableCollection([]);
        this.StatusList = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                 this._invoiceQueueWebService.GetInvoice().subscribe(data => {

                    (data.Result.Invoice as Invoices).InvoiceLines.forEach(
                        x => {
                            this.InvoiceLineList.Insert(x);
                        });

                    (data.Result.Invoice as Invoices).Statuses.forEach(
                        x => {
                            this.StatusList.Insert(x);
                        });


                    (data.Result.Invoice as Invoices).IntegratedInvoices.forEach(
                        x => {
                            this.IntegratedInvoiceList.Insert(x);
                        });


                    this._declarationPMService.get("1-5347").subscribe(
                        data => {
                            this.declaration = data.Result;
                        });
                   

                });

            });
        });
    }
    SetWindowArgs(args: any) {



    }
}
 

 

