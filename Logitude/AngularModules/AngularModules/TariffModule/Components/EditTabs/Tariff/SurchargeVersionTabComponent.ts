import { Component, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { PortPM } from '../../../../Common/EntityPMs/PortPM';
import { TariffDomainService, TariffFilterParameter, ExcelTariffLines } from '../../../Services/TariffDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { AppTool, FontTool, DateTool, FormatTool } from '../../../../Infrastructure/Tools';
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
declare var ResultAsArray: any;

@Component({
    moduleId: module.id,
    templateUrl: './SurchargeVersionTabComponent.html',
})

export class SurchargeVersionTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public TariffsLinesSource: ObservableCollection;
    public DataContext = this;
    private EntityArgs: EntityArgs;
    public IsResourcesReady: boolean = false;
    private TariffDomainService: TariffDomainService;
    private DocumentExtendedService: DocumentsFilingExtendedPMService;
    public IsApproveVersionButtonVisible: boolean = false;
    public IsDraftVersion: boolean = true;
    public CurrentVersion: TariffVersionPM;
    private FileName: string;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsUpdateSurchargesButtonVisible: boolean = false;
    public IsFirstDraft: boolean = false;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        if (this.EntityPM.TariffVersions == null || (this.EntityPM.TariffVersions != null && this.EntityPM.TariffVersions.length == 1)) {
            this.isComparToChecked = false;
            this.IsFirstDraft = true;
        }
        this.EntityArgs = entityArgs;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CurrentVersion = this.EntityPM.TariffVersions.filter(d => d.Version == this.CurrentVersion.Version)[0];

                    if (this.CurrentVersion == null) {
                        // after creating new version
                        this.CurrentVersion = this.EntityPM.TariffVersions.filter(d => d.Version == this.EntityPM.LastVersion)[0];
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

                    if (this.isUploadExcelFinished) {
                        this.isUploadExcelFinished = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    this.SetSurchargesLabelsAndVisibility();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    }

    public AllChargesTypes: ChargesTypeList[];
    public AllMeasurements: MeasurementList[];
    Intialize(args: any) {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.EntityPM = this.EntityArgs.EntityPM;
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService();
        this.TariffDomainService = new TariffDomainService();

        this.CurrentVersion = args['CurrentVersion'];

        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
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
                item.Name = iChargeType.Code;
                item.DisplyText = iChargeType.Code;
                item.Code_Int = index;

                var iMeasurement: MeasurementList = this.AllMeasurements.filter(f => f.Id == iMeasurementId)[0];
                if (iMeasurement) {
                    item.DisplyText = iChargeType.Code + " (" + iMeasurement.Code + ")";
                }

                this.tariffCharges.push(item);

                this['Surcharge' + index + 'PriceLabel'] = item.DisplyText;
                this['Surcharge' + index + 'PriceVisibility'] = true;
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

    private ItemsCollection: TariffLineData[] = [];
    public DeletedTariffsLines: TariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }

        this.ItemsCollection = [];        

        tariffLines.sort(p => p.Index).forEach(item => {
            this.ItemsCollection.push(new TariffLineData(item, this));
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
        this.ItemsCollection.forEach((item: TariffLineData) => {
            var line = this.compareTariffLines.sort(p => p.Index).filter(a => a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId)[0];
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

        this.compareTariffLines.sort(p => p.Index).forEach(item => {
            var line = lines.sort(p => p.Index).filter(a => a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId)[0];
            if (line == null) {
                this.DeletedTariffsLines.push(new TariffLineData(item, this));// Deleted 
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

            if (this.EntityPM.TypeCode == "ASC") {
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
        this.UIProperties.SetEnabled("WarningPercentage", null, this.IsComparToChecked);
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

        var Version: TariffVersionPM = this.EntityPM.TariffVersions.filter(p => p.Version == itemPM.Version)[0];
        if (Version) {
            if (Version.TariffLines.length > 0) {
                var index = Math.max.apply(Math, Version.TariffLines.map(function (o) { return o.Index; })) + 1;
                if (index) {
                    itemPM.Index = index;
                }
            }
        }

        var itemComponent = new TariffLineData(itemPM, this, true);
        logWindow.DataContext = itemComponent;
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: TariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.DataContext = item;
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    DeleteTariffButtonClicked(item: TariffLineData) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Tariff Line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                this.TariffsLinesSource.Remove(item);
                this.FillTariffLines(this.CurrentVersion.TariffLines);
            }
        });
    }

    // Upload Excel File 
    OnFileChanged(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                var file = fileEvent.target.files[0];
                this.UploadExcel(file);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    }
    UploadExcel(file: any) {
        this.FileName = null;
        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
            }
        }

        if (file && file.size > 0) {
            this.DocumentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }
    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }
    ConvertArrayBufferToBase64(file: any, context: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            var filter = new TariffFilterParameter();
            filter.FileData = window.btoa(binary);
            filter.PriceSteps = context.PriceSteps;
            filter.TariffId = context.EntityPM.Id;
            filter.Version = context.CurrentVersion.Version;
            filter.TariffType = context.EntityPM.TypeCode;
            filter.FileName = context.FileName;

            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
        context.EntityPM.FileUploadedName = this.FileName;

    }
    SendExcelToServer(filter: any) {
        this.TariffDomainService.PostUploadExcelFile(filter).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var tariffLines: ExcelTariffLines[] = response.Result;
                if (tariffLines) {
                    this.CurrentVersion.TariffLines = [];
                    this.InsertNewRowsFromExcel(tariffLines);
                }
            }
        });
    }

    private isUploadExcelFinished: boolean = false;
    private InsertNewRowsFromExcel(tariffLines: ExcelTariffLines[]) {
        tariffLines.forEach(item => {
            var tariffLine = new TariffLinePM(null);
            tariffLine.StartDate = this.StartDate;
            tariffLine.ExpirationDate = this.ExpirationDate;
            tariffLine.Tenant = SessionLocator.Tenant;
            tariffLine.Version = this.CurrentVersion.Version;
            tariffLine.OriginPortId = item.FromPortId;
            tariffLine.OriginPortCode = item.FromPortCode;
            tariffLine.OriginPortName = item.FromPortName;
            tariffLine.DestinationPortId = item.ToPortId;
            tariffLine.DestinationPortCode = item.ToPortCode;
            tariffLine.DestinationPortName = item.ToPortName;
            tariffLine.OriginPortText = item.FromPortText;
            tariffLine.DestinationPortText = item.ToPortText;
            tariffLine.HasErrors = item.HasErrors;
            tariffLine.ErrorText = item.ErrorText;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            tariffLine.StartDate = item.StartDate;
            //tariffLine.StartDateText = item.StartDateText;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1Id)) {
                tariffLine.Surcharge1Price = item.Surcharge1Price;
                tariffLine.Surcharge1PriceText = item.Surcharge1PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2Id)) {
                tariffLine.Surcharge2Price = item.Surcharge2Price;
                tariffLine.Surcharge2PriceText = item.Surcharge2PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3Id)) {
                tariffLine.Surcharge3Price = item.Surcharge3Price;
                tariffLine.Surcharge3PriceText = item.Surcharge3PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4Id)) {
                tariffLine.Surcharge4Price = item.Surcharge4Price;
                tariffLine.Surcharge4PriceText = item.Surcharge4PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5Id)) {
                tariffLine.Surcharge5Price = item.Surcharge5Price;
                tariffLine.Surcharge5PriceText = item.Surcharge5PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6Id)) {
                tariffLine.Surcharge6Price = item.Surcharge6Price;
                tariffLine.Surcharge6PriceText = item.Surcharge6PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7Id)) {
                tariffLine.Surcharge7Price = item.Surcharge7Price;
                tariffLine.Surcharge7PriceText = item.Surcharge7PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8Id)) {
                tariffLine.Surcharge8Price = item.Surcharge8Price;
                tariffLine.Surcharge8PriceText = item.Surcharge8PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9Id)) {
                tariffLine.Surcharge9Price = item.Surcharge9Price;
                tariffLine.Surcharge9PriceText = item.Surcharge9PriceText;
            }

            if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10Id)) {
                tariffLine.Surcharge10Price = item.Surcharge10Price;
                tariffLine.Surcharge10PriceText = item.Surcharge10PriceText;
            }

            this.CurrentVersion.AddTariffLine(tariffLine);
        });

        this.EntityPM.TariffLinesAddedFromExcel = true;
        this.isUploadExcelFinished = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
    }

    // Download Excel 
    DownloadExcelClicked(type: string) {
        this.TariffDomainService.DownloadTariff(this.EntityPM.Id, this.CurrentVersion.Version, type).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "Tariffs" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
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
            this.EntityPM.IsApprovingDraftVersion = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }

        //var errors: string[] = [];

        //if (this.CurrentVersion.TariffLines.filter(d => d.HasErrors).length > 0) {
        //    errors.push("Invalid Tariff Lines");
        //}

        ////if (DateTool.GetDateParts(this.CurrentVersion.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
        ////    errors.push("Approving past version is not allowed, please update the dates");
        ////}

        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;

        //if (errors.length == 0) {
        //    this.isApproveButtonClicked = true;

        //    if (this.EntityPM.IsDirty) {
        //        this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
        //    }

        //    else {
        //        this.DoApprove();
        //    }
        //}
    }
    //private DoApprove() {
    //    this.TariffDomainService.ApproveVersion(this.EntityPM.Id, this.CurrentVersion.Version).subscribe((response: ServiceResponse) => {
    //        if (!response.HasError) {
    //            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //        }
    //    });
    //}

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


        //var windowTitle = "New Copy Version";
        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 450;
        //logWindow.Height = 200;
        //logWindow.WindowArgs = this.CurrentVersion;
        //logWindow.Title = windowTitle;
        //logWindow.ComponentLoaded.subscribe(s => {
            //logWindow.WindowClosed.subscribe(d => {
                //if (s && d == "ok") {
                    this.isCopyButtonClicked = true;

                    this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
                    this.EntityPM.LastStartDate = this.StartDate;
                    this.EntityPM.LastExpirationDate = this.ExpirationDate;

                    var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
                    copiedVersion.TariffId = this.CurrentVersion.TariffId;
                    copiedVersion.Version = this.EntityPM.LastVersion;
                    copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
                    copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
                    //copiedVersion.ExpirationDate = s.ExpirationDate;
                    copiedVersion.IsDraft = true;
                    //copiedVersion.StartDate = s.StartDate;
                    copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
                    copiedVersion.ParentVersionNumber = this.CurrentVersion.Version;
                    this.EntityPM.AddTariffVersion(copiedVersion);
                    this.loadedTariffLines.sort(p => p.Index).forEach(item => {
                        var tariffLine = new TariffLinePM(copiedVersion);
                        tariffLine.StartDate = item.StartDate;
                        tariffLine.ExpirationDate = item.ExpirationDate;
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
                        copiedVersion.AddTariffLine(tariffLine);
                    });

                    this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
               // }
            //});
       // });

        //logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
    }

    UpdateSurchargesClicked() {
        var args: UpdateTariffArgs = new UpdateTariffArgs();
        args.Version = this.CurrentVersion;
        args.TariffCharges = this.tariffCharges;
        args.AirlineId = this.EntityPM.SellerId;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Title = "Tariff Surchage Update";
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UpdateSurchargesComponent');
    }
}

