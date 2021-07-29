using System.Collections.Generic;
using System.Linq;
using Todo.App.Models;
using Todo.App.Services;

namespace Todo.ContractTests.Repository
{
  public class TodoServiceFakeForContract : ITodoService
  {
    private static readonly TodoServiceFakeForContract _instance = new TodoServiceFakeForContract();
    private List<TodoItem> _todos = new List<TodoItem>();

    public TodoServiceFakeForContract()
    { 
        
    }

    public static TodoServiceFakeForContract GetInstance()
    {
      return _instance;
    }
    public void CreateTodoItem(TodoItem todo)
    {
      _todos.Add(todo);
    }

    public void DeleteTodoItem(long id)
    {
      var todo = GetTodoItem(id);
      _todos.Remove(todo);
    }

    public TodoItem GetTodoItem(long id)
    {
      return _todos.Where(todo => todo.Id == id).FirstOrDefault();
    }

    public IEnumerable<TodoItem> GetTodoItems(bool includeCompleted = true)
    {
      return _todos.ToList();
    }

    public bool Save()
    {
     return true;
    }

    public bool TodoItemExists(long id)
    {
      return _todos.Any(todo => todo.Id == id);
    }

    public void UpdateTodoItem(long id, TodoItem todo)
    {
      var existingTodo = GetTodoItem(id);
      existingTodo.Text = todo.Text;
      existingTodo.Completed = todo.Completed;
    }
  }
}