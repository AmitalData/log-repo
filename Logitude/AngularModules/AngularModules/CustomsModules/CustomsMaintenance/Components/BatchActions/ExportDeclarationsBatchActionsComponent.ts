import { Component } from '@angular/core';
import { DeclarationWebService, SendExportDeclarationsBatchRequestParams } from 'Customs/Services/WebServices/DeclarationWebService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { DataResult } from 'Customs/Services/Others/CourierMasterService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslationPipe } from 'Controls/Pipes/TextCodeTranslationPipe';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'export-declarations-batch-actions',
    templateUrl: './ExportDeclarationsBatchActionsComponent.html',
    providers: [TextCodeTranslationPipe]
})
export class ExportDeclarationsBatchActionsComponent extends BaseComponent {
    ObjectTableName = 'Customs.Declaration';
    DataContext: any = this;
    ValidationErrorsList: string[] = [];

    Actions: Array<{ code: string; Name: string }> = [];

    IsBusy = false;
    private _isConfirmOpen = false;

    Ready = false;
    IsContinueEnabled = false;
    ws: DeclarationWebService;

    constructor(
        private _entityResourceService: EntityResourceService,
        private t: TextCodeTranslationPipe
    ) {
        super();
        this._entityResourceService
            .getEntityResourceByTableName('Customs.Declaration', 0)
            .subscribe(() => {
                this.Ready = true;
                this.Actions = [
                    { code: 'CheckStatus', Name: this.t.transform('Customs.ExportDeclarations.BatchActions.O.CheckStatus') as string },
                    { code: 'OperationalClose', Name: this.t.transform('Customs.ExportDeclarations.BatchActions.O.CloseOperational') as string },
                    { code: 'DeclarationRestore', Name: this.t.transform('Customs.Declaration.B.DeclarationRestore') as string },

                ];
                this.ws = new DeclarationWebService();
            });
    }

    private _selectedAction: { code: string; Name: string } | null = null;
    get SelectedAction() { return this._selectedAction; }
    set SelectedAction(v: { code: string; Name: string } | null) {
        this._selectedAction = v;
        this.updateContinueState();
    }

    get ParsedIds(): string[] {
        return (this.DeclarationIds || '')
            .split(/[,;\n\r\t ]+/g)
            .map(s => s.trim())
            .filter(s => !!s);
    }

    updateContinueState() {
        this.IsContinueEnabled = !!this.SelectedAction && this.ParsedIds.length > 0 && !this.IsBusy;
    }

    private _declarationIds = '';
    public get DeclarationIds(): string { return this._declarationIds; }
    public set DeclarationIds(v: string) {
        this._declarationIds = v || '';
        this.updateContinueState();
    }

    CancelButtonClicked() {
        if ((this as any).CurrentSession?.CloseCurrentWindow) {
            (this as any).CurrentSession.CloseCurrentWindow();
        }
    }

    ContinueButtonClicked() {
        if (this.IsBusy || this._isConfirmOpen) return;

        this.ValidationErrorsList = [];
        if (!this.SelectedAction) this.ValidationErrorsList.push(this.t.transform('Customs.ExportDeclarations.BatchActions.E.SelectAction') as string);
        if (this.ParsedIds.length === 0) this.ValidationErrorsList.push(this.t.transform('Customs.ExportDeclarations.BatchActions.E.EnterIds') as string);
        if (this.ValidationErrorsList.length > 0) return;

        const confirmTitle = this.t.transform('Customs.ExportDeclarations.BatchActions.M.ConfirmTitle') as string;
        const confirmBodyTpl = this.t.transform('Customs.ExportDeclarations.BatchActions.M.ConfirmBody') as string;
        const confirmBody = this.format(confirmBodyTpl as string, [
            this.ParsedIds.length.toString(),
            this.SelectedAction?.Name || ''
        ]);

        const dlg = new ConfirmWindow();
        this._isConfirmOpen = true;
        dlg.YesButtonText = (this.t.transform('General.B.Yes') as string) || 'Yes';
        dlg.ShowNoButton = true;
        if (typeof (dlg as any).SetTitle === 'function') {
            (dlg as any).SetTitle(confirmTitle);
            dlg.Show(confirmBody);
        } else {
            dlg.Show(confirmTitle + '\n\n' + confirmBody);
        }

        dlg.WindowClosed.subscribe(() => {
            this._isConfirmOpen = false;
            if ((dlg as any).Yes) {
                this.IsBusy = true;
                this.updateContinueState();
                dlg.Close();
                this.sendBatch();
            }
        });
    }

    private sendBatch() {
        const filters = new ApiQueryFilters();
        filters.GetAll = false;

        const req: SendExportDeclarationsBatchRequestParams = {
            Action: this.SelectedAction?.code || '',
            SelectedIds: this.ParsedIds,
            IsAllSelected: false
        };

        this.ws.PostExportDeclarationsBatchActions(req, filters).pipe(finalize(() => { this.IsBusy = false; this.updateContinueState(); })).subscribe(
            (res: DataResult) => {
                const msgTpl = this.t.transform('Customs.ExportDeclarations.BatchActions.M.BatchSent') as string;
                const msg = this.format(msgTpl as string, [
                    res && res.Message ? String(res.Message) : '0',
                    res && (res as any).RequestInProgressList ? String((res as any).RequestInProgressList) : '0'
                ]);
                this.showInfo(msg);
                this.CancelButtonClicked();
            },
            () => {
                this.showError(this.t.transform('Customs.ExportDeclarations.BatchActions.E.SendFailed') as string);
            }
        );
    }

    private showInfo(message: string) {
        const mw = new MessageWindow();
        if (typeof (mw as any).Info === 'function') { (mw as any).Info(message); return; }
        if (typeof (mw as any).Show === 'function') { (mw as any).Show(message); return; }
    }

    private showError(message: string) {
        const mw = new MessageWindow();
        if (typeof (mw as any).Error === 'function') { (mw as any).Error(message); return; }
        if (typeof (mw as any).Show === 'function') { (mw as any).Show(message); return; }
    }

    private format(tpl: string, args: string[]) {
        let s = tpl || '';
        args.forEach((v, i) => { s = s.replace(new RegExp('\\{' + i + '\\}', 'g'), v == null ? '' : v); });
        return s;
    }
}
