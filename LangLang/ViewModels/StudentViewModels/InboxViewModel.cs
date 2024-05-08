using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.Services;
using LangLang.ViewModels.CourseViewModels;
using LangLang.Views.CourseViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace LangLang.ViewModels.StudentViewModels
{
    class InboxViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService = new MessageService();
        private readonly ObservableCollection<MessageViewModel> _messages;
        private readonly int _studentId;
        public InboxViewModel(int studentId)
        {
            _studentId = studentId;
            _messages = new ObservableCollection<MessageViewModel>(_messageService.GetUserMessages(_studentId).Select(message => new MessageViewModel(message)));
            MessagesCollectionView = CollectionViewSource.GetDefaultView(_messages);
        }
        public ICollectionView MessagesCollectionView { get; }

        public ObservableCollection<MessageViewModel> Messages => _messages;
        private void RefreshMessages()
        {
            Messages.Clear();
            _messageService.GetUserMessages(_studentId).Select(message => new MessageViewModel(message));
            MessagesCollectionView.Refresh();
        }
    }
}