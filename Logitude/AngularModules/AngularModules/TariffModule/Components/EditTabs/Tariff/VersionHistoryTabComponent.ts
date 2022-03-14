import { Component, OnDestroy, EventEmitter } from '@angular/core';
import { DatePipe } from '@angular/common';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { TariffVersionAllInChargePM } from '../../../EntityPMs/TariffVersionAllInChargePM';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { TariffDomainService } from '../../../Services/TariffDomainService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { MeasurementListService } from '../../../../Common/Services/StandardLists/MeasurementListService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { MeasurementList } from '../../../../Common/EntityLists/MeasurementList';
import { TariffVersionExtendedPMService } from '../../../Services/ExtendedPMs/TariffVersionExtendedPMService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PackageTypeList } from '../../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../../Common/Services/StandardLists/PackageTypeListService';
import { TariffLinesContainersPricePM } from '../../../EntityPMs/TariffLinesContainersPricePM';

@Component({
    
    templateUrl: './VersionHistoryTabComponent.html',
})

export class VersionHistoryTabComponent implements OnDestroy {
  public IsDraftVersion: any;
  public EditTariffButtonClicked(item: any) { }
  public DeleteTariffButtonClicked(item: any) { }


    public EntityPM: TariffPM;
    public VersionPM: TariffVersionPM;
    public VersionLinesSource: ObservableCollection;
    private TariffDomainService: TariffDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsActionsEnabled: boolean = false;
    public IsDownloadExcelTemplateVisible: boolean = false;
    public IsAllInChargesVisible: boolean = false;
    public LineIdFromPriceCheck: string;
    public chargeableWeightInKG: number;
    public selectedRow: any;
    public changeScrollPosition: EventEmitter<any> = new EventEmitter();
    public darkerColler: string = "#f8ca12";
    public IsViaFieldVisible: boolean = true;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.VersionLinesSource = new ObservableCollection([]);
        this.TariffDomainService = new TariffDomainService();

