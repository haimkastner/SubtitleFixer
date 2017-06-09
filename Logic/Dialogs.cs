using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
    /// <summary>
    /// This class gives API to give user WIN UI dialogs to select files and folders 
    /// </summary>
    public static class Dialogs
    {
        #region Data Members

        private static string GeneratedSubsTypeForDialog { get; set; }

        #endregion

        #region Ctor

        static Dialogs()
        {
            GeneratedSubsTypeForDialog = GeneratTypesStringToDialog();
        }

        #endregion

        #region API

        /// <summary>
        /// Dialog to select folder
        /// </summary>
        /// <returns>selected folder path</returns>
        public static string SelectFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = ConfigHandle.FolderPath;

            // show dialog and if all ok...
            if (System.Windows.Forms.DialogResult.OK == folderBrowserDialog.ShowDialog())
                return folderBrowserDialog.SelectedPath;

            return ConfigHandle.FolderPath;
        }

        /// <summary>
        /// Dialog to select file
        /// </summary>
        /// <returns>selected file path</returns>
        public static string SelectFile()
        {
            Microsoft.Win32.OpenFileDialog FileDialog = new Microsoft.Win32.OpenFileDialog();
            FileDialog.Title = "Open Subtitle File";
            FileDialog.Filter = $"Subtitle file |{GeneratedSubsTypeForDialog}";

            var resoltStatus = FileDialog.ShowDialog();

            if ((bool)resoltStatus)
                return FileDialog.FileName;

            return ConfigHandle.FilePath;
        }

        #endregion

        #region General 

        /// <summary>
        /// Generat string of all subtitles type for winApi32 dialog
        /// format looks like: *.sub1;*.sub2;*.sub3;
        /// </summary>
        /// <returns>string generated</returns>
        private static string GeneratTypesStringToDialog()
        {
            string output = "";
            bool isFirst = true;
            foreach (string currType in ConfigHandle.SubtitleTypes)
            {
                if (!isFirst)
                {
                    output += $";*{currType}";                 
                }
                else
                {
                    output = $"*{currType}";
                    isFirst = false;
                }
            }

            return output;
        }

        #endregion
    }
}
