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
import { tryParse } from 'selenium-webdriver/http';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

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
    public GeneralDetails: any;
    public declaration: DeclarationPM;
    CreateQInvoiceButtonDim: boolean;
    ErrorMessages: boolean;
    WarningMessages: boolean;
    IsPaymentDateGreaterThanInvoiceDate: boolean;
    SumAmountNIS: number;
    LabelSumAmountNIS: string;
    _invoiceQueueWebService: InvoiceQueueWebService = new InvoiceQueueWebService();
    RowIndex: any;
    ExcludeLines: string = "";
    UnifreightMessage: any;
    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                //this.GetData();
            });
        });
    }

    ExpandComment(entity: any, $event: any) {
        var windowArgs: any = {};
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 400;
        logitudeWindow.Width = 700;
        logitudeWindow.ShowCloseButton = true;
        windowArgs.remarks = entity.Comments;
        // logitudeWindow.Title = this.title;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsControls/Components/RemarksPopUp');
    }

    private GetData() {
        this.ResetVariables();
        this._declarationPMService.get(this.UnifreightMessage.LogitudeEntityNumber).subscribe(data => {
            //this._declarationPMService.get("1-211404").subscribe(data => {
            this.declaration = data.Result;
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (this.declaration == null) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show("declaration NOT FOUND");
            }
            this._invoiceQueueWebService.GetInvoice(this.declaration.Tenant, this.declaration.CustomFileNo).subscribe(data => {
                if ((data.Result.Invoice as AllInvoices).InvoiceLines != null) {
                    (data.Result.Invoice as AllInvoices).InvoiceLines.forEach(x => {
                        if (x.AmountNIS != "") {
                            this.SumAmountNIS += Number(x.AmountNIS);
                        }
                        x = this.setClientForwarder(x);
                        x.AmountForeign = this.SetFixedValue(x.AmountForeign);
                        x.AmountNIS = this.SetFixedValue(x.AmountNIS);
                        x.ExcludedLine = true;
                        this.InvoiceLineList.Insert(x);
                    });
                }
                this.LabelSumAmountNIS = this.SetFixedValue(String(this.SumAmountNIS));
                (data.Result.Invoice as AllInvoices).Statuses.forEach(x => {
                    this.StatusList.Insert(x);
                });
                (data.Result.Invoice as AllInvoices).IntegratedInvoices.forEach(x => {
                    x.InvoiceAmount = this.SetFixedValue(x.InvoiceAmount);
                    this.IntegratedInvoiceList.Insert(x);
                });
                this.GeneralDetails = (data.Result.Invoice as AllInvoices).GeneralDetails;

                if ((data.Result.Invoice as AllInvoices).Invoices != null) {
                    (data.Result.Invoice as AllInvoices).Invoices.forEach(x => {
                        if (x.InvoiceDate != null && x.InvoiceDate != "") {
                            var InvoiceDate = this.BuildDateFromString(x.InvoiceDate);
                            if (this.declaration.PaymentDate != null && InvoiceDate != null) {
                                var InvoiceDateMonth = InvoiceDate.getMonth();
                                var InvoiceDateYear = InvoiceDate.getFullYear();
                                var PaymentDateMonth = new Date(this.declaration.PaymentDate).getMonth();
                                var PaymentDateYear = new Date(this.declaration.PaymentDate).getFullYear();
                                if ((InvoiceDateMonth < PaymentDateMonth && InvoiceDateYear == PaymentDateYear) || InvoiceDateYear < PaymentDateYear) {
                                    this.IsPaymentDateGreaterThanInvoiceDate = true;
                                }
                            }
                        }
                        if (x.InvoiceTypeCode != null && x.InvoiceTypeCode == "R") { // חשבונית קבלה- סוג R
                            this.CreateQInvoiceButtonDim = true; // מקש הפקת חשבונית ב DIM
                        }
                        x.InvoiceAmount = this.SetFixedValue(x.InvoiceAmount);
                        this.InvoiceListList.Insert(x);
                    });
                }

                if ((data.Result.Invoice as AllInvoices).Messages != null) {
                    (data.Result.Invoice as AllInvoices).Messages.forEach(x => {
                        if (x.E != null) {
                            this.EMessagesList.Insert(x);
                            this.ErrorMessages = true;
                            this.CreateQInvoiceButtonDim = true; // מקש חשבוניות ב DIM םם יש שגםיה מסוג ERROR
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

    BuildDateFromString(date: string) {
        return new Date(date.replace(/(\d{2}).(\d{2}).(\d{4})/, "$2/$1/$3"));
    }

    ResetVariables() {
        this.IsPaymentDateGreaterThanInvoiceDate = false;
        this.SumAmountNIS = 0;
        this.LabelSumAmountNIS = "";
        this.CreateQInvoiceButtonDim = false;
        this.InvoiceLineList = new ObservableCollection([]);
        this.IntegratedInvoiceList = new ObservableCollection([]);
        this.StatusList = new ObservableCollection([]);
        this.InvoiceListList = new ObservableCollection([]);
        this.EMessagesList = new ObservableCollection([]);
        this.WMessagesList = new ObservableCollection([]);
        this.GeneralDetails = {};
        this.ErrorMessages = false;
        this.WarningMessages = false;
    }

    SetWindowArgs(args: any) {
        //var json = '{"UnifreightEntity"  :  "CFIFILEM" , "UnifreightEntityNumber"  :  "3000028" , "LogitudeEntity"  :  "Customs.Declaration" , "LogitudeEntityNumber"  :  "1-211622" , "LogitudeViewModel"  :  "UnifreightMassageHandler" , "LogitudeCommandId"  :  "CreateInvoiceCommand" , "formtitle"  :  "הצהרת יבום"}';

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

    OnExcludeLineChecked($event, lineNumber: any) {
        if (!$event) {
            if (!this.ExcludeLines.includes(lineNumber)) {
                this.ExcludeLines = this.ExcludeLines + lineNumber + ",";
            }
        }
        else {
            if (this.ExcludeLines.includes(lineNumber)) {
                this.ExcludeLines = this.ExcludeLines.replace(lineNumber + ",", "");
            }
        }
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
                    SessionLocator.SelectedSession.CurrentWindow.Close("");
                } else {
                    confirm.Close();
                }
            });
        } else {
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
                " הצגת מסך : רשימת הוצםות");
        }
        else {
            alert("ShowPayments");
        }
    }


    ShowDA() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowDA";
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
                "ScriptableGatewayUtil.ShowDAList",
                "CFIHMAIN.LogitudeTask",
                "ShowDA",
                unifreightMessageM,
                " הצגת מסך : DA");
        }
        else {
            alert("ShowDA");
        }
    }



    ShowDelivery() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowDelivery";
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
                "ScriptableGatewayUtil.ShowDelivery",
                "CFIHMAIN.LogitudeTask",
                "ShowDelivery",
                unifreightMessageM,
                " הצגת מסך : הובלות יבשתיות");
        }
        else {
            alert("ShowDelivery");
        }
    }


    ShowImportFile() {

        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowImportFile";
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
                "ScriptableGatewayUtil.ShowImportFile",
                "CFIHMAIN.LogitudeTask",
                "ShowImportFile",
                unifreightMessageM,
                " הצגת מסך : תיק שילוח");
        }
        else {
            alert("ShowImportFile");
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
        let message = "1" + ";" + this.ExcludeLines;        // send 1 + ExcludeLines seperated by ;
        SessionLocator.SelectedSession.CurrentWindow.Close(message);
    }

}




