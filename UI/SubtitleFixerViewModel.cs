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
    /// <summary>
    /// this class is the View Model of SubtitleFixerView.xaml view in MVVM 
    /// </summary>
    public class SubtitleFixerViewModel : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// event of messages to send to view
        /// </summary>
        /// <param name="message">message to send</param>
        public delegate void BrodcastMessageDel(string message);
        public event BrodcastMessageDel BrodcastMessage;

        #endregion

        #region Commands

        /// <summary>
        /// invoke select folder method
        /// </summary>
        public ICommand FolderSelectCommand { get; set; }
        /// <summary>
        /// invoke select file method
        /// </summary>
        public ICommand FileSelectCommand { get; set; }
        /// <summary>
        /// invoke FixSeries method
        /// </summary>
        public ICommand FixSeriesCommand { get; set; }
        /// <summary>
        /// invoke FixMovies method
        /// </summary>
        public ICommand FixMoviesCommand { get; set; }
        /// <summary>
        /// invoke FixEncoding method
        /// </summary>
        public ICommand FixEncodingCommand { get; set; }

        #endregion

        #region Data Members

        /// <summary>
        /// Selection status true meen file false meen folder ==== need to convert to ENUM
        /// </summary>
        private bool _selectionStatus;
        /// <summary>
        /// Folder Path
        /// </summary>
        private string _folderPath;
        /// <summary>
        /// File Path
        /// </summary>
        private string _filePath;

        #endregion

        #region Data Members Properties

        /// <summary>
        /// Selection status true meen file false meen folder ==== need to convert to ENUM
        /// </summary>
        public bool SelectionStatus
        {
            get
            {
                return _selectionStatus;
            }
            set
            {
                _selectionStatus = value;
                OnPropertyChanged("SelectionStatus");
            }
        }

        /// <summary>
        /// Folder Path
        /// </summary>
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

        /// <summary>
        /// File Path
        /// </summary>
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

        #endregion

        #region Ctor

        public SubtitleFixerViewModel()
        {
            // set ti to be ENUM and adding converts
            SelectionStatus = false;

            // for start postion
            FolderPath = ConfigHandle.FolderPath;
            FilePath= ConfigHandle.FilePath;

            // create the commands
            FolderSelectCommand = new SimpleCommends(new Action<object>(FolderSelect));
            FileSelectCommand = new SimpleCommends(new Action<object>(FileSelect));

            FixSeriesCommand = new FixSubsCommends(new Action<object>(FixSeries));
            FixMoviesCommand = new FixSubsCommends(new Action<object>(FixMovies));
            FixEncodingCommand = new FixEncodeCommand(new Action<object>(FixEncoding));
        }

        #endregion

        #region Methods for commends

        /// <summary>
        /// Select folder
        /// </summary>
        /// <param name="param">null</param>
        private void FolderSelect(object param)
        {
            ConfigHandle.FolderPath = FolderPath = Dialogs.SelectFolder();
        }

        /// <summary>
        /// Select file
        /// </summary>
        /// <param name="param">null</param>
        private void FileSelect(object param)
        {
            ConfigHandle.FilePath = FilePath = Dialogs.SelectFile();
        }

        /// <summary>
        /// Fix Movies
        /// </summary>
        /// <param name="param">null</param>
        private void FixMovies(object param)
        {

        }

        /// <summary>
        /// Fix Series
        /// </summary>
        /// <param name="param">null</param>
        private void FixSeries(object param)
        {
            if (Logics.FixSeries(FolderPath))
                BrodcastMessage?.Invoke("בוצע בהצלחה");
            else
                BrodcastMessage?.Invoke("שגיאה");
        }

        /// <summary>
        /// Fix Encoding
        /// </summary>
        /// <param name="param">null</param>
        private void FixEncoding(object param)
        {
            // select correct path (folder or file)
            var path = SelectionStatus ? FilePath : FolderPath;
            if (Logics.FixEncoding(path, SelectionStatus))
                BrodcastMessage?.Invoke("בוצע בהצלחה");
            else
                BrodcastMessage?.Invoke("שגיאה");
        }

        #endregion

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
