
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
import {GeneralFunctions} from '../Helpers/GeneralFunctions';

export class CRMComp {
  private Helper: FieldsHelper;

  private CRMTab: GeneralFunctions;

  constructor() {
    this.Helper = new FieldsHelper();
    this.CRMTab = new GeneralFunctions();
  }
  DoCRM(CRMcomponent : string) {
    this.CRMTab.GoToMainMenu('General.MH.CRM');

    if(CRMcomponent=='Overview'){
        this.CRMTab.SelectMenuWorkSpaceTabs('CRMOVE');

    }
    else if(CRMcomponent=='Customers'){
        this.CRMTab.SelectMenuWorkSpaceTabs('CRMCUS');
    }
    else if(CRMcomponent=='Quotes'){
        this.CRMTab.SelectMenuWorkSpaceTabs('CRMQUT');
    }
    else if(CRMcomponent=='Activities'){
        this.CRMTab.SelectMenuWorkSpaceTabs('CRMACT');
        
    }
    else if(CRMcomponent=='Opportunities'){
        this.CRMTab.SelectMenuWorkSpaceTabs('CRMOPP');

    }

  }
}

