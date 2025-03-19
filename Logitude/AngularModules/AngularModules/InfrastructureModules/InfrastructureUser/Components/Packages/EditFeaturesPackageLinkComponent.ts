import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {PackageFeatureClass} from './EditPackageFeaturesComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { AppTool } from 'Infrastructure/Tools';

@Component({
    
    templateUrl: './EditFeaturesPackageLinkComponent.html',
})

export class EditFeaturesPackageLinkComponent {
    public ItemsSource: PackageFeatureClass[] = [];
    public ItemsSourceOriginal: PackageFeatureClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;    
    private searchTimeout: any; 
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.ItemsSource = args['Items'];
        this.ItemsSourceOriginal=args['Items'];
        this.Clone();
    }

    CancelButtonClicked() {        
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    private AllCloners: Cloner[] = [];
    private Clone() {
        this.ItemsSource.forEach(item => {
            var myCloner = new Cloner(item);
            myCloner.AddField('IsActive');
            myCloner.AddEntity(item.Feature);

            this.AllCloners.push(myCloner);
        });
    }
    private RejectChanges() {
        this.AllCloners.forEach(myCloner => {
            myCloner.RejectChanges();
        });
    }

    SearchFeatures(text: string): void {
        if (!AppTool.IsNullOrEmpty(text) && !AppTool.IsNullOrEmpty(this.ItemsSourceOriginal) && this.ItemsSourceOriginal.length > 0) {
            const searchText = text.trim().toLowerCase();    
            clearTimeout(this.searchTimeout);
            this.searchTimeout = setTimeout(() => {
                this.ItemsSource = this.ItemsSourceOriginal.filter(d => 
                    typeof d["Name"] === "string" && d["Name"].toLowerCase().includes(searchText)
                );
            }, 200);        
        } else {
            this.ItemsSource = this.ItemsSourceOriginal;        
        }   
    }    
}
