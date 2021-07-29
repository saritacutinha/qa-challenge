describe('List items', () => {
  beforeEach(() => {
    cy.seedAndVisit()
  });
  it('displays completed items', () => {
    cy.get('.todo-list li')
      .filter('.completed')
      .should('have.length', 1)
      .and('contain', 'walk')
      .find('.toggle')
      .should('be.checked')
  })

  it('Shows remaining todos in the footer', () => {
    cy.get('.todo-count')
      .should('contain', 3)
  })

  it('Removes a todo', () => {
    cy.route({
      url: 'api/TodoItems/1',
      method: 'DELETE',
      status: 200,
      response: {}
    })
    cy.get('.todo-list li')
      .as('list')

    cy.get('@list')
      .first()
      .find('.destroy')
      .invoke('show')
      .click()

    cy.get('@list')
      .should('have.length', 3)
      .and('not.contain', 'dog')

  })
  it('can check off an item as completed', () => {
    cy.fixture('todos')
      .then(todos => {
        const target = Cypress._.head(todos)
        cy.route(
          'PUT',
          `/api/TodoItems/${target.id}`,
          Cypress._.merge(target, {
            isComplete: true
          })
        )
      })

    cy.get('.todo-list li')
      .first()
      .as('first-todo')

    cy.get('@first-todo')
      .find('.toggle')
      .click()
      .should('be.checked')

    cy.get('@first-todo')
      .should('have.class', 'completed')

    cy.get('.todo-count')
      .should('contain', 2)
  })

})