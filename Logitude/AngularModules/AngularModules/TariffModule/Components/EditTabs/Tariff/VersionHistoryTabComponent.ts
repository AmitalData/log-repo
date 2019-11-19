import { Component, OnDestroy } from '@angular/core';
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

@Component({
    moduleId: module.id,
    templateUrl: './VersionHistoryTabComponent.html',
})

export class VersionHistoryTabComponent implements OnDestroy {
    public EntityPM: TariffPM;
    public VersionPM: TariffVersionPM;
    public VersionLinesSource: ObservableCollection;
    private TariffDomainService: TariffDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsActionsEnabled: boolean = false;
    public IsDownloadExcelTemplateVisible: boolean = false;
    public IsAllInChargesVisible: boolean = false;
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

        else if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC") {
            this.IsDownloadExcelTemplateVisible = false;
            this.GetAllChargesTypes();
        }
        
        this.LoadVersions();
        this.Listen();
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
                var isFixed = false;

                var iMeasurement: MeasurementList = this.AllMeasurements.filter(f => f.Id == iMeasurementId)[0];
                if (iMeasurement) {
                    displyText = iChargeType.Code + " (" + iMeasurement.Code + ")";

                    if (iMeasurement.Code == "FIXD") {
                        isFixed = true;
                    }
                }

                this['Surcharge' + index + 'PriceLabel'] = displyText;
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

            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC") {
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
        var itemsCollection: TariffLinePM[] = [];

        this.tariffLines.sort((a, b) => a.Index - b.Index).forEach(item => {
            itemsCollection.push(item);
        });

        this.VersionLinesSource.InsertCollection(itemsCollection);
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
            tariffLine.OriginPortName = item.OriginPortName;
            tariffLine.DestinationPortId = item.DestinationPortId;
            tariffLine.DestinationPortCode = item.DestinationPortCode;
            tariffLine.DestinationPortName = item.DestinationPortName;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            tariffLine.IsFromAllOtherPorts = item.IsFromAllOtherPorts;
            tariffLine.IsToAllOtherPorts = item.IsToAllOtherPorts;

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

            else if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC") {
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
}