        if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            this.SetStepsLabelsAndVisibility();
            this.IsDownloadExcelTemplateVisible = true;
            this.IsAllInChargesVisible = true;
            this.GetAllPackageTypes();
        }

        else if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC" || this.EntityPM.TypeCode == "IFT") {
            this.IsViaFieldVisible = false;
            this.GetAllChargesTypes();
        }
        
        this.LoadVersions();
        this.Listen();
    }
    Intialize(args: any) {
        this.LineIdFromPriceCheck = args['LineIdFromPriceCheck'];
        this.chargeableWeightInKG = args['ChargeableWeightInKG'];
    }

    private AllChargesTypes: ChargesTypeList[];
    public AllMeasurements: MeasurementList[];
    private GetAllChargesTypes() {
        var iChargesTypeListService = new ChargesTypeListService();
        var iMeasurementListService = new MeasurementListService();

        iChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;

                iMeasurementListService.getAllFromCache().subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        this.AllMeasurements = myResponse2.Result;

                        this.SetSurchargesLabelsAndVisibility();
                    }
                });
            }
        });        
    }

    public AllPackageTypes: PackageTypeList[];
    private GetAllPackageTypes() {
        var iPackageTypeListService = new PackageTypeListService();

        iPackageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllPackageTypes = myResponse.Result;
                this.SetContainersLabelsAndVisibility();
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.GetAllChargesTypes();
                    if (this.isCopyButtonClicked) {
                        this.isCopyButtonClicked = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            });

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "TariffLinesDeleted") {

                    if (this.EntityPM != null && this.VersionPM != null) {
                        this.LoadTariffLines();
                    }
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
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

    SetStepsLabelsAndVisibility() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PriceSteps)) {
            if (this.EntityPM.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = [] = this.EntityPM.PriceSteps.split(",");
                var count = steps.length;
                if (count == 1) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step1PriceVisibility = true;
                }
                else if (count == 2) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                }
                else if (count == 3) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                }
                else if (count == 4) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                }
                else if (count == 5) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                }
                else if (count == 6) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                }
                else if (count == 7) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                }
                else if (count == 8) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step8PriceLabel = steps[7] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                    this.Step8PriceVisibility = true;
                }
            }
            else {
                this.Step1PriceLabel = this.EntityPM.PriceSteps;
                this.Step1PriceVisibility = true;
            }
        }
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

    public Surcharge1PriceLabel_FCL: string;
    public Surcharge2PriceLabel_FCL: string;
    public Surcharge3PriceLabel_FCL: string;
    public Surcharge4PriceLabel_FCL: string;
    public Surcharge5PriceLabel_FCL: string;
    public Surcharge6PriceLabel_FCL: string;
    public Surcharge7PriceLabel_FCL: string;
    public Surcharge8PriceLabel_FCL: string;
    public Surcharge9PriceLabel_FCL: string;
    public Surcharge10PriceLabel_FCL: string;

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
                var displyText: string = iChargeType.Code;
                var chargeCode: string = iChargeType.Code;
                var isFixed = false;

                var iMeasurement: MeasurementList = this.AllMeasurements.filter(f => f.Id == iMeasurementId)[0];
                if (iMeasurement) {
                    displyText = iChargeType.Code + " (" + iMeasurement.Code + ")";

                    if (iMeasurement.Code == "FIXD") {
                        isFixed = true;
                    }
                }

                this['Surcharge' + index + 'PriceLabel'] = displyText;
                this['Surcharge' + index + 'PriceLabel_FCL'] = chargeCode;
                this['Surcharge' + index + 'PriceVisibility'] = true;
                this['Surcharge' + index + 'MinPriceVisibility'] = !isFixed;
                this['Surcharge' + index + 'MinPriceLabel'] = "Min " + iChargeType.Code;
            }
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

    private versions: TariffVersionPM[] = [];
    private LoadVersions() {
        var service: TariffVersionExtendedPMService = new TariffVersionExtendedPMService();
        service.GetAllTariffVersionsForTariff(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.versions = response.Result;
                this.BuildVersionsList();
            }
        });
    }

    public VersionsList: CodeNameClass[];
    private BuildVersionsList() {
        this.VersionsList = [];

        var datePipe: DatePipe = new DatePipe("en-US");

        this.versions.filter(d => !d.IsDraft).forEach(item => {

            var newVersion: CodeNameClass = new CodeNameClass();
            newVersion.Code_Int = item.Version;

            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC" || this.EntityPM.TypeCode == "IFT") {
                newVersion.Name = "Version " + item.Version;
            }

            else {
                var from: string = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
                var to: string = datePipe.transform(item.ExpirationDate == null ? item.InitialEnddate : item.ExpirationDate, 'dd/MM/yyyy');

                if (!from) {
                    from = "";
                }

                if (!to) {
                    to = "";
                }

                if (!AppTool.IsNullOrEmpty(from) && !AppTool.IsNullOrEmpty(to)) {
                    newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";
                }

                else {
                    newVersion.Name = "Version " + item.Version + " (" + from + to + ")";
                }                
            }

            this.VersionsList.push(newVersion);
        });

        if (this.VersionsList.length > 0) {
            this.SelectedVersion = this.VersionsList[0];
            this.IsActionsEnabled = true;
        }
    }

    private selectedVersion: CodeNameClass;
    get SelectedVersion() { return this.selectedVersion; }
    set SelectedVersion(value: CodeNameClass) {
        if (this.selectedVersion != value) {
            this.selectedVersion = value;

            this.VersionPM = this.versions.filter(d => d.Version == value.Code_Int)[0];
            this.LoadTariffLines();
        }
    }

    private tariffLines: TariffLinePM[];
    private LoadTariffLines() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.VersionPM.Version).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.tariffLines = response.Result;

                this.FillLines();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private FillLines() {
        this.VersionLinesSource.Clear();
        var itemsCollection: VersionHistoryTariffLine[] = [];
        var count = 0; var selectRowIndex = 0; var isSelectRowExist = false;
        this.tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            var itemhistory = new VersionHistoryTariffLine(item, this, this.EntityPM);
            count++;
            itemsCollection.push(itemhistory);
            if (!AppTool.IsNullOrEmpty(this.LineIdFromPriceCheck) && itemhistory.myTariffLine.Id == this.LineIdFromPriceCheck) {
                this.selectedRow = itemhistory;
                selectRowIndex = count;
                isSelectRowExist = true;
            }
        });
        
        this.VersionLinesSource.InsertCollection(itemsCollection);
        if (isSelectRowExist) {
            this.changeScrollPosition.emit({
                RowIndex: selectRowIndex
            });
        }
    }

    // Download Excel 
    DownloadExcelClicked(type: string) {
        this.TariffDomainService.DownloadTariff(this.EntityPM.Id, this.VersionPM.Version, type).subscribe((myResponse: ServiceResponse) => {
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
        if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            var windowTitle = "New Copy Version";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 200;
            var windowArgs: any = {};
            windowArgs.CurrentVersion = this.VersionPM;
            windowArgs.TariffType = this.EntityPM.TypeCode;
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = windowTitle;
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (s && d == "ok") {
                        this.CopyTariffVersion();
                    }
                });
            });

            logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
        }
        else {
            this.CopyTariffVersion();
        }
    }
    CopyTariffVersion() {
        this.isCopyButtonClicked = true;
        this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
        this.EntityPM.LastStartDate = this.VersionPM.StartDate;
        this.EntityPM.LastExpirationDate = this.VersionPM.ExpirationDate;

        var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.VersionPM.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;

        if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            copiedVersion.ExpirationDate = this.VersionPM.ExpirationDate != null ? this.VersionPM.ExpirationDate : this.VersionPM.InitialEnddate;
        }

        copiedVersion.IsDraft = true;
        copiedVersion.StartDate = this.VersionPM.StartDate;
        copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.VersionPM.Version;

        this.EntityPM.AddTariffVersion(copiedVersion);

        this.tariffLines.forEach(item => {
            var tariffLine = new TariffLinePM(copiedVersion);            
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
            tariffLine.ViaPortId = item.ViaPortId;
            tariffLine.ViaPortCode = item.ViaPortCode;
            tariffLine.ViaPortCombinedCode = item.ViaPortCombinedCode;
            tariffLine.ViaPortName = item.ViaPortName;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            tariffLine.TransitTime = item.TransitTime;
            tariffLine.IsFromAllOtherPorts = item.IsFromAllOtherPorts;
            tariffLine.IsToAllOtherPorts = item.IsToAllOtherPorts;
            tariffLine.IsFromAllOtherCountries = item.IsFromAllOtherCountries;
            tariffLine.IsToAllOtherCountries = item.IsToAllOtherCountries;
            tariffLine.FromCountryId = item.FromCountryId;
            tariffLine.FromCountryCode = item.FromCountryCode;
            tariffLine.FromCountryName = item.FromCountryName;
            tariffLine.ToCountryId = item.ToCountryId;
            tariffLine.ToCountryCode = item.ToCountryCode;
            tariffLine.ToCountryName = item.ToCountryName;

            if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC") {
                tariffLine.MinPrice = item.MinPrice;
                tariffLine.Step1Price = item.Step1Price;
                tariffLine.Step2Price = item.Step2Price;
                tariffLine.Step3Price = item.Step3Price;
                tariffLine.Step4Price = item.Step4Price;
                tariffLine.Step5Price = item.Step5Price;
                tariffLine.Step6Price = item.Step6Price;
                tariffLine.Step7Price = item.Step7Price;
                tariffLine.Step8Price = item.Step8Price;
                tariffLine.StartDate = this.VersionPM.StartDate;
                tariffLine.ExpirationDate = this.VersionPM.ExpirationDate;
            }

            else if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
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
                tariffLine.StartDate = item.StartDate;
                tariffLine.CurrencyId = item.CurrencyId;
                tariffLine.CurrencyCode = item.CurrencyCode;
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
            }

            else if (this.EntityPM.TypeCode == "OFC") {
                tariffLine.StartDate = this.VersionPM.StartDate;
                tariffLine.ExpirationDate = this.VersionPM.ExpirationDate;
                tariffLine.Surcharge1Price = item.Surcharge1Price;
                tariffLine.Surcharge2Price = item.Surcharge2Price;
                tariffLine.Surcharge3Price = item.Surcharge3Price;
                tariffLine.Surcharge4Price = item.Surcharge4Price;
                tariffLine.Surcharge5Price = item.Surcharge5Price;
            }

            else if (this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "IFT") {
                tariffLine.StartDate = item.StartDate;
                tariffLine.CurrencyId = item.CurrencyId;
                tariffLine.CurrencyCode = item.CurrencyCode;
                tariffLine.IsDifferentCurrenciesPerCharge = item.IsDifferentCurrenciesPerCharge;

                item.ContainersPrices.forEach(containerItem => {
                    var containerPrice = new TariffLinesContainersPricePM(tariffLine);
                    containerPrice.SurchargeId = containerItem.SurchargeId;
                    containerPrice.Price1 = containerItem.Price1;
                    containerPrice.Price2 = containerItem.Price2;
                    containerPrice.Price3 = containerItem.Price3;
                    containerPrice.Price4 = containerItem.Price4;
                    containerPrice.Price5 = containerItem.Price5;
                    containerPrice.CostPrice = containerItem.CostPrice;
                    containerPrice.CurrencyId = containerItem.CurrencyId;
                    tariffLine.AddTariffLinesContainersPrice(containerPrice);
                });
            }

            copiedVersion.AddTariffLine(tariffLine);
        });

        this.VersionPM.TariffAllInCharges.forEach(item => {
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
    AllInChargesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { TariffPM: this.EntityPM, VersionPM: this.VersionPM, IsEditingEnabled: false };
        logWindow.Title = "All-In Charges";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditAllInChargesComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                
            }
        });
    }

    ViewUploadedExcelFilesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Uploaded Excel Files";
        logWindow.Width = 600;
        logWindow.Height = 500;
        logWindow.WindowArgs = { TariffId: this.EntityPM.Id, Version: this.VersionPM.Version };
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UploadedExcelsComponent');
    }
}

