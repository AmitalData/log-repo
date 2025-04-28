import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'app-SearchListDDL',
    template: `
        <div class="dropdown-container">
            <ul *ngIf="showDropdown && dropdownOptions?.length > 0" class="dropdown-list">
                <li *ngFor="let option of dropdownOptions" (click)="optionSelected.emit(option)" class="dropdown-item">
                    <ng-container *ngIf="option == 'all'; else notAll">
                        <a href="#" (click)="$event.preventDefault()">{{ 'General.O.ViewAll' | TextCodeTranslationPipe }}</a>
                    </ng-container>
                    <ng-template #notAll>
                        <!-- <span>{{ option['declarationNumber'] }} {{ option['declarationNumber'] == null }} {{ option['declarationNumber'] == '' }} {{ option['declarationNumber'] == undefined }}</span> -->
                        <span *ngFor="let label of labels; let first = first;" [ngSwitch]="label.name">
                            <span *ngIf="!first && label.lengthTemp !== 0" >&nbsp;|&nbsp;</span>
                            <img *ngSwitchCase="'transportModeId'"  [src]="'./Images/' + (option[label.name] === 'O' ? 'Vessel' : option[label.name] === 'A' ? 'Airline' : 'Trucker') + '.png'" alt="{{ option[label.name] }}" />
                            <span *ngSwitchCase="'createDateTime'">{{ option[label.name] | DateTimePipe:'D' }}</span>
                            <span *ngSwitchDefault [style.width]="label.lengthTemp > 0 ? (label.lengthTemp.toString() + 'px') : 'auto'">{{ option[label.name] }}</span>
                        </span>
                    </ng-template>
                </li>
            </ul>
        </div>
    `,
    styles: [`
        .dropdown-container {
            position: relative;
            z-index: 1;
        }

        .dropdown-list {
            position: absolute;
            top: 100%;
            left: 0;
            border: 1px solid #ccc;
            background: #fff;
            margin: 0;
            padding: 0;
            list-style-type: none;            
            min-width: 100%;
            width: auto;
        }

        .dropdown-item {
            padding: 4px;
            cursor: pointer;
        }

        .dropdown-item:hover {
            outline: 1px solid #b5c9d8;
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #cce0f7 100%);
            background: -webkit-linear-gradient(top, #ffffff 0%, #cce0f7 100%);
            background: linear-gradient(to bottom, #ffffff 0%, #cce0f7 100%);
        }        

        .dropdown-item img {
            width: 15px;
            vertical-align: baseline;
        }

        .dropdown-item a, .dropdown-item span {
            font-size: 14px;
            display: inline-block;
            overflow: hidden;
            text-overflow: ellipsis;
        }
    `]
})
export class SearchListDDLComponent implements OnInit {
    @Input()
    private _dropdownOptions: any[] = [];
    public get dropdownOptions(): any[] {
        return this._dropdownOptions;
    }
    public set dropdownOptions(options: any[]) {
        if (!options) return;
        console.log('dropdownOptions1', new Date().toISOString().substring(14, 23))

        options = [...options];

        if (this.maxResults != null && options?.length > this.maxResults) {
            options.splice(this.maxResults, options.length - this.maxResults);
            options.push('all');
        }

        this._dropdownOptions = options;
        this.initLabelLength();
        console.log('dropdownOptions2', new Date().toISOString().substring(14, 23))
    }
    @Input() showDropdown: boolean = true;
    @Input() maxResults: number = null;
    labels: DDLLable[] = [];
    public set displayPattern(pattern: string) {
        if (!pattern) return;
        this.labels = [];
        const matches = pattern.match(/{(.*?)}/g);
        if (!matches) return;

        matches.forEach(word => {
            const val = word.slice(1, -1).split(':');
            this.labels.push({
                name: val[0].trim(),
                length: val[1] ? parseInt(val[1]) : null
            });
        });

        this.initLabelLength();
    }
    @Output() optionSelected: EventEmitter<any> = new EventEmitter<any>();
    public defaultLabelLength: number = 20;

    ngOnInit() {
        this.optionSelected.subscribe((option: any) => this.showDropdown = false);
    }

    onDestroy() {
        this.optionSelected.unsubscribe();
    }

    initLabelLength(): void {
        if (this.dropdownOptions && this.dropdownOptions.length > 0) 
            this.labels.forEach((l: DDLLable) => {                
                const value: any = this.dropdownOptions.find(x => x[l.name] != null && x[l.name] != '' && x[l.name] != undefined)
                l.lengthTemp = value == undefined ? 0 : l.length || 'auto';
            });
    }
    
}

export interface DDLLable {
    name: string;
    length?: number;
    lengthTemp?: number | string;
}