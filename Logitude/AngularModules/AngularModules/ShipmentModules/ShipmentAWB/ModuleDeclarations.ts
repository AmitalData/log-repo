import {AWBWizardComponent} from './Components/AWBWizard/AWBWizardComponent';
import {AWBWizardLoadComponent} from './Components/AWBWizard/AWBWizardLoadComponent';
import {AWBOverviewTabComponent} from './Components/AWBWizard/Overview/AWBOverviewTabComponent';
import {AWBPartnersTabComponent} from './Components/AWBWizard/Partners/AWBPartnersTabComponent';
import {AWBAddEditPartnerComponent} from './Components/AWBWizard/Partners/AWBAddEditPartnerComponent';
import {AWBRoutingsTabComponent} from './Components/AWBWizard/Routings/AWBRoutingsTabComponent';
import {AWBHouseRoutingsTabComponent} from './Components/AWBWizard/Routings/AWBHouseRoutingsTabComponent';
import {AWBPackagesTabComponent} from './Components/AWBWizard/Packages/AWBPackagesTabComponent';
import { AWBAddEditPackageComponent } from './Components/AWBWizard/Packages/AWBAddEditPackageComponent';
import { AWBAddEditCommodityComponent } from './Components/AWBWizard/Packages/AWBAddEditCommodityComponent';
import { AWBAddEditCommodityPackageComponent } from './Components/AWBWizard/Packages/AWBAddEditCommodityPackageComponent';
import {AWBChooseCommodityComponent} from './Components/AWBWizard/Packages/AWBChooseCommodityComponent';
import {AWBDangerousPackageComponent} from './Components/AWBWizard/Packages/AWBDangerousPackageComponent';
import {FreightChargesTabComponent} from './Components/AWBWizard/FreightCharges/FreightChargesTabComponent';
import {GeneralDetailsTabComponent} from './Components/AWBWizard/GeneralDetails/GeneralDetailsTabComponent';
import {AdvancedAccountingComponent} from './Components/AWBWizard/GeneralDetails/AdvancedAccountingComponent';
import {AdvancedCommentsComponent} from './Components/AWBWizard/GeneralDetails/AdvancedCommentsComponent';
import {HAWBTabComponent} from './Components/AWBWizard/HAWB/HAWBTabComponent';
import {OCITabComponent} from './Components/AWBWizard/OCI/OCITabComponent';
import {AddEditOCIComponent} from './Components/AWBWizard/OCI/AddEditOCIComponent';
import {OtherChargesTabComponent} from './Components/AWBWizard/OtherCharges/OtherChargesTabComponent';
import {ManageDefaultsComponent} from './Components/AWBWizard/OtherCharges/ManageDefaultsComponent';
import {AddEditOtherChargeComponent} from './Components/AWBWizard/OtherCharges/AddEditOtherChargeComponent';
import {OtherPartnersTabComponent} from './Components/AWBWizard/OtherPartners/OtherPartnersTabComponent';
import {RADetailsTabComponent} from './Components/AWBWizard/RADetails/RADetailsTabComponent';
import {SendWindowComponent} from './Components/AWBWizard/SendWindowComponent';
import {PurchaseStockComponent} from './Components/AWBWizard/Others/PurchaseStockComponent';
import {SendFSRComponent} from './Components/FSRWizard/SendFSRComponent';
import {FSRWizardComponent} from './Components/FSRWizard/FSRWizardComponent';
import {SendShipmentFSRComponent} from './Components/FSRWizard/SendShipmentFSRComponent';
import { TestMultiHarmonizeComponent } from './Components/AWBWizard/TestMultiHarmonizeComponent';

