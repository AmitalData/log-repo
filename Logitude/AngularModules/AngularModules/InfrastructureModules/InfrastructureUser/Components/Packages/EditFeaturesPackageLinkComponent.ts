import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {PackageFeatureClass} from './EditPackageFeaturesComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './EditFeaturesPackageLinkComponent.html',
})

export class EditFeaturesPackageLinkComponent {
    public ItemsSource: PackageFeatureClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.ItemsSource = args['Items'];
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
}
