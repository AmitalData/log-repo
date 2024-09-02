import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ResponseDataBase, CustomsStepEnum } from '../../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomSendOptionsArgs, RequestParamsBase, SendRequestVIA } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CommunicationLogStepListService } from '../../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
  selector: 'courier-filter-button',

  host: { '(document:click)': 'handleClick($event)', },
  templateUrl: 'DropdownMenuFilterComponent.html',
})

export class DropdownMenuFilterComponent implements OnInit {
  @Input()
  public IsDisabled: boolean
  @Input()
  public Dropdownbutton_Text: string = "Show Dropdown Content";
  @Output()
  public DropdownMenuButtonClicked: EventEmitter<any> = new EventEmitter<any>();
  @Input()
  public DivLeft: number = -9999;
  @Input()
  public DivTop: number = -9999;

  @Input()
  public DivHight: number = -9999;


  private _CustomSendOptionsArgs: CustomSendOptionsArgs;
  public _DropdownDisplay: string = 'none';
  private _ElementRef: any;

  static MyId: number = 0;
  static LastDropdownMenuFilterId: number = 0;
  public _DropdownMenuFilterComponentId: string;
  public _DropdownMenuFilterComponentMenuId: string;
  MyDropdownMenuFilterId: number;

  constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
    this._ElementRef = myElement;
    ///this.DataContext = this; 
    this._CustomSendOptionsArgs = new CustomSendOptionsArgs();
    this._CustomSendOptionsArgs.ForcePersonalSign = false;
    var curId = DropdownMenuFilterComponent.MyId++;
    this.MyDropdownMenuFilterId = curId;
    this._DropdownMenuFilterComponentId = "DropdownMenuFilterComponent_" + curId;
    this._DropdownMenuFilterComponentMenuId = "DropdownButtonComponentMenuId_" + curId;
  }

  handleClick(event) {
    if (this._DropdownDisplay == 'none') {
      return;
    }
    var clickedComponent = event.target;
    var inside = false;
    let conter = 0;
    do {
      if (clickedComponent === this._ElementRef.nativeElement) {
        inside = true;
        break;
      }
      if (clickedComponent.class === "class-dropdownfilter-content") {
        inside = true;
        break;
      }
      if (conter > 50) {
        break;
      }
      conter++;
      clickedComponent = clickedComponent.parentNode;
    } while (clickedComponent);
    if (inside) {

    } else {

      //if (this._DropdownDisplay == 'block') {
      //    this.DropdowndisplayToggle(null);
      //}
      this.DropdownDisplayClose();
    }
  }

  ngOnInit() {
  }

  DropdownDisplayClose() {
    this._DropdownDisplay = 'none';
    this._CD.detectChanges();
  }


  Width = -60;
  Height = -40;

  public static EnsureLastDropdownMenuIsClosed() {
    //var lastDropdownMenuFilter = document.getElementById("DropdownButtonComponentMenuId_" + DropdownMenuFilterComponent.LastDropdownMenuFilterId);
    //if (!AppTool.IsNullOrEmpty(lastDropdownMenuFilter)) {
    //    lastDropdownMenuFilter.style.display = 'none';
    //}
  }

  DropdownMenuButtonClick(event) {
    this.DropdowndisplayToggle(event);
    this.DropdownMenuButtonClicked.emit(event);
  }

  MenuPosition(event) {
    // Get the button and dropdown menu elements
    const item = document.getElementById(this._DropdownMenuFilterComponentId);
    const dropdownMenu = document.getElementById(this._DropdownMenuFilterComponentMenuId);

    if (!item || !dropdownMenu) return; // Early return if either element doesn't exist

    const itemRect = item.getBoundingClientRect();
    let myTop = itemRect.top;
    let myLeft = itemRect.left;

    // Adjust position based on the event, if provided
    if (event) {
      myLeft = event.clientX;
      myTop = event.clientY;
    }

    // Set initial positioning of the dropdown
    dropdownMenu.style.top = `${myTop + 3}px`; // Initial position below the button
    dropdownMenu.style.left = `${myLeft - 70}px`; // Adjust left position as needed

    this._CD.detectChanges();

    // Adjust for screen space and dropdown height
    const screenHeight = window.innerHeight;
    const dropdownHeight = dropdownMenu.offsetHeight; // Assuming dropdownMenu is visible to calculate height
    const spaceBelow = screenHeight - itemRect.bottom;

    if (spaceBelow < dropdownHeight) {
      // Not enough space below, show above the button
      dropdownMenu.style.top = `${myTop - dropdownHeight}px`;
    }
  }

  DropdowndisplayToggle(event) {
    // Toggle dropdown visibility
    this._DropdownDisplay = this._DropdownDisplay == 'none' ? 'block' : 'none';
    DropdownMenuFilterComponent.LastDropdownMenuFilterId = this.MyDropdownMenuFilterId;

    // Position the dropdown menu relative to the button
    this.MenuPosition(event);

    // if (this._DropdownDisplay == 'none') {
    //   var item = document.getElementById(this._DropdownMenuFilterComponentId);
    //   var itemRect = item.getBoundingClientRect();
    //   let myTop = itemRect.top;
    //   let myleft = itemRect.left;
    //   if (!AppTool.IsNullOrEmpty(event)) {
    //     myleft = event.clientX;//: 19
    //     myTop = event.clientY;//: 19
    //     // event.stopPropagation();
    //   }

    //   //document.getElementById(this._DropdownMenuFilterComponentMenuId).style.top =
    //   //    (myTop/*itemRect.top*/ /*+ 27*/ /*-5*/) + 'px';

    //   let DDLHeight = 65 + 70;//    height: 22px; * 3 +30 
    //   let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN
    //   let ExtraTop = 150;
    //   document.getElementById(this._DropdownMenuFilterComponentMenuId).style.top = myTop + 'px';
    //   document.getElementById(this._DropdownMenuFilterComponentMenuId).style.left = (myleft - 100) + 'px';

    //   if (this.DivHight != -9999) {
    //     if (itemRect.bottom + DDLHeight + Extra > this.getScreenHeight()) {
    //       document.getElementById(this._DropdownMenuFilterComponentMenuId).style.marginTop =
    //         -1 * (this.DivHight) + 'px';
    //     }

    //   } else {

    //     if (itemRect.bottom + DDLHeight + Extra > this.getScreenHeight()) {
    //       document.getElementById(this._DropdownMenuFilterComponentMenuId).style.top =
    //         (itemRect.top - DDLHeight - Extra) + 'px';
    //     }
    //     if (itemRect.bottom + this.DivTop > this.getScreenHeight() && this.DivTop != -9999) {//this.PaintTop = true                
    //       document.getElementById(this._DropdownMenuFilterComponentMenuId).style.top =
    //         (itemRect.top - this.DivTop - ExtraTop) + 'px';
    //     }
    //   }

    //   if (this.DivLeft != -9999) {
    //     // document.getElementById(this._DropdownMenuFilterComponentMenuId).style.left =
    //     //(myleft + this.DivLeft)+ 'px';
    //   } else {
    //     document.getElementById(this._DropdownMenuFilterComponentMenuId).style.left =
    //       (myleft/*itemRect.left*/ /*- 50*/ - 100 /*+5*/) + 'px';//min-width: 80px
    //   }
    //   this._DropdownDisplay = 'block';
    // } else {
    //   this._DropdownDisplay = 'none';
    // }
    this._CD.detectChanges();
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
}
