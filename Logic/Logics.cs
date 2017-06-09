using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// This class gives API to fix and mend subtitel files
    /// </summary>
    public static class Logics
    {
        #region Data Members

        // regxp pattern for SxxExx pattren 
        private const string identityPattern = @"(S|s)(\d){2,2}(E|e)(\d){2,2}";

        #endregion

        #region API

        /// <summary>
        /// Fix text file/files encoding to be unicode
        /// </summary>
        /// <param name="path">path of file/files</param>
        /// <param name="IsFilePath">true if it file path and false if it for folder path</param>
        /// <returns>true if success</returns>
        public static bool FixEncoding(string path, bool IsFilePath = false)
        {
            if (IsFilePath)
                return FixFileEncoding(path);

            bool result = true;

            // get all fildes in folder
            string[] folderFiles = Directory.GetFiles(path);

            foreach (var file in folderFiles)
                result = FixFileEncoding(file);

            return result;
        }

        /// <summary>
        /// Fix subtitels name in folder of some Series
        /// </summary>
        /// <param name="folderPath">path of folder</param>
        /// <returns></returns>
        public static bool FixSeries(string folderPath)
        {
            try
            {
                // get all files
                string[] folderFiles = Directory.GetFiles(folderPath);

                FixSeries(folderFiles);

                return true;

            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

            return false;
        }

        #endregion

        #region Pathes and files handle method

        /// <summary>
        /// Get file identity format (SxxExx)
        /// </summary>
        /// <param name="filename">file path</param>
        /// <param name="identity">the found identity format</param>
        /// <returns>true if identity found</returns>
        private static bool GetFileIdentity(string filename, out string identity)
        {
            var identityRegex = Regex.Match(filename, identityPattern);
            identity = identityRegex.Value;
            return identityRegex.Success;
        }

        /// <summary>
        /// Check if current file extention in current list 
        /// </summary>
        /// <param name="extentionList">list of extentions to look in</param>
        /// <param name="path">file path</param>
        /// <returns></returns>
        private static bool IsExtentionInList(List<string> extentionList, string path)
        {
            return extentionList.Contains(Path.GetExtension(path).ToLower());
        }

        /// <summary>
        /// Check if current file is Subtitle
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>true if it Subtitle</returns>
        private static bool IsSubtitleFile(string path)
        {
            return IsExtentionInList(ConfigHandle.SubtitleTypes, path);
        }

        /// <summary>
        /// Check if current file is Movie
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>true if it Movie</returns>
        private static bool IsMovieFile(string path)
        {
            return IsExtentionInList(ConfigHandle.MovieTypes, path);
        }

        /// <summary>
        /// Get array of files path and map them to movies and subtitle files 
        /// </summary>
        /// <param name="movieFiles">movies list to load</param>
        /// <param name="subtitilsFiles">subtitle list to load</param>
        /// <param name="Files">array of files path to map</param>
        public static void MapFiles(out List<string> movieFiles, out List<string> subtitilsFiles, string[] Files)
        {
            subtitilsFiles = new List<string>();
            movieFiles = new List<string>();

            foreach (string currFile in Files)
            {
                // for every file try if extention is of movie or subtitle
                string ext = Path.GetExtension(currFile);
                if (IsSubtitleFile(currFile))
                    subtitilsFiles.Add(currFile);
                else if (IsMovieFile(currFile))
                    movieFiles.Add(currFile);
            }
        }

        #endregion

        #region Main Logics

        /// <summary>
        /// Fix the subtitile names in folder to be like episodes 
        /// </summary>
        /// <param name="allFolderFiles">array of all files in folder</param>
        private static void FixSeries(string[] allFolderFiles)
        {
            List<string> movieFiles;
            List<string> subtitilsFiles;
            MapFiles(out movieFiles, out subtitilsFiles, allFolderFiles);

            // run on every movie 
            foreach (string movie in movieFiles)
            {
                // get movie identity if you dont find identity move to next movie
                string movieIdentity = "";
                if (!GetFileIdentity(movie, out movieIdentity))
                    continue;

                // run on all substitls
                foreach (string subtitle in subtitilsFiles)
                {
                    // get subtitle identity if you dont find identity move to next subtitle
                    string subtitleIdentity = "";
                    if (!GetFileIdentity(subtitle, out subtitleIdentity))
                        continue;

                    // if the subtitle match to movie
                    if (movieIdentity == subtitleIdentity)
                    {
                        // the new subtitle name is made from 1: original path 
                        //                                    2: movie file name without extension 
                        //                                    3: original subtitle extension
                        string newSubtitleName = Path.Combine(Path.GetDirectoryName(movie),
                                                              Path.GetFileNameWithoutExtension(movie) + Path.GetExtension(subtitle));
                        // only if not alredy exist with same name create new copy of original file with new name!
                        if (!File.Exists(newSubtitleName))
                            File.Copy(subtitle, newSubtitleName);

                        // stop searching subtitle for current movie 
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Fix subtitle file by convert it from ASCII to UTF-16 (unicode)
        /// </summary>
        /// <param name="path">subtitle file path</param>
        /// <returns></returns>
        private static bool FixFileEncoding(string path)
        {
            try
            {
                if (!IsSubtitleFile(path))
                    return false;

                string data = File.ReadAllText(path, Encoding.GetEncoding("windows-1255"));
                File.WriteAllText(path, data, Encoding.Unicode);
                return true;
            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

            return false;           
        }

        #endregion
    }
}
