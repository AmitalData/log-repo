import { TextCodeTranslator } from './../../../Utilities/TextCodeTranslator';
import { ObjectsLocator } from './../../../Locators/ObjectsLocator';
import { Component, OnInit, Output, EventEmitter, Input, AfterViewInit } from '@angular/core';
import { AppTool } from './../../../Tools';
import { ControlsIdCounter } from 'Infrastructure/Utilities/ControlsIdCounter';

@Component({
    selector: 'LogToolTip',
    templateUrl: './LogToolTipComponent.html',
    styleUrls: ['./LogToolTipComponent.css']
})

export class LogToolTipComponent implements AfterViewInit {

    public isRTL: boolean = false;


    @Input() public title: string;
    @Input() public name: string = 'no1';
    @Input() public direction: string = 'bottomright';
    @Input() public mode: string = 'Info';
    @Input() public float: string = null;
    @Input() public bottom: number = 0;
    @Input() public Scrollable: boolean = false;
 
    private counterId:number;

    constructor() {

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
       
    }
    ngOnInit(){
        this.SetComponentId();
    }
    SetComponentId(){
        this.counterId=ControlsIdCounter.GetNextControlIdCounter(name);
        this.name+=this.counterId;
    }
    ngAfterViewInit(): void {

        switch (this.mode) {
            case 'Warning':
                if(this.title==null){
                    this.title = TextCodeTranslator.Translate("General.O.Warning"); // "Warning";
                }
                break;

            default:
                break;
        }

    }
    private isMouseIn: boolean = false;
    OnMouseOver() {
        this.isMouseIn = true;
        setTimeout(() => {

            if (this.isMouseIn) {
                this.draw();

            }

        }, 100);
    }
    OnMouseLeave() {
        this.isMouseIn = false;
        //if (this.currencyRate) {

            setTimeout(() => {
                document.getElementById("tooltip-body" + this.name).style.visibility = "hidden";

            }, 400);

        //}

    }

    OnClickInsideToolTip(){
        document.getElementById("tooltip-body" + this.name).style.visibility = "hidden";
    }

    draw(){


        // var i = document.getElementById("tooltip-body" + this.name);
        // if (AppTool.IsNullOrEmpty(i))
        //     return;

        var item = document.getElementById("tooltip" + this.name);
        if (AppTool.IsNullOrEmpty(item))
            return;

        var itemRect = item.getBoundingClientRect();

        var element = document.getElementById("tooltip-body" + this.name);

        element.style.position = "fixed";
        element.style.visibility = "visible";

        switch (this.direction) {
            case 'topright':
            {
                element.style.bottom = (this.getScreenHeight() - itemRect.top + 20) + 'px';
                element.style.left = (itemRect.left - 15) + 'px';
                break;
            }
            case 'topleft':
            {
                var bodyItem = document.getElementById("tooltip-body" + this.name);
                var bodyItemRect = bodyItem.getBoundingClientRect();

                element.style.bottom = (this.getScreenHeight() - itemRect.top + 20) + 'px';
                element.style.left = (itemRect.right - bodyItemRect.width  + 20) + 'px';
                break;
            }
            case 'bottomright':
            {
                 element.style.top = (itemRect.top - 3) + 'px';
                element.style.left = (itemRect.left + 32) + 'px';
                break;
            }
            case 'bottomleft':
            {
                // get popup width
                var bodyItem = document.getElementById("tooltip-body" + this.name);
                var bodyItemRect = bodyItem.getBoundingClientRect();

                element.style.top = (itemRect.top - 3) + 'px';
                element.style.left = (itemRect.right - bodyItemRect.width - 30) + 'px';
                break;
            }

            default:
                break;
        }
    }
    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    GetArrowClassName(){
        var className = "small-tooltip-arrow";
        switch (this.direction) {
            case 'topright':
            {

                className += " arrow-top"
                break;
            }
            case 'topleft':
            {

                className += " arrow-topleft"
                break;
            }
            case 'bottomright':
            {

                className += " arrow-left"
                break;
            }
            case 'bottomleft':
            {
                className += " arrow-right"
                break;
            }

            default:
                break;
        }
        return className;
    }


}
