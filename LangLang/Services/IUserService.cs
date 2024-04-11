using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface IUserService
{
    List<User> GetAll();
    User? GetById(int id);

    public void Add(string firstName, string lastName, string email, string password, Gender gender, string phone,
        Education? education = null, List<Language>? languages = null);

    public void Update(int id, string firstName, string lastName, string password, Gender gender, string phone,
        Education? education = null, List<Language>? languages = null);

    void Delete(int id);
    User? Login(string email, string password);
}