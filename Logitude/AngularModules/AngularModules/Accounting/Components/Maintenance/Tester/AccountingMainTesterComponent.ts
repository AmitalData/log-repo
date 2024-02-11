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
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { JournalExtendedPMService } from '../../../Services/ExtendedPMs/JournalExtendedPMService';
import { TaxReportExtendedPMService } from 'Accounting/Services/ExtendedPMs/TaxReportExtendedPMService';
declare var attachmentUploader, ResultAsArray: any;

@Component({

    templateUrl: './AccountingMainTesterComponent.html',
})
    //using gatewaytest
export class AccountingMainTesterComponent extends BaseComponent {
    public DataContext: AccountingMainTesterComponent = this;
    public ObjectTableName: string = "GLAccount";

    IsShowProgressBar: boolean = false;
    _TextBoxParam: string;
    _LabelLog: string;
    ErrorMess: string;
    JsonOut: string;
    SelectedItem: string;

    JournalId2Void: string = "1-5599183";
    OverrideStornoString: string = '{"AccountingEntityCode":"7","AccountingEntityReference":"Cash Deposit 7","AccountingEntityId":"1-22222"}';
    OverrideTaxReportString: string = '{"taxReportId":"1-22222"}';

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

       


        this._MenuList.push("MSIC");
        this._MenuList.push("Alex");
        this._MenuList.push("FIX");
        


