import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { GLAccountSecurityLevelService } from '../GLAccountSecurityLevelService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

const settingsGetSingle = jest.fn();
const loginGetLoggedUser = jest.fn();
const accountGetSingle = jest.fn();
const showEditComponentMock = jest.fn();
const messageShowMock = jest.fn();

jest.mock('Accounting/Services/StandardLists/FullAccountingSettingListService', () => ({
    FullAccountingSettingListService: jest.fn().mockImplementation(() => ({
        getSingle: settingsGetSingle,
    })),
}));

jest.mock('Accounting/Services/StandardLists/GLAccountListService', () => ({
    GLAccountListService: jest.fn().mockImplementation(() => ({
        getSingle: accountGetSingle,
    })),
}));

jest.mock('Infrastructure/Services/LoginService', () => ({
    LoginService: jest.fn().mockImplementation(() => ({
        GetLoggedUser: loginGetLoggedUser,
    })),
}));

jest.mock('Controls/Windows/LogitudeWindow', () => ({
    LogitudeWindow: jest.fn().mockImplementation(() => ({
        ShowHeaderButtons: false,
        Height: 0,
        Width: 0,
        ShowEditComponent: showEditComponentMock,
    })),
}));

jest.mock('Controls/Windows/MessageWindow', () => ({
    MessageWindow: jest.fn().mockImplementation(() => ({
        Width: 0,
        Height: 0,
        Title: '',
        Show: messageShowMock,
        InjectWindowComponent: jest.fn(),
    })),
}));

jest.mock('Infrastructure/Utilities/TextCodeTranslator', () => ({
    TextCodeTranslator: {
        Translate: (value: string) => value,
    },
}));

describe('GLAccountSecurityLevelService', () => {
    beforeEach(() => {
        SessionLocator.Tenant = 77;
        SessionInfo.LoggedUserEmail = 'user@example.com';
        settingsGetSingle.mockReturnValue(of({ Result: { IsSecurityLevelActivated: true } }));
        loginGetLoggedUser.mockReturnValue(of({ SecurityLevel: 3, IsCustomerCare: false }));
        accountGetSingle.mockReturnValue(of({ Result: { ChartOfAccountSecurityLevel: 2 } }));
        showEditComponentMock.mockClear();
        messageShowMock.mockClear();
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('resolves true when security level feature disabled', async () => {
        settingsGetSingle.mockReturnValue(of({ Result: { IsSecurityLevelActivated: false } }));

        const hasAccess = await GLAccountSecurityLevelService.CheckLevel('GL-1');

        expect(hasAccess).toBe(true);
        expect(loginGetLoggedUser).not.toHaveBeenCalled();
        expect(accountGetSingle).not.toHaveBeenCalled();
    });

    it('evaluates access using user security level', async () => {
        const hasAccess = await GLAccountSecurityLevelService.CheckLevel('GL-1');

        expect(settingsGetSingle).toHaveBeenCalledWith('77');
        expect(loginGetLoggedUser).toHaveBeenCalled();
        expect(accountGetSingle).toHaveBeenCalledWith('GL-1');
        expect(hasAccess).toBe(true);
    });

    it('denies access when GL account security level exceeds user level', async () => {
        accountGetSingle.mockReturnValue(of({ Result: { ChartOfAccountSecurityLevel: 5 } }));

        const hasAccess = await GLAccountSecurityLevelService.CheckLevel('GL-2');

        expect(hasAccess).toBe(false);
    });

    it('grants access for customer care users irrespective of account level', async () => {
        loginGetLoggedUser.mockReturnValue(of({ SecurityLevel: 1, IsCustomerCare: true }));
        accountGetSingle.mockReturnValue(of({ Result: { ChartOfAccountSecurityLevel: 9 } }));

        const hasAccess = await GLAccountSecurityLevelService.CheckLevel('GL-CARE');

        expect(hasAccess).toBe(true);
    });

    it('denies access when user security level missing', async () => {
        loginGetLoggedUser.mockReturnValue(of({ SecurityLevel: undefined, IsCustomerCare: false }));
        accountGetSingle.mockReturnValue(of({ Result: { ChartOfAccountSecurityLevel: 1 } }));

        const hasAccess = await GLAccountSecurityLevelService.CheckLevel('GL-UNSET');

        expect(hasAccess).toBe(false);
    });

    it('opens edit window when access granted', async () => {
        const checkSpy = jest.spyOn(GLAccountSecurityLevelService, 'CheckLevel').mockResolvedValue(true);

        GLAccountSecurityLevelService.OpenGLAccountEditWindow('GL-3', 'TAB');
        await Promise.resolve();

        expect(checkSpy).toHaveBeenCalledWith('GL-3');
        expect(showEditComponentMock).toHaveBeenCalledWith('GL-3', 'GLAccount', 'TAB');

        checkSpy.mockRestore();
    });

    it('shows blocking message when access denied', async () => {
        const checkSpy = jest.spyOn(GLAccountSecurityLevelService, 'CheckLevel').mockResolvedValue(false);

        GLAccountSecurityLevelService.OpenGLAccountEditWindow('GL-4');
        await Promise.resolve();

        expect(messageShowMock).toHaveBeenCalledWith('GLAccount.O.SecurityLevelHiddenItem');

        checkSpy.mockRestore();
    });
});