export class VersionHistoryTariffLine {
    public OriginPortCode: string;
    public DestinationPortCode: string;
    public OriginPortCombinedCode: string;
    public DestinationPortCombinedCode: string;
    public ViaPortCode: string;
    public ViaPortCombinedCode: string;
    public Notes: string;
    public ExpirationDate: Date;
    public IsFromAllOtherPorts: boolean;
    public IsToAllOtherPorts: boolean;
    public CurrencyCode: string;
    public StartDate: Date;
    public TransitTime: string;
    public IsDifferentCurrenciesPerCharge: boolean;
    public FromCountryCode: string;
    public ToCountryCode: string;

    //AFC || OLC
    public MinPrice: number;
    public Step1Price: number;
    public Step2Price: number;
    public Step3Price: number;
    public Step4Price: number;
    public Step5Price: number;
    public Step6Price: number;
    public Step7Price: number;
    public Step8Price: number;

    //ASC || OSC || OFC
    public Surcharge1MinPrice: number;
    public Surcharge2MinPrice: number;
    public Surcharge3MinPrice: number;
    public Surcharge4MinPrice: number;
    public Surcharge5MinPrice: number;
    public Surcharge6MinPrice: number;
    public Surcharge7MinPrice: number;
    public Surcharge8MinPrice: number;
    public Surcharge9MinPrice: number;
    public Surcharge10MinPrice: number;
    public Surcharge1Price: number;
    public Surcharge2Price: number;
    public Surcharge3Price: number;
    public Surcharge4Price: number;
    public Surcharge5Price: number;
    public Surcharge6Price: number;
    public Surcharge7Price: number;
    public Surcharge8Price: number;
    public Surcharge9Price: number;
    public Surcharge10Price: number;

