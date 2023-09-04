import { Component, ViewChild, ViewContainerRef, OnInit, ViewChildren, QueryList, AfterViewInit } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { GLAccountExtendedPMService } from 'Accounting/Services/ExtendedPMs/GLAccountExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { GLAccountPMService } from 'Accounting/Services/StandardPMs/GLAccountPMService';
import { AccountingEventManager } from 'Accounting/Utilities/AccountingEventManager';

@Component({
    template:
        `
    <div class="TabHolder">
        <table>
            <tr class="TabTitleRow">
                 <td>{{TabTitleTextCode | TextCodeTranslationPipe}}
                     ({{AccountInfo}})
               </td>
            </tr>

            <tr>
                <td>
                    <div class="MediaFill">
                        <div #Child></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    `,
})

export class AccountingTabComponent implements OnInit, AfterViewInit {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public TabTitleTextCode: string = null;
    public AccountInfo: string = null;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    // @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.TabTitleTextCode = this.ObjectTableName + ".TH.Accounting";

        this.InitializeComponent();


    }
    ngAfterViewInit(): void {
        this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("AccountingNote").subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => {
                    this._entityResourceService.getEntityResourceByTableName("Reconciliation").subscribe((response: any) => {
                        this._entityResourceService.getEntityResourceByTableName("ReconcileExternalPage").subscribe((response: any) => {
                            this._entityResourceService.getEntityResourceByTableName("ExternalReconciliation").subscribe((response: any) => {
                                this._entityResourceService.getEntityResourceByTableName("GLAccountInterestPeriod").subscribe((response: any) => {
                                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod").subscribe((response: any) => {


                                        this.LoadComponent();
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });

    }

    ngOnInit() {

        this.Listen();
    }

    private AccountingSystemPM: any = null;
    private isFullAccounting: boolean = false;
    private isQuickBooksOnline: boolean = false;
    private isPartnerEntity: boolean = false;
    private isQuickBooksOnlineEntity: boolean = false;
    private IsAccountingActivated: boolean = false;
    private IsExternalCodesFromAPI: boolean = false;
    private IsExternalCodesFromTable: boolean = false;
    private IsSingleCurrencyAccount: boolean = false;
    InitializeComponent() {

        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;

        this.AccountingSystemPM = SessionLocator.AccountingSystemPM;
        if (this.AccountingSystemPM) {
            this.IsExternalCodesFromAPI = this.AccountingSystemPM.IsExternalCodesFromAPI;
            this.IsExternalCodesFromTable = this.AccountingSystemPM.IsExternalCodesFromTable;
            this.IsSingleCurrencyAccount = this.AccountingSystemPM.IsSingleCurrencyAccount;
        }

        switch (this.ObjectTableName) {
            case "Agent":
            case "Airline":
            case "CustomAgent":
            case "Customer":
            case "ShippingAgent":
            case "ShippingLine":
            case "Trucker":
            case "Warehouse":
            case "Vendor":
            case "AccountingPartner":
                {
                    this.isPartnerEntity = true;
                    break;
                }
        }

        switch (this.ObjectTableName) {
            case "Customer":
            case "Currency":
            case "PaymentTerm":
            case "ARPaymentMethod":
            case "APPaymentMethod":
            case "ChargesType":
            case "Vendor":
            case "VatType":
            case "Warehouse":
            case "ShippingAgent":
            case "ShippingLine":
            case "Airline":
            case "CustomAgent":
            case "Agent":
            case "AccountingPaymentMethod":
            case "Trucker":
                {
                    this.isQuickBooksOnlineEntity = true;
                    break;
                }
        }

        if (this.IsAccountingActivated) {
            this.isFullAccounting = true;
        }

        if (this.IsExternalCodesFromAPI && this.isQuickBooksOnlineEntity) {
            this.isQuickBooksOnline = true;
        }
        this.GetAccountInfo();     


    }
    private GetAccountInfo(){
        var myService: GLAccountPMService = new GLAccountPMService();
        if (this.EntityPM?.card?.GLAccountId) {
            myService.get(this.EntityPM?.card?.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError && myResponse != null) {
                    var result = myResponse.Result;
                    if (result != null) {
                        if (result.CurrencySign != null) {
                            this.AccountInfo = result.LocalName + ',' + result.DisplayNumber + ',' + result.CurrencySign;
                        }
                        else {
                            this.AccountInfo = result.LocalName + ',' + result.DisplayNumber + ',' + "MULTI";
                        }
                    }
                }
            });
        }



    }
    private LoadCompletedEvent: any = null;

    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.GetAccountInfo();     
                }
            });
        }
        AccountingEventManager.CustomerChangedEvent.subscribe(($event) => {
            SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
            this.GetAccountInfo();
            
        });
    }

    LoadComponent() {
        var myComponentPath: string = null;

        if (this.isFullAccounting
            && this.ObjectTableName != 'ChargesType'
            && this.ObjectTableName != 'Currency'
            && this.ObjectTableName != 'PaymentTerm'
            && this.ObjectTableName != 'VatType'
            && this.ObjectTableName != 'Branch'
            && this.ObjectTableName != 'AccountingPaymentMethod'
            && this.ObjectTableName != 'APPaymentMethod'
            && !this.isQuickBooksOnline) {
            // this._entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => { 
            //     this._entityResourceService.getEntityResourceByTableName("AccountingNote").subscribe((response: any) => { 
            //         this._entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) => { 
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Full";
            //   });
            //  });
            // });
        }
        else if (this.isQuickBooksOnline) {
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_QuickBooksOnline";
        }
        else if (this.isPartnerEntity) {
            myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Partners";
        }
        else {
            switch (this.ObjectTableName) {
                case "Currency": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Currency";
                    break;
                }

                case "PaymentTerm": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_PaymentTerm";
                    break;
                }

                case "VatType": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_VatType";
                    break;
                }

                case "ChargesType": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_ChargesType";
                    break;
                }

                case "Branch": {
                    myComponentPath = "./Common/Components/AccountingTab/AccountingTab_Branch";
                    break;
                }

                case "AccountingPaymentMethod": {
                    myComponentPath = "./Invoice/Components/AccountingTab/AccountingTab_AccountingPaymentMethod";
                    break;
                }

                case "APPaymentMethod": {
                    myComponentPath = "./Invoice/Components/AccountingTab/AccountingTab_APPaymentMethod";
                    break;
                }

            }
        }

        if (!AppTool.IsNullOrEmpty(myComponentPath) && this.viewContainerRef) {
            SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                .then(cmpRef => {
                    //cmpRef.instance
                });
        }
    }
}

