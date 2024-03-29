using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class User
    { 
        public string FirstName {
            get
            {
                return FirstName;
            }
            set
            {
                FirstName = value;
            }
        }
        public string LastName {
            get
            {
                return LastName;
            }
            set
            {
                ValidateLastName(value);
                LastName = value;
            }
            }
        public string Email
        {
            get
            {
                return Email;
            }
            set
            {
                ValidateEmail(value);
                Email = value;
            }
        }
        public string Password
        {
            get
            {
                return Password;
            }
            set
            {
                ValidatePassword(value);
                Password = value;
            }
        }
        public Gender Gender { get; set; }
        public string Phone {
            get
            {
                return Phone;
            }
            set
            {
                ValidatePhoneNumber(value);
                Phone = value;
            }
        }

        public User(string firstName, string lastName, string email, string password, Gender gender, string phone)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);    
            ValidatePassword(password);
            ValidatePhoneNumber(phone);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Gender = gender;
            Phone = phone;
        }

        public void ValidateFirstName(string firstName)
        {
            if(firstName == null)
            {
                throw new ArgumentNullException("First name must not be null.");
            }
            if(firstName.Equals(""))
            {
                throw new InvalidInputException("First name must include at least one character.");
            }
        }
        public void ValidateLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException("Last name must not be null.");
            }
            if (lastName.Equals(""))
            {
                throw new InvalidInputException("Last name must include at least one character.");
            }
        }
        private void ValidateEmail(string email)
        {
            if (email == null || !Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n"))
            {
                throw new InvalidInputException("Email not valid");
            }
        }
        private void ValidatePassword(string password)
        {
            if (password == null)
            {
                throw new InvalidInputException("Password can not be null");
            }

            if (password.Length < 8)
            {
                throw new InvalidInputException("Password must be at least eight characters long");
            }

            if (!password.Any(char.IsUpper))
            {
                throw new InvalidInputException("Password must contain at least one uppercase letter");
            }

            if (!password.Any(char.IsLower))
            {
                throw new InvalidInputException("Password must contain at least one lowercase letter");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new InvalidInputException("Password must contain at least one digit");
            }
        }

        public void ValidatePhoneNumber(string phoneNumber)
        {
            if(phoneNumber == null)
            {
                throw new ArgumentNullException("Phone number must not be null.");
            }
            if(phoneNumber.Equals("")){
                throw new InvalidInputException("Phone number must not be empty.");
            }
            if(phoneNumber.Length < 10)
            {
                throw new InvalidInputException("Phone number must contain at least 10 numbers.");
            }
            if(!Regex.IsMatch(phoneNumber, "^\\d+$\r\n"))
            {
                throw new InvalidInputException("Phone number must contain only numbers.");
            }

        }
    }
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


