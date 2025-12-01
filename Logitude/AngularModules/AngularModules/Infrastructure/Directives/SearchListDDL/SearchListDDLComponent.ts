import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FastSearchSettings } from 'Customs/Services/WebServices/AzureSearchWebService';

@Component({
    selector: 'app-SearchListDDL',
    template: `
        <div *ngIf="showDropdown && dropdownOptions?.length > 0" class="dropdown-container" [style.left.px]="left">
            <table class="dropdown-list" [style.width]="DDLWidth">
                <tr *ngFor="let option of getTopResults()" (click)="optionSelected.emit(option)" class="dropdown-item">
                    <ng-container *ngFor="let label of labels; let first = first;" [ngSwitch]="label.name">
                        <td *ngIf="!first && label.lengthTemp !== 0" >&nbsp;|&nbsp;</td>
                        <td><img *ngSwitchCase="'transportModeId'"  [src]="'./Images/' + (option[label.name] === 'O' ? 'Vessel' : option[label.name] === 'A' ? 'Airline' : 'Trucker') + '.png'" alt="{{ option[label.name] }}" /></td>
                        <td> <span *ngSwitchCase="'entityCode'"  [class]="'SourceTypeIcon color-' + option[label.name]">{{option['iconCode']}}</span></td>
                        <td *ngSwitchCase="'createDateTime'">{{ option[label.name] | DateTimePipe:'D' }}</td>
                        <td *ngSwitchDefault [style.width]="label.lengthTemp > 0 ? (label.lengthTemp.toString() + 'px') : 'auto'"
                        [style.maxWidth]="label.lengthTemp > 0 ? (label.lengthTemp.toString() + 'px') : 'auto'">{{ option[label.name] }}</td>
                    </ng-container>
                </tr>
            </table>
            <div *ngIf="dropdownOptions.length > showTopResults;" class='bth-show-all'>
                <a href="#" (click)="optionSelected.emit(showAll);$event.preventDefault()">{{ 'General.O.ViewAll' | TextCodeTranslationPipe }}</a>
            </div>
        </div>
        <div *ngIf="showDropdown && dropdownOptions?.length === 0" class="dropdown-container">
            <div>{{ 'General.O.NoDataFound' | TextCodeTranslationPipe }}</div>
        </div>
    `,
    styles: [`
        .dropdown-container {
            position: absolute;
           
            z-index: 1;
            border: 1px solid #ccc;
            background: #fff;
            padding: 2px 4px;
            min-width: 162px;
        }

        .dropdown-list {
            border-collapse: separate;
            border-spacing: 0px 4px;
        }

        .dropdown-item {
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

        .dropdown-item td {
            font-size: 14px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .bth-show-all {
            background-color: #DFECF7;
        }

        .bth-show-all a {
            font-size: 14px;
        }
    `]
})
export class SearchListDDLComponent implements OnInit {
    public static showAll: string = 'showAll';
    showAll = SearchListDDLComponent.showAll;
    @Input()
    private _dropdownOptions: any[] = [];
    public get dropdownOptions(): any[] {
        return this._dropdownOptions;
    }
    public set dropdownOptions(options: any[]) {
        if (!options) return;

        this._dropdownOptions = [...options];
        this.initLabelLength();
    }
    @Input() showDropdown: boolean = false;
    @Input() showTopResults: number = null;
    labels: DDLLable[] = [];
    DDLWidth: string = '250px';
    left: number = 0;

    public set settings(settings: FastSearchSettings) {
        if (!settings) return;
        this.displayPattern = settings.ddlHtmlLine;
        this.showTopResults = settings.showTopResults;
        this.left = settings.left === -1 ? null : (settings.left ?? 0);

        if (settings.DDLWidth) 
            this.DDLWidth = settings.DDLWidth;        
    }
    public set displayPattern(pattern: string) {
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

    ngOnInit() {
        this.optionSelected.subscribe((option: any) => this.showDropdown = false);
    }

    ngOnDestroy() {
        this.optionSelected.complete();
    }

    initLabelLength(): void {
        if (this.dropdownOptions && this.dropdownOptions.length > 0) 
            this.labels.forEach((l: DDLLable) => {                
                const value: any = this.dropdownOptions.find(x => x[l.name] != null && x[l.name] != '' && x[l.name] != undefined)
                l.lengthTemp = value == undefined ? 0 : l.length || 'auto';
            });
    }    

    getTopResults(): any[] {
        if (!this.dropdownOptions || this.dropdownOptions.length === 0) return [];
        if (this.showTopResults == null || this.showTopResults <= 0) return this.dropdownOptions;
        return this.dropdownOptions.slice(0, this.showTopResults);
    }
}

export interface DDLLable {
    name: string;
    length?: number;
    lengthTemp?: number | string;
}