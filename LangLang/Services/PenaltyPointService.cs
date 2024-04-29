using LangLang.Model;
using LangLang.Repositories;
using System;
using System.Collections.Generic;

namespace LangLang.Services;
public class PenaltyPointService : IPenaltyPointService
{
    private readonly IPenaltyPointRepository _penaltyPointRepository = new PenaltyPointRepository();

    public List<PenaltyPoint> GetAll()
    {
        return _penaltyPointRepository.GetAll();
    }

    public PenaltyPoint? GetById(int id)
    {
        return _penaltyPointRepository.GetById(id);
    }
    public void Add(PenaltyPointReason penaltyPointReason, bool deleted, int studentId, int courseId, int teacherId, DateOnly datePenaltyPointGiven)
    {
        PenaltyPoint point = new PenaltyPoint(penaltyPointReason, deleted, studentId, courseId, teacherId, datePenaltyPointGiven);
        _penaltyPointRepository.Add(point);
    }
    public void Delete(int id)
    {
        PenaltyPoint point = _penaltyPointRepository.GetById(id) ?? throw new InvalidInputException("PenaltyPoint doesnt exist.");
        _penaltyPointRepository.Delete(id);
    }

}
