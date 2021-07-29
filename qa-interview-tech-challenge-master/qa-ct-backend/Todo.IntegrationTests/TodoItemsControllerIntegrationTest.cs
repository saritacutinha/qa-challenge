using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Todo.App;
using Xunit;

namespace Todo.IntegrationTests
{
    public class TodoItemsControllerIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public TodoItemsControllerIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public void  CanGetTodoItems()
        {
           
        }
    }
}
