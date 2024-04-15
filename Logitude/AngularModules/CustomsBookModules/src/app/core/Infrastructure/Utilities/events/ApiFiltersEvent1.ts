import { Subject } from 'rxjs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

export class ApiFiltersEvent1 extends Subject<ApiQueryFilters>
{
    constructor() { super(); }
    emit(value) { super.next(value); }
}

//import {CustomerEventEmitter} from './customer-event-emitter';
export class PubSubService1 {
    Stream: ApiFiltersEvent1;
    constructor() { this.Stream = new ApiFiltersEvent1(); }
}
