import { expect } from '@jest/globals';
import { of } from 'rxjs';

const mockGetAllFromCache = jest.fn();

jest.mock('Common/Services/StandardLists/DocumentTypeListService', () => ({
    DocumentTypeListService: jest.fn().mockImplementation(() => ({
        getAllFromCache: mockGetAllFromCache
    }))
}));

jest.mock('Common/Services/ExtendedPMs/DocumentTypePMExtendedService', () => ({
    DocumentTypePMExtendedService: jest.fn().mockImplementation(() => ({
        getSingleDocumentType: jest.fn()
    }))
}));

jest.mock('Common/Services/ExtendedPMs/DocumentOutPMService', () => ({
    DocumentOutPMService: jest.fn().mockImplementation(() => ({
        getDocumentOutByDocumentTypeEntityAndChild: jest.fn(),
        getCreateDocumentOut: jest.fn()
    }))
}));

jest.mock('Common/Services/ExtendedPMs/DocumentTypeCustomFieldService', () => ({
    DocumentTypeCustomFieldService: jest.fn().mockImplementation(() => ({}))
}));

jest.mock('Common/Services/DocumentServices/ExportDocumentService', () => ({
    ExportDocumentService: jest.fn().mockImplementation(() => ({
        GetIsRunStimulDocumentViaWorkerRole: jest.fn()
    }))
}));

jest.mock('Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService', () => ({
    DocumentTypeTemplateListExtendedService: jest.fn().mockImplementation(() => ({}))
}));

jest.mock('Common/Services/DocumentServices/HtmlEditorService', () => ({
    HtmlEditorService: jest.fn().mockImplementation(() => ({}))
}));

jest.mock('Accounting/DataContracts/InterestReportArgs', () => ({
    SelectItem: class {
        public Id = '';
        public SecurityId = '';
        public TempId = '';
    },
    InterestReportArguments: class {
        public Entities: any[] = [];
        public SelectedItems: any[] = [];
    }
}));

const mockSession = {
    StartBusyIndicatorLoading: jest.fn(),
    StartBusyIndicator: jest.fn(),
    StopBusyIndicator: jest.fn(),
    CurrentWindow: { StartBusyIndicator: jest.fn(), StopBusyIndicator: jest.fn() }
};

jest.mock('Infrastructure/Utilities/SessionLocator', () => ({
    SessionLocator: {
        SelectedSession: mockSession,
        Tenant: 1
    }
}));

jest.mock('Infrastructure/Locators/ObjectsLocator', () => ({
    ObjectsLocator: {
        GlobalSetting: { LayoutDirection: 'ltr' }
    }
}));

jest.mock('Controls/Windows/MessageWindow', () => ({
    MessageWindow: jest.fn().mockImplementation(() => ({
        Show: jest.fn(),
        Title: '',
        Width: 0,
        RTL: false
    }))
}));

jest.mock('Infrastructure/Helpers/GeneralPrintHelper', () => ({
    GeneralPrintHelper: jest.fn().mockImplementation(() => ({
        IsLoadPrintControl: true,
        ShowPrintControl: jest.fn()
    }))
}));

import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { BatchPrintServiceHelper } from '../BatchPrintServiceHelper';

describe('BatchPrintServiceHelper PreparedPrintVariable', () => {
    beforeEach(() => {
        mockGetAllFromCache.mockReset();
        SessionInfo.LoggedUserTenant = 5;
    const w = global.window as any;
    w.URL = { createObjectURL: jest.fn() };
    w.ObjectTables = [
        { Name: 'ARInvoice', Id: 'OBJ-ARINV' },
        { Name: 'ARPayment', Id: 'OBJ-ARPAY' },
        { Name: 'APPayment', Id: 'OBJ-APPAY' }
    ];
    w.open = jest.fn();
    });

    it('marks control for load when default template exists', () => {
        mockGetAllFromCache.mockReturnValue(
            of({
                HasError: false,
                Result: [{ Code: '999G', DocumentTypeDefaultReportTemplateId: 'TPL-1' }]
            })
        );

        const helper = new BatchPrintServiceHelper();
        helper.PreparedPrintVariable('ARInvoice', '999g', 'E1', null, null, null);

        expect(helper.IsLoadPrintControl).toBe(true);
        expect(helper.DocumentTypeCode).toBe('999G');
        expect(helper.EntityId).toBe('E1');
        expect(helper.ChildReference).toBe('');
    });

    it('shows message when default template missing', () => {
        mockGetAllFromCache.mockReturnValue(
            of({
                HasError: false,
                Result: [{ Code: '999G', DocumentTypeDefaultReportTemplateId: null }]
            })
        );

        const helper = new BatchPrintServiceHelper();
        const showSpy = jest.spyOn(helper, 'ShowMessage').mockImplementation(() => undefined);

        helper.PreparedPrintVariable('ARInvoice', '999g', 'E1', null, null, null);

        expect(showSpy).toHaveBeenCalledWith('Document type of code 999G has no default template');
        expect(helper.IsLoadPrintControl).toBe(false);
    });

    it('shows specific message when document type missing for ARPayment', () => {
        mockGetAllFromCache.mockReturnValue(
            of({
                HasError: false,
                Result: []
            })
        );

        const helper = new BatchPrintServiceHelper();
        const showSpy = jest.spyOn(helper, 'ShowMessage').mockImplementation(() => undefined);

        helper.PreparedPrintVariable('ARPayment', '123', 'E1', null, null, null);

        expect(showSpy).toHaveBeenCalledWith(
            'There is no document type for A/R Payment please go to maintenance and add it!'
        );
    });

    it('shows specific message when document type missing for APPayment', () => {
        mockGetAllFromCache.mockReturnValue(
            of({
                HasError: false,
                Result: []
            })
        );

        const helper = new BatchPrintServiceHelper();
        const showSpy = jest.spyOn(helper, 'ShowMessage').mockImplementation(() => undefined);

        helper.PreparedPrintVariable('APPayment', '123', 'E1', null, null, null);

        expect(showSpy).toHaveBeenCalledWith(
            'There is no document type for A/P Payment please go to maintenance and add it!'
        );
    });
});

