using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Logic
{
    /// <summary>
    /// This class gives API to read and write data from xml config files
    /// </summary>
    public static class ConfigHandle
    {
        #region Data Members

        private static string _folderPath = "";
        private static string _filePath = "";

        #endregion

        #region Properties

        /// <summary>
        /// all extention that belong to movie list (data is from configuration files)
        /// </summary>
        public static List<string> MovieTypes { get; private set; }
        /// <summary>
        /// all extention that belong to subtitle list (data is from configuration files)
        /// </summary>
        public static List<string> SubtitleTypes { get; private set; }
        /// <summary>
        /// Folder Path
        /// </summary>
        public static string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                // set data member and update file
                _folderPath = value;
                SetPath("Folder", value);
            }
        }
        /// <summary>
        /// File Path
        /// </summary>
        public static string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                // set data member and update file
                _filePath = value;
                SetPath("File", value);
            }
        }
        /// <summary>
        /// Current application exe path
        /// </summary>
        private static string RootPath { get; set; }
        /// <summary>
        /// Current application config files path
        /// </summary>
        private static string ConfigPath { get; set; }
        #endregion

        #region Init

        static ConfigHandle()
        {
            RootPath = AppDomain.CurrentDomain.BaseDirectory;
            ConfigPath = Path.Combine(RootPath, "Config");
            InitStaticLists();
        }

        /// <summary>
        /// Init properties to be like conf files
        /// </summary>
        private static void InitStaticLists()
        {
            _filePath = GetPath("File");
            _folderPath = GetPath("Folder");

            MovieTypes = GetListFromXml("/extensions/movies/movie", "name", Path.Combine(ConfigPath, "Extentions.xml"));
            SubtitleTypes = GetListFromXml("/extensions/subtitles/subtitle", "name", Path.Combine(ConfigPath, "Extentions.xml"));
        }

        #endregion

        #region Xml Files read methods

        /// <summary>
        /// Set paths conf file value
        /// </summary>
        /// <param name="pathName">path node name</param>
        /// <param name="value">value to set</param>
        private static void SetPath(string pathName, string value)
        {
            try
            {
                XDocument xml = XDocument.Load(Path.Combine(RootPath, "Config//Pathes.xml"));
                (from path in xml.Element("Paths").Elements("Path")
                 where path.Attribute("Name").Value.ToString() == pathName
                 select path).FirstOrDefault().SetValue(value);
                xml.Save(Path.Combine(RootPath, "Config//Pathes.xml"));
            }
            catch (Exception)
            {

                Console.WriteLine("Read file error");

            }

        }

        /// <summary>
        /// Get value of path node
        /// </summary>
        /// <param name="pathName">path node name</param>
        /// <returns></returns>
        private static string GetPath(string pathName)
        {
            try
            {
                XDocument xml = XDocument.Load(Path.Combine(RootPath, "Config//Pathes.xml"));
                return (from path in xml.Element("Paths").Elements("Path")
                        where path.Attribute("Name").Value.ToString() == pathName
                        select path.Value).FirstOrDefault().ToString();
            }
            catch (Exception)
            {

                Console.WriteLine("Read file error");
            }

            return "";
        }

        /// <summary>
        /// get list of string
        /// from string of xml data
        /// by getting path of node and node name (like "/superNode/nodesName")
        /// then for every node we add to list value by attrebute name 
        /// </summary>
        /// <param name="nodeAndNativ">Node path(native) and nodes to look name</param>
        /// <param name="attrbuteName">attrebute to get value from</param>
        /// <param name="XmlStringToPars">xml file path</param>
        /// <returns></returns>
        private static List<string> GetListFromXml(string nodeAndNativ, string attrbuteName, string XmlPath)
        {
            List<string> list = new List<string>();
            try
            {
                XmlDocument Xml = new XmlDocument();
                Xml.Load(XmlPath);

                foreach (XmlNode xn in Xml.SelectNodes(nodeAndNativ))
                    list.Add(xn.Attributes.GetNamedItem(attrbuteName).Value);
            }
            catch
            {
                System.Windows.MessageBox.Show("error while reading xml resourses", "xml pars error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return list;
        }

        #endregion
    }
}
