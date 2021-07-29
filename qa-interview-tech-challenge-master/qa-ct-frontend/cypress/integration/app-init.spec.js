describe('App intialisation', () => {
    it('Loads todos on page initialization', () => {

        //Arrange
        cy.seedAndVisit()

        // Act and Assertions
        cy.get('.todo-list li')
            .should('have.length', 4)
    });


    it('Displays an error if the service call responds with an error', () => {
        cy.server()
        cy.route({
            url: 'api/TodoItems/',
            method: 'GET',
            status: 500,
            response: {}
        })
        cy.visit('/')

        //Assertions to check error validations on UI -- Fails 
        cy.get('.todo-list li')
            .should('not.exist')
        cy.get('.error')
            .should('be.visible');

    })
})