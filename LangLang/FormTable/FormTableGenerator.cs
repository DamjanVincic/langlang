using LangLang.Models;
using LangLang.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.util;

namespace LangLang.FormTable
{
    public class FormTableGenerator<T>
    {
        private Type _type;
        private object _service;
        private IEnumerable<T> _data;
        private int _columnsPerPage;

        public FormTableGenerator(IEnumerable<T> data, object service, int columnsPerPage = 5)
        {
            _data = data;
            _type = typeof(T);
            _service = service;
            _columnsPerPage = columnsPerPage;
        }

        private List<object> GetData(User user)
        {
            List<object> arguments = new List<object>();
            var method = _service.GetType().GetMethod("Add");
            if (method == null)
            {
                Console.WriteLine("Metoda 'Add' nije pronađena na servisu.");
                return arguments;
            }

            var parameters = method.GetParameters();

            foreach (var param in parameters)
            {
                // i nastavnik i direktor su kreatori tako da ih samo dodaj
                // za id nastavnika, ako nastavnik kreira dodaj ga, ako direktor kreira kasnije mora da se pozove smart pick i promenice se tako da se moze ovo samo privremeno staviti
                if(param.Name == "teacherId" || param.Name == "creatorId")
                {
                    arguments.Add(user.Id);
                }
                else
                {
                    Console.WriteLine($"Enter {param.Name} ({param.ParameterType.Name}): ");
                    string input = Console.ReadLine();
                    object value = Memory.GetValueFromInput(input, param.ParameterType);
                    arguments.Add(value);
                }
            }
            return arguments;
        }

        public T Create(User user)
        {
            List<object> arguments = GetData(user);
            var serviceType = _service.GetType();
            var method = serviceType.GetMethod("Add");
            if (method == null)
            {
                Console.WriteLine("Method 'Add' not found on the service.");
                return default;
            }

            try
            {
                object result = method.Invoke(_service, arguments.ToArray());
                Console.WriteLine("Item successfully added.");
                return (T)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating item: {ex.Message}");
                return default;
            }
        }

        public void SmartPick(User user, object item)
        {
            var serviceType = _service.GetType();
            var method = serviceType.GetMethod("SmartPick");
            if (method == null)
            {
                Console.WriteLine("Method 'SmartPick' not found on the service.");
                return;
            }

            try
            {
                method.Invoke(_service, new object[] { item });
                Console.WriteLine("SmartPick completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SmartPick: {ex.Message}");
            }
        }


        public void ShowTable()
        {
            var properties = _type.GetProperties();
            int totalColumns = properties.Length;
            int pages = (int)Math.Ceiling((double)totalColumns / _columnsPerPage);

            for (int i = 0; i < pages; i++)
            {
                Console.WriteLine(CreateHeader(i));
                foreach (var item in _data)
                {
                    Console.WriteLine(CreateRow(item, i));
                }
                Console.WriteLine("\nPress Enter to see next page...");
                Console.ReadLine();
            }
        }

        private string CreateHeader(int pageIndex)
        {
            var properties = _type.GetProperties().Skip(pageIndex * _columnsPerPage).Take(_columnsPerPage);
            var sb = new StringBuilder();
            foreach (var item in properties)
            {
                sb.Append(item.Name.PadRight(15)).Append(" ");
            }
            return sb.ToString();
        }

        private string CreateRow(T item, int pageIndex)
        {
            var properties = _type.GetProperties().Skip(pageIndex * _columnsPerPage).Take(_columnsPerPage);
            var sb = new StringBuilder();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(item)?.ToString() ?? string.Empty;
                sb.Append(value.PadRight(15)).Append(" ");
            }
            return sb.ToString();
        }
    }
}
