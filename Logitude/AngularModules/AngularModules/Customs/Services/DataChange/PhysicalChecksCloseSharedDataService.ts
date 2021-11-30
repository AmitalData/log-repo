import { BehaviorSubject } from 'rxjs';
import { Injectable} from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


@Injectable()
export class PhysicalChecksCloseSharedDataService {
    public _SelectedItems: ObservableCollection = new ObservableCollection([]);
    public IsDisplayButtonClose: boolean = false;

  public SupperssOnRowSelectedAction: boolean = false;
    private messageSource = new BehaviorSubject('default message');
    CurrentMessage = this.messageSource.asObservable();
    public IsDisplayOnly: boolean = false;
    constructor()
    {
        debugger;
    }

    SendNextMessage(message: string) {
        this.messageSource.next(message)
    }

}
