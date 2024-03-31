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
        private string _firstName;
        public string FirstName {
            get => _firstName;
            set
            {
                ValidateFirstName(value);
                _firstName = value;
            }
        }

        private string _lastName;
        public string LastName {
            get => _lastName;
            set
            {
                ValidateLastName(value);
                _lastName = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                ValidateEmail(value);
                _email = value;
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                ValidatePassword(value);
                _password = value;
            }
        }
        public Gender Gender { get; set; }

        private string _phone;
        public string Phone {
            get => _phone;
            set
            {
                ValidatePhoneNumber(value);
                _phone = value;
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

        private void ValidateFirstName(string firstName)
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

        private void ValidateLastName(string lastName)
        {
            switch (lastName)
            {
                case null:
                    throw new ArgumentNullException(nameof(lastName));
                case "":
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

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if(phoneNumber == null)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
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
}


