import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { ModulesService } from '../ModulesService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

describe('ModulesService', () => {
    const baseUrl = 'https://accounting/';
    let httpClient: any;

    beforeEach(() => {
        httpClient = { get: jest.fn() };
        ServiceHelper.HttpClient = httpClient;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
    });

    afterEach(() => jest.restoreAllMocks());

    it('GetAccountPayablesSummary hits invoice domain endpoint', done => {
        httpClient.get.mockReturnValue(of({ summary: 'ap' }));
        const service = new ModulesService();

        service.GetAccountPayablesSummary().subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/InvoiceDomain/GetAccountPayablesSummary`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toEqual({ summary: 'ap' });
            done();
        });
    });

    it('GetAccountingReceivablesSummary hits invoice domain endpoint', done => {
        httpClient.get.mockReturnValue(of({ summary: 'ar' }));
        const service = new ModulesService();

        service.GetAccountingReceivablesSummary().subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/InvoiceDomain/GetAccountingReceivablesSummary`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toEqual({ summary: 'ar' });
            done();
        });
    });
});

