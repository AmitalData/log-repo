export function SelectElement(selector:string) {
    if(selector.startsWith("#")){
        let selectorArr = selector.split('_')
        let selectorArrLastElement = selectorArr[selectorArr.length - 1]
        let isSelectorArrLastElementNumber = (/^\d+$/).test(selectorArrLastElement)
        if(!isSelectorArrLastElementNumber){
            return cy.get("[id^='" + selector.replace("#", "") + "']").last()
        }else{
            return cy.get("[id^='" + selector.replace("#", "").replace(("_" + selectorArrLastElement), "") + "']").last()
        }
    }else{
        return cy.get(selector)
    }
}