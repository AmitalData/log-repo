import { Component, ContentChild, Directive, ElementRef, HostListener, Input, OnInit, ViewChild, TemplateRef, ContentChildren, ChangeDetectorRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MessageWindowComponent } from '../MessageWindow/MessageWindowComponent';

@Directive({
    selector: '[appLabelUtilContent]'
})
export class LabelUtilContentDirective {
    constructor(public templateRef: TemplateRef<unknown>) {}
}

@Directive({
    selector: '[appLabelUtilElement]'
})
export class LabelUtilElementDirective {
    constructor(public elementRef: ElementRef) {}
}

@Component({
    selector: 'custom-label',
    templateUrl: './CustomLabel.component.html',
    styleUrls: ['./CustomLabel.component.css']
})
export class CustomLabelComponent implements OnInit {
    @ViewChild(LabelUtilContentDirective) myContent!: LabelUtilContentDirective;
    @ViewChild(LabelUtilElementDirective) myElement!: LabelUtilElementDirective;

    @ViewChild('container', { static: true }) container: ElementRef;
    @ViewChild('content', { static: true }) content: ElementRef;
    @Input('width') width: number;
    isHasMore: boolean = false;
    originText: string;
    availableWidth: number;
    iconSize = 10;
    iconMargin = 4;
    isMobile = false;
    constructor(public dialog: MatDialog,private cdr: ChangeDetectorRef) { }

    ngOnInit() {
        this.isMobile= ( window.innerWidth <= 479 ) 
    }
   
    ngAfterViewChecked(){
        
        this.cdr.detectChanges();
     }
    ngAfterViewInit() {
        
        setTimeout(() => {
             if (!this.originText)
            this.originText = this.myContent.templateRef.elementRef.nativeElement.parentElement.innerText;
            this.checkText(); 
        }, 1);
            
    }

    checkText() {
        this.availableWidth = this.getAvailableWidth();
        if (!this.checkOverflow())
            return;
        this.isHasMore = true;
        this.myElement.elementRef.nativeElement.innerHTML= this.getSliceFromText();
    }
    getAvailableWidth(): number {
        if (this.width)
            return this.width;
        return this.container.nativeElement.parentElement.parentElement.offsetWidth;
    }
    getSliceFromText(): any {
        var fitSize = this.getFitSize(this.originText)
        return this.originText.slice(0, fitSize - 1);
    }
    getFitSize(text: string): number {
        var textWidth = this.myContent.templateRef.elementRef.nativeElement.parentElement.offsetWidth;
        var newWidth = this.availableWidth - this.iconSize - this.iconMargin - this.iconMargin;
        return (text.length * newWidth) / textWidth;
    }

    checkOverflow() {
        if (this.myContent.templateRef.elementRef.nativeElement.parentElement.offsetWidth <= 0)
            return false;
        return this.availableWidth < this.container.nativeElement.parentElement.offsetWidth;
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
    stopPropagation(event){
        event.stopPropagation();
    }
}
