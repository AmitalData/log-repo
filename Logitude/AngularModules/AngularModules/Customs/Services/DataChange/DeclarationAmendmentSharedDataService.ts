import { BehaviorSubject } from 'rxjs';
import { Injectable} from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


@Injectable()
export class DeclarationAmendmentSharedDataService {
     private messageSource = new BehaviorSubject('default message');
    CurrentMessage = this.messageSource.asObservable();
     public IsDisplayOnly: boolean = false;
    constructor() { }

    SendNextMessage(message: string) {
        this.messageSource.next(message)
    }

}
