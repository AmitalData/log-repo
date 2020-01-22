import { CustomsRequestsComponent } from './Components/CustomsRequestsComponent';
import { DeclarationRestoreComponent } from './Components/DeclarationRequests/DeclarationRestoreComponent';
import { DeclarationStatusComponent } from './Components/DeclarationRequests/DeclarationStatusComponent';
import { GuaranteeCertificateComponent } from './Components/TapagRequests/GuaranteeCertificateComponent';
import { FaultQueryComponent } from './Components/TapagRequests/FaultQueryComponent';
import { WarehouseBlockBalanceComponent } from './Components/DeclarationRequests/WarehouseBlockBalanceComponent';
import { MasavPaymentsToAgentComponent } from './Components/PaymentOrderRequests/MasavPaymentsToAgentComponent';
import { PaymentOrderQueryComponent } from './Components/PaymentOrderRequests/PaymentOrderQueryComponent';
import { PrintRequestComponent } from './Components/DeclarationRequests/PrintRequestComponent';
import { GuaranteeFileFilterQueryComponent } from './Components/TapagRequests/GuaranteeFileFilterQueryComponent';
import { PaymentOrderReplyComponent } from './Components/PaymentOrderRequests/PaymentOrderReplyComponent';
import { ExportDeclarationDataComponent } from './Components/DeclarationRequests/ExportDeclarationDataComponent';
import { DeclarationFilterComponent } from './Components/TapagRequests/DeclarationFilterComponent';
import { ClaimFileFilterComponent } from './Components/ClaimRequests/ClaimFileFilterComponent';
import { VehicleForGoodsItemComponent } from './Components/DeclarationRequests/VehicleForGoodsItemComponent';
import { BlockListInWarehouseComponent } from './Components/DeclarationRequests/BlockListInWarehouseComponent';
import { RequestDetailsComponent } from './Components/DeclarationRequests/RequestDetailsComponent';
import { ReleaseGoodsComponent } from './Components/DeclarationRequests/ReleaseGoodsComponent'; 
import { StorageEntranceComponent } from './Components/Courier/StorageEntranceComponent';
import { CargoSealsQueryComponent } from './Components/DeclarationRequests/CargoSealsQueryComponent';



export const Components =
    [
        CustomsRequestsComponent,
        DeclarationRestoreComponent,
        StorageEntranceComponent,
        DeclarationStatusComponent,
        GuaranteeCertificateComponent,
        FaultQueryComponent,
        WarehouseBlockBalanceComponent,
        MasavPaymentsToAgentComponent,
        PaymentOrderQueryComponent,
        PrintRequestComponent,
        GuaranteeFileFilterQueryComponent,
        PaymentOrderReplyComponent,
        ExportDeclarationDataComponent,
        DeclarationFilterComponent,
        ClaimFileFilterComponent,
        VehicleForGoodsItemComponent,
        BlockListInWarehouseComponent,
        RequestDetailsComponent,
        ReleaseGoodsComponent,
        CargoSealsQueryComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsRequestsComponent": { myResult = CustomsRequestsComponent; break; }
            case "DeclarationRestoreComponent": { myResult = DeclarationRestoreComponent; break; }
            case "StorageEntranceComponent": { myResult = StorageEntranceComponent; break; }
            case "DeclarationStatusComponent": { myResult = DeclarationStatusComponent; break; }
            case "GuaranteeCertificateComponent": { myResult = GuaranteeCertificateComponent; break; }
            case "FaultQueryComponent": { myResult = FaultQueryComponent; break; }
            case "WarehouseBlockBalanceComponent": { myResult = WarehouseBlockBalanceComponent; break; }
            case "MasavPaymentsToAgentComponent": { myResult = MasavPaymentsToAgentComponent; break; }
            case "PaymentOrderQueryComponent": { myResult = PaymentOrderQueryComponent; break; }
            case "PrintRequestComponent": { myResult = PrintRequestComponent; break; }
            case "GuaranteeFileFilterQueryComponent": { myResult = GuaranteeFileFilterQueryComponent; break; }
            case "PaymentOrderReplyComponent": { myResult = PaymentOrderReplyComponent; break; }
            case "ExportDeclarationDataComponent": { myResult = ExportDeclarationDataComponent; break; }
            case "DeclarationFilterComponent": { myResult = DeclarationFilterComponent; break; }
            case "ClaimFileFilterComponent": { myResult = ClaimFileFilterComponent; break; }
            case "VehicleForGoodsItemComponent": { myResult = VehicleForGoodsItemComponent; break; }
            case "BlockListInWarehouseComponent": { myResult = BlockListInWarehouseComponent; break; }
            case "RequestDetailsComponent": { myResult = RequestDetailsComponent; break; }
            case "ReleaseGoodsComponent": { myResult = ReleaseGoodsComponent; break; }
            case "CargoSealsQueryComponent": { myResult = CargoSealsQueryComponent; break; } 
        }

        return myResult;
    }
}
