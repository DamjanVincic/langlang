using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Repositories;

public interface IExamRepository
{
    public List<Exam> GetAll();
    public Exam? GetById(int id);
    public int GenerateId();
    public void Add(Exam exam);
    public void Update(Exam exam);
    public void Delete(int id);
}