import {LogBoxDocumentsComponent} from './Components/LogBox/LogBoxDocumentsComponent';
import {LogBoxMainComponent} from './Components/LogBox/LogBoxMainComponent';
import {EditLogBoxShipmentComponent} from './Components/LogBox/EditLogBoxShipmentComponent';
import {DigitalSignDocTypeComponent} from './Components/LogBox/DigitalSignDocTypeComponent';
import {AddEditImporterDocumentComponent} from './Components/LogBox/AddEditImporterDocumentComponent';
import {LogboxUploaderComponent} from './Components/Logbox/LogboxUploaderComponent';
import {AddEditImporterShipmentComponent} from './Components/Logbox/AddEditImporterShipmentComponent';
import {ForwarderShipmentsComponent} from './Components/Logbox/ForwarderShipmentsComponent';
import {MultiArchiveShipmentsComponent} from './Components/Logbox/MultiArchiveShipmentsComponent';
import {DownloadAllFilesComponent} from './Components/LogBox/DownloadAllFilesComponent';
import {AddEditPrivateLabelShipmentComponent} from './Components/Logbox/AddEditPrivateLabelShipmentComponent';
import {PrivateLabelApprovePaymentComponent} from './Components/Logbox/PrivateLabelApprovePaymentComponent';
import {PrivateLabelApprovebyMobileComponent} from './Components/Logbox/PrivateLabelApprovebyMobileComponent';
import {ECommercePaymentRequestMobileComponent} from './Components/Logbox/ECommercePaymentRequestMobileComponent';
import {TaxScreenComponent} from './Components/Logbox/TaxScreenComponent';
import {GoodsValueComponent} from './Components/Logbox/GoodsValueComponent';
import {DenyReasonComponent} from './Components/Logbox/DenyReasonComponent';
import {LogBoxPackagesComponent} from './Components/Logbox/LogBoxPackagesComponent';
import {DepositionRequestComponent} from './Components/Logbox/DepositionRequestComponent';



export const Components =
    [
        LogBoxDocumentsComponent,
        LogBoxMainComponent,
        AddEditImporterDocumentComponent,
        LogboxUploaderComponent,
        AddEditImporterShipmentComponent,
        ForwarderShipmentsComponent,
        MultiArchiveShipmentsComponent,
        DownloadAllFilesComponent,
        AddEditPrivateLabelShipmentComponent,
        PrivateLabelApprovePaymentComponent,
        PrivateLabelApprovebyMobileComponent,
        TaxScreenComponent,
        GoodsValueComponent,
        DenyReasonComponent,
        EditLogBoxShipmentComponent,
        DigitalSignDocTypeComponent,
        LogBoxPackagesComponent,
        ECommercePaymentRequestMobileComponent,
        DepositionRequestComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "LogBoxDocumentsComponent": { myResult = LogBoxDocumentsComponent; break; }
            case "LogBoxMainComponent": { myResult = LogBoxMainComponent; break; }
            case "AddEditImporterDocumentComponent": { myResult = AddEditImporterDocumentComponent; break; }
            case "LogboxUploaderComponent": { myResult = LogboxUploaderComponent; break; }
            case "AddEditImporterShipmentComponent": { myResult = AddEditImporterShipmentComponent; break; }
            case "ForwarderShipmentsComponent": { myResult = ForwarderShipmentsComponent; break; }
            case "MultiArchiveShipmentsComponent": { myResult = MultiArchiveShipmentsComponent; break; };
            case "DownloadAllFilesComponent": { myResult = DownloadAllFilesComponent; break; }
            case "AddEditPrivateLabelShipmentComponent": { myResult = AddEditPrivateLabelShipmentComponent; break; }
            case "PrivateLabelApprovePaymentComponent": { myResult = PrivateLabelApprovePaymentComponent; break; }
            case "PrivateLabelApprovebyMobileComponent": { myResult = PrivateLabelApprovebyMobileComponent; break; }
            case "TaxScreenComponent": { myResult = TaxScreenComponent; break; }
            case "GoodsValueComponent": { myResult = GoodsValueComponent; break; }
            case "DenyReasonComponent": { myResult = DenyReasonComponent; break; }
            case "EditLogBoxShipmentComponent": { myResult = EditLogBoxShipmentComponent; break; }
            case "DigitalSignDocTypeComponent": { myResult = DigitalSignDocTypeComponent; break; }
            case "LogBoxPackagesComponent": { myResult = LogBoxPackagesComponent; break; }
            case "ECommercePaymentRequestMobileComponent": { myResult = ECommercePaymentRequestMobileComponent; break; }
            case "DepositionRequestComponent": { myResult = DepositionRequestComponent; break; }
                
        }

        return myResult;
    }
}