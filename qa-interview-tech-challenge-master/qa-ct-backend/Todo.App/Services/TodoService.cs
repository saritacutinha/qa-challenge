using System;
using System.Collections.Generic;
using System.Linq;
using Todo.App.Models;
using Todo.App.Repository;

namespace Todo.App.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public void CreateTodoItem(TodoItem todo)
        {
            _repository.CreateTodoItem(todo);
        }

        public void DeleteTodoItem(long id)
        {
            _repository.DeleteTodoItem(id);
        }

        public TodoItem GetTodoItem(long id)
        {
            return _repository.GetTodoItem(id);
        }

        public IEnumerable<TodoItem> GetTodoItems(bool includeCompleted = true)
        {
            return _repository.GetTodoItems().Distinct(new TodoComparer())
                .Where(
                    t => t.DateAdded >= DateTime.Now.AddYears(-1) 
                    && (includeCompleted == true || !t.Completed)
                );
        }

        public bool Save()
        {
            return _repository.Save();
        }

        public bool TodoItemExists(long id) 
        {
            return _repository.TodoItemExists(id);
        }

        public void UpdateTodoItem(long id, TodoItem todo)
        {
            _repository.UpdateTodoItem(id, todo);
        }
    }
}