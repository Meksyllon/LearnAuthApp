using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AuthTestApp.Models;

namespace AuthTestApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ToDoItem> Tasks { get; } = new();
        public string Content => "VM CONTENT";        
    }
}
