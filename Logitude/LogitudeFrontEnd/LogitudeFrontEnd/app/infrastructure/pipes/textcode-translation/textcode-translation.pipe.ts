import {Pipe} from 'angular2/core';
import {TextCodeTranslator} from '../../utilities/TextCodeTranslator';

@Pipe({ name: 'textcodeTranslation' })

export class TextcodeTranslationPipe {
    transform(value: string): string {
        var translation: string = "";
        translation = TextCodeTranslator.transform(value);
        
        return translation;
    }
}