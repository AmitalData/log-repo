import { Component, EventEmitter, HostListener, Input, OnInit } from '@angular/core';

@Component({
    selector: 'panel',
    templateUrl: './PanelComponent.html',
    styleUrls: ['./PanelComponent.css']
})

export class PanelComponent implements OnInit
{
    HeaderLinkClickedEvent: EventEmitter<any> = new EventEmitter();

    ShowDetailsSection: boolean = false;
    @Input() Name: string;
    @Input() ShowWhitePanelOnMobile: boolean = false;
    @Input() ShowGrayPanelOnMobile: boolean = false;
    @Input() HaveDetailsSection: boolean = false;
    @Input() DetailsTitle: string = "";
    @Input() HeaderLinkText: string;
    @Input() Title: string = "";
    @Input() TransparentBackground: boolean = false;

    constructor() { }

    @HostListener('window:resize', ['$event'])
    onResize(event)
    {
        // var width = event.target.innerWidth;
        this.initPanel();
    }


    ngOnInit()
    {
        this.initPanel();
    }


    private initPanel()
    {
        var width = window.innerWidth;
        if (width < 470 && this.HaveDetailsSection) {
            this.ShowDetailsSection = true;
        }
    }

    ShowMoreDetails()
    {
        this.ShowDetailsSection = !this.ShowDetailsSection;
        // event emit
    }
    HeaderLinkClicked()
    {
        this.HeaderLinkClickedEvent.emit();
    }
}
