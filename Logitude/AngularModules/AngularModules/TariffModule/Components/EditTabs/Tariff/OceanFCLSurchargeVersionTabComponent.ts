import { Component, OnDestroy, EventEmitter, Output } from '@angular/core';
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
import { UpdateTariffArgs } from '../../../Args';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { TariffLinesContainersPricePM } from '../../../EntityPMs/TariffLinesContainersPricePM';
import { PackageTypeList } from '../../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../../Common/Services/StandardLists/PackageTypeListService';

@Component({
    moduleId: module.id,
    templateUrl: './OceanFCLSurchargeVersionTabComponent.html',
})

export class OceanFCLSurchargeVersionTabComponent extends BaseComponent implements OnDestroy {
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
    private deletedLinesExpirationDates: TariffLineExpirationDatePM[];
    @Output() ReloadDetails = new EventEmitter();
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    
    public AllChargesTypes: ChargesTypeList[];
    public AllMeasurements: MeasurementList[];
    public AllPackageTypes: PackageTypeList[];
    Intialize(args: any) {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.TariffDomainService = new TariffDomainService();
        this.deletedLinesExpirationDates = [];

        this.CurrentVersion = args['CurrentVersion'];
        this.SelectedVersionNumber = args['SelectedVersionNumber'];

        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
        }

        if (this.IsDraftVersion) {
            this.IsComparToChecked = true;
        }

        this.GetTariffSettings();

        var iChargesTypeListService = new ChargesTypeListService();
        var iMeasurementListService = new MeasurementListService();
        var iPackageTypeListService = new PackageTypeListService();

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

        iPackageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPackageTypes = myResponse.Result;
                this.SetContainersLabelsAndVisibility();
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
                    this.SetContainersLabelsAndVisibility();
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

                    //this.DoCompare();
                    this.BuildDeletedLines();
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

    public Container1Label: string;
    public Container2Label: string;
    public Container3Label: string;
    public Container4Label: string;
    public Container5Label: string;
    public Container1Visibility: boolean;
    public Container2Visibility: boolean;
    public Container3Visibility: boolean;
    public Container4Visibility: boolean;
    public Container5Visibility: boolean;

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
    
    public tariffCharges: CodeNameClass[] = [];
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
                item.Name = iChargeType.Code;
                item.DisplyText = iChargeType.Code;
                item.Code_Int = index;
                
                var iMeasurement: MeasurementList = this.AllMeasurements.filter(f => f.Id == iMeasurementId)[0];
                if (iMeasurement) {
                    item.DisplyText = iChargeType.Code + " (" + iMeasurement.Code + ")";
                    item.AdditionalField = iMeasurement.Code;                    
                }

                this.tariffCharges.push(item);

