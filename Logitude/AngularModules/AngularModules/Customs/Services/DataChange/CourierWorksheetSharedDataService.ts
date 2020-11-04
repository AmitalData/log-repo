import { BehaviorSubject } from 'rxjs';
import { Injectable} from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


@Injectable()
export class CourierWorksheetSharedDataService {
  public _SelectedItems: ObservableCollection = new ObservableCollection([]);
  public SupperssOnRowSelectedAction: boolean = false;
    private messageSource = new BehaviorSubject('default message');
    CurrentMessage = this.messageSource.asObservable();
    WebAPICourierGWMessageECTHRDataMaman: string;
    public IsDisplayOnly: boolean = false;
    SelectedTabFilterCode: string;
    constructor() { }

    SendNextMessage(message: string) {
        this.messageSource.next(message)
    }

}
