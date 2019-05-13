import { Component, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { PortPM } from '../../../../Common/EntityPMs/PortPM';
import { TariffDomainService, TariffFilterParameter, ExcelTariffLines } from '../../../../TariffModule/Services/TariffDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { TariffLinePM } from '../../../../TariffModule/EntityPMs/TariffLinePM';
import { TariffVersionPM } from '../../../../TariffModule/EntityPMs/TariffVersionPM';
import { AppTool, FontTool, DateTool, FormatTool } from '../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityArgs = entityArgs;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.FillTariffLines();

                    if (this.isApproveButtonClicked) {
                        this.DoApprove();
                    }

                    if (this.isCopyButtonClicked) {
                        this.isCopyButtonClicked = false;
                        this.CurrentSession.FireEvent("NewVersionAdded");
                    }

                    if (this.isUploadExcelFinished) {
                        this.isUploadExcelFinished = false;
                        this.FillTariffLines();
                    }
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if (this.CurrentVersion != null) {
                        this.IsDraftVersion = this.CurrentVersion.IsDraft;
                    }

                    if (this.isApproveButtonClicked) {
                        this.isApproveButtonClicked = false;
                        this.CurrentSession.FireEvent("VersionApproved");
                    }

                    this.SetUIProperties();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    Intialize(args: any) {
        this.TariffsLinesSource = new ObservableCollection([]);
        this.EntityPM = this.EntityArgs.EntityPM;
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService();
        this.TariffDomainService = new TariffDomainService();

        this.CurrentVersion = args['CurrentVersion'];

        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
        }

        this.SetUIProperties();

        this.FillTariffLines();
    }

    SetUIProperties() {
        var isApproveVersionButtonVisible: boolean = false;

        if (this.IsDraftVersion && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
            isApproveVersionButtonVisible = true;
        }

        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
    }

    get StartDate() {
        return this.CurrentVersion.StartDate;
    }
    set StartDate(value: Date) {
        if (this.CurrentVersion.StartDate != value) {
            this.CurrentVersion.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.CurrentVersion.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.CurrentVersion.ExpirationDate != value) {
            this.CurrentVersion.ExpirationDate = value;
        }
    }

    FillTariffLines() {
        this.TariffsLinesSource.Clear();
        var itemsCollection: TariffLineData[] = [];

        this.CurrentVersion.TariffLines.forEach(item => {
            itemsCollection.push(new TariffLineData(item, this));
        });

        this.TariffsLinesSource.InsertCollection(itemsCollection);
    }

    AddTariffLine() {
        var logWindow = new LogitudeWindow();
        var itemPM = new TariffLinePM(null);
        itemPM.StartDate = this.StartDate;
        itemPM.ExpirationDate = this.ExpirationDate;
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.Version = this.CurrentVersion.Version;
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
                this.FillTariffLines();
            }
        });
    }

    // Upload Excel File 
    OnFileChanged(event) {
        var file = event.target.files[0];
        this.UploadExcel(file);
    }
    UploadExcel(file: any) {
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

            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
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
            tariffLine.MinPrice = item.MinPrice;
            tariffLine.Step1Price = item.Step1Price;
            tariffLine.Step2Price = item.Step2Price;
            tariffLine.Step3Price = item.Step3Price;
            tariffLine.Step4Price = item.Step4Price;
            tariffLine.Step5Price = item.Step5Price;
            tariffLine.Step6Price = item.Step6Price;
            tariffLine.Step7Price = item.Step7Price;
            tariffLine.Step8Price = item.Step8Price;

            tariffLine.OriginPortText = item.FromPortText;
            tariffLine.DestinationPortText = item.ToPortText;
            tariffLine.MinPriceText = item.MinPriceText;
            tariffLine.Step1PriceText = item.Step1PriceText;
            tariffLine.Step2PriceText = item.Step2PriceText;
            tariffLine.Step3PriceText = item.Step3PriceText;
            tariffLine.Step4PriceText = item.Step4PriceText;
            tariffLine.Step5PriceText = item.Step5PriceText;
            tariffLine.Step6PriceText = item.Step6PriceText;
            tariffLine.Step7PriceText = item.Step7PriceText;
            tariffLine.Step8PriceText = item.Step8PriceText;

            this.CurrentVersion.AddTariffLine(tariffLine);
        });

        this.EntityPM.TariffLinesAdded = true;
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
        this.isApproveButtonClicked = true;

        if (this.EntityPM.IsDirty) {
            this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
        }

        else {
            this.DoApprove();
        }
    }
    private DoApprove() {
        this.TariffDomainService.ApproveVersion(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

        var copiedVersion: TariffVersionPM = new TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.CurrentVersion.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
        copiedVersion.ExpirationDate = this.CurrentVersion.ExpirationDate;
        copiedVersion.IsDraft = true;
        copiedVersion.StartDate = this.CurrentVersion.StartDate;
        copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.CurrentVersion.Version;

        this.EntityPM.AddTariffVersion(copiedVersion);

        this.CurrentVersion.TariffLines.forEach(item => {
            var tariffLine = new TariffLinePM(copiedVersion);
            tariffLine.StartDate = this.StartDate;
            tariffLine.ExpirationDate = this.ExpirationDate;
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

            this.CurrentVersion.AddTariffLine(tariffLine);
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
    }
}

export class TariffLineData extends BaseComponent {
    public EntityPM: TariffLinePM;
    public DataContext: TariffLineData = this;
    private ObjectTableName = "TariffLine";
    public IsNewEntity: boolean = false;

    constructor(entity: TariffLinePM, public FatherComponent: SurchargeVersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.SetUIProperties();

        this.CheckIfLineHasError();
    }

    public HasError: boolean = false;
    private CheckIfLineHasError() {
        var error: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            error = true;
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.MinPriceText) && AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                error = true;
            }
        }

        if (!error) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Step8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                error = true;
            }
        }

        this.HasError = error;
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
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

  
}

