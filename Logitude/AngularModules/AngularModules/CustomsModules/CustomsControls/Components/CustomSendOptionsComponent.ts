import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef} from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { ResponseDataBase, CustomsStepEnum } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';

import { CustomSendOptionsArgs, RequestParamsBase, SendRequestVIA} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

//////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////


@Component({
    selector: 'custom-send-options',
    moduleId: module.id,
    //templateUrl: 'CustomsRequestsComponent.html',
    host: {
        '(document:click)': 'handleClick($event)',
    },
    templateUrl:'./CustomSendOptionsComponent.html',
})


export class CustomSendOptionsComponent implements OnInit {
    

    @Input()
    public AvoidDoubleClick: boolean = false;
    @Output()
    public SendButtonClicked: EventEmitter<CustomSendOptionsArgs> = new EventEmitter<CustomSendOptionsArgs>();
    @Input()
    public IsDisabled: boolean = false;
    @Input()
    public CustomSendOptionsButtonCanForcePersonalSign: boolean = false;//Show ForcePersonalSign
    @Input()
    ButtonText: string = TextCodeTranslator.Translate("Customs.Declaration.O.Send");//"שלח";
    _ButtonCodeText: string;
    @Input()
    IsCheckBoxVisibile: boolean = true;
   
    @Input()
    public get ButtonCodeText() { return this._ButtonCodeText; }
    public set ButtonCodeText(val: string) {
        if (this._ButtonCodeText == val) return;
        this._ButtonCodeText = val;
        this.ButtonText = TextCodeTranslator.Translate(val);
        this._CD.detectChanges();
    }

      
    private _CustomSendOptionsArgs: CustomSendOptionsArgs;
    private _DropdownDisplay: string = 'none';
    private _ElementRef: any;

    static MyId: number = 0;
    private _CustomSendOptionsComponentId: string;
    private _CustomSendOptionsComponentMenuId: string;
    _IsLoaded: boolean = false;
    private EntityResourceService: EntityResourceService;
    constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        this._CustomSendOptionsArgs = new CustomSendOptionsArgs();
        this._CustomSendOptionsArgs.ForcePersonalSign = false;
        var curId = CustomSendOptionsComponent.MyId++;
        this._CustomSendOptionsComponentId = "CustomSendOptionsComponent_" + curId;
        this._CustomSendOptionsComponentMenuId = "CustomSendOptionsComponentMenuId_" + curId;

        this.EntityResourceService = new EntityResourceService();
      
        
    }
    public get ForcePersonalSign(){return this._CustomSendOptionsArgs.ForcePersonalSign;}
    public set ForcePersonalSign(val:boolean){
        this._CustomSendOptionsArgs.ForcePersonalSign=val;
    }
    SendDefault() {
        this._CustomSendOptionsArgs.Option = "";
        this._CustomSendOptionsArgs.RequestVIA = SendRequestVIA.Default;
        this.JustEmit();
    }
    SendWI()
    {
        this._CustomSendOptionsArgs.Option = "WI";
        this._CustomSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceInteractive;
        this.JustEmit();
    }
    public IsDisabledTimeout: boolean = false;
    JustEmit() {
        this.DropdownDisplayClose();
        var toSign = this._CustomSendOptionsArgs.ForcePersonalSign;
        this.SendButtonClicked.emit( { 
            Option :this._CustomSendOptionsArgs.Option,
            ForcePersonalSign: toSign ,
            RequestVIA: this._CustomSendOptionsArgs.RequestVIA,

        });
        this._CustomSendOptionsArgs.ForcePersonalSign = false;


        if (this.AvoidDoubleClick || this.CustomSendOptionsButtonCanForcePersonalSign) {//Due double request == double click 
            var featueDisable2Sec = true;
            if (featueDisable2Sec) {
                this.IsDisabledTimeout = true;
                setTimeout(() => {
                    this.IsDisabledTimeout = false;
                }, 2000);
            }
        }
    }
    SendWB() {
        
        this._CustomSendOptionsArgs.Option = "WB";
        this._CustomSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceBatch;
        this.JustEmit();
    }
    SendD() {
        
        this._CustomSendOptionsArgs.Option = "D";
        this._CustomSendOptionsArgs.RequestVIA = SendRequestVIA.DCABatch;
        this.JustEmit();
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
            if (conter > 10) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
            
        } else {
            
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
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
    }
    
    ForcePersonalSignToggle() {
        this._CustomSendOptionsArgs.ForcePersonalSign  =!this._CustomSendOptionsArgs.ForcePersonalSign  ;
    }
    Width = -30;
    Height = -20;
    dropdowndisplayToggle() {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._CustomSendOptionsComponentId);
            var itemRect = item.getBoundingClientRect();
            
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                itemRect.top + 'px';
            
            let DDLHeight = 67;//    height: 22px; * 3 +30 
            let Extra =  22+1+1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight > this.getScreenHeight()) {//this.PaintTop = true                
                document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra) + 'px';
            }
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.left =
                (itemRect.left + 80) + 'px';//min-width: 80px
            this._DropdownDisplay = 'block';
        } else {
            this._DropdownDisplay = 'none';
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

}
