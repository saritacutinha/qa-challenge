using System.Collections.Generic;
using Todo.App.Models;

namespace Todo.App.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetTodoItems(bool includeCompleted);
        TodoItem GetTodoItem(long id);
        void CreateTodoItem(TodoItem todo);
        void UpdateTodoItem(long id, TodoItem todo);
        void DeleteTodoItem(long id);
        bool TodoItemExists(long id);
        bool Save();

    }
}