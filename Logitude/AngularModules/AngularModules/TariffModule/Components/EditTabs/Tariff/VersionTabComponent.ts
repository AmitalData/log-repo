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
    templateUrl: './VersionTabComponent.html',
})

export class VersionTabComponent extends BaseComponent implements OnDestroy {
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
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    if (this.isApproveButtonClicked) {
                        this.isApproveButtonClicked = false;
                        this.DoApprove();
                    }

                    if (this.isCopyButtonClicked) {
                        this.isCopyButtonClicked = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

                    if (this.isUploadExcelFinished) {
                        this.isUploadExcelFinished = false;
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            });
        }
    }
    
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
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
        this.SetStepsLabelsAndVisibility();
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

    get PriceSteps() {
        return this.EntityPM.PriceSteps;
    }
    set PriceSteps(value: string) {
        if (this.EntityPM.PriceSteps != value) {
            this.EntityPM.PriceSteps = value;
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
        if (!AppTool.IsNullOrEmpty(this.PriceSteps)) {
            if (this.PriceSteps.indexOf(',') > -1) {
                var steps: string[] = [] = this.PriceSteps.split(",");
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
                this.Step1PriceLabel = this.PriceSteps;
                this.Step1PriceVisibility = true;
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
            filter.TariffType = context.EntityPM.TypeCode;

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

            tariffLine.HasErrors = item.HasErrors;
            tariffLine.ErrorText = item.ErrorText;

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
        var errors: string[] = [];

        if (this.CurrentVersion.TariffLines.filter(d => d.HasErrors).length > 0) {
            errors.push("Invalid Tariff Lines");
        }

        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.isApproveButtonClicked = true;

            if (this.EntityPM.IsDirty) {
                this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
            }

            else {
                this.DoApprove();
            }
        }
    }
    private DoApprove() {
        this.TariffDomainService.ApproveVersion(this.EntityPM.Id, this.CurrentVersion.Version).subscribe((response: ServiceResponse) => {
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
    public IsEditEnabled: boolean = false;
    constructor(entity: TariffLinePM, public FatherComponent: VersionTabComponent, isNew: boolean = false) {
        super();
        this.EntityPM = entity;
        this.IsNewEntity = isNew;
        this.IsEditEnabled = FatherComponent.IsDraftVersion;

        this.SetUIProperties();
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

        if (!AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
            }

            else {
                errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found"
            }            
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.MinPriceText) && AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Min price format is invalid";
            }

            else {
                errorText = errorText + ", Min price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step1PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 1 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 1 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step2PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 2 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 2 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step3PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 3 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 3 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step4PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 4 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 4 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step5PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 5 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 5 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step6PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 6 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 6 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step7PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 7 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 7 price format is invalid"
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.Step8PriceText) && AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
            error = true;

            if (AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Step 8 price format is invalid";
            }

            else {
                errorText = errorText + ", Step 8 price format is invalid"
            }
        }

        this.HasErrors = error;
        this.ErrorText = errorText;
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

    // Min Price
    get MinPrice() {
        return this.EntityPM.MinPrice;
    }
    set MinPrice(value: number) {
        if (this.EntityPM.MinPrice != value) {
            this.EntityPM.MinPrice = value;
            this.CheckIfLineHasError();
        }
    }

    get MinPriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
            return FormatTool.FormatNumber(this.EntityPM.MinPrice, "N3");
        }

        else {
            return this.EntityPM.MinPriceText;
        }
    }

    get MinPriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 1
    get Step1Price() {
        return this.EntityPM.Step1Price;
    }
    set Step1Price(value: number) {
        if (this.EntityPM.Step1Price != value) {
            this.EntityPM.Step1Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step1PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step1Price, "N3");
        }

        else {
            return this.EntityPM.Step1PriceText;
        }
    }

    get Step1PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 2
    get Step2Price() {
        return this.EntityPM.Step2Price;
    }
    set Step2Price(value: number) {
        if (this.EntityPM.Step2Price != value) {
            this.EntityPM.Step2Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step2PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step2Price, "N3");
        }

        else {
            return this.EntityPM.Step2PriceText;
        }
    }

    get Step2PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 3
    get Step3Price() {
        return this.EntityPM.Step3Price;
    }
    set Step3Price(value: number) {
        if (this.EntityPM.Step3Price != value) {
            this.EntityPM.Step3Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step3PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step3Price, "N3");
        }

        else {
            return this.EntityPM.Step3PriceText;
        }
    }

    get Step3PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 4
    get Step4Price() {
        return this.EntityPM.Step4Price;
    }
    set Step4Price(value: number) {
        if (this.EntityPM.Step4Price != value) {
            this.EntityPM.Step4Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step4PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step4Price, "N3");
        }

        else {
            return this.EntityPM.Step4PriceText;
        }
    }

    get Step4PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 5
    get Step5Price() {
        return this.EntityPM.Step5Price;
    }
    set Step5Price(value: number) {
        if (this.EntityPM.Step5Price != value) {
            this.EntityPM.Step5Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step5PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step5Price, "N3");
        }

        else {
            return this.EntityPM.Step5PriceText;
        }
    }

    get Step5PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 6
    get Step6Price() {
        return this.EntityPM.Step6Price;
    }
    set Step6Price(value: number) {
        if (this.EntityPM.Step6Price != value) {
            this.EntityPM.Step6Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step6PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step6Price, "N3");
        }

        else {
            return this.EntityPM.Step6PriceText;
        }
    }

    get Step6PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 7
    get Step7Price() {
        return this.EntityPM.Step7Price;
    }
    set Step7Price(value: number) {
        if (this.EntityPM.Step7Price != value) {
            this.EntityPM.Step7Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step7PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step7Price, "N3");
        }

        else {
            return this.EntityPM.Step7PriceText;
        }
    }

    get Step7PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }

    // Step 8
    get Step8Price() {
        return this.EntityPM.Step8Price;
    }
    set Step8Price(value: number) {
        if (this.EntityPM.Step8Price != value) {
            this.EntityPM.Step8Price = value;
            this.CheckIfLineHasError();
        }
    }

    get Step8PriceValue() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
            return FormatTool.FormatNumber(this.EntityPM.Step8Price, "N3");
        }

        else {
            return this.EntityPM.Step8PriceText;
        }
    }

    get Step8PriceColor() {
        if (!AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
            return FontTool.Black;
        }

        else {
            return FontTool.Red;
        }
    }
}

