using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
    public static class Dialogs
    {
        public static string SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = ConfigHandle.FolderPath;

            // show dialog and if all ok...
            if (System.Windows.Forms.DialogResult.OK == folderBrowserDialog.ShowDialog())
            {
                return folderBrowserDialog.SelectedPath;
                //General.SetFolderSelectedConfig(folderPath);
                
            }

            return "";
        }    
    }
}
