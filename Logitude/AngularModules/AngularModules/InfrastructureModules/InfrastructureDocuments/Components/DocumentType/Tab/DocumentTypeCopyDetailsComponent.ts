import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component }  from '@angular/core';
import { DocumentCopiesViewModel } from '../../DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './DocumentTypeCopyDetailsComponent.html',
})

export class DocumentTypeCopyDetailsComponent extends BaseComponent{
    public DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;
    ObjectTableName: string = "DocumentTypeCopy";
    DocumentCopy: DocumentCopiesViewModel;

    constructor() {
        super();
        this.DataContext = this;
    }

    SetWindowArgs(args: any) {
        this.DocumentCopy = args.Item;
        if (this.DocumentCopy) {
            this.Name = this.DocumentCopy.Name;
        }
    }

    SaveButtonClicked() {
        this.CurrentSession.CurrentWindow.Close(this.Name);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private name: string;
    get Name() { return this.name; }
    set Name(value: string) {
        if (this.name != value) {
            this.name = value;
        }
    }
}
