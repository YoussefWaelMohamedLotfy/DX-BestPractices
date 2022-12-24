using Learning.Domain.Entities;
using Learning.Infrastructure.Repositories;

using MapsterMapper;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;

namespace Learning.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseRepository _courseRepository;

    public CoursesController(CourseRepository courseRepository)
        => _courseRepository = courseRepository;

    [HttpGet("{id}", Name = "[Action]")]
    public async Task<IActionResult> GetCourse(int id, CancellationToken ct)
    {
        Course? course = await _courseRepository.GetCourseById(id, ct);

        return course is null ? NotFound() : Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses(CancellationToken ct)
        => Ok(await _courseRepository.GetCoursesKeysetPaginated(ct));

    [HttpPost]
    public async Task<IActionResult> PostCourse([FromBody] Course newCourse, [FromServices] IMapper mapper, CancellationToken ct)
    {
        var course =  mapper.Map<Course>(newCourse);
        await _courseRepository.AddCourse(course, ct);
        return CreatedAtRoute(nameof(GetCourse), new { id = course.ID}, course);
    }

    [HttpPut]
    public async Task<IActionResult> PutCourse([FromBody] Course updatedCourse, [FromServices] IMapper mapper, CancellationToken ct)
    {
        var course = mapper.Map<Course>(updatedCourse);

        await _courseRepository.UpdateCourse(course, ct);
        return Ok(course);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(int id, [FromBody] JsonPatchDocument<Course> patchDoc, CancellationToken ct)
    {
        if (patchDoc is not null)
        {
            var customer = await _courseRepository.GetCourseById(id, ct);

            patchDoc.ApplyTo(customer, (IObjectAdapter)ModelState);

            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(customer);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id, CancellationToken ct)
    {
        int coursesAffectedCount = await _courseRepository.DeleteCourse(id, ct);

        return coursesAffectedCount == 1 ? NoContent() : NotFound();
    }
}