    //OFS
    public Surcharge1PriceValue: string;
    public Surcharge2PriceValue: string;
    public Surcharge3PriceValue: string;
    public Surcharge4PriceValue: string;
    public Surcharge5PriceValue: string;
    public Surcharge6PriceValue: string;
    public Surcharge7PriceValue: string;
    public Surcharge8PriceValue: string;
    public Surcharge9PriceValue: string;
    public Surcharge10PriceValue: string;

    public myTariffLine: TariffLinePM;
    private myTariff: TariffPM;
    private fatherComponent: VersionHistoryTabComponent;
    constructor(tariffLine: TariffLinePM, public FatherComponent: VersionHistoryTabComponent, tariff: TariffPM) {
        this.myTariff = tariff;
        this.fatherComponent = FatherComponent;
        this.myTariffLine = tariffLine;
        this.AssignCommonData();
        this.SetCellColorsForPriceCheck();
        if (tariff.TypeCode == "AFC" || tariff.TypeCode == "OLC") {
            this.AssignData_FreightCost();
        }

        else if (tariff.TypeCode == "OFC" || tariff.TypeCode == "ASC" || tariff.TypeCode == "OSC" || tariff.TypeCode == "ICC" || tariff.TypeCode == "ECC") {
            this.AssignData_AIRLCLSurchargeCost();
        }

        else if (tariff.TypeCode == "OFS" || tariff.TypeCode == "IFT") {
            this.AssignData_OceanFCLSurchargeCost();
        }
    }
    public CellColor:string = "transparent";
    private SetCellColorsForPriceCheck() {
        if (!AppTool.IsNullOrEmpty(this.fatherComponent.LineIdFromPriceCheck) && this.fatherComponent.LineIdFromPriceCheck == this.myTariffLine.Id) {
            this.CellColor = "#f7dc6e";
        }

        else {
                this.CellColor = "rgba(230, 231, 232, 0.5)";
        }
    }
    private AssignCommonData() {
        this.OriginPortCode = this.myTariffLine.OriginPortCode;
        this.DestinationPortCode = this.myTariffLine.DestinationPortCode;
        this.ViaPortCode = this.myTariffLine.ViaPortCode;
        this.OriginPortCombinedCode = this.myTariffLine.OriginPortCombinedCode;
        this.DestinationPortCombinedCode = this.myTariffLine.DestinationPortCombinedCode;
        this.ViaPortCombinedCode = this.myTariffLine.ViaPortCombinedCode;
        this.Notes = this.myTariffLine.Notes;
        this.ExpirationDate = this.myTariffLine.ExpirationDate;
        this.IsFromAllOtherPorts = this.myTariffLine.IsFromAllOtherPorts;
        this.IsToAllOtherPorts = this.myTariffLine.IsToAllOtherPorts;
        this.CurrencyCode = this.myTariffLine.CurrencyCode;
        this.StartDate = this.myTariffLine.StartDate;
        this.TransitTime = this.myTariffLine.TransitTime;
        this.IsDifferentCurrenciesPerCharge = this.myTariffLine.IsDifferentCurrenciesPerCharge;
        this.FromCountryCode = this.myTariffLine.FromCountryCode;
        this.ToCountryCode = this.myTariffLine.ToCountryCode;
    }

