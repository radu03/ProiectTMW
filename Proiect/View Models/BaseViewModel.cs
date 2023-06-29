using System.ComponentModel;
using System.Runtime.CompilerServices;
using Proiect.Models;



namespace Proiect.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected Proiect.Models.Database Database { get; private set; }


        protected BaseViewModel(Proiect.Models.Database database)
        {
            Database = database;
        }

        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

