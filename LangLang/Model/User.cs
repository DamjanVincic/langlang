﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LangLang.Model
{
    public abstract class User
    {
        // = null! to suppress nullable warning because the values are validated
        private string _firstName = null!;
        private string _lastName = null!;
        private string _email = null!;
        private string _password = null!;
        private string _phone = null!;
        private bool _deleted;

        protected User(string firstName, string lastName, string email, string password, Gender gender, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Gender = gender;
            Phone = phone;
            Deleted = false;
        }

        public int Id { get; set; }

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
            private set
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

        public bool Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
            }
        }

        private static void ValidateFirstName(string firstName)
        {
            switch (firstName)
            {
                case null:
                    throw new ArgumentNullException(nameof(firstName));
                case "":
                    throw new InvalidInputException("First name must include at least one character.");
            }
        }

        private static void ValidateLastName(string lastName)
        {
            switch (lastName)
            {
                case null:
                    throw new ArgumentNullException(nameof(lastName));
                case "":
                    throw new InvalidInputException("Last name must include at least one character.");
            }
        }

        private static void ValidateEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                throw new InvalidInputException("Email not valid");
            }
        }

        private static void ValidatePassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
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

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            switch (phoneNumber)
            {
                case null:
                    throw new ArgumentNullException(nameof(phoneNumber));
                case "":
                    throw new InvalidInputException("Phone number must not be empty.");
            }

            if (phoneNumber.Length < 10)
            {
                throw new InvalidInputException("Phone number must contain at least 10 numbers.");
            }

            if (!Regex.IsMatch(phoneNumber, "^\\d+$"))
            {
                throw new InvalidInputException("Phone number must contain only numbers.");
            }
        }
    }
}