                this['Surcharge' + index + 'PriceLabel'] = item.DisplyText;
                this['Surcharge' + index + 'PriceVisibility'] = true;
            }
        }
    }

    private SetContainersLabelsAndVisibility() {
        this.AddContainerColumn(this.EntityPM.ContainerType1Id, 1);
        this.AddContainerColumn(this.EntityPM.ContainerType2Id, 2);
        this.AddContainerColumn(this.EntityPM.ContainerType3Id, 3);
        this.AddContainerColumn(this.EntityPM.ContainerType4Id, 4);
        this.AddContainerColumn(this.EntityPM.ContainerType5Id, 5);
    }
    AddContainerColumn(iContainerTypeId: string, index: number) {
        if (!AppTool.IsNullOrEmpty(iContainerTypeId)) {
            var iContainerType: PackageTypeList = this.AllPackageTypes.filter(a => a.Id == iContainerTypeId)[0];
            if (iContainerType) {

                this['Container' + index + 'Label'] = iContainerType.Code;
                this['Container' + index + 'Visibility'] = true;
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

    private ItemsCollection: OceanFCLSurchargeTariffLineData[] = [];
    public DeletedTariffsLines: OceanFCLSurchargeTariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }

        this.ItemsCollection = [];

        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            this.ItemsCollection.push(new OceanFCLSurchargeTariffLineData(item, this));
        });

        this.TariffsLinesSource.InsertCollection(this.ItemsCollection);
        this.DoCompare();

    }

    private DoCompare() {
        this.DeletedTariffsLines = [];
        if (this.IsComparToChecked && this.ComparedToVersionPM != null) {
            this.ComaredLines();
            this.BuildDeletedLines();
        }
    }

    ComaredLines() {
        this.ItemsCollection.forEach((item: OceanFCLSurchargeTariffLineData) => {
            item.IsNewEntity = false;
            var line = this.compareTariffLines.sort((a, b) => a.Index - b.Index).filter(a => a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId)[0];
            if (line) {
                item.ComparedEntity = line;
                //item.SetCellsComparingText();
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
                this.DeletedTariffsLines.push(new OceanFCLSurchargeTariffLineData(item, this));// Deleted 
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

        var itemComponent = new OceanFCLSurchargeTariffLineData(itemPM, this, true);
        logWindow.WindowArgs = { DataContext: itemComponent, EntityPM: itemPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: OceanFCLSurchargeTariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: item, EntityPM: item.EntityPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    private isTariffLinesDeleted: boolean = false;
    DeleteTariffButtonClicked(item: OceanFCLSurchargeTariffLineData) {
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

    UpdateSurchargesClicked() {
        var args: UpdateTariffArgs = new UpdateTariffArgs();
        args.Version = this.CurrentVersion;
        args.TariffCharges = this.tariffCharges;
        args.CarrierId = this.EntityPM.SellerId;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1100;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Title = "Tariff Surcharge Update";

        logWindow.WindowClosed.subscribe((s: any) => {
            if (s == "ok") {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });

        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UpdateSurchargesComponent');
    }




    public SelectedRow: OceanFCLSurchargeTariffLineData = null;
    OnRowSelected(itemComponent: OceanFCLSurchargeTariffLineData) {
        this.SelectedRow = itemComponent;
        //this.SetUIProperties_InsideButton();
    }
    OnRowLoaded(myRow: any) {
        if (myRow) {
            var isExpandaple = false;

            var item: OceanFCLSurchargeTariffLineData = myRow.rowData;
            if (item) {
                item.Row = myRow;

                //if (item.InsideItemsSource.length > 0) {
                //    isExpandaple = true;
                //}
            }

            myRow.SetExpandaple(isExpandaple);
        }
    }

    SetMouseHoverRow(item: OceanFCLSurchargeTariffLineData, isRowHover: boolean) {
        if (item) {
            item.IsRowHover = isRowHover;
        }
    }
}

export class OceanFCLSurchargeTariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public TariffPM: TariffPM;
    public DataContext: OceanFCLSurchargeTariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;
    private initialIndex: number;
    public IsRowHover: boolean = false;
    public Row: any;
    public ContainerPricesItemsSource: ContainerPricesItem[] = [];
    constructor(entity: TariffLinePM, public FatherComponent: OceanFCLSurchargeVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.TariffPM = FatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.initialIndex = entity.Index;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.SetUIProperties();
        this.BuildContainerPricesItemsSource();
    }

    private CheckIfLineHasError() {
        if (this.ErrorText != 'Line is a duplicate') {
            var error: boolean = false;
            var errorText: string;

            if (!this.IsFromAllOtherPorts) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                    }

                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found"
                    }
                }
                else if (AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Origin Port";
                    }

                    else {
                        errorText = errorText + ", Missing Origin Port"
                    }
                }
            }

            if (!this.IsToAllOtherPorts) {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                    }

                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found"
                    }
                }
                else if (AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;

                    if (AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Destination Port";
                    }

                    else {
                        errorText = errorText + ", Missing Destination Port"
                    }
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 1 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 1 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 2 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 2 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 3 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 3 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 4 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 4 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 5 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 5 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 6 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 6 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 7 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 7 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 8 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 8 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 9 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 9 Price format is invalid"
                }
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                error = true;

                if (AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 10 Price format is invalid";
                }

                else {
                    errorText = errorText + ", Surcharge 10 Price format is invalid"
                }
            }

            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    }

    private SetUIProperties() {
        this.SetUIProperties_From();
        this.SetUIProperties_To();
        this.SetUIProperties_Currency();
    }
    private SetUIProperties_From() {
        if (this.IsFromAllOtherPorts) {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsFromAllOtherPorts", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        }
    }
    private SetUIProperties_To() {
        if (this.IsToAllOtherPorts) {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsToAllOtherPorts", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
        }
    }
    private SetUIProperties_Currency() {
        var isCurrencyRequired: boolean = false;

        if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
            isCurrencyRequired = true;
        }

        this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, isCurrencyRequired);
    }

    get HasErrors() {
        return this.EntityPM.HasErrors;
    }
    set HasErrors(value: boolean) {
        if (this.EntityPM.HasErrors != value) {
            this.EntityPM.HasErrors = value;
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

    // Origin Port
    get OriginPortId() {
        return this.EntityPM.OriginPortId;
    }
    set OriginPortId(value: string) {
        if (this.EntityPM.OriginPortId != value) {
            this.EntityPM.OriginPortId = value;
            if (this.IsFromAllOtherPorts) {
                this.EntityPM.IsFromAllOtherPorts = false;
            }
            this.SetUIProperties_From();
            this.CheckIfLineHasError();
        }
    }

    get OriginPortCode() {
        return this.EntityPM.OriginPortCode;
    }
    set OriginPortCode(value: string) {
        if (this.EntityPM.OriginPortCode != value) {
            this.EntityPM.OriginPortCode = value;
        }
    }

    originPort: PortList;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortList) {
        if (this.originPort != value) {
            this.originPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.OriginPortCode = value.Code;
        } else {
            this.OriginPortCode = null;
        }
    }

    get OriginPortValue() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
            return this.EntityPM.OriginPortCode;
        }

        else {
            return this.EntityPM.OriginPortText;
        }
    }

    get OriginPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Destination Port
    get DestinationPortId() {
        return this.EntityPM.DestinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.EntityPM.DestinationPortId != value) {
            this.EntityPM.DestinationPortId = value;
            if (this.IsToAllOtherPorts) {
                this.EntityPM.IsToAllOtherPorts = false;
            }
            this.SetUIProperties_To();
            this.CheckIfLineHasError();
        }
    }

    get DestinationPortCode() {
        return this.EntityPM.DestinationPortCode;
    }
    set DestinationPortCode(value: string) {
        if (this.EntityPM.DestinationPortCode != value) {
            this.EntityPM.DestinationPortCode = value;
        }
    }

    destinationPort: PortList;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortList) {
        if (this.destinationPort != value) {
            this.destinationPort = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DestinationPortCode = value.Code;
        } else {
            this.DestinationPortCode = null;
        }
    }

    get DestinationPortValue() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
            return this.EntityPM.DestinationPortCode;
        }

        else {
            return this.EntityPM.DestinationPortText;
        }
    }

    get DestinationPortColor() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    currency: CurrencyList;
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

            this.SetUIProperties_Currency();
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
        }
    }

    get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }

    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    // Surcharge 1
    get Surcharge1Price() {
        return this.EntityPM.Surcharge1Price;
    }
    set Surcharge1Price(value: number) {
        if (this.EntityPM.Surcharge1Price != value) {
            this.EntityPM.Surcharge1Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 2
    get Surcharge2Price() {
        return this.EntityPM.Surcharge2Price;
    }
    set Surcharge2Price(value: number) {
        if (this.EntityPM.Surcharge2Price != value) {
            this.EntityPM.Surcharge2Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 3
    get Surcharge3Price() {
        return this.EntityPM.Surcharge3Price;
    }
    set Surcharge3Price(value: number) {
        if (this.EntityPM.Surcharge3Price != value) {
            this.EntityPM.Surcharge3Price = value;
            this.CheckIfLineHasError();

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

    // Surcharge 4
    get Surcharge4Price() {
        return this.EntityPM.Surcharge4Price;
    }
    set Surcharge4Price(value: number) {
        if (this.EntityPM.Surcharge4Price != value) {
            this.EntityPM.Surcharge4Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 5
    get Surcharge5Price() {
        return this.EntityPM.Surcharge5Price;
    }
    set Surcharge5Price(value: number) {
        if (this.EntityPM.Surcharge5Price != value) {
            this.EntityPM.Surcharge5Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 6
    get Surcharge6Price() {
        return this.EntityPM.Surcharge6Price;
    }
    set Surcharge6Price(value: number) {
        if (this.EntityPM.Surcharge6Price != value) {
            this.EntityPM.Surcharge6Price = value;
            this.CheckIfLineHasError();

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

    // Surcharge 7
    get Surcharge7Price() {
        return this.EntityPM.Surcharge7Price;
    }
    set Surcharge7Price(value: number) {
        if (this.EntityPM.Surcharge7Price != value) {
            this.EntityPM.Surcharge7Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 8
    get Surcharge8Price() {
        return this.EntityPM.Surcharge8Price;
    }
    set Surcharge8Price(value: number) {
        if (this.EntityPM.Surcharge8Price != value) {
            this.EntityPM.Surcharge8Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 9
    get Surcharge9Price() {
        return this.EntityPM.Surcharge9Price;
    }
    set Surcharge9Price(value: number) {
        if (this.EntityPM.Surcharge9Price != value) {
            this.EntityPM.Surcharge9Price = value;
            this.CheckIfLineHasError();
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

    // Surcharge 10
    get Surcharge10Price() {
        return this.EntityPM.Surcharge10Price;
    }
    set Surcharge10Price(value: number) {
        if (this.EntityPM.Surcharge10Price != value) {
            this.EntityPM.Surcharge10Price = value;
            this.CheckIfLineHasError();
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

    ////////////////////////////
    get IsFromAllOtherPorts() { return this.EntityPM.IsFromAllOtherPorts; }
    set IsFromAllOtherPorts(value: boolean) {
        if (this.EntityPM.IsFromAllOtherPorts != value) {
            this.EntityPM.IsFromAllOtherPorts = value;

            if (value) {
                this.OriginPortId = null;
                this.EntityPM.OriginPortText = null;
            }

            this.SetUIProperties_From();
            this.CheckIfLineHasError();
            this.ComputeIndex();
        }
    }

    get IsToAllOtherPorts() { return this.EntityPM.IsToAllOtherPorts; }
    set IsToAllOtherPorts(value: boolean) {
        if (this.EntityPM.IsToAllOtherPorts != value) {
            this.EntityPM.IsToAllOtherPorts = value;

            if (value) {
                this.DestinationPortId = null;
                this.EntityPM.DestinationPortText = null;
            }

            this.SetUIProperties_To();
            this.CheckIfLineHasError();
            this.ComputeIndex();
        }
    }

    private ComputeIndex() {
        if (this.IsFromAllOtherPorts || this.IsToAllOtherPorts) {
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




    public RowDetailsHeights: number = 0;
    BuildContainerPricesItemsSource() {
        this.ContainerPricesItemsSource = [];

        var list: TariffLinesContainersPricePM[] = [];
        this.EntityPM.ContainersPrices.forEach((item) => {
            list.push(item);
        });

        if (list.length < 10) {
            for (var i = list.length; i < 10; i++) {
                var chargeId: string = this.TariffPM['Surcharge' + (i + 1) + 'Id'];

                if (!AppTool.IsNullOrEmpty(chargeId)) {
                    var item: TariffLinesContainersPricePM = new TariffLinesContainersPricePM(null);
                    item.Tenant = this.EntityPM.Tenant;
                    item.TariffId = this.EntityPM.TariffId;
                    item.TariffLineId = this.EntityPM.Id;
                    item.SurchargeId = chargeId;
                    list.push(item);
                }
            }
        }

        list.forEach(item => {
            this.ContainerPricesItemsSource.push(new ContainerPricesItem(item, this));
        });


        //this.ContainerPricesItemsSource = [];

        //this.EntityPM.conta.forEach(item => {
        //    this.ContainerPricesItemsSource.push(new ContainerPricesItem(item, this));
        //});

        //this.RowDetailsHeights = (this.ContainerPricesItemsSource.length * 26) + 20 + 28;
        //this.FatherComponent.ReloadDetails.emit("");
    }
}

export class ContainerPricesItem extends BaseComponent {
    public EntityPM: TariffLinesContainersPricePM;
    public TariffPM: TariffPM;
    public TariffLinePM: TariffLinePM;
    public ObjectTableName: string = "TariffLinesContainersPrice";
    public IsNewEntity: boolean = false;

    constructor(entity: TariffLinesContainersPricePM, public FatherComponent: OceanFCLSurchargeTariffLineData, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.TariffPM = FatherComponent.TariffPM;
        this.TariffLinePM = FatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        
        this.SetUIProperties();
        if (this.IsNewEntity) {
            
        }

        this.FillChargeLabels();
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {        
        this.IsEditingEnabled = this.FatherComponent.IsEditEnabled;
        
        if (this.IsEditingEnabled) {
            
        }        
    }

    public ChargeLabel: string;
    private FillChargeLabels() {

    }

    get SurchargeId() {
        return this.EntityPM.SurchargeId;
    }
    set SurchargeId(value: string) {
        if (this.EntityPM.SurchargeId != value) {
            this.EntityPM.SurchargeId = value;
        }
    }

    get Price1() {
        return this.EntityPM.Price1;
    }
    set Price1(value: number) {
        if (this.EntityPM.Price1 != value) {
            this.EntityPM.Price1 = value;
        }
    }

    get Price2() {
        return this.EntityPM.Price2;
    }
    set Price2(value: number) {
        if (this.EntityPM.Price2 != value) {
            this.EntityPM.Price2 = value;
        }
    }

    get Price3() {
        return this.EntityPM.Price3;
    }
    set Price3(value: number) {
        if (this.EntityPM.Price3 != value) {
            this.EntityPM.Price3 = value;
        }
    }

    get Price4() {
        return this.EntityPM.Price4;
    }
    set Price4(value: number) {
        if (this.EntityPM.Price4 != value) {
            this.EntityPM.Price4 = value;
        }
    }

    get Price5() {
        return this.EntityPM.Price5;
    }
    set Price5(value: number) {
        if (this.EntityPM.Price5 != value) {
            this.EntityPM.Price5 = value;
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

export class ContainerChargesClass {
    public Header: string;
    public Index: number;
}
