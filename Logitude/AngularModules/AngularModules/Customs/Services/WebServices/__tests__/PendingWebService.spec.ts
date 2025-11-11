import { expect } from '@jest/globals';
import { of, throwError } from 'rxjs';
import { PendingWebService } from '../PendingWebService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

describe('PendingWebService', () => {
    const baseUrl = 'https://customs/';
    let httpClient: any;
    let tableDataService: any;

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn(),
        };
        tableDataService = {
            apiQueryFilterToQueryString: jest.fn().mockReturnValue('filters'),
            sendAjaxAndGetDataStandart: jest.fn().mockReturnValue(Promise.resolve('ok')),
        };
        ServiceHelper.HttpClient = httpClient;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation((err: any) => {
            const actual = err instanceof Error ? err : err();
            throw actual;
        });
    });

    afterEach(() => jest.restoreAllMocks());

    it('postBulkFeeding builds request payload and delegates to table service', async () => {
        httpClient.post.mockReturnValue(of({}));
        const service = new PendingWebService(tableDataService);
        const filters = new ApiQueryFilters();

        const promise = service.postBulkFeeding(
            ['pending'],
            ['remark'],
            ['decl'],
            'CM-1',
            true,
            ['without'],
            filters,
            true
        );

        await expect(promise).resolves.toBe('ok');
        expect(tableDataService.apiQueryFilterToQueryString).toHaveBeenCalledWith(filters);
        expect(httpClient.post).toHaveBeenCalledWith(
            `${baseUrl}api/PendingWebService/BulkFeeding?filters`,
            {
                listPending: ['pending'],
                listPendingRemark: ['remark'],
                declarationIdsList: ['decl'],
                allWithoutdeclarationIdsList: ['without'],
                courierMasterId: 'CM-1',
                checkboxAll: true,
                isCreateInvoiceDocument: true,
            },
            { headers: { Authorization: 'token' } }
        );
        expect(tableDataService.sendAjaxAndGetDataStandart).toHaveBeenCalledTimes(1);
    });

    it('PostSendMultiUpdate posts via http client', done => {
        httpClient.post.mockReturnValue(of({ Message: 'done', RequestInProgressList: [] }));
        const service = new PendingWebService(tableDataService);
        const filters = new ApiQueryFilters();

        service.PostSendMultiUpdate({ kind: 'test' } as any, filters).subscribe(res => {
            expect(httpClient.post).toHaveBeenCalledWith(
                `${baseUrl}api/PendingWebService/PostSendMultiUpdate?filters`,
                JSON.stringify({ kind: 'test' }),
                { headers: { Authorization: 'token' } }
            );
            expect((res as any).Message).toBe('done');
            done();
        });
    });

    it('PostSendMultiUpdate propagates errors', done => {
        const error = new Error('multi-update failed');
        httpClient.post.mockReturnValue(throwError(() => error));
        const service = new PendingWebService(tableDataService);

        service.PostSendMultiUpdate({} as any, new ApiQueryFilters()).subscribe({
            next: () => done(new Error('expected error')),
            error: err => {
                expect(err).toBeInstanceOf(Error);
                expect((err as Error).message).toContain('multi-update failed');
                done();
            },
        });
    });
});

