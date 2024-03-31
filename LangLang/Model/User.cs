﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LangLang.Model
{
    public class User
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _phone;

        public User(string firstName, string lastName, string email, string password, Gender gender, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Gender = gender;
            Phone = phone;
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                ValidateFirstName(value);
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                ValidateLastName(value);
                _lastName = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                ValidateEmail(value);
                _email = value;
            }
        }

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

        public string Phone
        {
            get => _phone;
            set
            {
                ValidatePhoneNumber(value);
                _phone = value;
            }
        }

        private void ValidateFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Equals(""))
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
            if (phoneNumber == null)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            if (phoneNumber.Equals(""))
            {
                throw new InvalidInputException("Phone number must not be empty.");
            }

            if (phoneNumber.Length < 10)
            {
                throw new InvalidInputException("Phone number must contain at least 10 numbers.");
            }

            if (!Regex.IsMatch(phoneNumber, "^\\d+$\r\n"))
            {
                throw new InvalidInputException("Phone number must contain only numbers.");
            }
        }
    }
}