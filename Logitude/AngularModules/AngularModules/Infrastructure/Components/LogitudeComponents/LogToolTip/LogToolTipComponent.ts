import { ObjectsLocator } from './../../../Locators/ObjectsLocator';
import { Component, OnInit, Output, EventEmitter, Input} from '@angular/core';
import { AppTool } from './../../../Tools';

@Component({
    moduleId: module.id,
    selector: 'LogToolTip',
    templateUrl: './LogToolTipComponent.html',
    styleUrls: ['./LogToolTipComponent.css']
})

export class LogToolTipComponent {
    public isRTL: boolean = false;


    @Input() public title: string;
    @Input() public name: string = 'no1';
    @Input() public direction: string = 'bottomright';

    constructor() {

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


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
                element.style.bottom = (this.getScreenHeight() - itemRect.top - 10) + 'px';
                element.style.left = (itemRect.left + 15) + 'px';
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
