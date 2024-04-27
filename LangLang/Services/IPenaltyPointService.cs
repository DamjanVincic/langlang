using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services;
public interface IPenaltyPointService
{
    public List<PenaltyPoint> GetAll();
    public PenaltyPoint? GetById(int id);

    public void Add(PenaltyPointReason penaltyPointReason, bool deleted, int studentId, int courseId, int teacherId, DateOnly datePenaltyPointGiven);

    // cant change ids only if point was removed/deleted
    public void Delete(int id);
}