    private AssignData_FreightCost() {
        this.MinPrice = this.myTariffLine.MinPrice;
        this.Step1Price = this.myTariffLine.Step1Price;
        this.Step2Price = this.myTariffLine.Step2Price;
        this.Step3Price = this.myTariffLine.Step3Price;
        this.Step4Price = this.myTariffLine.Step4Price;
        this.Step5Price = this.myTariffLine.Step5Price;
        this.Step6Price = this.myTariffLine.Step6Price;
        this.Step7Price = this.myTariffLine.Step7Price;
        this.Step8Price = this.myTariffLine.Step8Price;
    }

    private AssignData_AIRLCLSurchargeCost() {
        this.Surcharge1MinPrice = this.myTariffLine.Surcharge1MinPrice;
        this.Surcharge2MinPrice = this.myTariffLine.Surcharge2MinPrice;
        this.Surcharge3MinPrice = this.myTariffLine.Surcharge3MinPrice;
        this.Surcharge4MinPrice = this.myTariffLine.Surcharge4MinPrice;
        this.Surcharge5MinPrice = this.myTariffLine.Surcharge5MinPrice;
        this.Surcharge6MinPrice = this.myTariffLine.Surcharge6MinPrice;
        this.Surcharge7MinPrice = this.myTariffLine.Surcharge7MinPrice;
        this.Surcharge8MinPrice = this.myTariffLine.Surcharge8MinPrice;
        this.Surcharge9MinPrice = this.myTariffLine.Surcharge9MinPrice;
        this.Surcharge10MinPrice = this.myTariffLine.Surcharge10MinPrice;

        this.Surcharge1Price = this.myTariffLine.Surcharge1Price;
        this.Surcharge2Price = this.myTariffLine.Surcharge2Price;
        this.Surcharge3Price = this.myTariffLine.Surcharge3Price;
        this.Surcharge4Price = this.myTariffLine.Surcharge4Price;
        this.Surcharge5Price = this.myTariffLine.Surcharge5Price;
        this.Surcharge6Price = this.myTariffLine.Surcharge6Price;
        this.Surcharge7Price = this.myTariffLine.Surcharge7Price;
        this.Surcharge8Price = this.myTariffLine.Surcharge8Price;
        this.Surcharge9Price = this.myTariffLine.Surcharge9Price;
        this.Surcharge10Price = this.myTariffLine.Surcharge10Price;
    }

