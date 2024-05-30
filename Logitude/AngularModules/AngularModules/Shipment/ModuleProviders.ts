
import {AccountingInformationIdentifierListService} from './Services/StandardLists/AccountingInformationIdentifierListService';
import {AWBChargesCodeListService} from './Services/StandardLists/AWBChargesCodeListService';
import {AWBCustomsInformationListService} from './Services/StandardLists/AWBCustomsInformationListService';
import {AWBInformationListService} from './Services/StandardLists/AWBInformationListService';
import {MessagingStockListService} from './Services/StandardLists/MessagingStockListService';
import {AWBSpecialHandlingCodeListService} from './Services/StandardLists/AWBSpecialHandlingCodeListService';
import {ManifestStatusListService} from './Services/StandardLists/ManifestStatusListService';
import {OtherParticipantIdListService} from './Services/StandardLists/OtherParticipantIdListService';
import {ShipmentCustomerTypeListService} from './Services/StandardLists/ShipmentCustomerTypeListService';
import {ShipmentLevelListService} from './Services/StandardLists/ShipmentLevelListService';
import {ShipmentListService} from './Services/StandardLists/ShipmentListService';
import {LogBoxShipmentListService} from './Services/StandardLists/LogBoxShipmentListService';
import {ShipmentPayableStatusListService} from './Services/StandardLists/ShipmentPayableStatusListService';
import {ShipmentReceivableStatusListService} from './Services/StandardLists/ShipmentReceivableStatusListService';
import {ShipmentTypeListService} from './Services/StandardLists/ShipmentTypeListService';
import {SpecialServicesTypeListService} from './Services/StandardLists/SpecialServicesTypeListService';
import {ShipmentFollowUpListService} from './Services/StandardLists/ShipmentFollowUpListService';
import {MessagingStockPMService} from './Services/StandardPMs/MessagingStockPMService';
import {ShipmentPMService} from './Services/StandardPMs/ShipmentPMService';
import {SpecialServicesTypePMService} from './Services/StandardPMs/SpecialServicesTypePMService';
import {MessagingStockMenuButtonsHandler} from './Components/MenuButtons/MessagingStockMenuButtonsHandler';
import {ShipmentMenuButtonsHandler} from './Components/MenuButtons/ShipmentMenuButtonsHandler';
import {FBLStockExtenedPMService} from './Services/ExtendedPMs/FBLStockExtenedPMService';
import {ContainerFollowUpPMService} from './Services/StandardPMs/ContainerFollowUpPMService';
import {ContainerFollowUpListService} from './Services/StandardLists/ContainerFollowUpListService';
import {CustomsTransmissionsStatusListService} from './Services/StandardLists/CustomsTransmissionsStatusListService';
import {OBLTypeListService} from './Services/StandardLists/OBLTypeListService';
import {PickUpDeliveryTransportModeListService} from './Services/StandardLists/PickUpDeliveryTransportModeListService';
import {INTTRADocumentTypeListService} from './Services/StandardLists/INTTRADocumentTypeListService';
import { HarmonizeCodeListService } from './Services/StandardLists/HarmonizeCodeListService';
import { CustomsTransferHeaderListService } from './Services/StandardLists/CustomsTransferHeaderListService';
import { CustomsTransferHeaderPMService } from './Services/StandardPMs/CustomsTransferHeaderPMService';
import { AWBAdditionalHandlingInfoListService } from './Services/StandardLists/AWBAdditionalHandlingInfoListService';
import { ShipmentSubTypeListService } from './Services/StandardLists/ShipmentSubTypeListService';
import { ShipmentSubTypePMService } from './Services/StandardPMs/ShipmentSubTypePMService';
import { ContainerListService } from './Services/StandardLists/ContainerListService';
import { ContainerPMService } from './Services/StandardPMs/ContainerPMService';
import { ContainerMenuButtonsHandler } from './Components/MenuButtons/ContainerMenuButtonsHandler';
import { ContainerTrackingProviderListService } from './Services/StandardLists/ContainerTrackingProvidersListService';
import { ContainerTrackingProviderPMService } from './Services/StandardPMs/ContainerTrackingProvidersPMService';
import { ContainerStatusSourceListService } from './Services/StandardLists/ContainerStatusSourceListService';
import { ContainerTrackingProviderMenuButtonsHandler } from './Components/MenuButtons/ContainerTrackingProviderMenuButtonsHandler';
import { AWBAdditionalHandlingInfoPMService } from './Services/StandardPMs/AWBAdditionalHandlingInfoPMService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            // List
            case "AccountingInformationIdentifierListService": { myResult = new AccountingInformationIdentifierListService(); break; }
            case "AWBChargesCodeListService": { myResult = new AWBChargesCodeListService(); break; }
            case "AWBCustomsInformationListService": { myResult = new AWBCustomsInformationListService(); break; }
            case "AWBInformationListService": { myResult = new AWBInformationListService(); break; }
            case "MessagingStockListService": { myResult = new MessagingStockListService(); break; }
            case "AWBSpecialHandlingCodeListService": { myResult = new AWBSpecialHandlingCodeListService(); break; }
            case "ManifestStatusListService": { myResult = new ManifestStatusListService(); break; }
            case "OtherParticipantIdListService": { myResult = new OtherParticipantIdListService(); break; }
            case "ShipmentCustomerTypeListService": { myResult = new ShipmentCustomerTypeListService(); break; }
            case "ShipmentLevelListService": { myResult = new ShipmentLevelListService(); break; }
            case "ShipmentListService": { myResult = new ShipmentListService(); break; }
            case "LogBoxShipmentListService": { myResult = new LogBoxShipmentListService(); break; }
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
            case "HarmonizeCodeListService": { myResult = new HarmonizeCodeListService(); break; }
            case "CustomsTransferHeaderListService": { myResult = new CustomsTransferHeaderListService(); break; }
            case "AWBAdditionalHandlingInfoListService": { myResult = new AWBAdditionalHandlingInfoListService(); break; }
            case "ShipmentSubTypeListService": { myResult = new ShipmentSubTypeListService(); break; }    
            case "ContainerTrackingProviderListService": { myResult = new ContainerTrackingProviderListService(); break; }    
            case "ContainerStatusSourceListService": { myResult = new ContainerStatusSourceListService(); break; }    

            // PM
            case "MessagingStockPMService": { myResult = new MessagingStockPMService(); break; }
            case "ShipmentPMService": { myResult = new ShipmentPMService(); break; }
            case "SpecialServicesTypePMService": { myResult = new SpecialServicesTypePMService(); break; }
            case "FBLStockExtenedPMService": { myResult = new FBLStockExtenedPMService(); break; }
            case "CustomsTransferHeaderPMService": { myResult = new CustomsTransferHeaderPMService(); break; }
            case "ShipmentSubTypePMService": { myResult = new ShipmentSubTypePMService(); break; }
            case "ContainerListService": { myResult = new ContainerListService(); break; }
            case "ContainerPMService": { myResult = new ContainerPMService(); break; }
            case "ContainerTrackingProviderPMService": { myResult = new ContainerTrackingProviderPMService(); break; }
            case "AWBAdditionalHandlingInfoPMService": { myResult = new AWBAdditionalHandlingInfoPMService(); break; }

            // Handler
            case "MessagingStockMenuButtonsHandler": { myResult = new MessagingStockMenuButtonsHandler(); break; }
            case "ShipmentMenuButtonsHandler": { myResult = new ShipmentMenuButtonsHandler(); break; }
            case "ContainerMenuButtonsHandler": { myResult = new ContainerMenuButtonsHandler(); break; }
            case "ContainerTrackingProviderMenuButtonsHandler": { myResult = new ContainerTrackingProviderMenuButtonsHandler(); break; }                
        }

        return myResult;
    }
}
