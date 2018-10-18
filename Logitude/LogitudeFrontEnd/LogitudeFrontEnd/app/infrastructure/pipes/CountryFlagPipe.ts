import {Pipe} from 'angular2/core';

@Pipe({ name: 'CountryFlagPipe' })

export class CountryFlagPipe {

    transform(value: string): string {

        var myResult: string;

        if (value != null) {
            
            myResult = "images/Flags/" + value + ".png";

        }        

        return myResult;
    }
}
