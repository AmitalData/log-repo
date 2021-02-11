import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool, DateTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { AccountingOpService } from '../../../Services/Others/AccountingOpService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { JournalPMService } from '../../../Services/StandardPMs/JournalPMService';
import { ajax } from 'rxjs/ajax';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
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
        //SessionLocator.SelectedSession.CurrentEditComponent = this;
        this._MenuList.push("AccountingIntegrityService");
        this._MenuList.push("JournalSend");
        this._MenuList.push("JournalApproveService");
        this._MenuList.push("Reports");
        
        this._MenuList.push("Aging");
        this._MenuList.push("Alex");

        this.UIProperties.SetRequired("Email", this.ObjectTableName, true);
        this.CurrentSession.StopBusyIndicator();
        this._AccountingOpService = new AccountingOpService();
    }
    selected() {

    }

    JournalSend_Click() {
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this.SetJournalExample();
            this.ErrorMess = "Set Journal json";
            return;
        }
        let j = '';
        
        let parseobj = JSON.parse(this._TextBoxParam);
        let _http = ServiceHelper.HttpClient;
        let _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/journals';   
        _http.post(_apiUrl, JSON.stringify(parseobj), ServiceHelper.GetHttpFullHeaders())
        //ajax.post(
        //    ServiceHelper.GetLogitudeURL() + 'api/journals',
        //    this._TextBoxParam,
        //    ServiceHelper.GetHttpFullHeaders()
        //)
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator();}
                
            );
        //this._JournalPMService.insert(parseobj)
        //    .subscribe(
        //        (res: ServiceResponse) => {
        //            this._LabelLog = JSON.stringify(res.Result);
        //        },
        //        (err) => {

        //            alert(err);
        //        },
        //        () => {
        //            this.CurrentSession.StopBusyIndicator();
        //        }
        //    );

    }
    XXX_Click() {

    }
    YYY_Click() {

    }
    Aging_Click() {
        let aging_params = {
            Tenant: 1,
            "AgingForDate": new Date(),
            "NumberOfmonthsbackwards": 6,
            VendorCustomerId: "1-1",
            Aging4AccountTypeCode: 'Customer2',
            Category1Id: "",
            Category2Id: "",
            Category3Id: "",
            Category4Id: "",
            Category5Id: "",
            Category6Id: "",
            CollectorId: "",
            SalesmanId: "",
            ChartOfAccountsTypeCode: "",
            ChartOfAccountsId: "",
            AggregateByGLAccountCurrencies: false,
            AggregateByGLAccountChildren: false,
            AgingMethod_Options: 'TotalByMonthMethod;TotalByMonthFIFOMethod;ReconcileOpenBalanceMethod',
            AgingMethod: 'ReconcileOpenBalanceMethod',
            GroupByDate: 'DueDate',
            GroupByDate_Options: 'DueDate;AccountingDate',
            Aging4AccountTypeCode_Options: 'ControlAccountOnly1;Customer2;Vendor3',
            BuildPivot: true,
        };
        
        let opr = "Aging_Click";

        this.StrandartOp(opr, aging_params, () => {
            //if (aging_params.BuildPivot) {
                let resObj = JSON.parse(this.JsonOut);
                if (Array.isArray(resObj)) {
                    this.JsonList = resObj;
                }
            //}
        });

    }
    //XXX_Click() {

    //}
    //YYY_Click() {

    //}
    //Aging_Click() {
    //    let aging_params = {
    //        Tenant: 1,
    //        "AgingForDate": new Date(),
    //        "NumberOfmonthsbackwards": 6,
    //        VendorCustomerId: "1-1",
    //        Aging4AccountTypeCode: 'Customer2',
    //        Category1Id: "",
    //        Category2Id: "",
    //        Category3Id: "",
    //        Category4Id: "",
    //        Category5Id: "",
    //        Category6Id: "",
    //        CollectorId: "",
    //        SalesmanId: "",
    //        ChartOfAccountsTypeCode: "",
    //        ChartOfAccountsId: "",
    //        AggregateByGLAccountCurrencies: false,
    //        AggregateByGLAccountChildren: false,
    //        AgingMethod_Options: 'TotalByMonthMethod;TotalByMonthFIFOMethod;ReconcileOpenBalanceMethod',
    //        AgingMethod: 'ReconcileOpenBalanceMethod',
    //        GroupByDate: 'DueDate',
    //        GroupByDate_Options: 'DueDate;AccountingDate',
    //        Aging4AccountTypeCode_Options: 'ControlAccountOnly1;Customer2;Vendor3',
    //        BuildPivot: true,
    //    };
        
    //    let opr = "Aging_Click";

    //    this.StrandartOp(opr, aging_params, () => {
    //        //if (aging_params.BuildPivot) {
    //            let resObj = JSON.parse(this.JsonOut);
    //            if (Array.isArray(resObj)) {
    //                this.JsonList = resObj;
    //            }
    //        //}
    //    });

    //}


    CardIndexNew_Click() {
        let myLedgerTransactionBalanceFilter =
        {
            Tenant: 1,
            From: DateTool.AddDays(new Date(), -31),
            To: new Date(),
            CurrencyId: SessionLocator.AccountingCurrencyId,
            DateTypeCode: "1",
            GLAccountId: "1-140",

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

    CardIndexNewDrillDown1st_Click() {
        let resObj = JSON.parse(this.JsonOut);
        if (Array.isArray(resObj)) {
            this.JsonList =resObj[0]["MyLedgerTransactionList"];
        }
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
    JournalApproveQueue_Click() {
        let opr = "JournalApproveQueue_Click";
        let obj = {
            workerrolename_Options: "production,development,staging",
            theQueueStatuses: "development=-10;production=0;staging=-100;",
            workerrolename : "production",
            TimeOutinSec : 30,
            ConversionJournal: false,
        };
        this.StrandartOp(opr, obj, () => { });
    }
    WorkWithoutQueue_Click() {
        let opr = "WorkWithoutQueue_Click";
        let obj = { Tenant: 1, JournalId: "1-55235" };
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

    ButtonReconcileStageCBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id, or empty value to get all";
        defaultParam.AccountTypeCode = "2=Client, 3=Vendor";
        defaultParam.UpToDueDate = "01.01.2020";
        defaultParam.LT_LinesMaximum = 50;
        defaultParam.MaxPageSize = 1000;
        defaultParam.CloseOnlyZeroes = "FALSE";
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageCUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationStageC';
        let myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;
        myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        myUrl = myUrl + "&maximalPageSize=" + objToCheck1.MaxPageSize;///dif !!!
        myUrl = myUrl + "&closeOnlyZeroes=" + objToCheck1.CloseOnlyZeroes;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }
    ButtonReconcileStageCNoBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id, or empty value to get all";
        defaultParam.AccountTypeCode = "2=Client, 3=Vendor";
        defaultParam.UpToDueDate = "01.01.2020";
        defaultParam.LT_LinesMaximum = 50;
        defaultParam.MaxPageSize = 1000;
        defaultParam.CloseOnlyZeroes = "FALSE";
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageCUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationStageC';
        let myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;
        myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        myUrl = myUrl + "&maxPageSize=" + objToCheck1.MaxPageSize;
        myUrl = myUrl + "&closeOnlyZeroes=" + objToCheck1.CloseOnlyZeroes;
        myUrl = myUrl + "&noBatch=1";
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );

    }
    SetJournalExample() {
        let journal = {
            "Id": null,
            "Tenant": "1",
            "JournalNumber": null,
            "AccountingDate": "2016-12-11",
            "StatusCode": "2",
            "CreateDate": "2021-02-09",
            "TypeCode": "0",
            "CreatedByUserId": "1-587933",
            "AccountingEntityCode": "1",
            "ExternalNo": "2016:05:05027",
            "TypeName": null,
            "StatusName": null,
            "ConversionJournal": "true",
            "CreatedByUserName": null,
            "JournalLines": [
                {
                    "JournalId": null,
                    "Tenant": "1",
                    "Line": "1",
                    "EncodeBase64NVARCHARFieldsBy": "windows-1255",
                    "ActionCode": "1",
                    "CreditAccountId": "dmy",
                    "DebitAccountId": "dmy",
                    "DebitAccountNumber": "2910247",
                    "CreditAccountNumber": null,
                    "DocumentDate": "2016-12-01",
                    "AccountingDate": "2016-12-11",
                    "DueDate": "2016-12-01",
                    "LocalAmount": 2188.84,
                    "CurrencyCode": "USD",
                    "ForeignAmount": 564.00,
                    "ExchangeRate": "3.88092",
                    "ExternalOpenAmount": 0.00,
                    "ExternalReconcileNumber": null,
                    "Reference1": "ODMyNzc=",
                    "Reference2": null,
                    "Reference3": null,
                    "ActionName": null,
                    "ActionTypeCode": "1",
                    "CurrencyName": null,
                    "Notes": "8ucg6fr4+iDn5eE="
                },
                {
                    "JournalId": null,
                    "Tenant": "1",
                    "Line": "2",
                    "EncodeBase64NVARCHARFieldsBy": "windows-1255",
                    "ActionCode": "2",
                    "CreditAccountId": "dmy",
                    "DebitAccountId": "dmy",
                    "DebitAccountNumber": "2910247",
                    "CreditAccountNumber": null,
                    "DocumentDate": "2016-12-01",
                    "AccountingDate": "2016-12-11",
                    "DueDate": "2016-12-01",
                    "LocalAmount": 2188.84,
                    "CurrencyCode": "USD",
                    "ForeignAmount": 564.00,
                    "ExchangeRate": "3.88092",
                    "ExternalOpenAmount": 0.00,
                    "ExternalReconcileNumber": null,
                    "Reference1": "ODMyNzc=",
                    "Reference2": null,
                    "Reference3": null,
                    "ActionName": null,
                    "ActionTypeCode": "2",
                    "CurrencyName": null,
                    "Notes": "8ucg6fr4+iDn5eE="
                }
            ],
            "DeletedJournalLines": [],
            "UpdateDate": null,
            "UpdatedByUserId": null,
            "ApproveDate": null,
            "ApprovedByUserId": null,
            "UpdatedByUserName": null,
            "ApprovedByUserName": null,
            "SearchFields": null,
            "AccountingEntityReference": null,
            "OriginalJournalId": null,
            "VoidedByUserId": null,
            "VoidDate": null,
            "OriginalJournalName": null,
            "VoidedByUserName": null,
            "IsVoided": null,
            "VoidedBy": null,
            "ExternalSystem": "AMITAL"
        };
        this._TextBoxParam = JSON.stringify(journal);

    }
}
