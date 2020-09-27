import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { SupplierInvioceItemCertificatsService } from '../../../Customs/Services/WebServices/SupplierInvioceItemCertificatsService';
import { CreditQueryRequestParams } from '../../../Customs/DataContract/RequestParams/CreditQueryRequestParams';
import { RTGSInfoQueryResponseData } from '../../../Customs/DataContract/ResponseData/RTGSInfoQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent, CustomMessageProgressHelper } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ImageParameter} from '../../../Infrastructure/DataContracts/ImageParameter';
import {DocumentsFilingExtendedPMService} from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';


import {Guid} from '../../../Infrastructure/Utilities/Guid';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
declare var attachmentUploader, ResultAsArray: any;

@Component({
    selector: 'ReceiptCertificateFromFileComponent',
    
    templateUrl: './ReceiptCertificateFromFileComponent.html',
})

export class ReceiptCertificateFromFileComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: ReceiptCertificateFromFileComponent = this;
    public ObjectTableName: string = "Customs.SupplierInvioceItemsCertificates";
    UploadButtonIsEnabled: boolean = true;
    ExportAsExcelButtonIsEnabled: boolean = false;
    _supplierInvioceItemCertificatsService: SupplierInvioceItemCertificatsService = new SupplierInvioceItemCertificatsService();

    public ErrorsResultList: ObservableCollection;
    public UploadFileId: string = Guid.NewRandomString();
    FileName: string;
    FileSize: string;
    FileExtension: string;
    File: any;
    filterImageParameter: ImageParameter;
    FileData: number;
    ProgressBarPercentText: string;
    IsShowProgressBar: boolean = false;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    UploadSuccessLabel:boolean
    Placeholder: any="";
    tenant: number;
    ResponseMessage: any;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ErrorsResultList = new ObservableCollection([]);
        this.tenant = SessionLocator.Tenant;
        this.UploadSuccessLabel = false;
    }


    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new CreditQueryRequestParams();
        }

        if (this.ResponseData) {
            if (this.ResponseData.CertificateErrorViewList) {
                this.ErrorsResultList.InsertCollection(this.ResponseData.CertificateErrorViewList);
            }
        }
    }

    //#region Properties
    customerId: string;
    get CustomerId() { return this.customerId }
    set CustomerId(value: string) {
        this.customerId = value;
    }

    //#endregion Properties

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        if (this.filterImageParameter != null && this.filterImageParameter.Base64String != null) {
            this.OkButtonClicked();
        }
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    OpenUpLoadFile() {
        document.getElementById(this.UploadFileId).click();
    }

    UploadFile(event: any) {
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
    ArrayBufferToBase64(file: any, viewmodel: any) {
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
    UploadSuccess: any = false;
    OkButtonClicked() {


        this._supplierInvioceItemCertificatsService.PutSupplierInvioceItemCertificatFromFileRequest(this.filterImageParameter, this.tenant, this.CustomerId).subscribe((myServiceResponse: ServiceResponse) => {
            this.UploadSuccess = true;
            this.ErrorsResultList.InsertCollection(myServiceResponse.Result);
            if (this.ErrorsResultList.Collection.length > 0) {
                this.ExportAsExcelButtonIsEnabled = true;
            }
        });

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
        this.ErrorsResultList.Clear();
        this.UploadSuccessLabel = false;
        this.ExportAsExcelButtonIsEnabled = false;
    }
    ExportExcel() {
        var url = ServiceHelper.GetLogitudeURL() + 'api/SupplierInvioceItemCertificats/GetSupplierInvoiceItemCertificatErrors2Excel?' + 'tenant=' + this.tenant.toString() + '&key=' + this.filterImageParameter.Key;
        window.open(url);
    }
    IncreaseProgressBar(ProgressBarValue: number) {
        var elem = document.getElementById("myBar");
        if (ProgressBarValue == 100) {
            elem.style.width = (ProgressBarValue - 0.1) + '%';
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
        }
        else {
            elem.style.width = ProgressBarValue + '%';
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
        }

    }
}
export class CertificateErrorView {
    ExcelRow: string;
    CustomfileNr: string;
    DeclarationId: string;
    SupplierItemInvoice: string;
    Model: string;
    Errors: string;
}
