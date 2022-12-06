using Learning.Domain.Entities;
using Learning.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using MR.AspNetCore.Pagination;

namespace Learning.Infrastructure.Repositories;

public sealed class CourseRepository
{
    private readonly AppDbContext _context;
    private readonly IPaginationService _paginationService;

    public CourseRepository(AppDbContext context, IPaginationService paginationService)
    {
        _context = context;
        _paginationService = paginationService;
    }

    public async ValueTask<Course?> GetCourseById(int id, CancellationToken ct = default!) 
        => await _context.Courses.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<Course>> GetCourses(CancellationToken ct = default!)
        => await _context.Courses.AsNoTracking().ToListAsync(ct);

    public async Task<KeysetPaginationResult<Course>> GetCoursesKeysetPaginated(CancellationToken ct = default!)
    {
        return await _paginationService.KeysetPaginateAsync(_context.Courses,
            b => b.Ascending(x => x.ID),
            async id => await GetCourseById(int.Parse(id), ct));
    }

    public async Task<int> AddCourse(Course newEntity, CancellationToken ct = default!)
    {
        await _context.Courses.AddAsync(newEntity, ct);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> AddCourseRange(IEnumerable<Course> newEntities, CancellationToken ct = default!)
    {
        await _context.Courses.AddRangeAsync(newEntities, ct);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateCourse(Course updatedEntity, CancellationToken ct = default!)
    {
        _context.Courses.Update(updatedEntity);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateCourseRange(IEnumerable<Course> updatedEntities, CancellationToken ct = default!)
    {
        _context.Courses.UpdateRange(updatedEntities);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteCourse(int id, CancellationToken ct = default!)
        => await _context.Courses.Where(c => c.ID == id).ExecuteDeleteAsync(ct);
}
