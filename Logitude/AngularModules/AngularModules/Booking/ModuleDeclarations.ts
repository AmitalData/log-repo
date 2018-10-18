import {BookingsComponent} from './Components/Workspaces/BookingsComponent';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {BookingWizardComponent} from './Components/BookingWizard/BookingWizardComponent';
import {BookingWizardLoadComponent} from './Components/BookingWizard/BookingWizardLoadComponent';
import {OverviewTabComponent} from './Components/BookingWizard/Overview/OverviewTabComponent';
import {BookingDetailsTabComponent} from './Components/BookingWizard/BookingDetails/BookingDetailsTabComponent';
import {PartnersTabComponent} from './Components/BookingWizard/Partners/PartnersTabComponent';
import {AddEditPartnerComponent} from './Components/BookingWizard/Partners/AddEditPartnerComponent';
import {PackagesTabComponent} from './Components/BookingWizard/Packages/PackagesTabComponent';
import {AddEditPackageComponent} from './Components/BookingWizard/Packages/AddEditPackageComponent';
import {ChooseDescriptionOfGoodsComponent} from './Components/BookingWizard/Packages/ChooseDescriptionOfGoodsComponent';
import {DangerousPackageComponent} from './Components/BookingWizard/Packages/DangerousPackageComponent';
import {GeneralDetailsTabComponent} from './Components/BookingWizard/GeneralDetails/GeneralDetailsTabComponent';
import {NoRemainingStockComponent} from './Components/BookingWizard/NoRemainingStockComponent';

export const Components =
    [
        BookingsComponent,
        FieldTemplateComponent,
        BookingWizardComponent,
        BookingWizardLoadComponent,
        OverviewTabComponent,
        BookingDetailsTabComponent,
        PartnersTabComponent,
        AddEditPartnerComponent,
        PackagesTabComponent,
        AddEditPackageComponent,
        ChooseDescriptionOfGoodsComponent,
        DangerousPackageComponent,
        GeneralDetailsTabComponent,
        NoRemainingStockComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BookingsComponent": { myResult = BookingsComponent; break; }
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "BookingWizardComponent": { myResult = BookingWizardComponent; break; }
            case "BookingWizardLoadComponent": { myResult = BookingWizardLoadComponent; break; }
            case "OverviewTabComponent": { myResult = OverviewTabComponent; break; }
            case "BookingDetailsTabComponent": { myResult = BookingDetailsTabComponent; break; }
            case "PartnersTabComponent": { myResult = PartnersTabComponent; break; }
            case "AddEditPartnerComponent": { myResult = AddEditPartnerComponent; break; }
            case "PackagesTabComponent": { myResult = PackagesTabComponent; break; }
            case "AddEditPackageComponent": { myResult = AddEditPackageComponent; break; }
            case "ChooseDescriptionOfGoodsComponent": { myResult = ChooseDescriptionOfGoodsComponent; break; }
            case "DangerousPackageComponent": { myResult = DangerousPackageComponent; break; }
            case "GeneralDetailsTabComponent": { myResult = GeneralDetailsTabComponent; break; }
            case "NoRemainingStockComponent": { myResult = NoRemainingStockComponent; break; }
        }

        return myResult;
    }
}