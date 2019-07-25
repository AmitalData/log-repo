import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { QuoteHelper } from '../QuoteHelper'


export class NewQuote {
  private Helper: FieldsHelper;
  private Quotes: GeneralFunctions;
  private QuoteHepler : QuoteHelper;


  constructor() {
    this.Helper = new FieldsHelper();
    this.Quotes = new GeneralFunctions();
    this.QuoteHepler = new QuoteHelper();
   
  }
  DoOperations() {
    this.Quotes.GoToMainMenu('General.MH.CRM');
    this.Quotes.SelectMenuWorkSpaceTabs('CRMQUT');

    this.CreateQuote(browser.params.QuoteParams.Direction, browser.params.QuoteParams.TransportMode, browser.params.QuoteParams.QuoteType);
  
  }

  CreateQuote(Direction: string, TransportMode: string, ShipmentType: string) {
    this.QuoteHepler.CreateAndCloseNewQuote(Direction,TransportMode,ShipmentType);
    if (TransportMode == 'A') {
        
      var QuoteNumber = this.Quotes.RandomNum();
      this.FillQuoteFields(QuoteNumber,  TransportMode ,Direction, ShipmentType);
      this.Helper.WaitBusyIndicator();

      
     // this.GeneralFunction.UseSearchBox('Shipment_Search', QuoteNumber);
      //this.EditShipmentTabs.EditTabs(QuoteNumber, ShipmentLevelCode, ShipmentType,Direction);

    }
    else if ((TransportMode == 'O' || TransportMode == 'I') && ShipmentType != '') {
      var QuoteNumber = this.Quotes.RandomNum();
      this.FillQuoteFields(QuoteNumber,  TransportMode ,Direction, ShipmentType);
      this.Helper.WaitBusyIndicator();

      //this.GeneralFunction.UseSearchBox('Shipment_Search', QuoteNumber);
      //this.EditShipmentTabs.EditTabs(QuoteNumber, ShipmentLevelCode, ShipmentType,Direction);
    }

  }

  FillQuoteFields(QuoteNumber: string, TransportMode: string, Direction: string, ShipmentType: string) {

  }


}






