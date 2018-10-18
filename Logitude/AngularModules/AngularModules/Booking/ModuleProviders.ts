import {BookingAnswerStatusListService} from './Services/StandardLists/BookingAnswerStatusListService';
import {BookingLevelListService} from './Services/StandardLists/BookingLevelListService';
import {BookingListService} from './Services/StandardLists/BookingListService';
import {BookingProductListService} from './Services/StandardLists/BookingProductListService';
import {BookingSpaceAllocationListService} from './Services/StandardLists/BookingSpaceAllocationListService';
import {BookingStatusListService} from './Services/StandardLists/BookingStatusListService';
import {FFRStatusListService} from './Services/StandardLists/FFRStatusListService';
import {FlightsSchedulesRequestListService} from './Services/StandardLists/FlightsSchedulesRequestListService';
import {FlightsSchedulesRequestStatusListService} from './Services/StandardLists/FlightsSchedulesRequestStatusListService';

import {BookingPMService} from './Services/StandardPMs/BookingPMService';
import {FlightsSchedulesRequestPMService} from './Services/StandardPMs/FlightsSchedulesRequestPMService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            //List
            case "BookingAnswerStatusListService": { myResult = new BookingAnswerStatusListService(); break; }
            case "BookingLevelListService": { myResult = new BookingLevelListService(); break; }
            case "BookingListService": { myResult = new BookingListService(); break; }
            case "BookingProductListService": { myResult = new BookingProductListService(); break; }
            case "BookingSpaceAllocationListService": { myResult = new BookingSpaceAllocationListService(); break; }
            case "BookingStatusListService": { myResult = new BookingStatusListService(); break; }
            case "FFRStatusListService": { myResult = new FFRStatusListService(); break; }
            case "FlightsSchedulesRequestListService": { myResult = new FlightsSchedulesRequestListService(); break; }
            case "FlightsSchedulesRequestStatusListService": { myResult = new FlightsSchedulesRequestStatusListService(); break; }

            // PM
            case "BookingPMService": { myResult = new BookingPMService(); break; }
            case "FlightsSchedulesRequestPMService": { myResult = new FlightsSchedulesRequestPMService(); break; }
        }

        return myResult;
    }
}