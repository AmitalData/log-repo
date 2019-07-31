import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../Helpers/FieldsHelper';
export class LoginComp {
  private Helper: FieldsHelper;
  constructor() {
    this.Helper = new FieldsHelper();
  }
  navigateTo(url: string) {
    return browser.get(url + '?Menu=protractor');
  }

  DoLogin(userName: string, password: string) {
    browser.ignoreSynchronization = true;

    this.Helper.WaitByIdAndFill('Email', userName);
    this.Helper.WaitByIdAndFill('Password', password);
    this.Helper.ButtonClick('cmdLogin');

  }
}
