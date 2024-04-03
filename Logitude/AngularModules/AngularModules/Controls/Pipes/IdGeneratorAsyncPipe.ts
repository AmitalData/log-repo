import {Pipe} from '@angular/core';
import { Observable, Observer } from 'rxjs';
import {AppTool} from '../../Infrastructure/Tools';
import {ControlsIdCounter} from '../../Infrastructure/Utilities/ControlsIdCounter';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

@Pipe({ name: 'IdGeneratorAsyncPipe' })

export class IdGeneratorAsyncPipe {
    transform(value: string): Observable<string> {

        if (value) {
            value = AppTool.Replace(value, " ", "");
            if (this.CheckIfIsProtractorRunning() == false) {
                value = AppTool.Replace(value, ".", "");
            }
        }

         var idObservable = new Observable<string>((observer: Observer<string>) => {
            setTimeout(() => observer.next(this.GenerateId(value)), 1000);
          });

        return idObservable; 

    }
    private GenerateId(value : string):string{
        var UnuieqDomId: string = value;
        if (this.CheckIfExists(value)) {
            var counterId = ControlsIdCounter.GetNextControlIdCounter(value);

            if (counterId != null) {
                UnuieqDomId = UnuieqDomId + "_" + counterId;
            }
           
        }
        return UnuieqDomId;
    }

    CheckIfIsProtractorRunning() {
        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu) {
                    var menuName = SessionLocator.ExternalParams.Menu.toLocaleLowerCase();
                    if (menuName == "protractor") {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }
}

