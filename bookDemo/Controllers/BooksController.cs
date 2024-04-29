using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            List<Book> BookList = Data.ApplicationContext.Books;
            return Ok(BookList);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBookById([FromRoute(Name = "id")] int id)
        {
            Book book = Data.ApplicationContext.Books
                .Where(b => b.Id.Equals(id))
                .SingleOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult SaveBook([FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest();
                }
                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);

            } catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBookById([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {

            Book bookToUpdate = ApplicationContext.Books.Find(b => b.Id.Equals(book.Id));

            if (bookToUpdate is null)
            {
                return NotFound();
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            ApplicationContext.Books.Remove(bookToUpdate);
            book.Id = bookToUpdate.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);

        }

        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteBookById([FromRoute(Name = "id")] int id)
        {

            Book bootToDelete = ApplicationContext.Books
                .Find(b => b.Id == id);

            if (bootToDelete is null)
            {
                return NotFound
                    (
                        new { statusCode = 404, Message = "Böyle Bir Kitap Yok" }
                    );
            }

            ApplicationContext.Books.Remove(bootToDelete);
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        public IActionResult PatchUpdateBook(
            [FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> book)
        {

            Book bookToUpdate = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (bookToUpdate is null)
            {
                return NotFound();
            }

            book.ApplyTo(bookToUpdate);
            return NoContent();

        }

    }
}
