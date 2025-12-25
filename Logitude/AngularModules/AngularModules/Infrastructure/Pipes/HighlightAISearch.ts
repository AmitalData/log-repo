import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
    name: 'highlightAISearch',
    pure: true
})
export class HighlightAISearch implements PipeTransform {
    private lastSearch = '';
    private lastRegex!: RegExp;
    constructor(private sanitizer: DomSanitizer) {}
   
    transform(value: any, search: string, enabled = true,isNumeric = false ): SafeHtml | string {
        if (!enabled || !value || !search) {
          return value;
        }
        value = value.toString();

        if (isNumeric) {
          const valueNum = parseFloat(value.replace(/,/g, ''));
          const searchNum = parseFloat(search.replace(/,/g, ''));
      
          if (!isNaN(valueNum) && !isNaN(searchNum)) {
            const valueStr = valueNum.toString();
            const searchStr = searchNum.toString();
          
            if (searchStr.includes(valueStr) || valueStr.includes(searchStr)) {
              const result = `<span class="search-highlight">${value}</span>`;
              return this.sanitizer.bypassSecurityTrustHtml(result);
            }
          }
          
        }
        value = value.toString();        
        if (search !== this.lastSearch) {
          const escaped = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
          this.lastRegex = new RegExp(escaped, 'gi');
          this.lastSearch = search;
        }
    
        if (!this.lastRegex.test(value)) {
          return value;
        }

        const result = value.replace(
          this.lastRegex,
          match =>`<span class="search-highlight">${match}</span>`
        );
    
        return this.sanitizer.bypassSecurityTrustHtml(result);
      }
    

    
}