    private AssignData_OceanFCLSurchargeCost() {
        this.Surcharge1PriceValue = this.ComputePriceValue(this.myTariff.Surcharge1Id);
        this.Surcharge2PriceValue = this.ComputePriceValue(this.myTariff.Surcharge2Id);
        this.Surcharge3PriceValue = this.ComputePriceValue(this.myTariff.Surcharge3Id);
        this.Surcharge4PriceValue = this.ComputePriceValue(this.myTariff.Surcharge4Id);
        this.Surcharge5PriceValue = this.ComputePriceValue(this.myTariff.Surcharge5Id);
        this.Surcharge6PriceValue = this.ComputePriceValue(this.myTariff.Surcharge6Id);
        this.Surcharge7PriceValue = this.ComputePriceValue(this.myTariff.Surcharge7Id);
        this.Surcharge8PriceValue = this.ComputePriceValue(this.myTariff.Surcharge8Id);
        this.Surcharge9PriceValue = this.ComputePriceValue(this.myTariff.Surcharge9Id);
        this.Surcharge10PriceValue = this.ComputePriceValue(this.myTariff.Surcharge10Id);
    }
    private ComputePriceValue(ichargeTypeId): string {
        var myValue: string = "";

        if (!AppTool.IsNullOrEmpty(ichargeTypeId)) {
            if (this.myTariffLine.ContainersPrices.filter(d => d.SurchargeId == ichargeTypeId).length > 0) {
                this.myTariffLine.ContainersPrices.filter(d => d.SurchargeId == ichargeTypeId).forEach((item) => {

                    if (!AppTool.IsNullOrZero(item.CostPrice)) {
                        myValue = item.CostPrice.toString();
                    }

                    else {

                        if (!AppTool.IsNullOrEmpty(this.myTariff.ContainerType1Id)) {
                            if (AppTool.IsNullOrZero(item.Price1)) {
                                myValue = "-";
                            }

                            else {
                                myValue = item.Price1.toString();
                            }
                        }

                        if (!AppTool.IsNullOrEmpty(this.myTariff.ContainerType2Id)) {
                            if (AppTool.IsNullOrZero(item.Price2)) {
                                myValue = myValue + " / -";
                            }

                            else {
                                myValue = myValue + " / " + item.Price2.toString();
                            }
                        }

                        if (!AppTool.IsNullOrEmpty(this.myTariff.ContainerType3Id)) {
                            if (AppTool.IsNullOrZero(item.Price3)) {
                                myValue = myValue + " / -";
                            }

                            else {
                                myValue = myValue + " / " + item.Price3.toString();
                            }
                        }

                        if (!AppTool.IsNullOrEmpty(this.myTariff.ContainerType4Id)) {
                            if (AppTool.IsNullOrZero(item.Price4)) {
                                myValue = myValue + " / -";
                            }

                            else {
                                myValue = myValue + " / " + item.Price4.toString();
                            }
                        }

                        if (!AppTool.IsNullOrEmpty(this.myTariff.ContainerType5Id)) {
                            if (AppTool.IsNullOrZero(item.Price5)) {
                                myValue = myValue + " / -";
                            }

                            else {
                                myValue = myValue + " / " + item.Price5.toString();
                            }
                        }
                    }
                });
            }
        }

        return myValue;
    }
}
