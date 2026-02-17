import { Component, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { TariffDomainService, TariffFilterParameter, ExcelTariffLines } from '../../../Services/TariffDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { TariffVersionExtendedPMService } from '../../../Services/ExtendedPMs/TariffVersionExtendedPMService';
import { DatePipe } from '@angular/common';
import { TariffVersionAllInChargePM } from '../../../EntityPMs/TariffVersionAllInChargePM';
import { AirCostTariffLineData } from '../../../../TariffModule/Components/EditTabs/Tariff/TariffLineData';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
declare var ResultAsArray: any;

@Component({
    moduleId: module.id,
    templateUrl: './VersionTabComponent.html',
})

export class VersionTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public TariffsLinesSource: ObservableCollection;
    public DeletedTariffsLines: AirCostTariffLineData [] = [];
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    private TariffDomainService: TariffDomainService;
    private DocumentExtendedService: DocumentsFilingExtendedPMService;
    public IsApproveVersionButtonVisible: boolean = false;
    public IsUpdateMissingPortsVisible: boolean = false;
    public IsDraftVersion: boolean = true;
    public CurrentVersion: TariffVersionPM;
    private FileName: string;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsFirstDraft = false;
    public SelectedVersionNumber: number;
    public OriginDependencyFilterValue: string = "A";
    public DestinationDependencyFilterValue = "A";

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    Intialize(args: any) {
        this.CurrentVersion = args['CurrentVersion'];
        this.SelectedVersionNumber = args['SelectedVersionNumber'];
        this.LoadVersions();
        this.SetOriginDependencyFilterValue();

    }

    SetOriginDependencyFilterValue() {
        if (this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
        }
    }

    LoadVersions() {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService();
        this.TariffDomainService = new TariffDomainService();

        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
        }

        if (this.IsDraftVersion) {
            this.IsComparToChecked = true;
        }

        this.LoadCompareToVersions();

       
        if (this.CurrentVersion.IsDraft) {
            this.FillTariffLines(this.CurrentVersion.TariffLines);
        }

        else {
            this.LoadTariffLines("currentVersion");
        }
        this.GetTariffSettings();
        this.SetUIProperties();
        this.SetStepsLabelsAndVisibility();
    }

    private GetTariffSettings() {
        this.TariffDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.warningPercentage = myResponse.Result.DefaultWarningPercentage;
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private PriceStepsModifiedEvent: any = null;
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

                    if (this.isUploadExcelFinished) {
                        this.isUploadExcelFinished = false;
                        CachedDataManager.RefreshTableData("Port", true);
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    if (this.isUpdateMissingPortsClicked) {
                        this.isUpdateMissingPortsClicked = false;
                        CachedDataManager.RefreshTableData("Port", true);
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    this.LoadVersions();
                    //this.PriceStepsModifiedEvent = this.CurrentSession.SessionEvent.subscribe((res) => {
                    //    if (res == "PriceStepsModified") {
                    //        this.LoadVersions();
                    //    }
                    //});
                }

                else {
                    this.StopAllFlags();
                }
            });
        }
    }

    StopAllFlags() {
        if (this.EntityPM.IsApprovingDraftVersion) {
            this.EntityPM.IsApprovingDraftVersion = false;
        }

        this.isApproveButtonClicked = false;
        this.isUploadExcelFinished = false;
        this.isCopyButtonClicked = false;
    }

    KillEvents() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.PriceStepsModifiedEvent);
    }

    ngOnDestroy() {
        this.KillEvents();
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
        var isUpdateMissingPortsVisible: boolean = false;

        if (this.IsDraftVersion && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
            if (this.IsDraftVersion) {
                if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
                    isApproveVersionButtonVisible = true;
                }
                if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "UPDATEMISSINGPORTS")) {
                    isUpdateMissingPortsVisible = true;
                }
            }
        }

        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
        this.IsUpdateMissingPortsVisible = isUpdateMissingPortsVisible;
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

    get InitialEnddate() {
        return (this.CurrentVersion == null ? null : this.CurrentVersion.ExpirationDate != null ? this.CurrentVersion.ExpirationDate : this.CurrentVersion.InitialEnddate);
    }
    set InitialEnddate(value: Date) {
        if (this.CurrentVersion.InitialEnddate != value) {
            this.CurrentVersion.InitialEnddate = value;
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

    get PriceSteps() {
        return this.EntityPM.PriceSteps;
    }
    set PriceSteps(value: string) {
        if (this.EntityPM.PriceSteps != value) {
            this.EntityPM.PriceSteps = value;
        }
    }

    private ItemsCollection: AirCostTariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }
      
        this.ItemsCollection = [];  
        

        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            this.ItemsCollection.push(new AirCostTariffLineData(item, this));
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
        this.ItemsCollection.forEach((item: AirCostTariffLineData) => {
            item.IsNewEntity = false;

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
                this.DeletedTariffsLines.push(new AirCostTariffLineData(item, this));// Deleted 
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
    
    public Step1PriceLabel: string;
    public Step2PriceLabel: string;
    public Step3PriceLabel: string;
    public Step4PriceLabel: string;
    public Step5PriceLabel: string;
    public Step6PriceLabel: string;
    public Step7PriceLabel: string;
    public Step8PriceLabel: string;

    public Step1PriceVisibility: boolean;
    public Step2PriceVisibility: boolean;
    public Step3PriceVisibility: boolean;
    public Step4PriceVisibility: boolean;
    public Step5PriceVisibility: boolean;
    public Step6PriceVisibility: boolean;
    public Step7PriceVisibility: boolean;
    public Step8PriceVisibility: boolean;

    ResetStepsVisibility() {
        this.Step1PriceVisibility = false;
        this.Step2PriceVisibility = false;
        this.Step3PriceVisibility = false;
        this.Step4PriceVisibility = false;
        this.Step5PriceVisibility = false;
        this.Step6PriceVisibility = false;
        this.Step7PriceVisibility = false;
        this.Step8PriceVisibility = false;
    }
    SetStepsLabelsAndVisibility() {
        if (!AppTool.IsNullOrEmpty(this.PriceSteps)) {
            this.ResetStepsVisibility();
            if (this.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = [] = this.PriceSteps.split(",");
                var count = steps.length;
                if (count == 0) {
                    this.Step1PriceLabel = this.PriceSteps;
                    this.Step1PriceVisibility = true;
                }
                for (var i = 1; i <= count; i++) {
                    this["Step" + i + "PriceLabel"] = steps[i - 1] + " KG";
                    this["Step" + i + "PriceVisibility"] = true;
                }
            }
            else {
                this.Step1PriceLabel = this.PriceSteps;
                this.Step1PriceVisibility = true;
            }
        }
    }

    AddTariffLine() {
        var logWindow = new LogitudeWindow();
        var itemPM = new TariffLinePM(null);
        itemPM.StartDate = this.StartDate;
        itemPM.ExpirationDate = this.InitialEnddate;
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

        var itemComponent = new AirCostTariffLineData(itemPM, this, true);
        logWindow.WindowArgs = { DataContext: itemComponent, EntityPM: itemPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: AirCostTariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: item, EntityPM: item.EntityPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    DeleteTariffButtonClicked(item: AirCostTariffLineData) {
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
        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var tariffLine = new TariffLinePM(null);
            tariffLine.StartDate = this.StartDate;
            tariffLine.ExpirationDate = this.InitialEnddate;
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
            
            if (this.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = this.PriceSteps.split(",");
                var count = steps.length;

                tariffLine.MinPrice = item.MinPrice;
                tariffLine.MinPriceText = item.MinPriceText;

                for (var i = 1; i <= count; i++) {
                    tariffLine["Step" + i + "Price"] = item["Step" + i + "Price"];
                    tariffLine["Step" + i + "PriceText"] = item["Step" + i + "PriceText"];
                }                
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
        var windowTitle = "New Copy Version";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 200;
        logWindow.WindowArgs = { CurrentVersion: this.CurrentVersion, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = windowTitle;
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s && d == "ok") {
                    this.isCopyButtonClicked = true;
                    this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
                    this.EntityPM.LastStartDate = this.StartDate;
                    this.EntityPM.LastExpirationDate = this.InitialEnddate;
                    var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
                    copiedVersion.TariffId = this.CurrentVersion.TariffId;
                    copiedVersion.Version = this.EntityPM.LastVersion;
                    copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
                    copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
                    copiedVersion.StartDate = s.StartDate;
                    copiedVersion.InitialEnddate = s.InitialEnddate;
                    copiedVersion.ExpirationDate = s.InitialEnddate;

                    copiedVersion.IsDraft = true;
                    copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
                    copiedVersion.ParentVersionNumber = this.CurrentVersion.Version;
                    this.EntityPM.AddTariffVersion(copiedVersion);

                    this.loadedTariffLines.forEach(item => {
                        var tariffLine = new TariffLinePM(copiedVersion);
                        tariffLine.StartDate = this.StartDate;
                        tariffLine.ExpirationDate = this.InitialEnddate;
                        tariffLine.Tenant = SessionLocator.Tenant;
                        tariffLine.Version = copiedVersion.Version;
                        tariffLine.OriginPortId = item.OriginPortId;
                        tariffLine.OriginPortCode = item.OriginPortCode;
                        tariffLine.OriginPortName = item.OriginPortName;
                        tariffLine.DestinationPortId = item.DestinationPortId;
                        tariffLine.DestinationPortCode = item.DestinationPortCode;
                        tariffLine.DestinationPortName = item.DestinationPortName;
                        tariffLine.MinPrice = item.MinPrice;
                        tariffLine.Step1Price = item.Step1Price;
                        tariffLine.Step2Price = item.Step2Price;
                        tariffLine.Step3Price = item.Step3Price;
                        tariffLine.Step4Price = item.Step4Price;
                        tariffLine.Step5Price = item.Step5Price;
                        tariffLine.Step6Price = item.Step6Price;
                        tariffLine.Step7Price = item.Step7Price;
                        tariffLine.Step8Price = item.Step8Price;
                        tariffLine.Index = item.Index;
                        tariffLine.Notes = item.Notes;
                        copiedVersion.AddTariffLine(tariffLine);
                    });

                    this.CurrentVersion.TariffAllInCharges.forEach(item => {
                        var allInCharge = new TariffVersionAllInChargePM(copiedVersion);
                        allInCharge.ChargesTypeId = item.ChargesTypeId;
                        allInCharge.TariffId = this.EntityPM.Id;
                        allInCharge.Tenant = SessionLocator.Tenant;
                        allInCharge.Version = copiedVersion.Version;
                        allInCharge.AddDate = DateTool.GetCurrentDateAsUtc();
                        allInCharge.AddedByUserId = SessionInfo.LoggedUserId;
                        copiedVersion.AddTariffVersionAllInCharge(allInCharge);
                    });

                    this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
                }
            });
        });

        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
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
            var from: string = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
            var to: string = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');
            var newVersion: VersionClass = new VersionClass();
            newVersion.Version = item.Version;
            newVersion.ParentVersionNumber = item.ParentVersionNumber;
            newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";
            newVersion.Id = item.TariffId; 
            this.VersionsList.push(newVersion);
        });

        this.SelectedVersion = this.VersionsList.filter(a => a.Version == this.CurrentVersion.ParentVersionNumber)[0];

        if (this.VersionsList == null || (this.VersionsList != null && this.VersionsList.length == 0)) {
            this.isComparToChecked = false;
            this.IsFirstDraft = true;
        }
        this.UIProperties.SetEnabled("WarningPercentage", null, this.IsComparToChecked && !this.IsFirstDraft);

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

    AllInChargesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { TariffPM: this.EntityPM, VersionPM: this.CurrentVersion, IsEditingEnabled: this.IsDraftVersion };
        logWindow.Title = "All-In Charges";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditAllInChargesComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                
            }
        });
    }

    private isUpdateMissingPortsClicked: boolean = false;
    UpdateMissingPortsClicked() {
        if (!this.isUpdateMissingPortsClicked) {
            this.isUpdateMissingPortsClicked = true;
            this.EntityPM.IsUpdatingMissingPorts = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
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

