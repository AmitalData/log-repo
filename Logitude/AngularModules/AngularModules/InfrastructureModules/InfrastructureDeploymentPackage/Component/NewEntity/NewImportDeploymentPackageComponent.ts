import { Component } from '@angular/core';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ImageParameter } from '../../../../Infrastructure/DataContracts/ImageParameter';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeploymentPackageExecutionLogList } from '../../../../Infrastructure/EntityLists/DeploymentPackageExecutionLogList';
import { DeploymentPackageDetails } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackageExecutionLogListExtendedService } from '../../../../Infrastructure/Services/ExtendedLists/DeploymentPackageExecutionLogListExtendedService';
import { DeploymentPackageExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DeploymentPackageExtendedPMService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { MetadataService } from '../../ExternalService/MetadataService';
import { AddNewDeploymentPackageComponent } from './AddNewDeploymentPackageComponent';
declare var attachmentUploader, ResultAsArray: any;


@Component({

    templateUrl: './NewImportDeploymentPackageComponent.html',
})

export class NewImportDeploymentPackageComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public ValidationErrorsList: string[] = [];
    public DataContext: NewImportDeploymentPackageComponent = this;
    public ObjectTableName: string = "DeploymentPackage";
    private CurrentSession = SessionLocator.SelectedSession;
    public UploadedSuccessfully: boolean = false;
    public UploadFileId: string;
    public FileName: string;
    public FileSize: string;
    public FileExtension: string;
    public IsUploadCanceled: boolean;
    public IsUploadInProgress: boolean;
    public IsShowProgressBar: boolean;
    public IsUploadVisibile: boolean = true;
    public ProgressBarId: string;
    public ProgressBarPercentText: string;
    public IsNextClicked: boolean = false;
    private deploymentPackageExtendedPMService: DeploymentPackageExtendedPMService = new DeploymentPackageExtendedPMService();
    private imageLibraryService: ImageLibraryService = new ImageLibraryService();
    private documentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
    File: any;
    filterImageParameter: ImageParameter;
    public FileData: number;

    private uploadedDocumentId: string;
    public DeploymentPackageDetailsCollection = new ObservableCollection([]);
    private deploymentPackageDetailsList: Array<DeploymentPackageDetailsList>;
    public AddNewDeploymentPackageComponent: AddNewDeploymentPackageComponent;
    private deploymentPackageExecutionLogListExtendedService: DeploymentPackageExecutionLogListExtendedService;
    public HasErrorsWhileImporting: boolean = false;
    public ExceptionsCollection = new ObservableCollection([]);
    private exceptionsList: Array<string> = [];
    private deploymentValidationMessage: string = "";
    public IsValidZipFile: boolean = false;
    private metadataService: MetadataService = new MetadataService();
    constructor() {
        super();
        this.UploadFileId = Guid.NewRandomString();
        this.deploymentPackageDetailsList = [];
        this.EntityPM = new DeploymentPackagePM();
        this.UIProperties.SetRequired("Code", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Name", this.ObjectTableName, true);
    }

    Run() {
        this.ClearScreenFields();
        this.SetDefaultFields();
    }

    private ClearScreenFields() {
        this.Code = "";
        this.Name = "";
        this.Description = "";
        this.ValidationErrorsList = [];
        this.ProgressBarId = Guid.newGuid();
    }
    private SetDefaultFields() {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.DirectionId = "I";
    }
    public get Code() { return this.EntityPM.Code }
    public set Code(value: string) {
        if (this.EntityPM.Code == value) return;
        this.EntityPM.Code = value;
    }

    public get Name() { return this.EntityPM.Name }
    public set Name(value: string) {
        if (this.EntityPM.Name == value) return;
        this.EntityPM.Name = value;
        this.Code = this.Code = AppTool.Replace(value?.toLowerCase(), " ", "_");
    }

    public get Description() { return this.EntityPM.Description }
    public set Description(value: string) {
        if (this.EntityPM.Description == value) return;
        this.EntityPM.Description = value;
    }

    public ValidateDeploymentPackage() {

        let errors = [];

        if (!this.EntityPM.Code)
            errors.push("Code Field is Required");

        if (!this.EntityPM.Name)
            errors.push("Name Field is Required");

        if (this.EntityPM.Code && this.EntityPM.Code.length > 100)
            errors.push("Maximum length of Code Field is 100");

        if (this.EntityPM.Name && this.EntityPM.Name.length > 100)
            errors.push("Maximum length of Name Field is 100");

        this.ValidationErrorsList = errors;

        this.ValidateDeploymentPackageCode();
    }
    private ValidateDeploymentPackageCode() {
        this.CurrentSession.StartBusyIndicator("Validating ...");
        this.deploymentPackageExtendedPMService.ValidateDeploymentPackageCode(this.EntityPM.Code).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError && response.ErrorsArray.length > 0) {
                this.ValidationErrorsList.push(response.ErrorsArray[0]);
                return;
            }
            if (this.ValidationErrorsList.length == 0) {
                this.NextButtonClickedWithoutValidationErrors();
            }
            
        });
    }

    public ShowMessage(message: string) {

        const messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);       
    }

    public BrowseButtonClicked() {
        
        document.getElementById(this.UploadFileId).click();
    }

    UploadFile(event: any) {

        this.IsUploadVisibile = false;
        let uploadedFile: any = attachmentUploader(this.UploadFileId);
        if (!uploadedFile || uploadedFile.size <= 0) return;
        this.FileName = uploadedFile.name;
        this.FileExtension = this.GetFileExtension(uploadedFile);

        this.documentsFilingExtendedPMService.GetFileSizeAndUnit(uploadedFile.size).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.FileSize = pmResponse.Result;
            }
            if (this.FileExtension && this.FileExtension.length > 3) {
                this.ShowMessage("File extension should be 3 characters");
                return;
            }
            this.SetFilterImageParameter(uploadedFile);
        });

    }


    private GetFileExtension(uploadedFile: any) {
        let fileInfo = uploadedFile.name.split('.');
        if (fileInfo.length > 1) {
            return fileInfo[fileInfo.length - 1];
        }
        return fileInfo[1];
    }

    private SetFilterImageParameter(uploadedFile: any) {
        this.IsShowProgressBar = true;
        this.IsUploadInProgress = true;
        this.filterImageParameter = new ImageParameter();
        this.filterImageParameter.IsFirstTry = true;
        this.filterImageParameter.Tenant = SessionLocator.Tenant;
        this.filterImageParameter.Extension = this.FileExtension;
        this.filterImageParameter.UploadMode = "AttachmentUploader";
        this.filterImageParameter.FileLocation = "others";
        this.filterImageParameter.ForceCreateDocument = true;
        this.filterImageParameter.FileSize = uploadedFile.size;
        this.filterImageParameter.SendPartNumber = 1;
        this.filterImageParameter.BufferNumber = -1;
        this.filterImageParameter.SentSize = 0;
        this.filterImageParameter.IsFirstTry = true;
        this.filterImageParameter.FileName = this.FileName;

        var filebuffer = this.GetFileBuffer(uploadedFile);
        this.ArrayBufferToBase64(filebuffer, this);
    }

    private GetFileBuffer(uploadedFile: any) {
        this.File = uploadedFile;
        var ChunkSize = 100000;
        if (this.File.size > 2000000) {
            ChunkSize = 1000000;
        }
        this.filterImageParameter.PartsNumber = this.File.size / ChunkSize;

        if (this.filterImageParameter.PartsNumber > 1) {
            return this.File.slice(0, ChunkSize);
        }
        return this.File.slice(0, uploadedFile.size);
    }

    ArrayBufferToBase64(file: any, viewmodel: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            let binary = viewmodel.GetBinaryFile(e);
            viewmodel.filterImageParameter.Base64String = window.btoa(binary);
            viewmodel.SendBlockToServer(viewmodel.filterImageParameter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);

    }

    GetBinaryFile(e: ProgressEvent<FileReader>) {
        let binary = '';
        let bytes = new Uint8Array(ResultAsArray(e));
        for (var i = 0; i < bytes.byteLength; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return binary;
    }

    SendBlockToServer(filter: ImageParameter) {

        this.imageLibraryService.UploadFile(filter).subscribe((res: any) => {
            let pmResponse: ServiceResponse = res;
            if (pmResponse.HasError || !pmResponse.Result) {
                this.HandleResponseError(pmResponse);
                return;
            }
            let result = pmResponse.Result;
            this.SplitFileInChunks(result);
        });

    }

    private SplitFileInChunks(result: any) {
        this.filterImageParameter = result;
        var ChunkSize = 100000;
        if (result.FileSize > 2000000) {
            ChunkSize = 1000000;
        }
        if (result.SentSize < result.FileSize && !this.IsUploadCanceled) {
            this.UploadFileChunks(result, ChunkSize);
            return;
        }
        this.UploadFileAsPackage(result);
        
    }

    private UploadFileAsPackage(result: any) {
        if (!result.Result) return;
        this.IncreaseProgressBar(result);
        this.IsUploadInProgress = false;
        this.UploadedSuccessfully = true;
        this.uploadedDocumentId = result.Result.substring(0, result.Result.indexOf('.'));
    }

    private UploadFileChunks(result: any, ChunkSize: number) {
        var filebuffer = this.GetFileBufferOfChunks(result, ChunkSize);
        this.ArrayBufferToBase64(filebuffer, this);
        this.IsUploadInProgress = true;
        this.IncreaseProgressBar(result);
    }

    private GetFileBufferOfChunks(result: any, ChunkSize: number) {
        if ((result.FileSize - result.SentSize) >= ChunkSize) {
            return this.File.slice(result.SentSize, result.SentSize + ChunkSize);
        }
        return this.File.slice(result.SentSize, result.FileSize);
    }

    HandleResponseError(response: any) {
        let error = "Upload file Failed";
        if (response.HasError && response.ErrorsArray && response.ErrorsArray.length > 0)
            error = error + ": " + response.ErrorsArray[0];
        this.ShowMessage(error);
    }

    IncreaseProgressBar(filter: ImageParameter) {
        if (this.IsUploadCanceled) return;
        let pre = 100 / filter.BlocksNumber;
        let progressBarValue = (filter.BufferNumber + 1) * pre;
        this.SetProgressBarPercentText(progressBarValue);        
    }

    public SetProgressBarPercentText(progressBarValue: any) {
        var elem = document.getElementById(this.ProgressBarId);
        if (!elem) return;

        if (progressBarValue == 100) {
            elem.style.width = (progressBarValue - 0.6) + '%';
            this.ProgressBarPercentText = progressBarValue.toString() + ' %';
        }
        else {
            elem.style.width = progressBarValue + '%';
            this.ProgressBarPercentText = progressBarValue.toFixed(2).toString() + ' %';
        }
    }

    NextButtonClicked() {
        this.ValidateDeploymentPackage();        
    }

    NextButtonClickedWithoutValidationErrors() {
        if (this.IsValidZipFile) {
            this.CurrentSession.StopBusyIndicator();
            this.IsNextClicked = true;
            return;
        }
        if (AppTool.IsNullOrEmpty(this.uploadedDocumentId)) {
            this.ShowMessage("File not uploaded yet!");
            return;
        }
        this.CurrentSession.StartBusyIndicator("Validating ...");
        this.deploymentPackageExtendedPMService.GetDeploymentPackageDetailsListByDocumentId(this.uploadedDocumentId).subscribe((response: ServiceResponse) => {
            if(response.HasError) return;           
            if (!response.Result) return;
            this.CurrentSession.StopBusyIndicator();
            this.HandleGetDeploymentPackageDetailsListResult(response.Result);         
        });
    }
    HandleGetDeploymentPackageDetailsListResult(result: any) {
        if (!result) return;
        if (!result.IsValidZipFile) {
            this.HandleZipFileException();
            return;
        }
        this.IsNextClicked = true;
        this.IsValidZipFile = true;
        if (!AppTool.IsNullOrEmpty(result.ValidationMessage)) {
            this.HandleValidationMessage(result.ValidationMessage);
        }
        if (result.DeploymentPackageDetailsList) {
            this.HandleDeploymentPackageDetailsList(result.DeploymentPackageDetailsList);
        }
    }


    HandleZipFileException() {
        this.CurrentSession.StopBusyIndicator();
        this.IsValidZipFile = false;
        let msgWindow = new MessageWindow();
        msgWindow.Show("Invalid Zip file, please export the package from the source company and import it again");
        msgWindow.WindowClosed.subscribe((event: any) => {
            if (event != "event") return;
            this.HandleReUploadingZipFile();
        });

    }
    private HandleReUploadingZipFile() {
        this.DeleteUploadedDocument(true);
        this.IsUploadVisibile = true;
        this.IsShowProgressBar = false;
        this.FileName = "";
        this.FileSize = "";
        this.uploadedDocumentId = "";
        this.UploadedSuccessfully = false;
    }

    HandleValidationMessage(validationMessage: string) {
        this.deploymentValidationMessage = validationMessage;
    }
    HandleDeploymentPackageDetailsList(deploymentPackageDetailsList: Array<DeploymentPackageDetailsList>){
        this.deploymentPackageDetailsList = deploymentPackageDetailsList;
        this.DeploymentPackageDetailsCollection = new ObservableCollection(this.deploymentPackageDetailsList);
        this.CurrentSession.StopBusyIndicator();
    }

    public CancelButtonClicked() {
        if (AppTool.IsNullOrEmpty(this.uploadedDocumentId)) {
            this.CurrentSession.CloseCurrentWindow();
            return;
        }
        this.DeleteUploadedDocument();
    }

    private DeleteUploadedDocument(isDeleteToReUpload:boolean = false) {
        this.CurrentSession.StartBusyIndicator("");
        this.deploymentPackageExtendedPMService.DeleteImportedDocumentById(this.uploadedDocumentId).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
                this.ShowMessage(response.ErrorsArray[0]);
                return;
            }
            if (isDeleteToReUpload) return;
            this.CurrentSession.CloseCurrentWindow();
        });
    }

    public DeployButtonClicked() {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Once you deploy this package, the new changes are permanent. All components that are shown in the list will be added to the listed objects");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (!AppTool.IsNullOrEmpty(this.deploymentValidationMessage)) {
                    this.ShowErrorsComponent();
                    return;
                }
                this.EntityPM.DocumentId = this.uploadedDocumentId;
                this.CurrentSession.StartBusyIndicator("Installing New Changes ...");
                this.AddNewDeploymentPackageComponent.CreateDeploymentPackage();
            }
        });

    }

    initializeStartCheckDeploymentPackageDeployViaWorkerRoleTimer() {
        return interval(250).pipe(timeInterval());
    }


    private startCheckDeploymentPackageDeployViaWorkerRoleTimersub: any = null;
    private isStartCheckDeploymentPackageDeployViaWorkerRoleTimer: boolean = false;

    public StartCheckDeploymentPackageDeployViaWorkerRoleTimer(deploymentPackageExecutionLogId: string) {

        if (this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer) {
            this.startCheckDeploymentPackageDeployViaWorkerRoleTimersub.unsubscribe();
        }

        this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer = true;
        this.startCheckDeploymentPackageDeployViaWorkerRoleTimersub = this.initializeStartCheckDeploymentPackageDeployViaWorkerRoleTimer().subscribe(respose => {


            if (!this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer) {
                this.startCheckDeploymentPackageDeployViaWorkerRoleTimersub.unsubscribe();
                this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer = false;
                return;
            }
            if (this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer) {
                this.GetDeploymentPackageExecutionLogList(deploymentPackageExecutionLogId);
            }
        });

    }
    private GetDeploymentPackageExecutionLogList(deploymentPackageExecutionLogId: string) {
        if (this.deploymentPackageExecutionLogListExtendedService == null) {
            this.deploymentPackageExecutionLogListExtendedService = new DeploymentPackageExecutionLogListExtendedService();
        }


        this.deploymentPackageExecutionLogListExtendedService.GetDeploymentPackageExecutionLogList(deploymentPackageExecutionLogId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            var deploymentPackageExecutionLogList: DeploymentPackageExecutionLogList = pmResponse.Result;
            if (!this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer) return;
            if (pmResponse.HasError || !deploymentPackageExecutionLogList || (deploymentPackageExecutionLogList && (deploymentPackageExecutionLogList.StatusCode == "D" || deploymentPackageExecutionLogList.StatusCode == "F" || deploymentPackageExecutionLogList.StatusCode == "T"))) {
                this.startCheckDeploymentPackageDeployViaWorkerRoleTimersub.unsubscribe();
                this.isStartCheckDeploymentPackageDeployViaWorkerRoleTimer = false;
            }

            if ((pmResponse.HasError && pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) || (deploymentPackageExecutionLogList.StatusCode == "F" || deploymentPackageExecutionLogList.StatusCode == "T")) {
                this.CurrentSession.StopBusyIndicator();
                this.deploymentValidationMessage = deploymentPackageExecutionLogList.ExceptionMessage;
                this.ShowErrorsComponent();
                return;
            }

            if (!deploymentPackageExecutionLogList) {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage("Deployment Package execution Log not found");
                return;
            }



            if (deploymentPackageExecutionLogList.StatusCode == "D") {
                this.metadataService.Refresh();
            }
            

        });
    }
    ShowErrorsComponent() {
        if (AppTool.IsNullOrEmpty(this.deploymentValidationMessage)) return;
        this.HasErrorsWhileImporting = true;
        this.IsNextClicked = false;

        if (this.deploymentValidationMessage.lastIndexOf('-') > 0) {
            this.deploymentValidationMessage = this.deploymentValidationMessage.substring(0, this.deploymentValidationMessage.lastIndexOf('-'));
        }
        
        this.exceptionsList = this.deploymentValidationMessage.split('-');
        this.ExceptionsCollection = new ObservableCollection(this.exceptionsList);
    }

}

class DeploymentPackageDetailsList {
    public ComponentName: string;
    public Type: string;
    public Entity: string;

    public DeploymentPackageDetailsList() {

    }
}
