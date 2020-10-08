import { Component, EventEmitter, Input, OnInit } from '@angular/core';

@Component({
    selector: 'panel',
    templateUrl: './PanelComponent.html',
    styleUrls: ['./PanelComponent.css']
})

export class PanelComponent implements OnInit {
    HeaderLinkClickedEvent: EventEmitter<any> = new EventEmitter();

    ShowDetailsSection: boolean = false;
    @Input() Name: string;
    @Input() HaveDetailsSection: boolean = false;
    @Input() DetailsTitle: string = "";
    @Input() HeaderLinkText: string;
    @Input() Title: string = "";
    
    constructor() { }

    ngOnInit() { }

    ShowMoreDetails(){
        this.ShowDetailsSection = !this.ShowDetailsSection;
        // event emit
    }
    HeaderLinkClicked(){
        this.HeaderLinkClickedEvent.emit();
    }
}