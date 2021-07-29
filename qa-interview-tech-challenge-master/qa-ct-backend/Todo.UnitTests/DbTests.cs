using System;
using System.Linq;
using Todo.App.Models;
using Xunit;
using Todo.App.Repository;

namespace Todo.UnitTests {
  public class DbTests {
    public DbTests() {
      factory = new TestDataContextFactory("TodoListTest");
    }


    private TestDataContextFactory factory;


    [Fact]
    public void WhenDBContextIsNull_throwArgumentNullException() {
      Assert.Throws < ArgumentNullException > (() => new TodoRepository(null));
    }

    [Fact] 
    public void DataCanBeWrittenToTheDatabase() {
      using(var dbContext = factory.Create()) {
        dbContext.TodoItems.Add(new TodoItem() {
            Id = 15,
            Text = "Test all the things",
            Completed = false,
            DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
        });
        dbContext.SaveChanges();
      }

      using(var dbContext = factory.Create()) {
        Assert.True(dbContext.TodoItems.Any());
      }
    }


    [Fact]
    public void CanGetItemsFromDatabase() {
      using(var dbContext = factory.Create()) {
        dbContext.TodoItems.Add(new TodoItem() {
            Id = 11,
            Text = "Get things",
            Completed = false,
            DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
        });
        dbContext.SaveChanges();
      }

      using(var dbContext = factory.Create()) {
        var todoRepository = new TodoRepository(dbContext);
        var TodoList = todoRepository.GetTodoItems();
        Assert.NotNull(TodoList);

      }
    }

    [Fact]
    public void CanGetItemFromDatabase() {
      using(var dbContext = factory.Create()) {
        dbContext.TodoItems.Add(new TodoItem() {
            Id = 12,
            Text = "Get one thing",
            Completed = false,
            DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
        });
        dbContext.SaveChanges();
      }

      using(var dbContext = factory.Create()) {
        var todoRepository = new TodoRepository(dbContext);
        var TodoList = todoRepository.GetTodoItem(12);
        Assert.NotNull(TodoList);
        Assert.Equal(12, TodoList.Id);
      }
    }
  }
}