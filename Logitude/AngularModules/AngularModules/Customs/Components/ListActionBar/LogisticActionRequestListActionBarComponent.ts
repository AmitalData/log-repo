import { Component, AfterViewInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogisticActionRequestsCloseSharedDataService } from 'Customs/Services/DataChange/LogisticActionRequestCloseSharedDataService';
import { LogisticActionRequestWebService } from 'Customs/Services/WebServices/LogisticActionRequestWebService';
// import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';


@Component({
    templateUrl: './LogisticActionRequestListActionBarComponent.html',
})

export class LogisticActionRequestListActionBarComponent
    extends BaseComponent
    implements AfterViewInit {

    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: LogisticActionRequestListActionBarComponent = this;
    public ObjectTableName: string = "Customs.LogisticActionRequest";
    private _LogisticActionRequestWebService: LogisticActionRequestWebService = new LogisticActionRequestWebService(LogtuideTableDataService.createInstance());

    constructor(
        public _LogisticActionRequestsCloseSharedDataService: LogisticActionRequestsCloseSharedDataService,
    ) {
        super();
    }


    ngAfterViewInit() {
        SessionLocator.SelectedSession.CurrentListComponent.onRefershQueryEvent.subscribe(data => {
            this._LogisticActionRequestsCloseSharedDataService._SelectedItems.Clear();
            this._LogisticActionRequestsCloseSharedDataService.IsDisplayButtonClose = false;
        });
    }


    get count() {
        return this._LogisticActionRequestsCloseSharedDataService._SelectedItems.Collection.length.toString()
    }


    CloseMarkChecks() {
        SessionLocator.SelectedSession.StartBusyIndicator("");
        const ids: string[] = this._LogisticActionRequestsCloseSharedDataService._SelectedItems.Collection;        

        this._LogisticActionRequestWebService.closeRequest(ids)
            .subscribe((myResponse: ServiceResponse) => {

                SessionLocator.SelectedSession.StopBusyIndicator();
                let messageWindow = new MessageWindow();
                messageWindow.Width = 300;
                messageWindow.Height = 180;

                if (!myResponse.HasError) {
                    messageWindow.Show(ids.length + " " + TextCodeTranslator.Translate('Customs.General.O.RequestClosed'));
                    this.CurrentSession.CurrentListComponent.RefreshBtnClick();
                }
                else
                    messageWindow.Show(TextCodeTranslator.Translate('Customs.General.O.Fail'));
            });
    }
}
