import { Component, OnDestroy } from '@angular/core';
import { DatePipe } from '@angular/common';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
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
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { TariffVersionExtendedPMService } from '../../../Services/ExtendedPMs/TariffVersionExtendedPMService';

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
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.VersionLinesSource = new ObservableCollection([]);
        this.TariffDomainService = new TariffDomainService();

        if (this.EntityPM.TypeCode == "AFC") {
            this.SetStepsLabelsAndVisibility();
        }

        else if (this.EntityPM.TypeCode == "ASC") {
            this.GetAllChargesTypes();
        }
        
        this.LoadVersions();
        this.Listen();
    }

    private AllChargesTypes: ChargesTypeList[];
    private GetAllChargesTypes() {
        var chargesTypeListService = new ChargesTypeListService();
        chargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;
                if (this.AllChargesTypes != null) {
                    this.AllChargesTypes = this.AllChargesTypes.filter(d => d.InActive == false);
                    this.SetSurchargesLabelsAndVisibility();
                }
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isCopyButtonClicked) {
                        this.isCopyButtonClicked = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
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

    SetSurchargesLabelsAndVisibility() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge1Id)[0];
            this.Surcharge1PriceLabel = chargeType.Code;
            this.Surcharge1PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge2Id)[0];
            this.Surcharge2PriceLabel = chargeType.Code;
            this.Surcharge2PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge3Id)[0];
            this.Surcharge3PriceLabel = chargeType.Code;
            this.Surcharge3PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge4Id)[0];
            this.Surcharge4PriceLabel = chargeType.Code;
            this.Surcharge4PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge5Id)[0];
            this.Surcharge5PriceLabel = chargeType.Code;
            this.Surcharge5PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge6Id)[0];
            this.Surcharge6PriceLabel = chargeType.Code;
            this.Surcharge6PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge7Id)[0];
            this.Surcharge7PriceLabel = chargeType.Code;
            this.Surcharge7PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge8Id)[0];
            this.Surcharge8PriceLabel = chargeType.Code;
            this.Surcharge8PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge9Id)[0];
            this.Surcharge9PriceLabel = chargeType.Code;
            this.Surcharge9PriceVisibility = true;
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10Id)) {
            var chargeType = this.AllChargesTypes.filter(a => a.Id == this.EntityPM.Surcharge10Id)[0];
            this.Surcharge10PriceLabel = chargeType.Code;
            this.Surcharge10PriceVisibility = true;
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
            var from: string = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
            var to: string = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');

            var newVersion: CodeNameClass = new CodeNameClass();
            newVersion.Code_Int = item.Version;
            newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";

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

        this.tariffLines.forEach(item => {
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
                var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "Tariffs" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
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
        this.isCopyButtonClicked = true;

        this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
        this.EntityPM.LastStartDate = this.VersionPM.StartDate;
        this.EntityPM.LastExpirationDate = this.VersionPM.ExpirationDate;

        var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.VersionPM.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
        copiedVersion.ExpirationDate = this.VersionPM.ExpirationDate;
        copiedVersion.IsDraft = true;
        copiedVersion.StartDate = this.VersionPM.StartDate;
        copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.VersionPM.Version;

        this.EntityPM.AddTariffVersion(copiedVersion);

        this.tariffLines.forEach(item => {
            var tariffLine = new TariffLinePM(copiedVersion);
            tariffLine.StartDate = this.VersionPM.StartDate;
            tariffLine.ExpirationDate = this.VersionPM.ExpirationDate;
            tariffLine.Tenant = SessionLocator.Tenant;
            tariffLine.Version = copiedVersion.Version;
            tariffLine.OriginPortId = item.OriginPortId;
            tariffLine.OriginPortCode = item.OriginPortCode;
            tariffLine.OriginPortName = item.OriginPortName;
            tariffLine.DestinationPortId = item.DestinationPortId;
            tariffLine.DestinationPortCode = item.DestinationPortCode;
            tariffLine.DestinationPortName = item.DestinationPortName;

            if (this.EntityPM.TypeCode == "AFC") {
                tariffLine.MinPrice = item.MinPrice;
                tariffLine.Step1Price = item.Step1Price;
                tariffLine.Step2Price = item.Step2Price;
                tariffLine.Step3Price = item.Step3Price;
                tariffLine.Step4Price = item.Step4Price;
                tariffLine.Step5Price = item.Step5Price;
                tariffLine.Step6Price = item.Step6Price;
                tariffLine.Step7Price = item.Step7Price;
                tariffLine.Step8Price = item.Step8Price;
            }

            else if (this.EntityPM.TypeCode == "ASC") {
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
            }

            copiedVersion.AddTariffLine(tariffLine);
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
    }
}
