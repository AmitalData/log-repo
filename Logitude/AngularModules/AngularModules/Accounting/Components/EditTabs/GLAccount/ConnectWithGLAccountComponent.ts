import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {GLAccountExtendedPMService}  from '../../../Services/ExtendedPMs/GLAccountExtendedPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {GLAccountCurrencyExtendedPMService} from '../../../Services/ExtendedPMs/GLAccountCurrencyExtendedPMService';
import {GLAccountCurrencyPM} from '../../../EntityPMs/GLAccountCurrencyPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CardList } from '../../../../Common/EntityLists/CardList';

@Component({
    templateUrl: './ConnectWithGLAccountComponent.html',
})


export class ConnectWithGLAccountComponent extends BaseComponent {

    ObjectTableName: string = "GLAccountCurrency";
    DataContext: any = this;
    entityPM: GLAccountPM;
    ConnectedGLAccounts: any;
    TenantCurrency:string;
    GLAccountCurrencyPM: GLAccountCurrencyPM =new GLAccountCurrencyPM(null);
    public FIELD_IS_REQUIERD: string;
    public ValidationErrorsList: string[] = [];
    private gLAccountExtendedPMService = new GLAccountExtendedPMService();
    gLAccountCurrencyExtendedPMService: GLAccountCurrencyExtendedPMService = new GLAccountCurrencyExtendedPMService();
    public GLAccountCurrencyFilterItems: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
     }

     public get GLAccountId() { return this.GLAccountCurrencyPM.GLAccountId; }
     public set GLAccountId(value: string) {
         this.GLAccountCurrencyPM.GLAccountId = value;
     }

     public selectedGLAccount:GLAccountPM;
     public get SelectedGLAccount() { return this.selectedGLAccount; }
     public set SelectedGLAccount(value: GLAccountPM) {
         this.selectedGLAccount = value;
     }

     GetRequierdFieldErrorText(fieldName) {
        var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
       return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
   }

    SetWindowArgs(args: any) {
        this.entityPM = args.EntityPM;
        this.ConnectedGLAccounts= args.ConnectedGLAccounts;
        this.TenantCurrency= SessionLocator.TenantPM.CurrencyId;
        this.GLAccountCurrencyFilterItems = new ApiQueryFilters();
        this.GLAccountCurrencyFilterItems.addAdditionalFilter("GLAccountCurrencyFilter", this.TenantCurrency, null, null, "Equals", true, false, false, "string", false, true);
        this.GLAccountCurrencyFilterItems.addAdditionalFilter("Id", this.entityPM.Id, null, null, "NotEqual", true, false, false, "string", false, true);
        this.SetAccountTypeCodeFilter();
      }

      private SetAccountTypeCodeFilter(){
        this.GetConnectedCards(this.entityPM.Id).then((connectedCards: CardList[]) => {
             var firstConnectedCard = connectedCards[0];
             var PartnerTypeId = firstConnectedCard?firstConnectedCard.PartnerTypeId : null;
             this.AddAcountTypeFiler(PartnerTypeId);
        });
      }

      private AddAcountTypeFiler(PartnerTypeId:string){
        switch(PartnerTypeId){
            case"CS":{
              this.GLAccountCurrencyFilterItems.addAdditionalFilter("AccountTypeCode", "2", null, null, "Equals", false, false, false, "string", false, true);
                break;
            }
            case"AC":{
              this.GLAccountCurrencyFilterItems.addAdditionalFilter("AccountTypeCode", "3", "2","1" ,"Equals", false, false, false, "string", false, true);
              break;
            }
            case null: {
                this.GLAccountCurrencyFilterItems.addAdditionalFilter("AccountTypeCode", this.entityPM.AccountTypeCode, null, null, "Equals", false, false, false, "string", false, true);

                break;
            }
          default:{
              this.GLAccountCurrencyFilterItems.addAdditionalFilter("AccountTypeCode", "3", null, null, "Equals", false, false, false, "string", false, true);
              break;
          }
        }
    }

    GetConnectedCards(accountId: string)
    {
        return new Promise(resolve =>
        {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.gLAccountExtendedPMService.GetConnectedCardsForGLAccount(accountId)
                .subscribe((myResponse: ServiceResponse) =>
                {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    var connectedCards = myResponse.Result;
                    if (connectedCards)
                        resolve(connectedCards);
                });
        });
    }
  
    private ValidateGLAccountCurrency(){
        var errors: string[] = [];
        if (!this.GLAccountId) {
                errors.push(this.GetRequierdFieldErrorText("GLAccountCurrency.F.GLAccountId"));
        }
        else {  
          var ExistGLAccountSplited = this.ConnectedGLAccounts.filter(s=>s.CurrencyCode == this.selectedGLAccount.CurrencyCode);
          if(ExistGLAccountSplited && ExistGLAccountSplited.length > 0){
            errors.push(TextCodeTranslator.Translate("GLAccountCurrency.O.AlreadySplit")+" "+this.selectedGLAccount.CurrencyCode);
              }

          }

          this.ValidationErrorsList = errors;
      }
 

private CreateNewGLAccountCurrency(){
    var CurrencyGLAccount:GLAccountCurrencyPM = this.MappingAndGetCurrencyGlAccount();
    SessionLocator.SelectedSession.StartBusyIndicatorLoading();
    this.gLAccountCurrencyExtendedPMService.insert(CurrencyGLAccount).subscribe((response: ServiceResponse) => {

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

}

private MappingAndGetCurrencyGlAccount():GLAccountCurrencyPM{
    var glaccountCurrency: GLAccountCurrencyPM = new GLAccountCurrencyPM(null);
        glaccountCurrency.CurrencyId = this.selectedGLAccount.CurrencyId;
        glaccountCurrency.MainGLAccountId = this.entityPM.Id;
        glaccountCurrency.GLAccountId = this.selectedGLAccount.Id;
        glaccountCurrency.Tenant = this.entityPM.Tenant;

        return glaccountCurrency;
}

    OkButtonClicked() {
       this.ValidateGLAccountCurrency();  
       if (this.ValidationErrorsList.length == 0) {
           this.CreateNewGLAccountCurrency();
       }
    }


    CancelButtonClicked() {
         this.CurrentSession.CloseCurrentWindow();
    }



    SubmitChanges() {
       
    }

  
    
   
}
