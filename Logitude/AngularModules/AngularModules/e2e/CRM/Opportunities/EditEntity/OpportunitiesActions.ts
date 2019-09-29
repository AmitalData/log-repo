
import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { QuoteHelper } from '../../Quotes/QuoteHelper';

export class OpportunityActions {
  private Helper: FieldsHelper;
  private quoteHelper: QuoteHelper;
  constructor() {
    this.Helper = new FieldsHelper();
    this.quoteHelper = new QuoteHelper();
  }
  public OpportunityActions() {
    this.CloseAsWon();
    this.ReOpen('Opportunity_StageId_1');
    this.CloseAsLost();
    this.ReOpen('Opportunity_StageId_2');
    this.Copy();
    this.Cancel("MenuButtons_1");
  }
  CloseAsWon() {
    this.Helper.WaitByIdAndClick('Opportunity.B.CloseAsWon');
    this.Helper.WaitByIdAndFill('Opportunity_ClosingDescription', 'Closed As Won');
    this.Helper.WaitByIdAndClick('Ok-CloseAsWon');
    this.Helper.WaitBusyIndicator();
  }
  CloseAsLost() {
    this.Helper.WaitByIdAndClick('Opportunity.B.CloseAsLost');
    this.Helper.WaitByIdAndFill('Opportunity_ClosingReasonId', 'Lost');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndFill('Opportunity_ClosingDescription', 'Close As Lost');
    this.Helper.WaitByIdAndClick('Ok-CloseAsWon');
    this.Helper.WaitBusyIndicator();
  }
  ReOpen(StageId: string) {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Opportunity.B.ReOpen');
    this.Helper.WaitByIdAndFill(StageId, 'qua');
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
    this.Helper.WaitByIdAndClick('OK-ReOpen');
    this.Helper.WaitBusyIndicator();

  }
  Copy() {
    this.Helper.WaitByIdAndClick('MenuButtons');
    this.Helper.WaitByIdAndClick('Opportunity.B.Copy');
    this.Helper.WaitByIdAndClick('Ok-AddOpportunity');
    this.Helper.WaitBusyIndicator();

  }
  Cancel(MenuButtonsId: string) {

    this.Helper.WaitByIdAndClick(MenuButtonsId);
    this.Helper.WaitByIdAndClick('Opportunity.B.Cancel_1');
    this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');
    this.Helper.WaitBusyIndicator();
  }
  AddQuoteFromOpportunity() {
    this.quoteHelper.SelectDicrctionTransportMode('Export', 'A', '');

    this.Helper.WaitByIdAndFill('Quote_FromPortId', 'eze');
    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_FromPortId', 'eze');

    this.Helper.WaitByIdAndFill('Quote_ToPortId', 'eze');
    this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_ToPortId', 'eze');

    this.Helper.WaitByIdAndClick('CreateQuote');
    this.Helper.WaitBusyIndicator();
    this.Helper.WaitWindowClosed();
  }
}



