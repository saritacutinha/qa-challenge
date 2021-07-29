using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Todo.App.Models;
using Todo.App.Services;

namespace Todo.ContractTests.Middleware
{
  public class ProviderStateMiddleware
  {
    private const string ConsumerName = "qa-ct-frontend";
    private readonly RequestDelegate _next;
    private readonly IDictionary<string, Action> _providerStates;
    private ITodoService _todoService;

    public ProviderStateMiddleware(RequestDelegate next, ITodoService service)
    {
      _todoService = service;
      _next = next;
      _providerStates = new Dictionary<string, Action> {
        { "todos exists", AddTodos },
        { "todo id=102 exists", AddTodo },
        { "todo to be added", AddNewTodo },
        { "todo Id 102 exists to update", UpdateTodo }
      

       
      };
    }
    private void AddTodos()
    {
      _todoService.CreateTodoItem(new TodoItem()
      {
        Id = 100,
        Text = "A todo task 1",
        Completed = false,
        DateAdded = DateTime.Now
      });
       _todoService.CreateTodoItem(new TodoItem()
      {
        Id = 101,
        Text = "A todo task 2",
        Completed = true,
        DateAdded = DateTime.Now
      });
    }

    private void AddTodo()
    {
      _todoService.CreateTodoItem(new TodoItem()
      {
        Id = 102,
        Completed = false,
        Text = "A todo task 1",
        DateAdded = DateTime.Now
      });
    }

     private void AddNewTodo()
    {
      _todoService.CreateTodoItem(new TodoItem()
      {
        Id = 502,
        Completed = false,
        Text = "A todo task 500",
        DateAdded = DateTime.Now
      });
    }

    private void UpdateTodo()
    {
      _todoService.UpdateTodoItem(102, new TodoItem(){

         Id = 102,
        Completed = false,
        Text = "A todo task update",
        DateAdded = DateTime.Now
      });
    }

 
    public async Task Invoke(HttpContext context)
    {
      if (context.Request.Path.Value == "/provider-states")
      {
        this.HandleProviderStatesRequest(context);
        await context.Response.WriteAsync(String.Empty);
      }
      else
      {
        await this._next(context);
      }
    }

    private void HandleProviderStatesRequest(HttpContext context)
    {
      context.Response.StatusCode = (int)HttpStatusCode.OK;

      if (context.Request.Method.ToUpper() == HttpMethods.Post.ToString().ToUpper() &&
        context.Request.Body != null)
      {
        string jsonRequestBody = String.Empty;
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
        {
          jsonRequestBody = reader.ReadToEnd();
        }

        var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

        //A null or empty provider state key must be handled
        if (providerState != null &&
          !String.IsNullOrEmpty(providerState.State) &&
          providerState.Consumer == ConsumerName)
        {
          _providerStates[providerState.State].Invoke();
        }
      }
    }
  }
}