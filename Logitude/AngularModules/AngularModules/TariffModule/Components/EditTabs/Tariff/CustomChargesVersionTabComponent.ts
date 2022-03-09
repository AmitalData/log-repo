import { Component, OnDestroy, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
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
import { AppTool, DateTool, FontTool, FormatTool } from '../../../../Infrastructure/Tools';
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
import { CountryList } from '../../../../Common/EntityLists/CountryList';
import { AddressList } from '../../../../Common/EntityLists/AddressList';
import { AddressListService } from '../../../../Common/Services/StandardLists/AddressListService';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';

@Component({
    templateUrl: './CustomChargesVersionTabComponent.html',
})

export class CustomChargesVersionTabComponent extends BaseComponent implements OnDestroy {
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
    public IsFirstDraft: boolean = false;
    public SelectedVersionNumber: number;
    public selectedRow: any;
    public changeScrollPosition: EventEmitter<any> = new EventEmitter();
    private deletedLinesExpirationDates: TariffLineExpirationDatePM[];
    public LinesCount: number;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    public AllChargesTypes: ChargesTypeList[];
    public AllMeasurements: MeasurementList[];
    public AllCurrencies: CurrencyList[];
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
        var currencyListService = new CurrencyListService();

        iChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;

                iMeasurementListService.getAllFromCache().subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.AllMeasurements = myResponse2.Result;

                        currencyListService.getAllFromCache().subscribe((myResponse3: ServiceResponse) => {
                            this.AllCurrencies = myResponse3.Result;

                            this.LoadCompareToVersions();
                            this.SetUIProperties();
                            this.SetSurchargesLabelsAndVisibility();

                            if (this.CurrentVersion.IsDraft) {
                                this.FillTariffLines(this.CurrentVersion.TariffLines);
                            }

                            else {
                                this.LoadTariffLines("currentVersion");
                            }
                        });
                    }
                });
            }
        });
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

        if (this.IsDraftVersion) {
            if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
                isApproveVersionButtonVisible = true;
            }
        }

        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
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

    SetSurchargesLabelsAndVisibility() {
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

    AddChargeColumn(iChargeTypeId: string, iMeasurementId: string, index: number) {
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
                    item.DisplyText = iChargeType.EnglishName; // + " (" + iMeasurement.Code + ")";
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

    private ItemsCollection: CustomsChargesTariffLineData[] = [];
    public DeletedTariffsLines: CustomsChargesTariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }

        this.ItemsCollection = [];
        var count = 0; var selectedIndexRow = 0; var isItemSelectExist = false;
        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var customsChargesTariffLineData = new CustomsChargesTariffLineData(item, this)
            count++;
            this.ItemsCollection.push(customsChargesTariffLineData);
            if (!AppTool.IsNullOrEmpty(this.LineIdFromPriceCheck) && customsChargesTariffLineData.EntityPM.Id == this.LineIdFromPriceCheck) {
                this.selectedRow = customsChargesTariffLineData;
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
        this.ItemsCollection.forEach((item: CustomsChargesTariffLineData) => {
            item.IsNewEntity = false;
            var line = this.compareTariffLines.sort((a, b) => a.Index - b.Index).filter(a => a.ToCountryId == item.ToCountryId && a.FromCountryId == item.FromCountryId)[0];
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
            var line = lines.sort((a, b) => a.Index - b.Index).filter(a => a.ToCountryId == item.ToCountryId && a.FromCountryId == item.FromCountryId)[0];
            if (line == null) {
                this.DeletedTariffsLines.push(new CustomsChargesTariffLineData(item, this));
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
            newVersion.Name = "Version " + item.Version;
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
        var addressService: PartnersDomainService = new PartnersDomainService();
        addressService.GetMainAddressListByCardId(this.EntityPM.CustomsBrokerId).subscribe((response: any) => {
            if (!response.HasError) {
                var customsBrokerAddress = response;

                var logWindow = new LogitudeWindow();
                var itemPM = new TariffLinePM(null);
                itemPM.StartDate = this.StartDate;
                itemPM.ExpirationDate = this.ExpirationDate;
                itemPM.Tenant = SessionLocator.Tenant;
                itemPM.Version = this.CurrentVersion.Version;
                itemPM.Index = 0;
                itemPM.CurrencyId = this.EntityPM.CurrencyId;

                if (this.EntityPM.TypeCode == "ECC") {
                    itemPM.FromCountryId = customsBrokerAddress.CountryId;
                    itemPM.FromCountryCode = customsBrokerAddress.CountryCode;
                    itemPM.FromCountryName = customsBrokerAddress.CountryName;
                }

                else if (this.EntityPM.TypeCode == "ICC") {
                    itemPM.ToCountryId = customsBrokerAddress.CountryId;
                    itemPM.ToCountryCode = customsBrokerAddress.CountryCode;
                    itemPM.ToCountryName = customsBrokerAddress.CountryName;
                }

                var Version: TariffVersionPM = this.EntityPM.TariffVersions.filter(p => p.Version == itemPM.Version)[0];
                if (Version) {
                    if (Version.TariffLines.length > 0) {
                        var index = Math.max.apply(Math, Version.TariffLines.map(function (o) { return o.Index; })) + 1;
                        if (index) {
                            itemPM.Index = index;
                        }
                    }
                }

                var itemComponent = new CustomsChargesTariffLineData(itemPM, this, true);
                logWindow.WindowArgs = { DataContext: itemComponent, EntityPM: itemPM, TariffType: this.EntityPM.TypeCode };
                logWindow.Title = "New Tariff Line";
                logWindow.Width = 800;
                logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
            }
        });
    }

    EditTariffButtonClicked(item: CustomsChargesTariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: item, EntityPM: item.EntityPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "Edit Tariff Line";
        logWindow.Width = 800;
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    private isTariffLinesDeleted: boolean = false;
    DeleteTariffButtonClicked(item: CustomsChargesTariffLineData) {
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
                            deletedItem.FromCountryId = item.EntityPM.FromCountryId;
                            deletedItem.ToCountryId = item.EntityPM.ToCountryId;
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
            tariffLine.FromCountryId = item.FromCountryId;
            tariffLine.FromCountryCode = item.FromCountryCode;
            tariffLine.FromCountryName = item.FromCountryName;
            tariffLine.ToCountryId = item.ToCountryId;
            tariffLine.ToCountryCode = item.ToCountryCode;
            tariffLine.ToCountryName = item.ToCountryName;
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
            tariffLine.IsFromAllOtherCountries = item.IsFromAllOtherCountries;
            tariffLine.IsToAllOtherCountries = item.IsToAllOtherCountries;
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
            tariffLine.IsDifferentCurrenciesPerCharge = item.IsDifferentCurrenciesPerCharge;
            tariffLine.Surcharge1CurrencyId = item.Surcharge1CurrencyId;
            tariffLine.Surcharge2CurrencyId = item.Surcharge2CurrencyId;
            tariffLine.Surcharge3CurrencyId = item.Surcharge3CurrencyId;
            tariffLine.Surcharge4CurrencyId = item.Surcharge4CurrencyId;
            tariffLine.Surcharge5CurrencyId = item.Surcharge5CurrencyId;
            tariffLine.Surcharge6CurrencyId = item.Surcharge6CurrencyId;
            tariffLine.Surcharge7CurrencyId = item.Surcharge7CurrencyId;
            tariffLine.Surcharge8CurrencyId = item.Surcharge8CurrencyId;
            tariffLine.Surcharge9CurrencyId = item.Surcharge9CurrencyId;
            tariffLine.Surcharge10CurrencyId = item.Surcharge10CurrencyId;
            copiedVersion.AddTariffLine(tariffLine);
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
    }

    private isAllSelected: boolean = false;
    get IsAllSelected() { return this.isAllSelected; }
    set IsAllSelected(value: boolean) {
        if (this.isAllSelected != value) {
            this.isAllSelected = value;

            this.ItemsCollection.forEach((item: CustomsChargesTariffLineData) => {
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
                    this.ItemsCollection.filter(d => d.IsLineSelected).forEach((item: CustomsChargesTariffLineData) => {
                        this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                    });

                    this.FillTariffLines(this.CurrentVersion.TariffLines);
                }
            });
        }
    }
}

export class CustomsChargesTariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: CustomsChargesTariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;
    private initialIndex: number;
    private lineCurrencyId: string;
    public Surcharge1CurrencyMeasurementLabel: string;
    public Surcharge2CurrencyMeasurementLabel: string;
    public Surcharge3CurrencyMeasurementLabel: string;
    public Surcharge4CurrencyMeasurementLabel: string;
    public Surcharge5CurrencyMeasurementLabel: string;
    public Surcharge6CurrencyMeasurementLabel: string;
    public Surcharge7CurrencyMeasurementLabel: string;
    public Surcharge8CurrencyMeasurementLabel: string;
    public Surcharge9CurrencyMeasurementLabel: string;
    public Surcharge10CurrencyMeasurementLabel: string;
    constructor(entity: TariffLinePM, public FatherComponent: CustomChargesVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.initialIndex = entity.Index;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.lineCurrencyId = this.EntityPM.CurrencyId;
        this.SetUIProperties();
        this.SetCellColorsForPriceCheck();
        this.FillAllSurchargesCurrencyMeasurementLabel();
    }

    private FillAllSurchargesCurrencyMeasurementLabel() {
        for (var index = 1; index <= 10; index++) {
            this.FillCurrencyMeasurementLabel(index);
        }
    }

    private FillCurrencyMeasurementLabel(index) {
        var currencyMeasurementLabel = "";
        if (this.FatherComponent.AllMeasurements) {
            var iMeasurement = this.FatherComponent.AllMeasurements.filter(f => f.Id == this.FatherComponent.EntityPM['Surcharge' + index + 'UOM'])[0];
            var currencyCode = this.GetCurrencyCode(index);
            if (iMeasurement) {
                if (iMeasurement.Code == "FIXD") {
                    currencyMeasurementLabel = currencyCode;
                }
                else if (iMeasurement.Code == "PRFR" || iMeasurement.Code == "PRVL" || iMeasurement.Code == "PFCL") {

                    currencyMeasurementLabel = "% " + iMeasurement.Code;
                }
                else {
                    currencyMeasurementLabel = !AppTool.IsNullOrEmpty(currencyCode) ? currencyCode + "/" + iMeasurement.Code : iMeasurement.Code;
                }

                this['Surcharge' + index + 'CurrencyMeasurementLabel'] = currencyMeasurementLabel;
            }
        }
    }
    private GetCurrencyCode(index) {
        var currencyCode = "";
        var currencyId = null;
        if (this.EntityPM != null && !this.IsDifferentCurrenciesPerCharge) {
            currencyId = this.CurrencyId;
        }
        else {
            currencyId = this["Surcharge" + index + "CurrencyId"];
        }
        var currency = this.FatherComponent.AllCurrencies.filter(f => f.Id == currencyId)[0];
        currencyCode = currency != null ? currency.Code : "";
        return currencyCode;
    }
    public CellColor: string = "transparent";
    private SetCellColorsForPriceCheck() {
        if (!AppTool.IsNullOrEmpty(this.FatherComponent.LineIdFromPriceCheck) && this.FatherComponent.LineIdFromPriceCheck == this.EntityPM.Id) {
            this.CellColor = "#f7dc6e";
        }

        else {
            if (this.IsEditEnabled) {
                this.CellColor = "transparent";
            }

            else {
                this.CellColor = "rgba(230, 231, 232, 0.5)";
            }
        }
    }

    private SetUIProperties() {
        this.SetUIProperties_From();
        this.SetUIProperties_To();
        this.SetUIProperties_MinPrices();
        this.SetUIProperties_Currency();
    }
    private SetUIProperties_From() {
        if (this.IsFromAllOtherCountries) {
            this.UIProperties.SetRequired("FromCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("FromCountryId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("FromCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromCountryId));
            this.UIProperties.SetEnabled("FromCountryId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsFromAllOtherCountries", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FromCountryId));
        }
    }
    private SetUIProperties_To() {
        if (this.IsToAllOtherCountries) {
            this.UIProperties.SetRequired("ToCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToCountryId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("ToCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToCountryId));
            this.UIProperties.SetEnabled("ToCountryId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsToAllOtherCountries", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToCountryId));
        }
    }
    private SetUIProperties_Currency() {
        var isDefaultCurrencyRequired: boolean = false;
        var isDefaultCurrencyEnabled: boolean = false;

        if (this.IsDifferentCurrenciesPerCharge) {
            isDefaultCurrencyRequired = false;
            isDefaultCurrencyEnabled = false;
        }
        else {
            isDefaultCurrencyEnabled = true;
            if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
                isDefaultCurrencyRequired = true;
            }
        }

        this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, isDefaultCurrencyRequired);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, isDefaultCurrencyEnabled);

        for (var i = 1; i <= 10; i++) {
            this.UIProperties.SetEnabled("Surcharge" + i + "CurrencyId", this.ObjectTableName, !isDefaultCurrencyEnabled);
        }
    }
    private SetUIProperties_MinPrices() {
        this.UIProperties.SetVisibility("Surcharge1MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(1));
        this.UIProperties.SetVisibility("Surcharge2MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(2));
        this.UIProperties.SetVisibility("Surcharge3MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(3));
        this.UIProperties.SetVisibility("Surcharge4MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(4));
        this.UIProperties.SetVisibility("Surcharge5MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(5));
        this.UIProperties.SetVisibility("Surcharge6MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(6));
        this.UIProperties.SetVisibility("Surcharge7MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(7));
        this.UIProperties.SetVisibility("Surcharge8MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(8));
        this.UIProperties.SetVisibility("Surcharge9MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(9));
        this.UIProperties.SetVisibility("Surcharge10MinPrice", this.ObjectTableName, !this.IsMeasurmentFixed(10));
    }
    private IsMeasurmentFixed(index: number): boolean {
        var isFixed: boolean = false;

        if (this.FatherComponent.AllMeasurements) {
            var iMeasurement = this.FatherComponent.AllMeasurements.filter(f => f.Id == this.FatherComponent.EntityPM['Surcharge' + index + 'UOM'])[0];
            if (iMeasurement) {
                if (iMeasurement.Code == "FIXD") {
                    isFixed = true;
                }
            }
        }

        return isFixed;
    }

    public Surcharge1ComparingPrice: number;
    public Surcharge1ComparingTextColor: string = null;
    public Surcharge2ComparingPrice: number;
    public Surcharge2ComparingTextColor: string = null;
    public Surcharge3ComparingPrice: number;
    public Surcharge3ComparingTextColor: string = null;
    public Surcharge4ComparingPrice: number;
    public Surcharge4ComparingTextColor: string = null;
    public Surcharge5ComparingPrice: number;
    public Surcharge5ComparingTextColor: string = null;
    public Surcharge6ComparingPrice: number;
    public Surcharge6ComparingTextColor: string = null;
    public Surcharge7ComparingPrice: number;
    public Surcharge7ComparingTextColor: string = null;
    public Surcharge8ComparingPrice: number;
    public Surcharge8ComparingTextColor: string = null;
    public Surcharge9ComparingPrice: number;
    public Surcharge9ComparingTextColor: string = null;
    public Surcharge10ComparingPrice: number;
    public Surcharge10ComparingTextColor: string = null;

    public MinPrice1ComparingPrice: number;
    public MinPrice1ComparingTextColor: string = null;
    public MinPrice2ComparingPrice: number;
    public MinPrice2ComparingTextColor: string = null;
    public MinPrice3ComparingPrice: number;
    public MinPrice3ComparingTextColor: string = null;
    public MinPrice4ComparingPrice: number;
    public MinPrice4ComparingTextColor: string = null;
    public MinPrice5ComparingPrice: number;
    public MinPrice5ComparingTextColor: string = null;
    public MinPrice6ComparingPrice: number;
    public MinPrice6ComparingTextColor: string = null;
    public MinPrice7ComparingPrice: number;
    public MinPrice7ComparingTextColor: string = null;
    public MinPrice8ComparingPrice: number;
    public MinPrice8ComparingTextColor: string = null;
    public MinPrice9ComparingPrice: number;
    public MinPrice9ComparingTextColor: string = null;
    public MinPrice10ComparingPrice: number;
    public MinPrice10ComparingTextColor: string = null;
    private DefaultColor = "blue";

    SetCellsComparingText() {
        this.CompareSurcharge1Price();
        this.CompareSurcharge2Price();
        this.CompareSurcharge3Price();
        this.CompareSurcharge4Price();
        this.CompareSurcharge5Price();
        this.CompareSurcharge6Price();
        this.CompareSurcharge7Price();
        this.CompareSurcharge8Price();
        this.CompareSurcharge9Price();
        this.CompareSurcharge10Price();

        this.CompareSurchargeMinPrices();
    }
    private CompareSurchargeMinPrices() {
        this.CompareMainPrice(1);
        this.CompareMainPrice(2);
        this.CompareMainPrice(3);
        this.CompareMainPrice(4);
        this.CompareMainPrice(5);
        this.CompareMainPrice(6);
        this.CompareMainPrice(7);
        this.CompareMainPrice(8);
        this.CompareMainPrice(9);
        this.CompareMainPrice(10);
    }

    private CompareMainPrice(index: number) {
        if (this.ComparedEntity != null) {
            this['MinPrice' + index + 'ComparingPrice'] = null;
            this['MinPrice' + index + 'ComparingTextColor'] = this.DefaultColor;

            if (this.ComparedEntity['Surcharge' + index + 'MinPrice'] != null) {
                var priceValue = this['Surcharge' + index + 'MinPrice'] - this.ComparedEntity['Surcharge' + index + 'MinPrice'];
                if (!AppTool.IsNullOrZero(priceValue) && !AppTool.IsNullOrZero(this.ComparedEntity['Surcharge' + index + 'MinPrice'])) {
                    this['MinPrice' + index + 'ComparingPrice'] = (priceValue / this.ComparedEntity['Surcharge' + index + 'MinPrice']) * 100;
                    this['MinPrice' + index + 'ComparingTextColor'] = this.ComputeWarningPercentageColor(this['MinPrice' + index + 'ComparingPrice']);
                }
            }
        }
    }

    private CompareSurcharge1Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge1ComparingPrice = null;
            this.Surcharge1ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge1Price != null) {
                var surcharge1ComparingValue = this.Surcharge1Price - this.ComparedEntity.Surcharge1Price;
                if (!AppTool.IsNullOrZero(surcharge1ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge1Price)) {
                    this.Surcharge1ComparingPrice = (surcharge1ComparingValue / this.ComparedEntity.Surcharge1Price) * 100;
                    this.Surcharge1ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge1ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge2Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge2ComparingPrice = null;
            this.Surcharge2ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge2Price != null) {
                var surcharge2ComparingValue = this.Surcharge2Price - this.ComparedEntity.Surcharge2Price;
                if (!AppTool.IsNullOrZero(surcharge2ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge2Price)) {
                    this.Surcharge2ComparingPrice = (surcharge2ComparingValue / this.ComparedEntity.Surcharge2Price) * 100;
                    this.Surcharge2ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge2ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge3Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge3ComparingPrice = null;
            this.Surcharge3ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge3Price != null) {
                var surcharge3ComparingValue = this.Surcharge3Price - this.ComparedEntity.Surcharge3Price;
                if (!AppTool.IsNullOrZero(surcharge3ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge3Price)) {
                    this.Surcharge3ComparingPrice = (surcharge3ComparingValue / this.ComparedEntity.Surcharge3Price) * 100;
                    this.Surcharge3ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge3ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge4Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge4ComparingPrice = null;
            this.Surcharge4ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge4Price != null) {
                var surcharge4ComparingValue = this.Surcharge4Price - this.ComparedEntity.Surcharge4Price;
                if (!AppTool.IsNullOrZero(surcharge4ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge4Price)) {
                    this.Surcharge4ComparingPrice = (surcharge4ComparingValue / this.ComparedEntity.Surcharge4Price) * 100;
                    this.Surcharge4ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge4ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge5Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge5ComparingPrice = null;
            this.Surcharge5ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge5Price != null) {
                var surcharge5ComparingValue = this.Surcharge5Price - this.ComparedEntity.Surcharge5Price;
                if (!AppTool.IsNullOrZero(surcharge5ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge5Price)) {
                    this.Surcharge5ComparingPrice = (surcharge5ComparingValue / this.ComparedEntity.Surcharge5Price) * 100;
                    this.Surcharge5ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge5ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge6Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge6ComparingPrice = null;
            this.Surcharge6ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge6Price != null) {
                var surcharge6ComparingValue = this.Surcharge6Price - this.ComparedEntity.Surcharge6Price;
                if (!AppTool.IsNullOrZero(surcharge6ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge6Price)) {
                    this.Surcharge6ComparingPrice = (surcharge6ComparingValue / this.ComparedEntity.Surcharge6Price) * 100;
                    this.Surcharge6ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge6ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge7Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge7ComparingPrice = null;
            this.Surcharge7ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge7Price != null) {
                var surcharge7ComparingValue = this.Surcharge7Price - this.ComparedEntity.Surcharge7Price;
                if (!AppTool.IsNullOrZero(surcharge7ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge7Price)) {
                    this.Surcharge7ComparingPrice = (surcharge7ComparingValue / this.ComparedEntity.Surcharge7Price) * 100;
                    this.Surcharge7ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge7ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge8Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge8ComparingPrice = null;
            this.Surcharge8ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge8Price != null) {
                var surcharge8ComparingValue = this.Surcharge8Price - this.ComparedEntity.Surcharge8Price;
                if (!AppTool.IsNullOrZero(surcharge8ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge8Price)) {
                    this.Surcharge8ComparingPrice = (surcharge8ComparingValue / this.ComparedEntity.Surcharge8Price) * 100;
                    this.Surcharge8ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge8ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge9Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge9ComparingPrice = null;
            this.Surcharge9ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge9Price != null) {
                var surcharge9ComparingValue = this.Surcharge9Price - this.ComparedEntity.Surcharge9Price;
                if (!AppTool.IsNullOrZero(surcharge9ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge9Price)) {
                    this.Surcharge9ComparingPrice = (surcharge9ComparingValue / this.ComparedEntity.Surcharge9Price) * 100;
                    this.Surcharge9ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge9ComparingPrice);
                }
            }
        }
    }
    private CompareSurcharge10Price() {
        if (this.ComparedEntity != null) {
            this.Surcharge10ComparingPrice = null;
            this.Surcharge10ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge10Price != null) {
                var surcharge10ComparingValue = this.Surcharge10Price - this.ComparedEntity.Surcharge10Price;
                if (!AppTool.IsNullOrZero(surcharge10ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge10Price)) {
                    this.Surcharge10ComparingPrice = (surcharge10ComparingValue / this.ComparedEntity.Surcharge10Price) * 100;
                    this.Surcharge10ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge10ComparingPrice);
                }
            }
        }
    }
    ComputeWarningPercentageColor(price: number) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {

            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    }

    get FromCountryId() {
        return this.EntityPM.FromCountryId;
    }
    set FromCountryId(value: string) {
        if (this.EntityPM.FromCountryId != value) {
            this.EntityPM.FromCountryId = value;
            this.EntityPM.LineEdited = true;

            if (this.IsFromAllOtherCountries) {
                this.EntityPM.IsFromAllOtherCountries = false;
            }
            this.SetUIProperties_From();
        }
    }

    get FromCountryCode() {
        return this.EntityPM.FromCountryCode;
    }
    set FromCountryCode(value: string) {
        if (this.EntityPM.FromCountryCode != value) {
            this.EntityPM.FromCountryCode = value;
        }
    }

    private fromCountry: CountryList;
    get FromCountry() { return this.fromCountry; }
    set FromCountry(value: CountryList) {
        if (this.fromCountry != value) {
            this.fromCountry = value;
        }

        if (!AppTool.IsNullOrEmpty(value)) {
            this.FromCountryCode = value.Code;
        }
        else {
            this.FromCountryCode = null;
        }
    }

    get ToCountryId() {
        return this.EntityPM.ToCountryId;
    }
    set ToCountryId(value: string) {
        if (this.EntityPM.ToCountryId != value) {
            this.EntityPM.ToCountryId = value;
            this.EntityPM.LineEdited = true;

            if (this.IsToAllOtherCountries) {
                this.EntityPM.IsToAllOtherCountries = false;
            }
            this.SetUIProperties_To();
        }
    }

    get ToCountryCode() {
        return this.EntityPM.ToCountryCode;
    }
    set ToCountryCode(value: string) {
        if (this.EntityPM.ToCountryCode != value) {
            this.EntityPM.ToCountryCode = value;
        }
    }

    private toCountry: CountryList;
    get ToCountry() { return this.toCountry; }
    set ToCountry(value: CountryList) {
        if (this.toCountry != value) {
            this.toCountry = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.ToCountryCode = value.Code;
        }
        else {
            this.ToCountryCode = null;
        }
    }

    private currency: CurrencyList;
    get Currency() { return this.currency; }
    set Currency(value: CurrencyList) {
        if (this.currency != value) {
            this.currency = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CurrencyCode = value.Code;
        } else {
            this.CurrencyCode = null;
        }
    }

    get CurrencyCode() {
        return this.EntityPM.CurrencyCode;
    }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
        }
    }

    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.lineCurrencyId = this.EntityPM.CurrencyId;

            this.SetUIProperties_Currency();
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
            this.EntityPM.LineEdited = true;
        }
    }

    get IsDifferentCurrenciesPerCharge() {
        return this.EntityPM.IsDifferentCurrenciesPerCharge;
    }
    set IsDifferentCurrenciesPerCharge(value: boolean) {
        if (this.EntityPM.IsDifferentCurrenciesPerCharge != value) {
            this.EntityPM.IsDifferentCurrenciesPerCharge = value;

            this.SetUIProperties_Currency();
            this.SurchargesCurrencies(this.CurrencyId);

            if (value) {
                this.EntityPM.CurrencyId = null;
            }
            else {
                this.CurrencyId = this.lineCurrencyId;
                for (var i = 1; i <= 10; i++) {
                    this["Surcharge" + i + "CurrencyId"] = null;
                }
            }
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge1CurrencyId() {
        return this.EntityPM.Surcharge1CurrencyId;
    }
    set Surcharge1CurrencyId(value: string) {
        if (this.EntityPM.Surcharge1CurrencyId != value) {
            this.EntityPM.Surcharge1CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge2CurrencyId() {
        return this.EntityPM.Surcharge2CurrencyId;
    }
    set Surcharge2CurrencyId(value: string) {
        if (this.EntityPM.Surcharge2CurrencyId != value) {
            this.EntityPM.Surcharge2CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge3CurrencyId() {
        return this.EntityPM.Surcharge3CurrencyId;
    }
    set Surcharge3CurrencyId(value: string) {
        if (this.EntityPM.Surcharge3CurrencyId != value) {
            this.EntityPM.Surcharge3CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge4CurrencyId() {
        return this.EntityPM.Surcharge4CurrencyId;
    }
    set Surcharge4CurrencyId(value: string) {
        if (this.EntityPM.Surcharge4CurrencyId != value) {
            this.EntityPM.Surcharge4CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge5CurrencyId() {
        return this.EntityPM.Surcharge5CurrencyId;
    }
    set Surcharge5CurrencyId(value: string) {
        if (this.EntityPM.Surcharge5CurrencyId != value) {
            this.EntityPM.Surcharge5CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge6CurrencyId() {
        return this.EntityPM.Surcharge6CurrencyId;
    }
    set Surcharge6CurrencyId(value: string) {
        if (this.EntityPM.Surcharge6CurrencyId != value) {
            this.EntityPM.Surcharge6CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge7CurrencyId() {
        return this.EntityPM.Surcharge7CurrencyId;
    }
    set Surcharge7CurrencyId(value: string) {
        if (this.EntityPM.Surcharge7CurrencyId != value) {
            this.EntityPM.Surcharge7CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge8CurrencyId() {
        return this.EntityPM.Surcharge8CurrencyId;
    }
    set Surcharge8CurrencyId(value: string) {
        if (this.EntityPM.Surcharge8CurrencyId != value) {
            this.EntityPM.Surcharge8CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge9CurrencyId() {
        return this.EntityPM.Surcharge9CurrencyId;
    }
    set Surcharge9CurrencyId(value: string) {
        if (this.EntityPM.Surcharge9CurrencyId != value) {
            this.EntityPM.Surcharge9CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    get Surcharge10CurrencyId() {
        return this.EntityPM.Surcharge10CurrencyId;
    }
    set Surcharge10CurrencyId(value: string) {
        if (this.EntityPM.Surcharge10CurrencyId != value) {
            this.EntityPM.Surcharge10CurrencyId = value;
            this.EntityPM.LineEdited = true;
            this.FillAllSurchargesCurrencyMeasurementLabel();
        }
    }

    // Surcharge 1
    get Surcharge1Price() {
        return this.EntityPM.Surcharge1Price;
    }
    set Surcharge1Price(value: number) {
        if (this.EntityPM.Surcharge1Price != value) {
            this.EntityPM.Surcharge1Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge1Price();
        }
    }

    get Surcharge1MinPrice() {
        return this.EntityPM.Surcharge1MinPrice;
    }
    set Surcharge1MinPrice(value: number) {
        if (this.EntityPM.Surcharge1MinPrice != value) {
            this.EntityPM.Surcharge1MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(1);
        }
    }

    get Surcharge1PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge1Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge1PriceText;
        }
    }

    get Surcharge1PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge1MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge1MinPrice, "N3");
    }

    // Surcharge 2
    get Surcharge2Price() {
        return this.EntityPM.Surcharge2Price;
    }
    set Surcharge2Price(value: number) {
        if (this.EntityPM.Surcharge2Price != value) {
            this.EntityPM.Surcharge2Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge2Price();
        }
    }

    get Surcharge2MinPrice() {
        return this.EntityPM.Surcharge2MinPrice;
    }
    set Surcharge2MinPrice(value: number) {
        if (this.EntityPM.Surcharge2MinPrice != value) {
            this.EntityPM.Surcharge2MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(2);
        }
    }

    get Surcharge2PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge2Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge2PriceText;
        }
    }

    get Surcharge2PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge2MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge2MinPrice, "N3");
    }

    // Surcharge 3
    get Surcharge3Price() {
        return this.EntityPM.Surcharge3Price;
    }
    set Surcharge3Price(value: number) {
        if (this.EntityPM.Surcharge3Price != value) {
            this.EntityPM.Surcharge3Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge3Price();
        }
    }

    get Surcharge3MinPrice() {
        return this.EntityPM.Surcharge3MinPrice;
    }
    set Surcharge3MinPrice(value: number) {
        if (this.EntityPM.Surcharge3MinPrice != value) {
            this.EntityPM.Surcharge3MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(3);
        }
    }

    get Surcharge3PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge3Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge3PriceText;
        }
    }

    get Surcharge3PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge3MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge3MinPrice, "N3");
    }

    // Surcharge 4
    get Surcharge4Price() {
        return this.EntityPM.Surcharge4Price;
    }
    set Surcharge4Price(value: number) {
        if (this.EntityPM.Surcharge4Price != value) {
            this.EntityPM.Surcharge4Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge4Price();
        }
    }

    get Surcharge4MinPrice() {
        return this.EntityPM.Surcharge4MinPrice;
    }
    set Surcharge4MinPrice(value: number) {
        if (this.EntityPM.Surcharge4MinPrice != value) {
            this.EntityPM.Surcharge4MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(4);
        }
    }

    get Surcharge4PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge4Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge4PriceText;
        }
    }

    get Surcharge4PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge4MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge4MinPrice, "N3");
    }

    // Surcharge 5
    get Surcharge5Price() {
        return this.EntityPM.Surcharge5Price;
    }
    set Surcharge5Price(value: number) {
        if (this.EntityPM.Surcharge5Price != value) {
            this.EntityPM.Surcharge5Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge5Price();
        }
    }

    get Surcharge5MinPrice() {
        return this.EntityPM.Surcharge5MinPrice;
    }
    set Surcharge5MinPrice(value: number) {
        if (this.EntityPM.Surcharge5MinPrice != value) {
            this.EntityPM.Surcharge5MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(5);
        }
    }

    get Surcharge5PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge5Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge5PriceText;
        }
    }

    get Surcharge5PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge5MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge5MinPrice, "N3");
    }

    // Surcharge 6
    get Surcharge6Price() {
        return this.EntityPM.Surcharge6Price;
    }
    set Surcharge6Price(value: number) {
        if (this.EntityPM.Surcharge6Price != value) {
            this.EntityPM.Surcharge6Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge6Price();
        }
    }

    get Surcharge6MinPrice() {
        return this.EntityPM.Surcharge6MinPrice;
    }
    set Surcharge6MinPrice(value: number) {
        if (this.EntityPM.Surcharge6MinPrice != value) {
            this.EntityPM.Surcharge6MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(6);
        }
    }

    get Surcharge6PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge6Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge6PriceText;
        }
    }

    get Surcharge6PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge6MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge6MinPrice, "N3");
    }

    // Surcharge 7
    get Surcharge7Price() {
        return this.EntityPM.Surcharge7Price;
    }
    set Surcharge7Price(value: number) {
        if (this.EntityPM.Surcharge7Price != value) {
            this.EntityPM.Surcharge7Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge7Price();
        }
    }

    get Surcharge7MinPrice() {
        return this.EntityPM.Surcharge7MinPrice;
    }
    set Surcharge7MinPrice(value: number) {
        if (this.EntityPM.Surcharge7MinPrice != value) {
            this.EntityPM.Surcharge7MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(7);
        }
    }

    get Surcharge7PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge7Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge7PriceText;
        }
    }

    get Surcharge7PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge7MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge7MinPrice, "N3");
    }

    // Surcharge 8
    get Surcharge8Price() {
        return this.EntityPM.Surcharge8Price;
    }
    set Surcharge8Price(value: number) {
        if (this.EntityPM.Surcharge8Price != value) {
            this.EntityPM.Surcharge8Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge8Price();
        }
    }

    get Surcharge8MinPrice() {
        return this.EntityPM.Surcharge8MinPrice;
    }
    set Surcharge8MinPrice(value: number) {
        if (this.EntityPM.Surcharge8MinPrice != value) {
            this.EntityPM.Surcharge8MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(8);
        }
    }

    get Surcharge8PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge8Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge8PriceText;
        }
    }

    get Surcharge8PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge8MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge8MinPrice, "N3");
    }

    // Surcharge 9
    get Surcharge9Price() {
        return this.EntityPM.Surcharge9Price;
    }
    set Surcharge9Price(value: number) {
        if (this.EntityPM.Surcharge9Price != value) {
            this.EntityPM.Surcharge9Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge9Price();
        }
    }

    get Surcharge9MinPrice() {
        return this.EntityPM.Surcharge9MinPrice;
    }
    set Surcharge9MinPrice(value: number) {
        if (this.EntityPM.Surcharge9MinPrice != value) {
            this.EntityPM.Surcharge9MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(9);
        }
    }

    get Surcharge9PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge9Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge9PriceText;
        }
    }

    get Surcharge9PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge9MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge9MinPrice, "N3");
    }

    // Surcharge 10
    get Surcharge10Price() {
        return this.EntityPM.Surcharge10Price;
    }
    set Surcharge10Price(value: number) {
        if (this.EntityPM.Surcharge10Price != value) {
            this.EntityPM.Surcharge10Price = value;
            this.EntityPM.LineEdited = true;
            this.CompareSurcharge10Price();
        }
    }

    get Surcharge10MinPrice() {
        return this.EntityPM.Surcharge10MinPrice;
    }
    set Surcharge10MinPrice(value: number) {
        if (this.EntityPM.Surcharge10MinPrice != value) {
            this.EntityPM.Surcharge10MinPrice = value;
            this.EntityPM.LineEdited = true;
            this.CompareMainPrice(10);
        }
    }

    get Surcharge10PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Surcharge10Price, "N3");
        }

        else {
            return this.EntityPM.Surcharge10PriceText;
        }
    }

    get Surcharge10PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    get Surcharge10MinPriceValue() {
        return FormatTool.FormatNumber(this.EntityPM.Surcharge10MinPrice, "N3");
    }

    get IsFromAllOtherCountries() { return this.EntityPM.IsFromAllOtherCountries; }
    set IsFromAllOtherCountries(value: boolean) {
        if (this.EntityPM.IsFromAllOtherCountries != value) {
            this.EntityPM.IsFromAllOtherCountries = value;

            if (value) {
                this.FromCountryId = null;
            }

            this.SetUIProperties_From();
            this.ComputeIndex();
        }
    }

    get IsToAllOtherCountries() { return this.EntityPM.IsToAllOtherCountries; }
    set IsToAllOtherCountries(value: boolean) {
        if (this.EntityPM.IsToAllOtherCountries != value) {
            this.EntityPM.IsToAllOtherCountries = value;

            if (value) {
                this.ToCountryId = null;
            }

            this.SetUIProperties_To();
            this.ComputeIndex();
        }
    }

    private ComputeIndex() {
        if (this.IsFromAllOtherCountries || this.IsToAllOtherCountries) {
            this.EntityPM.Index = -1;
        }

        else {
            if (this.IsNewEntity) {
                this.EntityPM.Index = this.initialIndex;
            }

            else {
                if (this.initialIndex == -1) {
                    this.EntityPM.Index = 0;

                }

                else {
                    this.EntityPM.Index = this.initialIndex;
                }
            }
        }
    }

    private isLineSelected: boolean = false;
    get IsLineSelected() { return this.isLineSelected; }
    set IsLineSelected(value: boolean) {
        if (this.isLineSelected != value) {
            this.isLineSelected = value;
        }
    }

    private SurchargesCurrencies(defaultCurrencyId: string) {
        for (var i = 1; i <= 10; i++) {
            if (this.FatherComponent["Surcharge" + i + "PriceVisibility"]) {
                this["Surcharge" + i + "CurrencyId"] = defaultCurrencyId;
            }
        }
    }

    get ErrorText() {
        return this.EntityPM.ErrorText;
    }
    set ErrorText(value: string) {
        if (this.EntityPM.ErrorText != value) {
            this.EntityPM.ErrorText = value;
        }
    }

    get HasErrors() {
        return this.EntityPM.HasErrors;
    }
    set HasErrors(value: boolean) {
        if (this.EntityPM.HasErrors != value) {
            this.EntityPM.HasErrors = value;
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
