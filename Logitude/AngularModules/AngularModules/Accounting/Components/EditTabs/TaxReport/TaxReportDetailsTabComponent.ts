import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TaxReportPM} from '../../../EntityPMs/TaxReportPM';
import {TaxReportLinePM} from '../../../EntityPMs/TaxReportLinePM';
import {RatesTableExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {TaxReportLineStatusListService } from '../../../Services/StandardLists/TaxReportLineStatusListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './TaxReportDetailsTabComponent.html',
})

export class TaxReportDetailsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: TaxReportPM = null;
    public ObjectTableName = "TaxReport";
    public DataContext = this;
    public isRTL: boolean = false;
    public showLocals: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _TaxReportLineStatusListService: TaxReportLineStatusListService = new TaxReportLineStatusListService();

    ReportLines: ObservableCollection;
    OriginalReportLines: ObservableCollection;
    isReady: boolean = false;
    ShowErrorMsg: boolean = false;
    errorsCount: number = 0;

    constructor(private entityArgs: EntityArgs) {
        super();

        this._entityResourceService.getEntityResourceByTableName("TaxReport").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("TaxReportLine").subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("TaxReportLineTransmitStatus").subscribe((response: any) => {
                    this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => {
                        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe((response: any) => {
                            this.isReady = true;
                        });
                    });
                });
            });
        });


        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;

        this.EntityPM = entityArgs.EntityPM;

        this.SetUIProperty();

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        this.FillGrids();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    }

    ngOnInit() {
        this.GetStatuses();
        this.FillGrids();
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OutputTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LastUpdateDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ExemptTaxableOutput", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxReportMonth", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EquipmentInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OtherInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AmountForPayRefund", this.ObjectTableName, false);
    }

    //#region Properties
    //VatNumber
    //TaxableOutputAmount
    //LastUpdateDate
    //ExemptTaxableOutput
    //TaxReportMonth
    //TaxableOutputAmount
    //EquipmentInputsTaxAmount
    //OtherInputsTaxAmount
    //AmountForPayRefund

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(value: string) {
        if (this.EntityPM.VatNumber != value) {
            this.EntityPM.VatNumber = value;
        }
    }

    get OutputTaxAmount() { return this.EntityPM.OutputTaxAmount; }
    set OutputTaxAmount(value: number) {
        if (this.EntityPM.OutputTaxAmount != value) {
            this.EntityPM.OutputTaxAmount = value;
        }
    }

    get TaxableOutputAmount() { return this.EntityPM.TaxableOutputAmount; }
    set TaxableOutputAmount(value: number) {
        if (this.EntityPM.TaxableOutputAmount != value) {
            this.EntityPM.TaxableOutputAmount = value;
        }
    }

    get LastUpdateDate() { return this.EntityPM.LastUpdateDate; }
    set LastUpdateDate(value: Date) {
        if (this.EntityPM.LastUpdateDate != value) {
            this.EntityPM.LastUpdateDate = value;
        }
    }

    get ExemptTaxableOutput() { return this.EntityPM.ExemptTaxableOutput; }
    set ExemptTaxableOutput(value: number) {
        if (this.EntityPM.ExemptTaxableOutput != value) {
            this.EntityPM.ExemptTaxableOutput = value;
        }
    }

    get TaxReportMonth() { return this.EntityPM.TaxReportMonth; }
    set TaxReportMonth(value: Date) {
        if (this.EntityPM.TaxReportMonth != value) {
            this.EntityPM.TaxReportMonth = value;
        }
    }

    get EquipmentInputsTaxAmount() { return this.EntityPM.EquipmentInputsTaxAmount; }
    set EquipmentInputsTaxAmount(value: number) {
        if (this.EntityPM.EquipmentInputsTaxAmount != value) {
            this.EntityPM.EquipmentInputsTaxAmount = value;
        }
    }

    get OtherInputsTaxAmount() { return this.EntityPM.OtherInputsTaxAmount; }
    set OtherInputsTaxAmount(value: number) {
        if (this.EntityPM.OtherInputsTaxAmount != value) {
            this.EntityPM.OtherInputsTaxAmount = value;
        }
    }

    get AmountForPayRefund() { return this.EntityPM.AmountForPayRefund; }
    set AmountForPayRefund(value: number) {
        if (this.EntityPM.AmountForPayRefund != value) {
            this.EntityPM.AmountForPayRefund = value;
        }
    }


    //#endregion

    //#region Filter Methods
    TaxableTransactionsCount: number = 0;
    ExemptTransactionsCount: number = 0;
    AllTransactionsCount: number = 0;
    InputsEquipmentsCount: number = 0;
    InputsOtherCount: number = 0;
    AllCount: number = 0;

    public FilterSelectedValue: string = 'All';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
        }
    }

    FilterLines() {

        var filteredLines = [];
        filteredLines = this.OriginalReportLines.Collection;

        //search
        if (!AppTool.IsNullOrEmpty(this.searchText))
            filteredLines = filteredLines.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));

        //update filters count
        this.TaxableTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        this.ExemptTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        this.AllTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        this.InputsEquipmentsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        this.InputsOtherCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        this.AllCount = filteredLines.length;

        //toggle filters
        switch (this.FilterSelectedValue) {
            case "TaxableTransactions": {
                filteredLines = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0);
                break;
            }
            case "ExemptTransactions": {
                filteredLines = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0);
                break;
            }
            case "AllTransactions": {
                filteredLines = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O");
                break;
            }
            case "InputsEquipments": {
                filteredLines = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true);
                break;
            }
            case "InputsOther": {
                filteredLines = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false);
                break;
            }
            case "All": {
                break;
            }
        }

        //filter statuses
        if (this.SelectedStatusItems.length > 0) {
            filteredLines = filteredLines.filter(d => this.SelectedStatusItems.includes(d.StatusCode));
        }





        this.ReportLines = new ObservableCollection([]);
        this.ReportLines.InsertCollection(filteredLines);



    }

    private timerToken: any;
    searchText: string = "";
    TextChanged(searchtext) {

        this.timerToken = setTimeout(() => {
            this.searchText = searchtext;
            this.FilterLines();
        }, 500);

    }
    //#endregion

    //#region Data
    FillGrids() {
        var lines = [];

        this.ReportLines = new ObservableCollection([]);
        this.OriginalReportLines = new ObservableCollection([]);

        if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
            for (let item of this.EntityPM.TaxReportLines.sort((a, b) => { return (a.Line === b.Line) ? 0 : (a.Line < b.Line) ? -1 : 1 })) {
                lines.push(new ReportLineModel(item, this));
            }
        }

        this.ReportLines.InsertCollection(lines);
        this.OriginalReportLines.InsertCollection(lines);

        //calculate sums
        this.TaxableTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        this.ExemptTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        this.AllTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        this.InputsEquipmentsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        this.InputsOtherCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        this.AllCount = lines.length;
        this.errorsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.StatusCode != "6" && d.TaxReportLinePM.TransmitStatusCode == "1").length;
        this.ShowErrorMsg = this.errorsCount > 0;

    }

    StatusItems = [];
    SelectedStatusItems = [];
    GetStatuses() {
        this._TaxReportLineStatusListService.getAll().subscribe((myResult) => {
            this.StatusItems = myResult.Result;
        });
    }
    PushStatus(status) {
        this.SelectedStatusItems.push(status.Code);
        this.FilterLines();
    }
    PopStatus(status) {
        var itemIndex = this.SelectedStatusItems.indexOf(status.Code);
        if (itemIndex > -1)
            this.SelectedStatusItems.splice(itemIndex, 1);
        this.FilterLines();
    }

    //#endregion

    RefreshButtonClicked() {

        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        //this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
    }

    EditLine(line: ReportLineModel) {
        if (line) {
            var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + line.Line;

            var windowArgs: any = {};
            windowArgs.TaxReportPM = this.EntityPM;
            windowArgs.TaxReportLinePM = line.TaxReportLinePM;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 350;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event == "ok")
                    this.ReloadScreen();
            });
            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');
        }




    }

    ReloadScreen() {
        this.RefreshButtonClicked();
    }

    GetErrorMsg() {
        var msg = TextCodeTranslator.Translate("Accounting.O.TaxReportErrorMsg");
        return msg.replace("#Number", this.errorsCount.toString());
    }
}


