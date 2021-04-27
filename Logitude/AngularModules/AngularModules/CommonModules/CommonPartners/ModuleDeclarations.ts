import {NewPartnerTamplate} from './Components/Templates/NewPartnerTamplate';
import {ContactsTabComponent} from './Components/EditTabs/ContactsTabComponent';
import {ContactInputTemplate} from './Components/Templates/ContactInputTemplate';
import {SearchContactsComponent} from './Components/Templates/SearchContactsComponent';
import {ActionStepsTemplate} from './Components/Templates/ActionStepsTemplate';
import {BillingTabComponent} from './Components/EditTabs/BillingTabComponent';
import {AddressesTabComponent} from './Components/EditTabs/AddressesTabComponent';
import {AddEditContactComponent} from './Components/AddEdit/AddEditContactComponent';
import {AddEditAddressComponent} from './Components/AddEdit/AddEditAddressComponent';
import {ContactGeneralTabComponent} from './Components/EditTabs/Contact/ContactGeneralTabComponent';
import {PartnersTabComponent} from './Components/EditTabs/Contact/PartnersTabComponent';
import {WarehouseGeneralTabComponent} from './Components/EditTabs/Warehouse/WarehouseGeneralTabComponent';
import { ShippingLineInttraTabComponent } from './Components/EditTabs/ShippingLine/ShippingLineInttraTabComponent';
import { ShippinglineGeneralTabComponent } from './Components/EditTabs/ShippingLine/ShippinglineGeneralTabComponent';
import {NewCustomAgentComponent} from './Components/NewEntity/NewCustomAgentComponent';
import {NewShippingLineComponent} from './Components/NewEntity/NewShippingLineComponent';
import {NewTruckerComponent} from './Components/NewEntity/NewTruckerComponent';
import {NewVendorComponent} from './Components/NewEntity/NewVendorComponent';
import {NewWarehouseComponent} from './Components/NewEntity/NewWarehouseComponent';
import {NewShippingAgentComponent} from './Components/NewEntity/NewShippingAgentComponent';
import {NewContactComponent} from './Components/NewEntity/NewContactComponent';
import {NewPotentialCustomerComponent} from './Components/NewEntity/NewPotentialCustomerComponent';
import { ParticipantGeneralTabComponent } from './Components/EditTabs/Participant/ParticipantGeneralTabComponent';
import { ParticipantNotifyTabComponent } from './Components/EditTabs/Participant/ParticipantNotifyTabComponent';
import { ParticipantDocsInTabComponent } from './Components/EditTabs/Participant/ParticipantDocsInTabComponent';
import { CustomAgentDocsInTabComponent } from './Components/EditTabs/CustomAgent/CustomAgentDocsInTabComponent';
import { ShippingAgentDocsInTabComponent } from './Components/EditTabs/ShippingAgent/ShippingAgentDocsInTabComponent';
import { ShippingLineDocsInTabComponent } from './Components/EditTabs/ShippingLine/ShippingLineDocsInTabComponent';
import { TruckerDocsInTabComponent } from './Components/EditTabs/Trucker/TruckerDocsInTabComponent';
import { ExportGeneraInTabComponent } from './Components/EditTabs/Trucker/ExportGeneraInTabComponent';
import { VendorDocsInTabComponent } from './Components/EditTabs/Vendor/VendorDocsInTabComponent';
import { AccountingPartnerDocsInTabComponent } from './Components/EditTabs/AccountingPartner/AccountingPartnerDocsInTabComponent';
import { WarehouseDocsInTabComponent } from './Components/EditTabs/Warehouse/WarehouseDocsInTabComponent';
import { NewAccountingPartnerComponent } from './Components/NewEntity/NewAccountingPartnerComponent';
import { ContactOccasionsTabComponent} from './Components/EditTabs/Contact/ContactOccasionsTabComponent';
import { AreasTabComponent } from './Components/EditTabs/AreasTabComponent';
import { AddEditAreaComponent } from './Components/AddEdit/AddEditAreaComponent';
import { ChoosePortComponent } from './Components/AddEdit/ChoosePortComponent';
import { ChooseCountryPortComponent } from './Components/AddEdit/ChooseCountryPortComponent';
import { TariffTranslationsTabComponent } from './Components/EditTabs/TariffTranslations/TariffTranslationsTabComponent';
import { AddEditTariffTranslationComponent } from './Components/EditTabs/TariffTranslations/AddEditTariffTranslationComponent';
import { AddEditPartnerAddressComponent } from './Components/AddEdit/AddEditPartnerAddressComponent';
import { UploadPartnersComponent } from './Components/Maintenance/UploadPartnersComponent';
import { StorageDefaultsComponents } from './Components/EditTabs/Warehouse/StorageDefaultsComponents';
import { ShippingAgentGeneralTabComponent } from './Components/EditTabs/ShippingAgent/ShippingAgentGeneralTabComponent';

