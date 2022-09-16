using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public sealed class FolderManager
    {

        private static string _basePath => Settings.Settings.Instance.BasePath;
        private static readonly FolderManager instance = new FolderManager();
        private static string _parentPrefixForAuFolder = "ZAV";

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static FolderManager()
        {
        }

        private FolderManager()
        {
        }

        public static FolderManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void CreateBaseFolder()
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }


        public void CreateAUFolder(string aUName)
        {
            string folderPath = Path.Combine(_basePath, _parentPrefixForAuFolder, DateTime.Now.ToString("yy"), aUName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string archPath = Path.Combine(_basePath, _parentPrefixForAuFolder, DateTime.Now.ToString("yy"), aUName, "_archive");
            if (!Directory.Exists(archPath))
                Directory.CreateDirectory(archPath);
        }

        public string GetAUFolder(string aUName)
        {
            string folderPath = Path.Combine(_basePath, _parentPrefixForAuFolder, DateTime.Now.ToString("yy"), aUName);
            return folderPath;
        }

        public string GetRelativeAUFolderLocation(string aUName)
        {
            var refval = new AUReference(aUName);
            string folderPath = Path.Combine(_parentPrefixForAuFolder, refval.YearString, refval.AUReferenceString);
            return folderPath;
        }











    }
}
