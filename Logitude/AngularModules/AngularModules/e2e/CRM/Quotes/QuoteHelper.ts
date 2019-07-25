import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from './../../Helpers/FieldsHelper';

export class QuoteHelper {
  private Helper: FieldsHelper;


  constructor() {
    this.Helper = new FieldsHelper();

  }

  SelectDicrctionTransportMode( Direction: string, TransportMode: string) {

  }

  CreateAndCloseNewQuote(MasterDirectType: string, CancelBtnId: string,Direction:string,TransportMode:string) {
    this.Helper.WaitByIdAndClick('NewQuote');
    this.SelectDicrctionTransportMode( Direction,TransportMode);

    this.Helper.WaitByIdAndClick("CancelQuote");
  }

}