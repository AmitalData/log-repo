import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { QuoteHelper } from '../QuoteHelper'
import { timingSafeEqual } from 'crypto';
import { EditTabsComponent } from '../EditEntity/EditQuoteTabs.po';




export class NewQuote {
    private Helper: FieldsHelper;
    private Quotes: GeneralFunctions;
    private QuoteHepler: QuoteHelper;
    private EditQuoteTabs: EditTabsComponent;



    constructor() {
        this.Helper = new FieldsHelper();
        this.Quotes = new GeneralFunctions();
        this.QuoteHepler = new QuoteHelper();
        this.EditQuoteTabs = new EditTabsComponent();


    }

    DoQuoteActions() {
        this.Quotes.GoToMainMenu('General.MH.CRM');
        this.Quotes.SelectMenuWorkSpaceTabs('CRMQUT');
        return this.CreateQuote(browser.params.QuoteParams.Direction, browser.params.QuoteParams.TransportMode, browser.params.QuoteParams.ShipmentType, browser.params.QuoteParams.QuoteType);

    }
    SearchForQuote(QuoteNumber: string) {
        this.Quotes.UseSearchBox('Quote_Search', QuoteNumber, 'LogitudeQuickSearchItem');
    }

    EditQuote(QuoteNumber: string) {

        this.EditQuoteTabs.EditTabs(QuoteNumber, browser.params.QuoteParams.ShipmentType, browser.params.QuoteParams.Direction, browser.params.QuoteParams.TransportMode, browser.params.QuoteParams.QuoteType);
        //this.EditShipment(shipperRef1,browser.params.ShipParams.ShipmentLevelCode, browser.params.ShipParams.Direction, browser.params.ShipParams.TransportMode, browser.params.ShipParams.ShipmentType);


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
        return QuoteNumber;
        // this.Quotes.UseSearchBox('Quote_Search', QuoteNumber, 'LogitudeQuickSearchItem');
        //this.EditQuoteTabs.EditTabs(QuoteNumber, ShipmentType, Direction, TransportMode, QuoteType);
        // // this.Helper.WaitByIdAndClick('Quote-Save');
        // // this.Helper.WaitEditComponentBusyIndicator();
        // this.QuoteActions.QuoteMenubuttonActions('copybuild', Direction, TransportMode, QuoteType);
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


        this.Helper.WaitByIdAndFill('Quote_ShipperId', 'TestShipper');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_ShipperId', 'TestShipper');
        this.Helper.WaitByIdAndFill('Quote_ShipperReference1', QuoteNumber);// test random number randomWholeNum

        this.Helper.WaitByIdAndFill('Quote_ConsigneeId', 'TestShipper');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_ConsigneeId', 'TestShipper');
        this.Helper.WaitByIdAndFill('Quote_ConsigneeReference1', QuoteNumber);// test random number randomWholeNum


        this.Helper.WaitByIdAndFill('Quote_IncotermId', 'CIF');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_IncotermId', 'CIF');
        this.QuoteHepler.SelectQuoteType(QuoteType);

        if (Direction == 'Domestic') {
            if (TransportMode == 'I') {
            }
            else {
                this.Helper.WaitByIdAndFill('Quote_FromPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_FromPortId', 'eze');
                // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
                this.Helper.WaitByIdAndFill('Quote_ToPortId', 'eze');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_ToPortId', 'eze');
                // this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            }
        }
        else {
            this.Helper.WaitByIdAndFill('Quote_FromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_FromPortId', 'eze');
            this.Helper.WaitByIdAndFill('Quote_ToPortId', 'mvd');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_ToPortId', 'mvd');
        }

        if (TransportMode == 'A') {
            this.Helper.WaitByIdAndFill('Quote_MoveTypeId', 'TestMoveTypeIdAirMTA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_MoveTypeId', 'TestMoveTypeIdAirMTA');

            if (QuoteType == 'SpotRate') {
                this.Helper.WaitByIdAndFill('Quote_GrossWeight', '1000');
                this.Helper.WaitByIdAndFill('Quote_Volume', '3');
                this.Helper.WaitByIdAndFill('Quote_NumberOfPackages', '3');
            }
        }

        else if (TransportMode == 'O') {
            this.Helper.WaitByIdAndFill('Quote_MoveTypeId', 'TestMoveTypeIDOceanMTO');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_MoveTypeId', 'TestMoveTypeIDOceanMTO');
            if (ShipmentType == 'FCL') {
                if (QuoteType == 'SpotRate') {
                    this.Helper.WaitByIdAndFill('Quote_PackageType1Quantity', '1');
                }
                this.Helper.WaitByIdAndFill('Quote_PackageType1Id', '20bu');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_PackageType1Id', '20bu');

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
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_MoveTypeId', 'TestMoveTypeIdInlandMTI');
            if (ShipmentType == 'FTL') {
                if (QuoteType == 'SpotRate') {
                    this.Helper.WaitByIdAndFill('Quote_PackageType1Quantity', '1');
                }
                this.Helper.WaitByIdAndFill('Quote_PackageType1Id', '20bu');
                this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Quote_PackageType1Id', '20bu');
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
