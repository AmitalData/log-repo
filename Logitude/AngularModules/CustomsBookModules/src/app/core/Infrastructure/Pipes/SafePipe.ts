import {Pipe, PipeTransform} from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';
import {AppTool} from '../Tools';

@Pipe({ name: 'SafePipe' })

export class SafePipe implements PipeTransform {
    constructor(private sanitizer: DomSanitizer) { }
    transform(URI) {
        return this.sanitizer.bypassSecurityTrustResourceUrl(URI);
    }
}