using Microsoft.EntityFrameworkCore;
using Todo.App.Models;
using Todo.App.Repository;

namespace Todo.UnitTests
{
public class TestDataContextFactory
{
    private readonly string _dbName;
    public TestDataContextFactory(string dbName)
    {
       _dbName = dbName;
      var dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(_dbName)
              .Options;
       
      using (var dbContext = new TodoContext(dbContextOptions))
        {
            dbContext.Database.EnsureCreated();
        }
      _options = dbContextOptions;

    }
    private readonly DbContextOptions<TodoContext> _options;
    public TodoContext Create() => new TodoContext(_options);
 
    }
}