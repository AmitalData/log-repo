import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { JournalAnalyseResult } from '../../EntityPMs/JournalAnalyseResult';
import { BankAccountPM } from '../../EntityPMs/BankAccountPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { JournalPMService } from '../../Services/StandardPMs/JournalPMService';
import { CurrencyPMService } from '../../../Common/Services/StandardPMs/CurrencyPMService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { JournalExtendedPMService } from '../../Services/ExtendedPMs/JournalExtendedPMService';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { DefaultAndConfigurationExtendedService } from 'Infrastructure/Services/ExtendedPMs/DefaultAndConfigurationExtendedService';
import { JournalLinePM } from 'Accounting/EntityPMs/JournalLinePM';
declare var attachmentUploader

@Component({
    selector: 'JournalMichpalLoadComponent',
    templateUrl: './JournalMichpalLoadComponent.html',
})
export class JournalMichpalLoadComponent extends BaseComponent  
{
    public dataContext: JournalMichpalLoadComponent = this;
    public objectTableName = 'Journal';

    public uploadFileId: string = Guid.NewRandomString();
    public fileName: string;
    public fileExtension: string;

    public hasError = false;
    public errorLog: string;
    public newJournalNumber: string;

    public createdJournal: JournalPM;

    private readonly currentSession = SessionLocator.SelectedSession;
    private readonly journalExtendedService = new JournalExtendedPMService();

    constructor() {
        super();       
    }

    public cancel(): void {
        this.currentSession.CloseCurrentWindow();
    }

    public openUploadFile(): void {
        document.getElementById(this.uploadFileId).click();
    }

    public showMessage(message: string): void {
        new MessageWindow().Show(message);
    }


    public uploadFile(): void {
        const file = attachmentUploader(this.uploadFileId);
        if (!file) return;

        this.fileName = file.name;
        this.fileExtension = file.name.split('.').pop();

        const uploadParams = new ImageParameter();
        uploadParams.Key = Guid.newGuid();
        uploadParams.Extension = this.fileExtension;
        uploadParams.UploadMode = 'Block';
        uploadParams.FileSize = file.size;
        uploadParams.Tenant = SessionLocator.Tenant;

        this.readFileAsBuffer(file, uploadParams);
    }

    private readFileAsBuffer(file: File, uploadParams: ImageParameter): void {
        const reader = new FileReader();

        reader.onload = (e: any) => {
            const buffer = e.target.result as ArrayBuffer;
            uploadParams.Base64String = btoa(
                String.fromCharCode(...new Uint8Array(buffer))
            );

            this.parseFile(buffer, uploadParams);
        };

        reader.readAsArrayBuffer(file);
    }


    private parseFile(buffer: ArrayBuffer, uploadParams: ImageParameter): void {

        const bytes = new Uint8Array(buffer);    
        const lines: Uint8Array[] = [];
        let start = 0;
    
        for (let i = 0; i < bytes.length; i++) {
            if (bytes[i] === 0x0A) {
                lines.push(bytes.slice(start, i));
                start = i + 1;
            }
        }
    
        const dataLines = lines.slice(1, lines.length - 1);
    
        const journal = this.createJournal();
        let total = 0;
        const errors: string[] = [];
    
        dataLines.forEach((lineBytes, index) => {
            try {
                const journalLine = this.parseLineBytes(lineBytes, index + 1, journal);
                journal.JournalLines.push(journalLine);
                total += Math.abs(journalLine.LocalAmount);
            } catch (e) {
                errors.push(`Line ${index + 1}: ${e.message}`);
            }
        });
        
        if (errors.length) {
            this.errorLog = errors.join('\n');
            return;
        }
        journal.AccountingDate = journal.JournalLines[0]?.AccountingDate;
        this.submitJournal(journal);
    }

