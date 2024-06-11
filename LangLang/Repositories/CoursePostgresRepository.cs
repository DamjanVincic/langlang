using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using Microsoft.EntityFrameworkCore;

namespace LangLang.Repositories;

public class CoursePostgresRepository : ICourseRepository
{
    private readonly DatabaseContext _dbContext;
    
    public CoursePostgresRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public List<Course> GetAll()
    {
        return _dbContext.Courses.ToList();
    }

    public Course? GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public int GenerateId()
    {
        throw new System.NotImplementedException();
    }

    public void Add(Course course)
    {
        _dbContext.Courses.Add(course);
        _dbContext.SaveChanges();
    }

    public void Update(Course course)
    {
        throw new System.NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new System.NotImplementedException();
    }
}