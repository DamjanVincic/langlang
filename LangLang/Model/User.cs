using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace LangLang.Model
{
    public abstract class User
    {
        private static int _idCounter = 1;
        private static Dictionary<int, User> _users = new Dictionary<int, User>()
        {
            {0,new Director("Nadja", "Zoric", "nadjazoric@gmail.com",
                "PatrikZvezdasti011", Gender.Female, "123456789")}
        };
        public static Dictionary<int, User> Users => _users;
        
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _phone;

        public User(string firstName, string lastName, string email, string password, Gender gender, string phone,int id=-1)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Gender = gender;
            Phone = phone;

            if (id != -1)
                return;

            Id = _idCounter++;
            _users.Add(Id, this);
        }
        
        public void Edit(string firstName, string lastName, string password, Gender gender, string phone)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidatePassword(password);
            ValidatePhoneNumber(phone);
            
            _firstName = firstName;
            _lastName = lastName;
            _password = password;
            Gender = gender;
            _phone = phone;
        }
        
        public void Delete()
        {
            //TODO: Remove user from all courses and exams
            // throw new NotImplementedException();
            _users.Remove(Id);
        }
            
        public static User? Login(string email, string password)
        {
            return _users.Values.FirstOrDefault(user => user.Email.Equals(email) && user.Password.Equals(password));
        }
        
        public static User GetUserById(int id)
        {
            return _users[id];
        }
        
        public int Id { get; }

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
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            
            if (!Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                throw new InvalidInputException("Email not valid");
            }
            
            if (_users.Values.Any(user => user.Email.Equals(email)))
            {
                throw new InvalidInputException("Email already exists");
            }
        }

        private void ValidatePassword(string password)
        {
            if (password == null)
            {
                throw new InvalidInputException(nameof(password));
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

            if (!Regex.IsMatch(phoneNumber, "^\\d+$"))
            {
                throw new InvalidInputException("Phone number must contain only numbers.");
            }
        }
        
        public static bool TryAddUser(User user)
        {
            return _users.TryAdd(user.Id, user);
        }
        public static void LoadUsersFromJson(string jsonFilePath)
        {
            try
            {
                using (StreamReader r = new StreamReader(jsonFilePath))
                {
                    string json = r.ReadToEnd();
                    var usersData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);

                    foreach (var userData in usersData)
                    {
                        int id = int.Parse(userData.Key);
                        var userType = userData.Value["Education"] != null ? typeof(Student) : typeof(Teacher);
                        var user = (User)userData.Value.ToObject(userType);

                        // Check if the key already exists in the _users dictionary
                        if (!_users.ContainsKey(id))
                        {
                            _users.Add(id, user);
                        }
                        else
                        {
                            Console.WriteLine($"Skipping user with duplicate ID: {id}");
                        }

                        if (id >= _idCounter)
                        {
                            _idCounter = id + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading users from JSON: " + ex.Message);
            }
        }




    }
}