import { Component, OnDestroy, EventEmitter } from '@angular/core';
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
import { OceanFCLFreightTariffLineData } from '../../../../TariffModule/Components/EditTabs/Tariff/TariffLineData';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { PackageTypeListService } from '../../../../Common/Services/StandardLists/PackageTypeListService';
import { PackageTypeList } from '../../../../Common/EntityLists/PackageTypeList';
declare var ResultAsArray: any;

@Component({
    
    templateUrl: './OceanFCLVersionTabComponent.html',
})

export class OceanFCLVersionTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public TariffsLinesSource: ObservableCollection;
    public DeletedTariffsLines: OceanFCLFreightTariffLineData[] = [];
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    private TariffDomainService: TariffDomainService;
    private DocumentExtendedService: DocumentsFilingExtendedPMService;
    public IsApproveVersionButtonVisible: boolean = false;
    public IsUpdateMissingPortsVisible: boolean = false;
    public IsDraftVersion: boolean = true;
    public CurrentVersion: TariffVersionPM;
    private FileName: string;
    private FileExtension: string;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsFirstDraft = false;
    public SelectedVersionNumber: number;    
    public AllInCharges: string;
    public LinesCount: number;
    public selectedRow: any;
    public changeScrollPosition: EventEmitter<any> = new EventEmitter();
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    public AllPackageTypes: PackageTypeList[];
    public LineIdFromPriceCheck: string;
    Intialize(args: any) {
        this.CurrentVersion = args['CurrentVersion'];
        this.SelectedVersionNumber = args['SelectedVersionNumber'];        
        this.LineIdFromPriceCheck = args['LineIdFromPriceCheck'];

        var iPackageTypeListService = new PackageTypeListService();

        iPackageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPackageTypes = myResponse.Result;                

                this.LoadVersions();                
            }
      });
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

        this.BuildAllInChargesText();
        this.GetTariffSettings();
        this.SetUIProperties();
        this.SetContainersLabelsAndVisibility()
    }

    private BuildAllInChargesText() {
    var allInCharges: string = null;

    if (this.CurrentVersion != null) {
      var codesList: TariffVersionAllInChargePM[] = [];
      var addDots:boolean = false;

      if(this.CurrentVersion.TariffAllInCharges.length > 4){
        codesList = this.CurrentVersion.TariffAllInCharges.slice(0, 4);
        addDots = true;
      }

      else {
        codesList = this.CurrentVersion.TariffAllInCharges;
      }

      codesList.forEach((item: TariffVersionAllInChargePM) => {
        if (AppTool.IsNullOrEmpty(allInCharges)) {
          allInCharges = item.ChargesTypeCode;
        }

        else {
          allInCharges = allInCharges + ", " + item.ChargesTypeCode;
        }
      });
    }

    if(addDots) {
      allInCharges = allInCharges + "...";
    }

    this.AllInCharges = allInCharges;
  }

    private GetTariffSettings() {
        this.TariffDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.warningPercentage = myResponse.Result.DefaultWarningPercentage;
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
                  
                    if (this.isUpdateMissingPortsClicked) {
                        this.isUpdateMissingPortsClicked = false;
                        CachedDataManager.RefreshTableData("Port", true);
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    if (this.isRefreshTranslationsClicked) {
                        this.isRefreshTranslationsClicked = false;
                        this.DoRefresh();
                    }

                    this.LoadVersions();
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
        this.isCopyButtonClicked = false;
    }

    KillEvents() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
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
    
    private ItemsCollection: OceanFCLFreightTariffLineData[] = [];
    FillTariffLines(tariffLines: TariffLinePM[]) {
        this.LinesCount = tariffLines.length;

        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }

        this.ItemsCollection = [];

        var count = 0; var selectedIndexRow = 0; var isItemSelectExist = false;
        tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var itemOceanFCL = new OceanFCLFreightTariffLineData(item, this)
            count++;
            this.ItemsCollection.push(itemOceanFCL);
            if (!AppTool.IsNullOrEmpty(this.LineIdFromPriceCheck) && itemOceanFCL.EntityPM.Id == this.LineIdFromPriceCheck) {
                this.selectedRow = itemOceanFCL;
                selectedIndexRow = count;
                isItemSelectExist = true;
            }
        });

    this.TariffsLinesSource.InsertCollection(this.ItemsCollection);

    //this.InitializePager();
    //this.FillGridPagerItems();
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
        this.ItemsCollection.forEach((item: OceanFCLFreightTariffLineData) => {
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
                this.DeletedTariffsLines.push(new OceanFCLFreightTariffLineData(item, this));// Deleted 
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

    public Container1PriceLabel: string;
    public Container2PriceLabel: string;
    public Container3PriceLabel: string;
    public Container4PriceLabel: string;
    public Container5PriceLabel: string;

    public Container1PriceVisibility: boolean;
    public Container2PriceVisibility: boolean;
    public Container3PriceVisibility: boolean;
    public Container4PriceVisibility: boolean;
    public Container5PriceVisibility: boolean;

    SetContainersLabelsAndVisibility() {
        this.AddColumn(this.EntityPM.ContainerType1Id, 1);
        this.AddColumn(this.EntityPM.ContainerType2Id, 2);
        this.AddColumn(this.EntityPM.ContainerType3Id, 3);
        this.AddColumn(this.EntityPM.ContainerType4Id, 4);
        this.AddColumn(this.EntityPM.ContainerType5Id, 5);
    }

    AddColumn(iContainerTypeId: string, index: number) {
        if (!AppTool.IsNullOrEmpty(iContainerTypeId)) {
            var iPackageType: PackageTypeList = this.AllPackageTypes.filter(a => a.Id == iContainerTypeId)[0];
            if (iPackageType) {
                this['Container' + index + 'PriceLabel'] = iPackageType.Code;
                this['Container' + index + 'PriceVisibility'] = true;
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

        var itemComponent = new OceanFCLFreightTariffLineData(itemPM, this, true);
        logWindow.WindowArgs = { DataContext: itemComponent, EntityPM: itemPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    EditTariffButtonClicked(item: OceanFCLFreightTariffLineData) {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { DataContext: item, EntityPM: item.EntityPM, TariffType: this.EntityPM.TypeCode };
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    }

    DeleteTariffButtonClicked(item: OceanFCLFreightTariffLineData) {
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
    this.CurrentSession.StartBusyIndicator("Uploading...");

        this.FileName = null;
        this.FileExtension = null;

        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
                this.FileExtension = name[1];
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
            filter.TariffId = context.EntityPM.Id;
            filter.Version = context.CurrentVersion.Version;
            filter.TariffType = context.EntityPM.TypeCode;
            filter.TariffType = context.EntityPM.TypeCode;
            filter.FileName = context.FileName;
            filter.FileExtension = context.FileExtension;
            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
        context.EntityPM.FileUploadedName = this.FileName;
    }
    SendExcelToServer(filter: any) {
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.TariffDomainService.PostUploadExcelFile(filter).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
              this.CurrentSession.StopBusyIndicator();
              CachedDataManager.RefreshTableData("Port", true);
              this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }

            else {
              this.CurrentSession.StopBusyIndicator();
              this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
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
                        tariffLine.OriginPortCombinedCode = item.OriginPortCombinedCode;
                        tariffLine.OriginPortName = item.OriginPortName;
                        tariffLine.DestinationPortId = item.DestinationPortId;
                        tariffLine.DestinationPortCode = item.DestinationPortCode;
                        tariffLine.DestinationPortCombinedCode = item.DestinationPortCombinedCode;
                        tariffLine.DestinationPortName = item.DestinationPortName;
                        tariffLine.Surcharge1Price = item.Surcharge1Price;
                        tariffLine.Surcharge2Price = item.Surcharge2Price;
                        tariffLine.Surcharge3Price = item.Surcharge3Price;
                        tariffLine.Surcharge4Price = item.Surcharge4Price;
                        tariffLine.Surcharge5Price = item.Surcharge5Price;
                        tariffLine.Index = item.Index;
                        tariffLine.Notes = item.Notes;
                        tariffLine.TransitTime = item.TransitTime;
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
                this.BuildAllInChargesText();
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

    private isRefreshTranslationsClicked: boolean = false;
    RefreshPortsFromTranslations() {
        if (!this.isRefreshTranslationsClicked) {
            this.isRefreshTranslationsClicked = true;
            this.EntityPM.IsRefreshTranslations = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    private DoRefresh() {
        this.TariffDomainService.RefreshPortsFromTranslations(this.EntityPM.Id, this.VersionNumber).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
  }

    private isAllSelected: boolean = false;
    get IsAllSelected() { return this.isAllSelected; }
    set IsAllSelected(value: boolean) {
        if (this.isAllSelected != value) {
            this.isAllSelected = value;

            this.ItemsCollection.forEach((item: OceanFCLFreightTariffLineData) => {
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
                    this.ItemsCollection.filter(d => d.IsLineSelected).forEach((item: OceanFCLFreightTariffLineData) => {
                        this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                    });

                    this.FillTariffLines(this.CurrentVersion.TariffLines);
                }
            });
        }
    }

    ViewUploadedExcelFilesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Uploaded Excel Files";
        logWindow.Width = 600;
        logWindow.Height = 500;
        logWindow.WindowArgs = { TariffId: this.EntityPM.Id, Version: this.CurrentVersion.Version };
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UploadedExcelsComponent');
    }
}

export class VersionClass {
    public Code: number;
    public Name: string;
    public Id: string;
    public Version: number;
    public ParentVersionNumber: number;
}

