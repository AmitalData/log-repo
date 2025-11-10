import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { ModulesService } from '../ModulesService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

describe('Accounting ModulesService', () => {
    const baseUrl = 'https://logitude/';
    let httpClient: { get: jest.Mock };

    beforeEach(() => {
        httpClient = {
            get: jest.fn()
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('GetAccountPayablesSummary calls invoice domain endpoint', () => {
        httpClient.get.mockReturnValue(of({ summary: [] }));

        const service = new ModulesService();
        service.GetAccountPayablesSummary().subscribe(result => {
            expect(result).toEqual({ summary: [] });
        });

        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/InvoiceDomain/GetAccountPayablesSummary`,
            { headers: { Authorization: 'token' } }
        );
    });

    it('GetAccountingReceivablesSummary calls invoice domain endpoint', () => {
        httpClient.get.mockReturnValue(of({ receivables: [] }));

        const service = new ModulesService();
        service.GetAccountingReceivablesSummary().subscribe(result => {
            expect(result).toEqual({ receivables: [] });
        });

        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/InvoiceDomain/GetAccountingReceivablesSummary`,
            { headers: { Authorization: 'token' } }
        );
    });
});

