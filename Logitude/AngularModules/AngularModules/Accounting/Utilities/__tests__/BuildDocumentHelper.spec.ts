import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { BuildDocumentHelper } from '../BuildDocumentHelper';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

const createDocsOutContext = () => ({
    CurrentDocument: {
        Id: 'DOC-1',
        Tenant: 77,
        DocumentTemplateEditorTool: 'A',
        DocumentOutCopies: [],
        NeedsRebuild: false,
    },
    DocumentTypePM: { Id: 'TYPE-1', IsDocumentOneTimePrintLimited: false },
    invoiceType: 'INV',
});

const createHelper = (dataContext = createDocsOutContext()) => {
    const helper = Object.create(BuildDocumentHelper.prototype) as BuildDocumentHelper;
    helper.DataContext = dataContext as any;
    helper.CurrentDocumentOut = dataContext.CurrentDocument as any;
    helper.ObjectTableId = 'OBJ';
    helper.EntityId = 'ENTITY';
    (helper as any).LoadCopiesControl = jest.fn();
    (helper as any).UpdateDocument = jest.fn();
    (helper as any)._documentOutPMService = {
        getSingleDocumentOutPM: jest.fn().mockReturnValue(of({ HasError: false, Result: { Id: 'DOC-UPDATED' } })),
    };
    (helper as any)._documentTypePMService = {
        getSingleDocumentType: jest.fn().mockReturnValue(of({ HasError: false, Result: {} })),
    };
    (helper as any)._documentTypeCustomFieldService = {
        getDocumentTypeCustomFieldsByDocument: jest.fn().mockReturnValue(of({ HasError: false, Result: [] })),
    };
    (helper as any)._exportDocumentService = {
        GetIsRunStimulDocumentViaWorkerRole: jest.fn().mockReturnValue(of(false)),
    };
    (helper as any)._documentTypeTemplateListExtendedService = {
        getDocumentTypeTemplates: jest.fn(),
    };
    (helper as any)._htmlEditorService = {
        GetHtmlEditorData: jest.fn(),
    };
    (helper as any)._entityResourceService = {
        getEntityResourceByTableName: jest.fn().mockReturnValue(of({})),
    };
    helper.IsSystemAdditionalPrintingFields = false;
    helper.Start = jest.fn() as any;
    return helper;
};

describe('BuildDocumentHelper', () => {
    beforeEach(() => {
        SessionLocator.SelectedSession = {
            StartBusyIndicator: jest.fn(),
            StopBusyIndicator: jest.fn(),
            CurrentEditComponent: {
                EntityPM: {},
                SaveCompleted: { subscribe: jest.fn() },
            },
        } as any;
        SessionLocator.TenantPM = { AccountingActivated: true } as any;
        SessionLocator.LoggedUserPM = { DontShowLocal: false } as any;
        (global as any).window = {
            TextCodesCache: [],
        };
    });

    it('UpdateDocumentsAutomatically loads copies and updates document when service returns result', () => {
        const helper = createHelper();

        helper.UpdateDocumentsAutomatically();

        const docService = (helper as any)._documentOutPMService;
        expect((helper as any).LoadCopiesControl).toHaveBeenCalled();
        expect(docService.getSingleDocumentOutPM).toHaveBeenCalledWith('DOC-1', 77);
        expect((helper as any).UpdateDocument).toHaveBeenCalled();
        expect(helper.CurrentDocumentOut.Id).toBe('DOC-UPDATED');
    });

    it('UpdateDocumentsAutomatically skips update when service returns error', () => {
        const helper = createHelper();
        const docService = (helper as any)._documentOutPMService;
        docService.getSingleDocumentOutPM.mockReturnValue(of({ HasError: true }));

        helper.UpdateDocumentsAutomatically();

        expect((helper as any).UpdateDocument).not.toHaveBeenCalled();
    });

    it('LoadDocumentCustomFields populates args and invokes service', () => {
        const helper = createHelper();
        helper.LoadDocumentCustomFields();

        const customFieldService = (helper as any)._documentTypeCustomFieldService;
        expect(helper.DocumentCustomFieldsArgs).toBeDefined();
        expect(customFieldService.getDocumentTypeCustomFieldsByDocument).toHaveBeenCalledWith(77, 'TYPE-1');
    });
});

