describe('Test1 Cypres Rabaia', () => { 

        it('Login to test Env', function () {
            
        cy.visit('https://test.logitudeworld.com/test');
        cy.get('#Email').type('ahmada@logitudeworld.com').should('have.value','ahmada@logitudeworld.com');
        cy.get('#Password').type('!A123456');
        cy.get('#cmdLogin').click();
        cy.get('.k-input').type('Angular (Business Package) (951) (951)').should('have.value','Angular (Business Package) (951) (951)');
        cy.get('#cmdContinue').click();
        
        cy.server(); 
        cy.route('**/ObjectTableLastUpdate/**').as('LoginLoadDataCompleted');
        cy.window().then(win => { win.sessionStorage.setItem('ControlledByCypress', 'true') });

        //cy.window().then(win => { win.sessionStorage.setItem('ControlledByCypress', 'true') });

        cy.wait('@LoginLoadDataCompleted'); 

        });

        it('navigate to charge types', function () {
        
            cy.get('#GeneralMHMaintenance').click();
            cy.get('#BIL').click();
            cy.get('#MaintenanceItemMTCT').click(); 
        });

        it('add new charge types', function () {
            
            const Code = "";
            cy.get('#NewButton_ChargesType').click().then(()=>{
                this.Code = userID_Alpha();
            
            cy.get('#ChargesType_Code').type(this.Code).should('have.value',this.Code);
            cy.get('#ChargesType_EnglishName').type(this.Code).should('have.value',this.Code);
            cy.get('#ChargesType_MeasurementId').focus().type('{downarrow}');
            cy.get('#mydatalist_ChargesType_MeasurementId').children().eq(0).click();
            cy.get('#ChargesType_ChargesGroupId').focus().type('{downarrow}');
            cy.get('#mydatalist_ChargesType_ChargesGroupId').children().eq(0).click();

            cy.get('div.MediaFillRelative div.MediaFillRelative div.MediaFill div.MediaFillRelative div.SessionsTabControl div.TabControlBody div.MediaFillAbsolute div.MediaFill div.MediaFillAbsolute div.MediaFillAbsolute div.MediaFillRelative div.LogitudeWindow div.MediaFill div.MediaFill td:nth-child(1) table:nth-child(1) tr:nth-child(1) td:nth-child(4) > button.Button')
            .click();
            //cy.get('div.MediaFillRelative div.MediaFillRelative div.MediaFill div.MediaFillRelative div.SessionsTabControl div.TabControlBody div.MediaFillAbsolute div.MediaFill div.MediaFillAbsolute div.MediaFillAbsolute div.MediaFillRelative div.LogitudeWindow div.MediaFill div.MediaFill td:nth-child(1) table:nth-child(1) tr:nth-child(1) td:nth-child(4) > button.Button')
            //.click();
            cy.get('div.MediaFillRelative div.MediaFillRelative div.MediaFill div.MediaFillRelative div.SessionsTabControl div.TabControlBody div.MediaFillAbsolute div.MediaFill div.MediaFillAbsolute div.MediaFillAbsolute div.MediaFillRelative div.LogitudeWindow div.MediaFill div.MediaFill td:nth-child(1) table:nth-child(1) tr:nth-child(1) td:nth-child(6) > button.RedButton')
            .click();
            cy.get('.LogitudeWindow').should('not.exist');

        });
        
            function userID_Alpha() {
                var text = "";
                var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            
                for (var i = 0; i < 3; i++)
                  text += possible.charAt(Math.floor(Math.random() * possible.length));
            
                return text;
              }
        });
  });