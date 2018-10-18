
import {SocialMainComponent} from './Components/SocialMainComponent';
import {SocialPostsComponent} from './Components/SocialPostsComponent';
import {SocialMessagesComponent} from './Components/SocialMessagesComponent';
import {EditPostComponent} from './Components/EditPostComponent';
import {SocialPeopleComponent} from './Components/SocialPeopleComponent';
import {SocialContactNameLink} from './Components/QueryColumnsComponents/SocialContactNameLink';
import {SocialPeopleFollowComponent} from './Components/QueryColumnsComponents/SocialPeopleFollowComponent';
import {NewSocialMessageComponent} from './Components/NewSocialMessageComponent';
import {SocialMessageParticipantsComponent} from './Components/SocialMessageParticipantsComponent';
import {AddSocialMessageParticipantsComponent} from './Components/AddSocialMessageParticipantsComponent';
import {ConversationMessageComponent} from './Components/ConversationMessageComponent';
import {SocialInboxMessageComponent} from './Components/SocialInboxMessageComponent';

export const ControlsComponents =
    [
        ConversationMessageComponent,

    ]

export const Components =
    [
        SocialMainComponent,
        SocialPostsComponent,
        SocialMessagesComponent,
        EditPostComponent,
        SocialPeopleComponent,
        SocialContactNameLink,
        SocialPeopleFollowComponent,
        NewSocialMessageComponent,
        SocialMessageParticipantsComponent,
        AddSocialMessageParticipantsComponent,
        ConversationMessageComponent,
        SocialInboxMessageComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "SocialMainComponent": { myResult = SocialMainComponent; break; }
            case "SocialPostsComponent": { myResult = SocialPostsComponent; break; }
            case "SocialMessagesComponent": { myResult = SocialMessagesComponent; break; }
            case "EditPostComponent": { myResult = EditPostComponent; break; }
            case "SocialPeopleComponent": { myResult = SocialPeopleComponent; break; }
            case "SocialContactNameLink": { myResult = SocialContactNameLink; break; }
            case "SocialPeopleFollowComponent": { myResult = SocialPeopleFollowComponent; break; }
            case "NewSocialMessageComponent": { myResult = NewSocialMessageComponent; break; }
            case "SocialMessageParticipantsComponent": { myResult = SocialMessageParticipantsComponent; break; }
            case "AddSocialMessageParticipantsComponent": { myResult = AddSocialMessageParticipantsComponent; break; }
            case "ConversationMessageComponent": { myResult = ConversationMessageComponent; break; }
            case "SocialInboxMessageComponent": { myResult = SocialInboxMessageComponent; break; }
                
        }

        return myResult;
    }
}