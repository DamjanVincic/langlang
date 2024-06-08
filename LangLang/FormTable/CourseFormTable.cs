using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LangLang.Models;
namespace LangLang.FormTable;
public class CourseFormTable
{
    Type type = typeof(Course);
    

    public void CourseMenu()
    {
        while (true)
        {
            Console.WriteLine("Chose option for course \n" +
                "1) create \n" +
                "2) update \n" +
                "3) delete \n" +
                "4) list all exams \n" +
                "q)uit");
            string userInput = Console.ReadLine();
            if (userInput == "1") 
                Add();
            if (userInput == "2")
                Update();
            if (userInput == "3")
                Delete();
            if(userInput == "4")
                Read();
            if (userInput == "q" || userInput == "Q")
                break;
            else
                Console.WriteLine("invalid option \n");
        }
    }
    public void Add() { }
    public void Update() { }
    public void Delete() { }
    public void Read() { }
}