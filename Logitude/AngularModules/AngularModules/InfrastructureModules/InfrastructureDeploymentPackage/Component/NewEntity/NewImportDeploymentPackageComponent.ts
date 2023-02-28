import { Component } from '@angular/core';
import { DocumentsFilingExtendedPMService } from '../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ImageParameter } from '../../../../Infrastructure/DataContracts/ImageParameter';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeploymentPackageDetails } from '../../../../Infrastructure/EntityPMs/DeploymentPackageDetails';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackageExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DeploymentPackageExtendedPMService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
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
    public ProgressBarId: string = Guid.newGuid();
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

    constructor() {
        super();
        this.UploadFileId = Guid.NewRandomString();
        this.deploymentPackageDetailsList = [];
        this.EntityPM = new DeploymentPackagePM();
        this.UIProperties.SetRequired("Code", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Name", this.ObjectTableName, true);
    }

    Run() {

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

    SetProgressBarPercentText(progressBarValue: any) {
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
        if (AppTool.IsNullOrEmpty(this.uploadedDocumentId)) {
            this.ShowMessage("File not uploaded yet!");
            return;
        }
        this.IsNextClicked = true;
        if (this.DeploymentPackageDetailsCollection.Length > 0) return;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.deploymentPackageExtendedPMService.GetDeploymentPackageDetailsListByDocumentId(this.uploadedDocumentId).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            if (!response.Result) return;
            this.deploymentPackageDetailsList = response.Result;
            this.DeploymentPackageDetailsCollection = new ObservableCollection(this.deploymentPackageDetailsList);
            this.CurrentSession.StopBusyIndicator();
        });
    }

    public CancelButtonClicked() {
        if (AppTool.IsNullOrEmpty(this.uploadedDocumentId)) {
            this.CurrentSession.CloseCurrentWindow();
            return;
        }
        this.DeleteUploadedDocument();
    }

    private DeleteUploadedDocument() {
        this.CurrentSession.StartBusyIndicator("");
        this.deploymentPackageExtendedPMService.DeleteImportedDocumentById(this.uploadedDocumentId).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
                this.ShowMessage(response.ErrorsArray[0]);
                return;
            }
            this.CurrentSession.CloseCurrentWindow();
        });
    }

    public DeployButtonClicked() {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Once you deploy this package, the new changes are permanent. All components that are shown in the list will be added to the listed objects");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.DocumentId = this.uploadedDocumentId;

                this.AddNewDeploymentPackageComponent.CreateButtonClicked();
            }
        });

    }

}

class DeploymentPackageDetailsList {
    public ComponentName: string;
    public Type: string;
    public Entity: string;

    public DeploymentPackageDetailsList() {

    }
}