export const Components =
    [
        NewPartnerTamplate,
        ContactsTabComponent,
        ContactInputTemplate,
        SearchContactsComponent,
        ActionStepsTemplate,
        BillingTabComponent,
        AddressesTabComponent,
        AddEditContactComponent,
        AddEditAddressComponent,
        ContactGeneralTabComponent,
        PartnersTabComponent, 
        WarehouseGeneralTabComponent,
        ShippingLineInttraTabComponent,
        ShippinglineGeneralTabComponent,
        NewCustomAgentComponent,
        NewShippingLineComponent,
        NewTruckerComponent,
        NewVendorComponent,
        NewWarehouseComponent,
        NewShippingAgentComponent,
        NewContactComponent,   
        NewPotentialCustomerComponent,
        ParticipantGeneralTabComponent,
        ParticipantNotifyTabComponent,
        ParticipantDocsInTabComponent,
        CustomAgentDocsInTabComponent,
        ShippingAgentDocsInTabComponent,
        ShippingLineDocsInTabComponent,
        TruckerDocsInTabComponent,
        ExportGeneraInTabComponent,
        VendorDocsInTabComponent,
        AccountingPartnerDocsInTabComponent,
        WarehouseDocsInTabComponent,
        NewAccountingPartnerComponent,
        ContactOccasionsTabComponent,
        AreasTabComponent,
        AddEditAreaComponent,
        ChoosePortComponent,
        ChooseCountryPortComponent,
        TariffTranslationsTabComponent,
        AddEditTariffTranslationComponent,
        AddEditPartnerAddressComponent,
        UploadPartnersComponent,
        StorageDefaultsComponents,
        ShippingAgentGeneralTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewPartnerTamplate": { myResult = NewPartnerTamplate; break; }
            case "ContactsTabComponent": { myResult = ContactsTabComponent; break; }
            case "ContactInputTemplate": { myResult = ContactInputTemplate; break; }
            case "SearchContactsComponent": { myResult = SearchContactsComponent; break; }
            case "ActionStepsTemplate": { myResult = ActionStepsTemplate; break; }
            case "BillingTabComponent": { myResult = BillingTabComponent; break; }
            case "AddressesTabComponent": { myResult = AddressesTabComponent; break; }
            case "AddEditContactComponent": { myResult = AddEditContactComponent; break; }
            case "AddEditAddressComponent": { myResult = AddEditAddressComponent; break; }
            case "ContactGeneralTabComponent": { myResult = ContactGeneralTabComponent; break; }
            case "PartnersTabComponent": { myResult = PartnersTabComponent; break; }  
            case "WarehouseGeneralTabComponent": { myResult = WarehouseGeneralTabComponent; break; }
            case "ShippingLineInttraTabComponent": { myResult = ShippingLineInttraTabComponent; break; }
            case "ShippinglineGeneralTabComponent": { myResult = ShippinglineGeneralTabComponent; break; }   
            case "NewCustomAgentComponent": { myResult = NewCustomAgentComponent; break; }
            case "NewShippingLineComponent": { myResult = NewShippingLineComponent; break; }
            case "NewTruckerComponent": { myResult = NewTruckerComponent; break; }
            case "NewVendorComponent": { myResult = NewVendorComponent; break; }
            case "NewWarehouseComponent": { myResult = NewWarehouseComponent; break; }
            case "NewShippingAgentComponent": { myResult = NewShippingAgentComponent; break; }
            case "NewContactComponent": { myResult = NewContactComponent; break; }
            case "NewPotentialCustomerComponent": { myResult = NewPotentialCustomerComponent; break; }
            case "ParticipantGeneralTabComponent": { myResult = ParticipantGeneralTabComponent; break; }
            case "ParticipantNotifyTabComponent": { myResult = ParticipantNotifyTabComponent; break; }
            case "ParticipantDocsInTabComponent": { myResult = ParticipantDocsInTabComponent; break; }
            case "CustomAgentDocsInTabComponent": { myResult = CustomAgentDocsInTabComponent; break; }
            case "ShippingAgentDocsInTabComponent": { myResult = ShippingAgentDocsInTabComponent; break; }
            case "ShippingLineDocsInTabComponent": { myResult = ShippingLineDocsInTabComponent; break; }
            case "TruckerDocsInTabComponent": { myResult = TruckerDocsInTabComponent; break; }
            case "ExportGeneraInTabComponent": { myResult = ExportGeneraInTabComponent; break; }
            case "VendorDocsInTabComponent": { myResult = VendorDocsInTabComponent; break; }
            case "AccountingPartnerDocsInTabComponent": { myResult = AccountingPartnerDocsInTabComponent; break; }
            case "WarehouseDocsInTabComponent": { myResult = WarehouseDocsInTabComponent; break; }
            case "NewAccountingPartnerComponent": { myResult = NewAccountingPartnerComponent; break; }
            case "ContactOccasionsTabComponent": { myResult = ContactOccasionsTabComponent; break; }
            case "AreasTabComponent": { myResult = AreasTabComponent; break; }
            case "AddEditAreaComponent": { myResult = AddEditAreaComponent; break; }
            case "ChoosePortComponent": { myResult = ChoosePortComponent; break; }
            case "ChooseCountryPortComponent": { myResult = ChooseCountryPortComponent; break; }
            case "TariffTranslationsTabComponent": { myResult = TariffTranslationsTabComponent; break; }
            case "AddEditTariffTranslationComponent": { myResult = AddEditTariffTranslationComponent; break; }
            case "AddEditPartnerAddressComponent": { myResult = AddEditPartnerAddressComponent; break; }
            case "UploadPartnersComponent": { myResult = UploadPartnersComponent; break; }
            case "StorageDefaultsComponents": { myResult = StorageDefaultsComponents; break; }
            case "ShippingAgentGeneralTabComponent": { myResult = ShippingAgentGeneralTabComponent; break; }
        }

        return myResult;
    }
}
