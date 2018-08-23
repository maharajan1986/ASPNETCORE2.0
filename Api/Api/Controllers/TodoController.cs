using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/Todo")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public IActionResult Create([FromBody]TodoItem todo)
        {
            if (ModelState.IsValid)
            {
                _context.TodoItems.Add(todo);
                _context.SaveChanges();
            }
            return CreatedAtRoute("GetTodo", new { Id = todo.Id }, todo);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody]TodoItem item)
        {
            if(item == null || item.Id != id)
            {
                return BadRequest();
            }
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.TodoItems.Where(x => x.Id == id).FirstOrDefault();
            if(item ==null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }
    }
}