        this.UIProperties.SetRequired("Email", this.ObjectTableName, true);
        this.CurrentSession.StopBusyIndicator();
        this._AccountingOpService = new AccountingOpService();
    }
    selected() {

    }
    JournalId2Void_click() {
        
        let overrideStorno1 = JSON.parse(this.OverrideStornoString);
        let myJournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
        this.CurrentSession.StartBusyIndicatorCreating();
        myJournalExtendedPMService
            .VoidJournal(SessionLocator.Tenant, this.JournalId2Void,
                overrideStorno1.AccountingEntityCode,// "7",//	הפקדת מזומן	Cash Deposit
                overrideStorno1.AccountingEntityId,// "Deposit1212",
                overrideStorno1.AccountingEntityReference//"Cash Deposit 7",
            ).subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }

            );

    }

    SendGLaccount_Click() {
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this.SetGLAccountExample();
            this.ErrorMess = "Set GLAccount json";
            return;
        }
        let j = '';

        let parseobj = JSON.parse(this._TextBoxParam);
        let _http = ServiceHelper.HttpClient;
        let _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccounts';
        _http.post(_apiUrl, JSON.stringify(parseobj), ServiceHelper.GetHttpFullHeaders())
            //ajax.post(
            //    ServiceHelper.GetLogitudeURL() + 'api/journals',
            //    this._TextBoxParam,
            //    ServiceHelper.GetHttpFullHeaders()
            //)
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }

            );
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
    CancelReport(){
        let taxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
        let OverrideTaxReportString1 = JSON.parse(this.OverrideTaxReportString);

        taxReportExtendedPMService
        .CancelTaxReportByTester(OverrideTaxReportString1.taxReportId,).subscribe(
            r => { this._LabelLog = JSON.stringify(r); },
            e => { this._LabelLog = JSON.stringify(e); },
            () => { this.CurrentSession.StopBusyIndicator(); }

        );
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
            SuppressFromGLAccountAgingData: false,
            FroceFromGLAccountAgingData:true,
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

    closingVATReportClick() {
        const aging_params = {
            reportNumber: '',
            tenant:SessionLocator.Tenant
        };        
        const opr = "closingVATReport";
        const callBack = () => this.JsonList = ["Finish, result: " + JSON.parse(this.JsonOut)];

        this.StrandartOp(opr, aging_params, callBack);
    }

    RebuildFIXGLAccountAgingData_Click() {
        let opr = "RebuildFIXGLAccountAgingData_Click";
        let obj = { /*MyTenant: SessionLocator.Tenant,*/ Aging4AccountTypeCode: 'Customer2', MyGLAccId: "1-152", Aging4AccountTypeCode_Options: 'Customer2;Vendor3',};
        //this.StrandartOp(opr, obj, () => { });

        this.StrandartOp(opr, obj, () => {
            let resObj = JSON.parse(this.JsonOut);
            if (Array.isArray(resObj)) {
                this.JsonList = resObj;
            }
        });


    }


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
            IncludeRelatedCurrenciesAccount:false

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
    JournalApproveReturnToQueue_Click() {
        let opr = "JournalApproveReturnToQueue_Click";
        let obj = { Tenant: 1 , AllTenants:false};
        this.StrandartOp(opr, obj, () => { });
    }
    WorkWithoutQueue_Click() {
        let opr = "WorkWithoutQueue_Click";
        let obj = { Tenant: 1, JournalId: "1-55235" };
        this.StrandartOp(opr, obj, () => { });
    }


    _InterestReport_Click() {
        let opr = "InterestReport_Click";
        let obj = { Tenant: 1, JournalId: "1-55235" };
        this.StrandartOp(opr, obj, () => {  },true);
    }


    _ButtonReverseTrans_Click() {
        let opr = "_ButtonReverseTrans_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });
      
    }
   
    _ButtonReverseTotal_Click() {
        let opr = "_ButtonReverseTotal_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });
    }

    _ButtonReverseTotalFIX_Click() {
        let opr = "_ButtonReverseTotalFIX_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), -0), MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });

    }

    _ButtonReverseAllMonthsFIX_Click() {
        let opr = "_ButtonReverseAllMonthsFIX_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });
    }

    BatchYearlyFIX_Click() {
        let opr = "BatchYearlyFIX_Click";
        let obj = {
            MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), 0),
            MyFixType: "ReverseEngineerTotalByMonthService", MyFixTypeOption: "ReverseEngineerTotalByMonthService,ReverseEngineerTotalByMonthServiceControl,TODOMORE"
        };
        this.StrandartOp(opr, obj, () => { });
    }
    _ButtonReverseGLBalanceFIX_Click() {
        let opr = "_ButtonReverseGLBalanceFIX_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });

    }
    Change2MultiCurrency_Click() {
        let opr = "Change2MultiCurrency_Click";
        let obj = { /*MyTenant: SessionLocator.Tenant,*/  MyGLAccId: "1-1234567" };
        this.StrandartOp(opr, obj, () => { });
    }
    

    
    _ButtonReverseDueDate_Click() {
        let opr = "_ButtonReverseDueDate_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyGLAccId: "" };
        this.StrandartOp(opr, obj, () => { });

    }
    ButtonLoadSystem1000_Click() {
        let opr = "ButtonLoadSystem1000_Click";
        let str: string =
            `Please insert page, you can add a header  //Tenant=1071
Line2
Line3
`;
        this.PostOp(opr, str, () => { });
    }
    _ButtonFixDueLocalBalance_Click() {
        let opr = "_ButtonFixDueLocalBalance_Click";
        let obj = { MyTenant: SessionLocator.Tenant, /*MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: ""*/ };
        this.StrandartOp(opr, obj, () => { });

    }

    _ButtonReverseTotalFIXControl_Click() {
        let opr = "_ButtonReverseTotalFIXControl_Click";
        let obj = { MyTenant: SessionLocator.Tenant, MyDate: DateTool.AddDays(new Date(), 0), ChangeSupplier2Customer : false, };
        this.StrandartOp(opr, obj, () => { });
    }
    //type myCallback = () => any;

    StrandartOp(opr: string, defaultObj, onEndExec: () => any, isFlatFile: boolean = false) {
        try {
            if (!isFlatFile) {
                if (AppTool.IsNullOrEmpty(this._TextBoxParam) || this._LastState != opr) {


                    this._TextBoxParam = JSON.stringify(defaultObj);
                    return;
                }
                let parseobj = JSON.parse(this._TextBoxParam);
            } else {
                if (AppTool.IsNullOrEmpty(this._TextBoxParam) || this._LastState != opr) {


                    this._TextBoxParam = defaultObj.toString();
                    return;
                }
                let parseobj = { FlatFile: this._TextBoxParam };
            }
            
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
    PostOp(opr: string, defaultObj, onEndExec: () => any) {
        try {

            if (AppTool.IsNullOrEmpty(this._TextBoxParam) || this._LastState != opr) {


                this._TextBoxParam = defaultObj.toString();
                return;
            }
            let parseobj = { OperationId: opr,  FlatFile: this._TextBoxParam };


            this.CurrentSession.StartBusyIndicatorCreating();
            this._AccountingOpService.PostTestOperation(opr, parseobj)
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
        if (!AppTool.IsNullOrEmpty( this.JsonList)) {
            this.JsonList.forEach((value) => {
                if (!AppTool.IsNullOrEmpty(value)) {
                    Object.keys(value).forEach((key) => {
                        if (!headers.find((header) => header == key)) {
                            headers.push(key);
                        }
                    });
                }
            });
        }
        return headers;
    }
    CreateJournalTask_Click() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 750;
        logitudeWindow.Height = 500;
        logitudeWindow.Title = "Accounting Load Test";
        logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingLoadTestComponent');
    }
    BuildTenant_Click() {
        let paramDefault: any =
        {
            ///Tenant: 1,
            YYYY: 2020,
          
            //BuildAccountingTenant:
            //{
            //    VatAccounts: false,
            //    ControlAccounts: false,
            //    ExchangeRateDiff: false,
            //    RevenueExpense: false,
            //    TaxWithholding: false
            //},

            BuildGLAccountEachType: 2,
            BuildJournalEachMonth: 2,
        };
        let opr = "BuildTenant_Click";
        this.StrandartOp(opr, paramDefault, () => { });
    }
    YearTransfer_Click() {
        let paramDefault: any =
        {
            Tenant: 1,
            YY: 20,
            Immediate: false
        };
        let opr = "YearTransfer_Click";
        //let obj = { MyTenant: 1, MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: "" };
        this.StrandartOp(opr, paramDefault, () => { });
    }
    YearTransferCancel_Click() {
        let paramDefault: any =
        {
            Tenant: 1,
            YY: 20,
           
        };
        let opr = "YearTransferCancel_Click";
        //let obj = { MyTenant: 1, MyDate: DateTool.AddDays(new Date(), 0), MyGLAccId: "" };
        this.StrandartOp(opr, paramDefault, () => { });
    }


    ButtonReconcileStageABatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.FromExtNum = "1";
        defaultParam.ToExtNum = "99";

        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageAUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationAfterConversion';

        var myUrl;
        if (objToCheck1.FromExtNum == "" && objToCheck1.ToExtNum == "") {
            myUrl = _ReconciliationStageAUrl + "?tenant=" + objToCheck1.Tenant;
        }
        else {
            myUrl = _ReconciliationStageAUrl + "?tenant=" + objToCheck1.Tenant + "&fromExtNum=" + objToCheck1.FromExtNum + "&toExtNum=" + objToCheck1.ToExtNum;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }


    ButtonReconcileStageANoBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.FromExtNum = "1";
        defaultParam.ToExtNum = "99";

        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageAUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationAfterConversion';

        var myUrl;
        if (objToCheck1.FromExtNum == "" && objToCheck1.ToExtNum == "") {
            myUrl = _ReconciliationStageAUrl + "?tenant=" + objToCheck1.Tenant + "&noBatch=1";
        }
        else {
            myUrl = _ReconciliationStageAUrl + "?tenant=" + objToCheck1.Tenant + "&fromExtNum=" + objToCheck1.FromExtNum + "&toExtNum=" + objToCheck1.ToExtNum + "&noBatch=1";
        }

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );

    }


    ButtonReconcileStageBBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;

        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageBUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationStageB';

        let myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant;

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }


    ButtonReconcileStageBNoBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;

        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _ReconciliationStageBUrl = ServiceHelper.GetLogitudeURL() + '/api/ReconciliationStageB';

        var myUrl;
        if (objToCheck1.GLAccountId == "") {
            myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant + "&noBatch=1";
        }
        else {
            myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant + "&gLAccountId=" + objToCheck1.GLAccountId + "&noBatch=1";
        }

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );

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
        //let myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
        //myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        //myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        //myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;
        //myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        //myUrl = myUrl + "&maximalPageSize=" + objToCheck1.MaxPageSize;///dif !!!
        //myUrl = myUrl + "&closeOnlyZeroes=" + objToCheck1.CloseOnlyZeroes;
        let myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;
        myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        myUrl = myUrl + "&maxPageSize=" + objToCheck1.MaxPageSize;
        myUrl = myUrl + "&closeOnlyZeroes=" + objToCheck1.CloseOnlyZeroes;
        myUrl = myUrl + "&noBatch=0";
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


    ButtonCheckInterestTransactionsFromAccounts_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id, or empty value to get all";
        defaultParam.AccountTypeCode = "Id, or empty value to get all (2=Client, 3=Vendor)";
        // defaultParam.UpToDueDate = "01.01.2020";
        defaultParam.LT_LinesMaximum = 50;
        defaultParam.MaxPageSize = 1000;
        defaultParam.SpecificJournalId = "";
        defaultParam.LastMadeGLAccountId = "";
        defaultParam.MaxGLAccountsPerQuery = 100;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _InterestTransactionsCheckAUrl = ServiceHelper.GetLogitudeURL() + '/api/InterestTransactionsCheckA';
        let myUrl = _InterestTransactionsCheckAUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        // myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;
        myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        myUrl = myUrl + "&maxPageSize=" + objToCheck1.MaxPageSize;
        myUrl = myUrl + "&specificJournalId=" + objToCheck1.SpecificJournalId;
        myUrl = myUrl + "&lastMadeGLAccountId=" + objToCheck1.LastMadeGLAccountId;
        myUrl = myUrl + "&maxGLAccountsPerQuery=" + objToCheck1.MaxGLAccountsPerQuery;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(r);
                    let resObj = JSON.parse(this.JsonOut);
                    if (Array.isArray(resObj)) {
                        this.JsonList = resObj;
                    }
                },
                e => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(e);
                },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }

    ButtonCheckInterestReportBalance_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id, or empty value to get all";
        defaultParam.AccountTypeCode = "Id, or empty value to get all (2=Client, 3=Vendor)";
        defaultParam.LT_LinesMaximum = 50;
        defaultParam.MaxPageSize = 1000;
        defaultParam.SpecificJournalId = "";
        defaultParam.LastMadeGLAccountId = "";
        defaultParam.MaxGLAccountsPerQuery = 100;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _InterestTransactionsCheckBUrl = ServiceHelper.GetLogitudeURL() + '/api/InterestTransactionsCheckB';
        let myUrl = _InterestTransactionsCheckBUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;
        myUrl = myUrl + "&maxPageSize=" + objToCheck1.MaxPageSize;
        myUrl = myUrl + "&specificJournalId=" + objToCheck1.SpecificJournalId;
        myUrl = myUrl + "&lastMadeGLAccountId=" + objToCheck1.LastMadeGLAccountId;
        myUrl = myUrl + "&maxGLAccountsPerQuery=" + objToCheck1.MaxGLAccountsPerQuery;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(r);
                    let resObj = JSON.parse(this.JsonOut);
                    if (Array.isArray(resObj)) {
                        this.JsonList = resObj;
                    }
                },
                e => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(e);
                },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }



    ButtonGLAccountInterestActivationBalance_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id, or empty value to get all";
        defaultParam.AccountTypeCode = "Id, or empty value to get all (2=Client, 3=Vendor)";
        defaultParam.InterestActivationDate = "DD.MM.YYYY";
        defaultParam.BatchIt = 0;
        defaultParam.LastMadeGLAccountId = "";
        defaultParam.MaxGLAccountsPerQuery = 100;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _GLAccountInterestActivationBalanceUrl = ServiceHelper.GetLogitudeURL() + '/api/GLAccountInterestActivationBalance';
        let myUrl = _GLAccountInterestActivationBalanceUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
        myUrl = myUrl + "&interestActivationDate=" + objToCheck1.InterestActivationDate;
        myUrl = myUrl + "&batchIt=" + objToCheck1.BatchIt;
        myUrl = myUrl + "&lastMadeGLAccountId=" + objToCheck1.LastMadeGLAccountId;
        myUrl = myUrl + "&maxGLAccountsPerQuery=" + objToCheck1.MaxGLAccountsPerQuery;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(r);
                    let resObj = JSON.parse(this.JsonOut);
                    if (Array.isArray(resObj)) {
                        this.JsonList = resObj;
                    }
                },
                e => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(e);
                },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }


    ButtonGLAccountInterestDeactivationBalance_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.GLAccountId = "Id - a must";