    private parseLineBytes(
        line: Uint8Array,
        lineNumber: number,
        journal: JournalPM
    ): JournalLinePM {
    
        const ascii = (from: number, to: number) =>
            String.fromCharCode(...line.slice(from, to)).trim();
    
        const debitAccount  = ascii(0, 8);
        const creditAccount = ascii(8, 16);
    
        const reference1 = ascii(16, 21).replace(/\D+/g, '') || '0';
        const referenceDate = this.parseDate(ascii(21, 27));
        const reference2 = ascii(27, 32).replace(/\D+/g, '') || '0';
        const valueDate = this.parseDate(ascii(32, 38));
    
        const amount = this.parseAmount(ascii(38, 50));
    
        const notesBytes = line.slice(53, 75);
    
        const journalLine = new JournalLinePM(journal);
        journalLine.Line = lineNumber;
        journalLine.Tenant = SessionLocator.Tenant;
        journalLine.JournalId = journal.Id;
        journalLine.AccountingDate = valueDate;
        journalLine.DocumentDate = referenceDate;
        journalLine.DueDate = valueDate;
        journalLine.ActionCode = debitAccount ? '2' : '1';
        journalLine.DebitAccountNumber = debitAccount;
        journalLine.CreditAccountNumber = creditAccount;
        journalLine.LocalAmount = amount;
        journalLine.ForeignAmount = amount;
        journalLine.Reference1 = reference1;
        journalLine.Reference2 = reference2;    
        journalLine.Notes = btoa(
            String.fromCharCode(...notesBytes)
        );
    
        journalLine.CurrencyId = SessionLocator.AccountingCurrencyId;
        return journalLine;
    }
    
   
    private parseDate(value: string): Date {
        if (!/^\d{6}$/.test(value)) throw new Error(`Invalid date ${value}`);
        const day = +value.slice(0, 2);
        const month = +value.slice(2, 4) - 1;
        const year = 2000 + +value.slice(4, 6);
        return new Date(year, month, day);
    }

    private parseAmount(raw: string): number {
        if (!raw || raw.length < 2) {
            throw new Error(`Invalid amount: "${raw}"`);
        }
    
        const signChar = raw[0];
        if (signChar !== '+' && signChar !== '-') {
            throw new Error(`Amount missing sign (+/-): "${raw}"`);
        }
    
        const numericPart = raw
            .substring(1)
            .replace(/\s+/g, ''); 
    
        if (!/^\d+$/.test(numericPart)) {
            throw new Error(`Amount contains non-numeric characters: "${raw}"`);
        }
    
        if (numericPart.length > 11) {
            throw new Error(`Amount has more than 11 digits: "${raw}"`);
        }
    
        const padded = numericPart.padStart(11, '0');
        const value = Number(padded) / 100;
    
        return signChar === '-' ? -value : value;
    }

    private createJournal(): JournalPM {
        const journal = new JournalPM();
        journal.Tenant = SessionLocator.Tenant;
        journal.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        journal.UpdateDate = journal.CreateDate;
        journal.TypeCode = '0';
        journal.StatusCode = '1';
        journal.AccountingEntityCode = '1';
        journal.CreatedByUserId = SessionLocator.LoggedUserId;
        journal.ApprovedByUserId = SessionLocator.LoggedUserId;
        return journal;
    }

   

    private submitJournal(journal: JournalPM): void {
        this.currentSession.StartBusyIndicatorCreating();

        this.journalExtendedService
            .PostJournalAsMichpal(journal)
            .subscribe((response: ServiceResponse) => {
                this.currentSession.StopBusyIndicator();

                if (response.HasError) {
                    this.hasError = true;
                    this.errorLog = response.ErrorsArray.join(',');
                    return;
                }

                this.createdJournal = response.Result;
                this.newJournalNumber = this.createdJournal.JournalNumber;
            });
    }
    openJournal() {
        if (!AppTool.IsNullOrEmpty(this.createdJournal.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.currentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.createdJournal.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }
}
