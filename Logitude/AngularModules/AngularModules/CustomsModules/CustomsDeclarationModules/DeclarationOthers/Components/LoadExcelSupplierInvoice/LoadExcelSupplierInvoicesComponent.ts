import { OnInit, Component, ViewChild, ElementRef } from '@angular/core';
import { ExportDeclarationClosingDataPM } from '../../../../../Customs/EntityPMs/ExportDeclarationClosingDataPM';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ExportDeclarationClosingDataPMService } from '../../../../../Customs/Services/StandardPMs/ExportDeclarationClosingDataPMService';
import { Time } from '@angular/common';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { GenericRequestParams } from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { SupplierInvoiceService } from '../../../../../Customs/Services/Others/SupplierInvoiceService';
import { AppTool } from '../../../../../Infrastructure/Tools';
declare var attachmentUploader, ResultAsArray: any;
@Component({
    selector: 'LoadExcelSupplierInvoicesComponent',
    templateUrl: './LoadExcelSupplierInvoicesComponent.html',
})

export class LoadExcelSupplierInvoicesComponent extends BaseComponent {

    DataContext: any = this;
    FileName: string;
    FileExtension: string;
    File: any;
    IsShowProgressBar: boolean = false;
    UploadButtonIsEnabled: boolean = true;
    filterImageParameter: ImageParameter;
    UploadSuccessLabel: boolean
    Placeholder: any = "";
    ProgressBarPercentText: string;
    public UploadFileId: string = Guid.NewRandomString();
    private CurrentSession = SessionLocator.SelectedSession;
    public DeclarationId: string;

    constructor(private EntityResourceService: EntityResourceService) {
        super();
    }

    customerId: string;
    get CustomerId() { return this.customerId }
    set CustomerId(value: string) {
        this.customerId = value;
    }

    
    SetWindowArgs(args: any)
    {
        if (!AppTool.IsNullOrEmpty(args))
        {
            this.DeclarationId = args.DeclarationId;
        }
    }

    OpenUpLoadFile()
    {
        document.getElementById(this.UploadFileId).click();
    }

    public ShowMessage(message: string)
    {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    UploadFile(event: any)
    {
        var file: any = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");

            if (this.FileExtension != "csv") {
                this.ShowMessage("חובה קובץ CSV");
                return;
            }

            this.File = file;

            if (this.FileExtension && this.FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;

                this.filterImageParameter = new ImageParameter();
                this.filterImageParameter.Key = Guid.newGuid();
                this.filterImageParameter.IsFirstTry = true;
                this.filterImageParameter.Extension = this.FileExtension;
                this.filterImageParameter.UploadMode = "Block";
                this.filterImageParameter.FileSize = file.size;
                this.filterImageParameter.Tenant = SessionLocator.Tenant;
                this.UploadSuccessLabel = true;
                this.ArrayBufferToBase64(file, this);
            }
        }
    }

    ArrayBufferToBase64(file: any, viewmodel: any)
    {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.filterImageParameter.Base64String = window.btoa(binary);
            viewmodel.IncreaseProgressBar(100);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }

    IncreaseProgressBar(ProgressBarValue: number)
    {
        debugger;
        var elem = document.getElementById("myBar") as HTMLProgressElement;
        if (ProgressBarValue == 100) {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }
        else {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }

    }

    OkButtonClicked()
    {
        var supplierInvoiceService = new SupplierInvoiceService();
        supplierInvoiceService.PutSupplierInvioceFromFileRequest(this.filterImageParameter, SessionLocator.Tenant, this.CustomerId, this.DeclarationId).subscribe((myServiceResponse: ServiceResponse) => {

            if (myServiceResponse.HasError) {
                this.ShowMessage(myServiceResponse.ErrorsArray[0]);
            }
            else {
                this.ShowMessage(myServiceResponse.Result);
            }
            
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    @ViewChild('myInput')
    myInputVariable: ElementRef;
    DeleteFileButtonClicked() {
        this.myInputVariable.nativeElement.value = "";
        this.filterImageParameter = null;
        this.IsShowProgressBar = false;
        this.UploadButtonIsEnabled = true;
        this.FileName = "";
        this.ProgressBarPercentText = "";
        //this.ErrorsResultList.Clear();
        this.UploadSuccessLabel = false;
    }
}