class ReportLineModel extends BaseComponent {
    public TaxReportLinePM: TaxReportLinePM = null;
    public ObjectTableName = "TaxReportLine";
    public RowIndex: number;
    public DataContext = this;
    public isRTL: boolean = false;

    constructor(private taxReportLinePM: TaxReportLinePM, private parent: TaxReportDetailsTabComponent)
    {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.TaxReportLinePM = taxReportLinePM;

    }

    //#region Properties

    public get Tenant() { return this.TaxReportLinePM.Tenant; }
    public get LastUpdateDateTime() { return this.TaxReportLinePM.LastUpdateDateTime; }
    public get UpdatedByUserId() { return this.TaxReportLinePM.UpdatedByUserId; }
    public get SearchFields() { return this.TaxReportLinePM.SearchFields; }
    public get TaxReportId() { return this.TaxReportLinePM.TaxReportId; }
    public get Line() { return this.TaxReportLinePM.Line; }
    public get OutputOrInput() { return this.TaxReportLinePM.OutputOrInput; }
    public get LineTypeCode() { return this.TaxReportLinePM.LineTypeCode; }
    public get VatNumber() { return this.TaxReportLinePM.VatNumber; }
    public get Reference() { return this.TaxReportLinePM.Reference; }
    public get ReferecneGroup() { return this.TaxReportLinePM.ReferecneGroup; }
    public get ReferenceDate() { return this.TaxReportLinePM.ReferenceDate; }
    public get VatAmount() { return this.TaxReportLinePM.VatAmount; }
    public get VatableInvoiceAmount() { return this.TaxReportLinePM.VatableInvoiceAmount; }
    public get StatusCode() { return this.TaxReportLinePM.StatusCode; }
    public get TransmitStatusCode() { return this.TaxReportLinePM.TransmitStatusCode; }
    public get JournalId() { return this.TaxReportLinePM.JournalId; }
    public get JournalNumber() { return this.TaxReportLinePM.JournalNumber; }
    public get IsManuallyChanged() { return this.TaxReportLinePM.IsManuallyChanged; }
    public get IsEquipment() { return this.TaxReportLinePM.IsEquipment; }
    public get StatusEnglishName() { return this.TaxReportLinePM.StatusEnglishName; }
    public get StatusLocalName() { return this.TaxReportLinePM.StatusLocalName; }

    //#endregion

    OpenJournal() {
        var id = this.JournalId;
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk =>
                    {

                    });
                });
        }
    }
}
