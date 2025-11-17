import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { TicketCorrespondencesService } from '../TicketCorrespondencesService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

describe('TicketCorrespondencesService', () => {
    const baseUrl = 'https://crm/';
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

    it('GetCorrespondencesList fetches correspondence list', done => {
        httpClient.get.mockReturnValue(of({ list: [] }));
        const service = new TicketCorrespondencesService();

        service.GetCorrespondencesList('TICKET').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/TicketCorrespondences/GetTicketCorrespondences?entityId=TICKET`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toEqual({ list: [] });
            done();
        });
    });
});

