import { Component, OnDestroy, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { TariffDomainService } from '../../../Services/TariffDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffLineExpirationDatePM } from '../../../EntityPMs/TariffLineExpirationDatePM';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { MeasurementListService } from '../../../../Common/Services/StandardLists/MeasurementListService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { MeasurementList } from '../../../../Common/EntityLists/MeasurementList';
import { DatePipe } from '@angular/common';
import { TariffVersionExtendedPMService } from '../../../Services/ExtendedPMs/TariffVersionExtendedPMService';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { UpdateTariffArgs } from '../../../Args';
import { AirSurchargeTariffLineData } from '../../../../TariffModule/Components/EditTabs/Tariff/TariffLineData';

@Component({
    
    templateUrl: './SurchargeVersionTabComponent.html',
})

export class SurchargeVersionTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public TariffsLinesSource: ObservableCollection;
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    private TariffDomainService: TariffDomainService;
    public IsApproveVersionButtonVisible: boolean = false;
    public IsDraftVersion: boolean = true;
    public CurrentVersion: TariffVersionPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsUpdateSurchargesButtonVisible: boolean = false;
    public IsFirstDraft: boolean = false;
    public SelectedVersionNumber: number;
    public OriginDependencyFilterValue: string = "A";
    public DestinationDependencyFilterValue = "A";
    public IsAir: boolean = false;
    public selectedRow: any;
    public changeScrollPosition: EventEmitter<any> = new EventEmitter();
    private deletedLinesExpirationDates: TariffLineExpirationDatePM[];
    public LinesCount: number;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.GetTariffType();
        this.Listen();
    }

    GetTariffType() {
        if (this.EntityPM.TypeCode == "ASC") { 
            this.IsAir = true;
        }
    }

    GetDisplayMemberPath() {
        return this.IsAir ? "Code" : "CombinedCode";
    }

    SetOriginDependencyFilterValue() {
        if (this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
        }
    }

    public AllChargesTypes: ChargesTypeList[];
    public AllMeasurements: MeasurementList[];
    public LineIdFromPriceCheck: string;
    Intialize(args: any) {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.TariffDomainService = new TariffDomainService();
        this.deletedLinesExpirationDates = [];

        this.CurrentVersion = args['CurrentVersion'];
        this.SelectedVersionNumber = args['SelectedVersionNumber'];
        this.LineIdFromPriceCheck = args['LineIdFromPriceCheck'];
        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
        }

        if (this.IsDraftVersion) {
            this.IsComparToChecked = true;
        }

        this.GetTariffSettings();


        var iChargesTypeListService = new ChargesTypeListService();
        var iMeasurementListService = new MeasurementListService();

        iChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;

                iMeasurementListService.getAllFromCache().subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.AllMeasurements = myResponse2.Result;

                        this.LoadCompareToVersions();

                        this.SetUIProperties();
                        this.SetSurchargesLabelsAndVisibility();

                        if (this.CurrentVersion.IsDraft) {
                            this.FillTariffLines(this.CurrentVersion.TariffLines);
                        }

                        else {
                            this.LoadTariffLines("currentVersion");
                        }
                    }
                });
            }
        });
        this.SetOriginDependencyFilterValue();
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    this.CurrentVersion = this.EntityPM.TariffVersions.filter(d => d.Version == this.SelectedVersionNumber)[0];

                    if (this.CurrentVersion == null) {
                        this.CurrentVersion = this.EntityPM.ActiveVersions.filter(d => d.Version == this.SelectedVersionNumber)[0];   
                    }

                    if (this.CurrentVersion.IsDraft) {
                        this.FillTariffLines(this.CurrentVersion.TariffLines);
                    }
                    else {
                        this.LoadTariffLines("currentVersion");
                    }
                    
                    if (this.isApproveButtonClicked) {
                        this.isApproveButtonClicked = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    if (this.isCopyButtonClicked) {
                        this.isCopyButtonClicked = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    if (this.isTariffLinesDeleted) {
                        this.isTariffLinesDeleted = false;
                        this.CurrentSession.FireEvent("TariffLinesDeleted");                       
                    }

                    if (this.isUpdateButtonClicked) {
                        this.isUpdateButtonClicked = false;
                        this.StartUpdateSurcharges();
                    }
  
                    this.SetSurchargesLabelsAndVisibility();
                }

                else {
                    this.StopAllFlags();
                }
            });
        }
    }

    private StopAllFlags() {
        if (this.EntityPM.IsApprovingDraftVersion) {
            this.EntityPM.IsApprovingDraftVersion = false;
        }

        this.isApproveButtonClicked = false;
        this.isTariffLinesDeleted = false;
    }
    
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    }

    private GetTariffSettings() {
        this.TariffDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.warningPercentage = myResponse.Result.DefaultWarningPercentage;
            }
        });
    }
    private loadedTariffLines: TariffLinePM[];
    private compareTariffLines: TariffLinePM[];
    private LoadTariffLines(type: string) {
        this.CurrentSession.StartBusyIndicatorLoading();

        if (type == "currentVersion") {
            this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.CurrentVersion.Version).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    this.loadedTariffLines = response.Result;

                    this.FillTariffLines(this.loadedTariffLines);
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }

        else if (type == "compareVersion") {
            this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.ComparedToVersionPM.Version).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    this.compareTariffLines = response.Result;

                    this.DoCompare();  
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    SetUIProperties() {
        var isApproveVersionButtonVisible: boolean = false;
        var isUpdateSurchargesButtonVisible: boolean = false;

        if (this.IsDraftVersion) {
            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
                isApproveVersionButtonVisible = true;
            }

            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "UPDATESURCHARGES")) {
                isUpdateSurchargesButtonVisible = true;
            }
        }

        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
        this.IsUpdateSurchargesButtonVisible = isUpdateSurchargesButtonVisible;
    }

    public Surcharge1PriceLabel: string;
    public Surcharge2PriceLabel: string;
    public Surcharge3PriceLabel: string;
    public Surcharge4PriceLabel: string;
    public Surcharge5PriceLabel: string;
    public Surcharge6PriceLabel: string;
    public Surcharge7PriceLabel: string;
    public Surcharge8PriceLabel: string;
    public Surcharge9PriceLabel: string;
    public Surcharge10PriceLabel: string;

    public Surcharge1MinPriceLabel: string;
    public Surcharge2MinPriceLabel: string;
    public Surcharge3MinPriceLabel: string;
    public Surcharge4MinPriceLabel: string;
    public Surcharge5MinPriceLabel: string;
    public Surcharge6MinPriceLabel: string;
    public Surcharge7MinPriceLabel: string;
    public Surcharge8MinPriceLabel: string;
    public Surcharge9MinPriceLabel: string;
    public Surcharge10MinPriceLabel: string;

    public Surcharge1PriceVisibility: boolean;
    public Surcharge2PriceVisibility: boolean;
    public Surcharge3PriceVisibility: boolean;
    public Surcharge4PriceVisibility: boolean;
    public Surcharge5PriceVisibility: boolean;
    public Surcharge6PriceVisibility: boolean;
    public Surcharge7PriceVisibility: boolean;
    public Surcharge8PriceVisibility: boolean;
    public Surcharge9PriceVisibility: boolean;
    public Surcharge10PriceVisibility: boolean;

    public Surcharge1MinPriceVisibility: boolean;
    public Surcharge2MinPriceVisibility: boolean;
    public Surcharge3MinPriceVisibility: boolean;
    public Surcharge4MinPriceVisibility: boolean;
    public Surcharge5MinPriceVisibility: boolean;
    public Surcharge6MinPriceVisibility: boolean;
    public Surcharge7MinPriceVisibility: boolean;
    public Surcharge8MinPriceVisibility: boolean;
    public Surcharge9MinPriceVisibility: boolean;
    public Surcharge10MinPriceVisibility: boolean;

    public Surcharge1PricePercentageVisibility: boolean;
    public Surcharge2PricePercentageVisibility: boolean;
    public Surcharge3PricePercentageVisibility: boolean;
    public Surcharge4PricePercentageVisibility: boolean;
    public Surcharge5PricePercentageVisibility: boolean;
    public Surcharge6PricePercentageVisibility: boolean;
    public Surcharge7PricePercentageVisibility: boolean;
    public Surcharge8PricePercentageVisibility: boolean;
    public Surcharge9PricePercentageVisibility: boolean;
    public Surcharge10PricePercentageVisibility: boolean;

    private tariffCharges: CodeNameClass[] = [];
    SetSurchargesLabelsAndVisibility() {
        this.tariffCharges = [];
        this.AddChargeColumn(this.EntityPM.Surcharge1Id, this.EntityPM.Surcharge1UOM, 1);
        this.AddChargeColumn(this.EntityPM.Surcharge2Id, this.EntityPM.Surcharge2UOM, 2);
        this.AddChargeColumn(this.EntityPM.Surcharge3Id, this.EntityPM.Surcharge3UOM, 3);
        this.AddChargeColumn(this.EntityPM.Surcharge4Id, this.EntityPM.Surcharge4UOM, 4);
        this.AddChargeColumn(this.EntityPM.Surcharge5Id, this.EntityPM.Surcharge5UOM, 5);
        this.AddChargeColumn(this.EntityPM.Surcharge6Id, this.EntityPM.Surcharge6UOM, 6);
        this.AddChargeColumn(this.EntityPM.Surcharge7Id, this.EntityPM.Surcharge7UOM, 7);
        this.AddChargeColumn(this.EntityPM.Surcharge8Id, this.EntityPM.Surcharge8UOM, 8);
        this.AddChargeColumn(this.EntityPM.Surcharge9Id, this.EntityPM.Surcharge9UOM, 9);
        this.AddChargeColumn(this.EntityPM.Surcharge10Id, this.EntityPM.Surcharge10UOM, 10);
    }

    AddChargeColumn(iChargeTypeId: string, iMeasurementId:string, index: number) {
        if (!AppTool.IsNullOrEmpty(iChargeTypeId)) {
            var iChargeType: ChargesTypeList = this.AllChargesTypes.filter(a => a.Id == iChargeTypeId)[0];
            if (iChargeType) {

                var item: CodeNameClass = new CodeNameClass();
                item.Code = iChargeType.Id;
                item.Name = iChargeType.EnglishName;
                item.DisplyText = iChargeType.EnglishName;
                item.Code_Int = index;                

                var isMeasurmentFixed: boolean = false;
                var iMeasurement: MeasurementList = this.AllMeasurements.filter(f => f.Id == iMeasurementId)[0];
                if (iMeasurement) {
                    item.DisplyText = iChargeType.EnglishName + " (" + iMeasurement.Code + ")";
                    item.AdditionalField = iMeasurement.Code;

                    if (iMeasurement.Code == "FIXD") {
                        isMeasurmentFixed = true;
                    }

                    if (iMeasurement.Code == 'PRFR' || iMeasurement.Code == 'PRVL') {
                        this['Surcharge' + index + 'PricePercentageVisibility'] = true;
                    } else {
                        this['Surcharge' + index + 'PricePercentageVisibility'] = false;
                    }
                }

                this.tariffCharges.push(item);

                this['Surcharge' + index + 'PriceLabel'] = item.DisplyText;
                this['Surcharge' + index + 'PriceVisibility'] = true;
                this['Surcharge' + index + 'MinPriceVisibility'] = !isMeasurmentFixed;
                this['Surcharge' + index + 'MinPriceLabel'] = "Min " + iChargeType.EnglishName;               
            }
        }
    }

    get VersionNumber() {
        return (this.CurrentVersion == null ? null : this.CurrentVersion.Version);
    }

    get StartDate() {
        return (this.CurrentVersion == null ? null : this.CurrentVersion.StartDate);
    }
    set StartDate(value: Date) {
        if (this.CurrentVersion.StartDate != value) {
            this.CurrentVersion.StartDate = value;

            this.UpdateDates("start", value);
        }
    }

    get ExpirationDate() {
        return (this.CurrentVersion == null ? null : this.CurrentVersion.ExpirationDate);
    }
    set ExpirationDate(value: Date) {
        if (this.CurrentVersion.ExpirationDate != value) {
            this.CurrentVersion.ExpirationDate = value;

            this.UpdateDates("expire", value);
        }
    }

    private UpdateDates(dateType: string, date: Date) {
        if (dateType == "start") {
            this.EntityPM.LastStartDate = date;

            this.CurrentVersion.TariffLines.forEach(item => {
                item.StartDate = date;
            });
        }

        else if (dateType == "expire") {
            this.EntityPM.LastExpirationDate = date;

            this.CurrentVersion.TariffLines.forEach(item => {
                item.ExpirationDate = date;
            });
        }
    }

    private ItemsCollection: AirSurchargeTariffLineData[] = [];
    public DeletedTariffsLines: AirSurchargeTariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }

        this.ItemsCollection = [];        
        var count = 0; var selectedIndexRow = 0; var isItemSelectExist = false;
        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var itemSurchargeAir = new AirSurchargeTariffLineData(item, this)
            count++;
            this.ItemsCollection.push(itemSurchargeAir);
            if (!AppTool.IsNullOrEmpty(this.LineIdFromPriceCheck) && itemSurchargeAir.EntityPM.Id == this.LineIdFromPriceCheck) {
                this.selectedRow = itemSurchargeAir;
                selectedIndexRow = count;
                isItemSelectExist = true;
            }
        });

        this.TariffsLinesSource.InsertCollection(this.ItemsCollection);
      this.LinesCount = this.TariffsLinesSource.Length;
        this.DoCompare();

        if (isItemSelectExist) {
            this.changeScrollPosition.emit({
                RowIndex: selectedIndexRow
            });
        }
    }

    private DoCompare() {
        this.DeletedTariffsLines = [];
        if (this.IsComparToChecked && this.ComparedToVersionPM != null) {
            this.ComaredLines();
            this.BuildDeletedLines();
        }
    }

    ComaredLines() {
        this.ItemsCollection.forEach((item: AirSurchargeTariffLineData) => {
            item.IsNewEntity = false;
            var line = this.compareTariffLines.sort((a, b) => a.Index - b.Index).filter(a => a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId)[0];
            if (line) {
                item.ComparedEntity = line;
                item.SetCellsComparingText();
            }

            else {
                item.IsNewEntity = true;
            }
        });
    }
    BuildDeletedLines() {
        var lines: TariffLinePM[];
        if (this.CurrentVersion.IsDraft) {
            lines = this.CurrentVersion.TariffLines;
        }
        else {
            lines = this.loadedTariffLines;
        }

        this.compareTariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var line = lines.sort((a, b) => a.Index - b.Index).filter(a => a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId)[0];
            if (line == null) {
                this.DeletedTariffsLines.push(new AirSurchargeTariffLineData(item, this));// Deleted 
            }
        });
    }

    public isComparToChecked = false;
    get IsComparToChecked() {
        return this.isComparToChecked;
    }
    set IsComparToChecked(value: boolean) {
        if (this.isComparToChecked != value) {
            this.isComparToChecked = value;
            this.UIProperties.SetEnabled("WarningPercentage", null, value);
            this.ComparingCalculations(false);
        }
    }

    public warningPercentage: number;
    get WarningPercentage() {
        return this.warningPercentage;
    }
    set WarningPercentage(value: number) {
        if (this.warningPercentage != value) {
            this.warningPercentage = value;
            this.ComparingCalculations(false);
        }
    }

    private selectedVersion: VersionClass;
    get SelectedVersion() { return this.selectedVersion; }
    set SelectedVersion(value: VersionClass) {
        if (this.selectedVersion != value) {
            this.selectedVersion = value;
            this.ComparedToVersionPM = this.compareToVersions.filter(d => d.Version == this.SelectedVersion.Version)[0];
            this.ComparingCalculations(true);
        }

    }

    private compareToVersions: TariffVersionPM[];
    private LoadCompareToVersions() {
        var service: TariffVersionExtendedPMService = new TariffVersionExtendedPMService();
        service.GetAllTariffVersionsForTariff(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.compareToVersions = response.Result;
                this.BuildVersionsList();
            }
        });
    }

    public VersionsList: VersionClass[];
    public ComparedToVersionPM: TariffVersionPM;
    private BuildVersionsList() {
        this.VersionsList = [];
        var datePipe: DatePipe = new DatePipe("en-US");

        this.compareToVersions.filter(a => a.Version != this.CurrentVersion.Version).forEach(item => {

            var newVersion: VersionClass = new VersionClass();
            newVersion.Version = item.Version;
            newVersion.ParentVersionNumber = item.ParentVersionNumber;
            newVersion.Id = item.TariffId;

            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
                newVersion.Name = "Version " + item.Version;
            }

            else {
                var from: string = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
                var to: string = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');
                newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";
            }

            this.VersionsList.push(newVersion);

        });

        this.SelectedVersion = this.VersionsList.filter(a => a.Version == this.CurrentVersion.ParentVersionNumber)[0];
        if (this.VersionsList == null || (this.VersionsList != null && this.VersionsList.length == 0)) {
            this.isComparToChecked = false;
            this.IsFirstDraft = true;
        }

        this.UIProperties.SetEnabled("WarningPercentage", null, this.IsComparToChecked && !this.IsFirstDraft);
    }    

    ComparingCalculations(load: boolean) {
        if (load) {
            this.LoadTariffLines("compareVersion");
        }

        else {
            if (this.CurrentVersion != null && this.CurrentVersion.IsDraft) {
                this.FillTariffLines(this.CurrentVersion.TariffLines);
            }

            else {
                this.FillTariffLines(this.loadedTariffLines);
            }
        }
    }

    AddTariffLine() {
        var logWindow = new LogitudeWindow();
        var itemPM = new TariffLinePM(null);
        itemPM.StartDate = this.StartDate;
        itemPM.ExpirationDate = this.ExpirationDate;
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.Version = this.CurrentVersion.Version;
        itemPM.Index = 0;
        itemPM.CurrencyId = this.EntityPM.CurrencyId;

        var Version: TariffVersionPM = this.EntityPM.TariffVersions.filter(p => p.Version == itemPM.Version)[0];
        if (Version) {
            if (Version.TariffLines.length > 0) {
                var index = Math.max.apply(Math, Version.TariffLines.map(function (o) { return o.Index; })) + 1;
                if (index) {
                    itemPM.Index = index;
                }
            }
        }

        var itemComponent = new AirSurchargeTariffLineData(itemPM, this, true);
        logWindow.WindowArgs = { DataContext: itemComponent, EntityPM: itemPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: AirSurchargeTariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: item, EntityPM: item.EntityPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }
        
    private isTariffLinesDeleted: boolean = false;
    DeleteTariffButtonClicked(item: AirSurchargeTariffLineData) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Tariff Line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var logWindow = new LogitudeWindow();
                logWindow.Width = 450;
                logWindow.Height = 200;
                logWindow.WindowArgs = { CurrentLine: item.EntityPM, TariffType: this.EntityPM.TypeCode, };
                logWindow.Title = "Expiration Date";

                logWindow.ComponentLoaded.subscribe(s => {
                    logWindow.WindowClosed.subscribe(d => {
                        if (s && d == "ok") {
                            var deletedItem: TariffLineExpirationDatePM = new TariffLineExpirationDatePM();
                            deletedItem.OriginPortId = item.EntityPM.OriginPortId;
                            deletedItem.DestinationPortId = item.EntityPM.DestinationPortId;
                            deletedItem.ExpirationDate = item.EntityPM.ExpirationDate;
                            
                            this.deletedLinesExpirationDates.push(deletedItem);
                            this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                            this.TariffsLinesSource.Remove(item);
                            this.FillTariffLines(this.CurrentVersion.TariffLines);

                            this.isTariffLinesDeleted = true;
                        }
                    });
                });

                logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
            }
        });
    }

    // Download Excel 
    DownloadExcelClicked(type: string) {
        this.TariffDomainService.DownloadTariff(this.EntityPM.Id, this.CurrentVersion.Version, type).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + fileName;
                {
                    window.open(url);
                }
            }
        });
    }

    private isApproveButtonClicked: boolean = false;
    ApproveVersionClicked() {
        if (!this.isApproveButtonClicked) {
            this.isApproveButtonClicked = true;

            if (this.deletedLinesExpirationDates.length > 0) {
                this.deletedLinesExpirationDates.forEach(item => {
                    this.EntityPM.DeletedLinesExpirationDates.push(item);
                });
            }

            this.EntityPM.IsApprovingDraftVersion = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }        
    }

    CopyVersionClicked() {
        if (this.EntityPM.TariffVersions.filter(d => d.IsDraft)[0]) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("You can't copy this version since you already have draft one");
        }

        else {
            this.DoCopy();
        }
    }

    private isCopyButtonClicked: boolean = false;
    private DoCopy() {
        this.isCopyButtonClicked = true;

        this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
        this.EntityPM.IsFromCopy = true;
        this.EntityPM.LastStartDate = this.StartDate;
        this.EntityPM.LastExpirationDate = this.ExpirationDate;
        
        var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.CurrentVersion.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
        copiedVersion.IsDraft = true;
        copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.CurrentVersion.Version;
        this.EntityPM.AddTariffVersion(copiedVersion);

        this.loadedTariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var tariffLine = new TariffLinePM(copiedVersion);
            tariffLine.StartDate = item.StartDate;
            tariffLine.Tenant = SessionLocator.Tenant;
            tariffLine.Version = copiedVersion.Version;
            tariffLine.OriginPortId = item.OriginPortId;
            tariffLine.OriginPortCode = item.OriginPortCode;
            tariffLine.OriginPortName = item.OriginPortName;
            tariffLine.DestinationPortId = item.DestinationPortId;
            tariffLine.DestinationPortCode = item.DestinationPortCode;
            tariffLine.DestinationPortName = item.DestinationPortName;
            tariffLine.Surcharge1Price = item.Surcharge1Price;
            tariffLine.Surcharge2Price = item.Surcharge2Price;
            tariffLine.Surcharge3Price = item.Surcharge3Price;
            tariffLine.Surcharge4Price = item.Surcharge4Price;
            tariffLine.Surcharge5Price = item.Surcharge5Price;
            tariffLine.Surcharge6Price = item.Surcharge6Price;
            tariffLine.Surcharge7Price = item.Surcharge7Price;
            tariffLine.Surcharge8Price = item.Surcharge8Price;
            tariffLine.Surcharge9Price = item.Surcharge9Price;
            tariffLine.Surcharge10Price = item.Surcharge10Price;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            tariffLine.CurrencyId = item.CurrencyId;
            tariffLine.CurrencyCode = item.CurrencyCode;
            tariffLine.IsFromAllOtherPorts = item.IsFromAllOtherPorts;
            tariffLine.IsToAllOtherPorts = item.IsToAllOtherPorts;
            tariffLine.Surcharge1MinPrice = item.Surcharge1MinPrice;
            tariffLine.Surcharge2MinPrice = item.Surcharge2MinPrice;
            tariffLine.Surcharge3MinPrice = item.Surcharge3MinPrice;
            tariffLine.Surcharge4MinPrice = item.Surcharge4MinPrice;
            tariffLine.Surcharge5MinPrice = item.Surcharge5MinPrice;
            tariffLine.Surcharge6MinPrice = item.Surcharge6MinPrice;
            tariffLine.Surcharge7MinPrice = item.Surcharge7MinPrice;
            tariffLine.Surcharge8MinPrice = item.Surcharge8MinPrice;
            tariffLine.Surcharge9MinPrice = item.Surcharge9MinPrice;
            tariffLine.Surcharge10MinPrice = item.Surcharge10MinPrice;
            copiedVersion.AddTariffLine(tariffLine);
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
    }

    private isUpdateButtonClicked: boolean = false;
    UpdateSurchargesClicked() {
        this.isUpdateButtonClicked = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    private StartUpdateSurcharges() {
        var args: UpdateTariffArgs = new UpdateTariffArgs();
        args.Version = this.CurrentVersion;
        args.TariffCharges = this.tariffCharges;
        args.CarrierId = this.EntityPM.SellerId;
        args.TypeCode = this.EntityPM.TypeCode;
        args.FatherComponent = this;

        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.WindowArgs = args;
        logWindow.Title = "Tariff Surcharge Update";

        logWindow.WindowClosed.subscribe((s: any) => {
            if (s == "ok") {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });

        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UpdateSurchargesComponent');
    }

    private isAllSelected: boolean = false;
    get IsAllSelected() { return this.isAllSelected; }
    set IsAllSelected(value: boolean) {
        if (this.isAllSelected != value) {
            this.isAllSelected = value;

            this.ItemsCollection.forEach((item: AirSurchargeTariffLineData) => {
                item.IsLineSelected = value;
            });
        }
    }

    DeleteLinesClicked() {
        if (this.ItemsCollection.filter(f => f.IsLineSelected).length == 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please select lines you would like to delete");
        }

        else {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Selected lines will be deleted");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.ItemsCollection.filter(d => d.IsLineSelected).forEach((item: AirSurchargeTariffLineData) => {
                        this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                    });

                    this.FillTariffLines(this.CurrentVersion.TariffLines);
                }
            });
        }
    }
}

export class VersionClass {
    public Code: number;
    public Name: string;
    public Id: string;
    public Version: number;
    public ParentVersionNumber: number;
}
