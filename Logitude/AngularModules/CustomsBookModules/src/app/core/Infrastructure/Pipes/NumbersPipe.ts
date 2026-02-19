import {Pipe} from '@angular/core';
import {FormatTool} from '../Tools';

@Pipe({ name: 'NumbersPipe', standalone: true })

export class NumbersPipe {
    transform(myNumber: number, myFormat: string): string {
        return FormatTool.FormatNumber(myNumber, myFormat);
    }
}