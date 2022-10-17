import { AppTool } from "../../../../../../Infrastructure/Tools";

export class BuildStimulReportResult {
    ReportKey: string;
    PageCount: number;
    StimulImageBase64: string;
    EditableFieldPositionLists: EditableFieldPosition[];
}

export class EditableFieldPosition {
    Left: number;
    Top: number;
    PageFieldIndex: string;
    Width: number;
    Height: number;
    FieldName: string;
    FieldValue: string;
    BorderBrach: string;
    TextColor: string;
    Float: string;
    FontSize: number;
    Fontweight: string;
    FontFamily: string;
    NewValue: string;
    WidthPagePrecentage: number;
    HeightPagePrecentage: number;
    TextAligh: string;
    VerticalAlign: string;
    FieldPosition: string;
    IsEditedField: boolean;
    OldValue: string;
    ReturnToOriginValue: boolean;
    ControlType: string = "";
    NumberOfRequest: number;
    Key: string;
    OriginalFontSize: number;
    NewFontSize: number;
    IsFontSizeChange: boolean;
    IsTextValueChange: boolean;

    private status: string;
    public get Status() {
        return status;
    } 
    public set Status(value:string) {
        this.status = value;
        if (AppTool.IsNullOrEmpty(this.status)) {
            this.IsFontSizeChange = this.IsTextValueChange = false;
        }
    }


    constructor() {

    }


}

