import {MarkUpOPTypeListService} from './Services/StandardLists/MarkUpOPTypeListService';
import {QuoteOPClosingReasonListService} from './Services/StandardLists/QuoteOPClosingReasonListService';
import {QuoteOPCustomerTypeListService} from './Services/StandardLists/QuoteOPCustomerTypeListService';
import {QuoteOPListService} from './Services/StandardLists/QuoteOPListService';
import {QuoteOPStageListService} from './Services/StandardLists/QuoteOPStageListService';
import {QuoteOPTemplateListService} from './Services/StandardLists/QuoteOPTemplateListService';
import {QuoteOPTypeListService} from './Services/StandardLists/QuoteOPTypeListService';
import {QuoteOPRatingListService} from './Services/StandardLists/QuoteOPRatingListService';
import {QuoteOPPMService} from './Services/StandardPMs/QuoteOPPMService';
import {QuoteOPStagePMService} from './Services/StandardPMs/QuoteOPStagePMService';

import {QuoteOPMenuButtonsHandler} from './Components/MenuButtons/QuoteOPMenuButtonsHandler';
//  veiw !! import { QuoteOPFollowUpListService } from './Services/StandardLists/QuoteOPFollowUpListService';
import { QuoteOPClosingReasonPMService } from './Services/StandardPMs/QuoteOPClosingReasonPMService';
import { OPSpecialServicesTypeListService } from './Services/StandardLists/OPSpecialServicesTypeListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "MarkUpOPTypeListService": { myResult = new MarkUpOPTypeListService(); break; }
            case "QuoteOPClosingReasonListService": { myResult = new QuoteOPClosingReasonListService(); break; }
            case "QuoteOPCustomerTypeListService": { myResult = new QuoteOPCustomerTypeListService(); break; }
            case "QuoteOPListService": { myResult = new QuoteOPListService(); break; }
            case "QuoteOPStageListService": { myResult = new QuoteOPStageListService(); break; }
            case "QuoteOPTemplateListService": { myResult = new QuoteOPTemplateListService(); break; }
            case "QuoteOPTypeListService": { myResult = new QuoteOPTypeListService(); break; }
            case "QuoteOPRatingListService": { myResult = new QuoteOPRatingListService(); break; }
            case "QuoteOPPMService": { myResult = new QuoteOPPMService(); break; }
            case "QuoteOPStagePMService": { myResult = new QuoteOPStagePMService(); break; }
            
            case "QuoteOPMenuButtonsHandler": { myResult = new QuoteOPMenuButtonsHandler(); break; }
            // view todo case "QuoteOPFollowUpListService": { myResult = new QuoteOPFollowUpListService(); break; }
            case "QuoteOPClosingReasonPMService": { myResult = new QuoteOPClosingReasonPMService(); break; }
            case "OPSpecialServicesTypeListService": { myResult = new OPSpecialServicesTypeListService(); break; }
                
        }

        return myResult;
    }
}