export const Components = 
    [
        AWBWizardComponent,
        AWBWizardLoadComponent,
        AWBOverviewTabComponent,
        AWBPartnersTabComponent,
        AWBAddEditPartnerComponent,
        AWBRoutingsTabComponent,
        AWBHouseRoutingsTabComponent,
        AWBPackagesTabComponent,
        AWBAddEditPackageComponent,
        AWBAddEditCommodityComponent,
        AWBAddEditCommodityPackageComponent,
        AWBChooseCommodityComponent,
        AWBDangerousPackageComponent,
        FreightChargesTabComponent,
        GeneralDetailsTabComponent,
        AdvancedAccountingComponent,
        AdvancedCommentsComponent,
        HAWBTabComponent,
        OCITabComponent,
        AddEditOCIComponent,
        OtherChargesTabComponent,
        ManageDefaultsComponent,
        AddEditOtherChargeComponent,
        OtherPartnersTabComponent,
        RADetailsTabComponent,
        SendWindowComponent,
        PurchaseStockComponent,
        SendFSRComponent,
        FSRWizardComponent,
        SendShipmentFSRComponent,
        TestMultiHarmonizeComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AWBWizardComponent": { myResult = AWBWizardComponent; break; }
            case "AWBWizardLoadComponent": { myResult = AWBWizardLoadComponent; break; }
            case "AWBOverviewTabComponent": { myResult = AWBOverviewTabComponent; break; }
            case "AWBPartnersTabComponent": { myResult = AWBPartnersTabComponent; break; }
            case "AWBAddEditPartnerComponent": { myResult = AWBAddEditPartnerComponent; break; }
            case "AWBRoutingsTabComponent": { myResult = AWBRoutingsTabComponent; break; }
            case "AWBHouseRoutingsTabComponent": { myResult = AWBHouseRoutingsTabComponent; break; }
            case "AWBPackagesTabComponent": { myResult = AWBPackagesTabComponent; break; }
            case "AWBAddEditPackageComponent": { myResult = AWBAddEditPackageComponent; break; }
            case "AWBAddEditCommodityComponent": { myResult = AWBAddEditCommodityComponent; break; }
            case "AWBAddEditCommodityPackageComponent": { myResult = AWBAddEditCommodityPackageComponent; break; }                
            case "AWBChooseCommodityComponent": { myResult = AWBChooseCommodityComponent; break; }
            case "AWBDangerousPackageComponent": { myResult = AWBDangerousPackageComponent; break; }
            case "FreightChargesTabComponent": { myResult = FreightChargesTabComponent; break; }
            case "GeneralDetailsTabComponent": { myResult = GeneralDetailsTabComponent; break; }
            case "AdvancedAccountingComponent": { myResult = AdvancedAccountingComponent; break; }
            case "AdvancedCommentsComponent": { myResult = AdvancedCommentsComponent; break; }
            case "HAWBTabComponent": { myResult = HAWBTabComponent; break; }
            case "OCITabComponent": { myResult = OCITabComponent; break; }
            case "AddEditOCIComponent": { myResult = AddEditOCIComponent; break; }
            case "OtherChargesTabComponent": { myResult = OtherChargesTabComponent; break; }
            case "ManageDefaultsComponent": { myResult = ManageDefaultsComponent; break; }
            case "AddEditOtherChargeComponent": { myResult = AddEditOtherChargeComponent; break; }
            case "OtherPartnersTabComponent": { myResult = OtherPartnersTabComponent; break; }
            case "RADetailsTabComponent": { myResult = RADetailsTabComponent; break; }
            case "SendWindowComponent": { myResult = SendWindowComponent; break; }
            case "PurchaseStockComponent": { myResult = PurchaseStockComponent; break; } 
            case "SendFSRComponent": { myResult = SendFSRComponent; break; }
            case "FSRWizardComponent": { myResult = FSRWizardComponent; break; }
            case "SendShipmentFSRComponent": { myResult = SendShipmentFSRComponent; break; }
            case "TestMultiHarmonizeComponent": { myResult = TestMultiHarmonizeComponent; break; }
        }

        return myResult;
    }
}
