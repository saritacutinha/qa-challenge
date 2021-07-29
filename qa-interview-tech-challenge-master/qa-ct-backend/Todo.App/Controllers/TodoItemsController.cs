using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.App.Models;
using Todo.App.Services;

namespace Todo.App.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : Controller
    {
        private readonly ITodoService _todoService;


        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
        }

        // GET: api/TodoItems
        [SwaggerOperation(Summary = "Retrieve all the unique todo items less than a year old")]
        [HttpGet]
        [Route("/api/TodoItems/{includeCompleted:bool?}")]
        public IActionResult GetTodoItems(bool includeCompleted = true)
        {
            var todos = _todoService.GetTodoItems(includeCompleted)
              .Select(todo => ItemToDTO(todo))
              .ToList();
            return Ok(todos);
        }

        // GET: api/TodoItem/5
        [SwaggerOperation(Summary = "Retrieve the specific item")]
        [HttpGet("{id}")]
        public IActionResult GetTodoItem(long id)
        {
            var todoItem = _todoService.GetTodoItem(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(ItemToDTO(todoItem));
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [SwaggerOperation(Summary = "Update the specific item")]
        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = _todoService.GetTodoItem(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Text = todoItemDTO.Text;
            todoItem.Completed = todoItemDTO.Completed;

            if (!_todoService.Save())
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [SwaggerOperation(Summary = "Create an item")]
        [HttpPost]
        public IActionResult CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                Completed = todoItemDTO.Completed,
                Text = todoItemDTO.Text,
                DateAdded = DateTime.Now
            };

            _todoService.CreateTodoItem(todoItem);

            if (!_todoService.Save())
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [SwaggerOperation(Summary = "Delete the specific item")]
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(long id)
        {
            var todoItem = _todoService.GetTodoItem(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _todoService.DeleteTodoItem(id);

            if (!_todoService.Save())
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
          _todoService.TodoItemExists(id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) => new TodoItemDTO
        {
            Id = todoItem.Id,
            Text = todoItem.Text,
            Completed = todoItem.Completed
        };
    }
}
