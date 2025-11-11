(global as any).window = { TextCodesCache: [], TextCodes: [] };

jest.mock('../../EntityPMs/CorrespondencePM', () => ({
    CorrespondencePM: jest.fn().mockImplementation(() => ({
        Subject: '',
        EntityPMType: 'Correspondence',
        IsDirty: false,
        UIProperties: {},
        PropertyChanged: {},
    })),
}));

jest.mock('../../../Infrastructure/Components/LogitudeComponents/UIProperties', () => ({
    UIProperties: jest.fn().mockImplementation(() => ({})),
}));

jest.mock('../../../Infrastructure/Validators/ClassLevelValidator', () => ({
    ClassLevelValidator: jest.fn().mockImplementation(() => ({
        Validate: jest.fn().mockReturnValue([]),
    })),
}));

import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { InsertCorrespondenceService } from '../InsertCorrespondenceService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

const { ClassLevelValidator } = jest.requireMock('../../../Infrastructure/Validators/ClassLevelValidator');

describe('InsertCorrespondenceService', () => {
    const baseUrl = 'https://crm/';
    let httpClient: any;

    beforeEach(() => {
        httpClient = { post: jest.fn() };
        ServiceHelper.HttpClient = httpClient;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
        (global as any).window.TextCodesCache = [];
        ClassLevelValidator.mockImplementation(() => ({ Validate: jest.fn().mockReturnValue([]) }));
    });

    afterEach(() => jest.restoreAllMocks());

    it('insert posts mapped correspondence and returns response', done => {
        httpClient.post.mockReturnValue(of({ Id: 'CORR-1', Subject: 'Subject' }));
        const service = new InsertCorrespondenceService();
        const pm = {
            Subject: 'Subject',
            EntityPMType: 'Correspondence',
            IsDirty: false,
        } as any;

        service.insert(pm).subscribe(res => {
            expect(httpClient.post).toHaveBeenCalledTimes(1);
            const [url, body, options] = httpClient.post.mock.calls[0];
            expect(url).toBe(`${baseUrl}api/InsertCorrespondence`);
            const parsedBody = JSON.parse(body as string);
            expect(parsedBody.Subject).toBe('Subject');
            expect(options).toEqual({ headers: { Authorization: 'token' } });
            expect(res.Result.Subject).toBe('Subject');
            done();
        });
    });

    it('returns validation errors when validator fails', done => {
        ClassLevelValidator.mockImplementation(() => ({ Validate: jest.fn().mockReturnValue(['Subject required']) }));
        const service = new InsertCorrespondenceService();
        const pm = {
            Subject: '',
            EntityPMType: 'Correspondence',
            IsDirty: false,
        } as any;

        service.insert(pm).subscribe(res => {
            expect(httpClient.post).not.toHaveBeenCalled();
            expect(res.HasError).toBe(true);
            expect(res.ErrorsArray).toEqual(['Subject required']);
            done();
        });
    });
});

