import {Component, OnInit, Output, EventEmitter, ChangeDetectionStrategy, OnDestroy} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {QuotePM} from '../../Quote/EntityPMs/QuotePM';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';

@Component({
    selector: "HelperNotes",
    
    templateUrl: './HelperNotes.html',
    inputs: ['Title', 'EntityTitle', 'Text', 'IconCode', 'IsEnabled', 'ShipmentPM', 'QuotePM','IsCustom'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})

export class HelperNotes implements OnInit, OnDestroy {
    public ComponentId: string = null;
    public ComponentButtonId: string = null;
    public ComponentContentId: string = null;
    public Width: number = 250;
    public Height: number = 130;
    public MinHeight: number = 130;
    public MaxHeight: number = 350;
    public Title: string = "Notes";
    public EntityTitle: string;    
    public QuotePM: QuotePM;
    public ShipmentPM: ShipmentPM;
    public IsEnabled: boolean = true;
    public IsCustom: boolean = false;
    public IconCode: string;
    public IconPath: string;
    public IconOpacity: number = 1;
    @Output() TextChanged: EventEmitter<string> = new EventEmitter<string>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var idIndex = this.CurrentSession.GetNewId("HelperNotes");
        this.ComponentId = "HelperNotes_" + idIndex;
        this.ComponentButtonId = "HelperNotesButton_" + idIndex;
        this.ComponentContentId = "HelperNotesContent_" + idIndex;
    }

    ngOnInit() {
        this.SetIconPath();
        this.SetNotesList();
        this.SetPopupHeight();
        this.Listen();
    }

    private HelperNotesChangedEvent: any = null;
    Listen() {
        if (!this.HelperNotesChangedEvent) {
            this.HelperNotesChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                switch (s) {
                    case "QuotePartnersChanged":
                    case "ShipmentPartnersChanged":
                        {
                            this.SetNotesList();
                            this.SetPopupHeight();
                            break;
                        }
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.HelperNotesChangedEvent);
        this.HelperNotesChangedEvent = null;
        this.StopPositionTimer();
    }

    SetIconPath() {
        var file: string = "Gray.png";

        switch (this.IconCode) {
            case "G": {
                file = "Green.png";
                this.IconOpacity = AppTool.IsNullOrEmpty(this.Text) ? 0.5 : 1;
                break;
            }

            case "B": {
                file = "Blue.png";
                this.IconOpacity = AppTool.IsNullOrEmpty(this.Text) ? 0.5 : 1;
                break;
            }

            case "R": {
                file = AppTool.IsNullOrEmpty(this.Text) ? "Rosie.png" : "Or.png";
                this.IconOpacity = 1;
                break;
            }

            default: {
                file = AppTool.IsNullOrEmpty(this.Text) ? "Gray.png" : "Orange.png";
                this.IconOpacity = 1;
                break;
            }
        }

        var iconPath = "./_Resources/Images/Icons/Notes/" + file;
        this.IconPath = "url(" + iconPath + ")";
    }
    SetPopupHeight() {
        var myResult: number = 130;

        if (AppTool.IsNullOrEmpty(this.EntityTitle)) {
            myResult -= 15;
        }

        if (this.NotesList) {
            var itemHeight = 70;
            var itemsCount = this.NotesList.length;

            if (itemsCount <= 3) {
                myResult += itemsCount * itemHeight;
                myResult += 5;
            }

            else {
                myResult = 400;
            }
        }
       
        this.Height = myResult;        
    }

    private isTextLoaded: boolean = false;
    private text: string = null;
    get Text() { return this.text; }
    set Text(value: string) {
        if (this.text != value) {
            this.text = value;

            this.SetIconPath();

            if (this.isTextLoaded) {
                this.TextChanged.emit(value);
            }
        }

        this.isTextLoaded = true;
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (value) {
                    this.SetPopupHeight();
                    this.RunPositionTimer();

                    if (document.getElementById(this.ComponentContentId).style.visibility != "visible") {
                        document.getElementById(this.ComponentContentId).style.visibility = "visible";
                    }
                }

                else {
                    this.StopPositionTimer();

                    if (document.getElementById(this.ComponentContentId).style.visibility != "hidden") {
                        document.getElementById(this.ComponentContentId).style.visibility = "hidden";
                    }
                }
            }
        }
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculateFixedPosition(), 0);
    }
    private CalculateFixedPosition() {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        }
    }

    public IsMouseOver: boolean = false;
    public IsMouseOverButton: boolean = false;
    public IsMouseOverTextBox: boolean = false;
    public IsTextBoxFocused: boolean = false;
    OnButtonClicked() {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }

        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    }
    OnButtonLostFocus() {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverTextBox) {
                if (!this.IsMouseOverTextBox) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
            }

            else {
                this.IsOpened = false;
            }
        }
    }
    OnTextBoxFocus() {
        this.IsTextBoxFocused = true;
    }
    OnTextBoxLostFocus() {
        if (this.IsTextBoxFocused) {
            this.IsTextBoxFocused = false;

            if (!this.IsMouseOverButton) {
                if (this.IsMouseOver) {
                    document.getElementById(this.ComponentButtonId).focus();
                }

                else {
                    this.IsOpened = false;
                }
            }
        }
    }
    OnTextBoxKeydown(e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 9) {
            e.preventDefault();
        }
    }

    public NotesList: HelperNoteItem[];
    SetNotesList() {
        if(this.IsCustom)
            return;
        if (this.QuotePM) {
            this.BuildQuoteNotesList();
        }

        else if (this.ShipmentPM) {
            this.BuildShipmentNotesList();
        }
    }
    BuildQuoteNotesList() {
        this.NotesList = [];

        if (!AppTool.IsNullOrEmpty(this.QuotePM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.QuotePM.ShipperNote });
        }

        if (!AppTool.IsNullOrEmpty(this.QuotePM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.QuotePM.ConsigneeNote });
        }

        //if (!AppTool.IsNullOrEmpty(this.QuotePM.AgentNote)) {
        //    this.NotesList.push({ Header: "Agent", Notes: this.QuotePM.AgentNote });
        //}
    }
    BuildShipmentNotesList() {
        this.NotesList = [];

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.ShipperNote)) {
            this.NotesList.push({ Header: "Shipper", Notes: this.ShipmentPM.ShipperNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.ConsigneeNote)) {
            this.NotesList.push({ Header: "Consignee", Notes: this.ShipmentPM.ConsigneeNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.AgentNote)) {
            this.NotesList.push({ Header: "Agent", Notes: this.ShipmentPM.AgentNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.CustomAgentExportNote)) {
            this.NotesList.push({ Header: "Custom Agent Export", Notes: this.ShipmentPM.CustomAgentExportNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.CustomAgentImportNote)) {
            this.NotesList.push({ Header: "Custom Agent Import", Notes: this.ShipmentPM.CustomAgentImportNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.Notify1Note)) {
            this.NotesList.push({ Header: "Notify1", Notes: this.ShipmentPM.Notify1Note });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.Notify2Note)) {
            this.NotesList.push({ Header: "Notify2", Notes: this.ShipmentPM.Notify2Note });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.ConsigneeNotImporterNote)) {
            this.NotesList.push({ Header: "Consignee Not Importer", Notes: this.ShipmentPM.ConsigneeNotImporterNote });
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.ShipperNotExporterNote)) {
            this.NotesList.push({ Header: "Shipper Not Exporter", Notes: this.ShipmentPM.ShipperNotExporterNote });
        }
    }
}

interface HelperNoteItem {
    Header: string,
    Notes: string,
}
