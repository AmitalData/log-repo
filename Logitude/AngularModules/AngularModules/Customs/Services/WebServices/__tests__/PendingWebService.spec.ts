import { expect } from '@jest/globals';
import { of, throwError } from 'rxjs';
import { PendingWebService } from '../PendingWebService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

describe('PendingWebService', () => {
    const baseUrl = 'https://customs/';
    let httpClient: { get: jest.Mock; post: jest.Mock };
    let tableDataService: {
        apiQueryFilterToQueryString: jest.Mock;
        sendAjaxAndGetDataStandart: jest.Mock;
        standartSendAjax: jest.Mock;
    };
    let handleErrorSpy: jest.SpyInstance;

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn()
        };
        tableDataService = {
            apiQueryFilterToQueryString: jest.fn().mockReturnValue('skip=0&take=10'),
            sendAjaxAndGetDataStandart: jest.fn().mockReturnValue(Promise.resolve('ok')),
            standartSendAjax: jest.fn().mockReturnValue(Promise.resolve('ok'))
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        handleErrorSpy = jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('postBulkFeeding posts payload and delegates to table data service', async () => {
        httpClient.post.mockReturnValue(of({}));

        const service = new PendingWebService(tableDataService as any);
        const customFilter = new ApiQueryFilters();

        await service.postBulkFeeding(
            ['pending'],
            ['remark'],
            ['decl-1'],
            'CM-1',
            true,
            ['without'],
            customFilter,
            true
        );

        expect(httpClient.post).toHaveBeenCalledWith(
            `${baseUrl}api/PendingWebService/BulkFeeding?skip=0&take=10`,
            {
                listPending: ['pending'],
                listPendingRemark: ['remark'],
                declarationIdsList: ['decl-1'],
                allWithoutdeclarationIdsList: ['without'],
                courierMasterId: 'CM-1',
                checkboxAll: true,
                isCreateInvoiceDocument: true
            },
            { headers: { Authorization: 'token' } }
        );
        expect(tableDataService.sendAjaxAndGetDataStandart).toHaveBeenCalled();
    });

    it('PostSendMultiUpdate posts request and maps response', done => {
        const response = { Message: 'done', RequestInProgressList: [] };
        httpClient.post.mockReturnValue(of(response));

        const service = new PendingWebService(tableDataService as any);
        const requestParams = { ids: ['1'] } as any;
        const filters = new ApiQueryFilters();

        service.PostSendMultiUpdate(requestParams, filters).subscribe(res => {
            const data = res as any;
            expect(tableDataService.apiQueryFilterToQueryString).toHaveBeenCalled();
            expect(data.Message).toBe('done');
            expect(data.RequestInProgressList).toEqual([]);
            done();
        });
    });

    it('PostSendMultiUpdate propagates errors through observable', done => {
        const error = new Error('multi-update failed');
        httpClient.post.mockReturnValue(throwError(() => error));

        const service = new PendingWebService(tableDataService as any);

        service.PostSendMultiUpdate({} as any, new ApiQueryFilters()).subscribe({
            next: () => done(new Error('expected error')),
            error: () => {
                expect(handleErrorSpy).toHaveBeenCalled();
                done();
            },
        });
    });
});

