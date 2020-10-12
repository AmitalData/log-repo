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
import { AllInvoices, IntegratedInvoice, InvoiceLine } from "../../../Customs/EntityPMs/Extended/InvoiceQueue";
import { EntityResourceService } from "../../../Infrastructure/Services/EntityResourceService";
import { DeclarationPMService } from "../../../Customs/Services/StandardPMs/DeclarationPMService";
import { DeclarationPM } from "../../../Customs/EntityPMs/DeclarationPM";
import { DropdownMenuFilterComponent } from '../../../CustomsModules/CustomsCourier/Components/CourierWorkSheet/DropdownMenuFilterComponent';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

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
    public InvoiceListList: ObservableCollection;
    public EMessagesList: ObservableCollection;
    public WMessagesList: ObservableCollection;
    public declaration: DeclarationPM;
    ErrorMessages: boolean ;
    WarningMessages: boolean;
    _invoiceQueueWebService: InvoiceQueueWebService = new InvoiceQueueWebService();
    RowIndex: any;
    UnifreightMessage: any;
    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                //this.GetData();
            });
        });
    }
    private GetData() {
        this.ResetVariables();
        this._declarationPMService.get(this.UnifreightMessage.LogitudeEntityNumber).subscribe(data => {
        //this._declarationPMService.get("1-5362").subscribe(data => {
            this.declaration = data.Result;
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (this.declaration==null) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show("declaration NOT FOUND");
            }
            this._invoiceQueueWebService.GetInvoice(this.declaration.Tenant, this.declaration.CustomFileNo).subscribe(data => {
                if ((data.Result.Invoice as AllInvoices).InvoiceLines != null) {
                    (data.Result.Invoice as AllInvoices).InvoiceLines.forEach(x => {
                        x = this.setClientForwarder(x);
                        x.AmountForeign = this.SetFixedValue(x.AmountForeign);
                        x.AmountNIS = this.SetFixedValue(x.AmountNIS);
                        this.InvoiceLineList.Insert(x);
                    });
                }
                (data.Result.Invoice as AllInvoices).Statuses.forEach(x => {
                    this.StatusList.Insert(x);
                });
                (data.Result.Invoice as AllInvoices).IntegratedInvoices.forEach(x => {
                    x.InvoiceAmount = this.SetFixedValue(x.InvoiceAmount);
                    this.IntegratedInvoiceList.Insert(x);
                });
                if ((data.Result.Invoice as AllInvoices).Invoices != null) {
                    (data.Result.Invoice as AllInvoices).Invoices.forEach(x => {
                        x.InvoiceAmount = this.SetFixedValue(x.InvoiceAmount);
                        this.InvoiceListList.Insert(x);
                    });
                }
              
                if ((data.Result.Invoice as AllInvoices).Messages != null) {
                    (data.Result.Invoice as AllInvoices).Messages.forEach(x => {
                        if (x.E != null) {
                            this.EMessagesList.Insert(x);
                            this.ErrorMessages = true;
                        }
                        if (x.W != null) {
                            this.WMessagesList.Insert(x);
                            this.ErrorMessages = true;
                        }
                   });
                }
                this.WMessagesList.Collection.forEach(x => {
                    this.EMessagesList.Insert(x);
                });
            });
        });
    }


    ResetVariables() {
        this.InvoiceLineList = new ObservableCollection([]);
        this.IntegratedInvoiceList = new ObservableCollection([]);
        this.StatusList = new ObservableCollection([]);
        this.InvoiceListList = new ObservableCollection([]);
        this.EMessagesList = new ObservableCollection([]);
        this.WMessagesList = new ObservableCollection([]);
        this.ErrorMessages = false;
        this.WarningMessages = false;
    }
    SetWindowArgs(args: any) {
        //var json = '{"UnifreightEntity"  :  "CFIFILEM" , "UnifreightEntityNumber"  :  "3000028" , "LogitudeEntity"  :  "Customs.Declaration" , "LogitudeEntityNumber"  :  "1-211622" , "LogitudeViewModel"  :  "UnifreightMassageHandler" , "LogitudeCommandId"  :  "CreateInvoiceCommand" , "formtitle"  :  "הצהרת יבוא"}';

        this.UnifreightMessage = args.unifreightMessage;
        this.GetData();
    }


    public Run(args: any) {
        this.RowIndex = args['RowIndex'];

    }

    setClientForwarder(value: InvoiceLine) {
        switch (value.PayType) {
            case "E": {
                value.PayType = "Forwarder";
                break;
            }
            case "L": {
                value.PayType = "Client";
                break;
            }
            default: {
                break;
            }
        }
        return value;
    }

    SetFixedValue(value: string) {
        if (value != "" && value != null) {
            value = parseFloat(value).toLocaleString();
            if (value.indexOf('.') == -1) {
                value = value + ".00";
            }
            return value.toString();
        }
        return value;
    }

    private _Remarks: string;
    public get Remarks() { return this._Remarks; }
    public set Remarks(newValue: string) {
        this._Remarks = newValue;
    }

    CloseClicked() {

        if (this._Remarks != null && this._Remarks != "") {
            var confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("General.O.Confirm");
            confirm.NoButtonText = TextCodeTranslator.Translate("General.O.Void");
            // confirm.Show(TextCodeTranslator.Translate("Customs.Declarations.O.UnSavedRemark"));
            confirm.Show("ביציאה מהמסך לא ישמרו הערות לחשבונית שהוזנו במסך")
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    confirm.Close();
                    alert("0" + "with remark");
                    SessionLocator.SelectedSession.CurrentWindow.Close("0");
                } else {
                    confirm.Close();
                }
            });
        } else {
            alert("EMPTY" + "no remake ");
            SessionLocator.SelectedSession.CurrentWindow.Close("");
        }
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
                        IsMatchUnifreightCallbackCommand = true;
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            //SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
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
                        IsMatchUnifreightCallbackCommand = true;
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            //SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
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
                        //alert(JSON.stringify(mess));
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclaration.Id &&
                            mess.LogitudeViewModel == myViewModelName);
                        IsMatchUnifreightCallbackCommand = true;
                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            //let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            //SessionLocator.SelectedSession.CurrentListComponent.OnBackFromEdit(this.declaration.Id, { rowIndex: this.RowIndex });
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




