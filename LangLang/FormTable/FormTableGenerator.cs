using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.FormTable
{
    public class FormTableGenerator<T,S>
    {
        private Type _type;
        private Type _service;
        private IEnumerable<T> _data;
        private int _columnsPerPage;

        public FormTableGenerator(IEnumerable<T> data, int columnsPerPage = 5)
        {
            _data = data;
            _type = typeof(T);
            _service = typeof(S);
            _columnsPerPage = columnsPerPage;
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