//select
//ObjectTableTabs.Code, ObjectTables.Name, ObjectTableTabs.ControlPath
//from ObjectTableTabs
//join ObjectTables on ObjectTableTabs.ObjectTableId = ObjectTables.Id
//where ObjectTableTabs.HtmlComponentUrl = './Common/Components/AccountingTab/AccountingTabComponent'
//go

//CRAC	Currency	    Simplog.FreightLib.Views.Tabs.CurrencyAccountingTabControl
//CHAC	ChargesType	    Simplog.FreightLib.Views.ChargesTypes.AccountingTabControl
//VTAC	VatType	        Simplog.FreightLib.Views.VATTypeAccountingControl
//CLAC	Customer	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomerAccountingTabControl
//AGAC	Agent	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.AgentAccountingTabControl
//CUAC	CustomAgent	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomAgentAccountingTabControl
//SAAC	ShippingAgent	Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.ShippingAgentAccountingTabControl
//ALAC	Airline	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.AirlineAccountingTabControl
//SLAC	ShippingLine	Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.ShippingLineAccountingTabControl
//TRAC	Trucker	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.TruckerAccountingTabControl
//VDAC	Vendor	        Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.VendorAccountingTabControl
//WHAC	Warehouse	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.WarehouseAccountingTabControl
//PTAC	PaymentTerm	    Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.PaymentTermAccountingTabControl
