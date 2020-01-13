
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef} from '@angular/core';
import { AppTool } from '../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

//////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////


@Component({
  selector: 'split-button',
  moduleId: module.id,
  //templateUrl: 'CustomsRequestsComponent.html',
  host: {
    '(document:click)': 'handleClick($event)',
  },
  templateUrl: './SplitButtonComponent.html',
})


export class SplitButtonComponent implements OnInit {


  @Input()
  public AvoidDoubleClick: boolean = false;
  @Output()
  public DefaultSplitButtonClicked: EventEmitter<string> = new EventEmitter<string>();
  @Input()
  public IsDisabled: boolean = false;

  @Input()
  ButtonText: string = TextCodeTranslator.Translate("Customs.Declaration.O.Send");//"שלח";
  _ButtonCodeText: string;

  @Input()
  public MENUDivExtraTop: any = 24;

  @Input()
  public MENUDivExtraLeft: any = 0;


  @Input()
    public OnClickedShowMenuContent: boolean = false;

    @Input()
    public AsRegularButton: boolean = false;

  @Input()
  public get ButtonCodeText() { return this._ButtonCodeText; }
  public set ButtonCodeText(val: string) {
    if (this._ButtonCodeText == val) return;
    this._ButtonCodeText = val;
    this.ButtonText = TextCodeTranslator.Translate(val);
    this._CD.detectChanges();
  }



  private _DropdownDisplay: string = 'none';
  private _ElementRef: any;

  static MyCounterId: number = 0;
  static LastSplitButtonClickedId: number = 0;
  MyCurrentSplitButtonComponentId: number = 0;

  private _SplitButtonComponentId: string;
  private _SplitButtonComponentMenuId: string;
  _IsLoaded: boolean = false;
  private EntityResourceService: EntityResourceService;



  constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
    this._ElementRef = myElement;
    ///this.DataContext = this; 

    this.MyCurrentSplitButtonComponentId = SplitButtonComponent.MyCounterId++;
    this._SplitButtonComponentId = "SplitButtonComponent_" + this.MyCurrentSplitButtonComponentId;
    this._SplitButtonComponentMenuId = "SplitButtonComponentMenuId_" + this.MyCurrentSplitButtonComponentId;

    this.EntityResourceService = new EntityResourceService();


  }

  DefaultButtonClick(sourceButton,event) {
    this.CloseOtherLastmenu();

    if (this.OnClickedShowMenuContent) {
      this.dropdowndisplayToggle(null/*event*/);
        this.DefaultSplitButtonClicked.emit(sourceButton);
    } else {
      //event.stopPropagation();
        //this.DropdownDisplayCloseANdJustEmit();
        if (sourceButton == "splitterButton") {
            this.dropdowndisplayToggle(event);
        } else {
            this.DefaultSplitButtonClicked.emit(sourceButton);
        }
        
    }
    
  }

  public IsDisabledTimeout: boolean = false;
  //DropdownDisplayCloseANdJustEmit() {


  //  this.DropdownDisplayClose();
  //  this.DefaultSplitButtonClicked.emit("DefaultButtonClicked");



  //}

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
      if (conter > 10) {
        break;
      }
      conter++;
      clickedComponent = clickedComponent.parentNode;
    } while (clickedComponent);
    if (inside) {

    } else {

      this.DropdownDisplayClose();
      if (this._DropdownDisplay == 'block') {
        this.dropdowndisplayToggle(null);
      }

      //alert("outside");
    }
  }
  ngOnInit() {
    this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
      this._IsLoaded = true;
      /// alert("this._IsLoaded");
      if (AppTool.IsNullOrEmpty(this.ButtonText)) {
        this.ButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Send");//"שלח";
      }
    });
  }

  DropdownDisplayClose() {
    this._DropdownDisplay = 'none';
    this._CD.detectChanges();
  }


  Width = -30;
  Height = -20;
  CloseOtherLastmenu() {
    var suppress = true;
    if (suppress) {
      return;
    }
    if (SplitButtonComponent.LastSplitButtonClickedId != 0 && SplitButtonComponent.LastSplitButtonClickedId != this.MyCurrentSplitButtonComponentId) {
      var lastSplitButtonComponentMenu = document.getElementById("SplitButtonComponentMenuId_" + SplitButtonComponent.LastSplitButtonClickedId);
      if (!AppTool.IsNullOrEmpty(lastSplitButtonComponentMenu)) {
        lastSplitButtonComponentMenu.style.display = 'none';
      }
    }
    SplitButtonComponent.LastSplitButtonClickedId = this.MyCurrentSplitButtonComponentId;
  }

  public static EnsureLastSplitButtonIsClosed() {
    var suppress = true;
    if (suppress) {
      return;
    }
    var lastSplitButtonComponentMenu = document.getElementById("SplitButtonComponentMenuId_" + SplitButtonComponent.LastSplitButtonClickedId);
    if (!AppTool.IsNullOrEmpty(lastSplitButtonComponentMenu)) {
      lastSplitButtonComponentMenu.style.display = 'none';
    }
  }
  dropdowndisplayToggle(event) {
    if (!AppTool.IsNullOrEmpty(event)) {
      ///event.stopPropagation();
      this.CloseOtherLastmenu();
    }
    
    if (this._DropdownDisplay== 'none' ) {
      var item = document.getElementById(this._SplitButtonComponentId);
      var itemRect = item.getBoundingClientRect();

      //document.getElementById(this._SplitButtonComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
      //document.getElementById(this._SplitButtonComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
      document.getElementById(this._SplitButtonComponentMenuId).style.top =
        itemRect.top + 'px';

      let DDLHeight = 87;//    height: 22px; * 3 +30 
      let MENUDivExtraTop = Number(this.MENUDivExtraTop); //22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
      if (itemRect.bottom + DDLHeight > this.getScreenHeight()) {//this.PaintTop = true                
        document.getElementById(this._SplitButtonComponentMenuId).style.top =
          (itemRect.top - DDLHeight - MENUDivExtraTop) + 'px';
      }
      let MENUDivExtraLeft = Number(this.MENUDivExtraLeft); //22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
      document.getElementById(this._SplitButtonComponentMenuId).style.left =
        (itemRect.left + MENUDivExtraLeft) + 'px';//min-width: 80px
      this._DropdownDisplay = 'block';
    } else {
      this._DropdownDisplay = 'none';
    }
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
