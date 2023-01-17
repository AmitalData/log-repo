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
import { RuleUpdateHistoryComponent } from './Components/Customization/RulesComponents/RuleUpdateHistoryComponent';
import { SetWorkerRoleNameComponent } from './Components/SetWorkerRoleName/SetWorkerRoleNameComponent';
import { AddEditScreenComponent } from './Components/Customization/AddEditScreenComponent';
import { AddTabComponent } from './Components/Customization/AddTabComponent';
import { CustomizationTabsComponent } from './Components/Customization/CustomizationTabsComponent';
import { LighteningScreenComponent } from './Components/Customization/Screen/LighteningScreenComponent';
import { ClassicScreenComponent } from './Components/Customization/Screen/ClassicScreenComponent';
import { QueryFilterTreeComponent } from './Components/Customization/QueryFilterTreeComponent';
import { CustomizationEditComponent } from './Components/Customization/CustomizationEditComponent';
import { SubEntitiesComponent } from './Components/Customization/SubEntitiesComponent';
import { AddEditGridScreenSectionComponent } from './Components/Customization/Screen/Section/AddEditGridScreenSectionComponent';
import { GridScreenSectionPreviewComponent } from './Components/Customization/Screen/Section/GridScreenSectionPreviewComponent';
import { GridScreenComponent } from './Components/Customization/Screen/GridScreenComponent';
import { AddCustomObjectComponent } from './Components/Customization/AddCustomObjectComponent';
import { CustomizationQueriesComponent } from './Components/Customization/CustomizationQueriesComponent';

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
        RuleUpdateHistoryComponent,
        SetWorkerRoleNameComponent,
        AddEditScreenComponent,
        CustomizationTabsComponent,
        AddTabComponent,
        ClassicScreenComponent,
        LighteningScreenComponent,
        QueryFilterTreeComponent,
        CustomizationEditComponent,
        SubEntitiesComponent,
        AddEditGridScreenSectionComponent,
        GridScreenSectionPreviewComponent,
        GridScreenComponent,
        AddCustomObjectComponent,
        CustomizationQueriesComponent,
    ];
export const ControlsComponents =
    [
        ScreenLayoutComponent,
        ClassicScreenComponent,
        LighteningScreenComponent,
        QueryFilterTreeComponent,
        GridScreenSectionPreviewComponent,
        GridScreenComponent,

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
            case "RuleUpdateHistoryComponent": { myResult = RuleUpdateHistoryComponent; break; }
            case "SetWorkerRoleNameComponent": { myResult = SetWorkerRoleNameComponent; break; }
            case "AddEditScreenComponent": { myResult = AddEditScreenComponent; break; }
            case "AddTabComponent": { myResult = AddTabComponent; break; }
            case "CustomizationTabsComponent": { myResult = CustomizationTabsComponent; break; }
            case "ClassicScreenComponent": { myResult = ClassicScreenComponent; break; }
            case "LighteningScreenComponent": { myResult = LighteningScreenComponent; break; }
            case "QueryFilterTreeComponent": { myResult = QueryFilterTreeComponent; break; }
            case "CustomizationEditComponent": { myResult = CustomizationEditComponent; break; }
            case "SubEntitiesComponent": { myResult = SubEntitiesComponent; break; }
            case "AddEditGridScreenSectionComponent": { myResult = AddEditGridScreenSectionComponent; break; }
            case "GridScreenSectionPreviewComponent": { myResult = GridScreenSectionPreviewComponent; break; }
            case "GridScreenComponent": { myResult = GridScreenComponent; break; }
            case "AddCustomObjectComponent": { myResult = AddCustomObjectComponent; break; }
            case "CustomizationQueriesComponent": { myResult = CustomizationQueriesComponent; break; }
        }

        return myResult;
    }
}
