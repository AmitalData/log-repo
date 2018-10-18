import {Component, ChangeDetectionStrategy} from '@angular/core';

@Component({
    selector: 'ScrollViewer',
    inputs: ['Small', 'IsTowSides'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <div class="MediaFill">
        <div class="LogitudeScrollViewer" [class.LogitudeSmallScrollViewer]="Small" [ngStyle]="{'overflow-x': IsTowSides ? 'auto' : 'hidden'}">
            <ng-content></ng-content>
        </div>
    </div>
    `,

    styles:
    [`
    .LogitudeScrollViewer {
        height: 100%;
        width: 100%;
        max-height: 100%;
        max-width: 100%;
        overflow-y: auto;        
        position: relative;
    }
    `],

    // overflow-x: hidden;
})

export class ScrollViewer {
    public Small: boolean = false;
    public IsTowSides: boolean = false;
}