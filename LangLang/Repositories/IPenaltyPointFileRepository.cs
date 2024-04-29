using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Repositories;

public interface IPenaltyPointFileRepository
{
    public List<PenaltyPoint> GetAll();
    public PenaltyPoint? GetById(int id);
    public int GenerateId();
    public void Add(PenaltyPoint course);
    public void Update(PenaltyPoint course);
    public void Delete(int id);
}