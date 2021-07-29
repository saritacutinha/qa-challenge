import {
  InteractionObject,
  Matchers,
  Pact,
  PactOptions,
  Publisher,
} from '@pact-foundation/pact';
import {
  PublisherOptions
} from '@pact-foundation/pact-node';
import {
  like,
  regex,
  string
} from '@pact-foundation/pact/src/dsl/matchers';

import {
  ITodo
} from '../../api/type';
import {
  API
} from '../../api/api';

const opts: PublisherOptions = {
  pactBroker: 'https://scutinha.pactflow.io',
  pactBrokerToken: '-eWmD4COke9pkmmhkkZoXg',
  consumerVersion: '0.3.1',
  pactFilesOrDirs: ['./pacts'],
};

new Publisher(opts).publishPacts();

const pactOptions: PactOptions = {
  consumer: 'qa-ct-frontend',
  provider: 'qa-ct-backend',
};
const mockProvider: Pact = new Pact(pactOptions);

describe('Pact with TODO API', () => {
  beforeAll(() => mockProvider.setup());
  afterEach(() => mockProvider.verify());
  afterAll(() => mockProvider.finalize());

  const defaultTodo: ITodo = {
    id: 100,
    completed: false,
    text: 'A todo task 1',
  };
  const getMockTodo = (override ? : Partial < ITodo > ) => ({
    ...defaultTodo,
    ...override,
  });

  describe('given there are todos', () => {
    describe('when a GET /api/TodoItems call to the API is made', () => {
      it('todos exists', async () => {
        // Arrange
        const expectedTodos: ITodo[] = [
          getMockTodo(),
          getMockTodo({
            id: 101,
            completed: true,
            text: 'A todo task 2',
          }),
        ];

        const interactionObject: InteractionObject = {
          state: 'todos exists',
          uponReceiving: 'a request to get todos',
          withRequest: {
            method: 'GET',
            path: '/api/TodoItems',
          },
          willRespondWith: {
            status: 200,
            body: like(expectedTodos),
            headers: {
              'Content-Type': regex({
                generate: 'application/json; charset=utf-8',
                matcher: 'application/json;?.*',
              }),
            },
          },
        };

        await mockProvider.addInteraction(interactionObject);

        // Act
        const api = new API(`${mockProvider.mockService.baseUrl}/api`);
        const todos = await api.getTodos();

        // Assert
        expect(todos).toStrictEqual(expectedTodos);
      });
    });
  });

  describe('given the todo ID 102 exists', () => {

    describe('when a GET /api/TodoItems/102 call to the API is made', () => {
      it('todos ID 102 exists', async () => {
        // Arrange
        const expectedTodo: ITodo = getMockTodo({
          id: 102
        });

        const interactionObject: InteractionObject = {
          state: 'todo id=102 exists',
          uponReceiving: 'a request to get todo id=102',
          withRequest: {
            method: 'GET',
            path: '/api/TodoItems/102',
          },
          willRespondWith: {
            status: 200,
            body: like(expectedTodo),
            headers: {
              'Content-Type': regex({
                generate: 'application/json; charset=utf-8',
                matcher: 'application/json;?.*',
              }),
            },
          },
        };

        await mockProvider.addInteraction(interactionObject);

        // Act
        const api = new API(`${mockProvider.mockService.baseUrl}/api`);
        const todo = await api.getTodo(102);

        // Assert
        expect(todo).toStrictEqual(expectedTodo);

      });
    });
  });



  describe('create a new todo', () => {
    describe('when a POST /api/TodoItems call to the API is made', () => {
      it('todo item is added', async () => {
        // Arrange
        const PostToDoBody: ITodo = {
          id: 502,
          completed: false,
          text: 'A todo task 500',
        };

        const PostTodoExpected: ITodo = getMockTodo({
          id: 502,
          completed: PostToDoBody.completed,
          text: PostToDoBody.text,
        });
        
        const interactionObject: InteractionObject = {
          state: 'todo to be added',
          uponReceiving: 'request to add new todo',
          withRequest: {
            method: 'POST',
            path: '/api/TodoItems',
            body: like(PostToDoBody),
            headers: {
              'Content-Type': 'application/json;charset=utf-8',
              'Accept': 'application/json, text/plain, */*'
            }
          },
          willRespondWith: {
            status: 201,
            body: Matchers.like(PostTodoExpected),

            headers: {
              'Content-Type': regex({
                generate: 'application/json; charset=utf-8',
                matcher: 'application/json;?.*',
              }),
            },
          },
        };

        await mockProvider.addInteraction(interactionObject);

        // Act
        const api = new API(`${mockProvider.mockService.baseUrl}/api`);
        const todo = await api.addTodo(PostToDoBody);
        // Assert
        expect(todo.status).toBe(201);
      });
    });
  });

  describe('Todo', () => {
    describe('when a PUT /api/TodoItems/ call to the API is made', () => {
      it('existing todo is updated', async () => {
        // Arrange
        const updateToDoBody: ITodo = {
          id: 102,
          completed: false,
          text: 'A todo task update',
        };
        
        const interactionObject: InteractionObject = {
          state: 'todo Id 102 exists to update',
          uponReceiving: 'a request to update todo id=102',
          withRequest: {
            method: 'PUT',
            path: '/api/TodoItems/102',
            body: updateToDoBody,
            headers: {
              'Content-Type': 'application/json;charset=utf-8',
              'Accept': 'application/json, text/plain, */*'
            }
          },
          willRespondWith: {
            status: 204
          },
        };

        await mockProvider.addInteraction(interactionObject);

        // Act
        const api = new API(`${mockProvider.mockService.baseUrl}/api`);
        const todo = await api.updateTodo(updateToDoBody)
        // Assert
        expect(todo.status).toBe(204);
      });
    });
  });


});