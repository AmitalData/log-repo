import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool, DateTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { AccountingOpService } from '../../../Services/Others/AccountingOpService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { JournalPMService } from '../../../Services/StandardPMs/JournalPMService';
declare var attachmentUploader, ResultAsArray: any;

@Component({

    templateUrl: './AccountingMainTesterComponent.html',
})

export class AccountingMainTesterComponent extends BaseComponent {
    public DataContext: AccountingMainTesterComponent = this;
    public ObjectTableName: string = "GLAccount";

    IsShowProgressBar: boolean = false;
    _TextBoxParam: string;
    _LabelLog: string;
    ErrorMess: string;
    JsonOut: string;
    SelectedItem: string;

    JournalId2Void: string = "1-401";
    overrideStorno =
        {
            AccountingEntityCode: "7",//	הפקדת מזומן	Cash Deposit
            AccountingEntityReference: "Cash Deposit 7",
            AccountingEntityId: "Deposit1212",
        };

    _MenuList: string[] = [];
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    _AccountingOpService: AccountingOpService;
    _JournalPMService: JournalPMService = new JournalPMService();
    _LastState: string;

    public JsonList: any[];
    constructor() {
        super();
        this._MenuList.push("AccountingIntegrityService");
        this._MenuList.push("JournalSend");
        this._MenuList.push("JournalApproveService");
        this._MenuList.push("Reports");
        
        this._MenuList.push("Aging");

        this.UIProperties.SetRequired("Email", this.ObjectTableName, true);
        this.CurrentSession.StopBusyIndicator();
        this._AccountingOpService = new AccountingOpService();
    }
    selected() {

    }

    JournalSend_Click() {
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this.ErrorMess = "Set Journal json";
            return;
        }
        let j = '';
        let parseobj = JSON.parse(this._TextBoxParam);
        this._JournalPMService.insert(parseobj)
            .subscribe(
                (res: ServiceResponse) => {
                    this._LabelLog = JSON.stringify(res.Result);
                },
                (err) => {

                    alert(err);
                },
                () => {
                    this.CurrentSession.StopBusyIndicator();
                }
            );

    }

    CardIndexNew_Click() {
        let myLedgerTransactionBalanceFilter =
        {
            Tenant: 1,
            From: DateTool.AddDays(new Date(), -31),
            To: new Date(),
            CurrencyId: SessionLocator.AccountingCurrencyId,
            DateTypeCode: "1",
            GLAccountId: "1-1050",

            SearchFields: "",
            PageStartAtRecordIndex: 0,
            PageSize: 20000,
            Category1Id: "",
            Category2Id: "",
            Category3Id: "",
            Category4Id: "",
            Category5Id: "",
            ChartOfAccountsId: "",
            AccountTypeCode: "",
            IsReconciled: null,
            ChartOfAccountsTypeCode: "",
            SalesmanId: "",

        };
        let opr = "CardIndexNew_Click";

        this.StrandartOp(opr, myLedgerTransactionBalanceFilter, () => {
            let resObj = JSON.parse(this.JsonOut);
            if (Array.isArray(resObj)) {
                this.JsonList = resObj;
            }
        });
    }
    _TrailReport_Click() {
        let j = '{"Tenant":1,"FromDate":"2015-01-20T00:00:00","ToDate":"2021-01-26T00:00:00+02:00","TrailReportLevelOption":"ChartofaccountType=1,Chartofaccount=2,GLAccount=3","MyTrailReportLevel":3,"CurrenciesDetailed":true,"Category1":"","Category2":"","Category3":"","Category4":"","Category5":"","DetailedControlClients":false,"DetailedControlVendors":false,"DetailedControlJob":false,"DetailedControlFile":false,"Suppress_DoNotShowCardWithoutActivity":true,"DoNotShowCardWithLocalCloseBalanceEqualZero":true,"ChartOfAccountsTypeCodeList":["1","2"],"ChartOfAccountsIdList":[]}';
        let obj = JSON.parse(j);
        obj.FromDate = DateTool.AddDays( new Date(),-31);
        obj.ToDate = new Date();
        let opr = "_TrailReport_Click";
        
        this.StrandartOp(opr, obj, () => {
            let resObj = JSON.parse(this.JsonOut);
            if (Array.isArray(resObj)) {
                this.JsonList = resObj;
            }
        });
       
    }
    WorkWithQueue_Click() {

    }
    JournalId2Void_Click() {

    }
    WorkWithoutQueue_Click() {
        let opr = "WorkWithoutQueue_Click";
        let obj = { MyTenant: 1, JournalId: "1-55235" };
        this.StrandartOp(opr, obj, () => { });
    }
    _ButtonReverseTrans_Click() {
        let opr = "_ButtonReverseTrans_Click";
        let obj = { MyTenant: 1, MyDate: DateTool.AddDays(new Date(), -31), MyGLAccId: "1-131321" };
        this.StrandartOp(opr, obj, () => { });
      
    }
    _ButtonReverseTotal_Click() {
        let opr = "_ButtonReverseTotal_Click";
        let obj = { MyTenant: 1, MyDate: DateTool.AddDays(new Date(), -31), MyGLAccId: "1-131321" };
        this.StrandartOp(opr, obj, () => { });
    }
    //type myCallback = () => any;

    StrandartOp(opr: string, defaultObj, onEndExec: () => any) {
        try {
            if (AppTool.IsNullOrEmpty(this._TextBoxParam) || this._LastState != opr) {
                

                this._TextBoxParam = JSON.stringify(defaultObj);
                return;
            }
            
            let parseobj = JSON.parse(this._TextBoxParam);
            this.CurrentSession.StartBusyIndicatorCreating();
            this._AccountingOpService.GetTestOperation(opr, this._TextBoxParam)
                .subscribe(
                    (res: ServiceResponse) => {
                        this._LabelLog = res.Result.Log;
                        this.JsonOut = res.Result.JsonOut;
                        
                        this.ErrorMess = res.Result.ExceptionMess;
                     
                        this.CurrentSession.StopBusyIndicator();
                        onEndExec();
                    },
                    (err) => {
                        
                        alert(err);
                    },
                    () => {
                        this.CurrentSession.StopBusyIndicator();
                    }
                
                );
        }
        catch (err) {
            this._LabelLog = err;
        }
        finally {
            this._LastState = opr;
        }
        
        
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    


    getHeaders() {
        let headers: string[] = [];
        if (this.JsonList) {
            this.JsonList.forEach((value) => {
                Object.keys(value).forEach((key) => {
                    if (!headers.find((header) => header == key)) {
                        headers.push(key)
                    }
                })
            })
        }
        return headers;
    }
}
