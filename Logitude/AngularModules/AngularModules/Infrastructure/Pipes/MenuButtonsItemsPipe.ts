import {Pipe} from '@angular/core';
import {MenuButtonPM} from '../EntityPMs/MenuButtonPM'

@Pipe({ name: 'MenuButtonsItemsPipe' })

export class MenuButtonsItemsPipe {

    transform(menuItems: MenuButtonPM[], menuButtonParentId: string): MenuButtonPM[] {

        if (menuItems && menuButtonParentId) {
            var myResult = menuItems.filter(d=> (d.MenuButtonType == 'menuitem' || d.MenuButtonType=='separator') && d.ParentMenuButtonId == menuButtonParentId);
            myResult = myResult.sort((a, b) => {
                if (a.Index > b.Index) {
                    return 1;
                }
                else if (b.Index > a.Index) {
                    return -1;
                }

                return 0

            });
            return myResult;
        }
        return null;
    }
}