//        defaultParam.AccountTypeCode = "Id, or empty value to get all (2=Client, 3=Vendor)";
//        defaultParam.InterestActivationDate = "DD.MM.YYYY";
        defaultParam.BatchIt = 0;
//        defaultParam.LastMadeGLAccountId = "";
//        defaultParam.MaxGLAccountsPerQuery = 100;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _GLAccountInterestDeactivationBalanceUrl = ServiceHelper.GetLogitudeURL() + '/api/GLAccountInterestDeactivationBalance';
        let myUrl = _GLAccountInterestDeactivationBalanceUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;
//        myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;
//        myUrl = myUrl + "&interestActivationDate=" + objToCheck1.InterestActivationDate;
        myUrl = myUrl + "&batchIt=" + objToCheck1.BatchIt;
//        myUrl = myUrl + "&lastMadeGLAccountId=" + objToCheck1.LastMadeGLAccountId;
//        myUrl = myUrl + "&maxGLAccountsPerQuery=" + objToCheck1.MaxGLAccountsPerQuery;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(r);
                    let resObj = JSON.parse(this.JsonOut);
                    if (Array.isArray(resObj)) {
                        this.JsonList = resObj;
                    }
                },
                e => {
                    this.CurrentSession.StopBusyIndicator();
                    this._LabelLog = JSON.stringify(e);
                },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }




    ButtonAllOpenRevaluationsNoBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;

        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _RevaluationOpUrl = ServiceHelper.GetLogitudeURL() + '/api/RevaluationOp';
        let myUrl = _RevaluationOpUrl + "?tenant=" + objToCheck1.Tenant;

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); this.CurrentSession.StopBusyIndicator();},
                () => { this.CurrentSession.StopBusyIndicator(); }
            );

    }


    ButtonRunOneRevaluationNoBatch_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.RevaluationId = "RevaluationId, must be open";
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _RevaluationOpUrl = ServiceHelper.GetLogitudeURL() + '/api/RevaluationOp';
        let myUrl = _RevaluationOpUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&revaluationId=" + objToCheck1.RevaluationId;

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); this.CurrentSession.StopBusyIndicator();},
                () => { this.CurrentSession.StopBusyIndicator(); }
            );

    }



    ButtonCardGLAccountConnect_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _CardGLAccountConnectUrl = ServiceHelper.GetLogitudeURL() + '/api/CardGLAccountConnect';
        let myUrl = _CardGLAccountConnectUrl + "?tenant=" + objToCheck1.Tenant;
        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); },
                e => { this._LabelLog = JSON.stringify(e); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }


    ButtonBanksCCExternalReco_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.AccountId = "1-1234";
        defaultParam.AccountDisplayNumber = "12345678";
        defaultParam.ToAccountingDate = "30.04.2023";
        defaultParam.Batch = 0;
        //  defaultParam.Comment = "Enter InvoiceNumber, or leave it empty but enter the dates";
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _BanksCCExternalRecoUrl = ServiceHelper.GetLogitudeURL() + '/api/BanksCCExternalReco';
        let myUrl = _BanksCCExternalRecoUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&accountId=" + objToCheck1.AccountId;
        myUrl = myUrl + "&accountDisplayNumber=" + objToCheck1.AccountDisplayNumber;
        myUrl = myUrl + "&toAccountingDate=" + objToCheck1.ToAccountingDate;
        myUrl = myUrl + "&batch=" + objToCheck1.Batch;

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); this.CurrentSession.StopBusyIndicator(); },
                e => { this._LabelLog = JSON.stringify(e); this.CurrentSession.StopBusyIndicator(); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }


    ButtonGLAccountMultiToCurrency_Click() {
        let defaultParam: any = {};
        defaultParam.Tenant = 1;
        defaultParam.AccountId = "";
        defaultParam.AccountDisplayNumber = "12345678";
        defaultParam.ToCurrencyId = "";
        defaultParam.ToCurrencyCode = "USD";
        defaultParam.Batch = 0;
        if (AppTool.IsNullOrEmpty(this._TextBoxParam)) {
            this._TextBoxParam = JSON.stringify(defaultParam);
            return;
        }
        let objToCheck1 = JSON.parse(this._TextBoxParam);
        let _GLAccountMultiToCurrencyUrl = ServiceHelper.GetLogitudeURL() + '/api/GLAccountMultiToCurrency';
        let myUrl = _GLAccountMultiToCurrencyUrl + "?tenant=" + objToCheck1.Tenant;
        myUrl = myUrl + "&accountId=" + objToCheck1.AccountId;
        myUrl = myUrl + "&accountDisplayNumber=" + objToCheck1.AccountDisplayNumber;
        myUrl = myUrl + "&toCurrencyId=" + objToCheck1.ToCurrencyId;
        myUrl = myUrl + "&toCurrencyCode=" + objToCheck1.ToCurrencyCode;
        myUrl = myUrl + "&batch=" + objToCheck1.Batch;

        this.CurrentSession.StartBusyIndicatorCreating();
        let _http = ServiceHelper.HttpClient;
        _http.get(myUrl, ServiceHelper.GetHttpFullHeaders())
            .subscribe(
                r => { this._LabelLog = JSON.stringify(r); this.CurrentSession.StopBusyIndicator(); },
                e => { this._LabelLog = JSON.stringify(e); this.CurrentSession.StopBusyIndicator(); },
                () => { this.CurrentSession.StopBusyIndicator(); }
            );
    }




    ButtonLoadConsolTaxRep_Click() {
        let opr = "ButtonLoadConsolTaxRep_Click";
        let str: string =
            `Please insert page, you can add a header  //Tenant=1071
//ReportId=1-12345678
Line3
Line4
`;
        this.PostOp(opr, str, () => { });
    }



    ButtonLoadInterestTransactions_Click() {
        let opr = "ButtonLoadInterestTransactions_Click";
        let str: string =
            `Please insert page  //Tenant=1071
Line2
Line3
`;
        this.PostOp(opr, str, () => { });
    }



    ButtonLoadChargeTypes_Click() {
        let opr = "ButtonLoadChargeTypes_Click";
        let str: string =
            `Please insert page, you can add a header  //Tenant=1071
Headers - this line will be deleted
Line3
Line4
`;
        this.PostOp(opr, str, () => { });
    }


    ButtonLoadGLAccounts_Click() {
        let opr = "ButtonLoadGLAccounts_Click";
        let str: string =
            `Please insert page, you HAVE to add a header  //Tenant=1071
Headers - this line will be deleted
Line3
Line4
`;
        this.PostOp(opr, str, () => { });
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
            "ExternalSystem": "AMITAL",
            "AllTenants":"False"
        };
        this._TextBoxParam = JSON.stringify(journal);

    }
    SetGLAccountExample() {
        let defaultGLaccountPM = '{"AutomaticReconcile": null,"Category1": null,"Category2": null,"Category3": null,"Category4": null,"Category5": null,"ChartOfAccount": null,"ChartOfAccountsType": null,"ControlAccount": null,"Currency": null,"GLAccountType": 1,"PreviousChartOfAccount": null,"ReconcileMethod": null,"RevenueExpense": null,"Id": "","Tenant": 1,"InternalNumber": "1000","AccountTypeCode": "1","DisplayNumber": "Customers","LocalName": "יהי טוב","EnglishName": "Customer xx","SearchFields": "לקוחות","IsMultiCurrency": true,"CurrencyId": null,"RevenueExpenseType": "1","IsControlAccount": false,"ChartOfAccountsId": "1-105","Inactive": false,"ChartOfAccountsTypeCode": "3","ReconcileMethodCode": "0","ControlAccountId": null,"AutomaticReconcileId": null,"PreviousEnglishName": null,"PreviousEnglishNameChangeDate": "2016-11-23T07:00:35.407","PreviousLocalName": null,"PreviousLocalNameChangeDate": "2016-11-23T07:00:35.407","PreviousNumber": null,"PreviousNumberChangeDate": "2016-11-23T07:00:35.407","PreviousChartOfAccountsId": null,"PreviousChartOfAccountsChangeDate": "2016-11-23T07:00:35.407","CustomerGLAccountId": null,"BalanceInLocalCurrency": null,"RevaluationEnabled": null,"ParentAccountId": null,"Category1Id": null,"Category2Id": null,"Category3Id": null,"Category4Id": null,"Category5Id": null,"IsVATExempt": null}';
        this._TextBoxParam = defaultGLaccountPM;
    }


}
