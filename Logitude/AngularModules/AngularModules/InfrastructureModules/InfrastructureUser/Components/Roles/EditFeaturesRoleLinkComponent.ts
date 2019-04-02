import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {RoleFeatureClass} from './EditRoleFeaturesComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './EditFeaturesRoleLinkComponent.html',
})

export class EditFeaturesRoleLinkComponent {
    public ItemsSource: RoleFeatureClass[] = [];
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
            myCloner.AddField('AccessLevelCode');
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
