import {Pipe} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';

@Pipe({ name: 'CountryFlagPipe' })

export class CountryFlagPipe {
    transform(value: string): string {
        var myResult: string = "";

        if (!AppTool.IsNullOrEmpty(value)) {
            myResult = "./Images/Flags/" + value + ".png";
        }

        return myResult;
    }
}
