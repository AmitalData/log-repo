import { Selectors } from "../selectors/Selectors"

export function FillChangePasswordPage(CurrentPassword:string,password: string , confirmPassword:string) {
    cy.FillLogTextBox(Selectors.CurrentPassword ,CurrentPassword)
    cy.FillLogTextBox(Selectors.Password , password)
    if(confirmPassword!=null){
        cy.FillLogTextBox(Selectors.ConfirmPassword ,confirmPassword)
    }
    
}

