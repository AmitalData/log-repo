import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';
import { createAddressListServiceMock, createCardListServiceMock } from './helpers/service-stubs';

describe('SharedManifestAdditionalComponent (Agent)', () => {
    describe('AgentId setter', () => {
        it('populates agent details when value provided', () => {
            const agentCard = {
                Code: 'AGENT-1',
                PrimaryContactId: 'AG-CONTACT',
                EnglishName: 'Agent Name',
                Notes: 'Agent notes',
                MainAddressId: 'ADDR-AG'
            } as any;

            const agentAddress = { Id: 'ADDR-AG', CityName: 'Tel Aviv' } as any;

            const { component, serviceMocks } = buildComponent({
                cardListServiceMock: createCardListServiceMock(agentCard),
                addressListServiceMock: createAddressListServiceMock(agentAddress)
            });

            component.AgentId = 'AGENT-1';

            expect(serviceMocks.cardListService.spies.getSingle).toHaveBeenCalledWith('AGENT-1');
            expect(serviceMocks.addressListService.spies.getSingle).toHaveBeenCalledWith('ADDR-AG');
            expect(component.AgentContactId).toBe('AG-CONTACT');
            expect(component.EntityPM.AgentName).toBe('Agent Name');
            expect(component.EntityPM.AgentNote).toBe('Agent notes');
            expect(component.AgentAddressList).toEqual(agentAddress);
        });

        it('clears agent details when id becomes empty', () => {
            const { component } = buildComponent();

            component.EntityPM.AgentContactId = 'AG-CONTACT';
            component.EntityPM.AgentName = 'Agent Name';
            component.EntityPM.AgentNote = 'Note';
            component.EntityPM.AgentReference1 = 'REF1';
            component.EntityPM.AgentReference2 = 'REF2';
            component.EntityPM.AgentAddressId = 'ADDR-1';
            component.AgentAddressList = {} as any;
            component.EntityPM.AgentId = 'AGENT-KEEP';

            component.AgentId = null;

            expect(component.AgentContactId).toBeNull();
            expect(component.EntityPM.AgentName).toBeNull();
            expect(component.EntityPM.AgentNote).toBeNull();
            expect(component.EntityPM.AgentReference1).toBeNull();
            expect(component.EntityPM.AgentReference2).toBeNull();
            expect(component.AgentAddressList).toBeNull();
        });
    });
});

