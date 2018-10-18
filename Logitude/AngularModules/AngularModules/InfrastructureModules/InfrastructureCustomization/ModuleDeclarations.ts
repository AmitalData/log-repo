import {RulesMainComponent} from './Components/Customization/RulesComponents/RulesMainComponent';
import {AddEditRuleComponent} from './Components/Customization/RulesComponents/AddEditRuleComponent';
import {ObjectFieldsSearchComponent} from './Components/Customization/RulesComponents/ObjectFieldsSearchComponent';
import {AddRuleFieldComponent} from './Components/Customization/RulesComponents/AddRuleFieldComponent';
import {CustomizationMainComponent} from './Components/Customization/CustomizationMainComponent';
import {StandardFieldsComponent} from './Components/Customization/StandardFieldsComponent';
import {EditStandardFieldComponent} from './Components/Customization/EditStandardFieldComponent';
import {ObjectLabelsComponent} from './Components/Customization/ObjectLabelsComponent';
import {CustomFieldsComponent} from './Components/Customization/CustomFieldsComponent';
import {AddEditCustomFieldComponent} from './Components/Customization/AddEditCustomFieldComponent';
import {AddEditPickListComponent} from './Components/Customization/AddEditPickListComponent';
import {SelectLanguagesComponent} from './Components/TranslationLabels/SelectLanguagesComponent';
import {TranslateLabelsComponent} from './Components/TranslationLabels/TranslateLabelsComponent';
import {TranslationComponent} from './Components/Translations/TranslationComponent';
import {DefaultTranslationComponent} from './Components/Translations/DefaultTranslationComponent';
import {ScreenLayoutComponent} from './Components/Customization/ScreenLayoutComponent';
import {LanguageSettingsComponent} from './Components/LanguageSettings/LanguageSettingsComponent';

export const Components =
    [
        CustomizationMainComponent,
        StandardFieldsComponent,
        EditStandardFieldComponent,
        ObjectLabelsComponent,
        SelectLanguagesComponent,
        TranslateLabelsComponent,
        CustomFieldsComponent,
        AddEditCustomFieldComponent,
        RulesMainComponent,
        AddEditRuleComponent,
        TranslationComponent,
        DefaultTranslationComponent,
        AddEditPickListComponent,
        ObjectFieldsSearchComponent,
        AddRuleFieldComponent,
        LanguageSettingsComponent,

    ];
export const ControlsComponents =
    [
        ScreenLayoutComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomizationMainComponent": { myResult = CustomizationMainComponent; break; }
            case "StandardFieldsComponent": { myResult = StandardFieldsComponent; break; }
            case "EditStandardFieldComponent": { myResult = EditStandardFieldComponent; break; }
            case "ObjectLabelsComponent": { myResult = ObjectLabelsComponent; break; }
            case "SelectLanguagesComponent": { myResult = SelectLanguagesComponent; break; }
            case "TranslateLabelsComponent": { myResult = TranslateLabelsComponent; break; }
            case "CustomFieldsComponent": { myResult = CustomFieldsComponent; break; }
            case "AddEditCustomFieldComponent": { myResult = AddEditCustomFieldComponent; break; }
            case "TranslationComponent": { myResult = TranslationComponent; break; }
            case "DefaultTranslationComponent": { myResult = DefaultTranslationComponent; break; }
            case "RulesMainComponent": { myResult = RulesMainComponent; break; }
            case "AddEditRuleComponent": { myResult = AddEditRuleComponent; break; }
            case "AddEditPickListComponent": { myResult = AddEditPickListComponent; break; }
            case "ObjectFieldsSearchComponent": { myResult = ObjectFieldsSearchComponent; break; }
            case "AddRuleFieldComponent": { myResult = AddRuleFieldComponent; break; }
            case "ScreenLayoutComponent": { myResult = ScreenLayoutComponent; break; }
            case "LanguageSettingsComponent": { myResult = LanguageSettingsComponent; break; }

        }

        return myResult;
    }
}