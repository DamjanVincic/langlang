using LangLang.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LangLang.FormTable
{
    public class FormTableGenerator<T>
    {
        private readonly Type _type;
        private readonly object _service;
        private readonly IEnumerable<T> _data;
        private readonly int _columnsPerPage;

        public FormTableGenerator(IEnumerable<T> data, object service, int columnsPerPage = 5)
        {
            _data = data;
            _type = typeof(T);
            _service = service;
            _columnsPerPage = columnsPerPage;
        }
        public T GetById(object id)
        {
            var serviceType = _service.GetType();
            var getByIdMethods = serviceType.GetMethods().Where(m => m.Name == "GetById" && m.GetParameters().Length == 1);

            foreach (var getByIdMethod in getByIdMethods)
            {
                var parameterType = getByIdMethod.GetParameters()[0].ParameterType;
                if (parameterType.IsAssignableFrom(id.GetType()))
                {
                    try
                    {
                        var result = getByIdMethod.Invoke(_service, new object[] { id });
                        return (T)result!;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting item by ID: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"Method 'GetById' with compatible parameter type not found on the service.");
            return default!;
        }

        private List<object> GetData(User user)
        {
            List<object> arguments = new();
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
                    if (param.ParameterType.IsGenericType && param.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        arguments.Add(null);
                    }
                    else
                    {
                        Console.WriteLine($"Enter {param.Name} ({param.ParameterType.Name}): ");

                        if (param.ParameterType.IsEnum)
                        {
                            foreach (var en in Enum.GetValues(param.ParameterType))
                            {
                                Console.WriteLine(">>" + en);
                            }
                        }
                        string input = Console.ReadLine();
                        object value = Memory.GetValueFromInput(input, param.ParameterType);
                        arguments.Add(value);
                    }
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
                return default!;
            }

            try
            {
                object result = method.Invoke(_service, arguments.ToArray())!;
                Console.WriteLine("Item successfully added.");
                return (T)result!;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error! Item not created");

                return default!;
            }
        }

        // onlt update properties that service allows
        private Dictionary<string, object> Prompt(T item, MethodInfo updateMethod)
        {
            var values = new Dictionary<string, object?>();

            foreach (var parameter in updateMethod.GetParameters())
            { 

                var prop = _type.GetProperty(parameter.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    var value = prop.GetValue(item);
                    string formattedValue = value?.ToString() ?? string.Empty;

                    // prolaze iter, primitivni,enumi,datum i vreme
                    // ne prolazi Language i nullable
                    if ((parameter.ParameterType.IsPrimitive
                        || typeof(IEnumerable).IsAssignableFrom(parameter.ParameterType)
                        || parameter.ParameterType.IsEnum
                        || parameter.ParameterType == typeof(DateOnly) 
                        || parameter.ParameterType == typeof(TimeOnly))
                        && !(parameter.Name!.Equals("id", StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine($"{parameter.Name} ({parameter.ParameterType.Name}) [Current value: {formattedValue}]: ");
                        string input = Console.ReadLine()!;

                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            object newValue = Memory.GetValueFromInput(input, parameter.ParameterType);
                            values[parameter.Name] = newValue;
                        }
                        else
                        {
                            values[parameter.Name] = value;
                        }
                    }
                    else values[parameter.Name] = value;
                }
                else
                {
                    // if property not found, consider it empty or default
                    values[parameter.Name] = parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null;
                }
            }

            return values!;
        }

        public void Update(T item, params string[] propertiesToUpdate)
        {
            var serviceType = _service.GetType();
            var updateMethod = serviceType.GetMethod("Update");
            if (updateMethod == null)
            {
                Console.WriteLine("Method 'Update' not found on the service.");
                return;
            }

            // to get all new and old values
            var values = Prompt(item, updateMethod);
            // to invoke
            var methodParameters = new List<object>();
            object[] valuesArray = values.Values.ToArray();

            try
            {
                updateMethod.Invoke(_service, valuesArray);
                Console.WriteLine("Item successfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
            }
        }
        public void Delete(int id)
        {
            try
            {
                var serviceType = _service.GetType();
                var method = serviceType.GetMethod("Delete");
                method!.Invoke(_service, new object[] { id });
                Console.WriteLine("Success");
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input. Action terminated.");
            }

        }


        public void SmartPick(object item)
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
            var properties = _type.GetProperties()
                .Where(p => p.IsDefined(typeof(TableItemAttribute), false))
                .OrderBy(p => p.GetCustomAttribute<TableItemAttribute>()!.ColumnOrder)
                .Skip(pageIndex * _columnsPerPage)
                .Take(_columnsPerPage);

            var sb = new StringBuilder();
            foreach (var item in properties)
            {
                sb.Append(item.Name.PadRight(25)).Append(" ");
            }
            return sb.ToString();
        }

        private string CreateRow(T item, int pageIndex)
        {
            var properties = _type.GetProperties()
                .Where(p => p.IsDefined(typeof(TableItemAttribute), false))
                .OrderBy(p => p.GetCustomAttribute<TableItemAttribute>()!.ColumnOrder)
                .Skip(pageIndex * _columnsPerPage)
                .Take(_columnsPerPage);

            var sb = new StringBuilder();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(item);
                string formattedValue;
                if (value is IEnumerable enumerable && !(value is string))
                {
                    // If the value is enumerable (list, dictionary, etc.), format it for display
                    formattedValue = $"[{string.Join(", ", enumerable.Cast<object>())}]";
                }
                else
                {
                    // Otherwise, use the ToString() method
                    formattedValue = value?.ToString() ?? string.Empty;
                }
                sb.Append(formattedValue.PadRight(25)).Append(" ");
            }
            return sb.ToString();
        }
    }
}
