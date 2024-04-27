namespace LangLang.Model
{
    public class Director : User
    {
        public Director(string firstName, string lastName, string email, string password, Gender gender, string phone, bool deleted) :
            base(firstName, lastName, email, password, gender, phone)
        {
        }
    }
}