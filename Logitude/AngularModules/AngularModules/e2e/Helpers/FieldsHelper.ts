import { browser, by, element, WebDriver, protractor } from 'protractor';

export class FieldsHelper {
  constructor() {

  }

  WaitByCssStringAndClick(className: string, Text: string) {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.elementToBeClickable(element(by.cssContainingText(className, Text))), 100000000).then(a => {
      var Button = element(by.cssContainingText(className, Text)).click();

    });
  }

  WaitByIdAndClick(Id: string) {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.elementToBeClickable(element(by.id(Id))), 100000).then(a => {
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
    browser.wait(EC.invisibilityOf(element(by.id("BusyIndecator"))), 100000).then(a => {
    });
  }


  WaitWindowClosed() {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.invisibilityOf(element(by.css(".LogitudeWindow"))), 100000000).then(a => {
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
    browser.wait(EC.elementToBeClickable(element(by.buttonText(Text))), 100000).then(a => {
      var button = element(by.buttonText(Text)).click();
    });
  }
  
  public ButtonClick(BtnId: string) {
    var EC = protractor.ExpectedConditions;
    browser.wait(EC.elementToBeClickable(element(by.id(BtnId))), 100000000).then(a => {
      var button = element(by.id(BtnId)).click();
      console.log("Button Clicked");
    });
  }
}

