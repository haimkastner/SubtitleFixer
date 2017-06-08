using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Threading;

namespace SubtitleFixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SubtitleFixerView : MetroWindow
    {
        public SubtitleFixerView()
        {
            var subtitleFixerViewModel = new SubtitleFixerViewModel();
            subtitleFixerViewModel.BrodcastMessage += SnackbarShow;
            DataContext = subtitleFixerViewModel;
            InitializeComponent();

        }


        public void SnackbarShow(string message)
        {
            Task.Factory.StartNew(() =>
            {
                
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                Snackbar.MessageQueue.Enqueue(message);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
