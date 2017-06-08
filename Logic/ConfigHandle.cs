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
    public static class ConfigHandle
    {
        /// <summary>
        /// all extention that belong to movie list (data is from configuration files)
        /// </summary>
        public static List<string> MovieTypes { get; private set; }
        /// <summary>
        /// all extention that belong to subtitle list (data is from configuration files)
        /// </summary>
        public static List<string> SubtitleTypes { get; private set; }


        private static string _folderPath = "";
        public static string FolderPath
        {
            get
            {
                return _folderPath != "" ? _folderPath : _folderPath = GetPath("Folder");
            }
            set
            {
                _folderPath = value;
                SetPath("Folder", value);
            }
        }

        private static string _filePath = "";
        public static string FilePath
        {
            get
            {
                return _filePath != "" ? _filePath : _filePath = GetPath("File");
            }
            set
            {
                _filePath = value;
                SetPath("File", value);
            }
        }

        private static string RootPath { get; set; }

        static ConfigHandle()
        {
            RootPath = AppDomain.CurrentDomain.BaseDirectory;
            InitStaticLists();
        }

        private static void InitStaticLists()
        {
            MovieTypes = GetListFromXmlString("/extensions/movies/movie", "name", Path.Combine(RootPath, "Config//Extentions.xml"));
            SubtitleTypes = GetListFromXmlString("/extensions/subtitles/subtitle", "name", Path.Combine(RootPath, "Config//Extentions.xml"));
        }

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
        /// from string of xml data (we parse it to xml docoment)
        /// by getting path of node and node name (like "/superNode/nodesName")
        /// then for every node we add to list value of attrbute 
        /// </summary>
        /// <param name="nodeAndNativ">Node path(native) and nodes to look name</param>
        /// <param name="attrbuteName">attrebute to get value from</param>
        /// <param name="XmlStringToPars">xml string to pars and read</param>
        /// <returns></returns>
        private static List<string> GetListFromXmlString(string nodeAndNativ, string attrbuteName, string XmlPath)
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
    }
}
