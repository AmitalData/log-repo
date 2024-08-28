import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';

@Component({

    selector: "HelpIcon",
    inputs: ['Header', 'Text', 'HideHeader', 'IconSize', 'IconPath', 'V2Grid', 'Position', 'Height','BackgroundImage'],
    templateUrl: './HelpIcon.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
})

export class HelpIcon implements OnInit {
    public Width: number = null;
    public Position: number = null;
    public Height: number = null;
    public IconSize: number = 17;
    public HideHeader: boolean = false;
    public V2Grid: boolean = false;
    public IsVisible: boolean;
    public TooltipId: string = null;
    public TooltipContentId: string = null;
    public IconPath: string = "./Images/Help.png";
    public IconBackground: string = null;

    LayoutDirection: string = 'ltr';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var idIndex = this.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    ngOnInit() {
        if (this.Width == null) {
            this.Width = 265;
        }

        if (this.Height == null) {
            this.Height = 120;
        }

        this.IconBackground = "url(" + this.IconPath + ")";
    }

    private header: string = "Help";
    get Header() { return this.header; }
    set Header(newValue: string) {
        if (this.header != newValue) {
            this.header = newValue;
        }
    }
    private backgroundImage: string = "";
    get BackgroundImage() { return this.backgroundImage; }
    set BackgroundImage(newValue: string) {
        if (this.backgroundImage != newValue) {
            this.backgroundImage = newValue;
            this.Text="";
        }
        if (this.text == null && this.backgroundImage==null) {
            this.IsVisible = false;
        }

        else {
            this.IsVisible = true;
        }
    }

    private text: string = "Help";
    get Text() { return this.text; }
    set Text(newValue: string) {
        if (this.text != newValue) {
            this.text = newValue;

            if (this.text == null && this.BackgroundImage==null) {
                this.IsVisible = false;
            }

            else {
                this.IsVisible = true;
            }
        }
    }

    mouseover() {
        var item = document.getElementById(this.TooltipId);
        var itemRect = item.getBoundingClientRect();

        var isToRight = true;
        var ApplicationSession = document.getElementById("ApplicationSession");
        if (ApplicationSession) {
            var appWidth = ApplicationSession.clientWidth;
            var appHeight = ApplicationSession.clientHeight;

            if ((itemRect.left + this.Width) > appWidth) {
                isToRight = false;
            }
        }


        if (this.V2Grid) {
            this.SetToolTipPositionInV2Grid();
            return;
        }

        document.getElementById(this.TooltipContentId).style.position = "fixed";
        document.getElementById(this.TooltipContentId).style.top = (itemRect.top - this.Height + 5) + 'px';

        if (isToRight) {
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
            document.getElementById(this.TooltipContentId).style.left = (itemRect.left + 5) + 'px';
        }
        else {
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipFlipped.png')";
            document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width) + 'px';
        }
    }

    SetToolTipPositionInV2Grid() {
        if (this.Position > 3) {
            document.getElementById(this.TooltipContentId).style.marginTop = ((this.Height - 5) * -1) + 'px';
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
        }
        else {
            document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipBottom.png')";
            document.getElementById(this.TooltipContentId).style.paddingTop = '20px';
        }
    }
}
