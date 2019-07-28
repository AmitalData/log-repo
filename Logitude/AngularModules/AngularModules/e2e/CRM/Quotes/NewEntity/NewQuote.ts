import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { QuoteHelper } from '../QuoteHelper'
import { timingSafeEqual } from 'crypto';


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

    this.CreateQuote(browser.params.QuoteParams.Direction, browser.params.QuoteParams.TransportMode,browser.params.QuoteParams.ShipmentType, browser.params.QuoteParams.QuoteType);
  
  }

  CreateQuote(Direction: string, TransportMode: string,ShipmentType:string, QuoteType: string) {
    this.QuoteHepler.CreateAndCloseNewQuote(Direction,TransportMode,ShipmentType);
    this.Helper.WaitByIdAndClick('NewQuote');
    this.QuoteHepler.SelectDicrctionTransportMode( Direction,TransportMode,ShipmentType);
   // if (TransportMode == 'A') {
        
      var QuoteNumber = this.Quotes.RandomNum();
      this.FillQuoteFields(QuoteNumber,  TransportMode ,Direction,ShipmentType, QuoteType);
      this.Helper.WaitBusyIndicator();

      
     // this.GeneralFunction.UseSearchBox('Shipment_Search', QuoteNumber);
      //this.EditShipmentTabs.EditTabs(QuoteNumber, ShipmentLevelCode, QuoteType,Direction);

   /* }
    else if ((TransportMode == 'O' || TransportMode == 'I') && QuoteType != '') {
      var QuoteNumber = this.Quotes.RandomNum();
      this.FillQuoteFields(QuoteNumber,  TransportMode ,Direction, QuoteType);
      this.Helper.WaitBusyIndicator();

      //this.GeneralFunction.UseSearchBox('Shipment_Search', QuoteNumber);
      //this.EditShipmentTabs.EditTabs(QuoteNumber, ShipmentLevelCode, ShipmentType,Direction);
    }*/

  }

  FillQuoteFields(QuoteNumber: string, TransportMode: string, Direction: string,ShipmentType:string, QuoteType: string) {
    var quotetypeBtn:any;
    var EC = protractor.ExpectedConditions;
    
    this.Helper.WaitByIdAndFill('Quote_CustomerId','TestShipper');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
  
    this.Helper.WaitByIdAndFill('Quote_IncotermId','CIF');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
   
    if(TransportMode == 'A'){
      this.Helper.WaitByIdAndFill('Quote_MoveTypeId','TestMoveTypeIdAirMTA');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndFill('Quote_FromPortId','eze');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndFill('Quote_ToPortId','mvd');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndFill('Quote_GrossWeight','1000');
      this.Helper.WaitByIdAndFill('Quote_Volume','3');
      this.Helper.WaitByIdAndFill('Quote_NumberOfPackages','3');
    }

    this.Helper.WaitByIdAndClick('CreateQuote');
    
    
  }


}






