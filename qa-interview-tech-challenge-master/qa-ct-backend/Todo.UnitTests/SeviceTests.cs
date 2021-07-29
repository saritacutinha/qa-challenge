using System;
using System.Linq;
using Todo.App.Models;
using Todo.App.Repository;
using Todo.App.Services;
using Xunit;

namespace Todo.UnitTests {
    public class ServicesTest {
        public ServicesTest() {
            factory = new TestDataContextFactory("TodoListTestService");
        }
        private TestDataContextFactory factory;
        [Fact]
        public void ServiceCanReadData() {
            using(var dbContext = factory.Create()) {
                dbContext.TodoItems.Add(new TodoItem() {
                        Id = 10,
                        Text = "Test all the things",
                        Completed = false,
                        DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
                });
                dbContext.SaveChanges();
            }

            using(var dbContext = factory.Create())

            {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);

                Assert.True(todoService.GetTodoItems().Any());
            }
        }

        [Fact]
        public void CreateTodoItem_addsItem() {
            var todoItem = new TodoItem() {
                    Id = 15,
                    Text = "Keep feeding the cat",
                    Completed = false,
                    DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
            };
            using(var dbContext = factory.Create()) {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);
                todoService.CreateTodoItem(todoItem);
                todoService.Save();
            }
            using(var dbContext = factory.Create()) {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);
                Assert.NotNull(todoService.GetTodoItem(todoItem.Id));
            }

        }

        [Fact]
        public void GetTodoItems_returnsAllItems() {
            using(var dbContext = factory.Create()) {
                dbContext.TodoItems.Add(new TodoItem() {
                        Id = 16,
                        Text = "Grocery shopping",
                        Completed = false,
                        DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
                });
                dbContext.SaveChanges();

                dbContext.TodoItems.Add(new TodoItem() {
                        Id = 17,
                        Text = "GROCERY SHOPPING",
                        Completed = false,
                        DateAdded = DateTime.Now
                });
                dbContext.SaveChanges();
            }
            using(var dbContext = factory.Create())

            {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);
                var result = todoService.GetTodoItems();
                Assert.True(result.Count() == 1 );

            }

        }
    


        [Fact]
        public void DeleteTodoItem() {
            using(var dbContext = factory.Create()) {
                dbContext.TodoItems.Add(new TodoItem() {
                        Id = 19,
                        Text = "WEEKLY MEETING",
                        Completed = false,
                        DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
                });
                dbContext.SaveChanges();

                dbContext.TodoItems.Add(new TodoItem() {
                        Id = 20,
                        Text = "Weekly meeting",
                        Completed = false,
                        DateAdded = DateTime.Now
                });
                dbContext.SaveChanges();
            }
            using(var dbContext = factory.Create())

            {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);
                todoService.DeleteTodoItem(19);
                todoService.Save();
                
            }
            using(var dbContext = factory.Create()) {
                var todoRepository = new TodoRepository(dbContext);
                var todoService = new TodoService(todoRepository);
                Assert.Null(todoService.GetTodoItem(19));
            }

        }
    }
}