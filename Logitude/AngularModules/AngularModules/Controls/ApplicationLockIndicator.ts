import {Component} from '@angular/core';

@Component({
    selector: 'ApplicationLockIndicator',
    inputs: ['IsLocked'],

    styles:
    [`
    .ApplicationLockIndicatorControlLayout {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        opacity: 0.5;
        background: white;
        z-index: 99;
    }

    .ApplicationLockIndicatorControl {
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        margin: auto;
        z-index: 100;
    }
  `],

    template:
    `
    <div [hidden]="!IsLocked" class="ApplicationLockIndicatorControlLayout" tabindex="-1" contenteditable="false"></div>
    <div [hidden]="!IsLocked" class="ApplicationLockIndicatorControl">
       
    </div>
    `,
})

export class ApplicationLockIndicator {
    
    public IsLocked: boolean = false;
}