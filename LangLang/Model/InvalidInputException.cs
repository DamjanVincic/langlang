using System;

namespace LangLang.Model
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string message) : base(message)
        {
        }

    }
}
