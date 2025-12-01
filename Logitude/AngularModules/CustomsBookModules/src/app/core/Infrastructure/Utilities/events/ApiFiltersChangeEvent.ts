import {Subject } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

export class ApiFiltersChangeEvent extends Subject<ApiQueryFilters>
{
    constructor() { super(); }
    emit(value) { super.next(value); }
}

//import {CustomerEventEmitter} from './customer-event-emitter';
export class PubSubFiltersChangeEventService {
    Stream: ApiFiltersChangeEvent;
    constructor() {
        if (this.Stream == null) {
            this.Stream = new ApiFiltersChangeEvent();
        }
    }
}
