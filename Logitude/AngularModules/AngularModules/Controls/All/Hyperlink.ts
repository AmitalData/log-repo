import {Component, OnInit, ChangeDetectionStrategy, ElementRef} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';

@Component({
    selector: 'Hyperlink',
    inputs: ['Text', 'FontSize', 'Color', 'IsEnabled', 'Title', 'DataCy'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `    
        <button [attr.data-cy]="DataCy" class="HyperlinkButtonControl" [disabled]="!IsEnabled" [style.font-size.px]="FontSize" [style.color]="Color" tabindex="-1">
            {{Text}}
            
            <span style="pointer-events: none;" [style.font-size.px]="FontSize" [style.color]="Color">            
                <ng-content></ng-content>
            </span>
        </button>    
    `,

    styles:
    [`
    .HyperlinkButtonControl:disabled {
        opacity: 0.5;
        cursor: default;        
    }

    .HyperlinkButtonControl:hover:not(:disabled), .HyperlinkButtonControl:focus:not(:disabled) {
        color: #1E8DC4;
    }

    .HyperlinkButtonControl {
        height: auto !important;
        width: auto !important;
        line-height: unset !important;
        background: none !important;
        border: none !important;
        padding: 0 !important;
        color: #1E4AC4;
        cursor: pointer;
        text-decoration: underline;
        display: inline-block !important;        
        white-space: nowrap;        
    }
    `],
})

export class Hyperlink implements OnInit {
    public FontSize: number = 10;
    public Title: string = null;
    public DataCy: string = null;

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

    private myColor: string = "#1E4AC4";
    get Color() { return this.myColor; }
    set Color(value: string) {
        if (this.myColor != value) {

            if (AppTool.IsNullOrEmpty(value)) {
                value = "#1E4AC4";
            }

            this.myColor = value;
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

