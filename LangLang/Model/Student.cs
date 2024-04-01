namespace LangLang.Model;

public class Student : User
{
    public Education Education { get; set; }
    
    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education education)
        : base(firstName, lastName, email, password, gender, phone)
    {
        Education = education;
    }

    public void Edit(string firstName, string lastName, string password, Gender gender, string phone,
        Education education)
    {
        base.Edit(firstName, lastName, password, gender, phone);
        Education = education;
    }
}