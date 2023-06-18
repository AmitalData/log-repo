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
    public declaration: DeclarationPM;
    public ItemsSource: ObservableCollection;

    constructor() {

        super();
        this.ItemsSource = new ObservableCollection([]);
        this.CurrentSession.StartBusyIndicator('Loading...');
        this.LoadDefaults();

    }

    private LoadDefaults() {
        debugger;
        this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
        var myService: SupplierInvioceExportDefaultExtendedPMService = new SupplierInvioceExportDefaultExtendedPMService();

        myService.getByTenat(SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            this.SupplierInvioceExportDefaultPM = response.Result;

            if (this.SupplierInvioceExportDefaultPM == null)
                this.SupplierInvioceExportDefaultPM = new SupplierInvioceExportDefaultPM();
            this.IsVisibile = true;
            this.IsFromSupplierInvoice = this.CurrentSession?.Windows[0]?.WindowArgs?.IsFromSupplierInvoice;
            this.declaration = this.CurrentSession?.Windows[0]?.WindowArgs?.Declaration;
            this.SupplierInvoiceComprehensiveUpdate = this.CurrentSession?.Windows[0]?.WindowArgs?.SupplierInvoiceComprehensiveUpdate;
            this.CurrentSession.StopBusyIndicator();

        });


    }
    private sequenceNumeric = 0;
    Add() {

        var item: SupplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM(null);
        item.SequenceNumeric = ++this.sequenceNumeric;
        item.DeclarationId = this?.declaration.Id;
        item.Tenant = this.SupplierInvioceExportDefaultPM.Tenant;
        this.ItemsSource.Insert(new InvoiceItemCertificateLine(item, null));
    }

    DeleteButtonClicked(item: any) {
        this.ItemsSource.Remove(item);
    }

    get AccountTypeCode() { return this.SupplierInvioceExportDefaultPM?.AccountTypeCode ?? "380"; }
    set AccountTypeCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.AccountTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.AccountTypeCode = value;

        }
    }

    get PartyRelationshipCode() { return this.SupplierInvioceExportDefaultPM?.PartyRelationshipCode ?? "3" }
    set PartyRelationshipCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.PartyRelationshipCode != value) {
            this.SupplierInvioceExportDefaultPM.PartyRelationshipCode = value;

        }
    }

    get ClaimReasonCode() { return this.SupplierInvioceExportDefaultPM?.ClaimReasonCode ?? "6" }
    set ClaimReasonCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ClaimReasonCode != value) {
            this.SupplierInvioceExportDefaultPM.ClaimReasonCode = value;

        }
    }

    get TransactionNatureCode() { return this.SupplierInvioceExportDefaultPM?.TransactionNatureCode ?? "2" }
    set TransactionNatureCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.TransactionNatureCode != value) {
            this.SupplierInvioceExportDefaultPM.TransactionNatureCode = value;

        }
    }
    get ProcessTypeCode() { return this.SupplierInvioceExportDefaultPM?.ProcessTypeCode ?? "1100105" }
    set ProcessTypeCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.ProcessTypeCode != value) {
            this.SupplierInvioceExportDefaultPM.ProcessTypeCode = value;

        }
    }

    get BuyerRoleCode() { return this.SupplierInvioceExportDefaultPM?.BuyerRoleCode ?? "9" }
    set BuyerRoleCode(value: string) {
        if (this.SupplierInvioceExportDefaultPM.BuyerRoleCode != value) {
            this.SupplierInvioceExportDefaultPM.BuyerRoleCode = value;

        }
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        
        var errors: string[] = [];
        Validator.TryValidateObject(this.SupplierInvioceExportDefaultPM, this.DataContext.ObjectTableName, errors);

        this.ValidationErrorsList = errors;
        if (this.IsFromSupplierInvoice) {
            if (this.SupplierInvoiceComprehensiveUpdate.length > 0) {
                this.SupplierInvoiceComprehensiveUpdate.forEach(item => {
                    this.UpdateSupplierInvoice(this.declaration.SupplierInvoices.find(t=>t.InvoiceNumber==item.InvoiceNumber));
                });
            }
            this.SupplierInvoiceComprehensiveUpdate;
            this.declaration;
            this.SupplierInvioceExportDefaultPM;
           
        }
        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();

        }

    }
    UpdateSupplierInvoice(SupplierInvocie: SupplierInvoicePM) {
        SupplierInvocie.AccountTypeCode = this.SupplierInvioceExportDefaultPM.AccountTypeCode;
        SupplierInvocie.PartyRelationshipCode = this.SupplierInvioceExportDefaultPM.PartyRelationshipCode;
        SupplierInvocie.BuyerRoleCode = this.SupplierInvioceExportDefaultPM.BuyerRoleCode;
        SupplierInvocie.SupplierInvoiceItems.forEach(element => {
            element.SupplierInvoiceItemProcesTypes.forEach(ProcesTypes => {
                ProcesTypes.ProcessTypeCode=this.SupplierInvioceExportDefaultPM.ProcessTypeCode;
             });
             element.TransactionNatureCode=this.SupplierInvioceExportDefaultPM.TransactionNatureCode;
             element.ClaimReasonCode=this.SupplierInvioceExportDefaultPM.ClaimReasonCode;
             if(this.ItemsSource.Length>0){
                element.SupplierInvioceItemCertificats=this.ItemsSource.Collection;
             }
        });
        

    }


    SubmitTenantChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: SupplierInvioceExportDefaultPMService = new SupplierInvioceExportDefaultPMService();
        this.SupplierInvioceExportDefaultPM.Tenant = SessionLocator.Tenant;
        if (this.SupplierInvioceExportDefaultPM?.Id == null) {
            myService.insert(this.SupplierInvioceExportDefaultPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {

                        if(this.SupplierInvoiceComprehensiveUpdate)
                        {
                            this.CurrentSession.CloseCurrentWindowEmit("update")
                        } 
                      else{
                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                      }                    }
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
                      if(this.SupplierInvoiceComprehensiveUpdate)
                        {
                            this.CurrentSession.CloseCurrentWindowEmit("update")
                        } 
                      else{
                        this.CurrentSession.CloseCurrentWindowEmit("ok");

                      }
                    }
                    else {

                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }

    }
}
