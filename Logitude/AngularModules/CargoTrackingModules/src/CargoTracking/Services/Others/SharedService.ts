import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';

Injectable();

export class SharedService {
    isAdvancedFilterOpened$ = new BehaviorSubject(false);

    updateValue(value) {
        this.isAdvancedFilterOpened$.next(value);
    }
}
