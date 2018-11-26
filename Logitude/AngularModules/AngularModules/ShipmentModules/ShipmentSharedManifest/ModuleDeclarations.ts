
import {SharedManifestComponent} from './Components/SharedManifestComponent';
import {SharedManifestStarted} from './Components/SharedManifestStarted';
import {SharedManifestAdditionalComponent} from './Components/SharedManifestAdditionalComponent';
import {SharedManifestsWorkSpaces} from './Components/SharedManifestsWorkSpaces';
import {SharedManifestHeaderComponent} from './Components/SharedManifestHeaderComponent';
import {SharedManifestEditAgentComponent} from './Components/SharedManifestEditAgentComponent';
export const Components =
    [
        SharedManifestComponent,
        SharedManifestStarted,
        SharedManifestAdditionalComponent,
        SharedManifestHeaderComponent,
        SharedManifestsWorkSpaces,
        SharedManifestEditAgentComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "SharedManifestComponent": { myResult = SharedManifestComponent; break }
            case "SharedManifestStarted": { myResult = SharedManifestStarted; break }
            case "SharedManifestAdditionalComponent": { myResult = SharedManifestAdditionalComponent; break }
            case "SharedManifestHeaderComponent": { myResult = SharedManifestHeaderComponent; break }
            case "SharedManifestsWorkSpaces": { myResult = SharedManifestsWorkSpaces; break }
            case "SharedManifestEditAgentComponent": { myResult = SharedManifestEditAgentComponent; break }
                
        }

        return myResult;
    }
}