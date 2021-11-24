import { Component, ContentChild, ElementRef, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MessageWindowComponent } from '../MessageWindow/MessageWindowComponent';

@Component({
    selector: 'custom-label',
    templateUrl: './CustomLabel.component.html',
    styleUrls: ['./CustomLabel.component.css']
})
export class CustomLabelComponent implements OnInit {
    @ViewChild('container', { static: true }) container: ElementRef;
    @ViewChild('content', { static: true }) content: ElementRef;
    @Input('width') width:number;
    isHasMore: boolean = false;
    originText: string;
    availableWidth:number;
    iconSize = 10;
    iconMargin = 4;
    constructor(public dialog: MatDialog) { }

    ngOnInit() {

    }
    ngAfterContentInit(): void {
        this.originText = this.content.nativeElement.innerText;
        this.checkText();
    }

    checkText() {
        this.availableWidth = this.getAvailableWidth();
        if (!this.checkOverflow())
            return;
        this.isHasMore = true;
        this.content.nativeElement.innerHTML = this.getSliceFromText();
    }
    getAvailableWidth(): number {
        if(this.width)
            return this.width;
        return this.container.nativeElement.parentElement.parentElement.offsetWidth;
    }
    getSliceFromText(): any {
        var fitSize = this.getFitSize(this.originText)
        return this.originText.slice(0, fitSize - 1);
    }
    getFitSize(text: string): number {
        var textWidth = this.content.nativeElement.offsetWidth;
        var newWidth = this.availableWidth - this.iconSize - this.iconMargin - this.iconMargin ;
        return (text.length * newWidth) / textWidth;
    }

    checkOverflow() {
        if (this.content.nativeElement.offsetWidth <= 0)
            return false;
        return  this.availableWidth < this.container.nativeElement.parentElement.offsetWidth;
    }

    showPopup(event) {
        event.stopPropagation();
        this.dialog.open(MessageWindowComponent, {
            data: {
                title: '',
                description: this.originText,
            }
        });
    }
}
