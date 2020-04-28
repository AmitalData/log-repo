import {Subject } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

export class ApiFiltersEvent extends Subject<ApiQueryFilters>
{
    constructor() { super(); }
    emit(value) { super.next(value); }
}

//import {CustomerEventEmitter} from './customer-event-emitter';
export class PubSubService {
    Stream: ApiFiltersEvent;
    constructor() { this.Stream = new ApiFiltersEvent(); }
}
