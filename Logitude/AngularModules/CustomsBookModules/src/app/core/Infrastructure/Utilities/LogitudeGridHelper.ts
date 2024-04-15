export class LogitudeGridHelper {
    public SessionIndex: number;
    constructor(sessionIndex: number) {
        this.SessionIndex = sessionIndex;
    }

    private LogGridIndexId: number = null;
    private LogGridColumnsIndexId: number = null;
    private LogGridRowsIndexId: number = null;
    private EditableLogGridIndexId: number = null;
    private EditableLogGridColumnsIndexId: number = null;
    private EditableLogGridRowsIndexId: number = null;
    private ColumnId: number = null;
    private EditCellIndex: number = null;
    private ColumnsCount: number = null;
    private RowIndex: number = null;
    private DetailsDivIndex: number = null;
    private GridRowIndex: number = null;
    private ListComponentIndexId: number = null;
    

    public GetGridRowIndex(reset: boolean = false) {
        if (reset == false) {
            if (this.GridRowIndex == null) {
                this.GridRowIndex = 0;
            }
            else {
                this.GridRowIndex += 1;
            }
        }
        else {
            this.GridRowIndex = null;
        }
        return this.GridRowIndex;
    }

    public GetEditCellIndex() {

        if (this.EditCellIndex == null) {
            this.EditCellIndex = 0;
        }
        else {
            this.EditCellIndex += 1;
        }
        return this.EditCellIndex;
    }

    public SetRowIndex(GridId: string) {
        if (this.GridsRowIndex == null) {
            this.GridsRowIndex = [];
        }
        if (this.GridsRowIndex.filter(a => a.Id == GridId).length == 0) {
            this.GridsRowIndex.push({ Index: 0, Id: GridId });
        }
        else {
            this.GridsRowIndex.filter(a => a.Id == GridId)[0].Index += 1;
        }

        //if (this.RowIndex == null) {
        //    this.RowIndex = 0;
        //}
        //else {
        //    this.RowIndex += 1;
        //} 
        if (this.GridsRowIndex.filter(a => a.Id == GridId).length > 0) {
            return this.GridsRowIndex.filter(a => a.Id == GridId)[0].Index;
        }
        //return this.RowIndex;
    }

    GridsRowIndex: any[];
    public GetRowIndex(GridId: string) {
        //return this.RowIndex;
        if (this.GridsRowIndex.filter(a => a.Id == GridId).length > 0) {
            return this.GridsRowIndex.filter(a => a.Id == GridId)[0].Index;
        }
        else {
            this.GridsRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
    }

    public ResetRowIndex(GridId: string) {
        if (this.GridsRowIndex == null) {
            this.GridsRowIndex = [];
        }
        if (this.GridsRowIndex.filter(a => a.Id == GridId).length > 0) {
            this.GridsRowIndex.filter(a => a.Id == GridId)[0].Index = -1;
        }
        //this.RowIndex = 0;
    }

    public ResetNextRowIndex(GridId: string) {
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
        }
        if (this.GridsNextRowIndex.filter(a => a.Id == GridId).length > 0) {
            this.GridsNextRowIndex.filter(a => a.Id == GridId)[0].Index = 0;
        }
        //this.RowIndex = 0;
    }

    GridsNextRowIndex: any[];
    public SetNextRowIndex(GridId: string) {
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
        }
        if (this.GridsNextRowIndex.filter(a => a.Id == GridId).length == 0) {
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
        }
        else {
            this.GridsNextRowIndex.filter(a => a.Id == GridId)[0].Index += 1;
        }        
        //if (this.GridsNextRowIndex.filter(a => a.Id == GridId).length > 0) {
        //    return this.GridsNextRowIndex.filter(a => a.Id == GridId)[0].Index;
        //}
        //return this.RowIndex;
    }
    public GetNextRowIndex(GridId: string) {
        //return this.RowIndex;
        if (this.GridsNextRowIndex == null) {
            this.GridsNextRowIndex = [];
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
        if (this.GridsNextRowIndex.filter(a => a.Id == GridId).length > 0) {
            return this.GridsNextRowIndex.filter(a => a.Id == GridId)[0].Index;
        }
        else {
            this.GridsNextRowIndex.push({ Index: 0, Id: GridId });
            return 0;
        }
    }

    public ResetEditCellIndex() {
        this.EditCellIndex = null;
    }
    GridColumnsCount: any[] = [];
    public SetColumnsCount(reset: boolean, GridId: string) {
        if (this.GridColumnsCount == null) {
            this.GridColumnsCount = [];
        }
        if (reset == true) {
            if (this.GridColumnsCount.filter(a => a.Id == GridId).length == 0) {
                this.GridColumnsCount.push({ Count: 0, Id: GridId });
            }
            else {
                this.GridColumnsCount.filter(a => a.Id == GridId)[0].Count = 0;
            }
            //this.ColumnsCount = 0;
        }
        else {
            if (this.GridColumnsCount.filter(a => a.Id == GridId).length == 0) {
                this.GridColumnsCount.push({ Count: 0, Id: GridId });
            }
            else {
                this.GridColumnsCount.filter(a => a.Id == GridId)[0].Count += 1;
            }
            //if (this.ColumnsCount == null) {
            //    this.ColumnsCount = 0;
            //}
            //else {
            //    this.ColumnsCount += 1;
            //}
        }
        //return this.ColumnsCount;
    }

    public getColumnsCount(GridId: string) {
        if (this.GridColumnsCount.filter(a => a.Id == GridId).length > 0) {
            return this.GridColumnsCount.filter(a => a.Id == GridId)[0].Count;
        }
        //return this.ColumnsCount;
    }
    public GetLListComponentIndexId() {

        if (this.ListComponentIndexId == null) {
            this.ListComponentIndexId = 0;
        }

        else {
            this.ListComponentIndexId += 1;
        }

        return this.SessionIndex + "_" + this.ListComponentIndexId;
    }
    public GetLogGridIndexId() {

        if (this.LogGridIndexId == null) {
            this.LogGridIndexId = 0;
        }

        else {
            this.LogGridIndexId += 1;
        }

        return this.SessionIndex + "_" + this.LogGridIndexId;
    }
    public GetLogGridColumnsIndexId() {

        if (this.LogGridColumnsIndexId == null) {
            this.LogGridColumnsIndexId = 0;
        }

        else {
            this.LogGridColumnsIndexId += 1;
        }

        return this.SessionIndex + "_" + this.LogGridColumnsIndexId;
    }
    public GetDetailsDivId() {

        if (this.DetailsDivIndex == null) {
            this.DetailsDivIndex = 0;
        }

        else {
            this.DetailsDivIndex += 1;
        }

        return this.SessionIndex + "_" + this.DetailsDivIndex;
    }
    public GetLogGridRowsIndexId() {

        if (this.LogGridRowsIndexId == null) {
            this.LogGridRowsIndexId = 0;
        }

        else {
            this.LogGridRowsIndexId += 1;
        }

        return this.SessionIndex + "_" + this.LogGridRowsIndexId;
    }
    public GetEditableLogGridIndexId() {

        if (this.EditableLogGridIndexId == null) {
            this.EditableLogGridIndexId = 0;
        }

        else {
            this.EditableLogGridIndexId += 1;
        }

        return this.SessionIndex + "_" + this.EditableLogGridIndexId;
    }
    public GetEditableLogGridColumnsIndexId() {

        if (this.EditableLogGridColumnsIndexId == null) {
            this.EditableLogGridColumnsIndexId = 0;
        }

        else {
            this.EditableLogGridColumnsIndexId += 1;
        }

        return this.SessionIndex + "_" + this.EditableLogGridColumnsIndexId;
    }
    public GetEditableLogGridRowsIndexId() {

        if (this.EditableLogGridRowsIndexId == null) {
            this.EditableLogGridRowsIndexId = 0;
        }

        else {
            this.EditableLogGridRowsIndexId += 1;
        }

        return this.SessionIndex + "_" + this.EditableLogGridRowsIndexId;
    }
    public GetColumnId() {

        if (this.ColumnId == null) {
            this.ColumnId = 0;
        }

        else {
            this.ColumnId += 1;
        }
        //if (this.CurrentWindow) {
        //    return this.SessionIndex + "_" + this.ColumnId + this.CurrentWindow.WindowIndex;
        //}
        //else {
        return this.SessionIndex + "_" + this.ColumnId;
        //}
    }

}
