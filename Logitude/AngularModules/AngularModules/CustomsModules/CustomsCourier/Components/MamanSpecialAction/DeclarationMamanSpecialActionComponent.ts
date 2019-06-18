import { Component, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { DeclarationMamanSpecialActionPM } from '../../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM';
import { DeclarationMamanSpecialActionPMService } from '../../../../Customs/Services/StandardPMs/DeclarationMamanSpecialActionPMService';
import { DeclarationWebService } from '../../../../Customs/Services/WebServices/DeclarationWebService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationMamanSpecialActionList } from '../../../../Customs/EntityLists/DeclarationMamanSpecialActionList';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationMamanSpecialActionComponent.html',
})

export class DeclarationMamanSpecialActionComponent
    extends BaseComponent {

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.DeclarationMamanSpecialAction";
    public EntityPM: DeclarationMamanSpecialActionPM;
    public _FetchDeclarationMamanSpecialActionList: ObservableCollection;
    _TerminalSuspentionNumber: string;

    
    constructor(public entityArgs: EntityArgs) {
        super();
        
        this._FetchDeclarationMamanSpecialActionList = new ObservableCollection([]);
    }

    SetWindowArgs(entityArgs: any) {
        this._TerminalSuspentionNumber = entityArgs.TerminalSuspentionNumber;
        //this._TerminalSuspentionNumber = "dd";

        let list: DeclarationMamanSpecialActionList[] = entityArgs.MamanSpecialActionList;
        this._FetchDeclarationMamanSpecialActionList.InsertCollection(list);
    }



    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }


}
