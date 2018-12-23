import { browser, by, element, WebDriver, protractor } from 'protractor';

export class FieldsHelper {
  constructor() {

  }

  private GetObjectById(value: string) {
    return element(by.id(value));
  }
  ClearFieldAndType(Input, keys) {
    Input.clear();
    Input.sendKeys(keys);

  }

  private GetObjectByCssString(className: string, Text: string) {
    return element(by.cssContainingText(className, Text));
  }


  WaitByCss(className: string) {
    browser.wait(function () {
      if (browser.isElementPresent(by.css(className))) {
        return true;
      }
    }, 10000000000000000);
  }

  WaitById(Id: string) {
    browser.wait(function () {
      if (browser.isElementPresent(by.Id(Id))) {
        console.log(Id);
        return true;
      }
    }, 100000000);
  }

  // WaitByCssStringAndClick(className: string, Text: string) {
  //   var EC = protractor.ExpectedConditions;
  // browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className,Text))), 2000000).then(a=>{
  //   var Button = element(by.cssContainingText(className, Text)).click();
  // });
  // }

  WaitByCssStringAndClick(className: string, Text: string) {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text))), 100000000).then(a => {
      var Button = element(by.cssContainingText(className, Text)).click();

    });
  }

  WaitByIdAndClick(Id: string) {
    var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000000).then(a=>{
     element(by.id(Id)).click();

    });

  }

  // WaitBusyIndicator(){

  //   var EC = protractor.ExpectedConditions;

  //     browser.wait(EC.invisibilityOf(element(by.css(".BusyIndicatorControl"))), 100000).then(a=>{
  //     });
  // }
  WaitBusyIndicator() {

  var EC = protractor.ExpectedConditions;

    browser.wait(EC.invisibilityOf(element(by.id("BusyIndecator"))), 100000).then(a=>{
    });
  }


  WaitWindowClosed() {

  var EC = protractor.ExpectedConditions;

    browser.wait(EC.invisibilityOf(element(by.css(".LogitudeWindow"))), 100000000).then(a=>{
    });
  }

  // WaitByIdAndFill(Id: string, Value: string) {
  //   var EC = protractor.ExpectedConditions;
  //   browser.wait(EC.elementToBeClickable(element(by.id(Id))), 10000000).then(a => {
  //     var input = element(by.id(Id));
  //     input.clear();
  //     input.sendKeys(Value);

  //   });

  // }
  WaitByIdAndFill(Id: string, Value: string) {
    var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000000).then(a => {
      var input = element(by.id(Id));
      input.clear();
      browser.wait(EC.textToBePresentInElementValue(element(by.id(Id)), ''), 10000000).then(a => { });
     input.clear();
      input.sendKeys(Value);

    });

  }

  WaitByCssAndClick_SelectItemFromList(className: string, index: number) {
    var EC = protractor.ExpectedConditions;
      browser.wait(EC.visibilityOf(element(by.css(className))), 100000000).then(a => {
      var shipment = element(by.css(className)).all(by.tagName('li'));
      shipment.get(index).click();
      console.log('Razan');
    });
  }
  WaitByCssAndClick_FromTagInsideList(className: string, index: number) {// the item exists in a tag inside li
    var EC = protractor.ExpectedConditions;

      browser.wait(EC.elementToBeClickable(element(by.css(className))), 100000000).then(a => {
      element.all(by.css(className)).get(index).click();
    });
  }

  WaitByCssButtonClick(className: string, Text: string) {

    var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.buttonText(Text))), 100000000).then(a => {
      var button = element(by.buttonText(Text)).click();
    });

  }


  public SetTextFieldValue(keys, value: string) {
    return this.ClearFieldAndType(this.GetObjectById(value), keys);
    // return element(by.id(value)).sendKeys(keys);      

  }
  public ButtonClick(BtnId: string) {
    var EC = protractor.ExpectedConditions;
      browser.wait(EC.elementToBeClickable(element(by.id(BtnId))), 100000000).then(a => {
      var button = element(by.id(BtnId)).click();
      console.log("Button Clicked");
    });
  }


  public ButtonClickByCss(className: string, Text: string) {
    return this.GetObjectByCssString(className, Text).click();
  }

}

