import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { QuoteHelper } from '../QuoteHelper'
import { timingSafeEqual } from 'crypto';
import { EditTabsComponent } from '../EditEntity/EditQuoteTabs.po';
import { QuoteActions } from '../EditEntity/QuoteActions';



export class NewQuote {
  private Helper: FieldsHelper;
  private Quotes: GeneralFunctions;
  private QuoteHepler: QuoteHelper;
  private EditQuoteTabs: EditTabsComponent;
  private QuoteActions: QuoteActions;


  constructor() {
    this.Helper = new FieldsHelper();
    this.Quotes = new GeneralFunctions();
    this.QuoteHepler = new QuoteHelper();
    this.EditQuoteTabs = new EditTabsComponent();
    this.QuoteActions = new QuoteActions();

  }

  DoOperations() {
    this.Quotes.GoToMainMenu('General.MH.CRM');
    this.Quotes.SelectMenuWorkSpaceTabs('CRMQUT');
    this.CreateQuote(browser.params.QuoteParams.Direction, browser.params.QuoteParams.TransportMode, browser.params.QuoteParams.ShipmentType, browser.params.QuoteParams.QuoteType);

  }

  CreateQuote(Direction: string, TransportMode: string, ShipmentType: string, QuoteType: string) {
    var EC = protractor.ExpectedConditions;
    this.QuoteHepler.CreateAndCloseNewQuote(Direction, TransportMode, ShipmentType);
    this.Helper.WaitByIdAndClick('NewQuote');
    this.QuoteHepler.SelectDicrctionTransportMode(Direction, TransportMode, ShipmentType);
    // if (TransportMode == 'A') {

    var QuoteNumber = this.Quotes.RandomNum();
    this.FillQuoteFields(QuoteNumber, TransportMode, Direction, ShipmentType, QuoteType);

    this.Helper.WaitWindowClosed();
    this.Helper.WaitBusyIndicator();

    this.Quotes.UseSearchBox('Quote_Search', QuoteNumber, 'LogitudeQuickSearchItem');
    this.EditQuoteTabs.EditTabs(QuoteNumber, ShipmentType, Direction, TransportMode, QuoteType);


    this.Helper.WaitByIdAndClick('Quote-Save');
    this.Helper.WaitEditComponentBusyIndicator();
    this.QuoteActions.CopyQuote();
    // this.QuoteActions.QuoteAccepted();

    // this.QuoteActions.BuildShipmentFromQuote();

    // browser.driver.sleep(5000)
    /* }
     else if ((TransportMode == 'O' || TransportMode == 'I') && QuoteType != '') {
       var QuoteNumber = this.Quotes.RandomNum();
       this.FillQuoteFields(QuoteNumber,  TransportMode ,Direction, QuoteType);
       this.Helper.WaitBusyIndicator();
 
       //this.GeneralFunction.UseSearchBox('Shipment_Search', QuoteNumber);
       //this.EditShipmentTabs.EditTabs(QuoteNumber, ShipmentLevelCode, ShipmentType,Direction);
     }*/

  }

  FillQuoteFields(QuoteNumber: string, TransportMode: string, Direction: string, ShipmentType: string, QuoteType: string) {
    var quotetypeBtn: any;
    var EC = protractor.ExpectedConditions;
    this.Helper.WaitByIdAndFill('Quote_CustomerId', 'TestShipper');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndFill('Quote_ConsigneeId', 'TestShipper');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    if (Direction == 'Export' || Direction == 'Domestic')
      this.Helper.WaitByIdAndFill('Quote_ShipperReference1', QuoteNumber);// test random number randomWholeNum
    else
      this.Helper.WaitByIdAndFill('Quote_ConsigneeReference1', QuoteNumber);// test random number randomWholeNum

    this.Helper.WaitByIdAndFill('Quote_IncotermId', 'CIF');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.QuoteHepler.SelectQuoteType(QuoteType);

    if (Direction == 'Domestic') {
      if (TransportMode == 'I') {
      }
      else {
        this.Helper.WaitByIdAndFill('Quote_FromPortId', 'eze');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.Helper.WaitByIdAndFill('Quote_ToPortId', 'eze');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      }
    }
    else {
      this.Helper.WaitByIdAndFill('Quote_FromPortId', 'eze');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      this.Helper.WaitByIdAndFill('Quote_ToPortId', 'mvd');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    }

    if (TransportMode == 'A') {
      this.Helper.WaitByIdAndFill('Quote_MoveTypeId', 'TestMoveTypeIdAirMTA');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      if (QuoteType == 'SpotRate') {
        this.Helper.WaitByIdAndFill('Quote_GrossWeight', '1000');
        this.Helper.WaitByIdAndFill('Quote_Volume', '3');
        this.Helper.WaitByIdAndFill('Quote_NumberOfPackages', '3');
      }
    }

    else if (TransportMode == 'O') {
      this.Helper.WaitByIdAndFill('Quote_MoveTypeId', 'TestMoveTypeIDOceanMTO');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      if (ShipmentType == 'FCL') {
        if (QuoteType == 'SpotRate') {
          this.Helper.WaitByIdAndFill('Quote_PackageType1Quantity', '1');
        }
        this.Helper.WaitByIdAndFill('Quote_PackageType1Id', '20bu');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

      }
      else {
        if (QuoteType == 'SpotRate') {
          this.Helper.WaitByIdAndFill('Quote_GrossWeight', '1000');
          this.Helper.WaitByIdAndFill('Quote_Volume', '3');
          this.Helper.WaitByIdAndFill('Quote_NumberOfPackages', '3');
        }
      }

    }

    else if (TransportMode == 'I') {
      this.Helper.WaitByIdAndFill('Quote_MoveTypeId', 'TestMoveTypeIdInlandMTI');
      this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      if (ShipmentType == 'FTL') {
        if (QuoteType == 'SpotRate') {
          this.Helper.WaitByIdAndFill('Quote_PackageType1Quantity', '1');
        }
        this.Helper.WaitByIdAndFill('Quote_PackageType1Id', '20bu');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
      }
      else {
        if (QuoteType == 'SpotRate') {
          this.Helper.WaitByIdAndFill('Quote_GrossWeight', '1000');
          this.Helper.WaitByIdAndFill('Quote_Volume', '3');
          this.Helper.WaitByIdAndFill('Quote_NumberOfPackages', '3');
        }
      }
    }
    this.Helper.WaitByIdAndClick('CreateQuote');
  }
}






