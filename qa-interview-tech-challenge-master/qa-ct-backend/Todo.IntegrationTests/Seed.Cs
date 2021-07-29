using System;
using Todo.App.Models;

namespace Todo.IntegrationTests
{
    public static class Seed
    {
        public static void PopulateTestData(TodoContext dbContext)
        {
            dbContext.TodoItems.Add(new TodoItem()
            {
                Id = 10,
                Text = "Test all the things",
                Completed = false,
                DateAdded = DateTime.Parse("2021-05-11T18:46:37.727957+10:00")
            });
            dbContext.SaveChanges();
        }
    }
}
