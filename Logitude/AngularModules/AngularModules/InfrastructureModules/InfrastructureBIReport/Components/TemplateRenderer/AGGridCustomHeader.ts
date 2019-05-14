import { Component, ViewChild, ElementRef } from '@angular/core';
import { ILoadingOverlayComp } from "ag-grid-community"; 
import { IHeaderAngularComp  } from 'ag-grid-angular';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
    selector: 'app-loading-overlay',
    template: `
        <div class="MediaFill">
            <div *ngIf="params.enableMenu && params.menuIcon !='fa-list-ol'" #menuButton class="customHeaderMenuButton" (click)="onMenuClicked($event)">
                <i class="fa {{params.menuIcon}}"></i>
            </div>
            <div *ngIf="params.enableMenu && params.menuIcon =='fa-list-ol'" #menuButton class="customHeaderMenuButton" (click)="onMenuClicked($event)">
                <img  src="./Images/numberIcon.png" [className]="'LeftCenter'"/>
            </div>
            <div class="customHeaderLabel TextTrimming">{{params.displayName}}</div>
            <div class="action-holders__sort-number action-holders__sorting">
                <p id="sortingOrder{{colId}}"></p>
            </div>
            <div *ngIf="params.enableSorting" (click)="onSortRequested('asc', $event)" [ngClass]="ascSort" class="customSortDownLabel"><i class="fa fa-long-arrow-up"></i></div> 
            <div *ngIf="params.enableSorting" (click)="onSortRequested('desc', $event)" [ngClass]="descSort" class="customSortUpLabel"><i class="fa fa-long-arrow-down"></i></div> 
            <div *ngIf="params.enableSorting" (click)="onSortRequested('', $event)" [ngClass]="noSort" class="customSortRemoveLabel"><i class="fa fa-times"></i></div>
        </div>
    `,
    styles: [
        '../_Resources/Froala/css/font-awesome.min.css',
        `
        .action-holders__sort-number.action-holders__sorting {
            position: relative;
            float: left;
            width: 7px;
            height: 32px;
            margin-right: 3px;
            margin-left: 3px;
        }
        .customHeaderLabel {
            float: left;
            min-width: 70px;   
            margin: 0 0 0 15px;
         }
        .customHeaderMenuButton, 
        .customSortDownLabel, 
        .customSortUpLabel, 
        .customSortRemoveLabel 
        {
            float: left;
            margin: 0 0 0 5px;
        }
            .customSortUpLabel {
                margin: 0;
            }

            .customSortRemoveLabel {
                font-size: 11px;
            }

            .active {
                color: cornflowerblue;
            }
    `
    ]
})
export class AGGridCustomHeader implements IHeaderAngularComp  {
    public params: any;
    public ascSort: string;
    public colId;
    public sortNumber = 0;
    public descSort: string;
    public noSort: string;

    @ViewChild('menuButton', {read: ElementRef}) public menuButton;

    agInit(params): void {
        this.params = params;
        params.column.addEventListener('sortChanged', this.onSortChanged.bind(this));
        this.onSortChanged();
        this.colId = this.params.column.getColId().replace(/\s/g, '');
    }

    refresh() {
        return false; 
    }
    onMenuClicked() {
        //this.params.showColumnMenu(this.menuButton.nativeElement);
    };

    onSortChanged() {
        this.checkSortOrder();
        this.ascSort = this.descSort = this.noSort = 'inactive';
        if (this.params.column.isSortAscending()) {
            this.ascSort = 'active';
        } else if (this.params.column.isSortDescending()) {
            this.descSort = 'active';
        } else {
            this.noSort = 'active';
        }
    }
    
   /**
   * Check for sort Order
   */
    checkSortOrder() {
        const sortingArray = this.params.api.sortController.getColumnsWithSortingOrdered();
        let j: number;
        if (sortingArray.length > 0) {
            setTimeout(() => {
                for (j = 0; j < sortingArray.length; j++) {
                    this.sortNumber = j + 1;
                    const sortingDom = <HTMLElement>document.getElementById('sortingOrder' + sortingArray[j].colId.replace(/\s/g, ''))
                    sortingDom.innerHTML = this.sortNumber.toString();
                }
            });
        }
    }

    onSortRequested(order, event) {
        this.params.setSort(order, true);
        this.checkSortOrder();
    }
}
