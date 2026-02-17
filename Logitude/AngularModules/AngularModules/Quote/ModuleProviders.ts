import {MarkUpTypeListService} from './Services/StandardLists/MarkUpTypeListService';
import {QuoteClosingReasonListService} from './Services/StandardLists/QuoteClosingReasonListService';
import {QuoteCustomerTypeListService} from './Services/StandardLists/QuoteCustomerTypeListService';
import {QuoteListService} from './Services/StandardLists/QuoteListService';
import {QuoteStageListService} from './Services/StandardLists/QuoteStageListService';
import {QuoteTemplateListService} from './Services/StandardLists/QuoteTemplateListService';
import {QuoteTypeListService} from './Services/StandardLists/QuoteTypeListService';
import {QuoteRatingListService} from './Services/StandardLists/QuoteRatingListService';
import {QuotePMService} from './Services/StandardPMs/QuotePMService';
import {QuoteStagePMService} from './Services/StandardPMs/QuoteStagePMService';
//import {QuoteTemplatePMService} from './Services/StandardPMs/QuoteTemplatePMService';
import {QuoteMenuButtonsHandler} from './Components/MenuButtons/QuoteMenuButtonsHandler';
import {QuoteFollowUpListService} from './Services/StandardLists/QuoteFollowUpListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "MarkUpTypeListService": { myResult = new MarkUpTypeListService(); break; }
            case "QuoteClosingReasonListService": { myResult = new QuoteClosingReasonListService(); break; }
            case "QuoteCustomerTypeListService": { myResult = new QuoteCustomerTypeListService(); break; }
            case "QuoteListService": { myResult = new QuoteListService(); break; }
            case "QuoteStageListService": { myResult = new QuoteStageListService(); break; }
            case "QuoteTemplateListService": { myResult = new QuoteTemplateListService(); break; }
            case "QuoteTypeListService": { myResult = new QuoteTypeListService(); break; }
            case "QuoteRatingListService": { myResult = new QuoteRatingListService(); break; }
            case "QuotePMService": { myResult = new QuotePMService(); break; }
            case "QuoteStagePMService": { myResult = new QuoteStagePMService(); break; }
            //case "QuoteTemplatePMService": { myResult = new QuoteTemplatePMService(); break; }
            case "QuoteMenuButtonsHandler": { myResult = new QuoteMenuButtonsHandler(); break; }
            case "QuoteFollowUpListService": { myResult = new QuoteFollowUpListService(); break;}
        }

        return myResult;
    }
}

