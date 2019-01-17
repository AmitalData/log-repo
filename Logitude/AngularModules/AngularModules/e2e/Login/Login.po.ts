import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
export class LoginComp {
  private Helper: FieldsHelper;
  constructor() {
    this.Helper = new FieldsHelper();
  }
  navigateTo() {
    // return browser.get('http://test.logitudeworld.com/staging?Menu=protractor');
    return browser.get('http://test.logitudeworld.com/test?Menu=protractor');
    // return browser.get('http://localhost:4200?Menu=protractor');
    
  }
  DoLogin() {
    browser.ignoreSynchronization = true;
    //this.Helper.WaitByIdAndFill('Email','angular@fnarsoft.com' );
    //this.Helper.WaitByIdAndFill('Password','1' );

    this.Helper.WaitByIdAndFill('Email','razan@razancompany.com' );
    this.Helper.WaitByIdAndFill('Password','!R123j456' );
    this.Helper.ButtonClick('cmdLogin');



    // // LOCALLLY
    // this.Helper.WaitByIdAndFill('Email','angular@fnarsoft.com' );

    // this.Helper.WaitByIdAndFill('Password','1' );
    // this.Helper.ButtonClick('cmdLogin');

  }
}
