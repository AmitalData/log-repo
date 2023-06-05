import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {TenantPMService} from '../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {AppTool} from '../../../Infrastructure/Tools';
import {QuestionnaireList} from '../../../CRM/EntityLists/QuestionnaireList'; 
import {QuestionnaireListService} from '../../../CRM/Services/StandardLists/QuestionnaireListService'; 
import {VatFormatTypeListService} from '../../../Common/Services/StandardLists/VatFormatTypeListService'; 
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'OcrDefaultsSettingsComponent',
    
    templateUrl: './OcrDefaultsSettingsComponent.html',
})

export class OcrDefaultsSettingsComponent extends BaseComponent {
    public DataContext: any = this;
   
    public ObjectTableName: string = "Customs.SupplierInvoice";
    private tenantPM: TenantPM;
    public ValidationErrorsList: string[];
    public QuestionnaireList: QuestionnaireList []= [];
    public IsVisibile = false;
    IsShowAreaDefaultQuestionnaire: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor() {
        super();
        this.CurrentSession.StartBusyIndicator("Loading...");

        this._entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem", 0).subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType", 0).subscribe((response: any) => {
                    this.LoadDefaults();
                })
            })
        })
        this.tenantPM = new TenantPM();
       
       
     
    }

    private LoadDefaults() {
              
        this.AccountTypeCode="380";
        this.PartyRelationshipCode="3";
        this.BuyerRoleCode="9";
        
    }
    
    //SetUIProperties
   
    
    

   
    

    

   

    get AccountTypeCode() { return "380"; }
    set AccountTypeCode(value: string) {
       
    }

    get PartyRelationshipCode() { return  "3"}
    set PartyRelationshipCode(value: string) {
       
    }

    get ClaimReasonCode() { return  "6"}
    set ClaimReasonCode(value: string) {
       
    }

    get TransactionNatureCode() { return  "2"}
    set TransactionNatureCode(value: string) {
       
    }
    get ProcessTypeCode() { return  "1100105"}
    set ProcessTypeCode(value: string) {
       
    }

   

    

   
    get BuyerRoleCode() { return  "9" }
    set BuyerRoleCode(value: string) {
       
    }

   

    
    
    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();
        }

    }
    SubmitTenantChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: TenantPMService = new TenantPMService();
        myService.update(this.tenantPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.tenantPM;
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
