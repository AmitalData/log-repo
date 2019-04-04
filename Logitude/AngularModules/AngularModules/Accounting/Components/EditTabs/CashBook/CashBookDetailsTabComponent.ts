import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {CashBookLinePM} from '../../../EntityPMs/CashBookLinePM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {GLAccountListService} from '../../../Services/StandardLists/GLAccountListService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
    templateUrl: './CashBookDetailsTabComponent.html',
})

export class CashBookDetailsTabComponent extends BaseComponent {
    public EntityPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    public DataContext = this;
    public TotalSum = 0;
    public NoRows: boolean = false;
    tenantCurrency: string = SessionLocator.TenantPM.CurrencyCode;
    searchText: string = "";
    ItemSource: CashBookLinePM[];
    FilteredLines: CashBookLinePM[]; // only non deposited chequeus
    public isRTL: boolean = false;
    _GLAccountListService: GLAccountListService = new GLAccountListService();
    ChequesList: ObservableCollection;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.Listen();
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //this.ItemSource = this.EntityPM.CashBookLines;
        this.LoadScreen();


        //if (this.TotalSum > 0) {
        //    this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
        //}

        this.SetUIProperties();
    }
    LoadScreen() {
        this.RemoveDepositedLines();
        this.CalculateTotals();
        this.ComputeFilterTotals();

        // toggle GLAccount editability
        if (this.EntityPM.AccountId)
            this._GLAccountListService.getSingle(this.EntityPM.AccountId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var glaccount: GLAccountList = myResponse.Result;
                        var glaBalance = glaccount.BalanceInLocalCurrency

                        if (this.TotalSum == 0 && (!glaBalance || glaBalance == 0)) {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, true);
                        } else {
                            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
                        }
                        this.SetUIProperties();

                    }
                }
            });
    }


    SetUIProperties() {
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false); // always dim, WI 41740

        if (this.EntityPM.Inactive) {
            this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AccountId", this.ObjectTableName, false);
            //this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookTypeCode", this.ObjectTableName, false);
        }
    }



    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadScreen();
                        this.SetUIProperties();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        this.LoadScreen();
                        this.SetUIProperties();
                    }
                });
            }
        }
    }

    //#region Properties
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

    get AccountId() { return this.EntityPM.AccountId; }
    set AccountId(value: string) {
        if (this.EntityPM.AccountId != value) {
            this.EntityPM.AccountId = value;

        }
    }

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;

        }
    }

    get CashBookTypeCode() { return this.EntityPM.CashBookTypeCode; }
    set CashBookTypeCode(value: string) {
        if (this.EntityPM.CashBookTypeCode != value) {
            this.EntityPM.CashBookTypeCode = value;
        }
    }

    //get AccountId() { return this.EntityPM.AccountId; }
    //#endregion

    private timerToken: any;
    TextChanged(searchtext) {
        this.timerToken = setTimeout(() => {
            this.searchText = searchtext;
            this.FilterLines();
        }, 500);
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    OpenARPayment(id) {
        // open ARPayment screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARPayment' });
                });
        }
    }

    CalculateTotals() {
        if (!AppTool.IsNullOrEmpty(this.ItemSource)) {

            this.TotalSum = 0;

            if (this.CashBookTypeCode == "1") { //1-cash
                this.TotalSum = this.EntityPM.TotalAmount;
            } else {
                for (let line of this.ItemSource) {
                    this.TotalSum += line.ForeignAmount;
                }
            }


        }
    }

    FilterLines() {
        this.FilterCheques();
        var lines = this.ItemSource;

        // Filtering
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            lines = lines.filter((el) => {
                if (el.ChequeNumber != null)
                    if (el.ChequeNumber.toLowerCase().includes(this.searchText.toLowerCase())) return true;
                if (el.AccountNumber != null)
                    if (el.AccountNumber.toLowerCase().includes(this.searchText.toLowerCase())) return true;
                return false;
            });
        }
        this.ItemSource = lines;

        this.ChequesList = new ObservableCollection([]);
        this.ChequesList.InsertCollection(lines, true);

        this.NoRows = lines.length == 0;
    }

    RemoveDepositedLines() {
        var lines = this.EntityPM.CashBookLines;

        // Filtering
        lines = lines.filter((el) => {
            if (el.ARPChequeStatusCode == "5") return false; // 5- Returned to Customer
            else if (el.IsDeposited == true) return false;
            else return true;
        });

        this.FilteredLines = lines;
        this.ItemSource = this.FilteredLines;

        this.ChequesList = new ObservableCollection([]);
        this.ChequesList.InsertCollection(this.FilteredLines, true);

        this.NoRows = this.FilteredLines.length == 0;
    }

    //#region Filter Methods
    public FilterSelectedValue: string = 'all';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            //this.FilterCheques();
            this.FilterLines(); // set search then filter
        }
    }
    FilterCheques() {

        var originalCheques = this.FilteredLines;
        var filteredQuery = originalCheques;
        var today = new Date();
        this.NoRows = false;
        if (this.FilterSelectedValue == 'cash') {
            filteredQuery = originalCheques.filter((el) => {

                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (date <= today) {
                        return true;
                    }
                    return false;

                }
                return false;
            }); // cash cheques

        } else if (this.FilterSelectedValue == 'postdated') {
            filteredQuery = originalCheques.filter((el) => {

                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (date > today) {
                        return true;
                    }
                    return false;

                }
                return false;
            }); // postdated cheques
        }

        this.ItemSource = filteredQuery;

        this.ChequesList = new ObservableCollection([]);
        this.ChequesList.InsertCollection(filteredQuery, true);

        this.CalculateTotals();
        this.ComputeFilterTotals();

    }

    //#endregion
    CashCount: number = 0;
    PostdatesCount: number = 0;
    ComputeFilterTotals() {

        this.CashCount = 0;
        this.PostdatesCount = 0;

        var todayDate = new Date();
        this.CashCount = this.FilteredLines.filter((el) => {

            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date <= todayDate) {
                    return true;
                }
                return false;

            }
            return false;
        }).length;
        this.PostdatesCount = this.FilteredLines.filter((el) => {

            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date > todayDate) {
                    return true;
                }
                return false;

            }
            return false;
        }).length;
    }

}
