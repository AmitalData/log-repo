import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { TicketCorrespondencesService } from '../TicketCorrespondencesService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

describe('TicketCorrespondencesService', () => {
    const baseUrl = 'https://crm/';
    let httpClient: { get: jest.Mock };

    beforeEach(() => {
        httpClient = {
            get: jest.fn()
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('GetCorrespondencesList calls ticket correspondences endpoint', () => {
        const response = [{ id: '1' }];
        httpClient.get.mockReturnValue(of(response));

        const service = new TicketCorrespondencesService();
        service.GetCorrespondencesList('TICKET-1').subscribe(result => {
            expect(result).toEqual(response);
        });

        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/TicketCorrespondences/GetTicketCorrespondences?entityId=TICKET-1`,
            { headers: { Authorization: 'token' } }
        );
    });
});

