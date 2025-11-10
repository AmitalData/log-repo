import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { CourierMasterService } from '../CourierMasterService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

describe('CourierMasterService', () => {
    const baseUrl = 'https://customs/';
    let httpClient: { get: jest.Mock; post: jest.Mock };

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn()
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
        jest.spyOn(ServiceHelper, 'CheckIsLock').mockReturnValue(of(new ServiceResponse()) as any);
        jest.spyOn(ServiceHelper, 'GetHttpHeadersGeneralLock').mockReturnValue({ headers: { Authorization: 'token' } } as any);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('GetIfCourierMasterExists builds expected query string', () => {
        httpClient.get.mockReturnValue(of({}));

        const service = new CourierMasterService();
        service.GetIfCourierMasterExists('CM1', 'AIR', '123', '987').subscribe();

        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/CourierMaster/GetIfCourierMasterExists?Id=CM1&airlineId=AIR&HAWB=123&MAWB=987`,
            { headers: { Authorization: 'token' } }
        );
    });

    it('getByFilters maps declarations to DeclarationPM instances', done => {
        const rawDeclarations = {
            Result: [
                { Id: 'DECL-1', CustomerName: 'ACME' },
                { Id: 'DECL-2', CustomerName: 'Globex' }
            ]
        };
        httpClient.get.mockReturnValue(of(rawDeclarations));

        const service = new CourierMasterService();
        const filters: any = {
            CustomerName: 'ACME',
            AdditionalFilters: []
        };

        service.getByFilters(filters).subscribe(res => {
            const calledUrl = httpClient.get.mock.calls[0][0] as string;
            expect(calledUrl).toContain('GetCourierConnectedDeclarations');
            expect(calledUrl).toContain('CustomerName=ACME');
            const calledHeaders = httpClient.get.mock.calls[0][1];
            expect(calledHeaders).toEqual({ headers: { Authorization: 'token' } });
            expect(res.Result).toHaveLength(2);
            expect(res.Result[0].Id).toBe('DECL-1');
            expect(res.Result[0].CustomerName).toBe('ACME');
            expect(res.Result[1].CustomerName).toBe('Globex');
            done();
        });
    });

    it('getByFilters appends additional filters to query string', () => {
        httpClient.get.mockReturnValue(of({ Result: [] }));

        const service = new CourierMasterService();
        const filters: any = {
            CustomerName: 'ACME',
            AdditionalFilters: [
                {
                    FieldName: 'Status',
                    FieldValue: 'Open'
                }
            ]
        };

        service.getByFilters(filters).subscribe(res => {
            expect(res.Result).toHaveLength(0);
        });

        const requestedUrl: string = httpClient.get.mock.calls[0][0];
        expect(requestedUrl).toContain('CustomerName=ACME');
        expect(requestedUrl).toContain('&AdditionalFilters=[');
    });
});

