describe('ToDo App', () => {
   beforeEach(() => {
     cy.seedAndVisit([])
   })

   it('Focus input on load', () => {
     cy.focused()
       .should('have.class', 'new-todo')
   })
   it('adds input', () => {
     const newTodo = "Walk the dog"
     cy.get('.new-todo')
       .type(newTodo)
       .should('have.value', newTodo);
   })

   context('Form Submission', () => {
     beforeEach(() => {
       cy.server()
     });
     it('can add a new todo', () => {
       const newTodo = 'Feed the dog'

       cy.route('POST', 'api/TodoItems', {
         id: 16,
         text: newTodo,
         completed: false
       });

       cy.get('.new-todo')
         .type(newTodo)
         .type('{enter}')
         .should('have.value', '');

       cy.get('.todo-list li')
         .should('have.length', 1)
         .and('contain', newTodo)

     });

     it('Shows error on failed add', () => { //Fails as no error validation is done on UI on failed adds
       cy.route({
         url: 'api/TodoItems/',
         method: 'POST',
         status: 500,
         response: {}
       });
       cy.get('.new-todo')
         .type('test{enter}');
       cy.get('.todo-list li')
         .should('not.exist')
       cy.get('.error')
         .should('be.visible');
     });
   });
 });