import {Component}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {RevaluationPM} from '../../EntityPMs/RevaluationPM';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {FullAccountingSettingPMService} from '../../Services/StandardPMs/FullAccountingSettingPMService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {RevaluationPMService} from '../../Services/StandardPMs/RevaluationPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './NewRevaluationComponent.html',
})

export class NewRevaluationComponent extends BaseComponent {

    public EntityPM: RevaluationPM;
    ObjectTableName: string = "Revaluation";
    DataContext: any = this;
    public TenantPM: TenantPM;
    fullAccountingSettingPMService: FullAccountingSettingPMService = new FullAccountingSettingPMService();
    GLAccountFilterItems: ApiQueryFilters;
    RevaluationService: RevaluationPMService;
    entityResourceService: EntityResourceService = new EntityResourceService();
    public ValidationErrorsList: string[] = [];
    FIELD_IS_REQUIERD: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
      
            this.EntityPM = new RevaluationPM();
            this.TenantPM = SessionLocator.TenantPM;
            this.EntityPM.Tenant = this.TenantPM.Id;
            this.RevaluationDate = DateTool.GetCurrentDateTimeAsUtc();
            this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
            this.RevaluationService = new RevaluationPMService();
            this.EntityPM.RevaluationNumber = 0;
            this.EntityPM.CreatedByUserId = "new";
            this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.RevaluationEnabled = true;
            this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.fullAccountingSettingPMService.get(this.TenantPM.Id.toString()).subscribe((myResult: any) => {
                if (myResult) {
                    if (!myResult.HasError) {
                        this.RevaluationsGLAccountId = myResult.Result.ExchangeRateDiffGLAccountId;
                      //  this.EnglishName = myResult.Result.EnglishName;
                    }
                }
            });

            this.GLAccountFilterItems = new ApiQueryFilters();
            this.GLAccountFilterItems.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
       
    }
    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }


    get RevaluationDate() { return this.EntityPM.RevaluationDate; }
    set RevaluationDate(value: Date) {
        this.UIProperties.SetValidity("RevaluationDate", "Revaluation", true, "");
        if (this.EntityPM.RevaluationDate != value) {
          

            if (value > DateTool.GetCurrentDateTimeAsUtc()) {
              this.UIProperties.SetValidity("RevaluationDate", "Revaluation", false, TextCodeTranslator.Translate("Revaluation.O.FutureDateIsNotAllowed"));
            }
            this.EntityPM.RevaluationDate = value;
        }
    }

    get RevaluationsGLAccountId() { return this.EntityPM.RevaluationsGLAccountId; }
    set RevaluationsGLAccountId(value: string) {
        if (this.EntityPM.RevaluationsGLAccountId != value) {
            this.EntityPM.RevaluationsGLAccountId = value;
        }
    }

    //englishName: string;
    //get EnglishName() { return this.englishName; }
    //set EnglishName(value: string) {
    //    if (this.englishName != value) {
    //        this.englishName = value;
    //    }
    //}



    get GLAccountId() { return this.EntityPM.GLAccountId; }
    set GLAccountId(value: string) {
        if (this.EntityPM.GLAccountId != value) {
            this.EntityPM.GLAccountId = value;
        }
    }

    get ChartOfAccountsId() { return this.EntityPM.ChartOfAccountsId; }
    set ChartOfAccountsId(value: string) {
        if (this.EntityPM.ChartOfAccountsId != value) {
            this.EntityPM.ChartOfAccountsId = value;
        }
    }

    get DefaultGLAccountId() { return this.EntityPM.ChartOfAccountsId; }
    set DefaultGLAccountId(value: string) {
        if (this.EntityPM.ChartOfAccountsId != value) {
            this.EntityPM.ChartOfAccountsId = value;
        }
    }
  
    chartOfAccount: boolean;
    get ChartOfAccount() { return this.chartOfAccount; }
    set ChartOfAccount(value: boolean) {
      
        this.chartOfAccount = value;
        if (value) {
            this.DefaultGLAccount = false;
            this.GLAccount = false;
            this.UIProperties.SetEnabled("DefaultGLAccountId", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
            this.EntityPM.RevaluationEnabled = false;
        }
         //   this.SetChartOfAccount(value);
      
    }


    defaultGLAccount: boolean= true;
    get DefaultGLAccount() { return this.defaultGLAccount; }
    set DefaultGLAccount(value: boolean) {
       
        this.defaultGLAccount = value;
        if (value) {
            this.ChartOfAccount = false;
            this.GLAccount = false;
            this.UIProperties.SetEnabled("DefaultGLAccountId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
          
            this.EntityPM.RevaluationEnabled = true;
           
        }
         //   this.SetDefaultGLAccount(value);
        
    }

    gLAccount: boolean;
    get GLAccount() { return this.gLAccount; }
    set GLAccount(value: boolean) {
    
        this.gLAccount = value;
        if (value) {
            this.DefaultGLAccount = false;
            this.ChartOfAccount = false;
            this.UIProperties.SetEnabled("DefaultGLAccountId", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("GLAccountId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
            this.EntityPM.RevaluationEnabled = false;
        }
        //    this.SetGLAccount(value);
       
    }


    SetDefaultGLAccount(value: boolean) {
      
        this.DefaultGLAccount = value;
        
         
      
    }

    SetChartOfAccount(value: boolean) {
       
            this.ChartOfAccount = value;
           
       
    }

    SetGLAccount(value: boolean) {
        this.GLAccount = value;
     
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        // Custom Validation
        if (this.RevaluationDate == null) {
            errors.push(this.GetRequierdFieldErrorText("Revaluation.F.RevaluationDate"));
        }

        if (this.RevaluationsGLAccountId == null) {
            errors.push(this.GetRequierdFieldErrorText("Revaluation.F.RevaluationsGLAccountId"));
        }
        if  (this.GLAccount && this.GLAccountId == null) {
            errors.push(TextCodeTranslator.Translate("Revaluation.O.GLAccountForRevaluation"));
        }

        if (this.ChartOfAccount && this.ChartOfAccountsId == null) {
            errors.push(TextCodeTranslator.Translate("Revaluation.O.ChartAccountForRevaluation"));
        }
      if (this.RevaluationDate > DateTool.GetCurrentDateTimeAsUtc()) {
        errors.push(TextCodeTranslator.Translate("Revaluation.O.FutureDateIsNotAllowed"));
      }
       
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
        
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("");
        this.RevaluationService.insert(this.EntityPM).subscribe((Result:any) => {
          
            var mm: ServiceResponse = Result;
            if (!mm.HasError) {
                var entity = mm.Result;
                this.CurrentSession.CloseCurrentWindowEmit("ok");
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
