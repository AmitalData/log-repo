import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BankAccountPM } from '../../../EntityPMs/BankAccountPM';
import { GLAccountPM } from '../../../EntityPMs/GLAccountPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ARPaymentChequeOperationsService } from 'Accounting/Services/Others/ARPaymentChequeOpService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';

@Component({

    templateUrl: './BankAccountGeneralTabComponent.html'
})

export class BankAccountGeneralTabComponent extends BaseComponent {
    public oldCurrency: string = null;
    public EntityPM: BankAccountPM = null;
    public ObjectTableName = "BankAccount";
    public DataContext = this;
    public GLAccountsFilterItems: ApiQueryFilters;

    public isRTL: boolean = false;


    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();
        this.InitLOVFilters();

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
          

        }
    }
   
    InitLOVFilters() {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("ChartOfAccountsTypeCode", "5", null, null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", false, null, null, "Equals", false, false, false, "string");
    }

    defineSerials() {
        var windowArgs: any = {};
        var windowTitle = "הגדרת סדרות";
        var logWindow = new LogitudeWindow();
        windowArgs.EntityPM = this.EntityPM;
        logWindow.Width = 680;
        logWindow.Height = 400;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == "ok") {
                this.LoadAllScreenData()
            }
        });
        logWindow.Show('./Accounting/Components/EditTabs/BankAccount/DetailsTab/ChequeCounterSerialComponent');
    }

    LoadAllScreenData() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
    }

    //#region Properties
    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(value: string) {
        if (this.EntityPM.AccountNumber != value) {
            this.EntityPM.AccountNumber = value;
        }
    }

    get GLAccountId() { return this.EntityPM.GLAccountId; }
    set GLAccountId(value: string) {
        if (this.EntityPM.GLAccountId != value) {
            this.EntityPM.GLAccountId = value;
        }
    }

    glAccount: GLAccountPM;
    get GLAccount() { return this.glAccount; }
    set GLAccount(value: GLAccountPM) {
        if (this.glAccount != value) {
            this.glAccount = value;
        }
    }

    get DeferredGLAccountId() { return this.EntityPM.DeferredGLAccountId; }
    set DeferredGLAccountId(value: string) {
        if (this.EntityPM.DeferredGLAccountId != value) {
            this.EntityPM.DeferredGLAccountId = value;
        }
    }

    deferredGLAccount: GLAccountPM;
    get DeferredGLAccount() { return this.deferredGLAccount; }
    set DeferredGLAccount(value: GLAccountPM) {
        if (this.deferredGLAccount != value) {
            this.deferredGLAccount = value;
        }
    }

    get TransferGLAcccountId() { return this.EntityPM.TransferGLAcccountId; }
    set TransferGLAcccountId(value: string) {
        if (this.EntityPM.TransferGLAcccountId != value) {
            this.EntityPM.TransferGLAcccountId = value;
        }
    }
    get MasavGLAcccountId() { return this.EntityPM.MasavGLAcccountId; }
    set MasavGLAcccountId(value: string) {
        if (this.EntityPM.MasavGLAcccountId != value) {
            this.EntityPM.MasavGLAcccountId = value;
        }
    }
    get BankId() { return this.EntityPM.BankId; }
    set BankId(value: string) {
        if (this.EntityPM.BankId != value) {
            this.EntityPM.BankId = value;
        }
    }

    get BranchNumber() { return this.EntityPM.BranchNumber; }
    set BranchNumber(value: string) {
        if (this.EntityPM.BranchNumber != value) {
            this.EntityPM.BranchNumber = value;
        }
    }
    get PrintingBranchNumber() { return this.EntityPM.PrintingBranchNumber; }
    set PrintingBranchNumber(value: string) {
        if (this.EntityPM.PrintingBranchNumber != value) {
            this.EntityPM.PrintingBranchNumber = value;
        }
    }
    get PrintingAccountNumber() { return this.EntityPM.PrintingAccountNumber; }
    set PrintingAccountNumber(value: string) {
        if (this.EntityPM.PrintingAccountNumber != value) {
            this.EntityPM.PrintingAccountNumber = value;
        }
    }

    get ChequeCounter() { return this.EntityPM.ChequeCounter; }
    set ChequeCounter(value: number) {
        if (this.EntityPM.ChequeCounter != value) {
            this.EntityPM.ChequeCounter = value;
        }
    }

    get ChequeCounterSeriesID() {
        return this.EntityPM.ChequeCounterSeriesID;
    }
    set ChequeCounterSeriesID(value: number) {
        if (this.EntityPM.ChequeCounterSeriesID != value) {
            this.EntityPM.ChequeCounterSeriesID = value;
        }
    }

    get IBAN() { return this.EntityPM.IBAN; }
    set IBAN(value: string) {
        if (this.EntityPM.IBAN != value) {
            this.EntityPM.IBAN = value;
        }
    }

    get SwiftCode() { return this.EntityPM.SwiftCode; }
    set SwiftCode(value: string) {
        if (this.EntityPM.SwiftCode != value) {
            this.EntityPM.SwiftCode = value;
        }
    }

    get BranchAddress() { return this.EntityPM.BranchAddress; }
    set BranchAddress(value: string) {
        if (this.EntityPM.BranchAddress != value) {
            this.EntityPM.BranchAddress = value;
        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }
    get FactoringBank() { return this.EntityPM.FactoringBank; }
    set FactoringBank(value: boolean) {
        if (this.EntityPM.FactoringBank  !==  value) {
            this.EntityPM.FactoringBank = value;
        }
    }
    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }


    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    //#endregion

    SetUIProperties() {
        this.UIProperties.SetEnabled("ChequeCounter", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ChequeCounterSeriesID", this.ObjectTableName, false);
    }

}
