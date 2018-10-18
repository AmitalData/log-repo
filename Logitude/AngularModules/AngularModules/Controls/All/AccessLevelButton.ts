import {Component, OnInit, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {FeaturePM} from '../../Infrastructure/EntityPMs/FeaturePM';
import {AppTool} from '../../Infrastructure/Tools';

@Component({
    selector: 'AccessLevelButton',
    inputs: ['IsEnabled', 'Feature', 'AccessLevelCode'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <div class="MediaFill">
        <button class="AccessLevelButton CenterCenter" [disabled]="!IsEnabled" tabindex="-1" *ngIf="Feature" (click)="ButtonClicked()" [attr.title]="AccessLevelName">
            <img [attr.src]="Source" style="visibility: inherit; vertical-align: middle; display: block;" />
        </button> 
    </div>
    `,

    styles:
    [`
    .AccessLevelButton:disabled {
        opacity: 0.5;
        cursor: default;        
    }

    .AccessLevelButton {
        height: 16px !important;
        width: 16px !important;        
        background: none !important;
        border: none !important;
        padding: 0 !important;        
        cursor: pointer;        
    }
    `],
})

export class AccessLevelButton implements OnInit {
    public Feature: FeaturePM;
    private isDown: boolean = false;
    @Output() AccessLevelChanged: EventEmitter<string> = new EventEmitter<string>();
    constructor() {

    }

    ngOnInit() {
        if (this.Feature) {
            this.AccessLevelCode = this.Feature.AccessLevelCode;
            this.isDown = this.AccessLevelCode == "OR" ? true : false;
            this.SetSource();
        }
    }

    SetSource() {
        if (this.Feature) {

            var levelCode = this.AccessLevelCode;
            if (AppTool.IsNullOrEmpty(levelCode)) {
                levelCode = "NO";
            }

            this.Source = "./_Resources/Images/Icons/AccessLevels/" + levelCode + ".png";
        }
    }

    private accessLevelCode: string = null;
    get AccessLevelCode() { return this.accessLevelCode; }
    set AccessLevelCode(value: string) {
        if (this.Feature != null) {
            if (this.accessLevelCode != value) {
                this.accessLevelCode = value;
                this.SetSource();

                switch (value) {
                    case "OR": { this.AccessLevelName = "Organization"; break; }
                    case "PR": { this.AccessLevelName = "Parent"; break; }
                    case "BU": { this.AccessLevelName = "Business Unit"; break; }
                    case "US": { this.AccessLevelName = "User"; break; }
                    default: { this.AccessLevelName = "None"; break; }
                }
            }
        }
    }

    private accessLevelName: string = null;
    get AccessLevelName() { return this.accessLevelName; }
    set AccessLevelName(value: string) {
        if (this.Feature != null) {
            if (this.accessLevelName != value) {
                this.accessLevelName = value;
            }
        }
    }

    private mySource: string = null;
    get Source() { return this.mySource; }
    set Source(value: string) {
        if (this.mySource != value) {
            this.mySource = value;
        }
    }

    private isEnabled: boolean = true;
    get IsEnabled() { return this.isEnabled; }
    set IsEnabled(value: boolean) {
        if (this.isEnabled != value) {
            this.isEnabled = value;
        }
    }

    ButtonClicked() {
        if (this.Feature) {
            var levelCode = this.AccessLevelCode;
            if (AppTool.IsNullOrEmpty(levelCode)) {
                levelCode = "NO";
            }

            if (this.Feature.IsBusinessUnitEnabled == false) {
                if (AppTool.IsNullOrEmpty(levelCode) || levelCode == "NO") {
                    levelCode = "OR";
                }

                else {
                    levelCode = "NO";
                }
            }

            else {
                if (this.isDown) {
                    switch (levelCode) {
                        case "OR": { levelCode = "PR"; break; }
                        case "PR": { levelCode = "BU"; break; }
                        case "BU": { levelCode = "US"; break; }
                        case "US": { levelCode = "NO"; this.isDown = false; break; }
                    }
                }

                else {
                    switch (levelCode) {
                        case "NO": { levelCode = "US"; break; }
                        case "US": { levelCode = "BU"; break; }
                        case "BU": { levelCode = "PR"; break; }
                        case "PR": { levelCode = "OR"; this.isDown = true; break; }
                    }
                }
            }

            if (this.AccessLevelCode != levelCode) {
                this.AccessLevelCode = levelCode;
                this.AccessLevelChanged.emit(levelCode);
            }
        }
    }
}