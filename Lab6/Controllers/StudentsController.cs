using Lab6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _context;
        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }
        //GET ALL
        /// <summary>
        /// Get collection of Students.
        /// </summary>
        /// <returns>A collection of Students</returns>
        /// <response code="200">Returns a collection of Students</response>
        /// <response code="500">Internal error</response>      
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return Ok(await _context.Student.ToListAsync());
        }
        // GET ON ID
        /// <summary>
        /// Get a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <returns>A Student</returns>
        /// <response code="201">Returns a collection of Student</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="404">If the Student is null</response>      
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            Student s = await _context.Student.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }
            return Ok(s);
        }
        // POST
        /// <summary>
        /// Creates a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Student
        ///     {
        ///        "FirstName": "Barzin",
        ///        "LastName": "Farahani"
        ///        "Program": "Program"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Student</returns>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateAsync([Bind("FirstName,LastName,Program")] StudentBase StudentBase)
        {
            Student stu = new Student
            {
               FirstName = StudentBase.FirstName,
               LastName = StudentBase.LastName,
               Program = StudentBase.Program
            };

            _context.Add(stu);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stu.ID }, stu);
        }
        // PUT
        /// <summary>
        /// Upserts a Student.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Student
        ///     {
        ///        "FirstName": "Barzin",
        ///        "LastName": "Farahani"
        ///        "Program": "Program"
        ///     }
        ///
        /// </remarks>
        /// <param id="id"></param>
        /// <returns>An upserted Student</returns>
        /// <response code="200">Returns the updated Student</response>
        /// <response code="201">Returns the newly created Student</response>
        /// <response code="400">If the Student or id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> Upsert(Guid id, [Bind("FirstName,LastName,Program")] StudentBase StudentBase)
        {
            Student stu = new Student
            {
               FirstName = StudentBase.FirstName,
               LastName = StudentBase.LastName,
               Program = StudentBase.Program
            };

            if (!stuExists(id))
            {
                stu.ID = id;
                _context.Add(stu);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = stu.ID }, stu);
            }

            Student dbStu = await _context.Student.FindAsync(id);
            dbStu.FirstName = stu.FirstName;
            dbStu.LastName = stu.LastName;
            dbStu.Program = stu.Program;
            _context.Update(dbStu);
            await _context.SaveChangesAsync();

            return Ok(dbStu);
        }
        // DELETE
        /// <summary>
        /// Deletes a Student.
        /// </summary>
        /// <param id="id"></param>
        /// <response code="202">Student is deleted</response>
        /// <response code="400">If the id is malformed</response>      
        /// <response code="500">Internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _context.Student.FindAsync(id);
            _context.Student.Remove(car);
            await _context.SaveChangesAsync();
            return Accepted();
        }
        private bool stuExists(Guid id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }

}
