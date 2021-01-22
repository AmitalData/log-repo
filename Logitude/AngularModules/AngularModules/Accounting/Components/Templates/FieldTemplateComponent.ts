import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { JournalExtendedListService } from '../../Services/ExtendedLists/JournalExtendedListService';
import { ARPaymentExtendedListService } from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { JournalList } from '../../EntityLists/JournalList';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { GLAccountExtendedListService } from '../../Services/ExtendedLists/GLAccountExtendedListService';

@Component({
    
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public IsHeaderScreenTemplate: boolean = false;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public TenantCurrencySign: string;

    public fontColor: string;
    public textColor: string;
    public _JournalExtendedListService = new JournalExtendedListService();
    public _ARPaymentExtendedListService = new ARPaymentExtendedListService();

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    public tenantCurrency: string = "";
    public ChildrenGLAccounts: GLAccountPM[]=[];
    public numberOfChildren : number = 0;
    glAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.tenantCurrency = SessionLocator.TenantPM.CurrencyCode;

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];

        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
        }
        if (this.ObjectTableName == "Revaluation" && this.FieldName == "Status") {
            if (this.FieldValue == "Done") { this.fontColor = "green"; }
            else if (this.FieldValue == "In Progress") { this.fontColor = "orange"; }
            else this.fontColor = "black";
        }

        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusCode") {
            if (this.FieldValue == "1") {
                this.textColor = "orange";
            }
            else if (this.FieldValue == "2") {
                this.textColor = "green";
            }
            else if (this.FieldValue == "4") {
                this.textColor = "red";
            }
        }

        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusName") {
            if (this.Entity.PaymentChequeStatusCode == "1") {
                this.textColor = "orange";
            }
            else if (this.Entity.PaymentChequeStatusCode == "2") {
                this.textColor = "green";
            }
            else if (this.Entity.PaymentChequeStatusCode == "4") {
                this.textColor = "red";
            }
        }
        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusName") {

            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.StatusEnglishName;
            }
            else {
                this.FieldValue = this.Entity.PaymentChequeStatusName;
            }
        }

        if (this.ObjectTableName == "TaxDeductionReport" && this.FieldName == "Status") {

            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.Status;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }

            if (this.Entity.StatusTypeCode == "2") {
                this.textColor = "orange";
            }
            else if (this.Entity.StatusTypeCode == "3") {
                this.textColor = "green";
            }
            else if (this.Entity.StatusTypeCode == "4") {
                this.textColor = "red";
            }
        }

        if (this.ObjectTableName == "OpenFormatReport" && this.FieldName == "Status") {

            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.Status;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }
        }

        if (this.ObjectTableName == "InterestReport") {  
           if(this.FieldName == "InterestReportStatusName"){
            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.InterestReportStatusName;
            }
            else {
                this.FieldValue = this.Entity.InterestReportStatusLocalName;
            }

            if (this.Entity.InterestReportStatusCode == "5") {
                this.textColor = "orange";
            }
             else if (this.Entity.InterestReportStatusCode == "6") {
                this.textColor = "red";
            }
            
           }
          else if(this.FieldName == "InterestReportStatusLocalName"){
            if (this.Entity.InterestReportStatusCode == "5") {
                this.textColor = "orange";
            }
             else if (this.Entity.InterestReportStatusCode == "6") {
                this.textColor = "red";
            }
          }

        }

        if (this.ObjectTableName == "OpenFormatReport" && this.FieldName == "CreatedByUserName") {

            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.CreatedByUserName;
            }
            else {
                this.FieldValue = this.Entity.UserLocalName;
            }
        }

        if (this.ObjectTableName == "TaxReport" && this.FieldName == "StatusEnglishName") {

            if (SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.StatusEnglishName;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }
        }

        if (this.ObjectTableName == "GLAccount") {
            this.BuildChildrenGLAccountsList();
        }
    }

    Abs(num: number) {
        if (!AppTool.IsNullOrEmpty(num)) {
            return num > 0 ? num : num * -1;
        }
    }

    OpenCashBook(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'CashBook' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }


    OpenARInvoice(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARInvoice' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    
    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    OpenSource(id: string) {

        // Type:    AccountingEntityCode
        // Id:      AccountingEntityId
        // Display: AccountingEntityReference

        var tableName = AccountingEntityHelper.getEntityObjectTableName(this.Entity.AccountingEntityCode);
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName,
                    BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.SourceJournal"),
                });
            });

    }

    OpenGLAccount(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'GLAccount' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    });
                });
        }
    }

    OpenBankAccount(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'BankAccount', BackButtonLabel: 'Deposit' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    OpenAPPayment(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'APPayment' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    VatNumberClicked() {

    }

    GetAccountingIntegrityCheckStatus() {
        var color = "black";
        switch (this.Entity['StatusCode']) {
            case '2': // 2- in progress
                color = '#0043ff';
                break;

            case '3':
                color = '#d69f03';
                break;

            case '4':
                color = 'green';
                break;

            case '5':
                color = 'red';
                break;

            default:
                break;
        }

        return color;
    }

    BuildChildrenGLAccountsList() {
        //ChildrenGLAccounts = new List<GLAccountPM>();
        this.glAccountExtendedListService.GetChildrenGLAccounts(this.Entity.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (!myResponse.HasError) {
                    for (let item of myResponse.Result) {
                        this.ChildrenGLAccounts.push(item);
                        this.numberOfChildren += 1;
                    }
                }
            }
        });


    }
}
