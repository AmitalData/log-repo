import { Component, OnInit, ViewChild, AfterViewInit } from "@angular/core";
import { BaseComponent } from "../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { CustomMessageWrapperComponent } from "../../CustomsControls/Components/CustomMessageWrapperComponent";
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from "../../CustomsRequests/Components/BaseRequestsSheetMassaging";
import { CustomSendOptionsArgs } from "../../../Customs/DataContract/RequestParams/RequestParamsBase";
import { DeclarationRemarks } from "../../../Customs/EntityPMs/Extended/DeclarationRemarks";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";
import { ObservableCollection } from "../../../Infrastructure/Utilities/ObservableCollection";
import { AppTool, DateTool } from "../../../Infrastructure/Tools";
import { InvoiceQueueWebService } from "../../../Customs/Services/WebServices/InvoiceQueueWebService";
import { AllInvoices, GeneralDetails, IntegratedInvoice, Invoice, InvoiceLine, MessagesData, StatusData } from "../../../Customs/EntityPMs/Extended/InvoiceQueue";
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
import { CustomsSettingListService } from "Customs/Services/StandardLists/CustomsSettingListService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";

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
    customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    public isConnectToUnifreight: boolean = false;

    constructor(private EntityResourceService: EntityResourceService, private _declarationPMService: DeclarationPMService) {
        super();
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
            .subscribe((customsSettingList: ServiceResponse) => {
                if (customsSettingList) {
                    this.isConnectToUnifreight = customsSettingList.Result ? customsSettingList?.Result?.IsConnectedToUniFreight : false;
                }
            });

        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                /*if (!this.isConnectToUnifreight) {
                    this.GetData();
                }*/
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
             //this._declarationPMService.get("1-17965094").subscribe(data => {

            this.declaration = data.Result;
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (this.declaration == null) {
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show("declaration NOT FOUND");
            }
            if (!this.isConnectToUnifreight) {
                this.GetInvoiceFromUnifreight();
            } else {
                this._invoiceQueueWebService.GetInvoice(this.declaration.Tenant, this.declaration.CustomFileNo).subscribe(data => {
                    this.processInvoiceData(data.Result, this);
                });
            }
        });

    }

    processInvoiceData(invoiceData: AllInvoices, context: any) {
        if (invoiceData.InvoiceLines != null) {
            invoiceData.InvoiceLines.forEach(x => {
                if (x.AmountNIS !== "") {
                    context.SumAmountNIS += Number(x.AmountNIS);
                }
                x = context.setClientForwarder(x);
                x.AmountForeign = context.SetFixedValue(x.AmountForeign);
                x.AmountNIS = context.SetFixedValue(x.AmountNIS);
                x.ExcludedLine = true;
                context.InvoiceLineList.Insert(x);
            });
        }

        context.LabelSumAmountNIS = context.SetFixedValue(String(context.SumAmountNIS));
        invoiceData.Statuses.forEach(x => context.StatusList.Insert(x));
        invoiceData.IntegratedInvoices.forEach(x => {
            x.InvoiceAmount = context.SetFixedValue(x.InvoiceAmount);
            context.IntegratedInvoiceList.Insert(x);
        });
        context.GeneralDetails = invoiceData.GeneralDetails;

        invoiceData.Invoices?.forEach(x => {
            if (x.InvoiceDate) {
                let InvoiceDate = new Date(x.InvoiceDate);
                if (context.declaration.PaymentDate && InvoiceDate) {
                    let InvoiceDateMonth = InvoiceDate.getMonth();
                    let InvoiceDateYear = InvoiceDate.getFullYear();
                    let PaymentDateMonth = new Date(context.declaration.PaymentDate).getMonth();
                    let PaymentDateYear = new Date(context.declaration.PaymentDate).getFullYear();
                    if ((InvoiceDateMonth < PaymentDateMonth && InvoiceDateYear == PaymentDateYear) || InvoiceDateYear < PaymentDateYear) {
                        context.IsPaymentDateGreaterThanInvoiceDate = true;
                    }
                }
            }
            if (x.InvoiceTypeCode === "R") { // Receipt invoice type
                context.CreateQInvoiceButtonDim = true;
            }
            x.InvoiceAmount = context.SetFixedValue(x.InvoiceAmount);
            context.InvoiceListList.Insert(x);
        });

        invoiceData.Messages?.forEach(x => {
            if (x.E) {
                context.EMessagesList.Insert(x);
                context.ErrorMessages = true;
                context.CreateQInvoiceButtonDim = true; // Disable button on error
            }
            if (x.W) {
                context.WMessagesList.Insert(x);
                context.ErrorMessages = true;
            }
        });

        context.WMessagesList.Collection.forEach(x => context.EMessagesList.Insert(x));
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
        debugger;
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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

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

    ShowConnectedFiling() {
        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-ShowConnectedFiling";
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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

            AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
                "ScriptableGatewayUtil.ShowConnectedFiling",
                "CFIHMAIN.LogitudeTask",
                "ShowConnectedFiling",
                unifreightMessageM,
                " הצגת מסך : מסמכים");
        }
        else {
            alert("ShowConnectedFiling");
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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));

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
                        myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(myDeclaration.Direction));


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

    invoice: AllInvoices;
    GetInvoiceFromUnifreight() {
        let myDeclaration: DeclarationPM = this.declaration;
        let myViewModelName = "InvoiceQueueComponent.ts-GetQInvoice";

        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
          let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        //alert(JSON.stringify(mess));
                        var IsMatchUnifreightCallbackCommand = (
                            mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                            mess.LogitudeEntityNumber == myDeclaration.Id &&
                            mess.UnifreightEntityNumber == myDeclaration.CustomFileNo &&
                            mess.LogitudeViewModel == myViewModelName);
                        IsMatchUnifreightCallbackCommand = true;
                       
                    if (IsMatchUnifreightCallbackCommand) { 
                        sub.unsubscribe();
                        let XMLResponse = UnifreightMessageM.GetStringValue(mess, "XMLResponse");
                        const xmlData = (xml: string) => xml.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
                        this.invoice = this.parseXmlWithMultipleRoots(xmlData(XMLResponse));
                        if (this.invoice.InvoiceLines.length > 0) {  
                            this.processInvoiceData(this.invoice, this);
                        }
                        SessionLocator.SelectedSession.StopBusyIndicator();

                    }
                }
            );

        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(myDeclaration.CustomFileNo, myDeclaration.Id, myViewModelName, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity());
        unifreightMessageM.Requset.push(["XMLRequest", this.convertToXML()]);
       
        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "AmitalGatewayUtil.GetQInvoice",
            "CFIFILEM.LogitudeTask",
            "GetQInvoice",
            unifreightMessageM,
            "");
        }
    }

    parseXmlWithMultipleRoots(xmlString: string): AllInvoices {
        const parser = new DOMParser();
        let allInvoices = this.getDefaultAllInvoices();
    
        // Regular expressions to extract each section from the XML string
        const invoiceLinesMatch = xmlString.match(/<InvoiceLines>[\s\S]*?<\/InvoiceLines>/);
        const integratedInvoicesMatch = xmlString.match(/<IntegratedInvoices[\s\S]*?\/>/);
        const invoicesMatch = xmlString.match(/<Invoices>[\s\S]*?<\/Invoices>/);
        const messagesMatch = xmlString.match(/<Messages[\s\S]*?\/>/);
        const generalDetailsMatch = xmlString.match(/<GeneralDetails>[\s\S]*?<\/GeneralDetails>/);
    
        // Parse and extract data for each section if available
        if (invoiceLinesMatch) {
            const invoiceLinesDoc = parser.parseFromString(invoiceLinesMatch[0], 'application/xml');
            allInvoices.InvoiceLines = this.extractInvoiceLines(invoiceLinesDoc);
        }
    
        if (integratedInvoicesMatch) {
            const integratedInvoicesDoc = parser.parseFromString(integratedInvoicesMatch[0], 'application/xml');
            allInvoices.IntegratedInvoices = this.extractIntegratedInvoices(integratedInvoicesDoc);
        }
    
        if (invoicesMatch) {
            const invoicesDoc = parser.parseFromString(invoicesMatch[0], 'application/xml');
            allInvoices.Invoices = this.extractInvoices(invoicesDoc);
        }
    
        if (messagesMatch) {
            const messagesDoc = parser.parseFromString(messagesMatch[0], 'application/xml');
            allInvoices.Messages = this.extractMessages(messagesDoc);
        }
    
        if (generalDetailsMatch) {
            const generalDetailsDoc = parser.parseFromString(generalDetailsMatch[0], 'application/xml');
            allInvoices.GeneralDetails = this.extractGeneralDetails(generalDetailsDoc);
        }
    
        return allInvoices;
    }
    
    
    getDefaultAllInvoices(): AllInvoices {
        return {
            Statuses: [], // Empty array of StatusData
            InvoiceLines: [], // Empty array of InvoiceLine
            IntegratedInvoices: [], // Empty array of IntegratedInvoice
            Invoices: [], // Empty array of Invoice
            Messages: [], // Empty array of MessagesData
            GeneralDetails: { // Default GeneralDetails object
                Forwarder: '',
                TypeOfDelivery: '',
                TransportResponsibility: ''
            }
        };
    }
    

    extractStatuses(xmlDoc: Document): StatusData[] {
        return Array.from(xmlDoc.getElementsByTagName('StatusData')).map(statusNode => ({
            Code: statusNode.getElementsByTagName('Code')[0]?.textContent || '',
            Name: statusNode.getElementsByTagName('Name')[0]?.textContent || '',
            Date: statusNode.getElementsByTagName('Date')[0]?.textContent || '',
            Time: statusNode.getElementsByTagName('Time')[0]?.textContent || '',
            Comments: statusNode.getElementsByTagName('Comments')[0]?.textContent || ''
        }));
    }

    extractInvoiceLines(xmlDoc: Document): InvoiceLine[] {
        return Array.from(xmlDoc.getElementsByTagName('InvoiceLine')).map(node => ({
            ServiceCode: node.getElementsByTagName('ServiceCode')[0]?.textContent || '',
            ServiceName: node.getElementsByTagName('ServiceName')[0]?.textContent || '',
            PayType: node.getElementsByTagName('PayType')[0]?.textContent || '',
            AmountNIS: node.getElementsByTagName('AmountNIS')[0]?.textContent || '',
            AmountForeign: node.getElementsByTagName('AmountForeign')[0]?.textContent || '',
            Wip: node.getElementsByTagName('Wip')[0]?.textContent || '',
            Currency: node.getElementsByTagName('Currency')[0]?.textContent || '',
            LineNumber: node.getElementsByTagName('LineNumber')[0]?.textContent || '',
            ExcludedLine: true  // Assuming default value as true
        }));
    }

    extractIntegratedInvoices(xmlDoc: Document): IntegratedInvoice[] {
        return Array.from(xmlDoc.getElementsByTagName('IntegratedInvoice')).map(node => ({
            InvoiceNumber: node.getElementsByTagName('InvoiceNumber')[0]?.textContent || '',
            ForwarderFile: node.getElementsByTagName('ForwarderFile')[0]?.textContent || '',
            InvoiceCurrency: node.getElementsByTagName('InvoiceCurrency')[0]?.textContent || '',
            BillTo: node.getElementsByTagName('BillTo')[0]?.textContent || '',
            InvoiceAmount: node.getElementsByTagName('InvoiceAmount')[0]?.textContent || ''
        }));
    }

    extractInvoices(xmlDoc: Document): Invoice[] {
        return Array.from(xmlDoc.getElementsByTagName('Invoice')).map(node => ({
            InvoiceBillTo: node.getElementsByTagName('InvoiceBillTo')[0]?.textContent || '',
            InvoiceBillToCard: node.getElementsByTagName('InvoiceBillToCard')[0]?.textContent || '',
            InvoiceType: node.getElementsByTagName('InvoiceType')[0]?.textContent || '',
            InvoiceDate: node.getElementsByTagName('InvoiceDate')[0]?.textContent || '',
            InvoiceCurrency: node.getElementsByTagName('InvoiceCurrency')[0]?.textContent || '',
            InvoiceTypeCode: node.getElementsByTagName('InvoiceTypeCode')[0]?.textContent || '',
            InvoiceAmount: node.getElementsByTagName('InvoiceAmount')[0]?.textContent || ''
        }));
    }

    extractMessages(xmlDoc: Document): MessagesData[] {
        return Array.from(xmlDoc.getElementsByTagName('MessagesData')).map(node => ({
            W: node.getElementsByTagName('W')[0]?.textContent || '',
            E: node.getElementsByTagName('E')[0]?.textContent || ''
        }));
    }

    extractGeneralDetails(xmlDoc: Document): GeneralDetails {
        const generalDetailsNode = xmlDoc.getElementsByTagName('GeneralDetails')[0];
        return {
            Forwarder: generalDetailsNode.getElementsByTagName('Forwarder')[0]?.textContent || '',
            TypeOfDelivery: generalDetailsNode.getElementsByTagName('TypeOfDelivery')[0]?.textContent || '',
            TransportResponsibility: generalDetailsNode.getElementsByTagName('TransportResponsibility')[0]?.textContent || ''
        };
    }

    getUnifreightFormattedDate(txt: string, time: string, dtdField: string): Date | null {
        if (!txt || txt.trim().length === 0) return null;

        const formats = [
            "yyyyMMddHHmm",   // 202305011230
            "dd.MM.yy",       // 01.05.23
            "yyyyMMdd",       // 20230501
            "yyyyMMddHHmmffff" // 2023050112301234
        ];

        for (const format of formats) {
            const date = this.parseDate(txt, format);
            if (date) {
                return date;
            }
        }
        return null;
    }

    parseDate(txt: string, format: string): Date | null {
        const date = new Date(txt);
        return isNaN(date.getTime()) ? null : date;
    }
    convertToXML() {

        const xmlDocument = document.implementation.createDocument('', '', null);
        const arrayOfEntry = xmlDocument.createElement('ArrayOfEntry');

        const entries = [
            { key: 'componentname', value: 'GDSHMAINXML' },
            { key: 'Operation', value: 'GetQInvoice' },
            { key: 'Subject', value: `GetQInvoice: file ${this.declaration?.CustomFileNo}` },
            { key: 'StatusList', value: '' },
            { key: 'CFIHMAIN:Xml', value: '' },
            { key: 'FileNo', value: this.declaration?.CustomFileNo }
        ];

        entries.forEach(entry => {
            const entryElement = xmlDocument.createElement('Entry');
            const keyElement = xmlDocument.createElement('Key');
            keyElement.textContent = entry.key;
            const valueElement = xmlDocument.createElement('Value');
            valueElement.textContent = entry.value;
            entryElement.appendChild(keyElement);
            entryElement.appendChild(valueElement);
            arrayOfEntry.appendChild(entryElement);
        });

        xmlDocument.appendChild(arrayOfEntry);
        const xmlString = new XMLSerializer().serializeToString(xmlDocument);
        console.log(xmlString);
        return xmlString;
    }


}




