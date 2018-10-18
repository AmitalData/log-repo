import {Pipe} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {ControlsIdCounter} from '../../Infrastructure/Utilities/ControlsIdCounter';

@Pipe({ name: 'IdGeneratorPipe' })

export class IdGeneratorPipe {
    transform(value: string): string {

        if (value) {
            value = AppTool.Replace(value, " ", "");
        }

        var UnuieqDomId: string = value;

        if (this.CheckIfExists(value)) {
            var counterId = ControlsIdCounter.GetNextControlIdCounter(value);

            if (counterId != null) {
                UnuieqDomId = UnuieqDomId + "_" + counterId;
            }
        }

        return UnuieqDomId;

    }

    private CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }
}

