namespace LangLang.Model;

public class Student : User
{
    public Education Education { get; set; }
    
    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education education, int id = -1)
        : base(firstName, lastName, email, password, gender, phone, id)
    {
        Education = education;
    }

    public void Edit(string firstName, string lastName, string password, Gender gender, string phone,
        Education education)
    {
        //TODO: Validate if user hasn't applied to any courses or exams
        base.Edit(firstName, lastName, password, gender, phone);
        Education = education;
    }
}