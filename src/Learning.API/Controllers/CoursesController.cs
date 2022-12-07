using Learning.Domain.Entities;
using Learning.Infrastructure.Repositories;

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id, CancellationToken ct)
    {
        int coursesAffectedCount = await _courseRepository.DeleteCourse(id, ct);

        return coursesAffectedCount == 1 ? NoContent() : NotFound();
    }
}
