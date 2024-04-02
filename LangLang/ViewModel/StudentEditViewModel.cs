using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;

namespace LangLang.ViewModel;

public class StudentEditViewModel : ViewModelBase
{
    private Student _student;
    
    // public string FirstName => _student.FirstName;
    // public string LastName => _student.LastName;
    // // public string Email => _student.Email;
    // public string Phone => _student.Phone;
    // public Gender Gender => _student.Gender;
    // public Education Education => _student.Education;
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public Gender Gender { get; set; }
    public Education Education { get; set; }

    public Array GenderValues => Enum.GetValues(typeof(Gender));
    public Array EducationValues => Enum.GetValues(typeof(Education));
    
    public ICommand SaveCommand { get; }

    private Window _editWindow;
    
    public StudentEditViewModel(Student student, Window editWindow)
    {
        _editWindow = editWindow;
        
        _student = student;
        FirstName = _student.FirstName;
        LastName = _student.LastName;
        Password = _student.Password;
        Phone = _student.Phone;
        Gender = _student.Gender;
        Education = _student.Education;
        
        SaveCommand = new RelayCommand(Save);
    }

    public void Save()
    {
        try
        {
            _student.Edit(FirstName, LastName, Password, Gender, Phone, Education);
            _editWindow.Close();
            MessageBox.Show("Information successfully edited.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (InvalidInputException exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (ArgumentNullException exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}