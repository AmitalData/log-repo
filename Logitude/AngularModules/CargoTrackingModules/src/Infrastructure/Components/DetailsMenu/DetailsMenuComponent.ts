import { Component, Input, OnInit, Output, EventEmitter, AfterContentInit, AfterViewInit } from '@angular/core';


@Component({
    selector: 'details-menu',
    templateUrl: './DetailsMenuComponent.html',
    styleUrls: ['./DetailsMenuComponent.css']
})
export class DetailsMenuComponent implements AfterViewInit
{

    @Output() ButtonClicked: EventEmitter<string> = new EventEmitter<string>();

    @Input() MenuTitle: string;
    @Input() Buttons: MenuButton[] = [];
    MenuHeight: number = 44; // 44 title height
    ITEM_HEIGHT: number = 40;
    constructor() { }

    ngAfterViewInit(): void
    {
        this.MenuHeight = 44 + this.Buttons.length * this.ITEM_HEIGHT;
    }


    private show: boolean;
    @Input()
    public get Show(): boolean { return this.show; }
    public set Show(v: boolean)
    {
        this.show = v;
    }

    MenuButtonClicked(code: string)
    {
        this.ButtonClicked.emit(code);
        this.Show = false;
        console.log("[details menu] button clicked: " + code);

    }

}

export class MenuButton
{

    public Code: string;
    public Name: string;


}
