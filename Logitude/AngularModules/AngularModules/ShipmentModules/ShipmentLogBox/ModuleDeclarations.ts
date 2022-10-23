import { LogBoxDocumentsComponent } from './Components/Logbox/LogBoxDocumentsComponent';
import { LogBoxMainComponent } from './Components/Logbox/LogBoxMainComponent';
import { EditLogBoxShipmentComponent } from './Components/Logbox/EditLogBoxShipmentComponent';
import { DigitalSignDocTypeComponent } from './Components/Logbox/DigitalSignDocTypeComponent';
import { AddEditImporterDocumentComponent } from './Components/Logbox/AddEditImporterDocumentComponent';
import {LogboxUploaderComponent} from './Components/Logbox/LogboxUploaderComponent';
import {AddEditImporterShipmentComponent} from './Components/Logbox/AddEditImporterShipmentComponent';
import {ForwarderShipmentsComponent} from './Components/Logbox/ForwarderShipmentsComponent';
import {MultiArchiveShipmentsComponent} from './Components/Logbox/MultiArchiveShipmentsComponent';
import { DownloadAllFilesComponent } from './Components/Logbox/DownloadAllFilesComponent';
import {AddEditPrivateLabelCustomsShipmentComponent} from './Components/Logbox/AddEditPrivateLabelCustomsShipmentComponent';
import { DSVApprovePaymentComponent } from './Components/Logbox/DSVApprovePaymentComponent';
import { LogBoxApprovePaymentComponent } from './Components/Logbox/LogBoxApprovePaymentComponent';
import {PrivateLabelApprovebyMobileComponent} from './Components/Logbox/PrivateLabelApprovebyMobileComponent';
import {ECommercePaymentRequestMobileComponent} from './Components/Logbox/ECommercePaymentRequestMobileComponent';
import {TaxScreenComponent} from './Components/Logbox/TaxScreenComponent';
import {GoodsValueComponent} from './Components/Logbox/GoodsValueComponent';
import {DenyReasonComponent} from './Components/Logbox/DenyReasonComponent';
import {LogBoxPackagesComponent} from './Components/Logbox/LogBoxPackagesComponent';
import {DepositionRequestComponent} from './Components/Logbox/DepositionRequestComponent';
import { UserIdNumberMobileComponent } from './Components/Logbox/UserIdNumberMobileComponent';
import { WarningApprovePaymentComponent } from './Components/Logbox/WarningApprovePaymentComponent';
import { PrivateLabelApprovePaymentComponent } from './Components/Logbox/PrivateLabelApprovePaymentComponent';
import { ApprovePaymentBaseComponent } from './Components/Logbox/ApprovePaymentBaseComponent'; 
import { PrivateLabelPackageComponent } from './Components/Logbox/PrivateLabelPackageComponent';
import { AddEditPrivateLabelShipmentComponent } from './Components/Logbox/AddEditPrivateLabelShipmentComponent';


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
        AddEditPrivateLabelCustomsShipmentComponent,
        DSVApprovePaymentComponent,
        LogBoxApprovePaymentComponent,
        PrivateLabelApprovebyMobileComponent,
        TaxScreenComponent,
        GoodsValueComponent,
        DenyReasonComponent,
        EditLogBoxShipmentComponent,
        DigitalSignDocTypeComponent,
        LogBoxPackagesComponent,
        ECommercePaymentRequestMobileComponent,
        DepositionRequestComponent,
        UserIdNumberMobileComponent,
        WarningApprovePaymentComponent,
        PrivateLabelApprovePaymentComponent,
        ApprovePaymentBaseComponent,
        AddEditPrivateLabelShipmentComponent,
        PrivateLabelPackageComponent,
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
            case "AddEditPrivateLabelCustomsShipmentComponent": { myResult = AddEditPrivateLabelCustomsShipmentComponent; break; }
            case "DSVApprovePaymentComponent": { myResult = DSVApprovePaymentComponent; break; }
            case "LogBoxApprovePaymentComponent": { myResult = LogBoxApprovePaymentComponent; break; }
            case "PrivateLabelApprovebyMobileComponent": { myResult = PrivateLabelApprovebyMobileComponent; break; }
            case "TaxScreenComponent": { myResult = TaxScreenComponent; break; }
            case "GoodsValueComponent": { myResult = GoodsValueComponent; break; }
            case "DenyReasonComponent": { myResult = DenyReasonComponent; break; }
            case "EditLogBoxShipmentComponent": { myResult = EditLogBoxShipmentComponent; break; }
            case "DigitalSignDocTypeComponent": { myResult = DigitalSignDocTypeComponent; break; }
            case "LogBoxPackagesComponent": { myResult = LogBoxPackagesComponent; break; }
            case "ECommercePaymentRequestMobileComponent": { myResult = ECommercePaymentRequestMobileComponent; break; }
            case "DepositionRequestComponent": { myResult = DepositionRequestComponent; break; }
            case "UserIdNumberMobileComponent": { myResult = UserIdNumberMobileComponent; break; }
            case "WarningApprovePaymentComponent": { myResult = WarningApprovePaymentComponent; break; }
            case "PrivateLabelApprovePaymentComponent": { myResult = PrivateLabelApprovePaymentComponent; break; }            
            case "ApprovePaymentBaseComponent": { myResult = ApprovePaymentBaseComponent; break; }
            case "AddEditPrivateLabelShipmentComponent": { myResult = AddEditPrivateLabelShipmentComponent; break; }
            case "PrivateLabelPackageComponent": { myResult = PrivateLabelPackageComponent; break; }
                 
        }

        return myResult;
    }
}
