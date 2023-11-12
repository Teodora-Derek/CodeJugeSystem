using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeJudgeSystemWebApplication.Models;

namespace CodeJudgeSystemWebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly StudentContext _context;

    public StudentsController(StudentContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
    {
        return await _context.Students
            .Select(x => StudentToDTO(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDTO>> GetStudent(string id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return StudentToDTO(student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(string id, StudentDTO studentDTO)
    {
        if (id != studentDTO.FacultyNumber)
        {
            return BadRequest();
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        student.FirstName = studentDTO.FirstName;
        student.LastName = studentDTO.LastName;
        student.AcademicDegreeName = studentDTO.AcademicDegreeName;
        student.Group = studentDTO.Group;
        student.Semester = studentDTO.Semester;
        student.Course = studentDTO.Course;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!StudentExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
    {
        var student = new Student
        {
            FirstName = studentDTO.FirstName,
            LastName = studentDTO.LastName,
            AcademicDegreeName = studentDTO.AcademicDegreeName,
            Group = studentDTO.Group,
            Semester = studentDTO.Semester,
            Course = studentDTO.Course,
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetStudent),
            new
            {
                id = student.FacultyNumber
            },
            StudentToDTO(student));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(string id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentExists(string id)
    {
        return _context.Students.Any(e => e.FacultyNumber == id);
    }

    private static StudentDTO StudentToDTO(Student student) =>
       new StudentDTO
       {
           FirstName = student.FirstName,
           LastName = student.LastName,
           AcademicDegreeName = student.AcademicDegreeName,
           Group = student.Group,
           Semester = student.Semester,
           Course = student.Course,
       };
}