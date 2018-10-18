
declare var System: any;
declare var window: any;
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {EventTypePM} from '../../../Infrastructure/EntityPMs/EventTypePM';

import {CardLogDetails} from '../../DataContracts/CardLogDetails';

export class ActivityZoomItemViewModel {

    public CardName: string;
    public Activities: number;
    public ContactName: string;
    
    LogDetails: CardLogDetails;

    constructor(logDetails: CardLogDetails) {
        this.LogDetails = logDetails;
        this.CardName = logDetails.CardName
        this.Activities = logDetails.NumberOfActivities
        this.ContactName = logDetails.ContactName

    }


















}