import {Pipe} from '@angular/core';
import {AppTool} from '../Tools';

@Pipe({ name: 'ReplacePipe' })

export class ReplacePipe {
    transform(text: string, replaceFrom: string, replaceTo: string): string {
        var myResult = "";

        if (!AppTool.IsNullOrEmpty(text)) {
            if (!AppTool.IsNullOrEmpty(replaceFrom)) {
                if (!AppTool.IsNullOrEmpty(replaceTo)) {
                    //myResult = text.replace(/replaceFrom/gi, replaceTo);
                    myResult = text.replace(new RegExp(replaceFrom, 'g'), replaceTo);
                }
            }
        }

        return myResult;
    }
}