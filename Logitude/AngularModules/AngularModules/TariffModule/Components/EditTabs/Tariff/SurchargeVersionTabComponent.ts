import { Component, OnDestroy, OnInit } from '@angular/core';
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
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';

declare var ResultAsArray: any;

@Component({
    moduleId: module.id,
    templateUrl: './SurchargeVersionTabComponent.html',
})

export class SurchargeVersionTabComponent extends BaseComponent implements OnDestroy, OnInit {
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
    private ChargesTypeListService: ChargesTypeListService;

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
    ngOnInit() {
       
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
        this.SetSurchargesLabelsAndVisibility();
        this.FillTariffLines();
    }

    SetUIProperties() {
        var isApproveVersionButtonVisible: boolean = false;

        if (this.IsDraftVersion && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
            isApproveVersionButtonVisible = true;
        }

        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
    }

    public AllChargesTypes: ChargesTypeList[]; 
    public GetAllChargesTypes() {
        this.ChargesTypeListService = new ChargesTypeListService();
        this.ChargesTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllChargesTypes = myResponse.Result;
                if (this.AllChargesTypes != null) {
                    this.AllChargesTypes = this.AllChargesTypes.filter(d=> d.InActive == false);
                }
            }
        });
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
        return this.EntityPM.Surcharge8Price;
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
        return this.EntityPM.Surcharge8Price;
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

    get Surcharge10PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }


}

