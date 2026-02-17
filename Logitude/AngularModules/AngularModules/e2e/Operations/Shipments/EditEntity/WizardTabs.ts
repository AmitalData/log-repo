import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class WizardTabComponent {

    private Helper: FieldsHelper;

    constructor() {
        this.Helper = new FieldsHelper();

    }
    GeneralTab(LogitudeShipType: string) {
    }

    FillPartnersTab(shipperRef1: string, LogitudeWizardType: string) {

        this.Helper.WaitByIdAndClick('PAR');

        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'razan');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', shipperRef1);

            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'razan cons');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


            this.Helper.WaitByIdAndFill('Shipment_Notify1Id', 'razannoti');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
        else if (LogitudeWizardType) {
            // element(by.id('Master_ShipperId')).clear();
            this.Helper.WaitByIdAndFill('Master_ShipperId', 'razan');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_ShipperReference1', shipperRef1);

            this.Helper.WaitByIdAndFill('Master_ConsigneeId', 'age');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


            this.Helper.WaitByIdAndFill('Master_Notify1Id', 'razannoti');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);


        }

    }
    FillRoutingTab(shipperRef1: string, LogitudeWizardType: string) {
        // var partners = this.Helper.WaitByCssStringAndClick('.InnerChild', 'Routings');
        this.Helper.WaitByIdAndClick('ROU');

        if (LogitudeWizardType == 'D') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFinalDestinationPortId', 'mvd');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'aerol');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '225');
            // this.Helper.WaitByCssButtonClick('.Button','Get from stock');
            // this.Helper.WaitByCssAndClick_FromTagInsideList('.SimpleGridViewRow',1)

            this.Helper.WaitByIdAndFill('date_Shipment_MAWBOBLDate', '11');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'am');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'air fr');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '226');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'fran');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'air');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '227');


        }
        else if (LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'am');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_ToPortId', 'abu');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_House', 'House #' + shipperRef1);

        }
        else if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_MainCarriageFinalDestinationPortId', 'mvd');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'aerol');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '225');

            this.Helper.WaitByIdAndClick('GetFromStockBtn');

            var EC = protractor.ExpectedConditions;
            browser.wait(EC.elementToBeClickable(element(by.css(".SimpleGridViewRow "))), 100000).then(a => {

            });

            element.all(by.css('.SimpleGridViewRow')).get(1).click();
            this.Helper.WaitByCssStringAndClick('.RedButton', 'Ok');
            this.Helper.WaitByIdAndClick('ConfirmWindow_Yes_0');

            this.Helper.WaitBusyIndicator();

            this.Helper.WaitByIdAndFill('Master_Transshipment1FromPortId', 'am');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_Transshipment1CarrierId', 'air fr');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_Transshipment1CarrierNumber', '226');

            this.Helper.WaitByIdAndFill('Master_Transshipment2FromPortId', 'fran');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_Transshipment2CarrierId', 'air');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 1);

            this.Helper.WaitByIdAndFill('Master_Transshipment2CarrierNumber', '227');

            this.Helper.WaitByIdAndFill('date_Master_MainCarriageETD', '30');

        }


    }

    FillPackagesTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('PAC');

        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('ShipmentPackage_Quantity', '1');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Length', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Width', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Height', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Weight', '120');
            this.Helper.WaitByIdAndFill('Shipment_DescriptionOfGoods', 'Protractor testing .. ');
        }
        else if (LogitudeWizardType == 'M') {

            this.Helper.WaitByIdAndClick('AddPackageBtn');

            this.Helper.WaitByIdAndFill('ShipmentPackage_Quantity', '1');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Length', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Width', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Height', '100');
            this.Helper.WaitByIdAndFill('ShipmentPackage_Weight', '120');

            this.Helper.WaitByIdAndClick('OKBtn');
            this.Helper.WaitByIdAndFill('Master_DescriptionOfGoods', 'Protractor testing .. ');
        }
    }
    FillFreightChargesTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('FRE');

        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_AWBChargeRate', '10');
        }
        else if (LogitudeWizardType == 'M') {

            this.Helper.WaitByIdAndFill('Master_AWBChargeRate', '2');
        }
    }
    FillOtherChargesTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('OTC');

        this.Helper.WaitByIdAndClick('AddCharge');

        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_IATACodeId', 'a');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_DueTypeCode', 'ag');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_Quantity', '10');
        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_UnitPrice', '20');

        this.Helper.WaitByCssButtonClick('.RedButton', 'Ok');

    }

    FillGeneralDetailsTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('GEN');

        // this.Helper.WaitByCssStringAndClick('.Button', 'Advanced');
        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_AWBAccountingInformation', 'Accounting information');
            this.Helper.WaitByIdAndFill('Shipment_AWBHandlingInformation', 'Handling information ');
            this.Helper.WaitByIdAndFill('Shipment_AWBComments', 'AWB Comment .. ');
            // browser.driver.sleep(3000);
        }
        else if (LogitudeWizardType == 'M') {
            //Accounting Information
            this.Helper.WaitByIdAndClick('AdvancedFWBBtn');

            this.Helper.WaitByIdAndFill('Master_AccountingInformationIdentifierCode1', 'c');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_AccountingInformation1', 'Accounting Information 1 ');

            this.Helper.WaitByIdAndFill('Master_AccountingInformationIdentifierCode2', 's');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
            this.Helper.WaitByIdAndFill('Master_AccountingInformation2', 'Accounting Information 2 ');

            this.Helper.WaitByIdAndClick('OkAdvanceBtn');
            // browser.driver.sleep(2000);
            // this.Helper.WaitByIdAndFill('Master_AWBAccountingInformation', 'Accounting information - Master');
            // browser.driver.sleep(2000);

            this.Helper.WaitByIdAndFill('Master_AWBHandlingInformation', 'Handling Information - Master');

            // Comment - AdvancedComment 
            this.Helper.WaitByIdAndClick('AdvancedAWBBtn');
            this.Helper.WaitByIdAndFill('Shipment_AWBComments', 'Comment Field .. ');
            this.Helper.WaitByIdAndFill('Shipment_AWBPrintingComments', 'Advnaced comment field .. ');
            this.Helper.WaitByIdAndClick('OKAdvanceCommandBtn');

            //Special Handling Codes
            this.Helper.WaitByIdAndFill('Master_AWBSpecialHandlingCodeId1', 's');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_AWBSpecialHandlingCodeId2', 'b');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        }
    }
    FillOCITab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('OCI');

        // Add OCI Line
        // this.Helper.WaitByIdAndClick('BtnAddLineOCI');


        this.Helper.WaitByIdAndFill('AWBOCI_CountryId', 'ad');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('AWBOCI_AWBInformationCode', 'abi');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('AWBOCI_AWBCustomsInformationCode', 'ac');
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

        this.Helper.WaitByIdAndFill('AWBOCI_SupplementaryCustomsInfo', 'Supplimentery 1 ');
    }
    FillOtherPartnersTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('OTP');
        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_NominatedHandlingPartyId', 'a');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantIdCode1', 's');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationCode1', 'Ptc');
            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationName1', 'Participant Name');
            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationPortCode1', 'NAB');

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationReference1', 'Participant Ref');
        }
        else if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_NominatedHandlingPartyId', 'a');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_OtherParticipantIdCode1', 's');
            this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationCode1', 'Ptc');
            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationName1', 'Participant Name');
            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationPortCode1', 'NAB');

            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationReference1', 'Participant Ref');
        }




    }
}

