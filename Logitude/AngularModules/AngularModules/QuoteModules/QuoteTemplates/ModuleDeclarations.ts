import {AddEditQuoteTemplateSectionComponent} from './Components/AddEditQuoteTemplateSectionComponent';
import {AddQuoteTemplateFromLibraryComponent} from './Components/AddQuoteTemplateFromLibraryComponent';
import {AdvanceDesignTableComponent} from './Components/AdvanceDesignTableComponent';
import {EditQuoteTemplateComponent} from './Components/EditQuoteTemplateComponent';
import {NewQuoteTemplateComponent} from './Components/NewQuoteTemplateComponent';
import {PageAreaHeaderFooterComponent} from './Components/PageAreaHeaderFooterComponent';
import {PreviewQuoteTemplateReportComponent} from './Components/PreviewQuoteTemplateReportComponent';
import {QuoteTemplateGeneralSetting} from './Components/QuoteTemplateGeneralSetting';
import {QuoteTemplateHeaderDetailsSettingComponent} from './Components/QuoteTemplateHeaderDetailsSettingComponent';
import {QuoteTemplateHeaderFooterSettingComponent} from './Components/QuoteTemplateHeaderFooterSettingComponent';
import {QuoteTemplatePricingSettingComponent} from './Components/QuoteTemplatePricingSettingComponent';
import {QuoteTemplateTotalPerContainerSetting} from './Components/QuoteTemplateTotalPerContainerSetting';

export const Components =
    [
        AddEditQuoteTemplateSectionComponent,
        AddQuoteTemplateFromLibraryComponent,
        AdvanceDesignTableComponent,
        EditQuoteTemplateComponent,
        NewQuoteTemplateComponent,
        PageAreaHeaderFooterComponent,
        PreviewQuoteTemplateReportComponent,
        QuoteTemplateGeneralSetting,
        QuoteTemplateHeaderDetailsSettingComponent,
        QuoteTemplateHeaderFooterSettingComponent,
        QuoteTemplatePricingSettingComponent,
        QuoteTemplateTotalPerContainerSetting,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AddEditQuoteTemplateSectionComponent": { myResult = AddEditQuoteTemplateSectionComponent; break; }
            case "AddQuoteTemplateFromLibraryComponent": { myResult = AddQuoteTemplateFromLibraryComponent; break; }
            case "AdvanceDesignTableComponent": { myResult = AdvanceDesignTableComponent; break; }
            case "EditQuoteTemplateComponent": { myResult = EditQuoteTemplateComponent; break; }
            case "NewQuoteTemplateComponent": { myResult = NewQuoteTemplateComponent; break; }
            case "PageAreaHeaderFooterComponent": { myResult = PageAreaHeaderFooterComponent; break; }
            case "PreviewQuoteTemplateReportComponent": { myResult = PreviewQuoteTemplateReportComponent; break; }
            case "QuoteTemplateGeneralSetting": { myResult = QuoteTemplateGeneralSetting; break; }
            case "QuoteTemplateHeaderDetailsSettingComponent": { myResult = QuoteTemplateHeaderDetailsSettingComponent; break; }
            case "QuoteTemplateHeaderFooterSettingComponent": { myResult = QuoteTemplateHeaderFooterSettingComponent; break; }
            case "QuoteTemplatePricingSettingComponent": { myResult = QuoteTemplatePricingSettingComponent; break; }
            case "QuoteTemplateTotalPerContainerSetting": { myResult = QuoteTemplateTotalPerContainerSetting; break; }
        }

        return myResult;
    }
}