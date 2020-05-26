import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


@Injectable()
export class SpotlightSharedDataService {
    public IsDisplayButtonSend: boolean;


    public SupperssOnRowSelectedAction: boolean = false;
    private messageSource = new BehaviorSubject('default message');
    CurrentMessage = this.messageSource.asObservable();
    WebAPICourierGWMessageECTHRDataMaman: string;
    public IsDirty: boolean = false;
    constructor() { }

    SendNextMessage(message: string) {
        this.messageSource.next(message)
    }

}
