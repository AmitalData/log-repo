import { CourierBOLQueryComponent } from './Components/CourierBOLQueryComponent';
import { ExchangeRatesQueryComponent } from './Components/ExchangeRatesQueryComponent';
import { MasterBOLQueryComponent } from './Components/MasterBOLQueryComponent';
import { CustomItemLegalDemandsQueryComponent } from './Components/CustomItemLegalDemandsQueryComponent';
import { CreditLimitQueryComponent } from './Components/CreditLimitQueryComponent';
import { GoldCreditLimitQueryComponent } from './Components/GoldCreditLimitQueryComponent';
import { ImporterDeclarationComponent } from './Components/ImporterDeclarationComponent';
import { SpecialActivityRequestComponent } from './Components/SpecialActivityRequestComponent';
import { CargoQueryRequestComponent } from './Components/CargoQueryRequestComponent';
import { CustomsRestoreMessagesComponent } from './Components/CustomsRestoreMessagesComponent';
import { CustomsBookQueryComponent } from './Components/CustomsBookQueryComponent';
import { DeficitFileFilterComponent } from './Components/DeficitFileFilterComponent';
import { DeclarationReshimonConversionComponent } from './Components/DeclarationReshimonConversionComponent';
import { AddAttachmentResponseComponent } from './Components/AddAttachmentResponseComponent';
import { RequiredDocumentComponent } from './Components/RequiredDocumentComponent';
import { RecallSuppliersFromFileComponent } from './Components/RecallSuppliersFromFileComponent';
import { ClientSearchByIDComponent } from './Components/ClientSearchByIDComponent';
import { CustomerIndicationComponent } from './Components/CustomerIndicationComponent';
import { RecallClientsForCutoms } from './Components/RecallClientsForCutoms';
import { MorningMessageComponent } from './Components/MorningMessageComponent';

export const Components =
    [
        CourierBOLQueryComponent,
        ExchangeRatesQueryComponent,
        MasterBOLQueryComponent,
        CustomItemLegalDemandsQueryComponent,
        CreditLimitQueryComponent,
        GoldCreditLimitQueryComponent,
        ImporterDeclarationComponent,
        SpecialActivityRequestComponent,
        CargoQueryRequestComponent,
        CustomsRestoreMessagesComponent,
        CustomsBookQueryComponent,
        DeficitFileFilterComponent,
        DeclarationReshimonConversionComponent,
        AddAttachmentResponseComponent,
        RequiredDocumentComponent,
        RecallSuppliersFromFileComponent,
        ClientSearchByIDComponent,
        CustomerIndicationComponent,
        RecallClientsForCutoms,
        MorningMessageComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {

            case "CourierBOLQueryComponent": { myResult = CourierBOLQueryComponent; break; }
            case "ExchangeRatesQueryComponent": { myResult = ExchangeRatesQueryComponent; break; }
            case "MasterBOLQueryComponent": { myResult = MasterBOLQueryComponent; break; }
            case "CustomItemLegalDemandsQueryComponent": { myResult = CustomItemLegalDemandsQueryComponent; break; }
            case "CreditLimitQueryComponent": { myResult = CreditLimitQueryComponent; break; }
            case "GoldCreditLimitQueryComponent": { myResult = GoldCreditLimitQueryComponent; break; }
            case "ImporterDeclarationComponent": { myResult = ImporterDeclarationComponent; break; }
            case "SpecialActivityRequestComponent": { myResult = SpecialActivityRequestComponent; break; }
            case "CargoQueryRequestComponent": { myResult = CargoQueryRequestComponent; break; }
            case "CustomsRestoreMessagesComponent": { myResult = CustomsRestoreMessagesComponent; break; }
            case "CustomsBookQueryComponent": { myResult = CustomsBookQueryComponent; break; }
            case "DeficitFileFilterComponent": { myResult = DeficitFileFilterComponent; break; }
            case "DeclarationReshimonConversionComponent": { myResult = DeclarationReshimonConversionComponent; break; }
            case "AddAttachmentResponseComponent": { myResult = AddAttachmentResponseComponent; break; }
            case "RequiredDocumentComponent": { myResult = RequiredDocumentComponent; break; }
            case "RecallSuppliersFromFileComponent": { myResult = RecallSuppliersFromFileComponent; break; }
            case "ClientSearchByIDComponent": { myResult = ClientSearchByIDComponent; break; }
            case "CustomerIndicationComponent": { myResult = CustomerIndicationComponent; break; }
            case "RecallClientsForCutoms": { myResult = RecallClientsForCutoms; break; }
            case "MorningMessageComponent": { myResult = MorningMessageComponent; break; }

        }

        return myResult;
    }
}