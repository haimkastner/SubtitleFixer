using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
    public static class Logics
    {
        // regxp pattern for SxxExx pattren 
        private const string identityPattern = @"^(S|s)(\d){2,2}(E|e)(\d){2,2}$";

        private static string GetIdentityInSplit(string filename, char splitBy)
        {
            // run one every file name part and if it math to identity pattern return it 
            foreach (string currPart in Path.GetFileName(filename).Split(splitBy))
                if (Regex.Match(currPart, identityPattern).Success)
                    return currPart.ToLower();
            return "";
        }

        public static bool GetFileIdentity(string filename, out string identity)
        {
            // try to get identity by split to thoes characters
            foreach (char currentSplit in new List<char> { '.', '-', ',', '_', ' ', '/', '\\', '#', '@' })
            {
                identity = GetIdentityInSplit(filename, currentSplit);
                if (identity != "")
                    return true;
            }

            identity = "";
            return false;
        }

        public static bool FixSerios(string folderPath)
        {
            try
            {
                string[] folderFiles = Directory.GetFiles(folderPath);
                FixSerios(folderFiles);

                foreach (var file in folderFiles)
                    Logics.FixFileEncoding(file);

                return true;

            }
            catch (Exception)
            {

                Console.WriteLine("Error");
            }

            return false;
        }

        private static void FixSerios(string[] allFolderFiles)
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


        private static bool IsExtentionInList(List<string> extentionList, string path)
        {
            return extentionList.Contains(Path.GetExtension(path).ToLower());
        }

        private static bool IsSubtitleFile(string path)
        {
            return IsExtentionInList(ConfigHandle.SubtitleTypes, path);
        }

        public static bool IsMovieFile(string path)
        {
            return IsExtentionInList(ConfigHandle.MovieTypes, path);
        }

        public static bool FixFileEncoding(string path)
        {
            if (!IsSubtitleFile(path))
                return false;
          
            string data = File.ReadAllText(path, Encoding.GetEncoding("windows-1255"));
            File.WriteAllText(path, data, Encoding.Unicode);
            return true;
        }


        /// <summary>
        /// Get array files and map them to movies and subtitle files 
        /// </summary>
        /// <param name="movieFiles">movies list to load</param>
        /// <param name="subtitilsFiles">subtitle list to load</param>
        /// <param name="Files">array files to map</param>
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
    }
}