export class TariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: TariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;
    public IsEditEnabled: boolean = false;
    public ComparedEntity: TariffLinePM;
    constructor(entity: TariffLinePM, public FatherComponent: SurchargeVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;
        this.SetUIProperties();
    }
    
    private CheckIfLineHasError() {
        var error: boolean = false;
        var errorText: string;

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

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 1 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 1 price format is invalid"
            }
        }
        
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 2 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 2 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 3 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 3 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 4 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 4 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 5 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 5 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 6 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 6 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 7 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 7 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 8 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 8 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 9 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 9 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10PriceText) && AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 10 price format is invalid";
            }

            else {
                errorText = errorText + ", Surcharge 10 price format is invalid"
            }
        }

        this.HasErrors = error;
        this.ErrorText = errorText;
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
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
                if (!AppTool.IsNullOrZero(surcharge10ComparingValue) && !AppTool.IsNullOrZero(this.ComparedEntity.Surcharge10Price))  {
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
            this.SetUIProperties();
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

    originPort: PortPM;
    get OriginPort() { return this.originPort; }
    set OriginPort(value: PortPM) {
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
            this.SetUIProperties();
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

    destinationPort: PortPM;
    get DestinationPort() { return this.destinationPort; }
    set DestinationPort(value: PortPM) {
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
            this.CompareSurcharge1Price();
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

    // Surcharge 2
    get Surcharge2Price() {
        return this.EntityPM.Surcharge2Price;
    }
    set Surcharge2Price(value: number) {
        if (this.EntityPM.Surcharge2Price != value) {
            this.EntityPM.Surcharge2Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge2Price();
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

    // Surcharge 3
    get Surcharge3Price() {
        return this.EntityPM.Surcharge3Price;
    }
    set Surcharge3Price(value: number) {
        if (this.EntityPM.Surcharge3Price != value) {
            this.EntityPM.Surcharge3Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge3Price();
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

    // Surcharge 4
    get Surcharge4Price() {
        return this.EntityPM.Surcharge4Price;
    }
    set Surcharge4Price(value: number) {
        if (this.EntityPM.Surcharge4Price != value) {
            this.EntityPM.Surcharge4Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge4Price();
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

    // Surcharge 5
    get Surcharge5Price() {
        return this.EntityPM.Surcharge5Price;
    }
    set Surcharge5Price(value: number) {
        if (this.EntityPM.Surcharge5Price != value) {
            this.EntityPM.Surcharge5Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge5Price();
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

    // Surcharge 6
    get Surcharge6Price() {
        return this.EntityPM.Surcharge6Price;
    }
    set Surcharge6Price(value: number) {
        if (this.EntityPM.Surcharge6Price != value) {
            this.EntityPM.Surcharge6Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge6Price();
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

    // Surcharge 7
    get Surcharge7Price() {
        return this.EntityPM.Surcharge7Price;
    }
    set Surcharge7Price(value: number) {
        if (this.EntityPM.Surcharge7Price != value) {
            this.EntityPM.Surcharge7Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge7Price();
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

    // Surcharge 8
    get Surcharge8Price() {
        return this.EntityPM.Surcharge8Price;
    }
    set Surcharge8Price(value: number) {
        if (this.EntityPM.Surcharge8Price != value) {
            this.EntityPM.Surcharge8Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge8Price();
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

    // Surcharge 9
    get Surcharge9Price() {
        return this.EntityPM.Surcharge9Price;
    }
    set Surcharge9Price(value: number) {
        if (this.EntityPM.Surcharge9Price != value) {
            this.EntityPM.Surcharge9Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge9Price();
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

    // Surcharge 10
    get Surcharge10Price() {
        return this.EntityPM.Surcharge10Price;
    }
    set Surcharge10Price(value: number) {
        if (this.EntityPM.Surcharge10Price != value) {
            this.EntityPM.Surcharge10Price = value;
            this.CheckIfLineHasError();
            this.CompareSurcharge10Price();
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
}

export class VersionClass {
    public Code: number;
    public Name: string;
    public Id: string;
    public Version: number;
    public ParentVersionNumber: number;
}
