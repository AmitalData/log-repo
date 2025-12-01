import {Pipe} from '@angular/core';
import {AppTool} from '../Tools';

@Pipe({ name: 'PaddingPipe' })

export class PaddingPipe {
    transform(number: any, Direction: string, Count: number, Character: string): string {
        var myResult = "";

        if (!AppTool.IsNullOrEmpty(number)) {
            myResult = number.toString();

            if (!AppTool.IsNullOrEmpty(myResult)) {
                if (Count > 0) {
                    if (!AppTool.IsNullOrEmpty(Character)) {
                        if (Direction == "L") {
                            while (myResult.length < Count) {
                                myResult = Character + myResult;
                            }
                        }

                        else if (Direction == "R") {
                            while (myResult.length < Count) {
                                myResult = myResult + Character;
                            }
                        }
                    }
                }
            }
        }

        return myResult;
    }
}