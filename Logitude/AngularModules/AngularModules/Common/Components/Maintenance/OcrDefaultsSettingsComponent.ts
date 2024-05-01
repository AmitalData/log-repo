import { Component, NgModule } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { TenantPMService } from '../../../Common/Services/StandardPMs/TenantPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { AppTool } from '../../../Infrastructure/Tools';
import { QuestionnaireList } from '../../../CRM/EntityLists/QuestionnaireList';
import { QuestionnaireListService } from '../../../CRM/Services/StandardLists/QuestionnaireListService';
import { VatFormatTypeListService } from '../../../Common/Services/StandardLists/VatFormatTypeListService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SupplierInvioceExportDefaultPMService } from 'Customs/Services/StandardPMs/SupplierInvioceExportDefaultPMService';
import { SupplierInvioceExportDefaultListService } from 'Customs/Services/StandardLists/SupplierInvioceExportDefaultListService';
import { SupplierInvioceExportDefaultPM } from 'Customs/EntityPMs/SupplierInvioceExportDefaultPM';
import { SupplierInvioceExportDefaultExtendedPMService } from 'Customs/Services/ExtendedPMs/SupplierInvioceExportDefaultExtendedPMService';
import { InvoiceItemCertificateLine, SupplierInvoiceItemCertificatesComponent } from 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SupplierInvioceItemCertificatPM } from 'Customs/EntityPMs/SupplierInvioceItemCertificatPM';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { SupplierInvoicePM } from 'Customs/EntityPMs/SupplierInvoicePM';
import { DeclarationPMService } from 'Customs/Services/StandardPMs/DeclarationPMService';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { SupplierInvoicePMService } from 'Customs/Services/StandardPMs/SupplierInvoicePMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { SupplierInvoiceService } from 'Customs/Services/Others/SupplierInvoiceService';
import { MultiUpdateOcrParams, SupplierInvioceItemCertificat } from 'Customs/DataContract/RequestParams/MultiUpdateOcrParams';
import { forEach } from 'cypress/types/lodash';
import { SupplierInvioceItemCertificatsService } from 'Customs/Services/WebServices/SupplierInvioceItemCertificatsService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    selector: 'OcrDefaultsSettingsComponent',

    templateUrl: './OcrDefaultsSettingsComponent.html',
})


export class OcrDefaultsSettingsComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.SupplierInvioceExportDefault";
    private SupplierInvioceExportDefaultPM: SupplierInvioceExportDefaultPM;
    private SupplierInvoiceComprehensiveUpdate: SupplierInvoicePM[] = [];
    public ValidationErrorsList: string[];
    public IsVisibile = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsFromSupplierInvoice = false;
    public DeclarationPM: DeclarationPM;
    public supplierInvoicePMService = new SupplierInvoicePMService();
    public ItemsSource: ObservableCollection;
    private claimReasonCodeChecked: boolean = true
    private transactionNatureCodeChecked: boolean = true
    private processTypeCodeChecked: boolean = true
    private buyerRoleCodeChecked: boolean = true
    private partyRelationshipCodeChecked: boolean = true
    private accountTypeCodeChecked: boolean = true
    private disableSubmit: boolean = false
    FIELD_IS_REQUIERD: string;
    public ProcessTypeCodeFilterItems: ApiQueryFilters;

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        this.CurrentSession.StartBusyIndicator('Loading...');
        debugger
        this.ProcessTypeCodeFilterItems = new ApiQueryFilters();
       
        this.LoadDefaults();

    }
    SetWindowArgs(args: any) {

        if (!AppTool.IsNullOrEmpty(args)) {
            this.DeclarationPM = args.EntityPM;
            this.SupplierInvoiceComprehensiveUpdate = args.SupplierInvoiceComprehensiveUpdate;
            this.IsFromSupplierInvoice = args.IsFromSupplierInvoice
        }
        if (this.DeclarationPM.Direction == "E") {
            this.ProcessTypeCodeFilterItems.addAdditionalFilter("LeadDocumentTypeID", this.DeclarationPM.DeclarationTypeCode, null, null, "Equals", false, false, false, "string",false,true);
        }
    }
    private LoadDefaults() {
        this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
        var myService: SupplierInvioceExportDefaultExtendedPMService = new SupplierInvioceExportDefaultExtendedPMService();
        myService.getByTenat(SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            this.SupplierInvioceExportDefaultPM = response.Result;

            if (this.SupplierInvioceExportDefaultPM == null) {
                this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
                this.DefaultsSupplierInvioceExportDefaultPM()
            }
            this.IsVisibile = true;
            this.CurrentSession.StopBusyIndicator();

        });


    }

    DefaultsSupplierInvioceExportDefaultPM() {
        this.AccountTypeCode = "380"
        this.PartyRelationshipCode = "3"
        this.ClaimReasonCode = "6"
        this.TransactionNatureCode = "2"
        this.ProcessTypeCode = "1100105"
        this.BuyerRoleCode = "9"
    }

    Add() {

        var item: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(null);
        item.SequenceNumeric = this.ItemsSource.Collection.length + 1;
        item.Tenant = this.SupplierInvioceExportDefaultPM.Tenant;
        this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, null));
    }

    DeleteButtonClicked(item: any) {
        var sequenceNumeric = 1;
        this.ItemsSource.Remove(item);
        this.ItemsSource.Collection.forEach((item: InvoiceItemCertificateLine) => {
            item.SequenceNumeric = sequenceNumeric;
            sequenceNumeric++;
        });

    }



    get AccountTypeCode() { return this.SupplierInvioceExportDefaultPM?.AccountTypeCode; }
    set AccountTypeCode(value: string) {

        if (this.SupplierInvioceExportDefaultPM.AccountTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.AccountTypeCode = this.AccountTypeCodeChecked ? value : "non";

        }
    }

    get PartyRelationshipCode() { return this.SupplierInvioceExportDefaultPM?.PartyRelationshipCode }
    set PartyRelationshipCode(value: string) {

        if (this.SupplierInvioceExportDefaultPM.PartyRelationshipCode != value) {
            this.SupplierInvioceExportDefaultPM.PartyRelationshipCode = this.PartyRelationshipCodeChecked ? value : "non";

        }
    }

    get ClaimReasonCode() { return this.SupplierInvioceExportDefaultPM?.ClaimReasonCode }
    set ClaimReasonCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ClaimReasonCode != value) {
            this.SupplierInvioceExportDefaultPM.ClaimReasonCode = this.ClaimReasonCodeChecked ? value : "non";

        }
    }

    get TransactionNatureCode() { return this.SupplierInvioceExportDefaultPM?.TransactionNatureCode }
    set TransactionNatureCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.TransactionNatureCode != value) {
            this.SupplierInvioceExportDefaultPM.TransactionNatureCode = this.TransactionNatureCodeChecked ? value : "non";

        }
    }
    get ProcessTypeCode() { return this.SupplierInvioceExportDefaultPM?.ProcessTypeCode }
    set ProcessTypeCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ProcessTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.ProcessTypeCode = this.ProcessTypeCodeChecked ? value : "non";

        }
    }

    get BuyerRoleCode() { return this.SupplierInvioceExportDefaultPM?.BuyerRoleCode }
    set BuyerRoleCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.BuyerRoleCode != value) {
            this.SupplierInvioceExportDefaultPM.BuyerRoleCode = this.BuyerRoleCodeChecked ? value : "non";

        }
    }


    get AccountTypeCodeChecked() { return this.accountTypeCodeChecked }
    set AccountTypeCodeChecked(value: boolean) {
        if (!value)
            this.SupplierInvioceExportDefaultPM.AccountTypeCode = "non";
        this.accountTypeCodeChecked = value

    }


    get PartyRelationshipCodeChecked() { return this.partyRelationshipCodeChecked }
    set PartyRelationshipCodeChecked(value: boolean) {

        if (!value)
            this.SupplierInvioceExportDefaultPM.PartyRelationshipCode = "non";
        this.partyRelationshipCodeChecked = value
    }


    get ClaimReasonCodeChecked() { return this.claimReasonCodeChecked }
    set ClaimReasonCodeChecked(value: boolean) {
        if (!value)
            this.SupplierInvioceExportDefaultPM.ClaimReasonCode = "non";
        this.claimReasonCodeChecked = value

    }

    get TransactionNatureCodeChecked() { return this.transactionNatureCodeChecked }
    set TransactionNatureCodeChecked(value: boolean) {
        if (!value)
            this.SupplierInvioceExportDefaultPM.TransactionNatureCode = "non";
        this.transactionNatureCodeChecked = value
    }
    get ProcessTypeCodeChecked() { return this.processTypeCodeChecked }
    set ProcessTypeCodeChecked(value: boolean) {
        if (!value)
            this.SupplierInvioceExportDefaultPM.ProcessTypeCode = "non";
        this.processTypeCodeChecked = value

    }

    get BuyerRoleCodeChecked() { return this.buyerRoleCodeChecked }
    set BuyerRoleCodeChecked(value: boolean) {
        if (!value)
            this.SupplierInvioceExportDefaultPM.BuyerRoleCode = "non";
        this.buyerRoleCodeChecked = value

    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        var errors: string[] = [];
        //Validator.TryValidateObject(this.SupplierInvioceExportDefaultPM, this.DataContext.ObjectTableName, errors);

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsFromSupplierInvoice)
                this.SupplierInvoiceMultiUpdate();
            else
                this.SubmitChanges();

        }
    }
    notMandatoryIsNotEmpty: boolean = false;

    Update(multiUpdateOcrParams: MultiUpdateOcrParams) {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        var isValid = true;
        var inValid = false;
        const decPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
        const isExport: boolean = decPM.direction == 'E'
        for (let item of multiUpdateOcrParams.SupplierInvioceItemCertificats) {
          
            if (item.AttachmentTypeCode == null) {
                //var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                //var fieldError: string = translatedRequiredError.replace("%FieldName", "AttachmentTypeCode");
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.SupplierInvioceItemCertificat.F.AttachmentTypeCode")));
                isValid = false;
                break;
            }
            else {
                if (item.AttachmentTypeCode == "1" || item.AttachmentTypeCode == "2") {
                    if (AppTool.IsNullOrEmpty(item.CertificateNumber) || AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode) || AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode)) {
                        isValid = false;
                        inValid = true;
                    }
                    if (!AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || (!isExport && !AppTool.IsNullOrEmpty(item.CustomsAttachmentID))) {
                        inValid = true;
                        this.notMandatoryIsNotEmpty = true;
                    }
                }
                else {
                    if (item.AttachmentTypeCode == "4") {
                        if (AppTool.IsNullOrEmpty(item.CertificateExemptionTypeCode) || AppTool.IsNullOrEmpty(item.ReqConfirmationTypeCode)) {
                            isValid = false;
                            inValid = true;
                        }
                        if (!AppTool.IsNullOrEmpty(item.CertificateNumber) || !AppTool.IsNullOrEmpty(item.ResConfirmationTypeCode) || (!isExport && !AppTool.IsNullOrEmpty(item.CustomsAttachmentID))) {
                            inValid = true;
                            this.notMandatoryIsNotEmpty = true;
                        }
                    }
                }
            }
        }
        if (inValid) {
            isValid = false;
            var confirm = new ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirm.ShowNoButton = true;
            if (this.notMandatoryIsNotEmpty) {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateNotMandatoryFields"));
            }
            else {
                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CertificateMandatoryFields"));
            }
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    if (errors.length == 0) {
                        SessionLocator.SelectedSession.StartBusyIndicator("");

                        this._SupplierInvoiceService.PostMultiUpdateOCR(multiUpdateOcrParams)
                            .subscribe((res: any) => {
                                this.CurrentSession.CloseCurrentWindowEmit("update");

                                SessionLocator.SelectedSession.StopBusyIndicator();
                                var myMessageWindow = new MessageWindow();
                                myMessageWindow.Show(res.Result);
                                myMessageWindow.WindowClosed.subscribe(s => {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                                });
                            });
                    }
                    else {
                        this.ValidationErrorsList = errors;
                    }
                    confirm.Close();
                }
            });
        }
        else {
            if (errors.length == 0) {
                SessionLocator.SelectedSession.StartBusyIndicator("");

                this._SupplierInvoiceService.PostMultiUpdateOCR(multiUpdateOcrParams)
                    .subscribe((res: any) => {
                        this.CurrentSession.CloseCurrentWindowEmit("update");

                        SessionLocator.SelectedSession.StopBusyIndicator();
                        var myMessageWindow = new MessageWindow();
                        myMessageWindow.Show(res.Result);
                        myMessageWindow.WindowClosed.subscribe(s => {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                        });
                    });
            }
            else {
                this.ValidationErrorsList = errors;
            }
        }
        return isValid;
    }



    _SupplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();


    SupplierInvoiceMultiUpdate() {
        var currRequestParams = new MultiUpdateOcrParams();
        currRequestParams.DeclarationId = this.CurrentSession.CurrentEditComponent.EntityPM.Id;
        currRequestParams.SupplierInvoiceList = this.SupplierInvoiceComprehensiveUpdate.map(p => p.InvoiceCounterKey).join(',');
        currRequestParams.SupplierInvioceItemCertificats = []
        currRequestParams.SupplierInvioceExportDefault = this.SupplierInvioceExportDefaultPM;
        this.ItemsSource.Collection.forEach(element => {
            var supplierInvioceItemCertificat: SupplierInvioceItemCertificat = new SupplierInvioceItemCertificat();
            supplierInvioceItemCertificat.CertificateNumber = element.CertificateNumber;
            supplierInvioceItemCertificat.ReqConfirmationTypeCode = element.ConfirmationTypeCode;
            supplierInvioceItemCertificat.CertificateExemptionTypeCode = element.CertificateExemptionTypeCode;
            supplierInvioceItemCertificat.AttachmentTypeCode = element.AttachmentTypeCode;
            supplierInvioceItemCertificat.ResConfirmationTypeCode = element.ResConfirmationTypeCode;
            supplierInvioceItemCertificat.CustomsAttachmentID = element.CustomsAttachmentID;
            supplierInvioceItemCertificat.ReqConfirmationTypeName = element.ConfirmationTypeName;
            supplierInvioceItemCertificat.CertificateExemptionTypeName = element.CertificateExemptionTypeName;
            supplierInvioceItemCertificat.AttachmentTypeName = element.AttachmentTypeName;
            supplierInvioceItemCertificat.ResConfirmationTypeName = element.ResConfirmationTypeName;
            supplierInvioceItemCertificat.SequenceNumeric = element.SequenceNumeric;
            supplierInvioceItemCertificat.ExternalCertificatCode = element.ExternalCertificatCode;
            supplierInvioceItemCertificat.ExternalRequestTypeCode = element.ExternalRequestTypeCode;
            supplierInvioceItemCertificat.ApprovalRequestNumber = element.ApprovalRequestNumber;
            currRequestParams.SupplierInvioceItemCertificats.push(supplierInvioceItemCertificat)

        });
        this.Update(currRequestParams)


    }




    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: SupplierInvioceExportDefaultPMService = new SupplierInvioceExportDefaultPMService();
        this.SupplierInvioceExportDefaultPM.Tenant = SessionLocator.Tenant;
        if (this.SupplierInvioceExportDefaultPM?.Id == null) {
            myService.insert(this.SupplierInvioceExportDefaultPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {


                        this.CurrentSession.CloseCurrentWindowEmit("ok");


                    }
                    else {

                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
        else {
            myService.update(this.SupplierInvioceExportDefaultPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {

                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                    }
                    else {

                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }

    }

    ngDoCheck() {
        if (!this.accountTypeCodeChecked && !this.partyRelationshipCodeChecked && !this.buyerRoleCodeChecked && !this.processTypeCodeChecked && !this.transactionNatureCodeChecked && !this.claimReasonCodeChecked) {
            this.disableSubmit=true
        }else{
            this.disableSubmit=false
        }
    }
}
