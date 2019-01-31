import {AccountingInformationIdentifierListService} from './Services/StandardLists/AccountingInformationIdentifierListService';
import {AWBChargesCodeListService} from './Services/StandardLists/AWBChargesCodeListService';
import {AWBCustomsInformationListService} from './Services/StandardLists/AWBCustomsInformationListService';
import {AWBInformationListService} from './Services/StandardLists/AWBInformationListService';
import {AWBMessagingStockListService} from './Services/StandardLists/AWBMessagingStockListService';
import {AWBSpecialHandlingCodeListService} from './Services/StandardLists/AWBSpecialHandlingCodeListService';
import {ManifestStatusListService} from './Services/StandardLists/ManifestStatusListService';
import {OtherParticipantIdListService} from './Services/StandardLists/OtherParticipantIdListService';
import {ShipmentCustomerTypeListService} from './Services/StandardLists/ShipmentCustomerTypeListService';
import {ShipmentLevelListService} from './Services/StandardLists/ShipmentLevelListService';
import {ShipmentListService} from './Services/StandardLists/ShipmentListService';
import {ShipmentPayableStatusListService} from './Services/StandardLists/ShipmentPayableStatusListService';
import {ShipmentReceivableStatusListService} from './Services/StandardLists/ShipmentReceivableStatusListService';
import {ShipmentTypeListService} from './Services/StandardLists/ShipmentTypeListService';
import {SpecialServicesTypeListService} from './Services/StandardLists/SpecialServicesTypeListService';
import {ShipmentFollowUpListService} from './Services/StandardLists/ShipmentFollowUpListService';
import {AWBMessagingStockPMService} from './Services/StandardPMs/AWBMessagingStockPMService';
import {ShipmentPMService} from './Services/StandardPMs/ShipmentPMService';
import {SpecialServicesTypePMService} from './Services/StandardPMs/SpecialServicesTypePMService';
import {AWBMessagingStockMenuButtonsHandler} from './Components/MenuButtons/AWBMessagingStockMenuButtonsHandler';
import {ShipmentMenuButtonsHandler} from './Components/MenuButtons/ShipmentMenuButtonsHandler';
import {FBLStockExtenedPMService} from './Services/ExtendedPMs/FBLStockExtenedPMService';
import {ContainerFollowUpPMService} from './Services/StandardPMs/ContainerFollowUpPMService';
import {ContainerFollowUpListService} from './Services/StandardLists/ContainerFollowUpListService';
import {CustomsTransmissionsStatusListService} from './Services/StandardLists/CustomsTransmissionsStatusListService';
import {OBLTypeListService} from './Services/StandardLists/OBLTypeListService';
import {PickUpDeliveryTransportModeListService} from './Services/StandardLists/PickUpDeliveryTransportModeListService';
import {INTTRADocumentTypeListService} from './Services/StandardLists/INTTRADocumentTypeListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            // List
            case "AccountingInformationIdentifierListService": { myResult = new AccountingInformationIdentifierListService(); break; }
            case "AWBChargesCodeListService": { myResult = new AWBChargesCodeListService(); break; }
            case "AWBCustomsInformationListService": { myResult = new AWBCustomsInformationListService(); break; }
            case "AWBInformationListService": { myResult = new AWBInformationListService(); break; }
            case "AWBMessagingStockListService": { myResult = new AWBMessagingStockListService(); break; }
            case "AWBSpecialHandlingCodeListService": { myResult = new AWBSpecialHandlingCodeListService(); break; }
            case "ManifestStatusListService": { myResult = new ManifestStatusListService(); break; }
            case "OtherParticipantIdListService": { myResult = new OtherParticipantIdListService(); break; }
            case "ShipmentCustomerTypeListService": { myResult = new ShipmentCustomerTypeListService(); break; }
            case "ShipmentLevelListService": { myResult = new ShipmentLevelListService(); break; }
            case "ShipmentListService": { myResult = new ShipmentListService(); break; }
            case "ShipmentPayableStatusListService": { myResult = new ShipmentPayableStatusListService(); break; }
            case "ShipmentReceivableStatusListService": { myResult = new ShipmentReceivableStatusListService(); break; }
            case "ShipmentTypeListService": { myResult = new ShipmentTypeListService(); break; }
            case "SpecialServicesTypeListService": { myResult = new SpecialServicesTypeListService(); break; }
            case "ShipmentFollowUpListService": { myResult = new ShipmentFollowUpListService(); break; }
            case "ContainerFollowUpPMService": { myResult = new ContainerFollowUpPMService(); break; }
            case "ContainerFollowUpListService": { myResult = new ContainerFollowUpListService(); break; }
            case "CustomsTransmissionsStatusListService": { myResult = new CustomsTransmissionsStatusListService(); break; }
            case "OBLTypeListService": { myResult = new OBLTypeListService(); break; }
            case "PickUpDeliveryTransportModeListService": { myResult = new PickUpDeliveryTransportModeListService(); break; }
            case "INTTRADocumentTypeListService": { myResult = new INTTRADocumentTypeListService(); break; }               
                
            // PM
            case "AWBMessagingStockPMService": { myResult = new AWBMessagingStockPMService(); break; }
            case "ShipmentPMService": { myResult = new ShipmentPMService(); break; }
            case "SpecialServicesTypePMService": { myResult = new SpecialServicesTypePMService(); break; }
            case "FBLStockExtenedPMService": { myResult = new FBLStockExtenedPMService(); break; }

            // Handler
            case "AWBMessagingStockMenuButtonsHandler": { myResult = new AWBMessagingStockMenuButtonsHandler(); break; }
            case "ShipmentMenuButtonsHandler": { myResult = new ShipmentMenuButtonsHandler(); break; }                           
        }

        return myResult;
    }
}