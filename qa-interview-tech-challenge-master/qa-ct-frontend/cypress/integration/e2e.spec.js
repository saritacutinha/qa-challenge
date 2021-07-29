describe('e2e tests', () => {
  beforeEach(() => {
    cy.request('GET', 'https://localhost:5001/api/TodoItems')
  })

  context('With seeded todos', () => {
    it('Saves new todos', () => {
      //Arrange
      const items = [{
          text: 'Book flights',
          expectedLength: 4
        },
        {
          text: 'Pack bag',
          expectedLength: 5
        },
        {
          text: 'Enjoy holiday',
          expectedLength: 6
        }
      ]

      //Act
      cy.visit('/')
      cy.server()
      cy.route('POST', 'https://localhost:5001/api/TodoItems')
        .as('create')

      cy.wrap(items)
        .each(todo => {
          cy.focused()
            .type(todo.text)
            .type('{enter}')

          cy.wait('@create')

          //Assert
          cy.get('.todo-list li')
            .should('have.length', todo.expectedLength)
        })
    })
  })

  context('With active todos', () => {
    beforeEach(() => {
      cy.fixture('todos')
        .each(todo => {
          const newTodo = Cypress._.merge(todo, {
            completed: false
          })
          cy.request('POST', 'https://localhost:5001/api/TodoItems', newTodo)
        })
      cy.visit('/')
    })

    it('Loads existing data from the DB', () => {
      cy.get('.todo-list li')
        .should('have.length', 9)
    })

    it('Deletes todos', () => {
      cy.server()
      cy.route('DELETE', 'https://localhost:5001/api/TodoItems/*')
        .as('delete')

      cy.get('.todo-list li')
        .each($el => {
          cy.wrap($el)
            .find('.destroy')
            .invoke('show')
            .click()

          cy.wait('@delete')
        })
        .should('not.exist')
    })

    it('Toggles todos', () => {
      const clickAndWait = ($el) => {
        cy.wrap($el)
          .as('item')
          .find('.toggle')
          .click()

        cy.wait('@update')
      }
      cy.server()
      cy.route('PUT', 'https://localhost:5001/api/TodoItems/*')
        .as('update')

      cy.get('.todo-list li')
        .each($el => {
          clickAndWait($el)
          cy.get('@item')

            .should('have.class', 'completed')
        })
        .each($el => {
          clickAndWait($el)
          cy.get('@item')
            .should('not.have.class', 'completed')
        })
    })
  })
})