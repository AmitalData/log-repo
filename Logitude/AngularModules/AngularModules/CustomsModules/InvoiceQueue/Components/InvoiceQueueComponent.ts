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
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'InvoiceQueueComponent',
    templateUrl: './InvoiceQueueComponent.html',
    providers: [DeclarationPMService]

})
export class InvoiceQueueComponent
    extends BaseComponent {
    public DataContext: any = this;
    public InvoiceLineList: ObservableCollection;
    public IntegratedInvoiceList: ObservableCollection;
    public StatusList: ObservableCollection;
    public declaration: DeclarationPM;
    _invoiceQueueWebService: InvoiceQueueWebService = new InvoiceQueueWebService();
    RowIndex: any;
    UnifreightMessage: any;
    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService) {
        super();
        this.InvoiceLineList = new ObservableCollection([]);
        this.IntegratedInvoiceList = new ObservableCollection([]);
        this.StatusList = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
               // this.GetData();

            });
        });
    }
    private GetData() {
        this._declarationPMService.get(this.UnifreightMessage.LogitudeEntityNumber).subscribe(data => {
            this.declaration = data.Result;
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (this.declaration==null) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show("declaration NOT FOUND");
            }
            this._invoiceQueueWebService.GetInvoice(this.declaration.Tenant, this.declaration.CustomFileNo).subscribe(data => {
                
                (data.Result.Invoice as Invoices).InvoiceLines.forEach(x => {
                    this.InvoiceLineList.Insert(x);
                });
                (data.Result.Invoice as Invoices).Statuses.forEach(x => {
                    this.StatusList.Insert(x);
                });
                (data.Result.Invoice as Invoices).IntegratedInvoices.forEach(x => {
                    this.IntegratedInvoiceList.Insert(x);
                });

            });
        });
    }

    SetWindowArgs(args: any) {
        //var json = '{"UnifreightEntity"  :  "CFIFILEM" , "UnifreightEntityNumber"  :  "3000028" , "LogitudeEntity"  :  "Customs.Declaration" , "LogitudeEntityNumber"  :  "1-211622" , "LogitudeViewModel"  :  "UnifreightMassageHandler" , "LogitudeCommandId"  :  "CreateInvoiceCommand" , "formtitle"  :  "הצהרת יבוא"}';

        this.UnifreightMessage = args.unifreightMessage;
        this.GetData();
    }


    public Run(args: any) {
        this.RowIndex = args['RowIndex'];

    }


    ShowDisbursement() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowDisbursement";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclaration.Id &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
                            this.GetData();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclaration.CustomFileNo, myDeclaration.Id,
                        myViewModelName);

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowDisbursementList",
                "CFIHMAIN.LogitudeTask",
                "ShowDisbursement",
                unifreightMessageM,
                " הצגת מסך : בילינג");
        }
        else {
            alert("ShowDisbursement");
        }
    }


    ShowPayments() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowPayments";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclaration.Id &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
                            this.GetData();
                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclaration.CustomFileNo, myDeclaration.Id,
                        myViewModelName);

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowPaymentsList",
                "CFIHMAIN.LogitudeTask",
                "ShowPayments",
                unifreightMessageM,
                " הצגת מסך : רשימת הוצאות");
        }
        else {
            alert("ShowPayments");
        }
    }

    ShowCustomFileOPCFromDeclaration() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "FieldTemplateComponent.ts-ShowCustomFileOPCFromDeclaration";
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclaration.Id &&
                            mess.LogitudeViewModel == myViewModelName);
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
                            this.GetData();

                        }
                    }
                );

            SessionLocator.SelectedSession.StartBusyIndicator("");
            var unifreightMessageM =
                AmitalGatewayUtil.Instance.
                    DeclarationMessaging.GetMessage(myDeclaration.CustomFileNo, myDeclaration.Id,
                        myViewModelName);


            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowCustomFileOPCFromDeclarationList",
                "CFIHMAIN.LogitudeTask",
                "ShowCustomFileOPCFromDeclaration",
                unifreightMessageM,
                " הצגת OPC תיק עמילות מכס");

        }
        else {
            alert("ShowCustomFileOPCFromDeclaration");
        }

    }


    CreateQInvoice() {
        SessionLocator.SelectedSession.CurrentWindow.Close("1");
    }

}




