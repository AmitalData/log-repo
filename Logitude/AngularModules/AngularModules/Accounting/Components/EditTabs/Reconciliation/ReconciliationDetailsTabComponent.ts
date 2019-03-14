import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReconciliationPM} from '../../../EntityPMs/ReconciliationPM';
import {ReconciliationLinePM} from '../../../EntityPMs/ReconciliationLinePM';
import {LedgerTransactionList} from '../../../EntityLists/LedgerTransactionList';
import {LedgerTransactionListService} from '../../../Services/StandardLists/LedgerTransactionListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';


export class ReconciliationLineModel {

    constructor(line, color) {
        this.Line = line;
        this.OddEven = color;

        // Calculate Transaction Amount
        if (AppTool.IsNullOrZero(this.Line.ForeignAmountCredit)) {
            this.TransactionAmount = -1 * this.Line.ForeignAmountDebit;
        } else {
            this.TransactionAmount = this.Line.ForeignAmountCredit;
        }
    }

    Line: ReconciliationLinePM;
    OddEven: boolean;
    TransactionAmount: number;


}

@Component({
    moduleId: module.id,
    templateUrl: './ReconciliationDetailsTabComponent.html',
})

export class ReconciliationDetailsTabComponent extends BaseComponent {
    public EntityPM: ReconciliationPM = null;
    public ObjectTableName = "Reconciliation";
    public DataContext = this;
    public TotalSum = 0;
    public NoRows: boolean = false;
    searchText: string = "";
    ItemSource: ReconciliationLineModel[];
    public CurrencyCode;
    public AmountText: string;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.LoadData();
        this.AmountText = TextCodeTranslator.Translate("Accounting.General.O.Amount") + " ("+  this.EntityPM.CurrencyCode + ")";
    }

    //#region Properties

    //get Number() { return this.EntityPM.Number; }

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

    LoadData() {
        this.ItemSource = [];
        this.originalItemSource = [];

        this.EntityPM.ReconciliationLines.forEach((line) => {
            var item = new ReconciliationLineModel(line, this.ColorMe(line));
            this.ItemSource.push(item);
        });
        this.originalItemSource = this.ItemSource;


        this.CalculateTotals();
    }

    OpenJournal(id) {
        // open Journal screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                });
        }
    }

    CalculateTotals() {
        if (!AppTool.IsNullOrEmpty(this.ItemSource)) {
            for (let line of this.ItemSource) {
                this.TotalSum += line.Line.ReconciliationAmount;
            }
        }
    }

    originalItemSource;
    FilterLines() {
        var lines = this.originalItemSource;

        // Filtering
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
           lines = lines.filter((el) => {
               var line = el.Line;
               if (line.SearchFields != null)
                   if (line.SearchFields.toLowerCase().includes(this.searchText.toLowerCase())) return true;
               return false;
           });
        }
        this.ItemSource = lines;
        this.NoRows = lines.length == 0;
    }

    //#region Row Coloring
    lastGroupNumber: number;
    lastColorOperation: boolean = true;
    ColorMe(line: ReconciliationLinePM) { // "Line says"
        if (!AppTool.IsNullOrEmpty(line)) {

            if (AppTool.IsNullOrEmpty(this.lastGroupNumber)) this.lastGroupNumber = line.GroupNumber;

            if (this.lastGroupNumber == line.GroupNumber) {
                return this.lastColorOperation == true;
            } else {
                this.lastGroupNumber = line.GroupNumber;
                this.lastColorOperation = !this.lastColorOperation;
                return this.lastColorOperation == true;
            }

        }
        return false;
    }
    //#endregion

    RefreshButtonClicked() {
        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
        this.LoadData();
    }

}
