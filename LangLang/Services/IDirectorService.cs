namespace LangLang.Services
{
    public interface IDirectorService
    {
        public void GeneratePenaltyReport();
        public void NotifyBestStudents(int courseId, bool knowledgePoints);
    }
}
