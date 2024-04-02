using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Director : User
    {
        public Director(string firstName, string lastName, string email, string password, Gender gender, string phone) : base(firstName, lastName, email, password, gender, phone, 0)
        {
        }
    }
}