import {Component, OnInit, ChangeDetectionStrategy, ElementRef} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';

@Component({
    selector: 'HyperlinkQuery',
    inputs: ['Text', 'IsEnabled'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `      <button id="{{'ReportID' | IdGeneratorPipe}}" class="HyperlinkQueryButtonControl" [disabled]="!IsEnabled" tabindex="-1">            
            {{Text}}
       
            <span style="pointer-events: none;">            
                <ng-content></ng-content>
            </span>
        </button>
    `,

    styles:
    [`
    .HyperlinkQueryButtonControl:disabled {
        opacity: 0.5;
        cursor: default;

    }

    .HyperlinkQueryButtonControl:hover:not(:disabled), .HyperlinkQueryButtonControl:focus:not(:disabled) {
        color: #1E8DC4;
    }

    .HyperlinkQueryButtonControl {
        height: 21px !important;
        width: auto !important;
        line-height: 21px !important;
        background: none !important;
        border: none !important;
        padding: 0 !important;
        padding-left: 10px !important;
        color: #282E30;
        font-size: 12px !important;
        cursor: pointer;
        display: block !important;        
        white-space: nowrap;        
    }
    `],
})

export class HyperlinkQuery implements OnInit {
    constructor(private elementRef: ElementRef) {

    }

    ngOnInit() {
        this.SetComponent();
    }

    SetComponent() {
        if (this.elementRef) {
            var nativeElement = this.elementRef.nativeElement
            if (nativeElement) {

                if (this.isEnabled) {
                    nativeElement.style.pointerEvents = "auto";
                }

                else {
                    nativeElement.style.pointerEvents = "none";
                }
            }
        }
    }

    private myText: string = null;
    get Text() { return this.myText; }
    set Text(value: string) {
        if (this.myText != value) {
            this.myText = value;
        }
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
            this.SetComponent();
        }
    }
}

