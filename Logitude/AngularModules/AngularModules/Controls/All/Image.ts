import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool, ImageTool} from  '../../Infrastructure/Tools';

@Component({
    selector: "Image",
    inputs: ['Src', 'Width', 'Height', 'Title'],
    template:
    `
        <div *ngIf="IsDispaly" [style.width.px]="Width" [style.height.px]="Height">
             <img [attr.src]="Src" [style.width.px]="Width" [style.height.px]="Height" [attr.title]="Title" (error)="onError()"/>
         </div> 
    `,
})

export class Image {

    public ControlId: string = null;
    public Width: number = 16;
    public Height: number = 16;
    public Src: string = null;
    public Title: string = null;
    public IsDispaly = true;
    constructor() {

    }
    onError() {
        this.IsDispaly = false;
    }
}