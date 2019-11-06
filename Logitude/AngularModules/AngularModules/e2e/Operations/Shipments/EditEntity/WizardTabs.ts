import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { ShipmentHelper } from '../ShipmentHelper';

export class WizardTabComponent {
    private Helper: FieldsHelper;
    private generalFun: GeneralFunctions;
    private shipHelper: ShipmentHelper = new ShipmentHelper();
    constructor() {
        this.Helper = new FieldsHelper();
        this.generalFun = new GeneralFunctions();
    }
    GeneralTab(LogitudeShipType: string) {
    }

    FillPartnersTab(shipperRef1: string, LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('PAR');
        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_ShipperId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ShipperId', 'TestShipper');

            this.Helper.WaitByIdAndFill('Shipment_ShipperReference1', shipperRef1);

            this.Helper.WaitByIdAndFill('Shipment_ConsigneeId', 'TestConsignee');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ConsigneeId', 'TestConsignee');

            this.Helper.WaitByIdAndFill('Shipment_Notify1Id', 'TestConsignee');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Notify1Id', 'TestConsignee');
        }
        else if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_ShipperId', 'TestAgentExport1');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_ShipperId', 'TestAgentExport1');

            this.Helper.WaitByIdAndFill('Master_ShipperReference1', shipperRef1);

            this.Helper.WaitByIdAndFill('Master_ConsigneeId', 'TestAgentExport1');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_ConsigneeId', 'TestAgentExport1');

            this.Helper.WaitByIdAndFill('Master_Notify1Id', 'TestAgentExport1');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_Notify1Id', 'TestAgentExport1');
        }
    }
    FillRoutingTab(shipperRef1: string, LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('ROU');
        if (LogitudeWizardType == 'D') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'eze');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageCarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageCarrierNumber', '225');

            this.Helper.WaitByIdAndFill('date_Shipment_MAWBOBLDate', '11');
            this.Helper.WaitByIdAndFill('date_Shipment_MainCarriageETD', '+1');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1FromPortId', 'JFK');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1FromPortId', 'JFK');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment1CarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment1CarrierNumber', '226');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2FromPortId', 'LHR');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2FromPortId', 'LHR');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_Transshipment2CarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Shipment_Transshipment2CarrierNumber', '227');

            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFinalDestinationPortId', 'JFK');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'JFK');

            this.shipHelper.AddAirlineStock('wizard');
        }
        else if (LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_MainCarriageFromPortId', 'eze');

            this.Helper.WaitByIdAndFill('Shipment_ToPortId', 'JFK');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_ToPortId', 'JFK');

            this.Helper.WaitByIdAndFill('Shipment_House', 'House #' + shipperRef1);
        }
        else if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_MainCarriageFromPortId', 'eze');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageFromPortId', 'eze');

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageCarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Master_MainCarriageCarrierNumber', '225');

            this.Helper.WaitByIdAndFill('date_Master_MAWBOBLDate', '11');
            this.Helper.WaitByIdAndFill('date_Master_MainCarriageETD', '+1');

            this.Helper.WaitByIdAndFill('Master_Transshipment1FromPortId', 'JFK');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_Transshipment1FromPortId', 'JFK');

            this.Helper.WaitByIdAndFill('Master_Transshipment1CarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_Transshipment1CarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Master_Transshipment1CarrierNumber', '226');

            this.Helper.WaitByIdAndFill('Master_Transshipment2FromPortId', 'LHR');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_Transshipment2FromPortId', 'LHR');

            this.Helper.WaitByIdAndFill('Master_Transshipment2CarrierId', 'BA');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_Transshipment2CarrierId', 'BA');

            this.Helper.WaitByIdAndFill('Master_Transshipment2CarrierNumber', '227');

            this.Helper.WaitByIdAndFill('Master_MainCarriageFinalDestinationPortId', 'JFK');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_MainCarriageFinalDestinationPortId', 'JFK');

            // ------------------------------------------------------ Stocks -------------------------------------------------------------
            this.shipHelper.AddAirlineStock('wizard');
            // --------------------------------------------------- End of Stocks ---------------------------------------------------------
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
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentAWBPrintOnly_IATACodeId', 'a');

        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_DueTypeCode', 'ag');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'ShipmentAWBPrintOnly_DueTypeCode', 'ag');

        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_Quantity', '10');
        this.Helper.WaitByIdAndFill('ShipmentAWBPrintOnly_UnitPrice', '20');

        this.Helper.WaitByIdAndClick('OkAddCharge');
    }
    FillRADetails(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('RAD');
        if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_KnownConsignorNumber', '111');
            this.Helper.WaitByIdAndFill('Master_ColoaderRANumber', '222');
            this.Helper.WaitByIdAndFill('Master_AdditionalHandlingInfo', 'Handling Information ..');
        } else {
            this.Helper.WaitByIdAndFill('Shipment_KnownConsignorNumber', '111');
            this.Helper.WaitByIdAndFill('Shipment_ColoaderRANumber', '222');
            this.Helper.WaitByIdAndFill('Shipment_AdditionalHandlingInfo', 'Handling Information ..');
        }
    }
    FillGeneralDetailsTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('GEN');
        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_AWBAccountingInformation', 'Accounting information');
            this.Helper.WaitByIdAndFill('Shipment_AWBHandlingInformation', 'Handling information ');
            this.Helper.WaitByIdAndFill('Shipment_AWBComments', 'AWB Comment .. ');
        }
        else if (LogitudeWizardType == 'M') {
            //Accounting Information
            this.Helper.WaitByIdAndClick('AdvancedFWBBtn');

            this.Helper.WaitByIdAndFill('Master_AccountingInformationIdentifierCode1', 'CRD');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_AccountingInformationIdentifierCode1', 'CRD');
            this.Helper.WaitByIdAndFill('Master_AccountingInformation1', 'Credit Card Expiry Date ');

            this.Helper.WaitByIdAndFill('Master_AccountingInformationIdentifierCode2', 'CRN');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_AccountingInformationIdentifierCode2', 'CRN');
            this.Helper.WaitByIdAndFill('Master_AccountingInformation2', 'Credit Card  Number ');

            this.Helper.WaitByIdAndClick('OkAdvanceBtn');

            this.Helper.WaitByIdAndFill('Master_AWBHandlingInformation', 'Handling Information - Master');

            // Comment - AdvancedComment 
            this.Helper.WaitByIdAndClick('AdvancedAWBBtn');
            this.Helper.WaitByIdAndFill('Shipment_AWBComments', 'Comment Field .. ');
            this.Helper.WaitByIdAndFill('Shipment_AWBPrintingComments', 'Advnaced comment field .. ');
            this.Helper.WaitByIdAndClick('OKAdvanceCommandBtn');

            //Special Handling Codes
            this.Helper.WaitByIdAndFill('Master_AWBSpecialHandlingCodeId1', 'AVI');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_AWBSpecialHandlingCodeId1', 'AVI');

            this.Helper.WaitByIdAndFill('Master_AWBSpecialHandlingCodeId2', 'SHL');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_AWBSpecialHandlingCodeId2', 'SHL');
        }
    }
    FillOCITab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('OCI');

        this.Helper.WaitByIdAndFill('AWBOCI_CountryId', 'us');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'AWBOCI_CountryId', 'us');

        this.Helper.WaitByIdAndFill('AWBOCI_AWBInformationCode', 'abi');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'AWBOCI_AWBInformationCode', 'abi');

        this.Helper.WaitByIdAndFill('AWBOCI_AWBCustomsInformationCode', 'AC');
        this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'AWBOCI_AWBCustomsInformationCode', 'AC');

        this.Helper.WaitByIdAndFill('AWBOCI_SupplementaryCustomsInfo', 'Supplimentery 1 ');
    }
    FillOtherPartnersTab(LogitudeWizardType: string) {
        this.Helper.WaitByIdAndClick('OTP');
        if (LogitudeWizardType == 'D' || LogitudeWizardType == 'H') {
            this.Helper.WaitByIdAndFill('Shipment_NominatedHandlingPartyId', 'TestShipper');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_NominatedHandlingPartyId', 'TestShipper');

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantIdCode1', 'AGT');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Shipment_OtherParticipantIdCode1', 'AGT');

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationCode1', 'AGT');
            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationName1', 'Participant Name : Agent');
            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationPortCode1', 'NAB');

            this.Helper.WaitByIdAndFill('Shipment_OtherParticipantInformationReference1', 'Participant Ref');
        }
        else if (LogitudeWizardType == 'M') {
            this.Helper.WaitByIdAndFill('Master_NominatedHandlingPartyId', 'TestAgent');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_NominatedHandlingPartyId', 'TestAgent');

            this.Helper.WaitByIdAndFill('Master_OtherParticipantIdCode1', 'AGT');
            this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0, 'Master_OtherParticipantIdCode1', 'AGT');

            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationCode1', 'AGT');
            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationName1', 'Participant Name : Agent');
            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationPortCode1', 'NAB');

            this.Helper.WaitByIdAndFill('Master_OtherParticipantInformationReference1', 'Participant Ref');
        }
    }
}

