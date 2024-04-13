using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Repositories;

public interface ICourseRepository
{
    public List<Course> GetAll();
    public Course? GetById(int id);
    public int GenerateId();
    public void Add(Course course);
    public void Update(Course course);
    public void Delete(int id);
}