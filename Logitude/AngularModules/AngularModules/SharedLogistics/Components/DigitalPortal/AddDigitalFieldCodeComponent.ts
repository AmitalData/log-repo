import { Component } from '@angular/core';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DigitalTextService, DigitalTextCodeObject } from '../../../Infrastructure/Services/WebServices/DigitalTextService'
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    templateUrl: './AddDigitalFieldCodeComponent.html',
})

export class AddDigitalFieldCodeComponent {

    public LabelsItemsSource: ObservableCollection;
    ObjectTableId: string;
    digitalTextService: DigitalTextService;
    SelectedDigitalFieldCode: DigitalTextCodeObject;
    private CurrentSession = SessionLocator.SelectedSession;
    IsAddingComponent = false;

    constructor() {
       
    }

    SetWindowArgs(args: any) {
        this.LabelsItemsSource = new ObservableCollection([]);
        this.digitalTextService = new DigitalTextService();
        this.ObjectTableId = args.ObjectTableId;
        this.BuildItemsSource();
    }

    BuildItemsSource() {
        this.LabelsItemsSource = new ObservableCollection([]);
        this.digitalTextService.GetTextCodesByFilters(null, this.ObjectTableId, null).subscribe((myResult) => {
            if (!myResult.HasError) {
                var data = myResult.Result?.filter(a => !AppTool.IsNullOrEmpty(a.FieldCode));
                if (!AppTool.IsNullOrEmpty(this.SearchText)) {
                    data = data.filter(f => f.FieldCode.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1);
                }
                
                this.LabelsItemsSource.InsertCollection(data);
            }
        });
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.BuildItemsSource();
        }
    }


    Selecting(item) {
        this.SelectedDigitalFieldCode = item;
    }

    AddFieldComponentClicked() {
        this.IsAddingComponent = true;
        var fieldcode = this.SelectedDigitalFieldCode?.FieldCode;
        this.CurrentSession.CloseCurrentWindowEmit(fieldcode);
    }

    AddFieldCodeClicked() {
        this.IsAddingComponent = false;
        var fieldcode = this.SelectedDigitalFieldCode?.FieldCode;
        this.CurrentSession.CloseCurrentWindowEmit(fieldcode);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
