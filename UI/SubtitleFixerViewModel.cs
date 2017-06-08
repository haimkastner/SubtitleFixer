using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SubtitleFixer
{

    public class SubtitleFixerViewModel : INotifyPropertyChanged
    {
        public delegate void BrodcastMessageDel(string message);
        public event BrodcastMessageDel BrodcastMessage;

        public ICommand FolderSelectCommand { get; set; }
        public ICommand FileSelectCommand { get; set; }

        public ICommand FixSeriosCommand { get; set; }
        public ICommand FixMoviesCommand { get; set; }

        private string _folderPath;
        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public SubtitleFixerViewModel()
        {
            // for debug
            FolderPath = ConfigHandle.FolderPath;

            FolderSelectCommand = new RegularCommends(new Action<object>(FolderSelect));
            FileSelectCommand = new RegularCommends(new Action<object>(FileSelect));

            FixSeriosCommand = new ActionCommends(new Action<object>(FixSerois));
            FixMoviesCommand = new ActionCommends(new Action<object>(FixMovies));
        }

        private void FolderSelect(object param)
        {
            ConfigHandle.FolderPath = FolderPath = Dialogs.SelectFolder();
        }

        private void FileSelect(object param)
        {
            
        }

        private void FixMovies(object param)
        {

        }

        private void FixSerois(object param)
        {
            if (Logics.FixSerios(FolderPath))
                BrodcastMessage?.Invoke("בוצע בהצלחה");
            else
                BrodcastMessage?.Invoke("שגיאה");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
