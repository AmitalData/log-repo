

import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {GLAccountExtendedPMService}  from '../../../Services/ExtendedPMs/GLAccountExtendedPMService';
import {GLAccountPMService}  from '../../../Services/StandardPMs/GLAccountPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GLAccountAdditionalDataTabComponent} from './GLAccountAdditionalDataTabComponent';
import {CurrencyPM} from '../../../../Common/EntityPMs/CurrencyPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {GLAccountCurrencyExtendedPMService} from '../../../Services/ExtendedPMs/GLAccountCurrencyExtendedPMService';
import {GLAccountCurrencyPM} from '../../../EntityPMs/GLAccountCurrencyPM';


@Component({
    
    templateUrl: './NewConnectedGLAccountComponent.html',

})


export class NewConnectedGLAccountComponent extends BaseComponent {

    ObjectTableName: string = "GLAccount";
    DataContext: any = this;
    entityPM: GLAccountPM;
    accountPM: GLAccountPM;
    Parent: GLAccountAdditionalDataTabComponent;
    public ValidationErrorsList: string[] = [];
    FIELD_IS_REQUIERD: string;
    entityResourceService: EntityResourceService = new EntityResourceService();
    GLAccountPMService: GLAccountPMService = new GLAccountPMService();
    gLAccountCurrencyExtendedPMService: GLAccountCurrencyExtendedPMService = new GLAccountCurrencyExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    ReconcileMethod_Local: string = "0";
    ReconcileMethod_Foreign: string = "1";

    constructor() {
        super();
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }

    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {

            this.entityPM = args.EntityPM;
            this.Parent = args.Parent;
            this.accountPM = new GLAccountPM();
            this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            this.UIProperties.SetEnabled("DisplayNumber", "GLAccount", false);
        });

    }
    valid: boolean = true;
    currencyId: string;
    public get CurrencyId() { return this.accountPM.CurrencyId; }
    public set CurrencyId(value: string) {
        this.UIProperties.SetValidity("CurrencyId", "GLAccount", true, null);
        var currency = this.Parent.ConnectedGLAccounts.Collection.filter(d => d.CurrencyId == value)[0];
        if (currency) {
            this.valid = false;
            this.UIProperties.SetValidity("CurrencyId", "GLAccount", false, TextCodeTranslator.Translate("GLAccounts.O.CurrencyExists"));

        }
        else {
            this.valid = true;
        }
        this.accountPM.CurrencyId = value;
      
    }

    public get CurrencyCode() { return this.accountPM.CurrencyCode; }
    public set CurrencyCode(value: string) {
        this.accountPM.CurrencyCode = value;
    }
    public get DisplayNumber() { return this.entityPM.DisplayNumber; }


    currency: CurrencyPM;
    get Currency() { return this.currency; }
    set Currency(value: CurrencyPM) {

        if (this.currency != value) {
            this.currency = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CurrencyCode = value.Code;


        } else {
            this.CurrencyCode = null;
            this.CurrencyId = null;
        }
    }

    GetRequierdFieldErrorText(fieldName) {
         var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }
    OkButtonClicked() {
        var errors: string[] = [];
        if (!this.CurrencyId) {
     
            errors.push(this.GetRequierdFieldErrorText("GlAccount.F.CurrencyId"));
        }
        if (!this.valid) {
            errors.push(TextCodeTranslator.Translate("GLAccounts.O.CurrencyExists"));
        }
          this.ValidationErrorsList = errors;
          if (this.ValidationErrorsList.length == 0) {
              this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
              this.GLAccountPMService.get(this.entityPM.ControlAccountId).subscribe((response: ServiceResponse) => {
                  if (response) {
                      if (!response.HasError) {
                          if (response.Result) 
                              {

                                  this.accountPM.AccountTypeCode = this.entityPM.AccountTypeCode;
                                  this.accountPM.DisplayNumber = this.entityPM.DisplayNumber + "\\" + this.CurrencyCode;
                                  this.accountPM.LocalName = this.entityPM.LocalName + "\\" + this.CurrencyCode;
                                  this.accountPM.EnglishName = this.entityPM.EnglishName + "\\" + this.CurrencyCode;
                                  this.accountPM.IsMultiCurrency = false;
                                  this.accountPM.CurrencyId = this.CurrencyId;
                                  this.accountPM.RevenueExpenseType = "3";
                                  this.accountPM.CustomerGLAccountId = this.entityPM.Id;
                                  this.accountPM.IsControlAccount = false;
                                  this.accountPM.ChartOfAccountsId = this.entityPM.ChartOfAccountsId;
                                  this.accountPM.ChartOfAccountsTypeCode = response.Result.ChartOfAccountsTypeCode;
                              this.accountPM.ReconcileMethodCode = SessionLocator.TenantPM.CurrencyId == this.CurrencyId ? this.ReconcileMethod_Local : this.ReconcileMethod_Foreign;
                                  this.accountPM.AutomaticReconcileId = this.entityPM.AutomaticReconcileId;
                                  this.accountPM.ControlAccountId = this.entityPM.ControlAccountId;
                                  this.accountPM.Tenant = this.entityPM.Tenant;
                              this.accountPM.Type = "ADDED";
                              this.accountPM.CardsDataId = this.entityPM.CardsDataId;
                               //   this.accountPM.ParentAccountByCurrency = this.entityPM.in;



                                  this.GLAccountPMService.insert(this.accountPM).subscribe((response: ServiceResponse) => {

                                      if (response) {
                                          if (!response.HasError) {
                                              var glaccountCurrency: GLAccountCurrencyPM = new GLAccountCurrencyPM(this.accountPM);
                                              glaccountCurrency.CurrencyId = this.CurrencyId;
                                              glaccountCurrency.MainGLAccountId = this.entityPM.Id;

                                              glaccountCurrency.GLAccountId = response.Result.Id;

                                              glaccountCurrency.Tenant = this.entityPM.Tenant;
                                              this.gLAccountCurrencyExtendedPMService.insert(glaccountCurrency).subscribe((response: ServiceResponse) => {

                                                  if (response) {
                                                      if (!response.HasError) 
                                                      {
                                                            this.CurrentSession.StopBusyIndicator();
                                                          this.CurrentSession.CloseCurrentWindowEmit("ok");
                                                      }
                                                      else {
                                                            this.CurrentSession.StopBusyIndicator();
                                                          this.ValidationErrorsList = response.ErrorsArray;
                                                      }
                                                  }
                                              });

                                           //  this.CurrentSession.CloseCurrentWindow();
                                          }
                                          else {
                                                 this.CurrentSession.StopBusyIndicator();
                                              this.ValidationErrorsList = response.ErrorsArray;
                                          }
                                      }



                                      //  this.CurrentSession.StopBusyIndicator();

                                  });
                                 
                              }
                          }

                          else {
                                 this.CurrentSession.StopBusyIndicator();
                              errors.push(TextCodeTranslator.Translate("GLAccounts.O.ControlAccountNotFound"));
                              this.ValidationErrorsList = errors;
                          }
                      }
               
              });
            
        }
     

    }

    CancelButtonClicked() {
        this.accountPM = null;
        this.CurrentSession.CloseCurrentWindow();
    }



    SubmitChanges() {
       
    }
   
}
