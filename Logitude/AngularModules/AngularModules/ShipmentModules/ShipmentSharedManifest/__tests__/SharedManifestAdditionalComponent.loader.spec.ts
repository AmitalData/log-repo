import { expect } from '@jest/globals';
import { buildComponent } from './helpers/component-builder';
import { ManifestSL } from '../../../Common/DataContracts/ManifestSL';

const flushPromises = () => new Promise(resolve => setTimeout(resolve, 0));

describe('SharedManifestAdditionalComponent (Loader)', () => {
    describe('LoadChildComponent', () => {
        it('invokes dynamic loader for additional and header screens', async () => {
            const { component, sessionMocks } = buildComponent({
                locations: [
                    { Code: 'GECO', viewContainerRef: {} },
                    { Code: 'SHCO', viewContainerRef: {} }
                ],
                agentSharedManifestList: []
            });

            component.ManifestSL = {} as ManifestSL;
            component.HouseEntity = null;
            component.IsLoadAdditionalScreen = true;
            component.IsLoadSharedManifestheaderScreen = false;

            component.LoadChildComponent();
            await flushPromises();

            expect(sessionMocks.dynamicLoader.Load).toHaveBeenCalledTimes(2);
            expect(sessionMocks.dynamicLoader.Load).toHaveBeenNthCalledWith(
                1,
                './Infrastructure/GenericComponents/GeneratedComponent',
                expect.any(Object)
            );
            expect(sessionMocks.dynamicLoader.Load).toHaveBeenNthCalledWith(
                2,
                './ShipmentModules/ShipmentSharedManifest/Components/SharedManifestHeaderComponent',
                expect.any(Object)
            );

            const generatedComponentRef = await sessionMocks.dynamicLoader.Load.mock.results[0].value;
            expect(generatedComponentRef.instance.Run).toHaveBeenCalledWith(
                component.EntityPM,
                'Shipment',
                'SharedManifestAdditionalScreen'
            );

            const headerComponentRef = await sessionMocks.dynamicLoader.Load.mock.results[1].value;
            expect(headerComponentRef.instance.Run).toHaveBeenCalledWith(
                component.CurrentEntity,
                component.AgentSharedManifestList
            );

            expect(component.IsLoadAdditionalScreen).toBe(false);
            expect(component.IsLoadSharedManifestheaderScreen).toBe(true);
        });
    });
});

