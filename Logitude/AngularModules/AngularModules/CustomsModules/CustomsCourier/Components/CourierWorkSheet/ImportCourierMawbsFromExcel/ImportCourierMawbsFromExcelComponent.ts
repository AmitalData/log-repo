import { Component,ElementRef} from '@angular/core';
import { AfterViewInit, ViewChild } from '@angular/core';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { CreditQueryRequestParams } from 'Customs/DataContract/RequestParams/CreditQueryRequestParams';
import { CustomSendOptionsArgs } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { SupplierInvioceItemCertificatsService } from 'Customs/Services/WebServices/SupplierInvioceItemCertificatsService';
import { CustomMessageWrapperComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { ImageParameter } from 'Infrastructure/DataContracts/ImageParameter';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
declare var attachmentUploader;
@Component({
    selector: 'ImportCourierMawbsFromExcelComponent',
    templateUrl: './ImportCourierMawbsFromExcelComponent.html',
})

export class ImportCourierMawbsFromExcelComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    UploadButtonIsEnabled: boolean = true;
    ExportAsExcelButtonIsEnabled: boolean = false;
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    public ObjectTableName: string = "Customs.Declarations";
    public DataContext: ImportCourierMawbsFromExcelComponent = this;

    public ErrorsResultList: ObservableCollection;
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

    @ViewChild('myInput', { static: false }) myInput!: ElementRef;
    openFileUploader() {
        this.myInput.nativeElement.click();
    }

    formData: FormData = new FormData();
    UploadFile(event: any) {
        const fileList: FileList = event.target.files;
        if (fileList.length > 0) {
          const file: File = fileList[0];
          this.FileName = file.name.replace("." + this.FileExtension, "");
          this.UploadButtonIsEnabled = false;
          this.formData.append('file', file, file.name);
        }
    }
    
    UploadSuccess: any = false;

    OkButtonClicked() {
        this.ErrorsResultList.Clear();
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._DeclarationWebService.ImportCourierMawbsFromExcel(this.formData,SessionLocator.LoggedUserId,this.tenant).subscribe((res:string[]) => {
            if(res == null || res.length == 0){
                this.CurrentSession.CloseCurrentWindow();
            }
            this.UploadSuccess = true;
            this.ErrorsResultList.InsertCollection(res);
            SessionLocator.SelectedSession.StopBusyIndicator();
               // this.ErrorsResultList.InsertCollection(res);
            
        });

    }

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
