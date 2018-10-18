import {Pipe} from 'angular2/core';

@Pipe({ name: 'StringToColorPipe' })

export class StringToColorPipe {

    transform(value: string): string {

        var myResult: string = "#282E30";

        if (value != null) {
            switch (value) {
                case "Draft": {
                    myResult = "Orange";
                    break;
                }
            }
        }

        return myResult;
    }
}
