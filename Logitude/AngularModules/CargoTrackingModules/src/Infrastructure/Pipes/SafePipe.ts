import {Pipe, PipeTransform} from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';

@Pipe({ name: 'SafePipe' })

export class SafePipe implements PipeTransform {
    constructor(private sanitizer: DomSanitizer) { }
    transform(URI) {
        return this.sanitizer.bypassSecurityTrustResourceUrl(URI);
    }
}