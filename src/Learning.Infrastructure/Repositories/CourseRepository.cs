using Learning.Domain.Entities;
using Learning.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Repositories;

public sealed class CourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
        => _context = context;

    public async Task<Course?> GetCourseById(int id, CancellationToken ct = default!) 
        => await _context.Courses.FindAsync(new object[] { id }, ct);

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
