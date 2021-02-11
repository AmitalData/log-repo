import {DocumentTypeCopyPM} from '../../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {ShipmentPM} from '../../../../../../Shipment/EntityPMs/ShipmentPM';
import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypePM} from '../../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentOutPM} from '../../../../../../Common/EntityPMs/DocumentOutPM';
import {DocumentOutCopyPM} from '../../../../../../Common/EntityPMs/DocumentOutCopyPM';
import {DateTimeToTimePipe} from '../../../../../../Infrastructure/Pipes/DateTimeToTimePipe';
import {DateTimeToDatePipe} from '../../../../../../Controls/Pipes/DateTimeToDatePipe';
import {AppTool} from '../../../../../../Infrastructure/Tools';
import {PrintDocumentComponent} from '../../PrintDocumentComponent';
import {Guid} from '../../../../../../Infrastructure/Utilities/Guid';

export class DocumentCopiesViewModel {
    public ObjectTableName: string;
    public CurrentObjectTableId: string;
    public ChildEntityId: string;
    public CurrentEntityId: string;
    public ChildObjectTableId: string;
    public CurrentDocumentOutCopy: DocumentOutCopyPM;
    public CurrentDocumentOut: DocumentOutPM;
    public CurrentDocumentType: DocumentTypePM;
    public CurrentDocumentTypeCopy: DocumentTypeCopyPM;
    public DocumentOutCopies: DocumentOutCopyPM[];
    public IsSelected: boolean;
    public Exists: boolean;
    public Name: string;
    public Code: string;
    public Key: string;
    public IsButtonsStackPanelVisible: boolean;
    public Status: string;
    public VisiblePrint: boolean;
    public Id: string;
    public PrintedByMessage: string;
    public IsSelectedByDefault: boolean;
    public IsDiableSelctedDocumentTypeCopy: boolean = false;
    public IsPrintButtonEnabled: boolean = false;
    public IsOriginal: boolean = false;
    
    public IsHideSetSelectedAsDefaultBtn: boolean;
    public DivSelectBackgroud: string;
    IndexOrder: number;
    IsCheckBoxesVisible: boolean = true;

    DivSelectBackground: string = "";
    constructor(documentTypeCopy: DocumentTypeCopyPM, documentOutPM: DocumentOutPM, currentEntityId: string, childEntityId: string, currentObjectTableId: string, childObjectTableId: string, documenttype: DocumentTypePM, entityReference: string) {

        this.Key = Guid.newGuid();
        this.CurrentDocumentOut = documentOutPM;

        this.IsHideSetSelectedAsDefaultBtn = true;
        this.Id = documentTypeCopy.Id;
        this.CurrentObjectTableId = currentObjectTableId;
        this.ChildEntityId = childEntityId;
        this.CurrentEntityId = currentEntityId;
        this.ChildObjectTableId = childObjectTableId;

        this.CurrentDocumentType = documenttype;


        if (this.CurrentDocumentType) {

            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited) {
                this.IsCheckBoxesVisible = false;
            }
        }

        this.CurrentDocumentTypeCopy = documentTypeCopy;
        this.IndexOrder = this.CurrentDocumentTypeCopy.IndexOrder;
        this.Name = documentTypeCopy.Name;
        this.Code = documentTypeCopy.Code;
        

        if (documentOutPM) {

            this.DocumentOutCopies = documentOutPM.DocumentOutCopies;

            this.CurrentDocumentOutCopy = this.DocumentOutCopies.filter(d => d.DocumentTypeCopyId == documentTypeCopy.Id)[0];

            if (this.CurrentDocumentOutCopy != null) {

                this.Exists = true;
                this.IsButtonsStackPanelVisible = true;
            }
            else {
                this.Exists = false;
                this.IsButtonsStackPanelVisible = false;
            }
            this.IsSelected = this.Exists;

            this.PrintedByMessage = "";
            if (this.CurrentDocumentOutCopy != null) {

                if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && this.CurrentDocumentOutCopy.LastPrintedByUserId != null && this.CurrentDocumentOutCopy.LastPrintedByUserId != undefined) {

                    var data = "";

                    if (this.CurrentDocumentOutCopy.LastPrintDate) {
                        var date = DateTimeToDatePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                        var time = DateTimeToTimePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                        date = date + " " + time;
                    }


                    this.PrintedByMessage = "This document is already printed by " + this.CurrentDocumentOutCopy.LastPrintedByUserName + " at " + date;
                }

            }
        }

        if (this.CurrentDocumentType) {

            if (this.CurrentDocumentType.DocumentTypeCopies.length == 1) {
                this.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                this.IsDiableSelctedDocumentTypeCopy = true;
                this.IsHideSetSelectedAsDefaultBtn = true;

            }
            else {

                this.IsDiableSelctedDocumentTypeCopy = false;
                this.IsHideSetSelectedAsDefaultBtn = false;

            }
        }

        this.IsSelectedByDefault = this.CurrentDocumentTypeCopy.IsSelectedByDefault;


        this.IsPrintButtonEnabled = true;
        if (this.CurrentDocumentOutCopy != null) {

            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && !AppTool.IsNullOrEmpty(this.CurrentDocumentOutCopy.LastPrintedByUserId)) {
                this.IsPrintButtonEnabled = false;
            }
        }

        if (this.CurrentDocumentTypeCopy != null) {
            if (this.CurrentDocumentTypeCopy && this.CurrentDocumentTypeCopy.IsOriginal) {
                this.IsOriginal = true;
            }
        }

        this.PrintedByMessage = "";

        if (this.CurrentDocumentOutCopy) {

            if (this.CurrentDocumentType.IsDocumentOneTimePrintLimited && this.CurrentDocumentType.LimitedPrintCopyId == this.CurrentDocumentOutCopy.DocumentTypeCopyId && !AppTool.IsNullOrEmpty(this.CurrentDocumentOutCopy.LastPrintedByUserId)) {

                var data = "";

                if (this.CurrentDocumentOutCopy.LastPrintDate) {
                    var date = DateTimeToDatePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                    var time = DateTimeToTimePipe.Pipe(this.CurrentDocumentOutCopy.LastPrintDate);
                    date = date + " " + time;
                }

                this.PrintedByMessage = "This document is already printed by " + this.CurrentDocumentOutCopy.LastPrintedByUserName + " at " + date;
            }
        }
    }


    public RefereshDocumentOutCopies(item: DocumentOutPM) {

        this.CurrentDocumentOut = item;
        this.DocumentOutCopies = this.CurrentDocumentOut.DocumentOutCopies;
        this.CurrentDocumentOutCopy = this.CurrentDocumentOut.DocumentOutCopies.filter(d=> d.DocumentTypeCopyId == this.CurrentDocumentTypeCopy.Id)[0];
        if (this.CurrentDocumentType.DocumentTypeCopies.length == 1) {
            this.IsDiableSelctedDocumentTypeCopy = true;
            this.IsHideSetSelectedAsDefaultBtn = true;

        }
        else {
            this.IsDiableSelctedDocumentTypeCopy = false;
            this.IsHideSetSelectedAsDefaultBtn = false;
        }


        if (this.CurrentDocumentOutCopy != null) {
            this.IsHideSetSelectedAsDefaultBtn = true;
            this.Exists = true;
            this.IsSelected = this.Exists;
            this.IsButtonsStackPanelVisible = true;
        }
        else {
            // this.IsHideSetSelectedAsDefaultBtn = true;
            this.IsButtonsStackPanelVisible = false;
        }

    }
}
