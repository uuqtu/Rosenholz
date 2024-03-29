﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public sealed class FolderManager
    {

        private static string _basePath => Settings.Settings.Instance.StorageBaseLocation;
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
            string notePath = Path.Combine(_basePath, _parentPrefixForAuFolder, DateTime.Now.ToString("yy"), aUName, "_notes");
            if (!Directory.Exists(notePath))
                Directory.CreateDirectory(notePath);
            string snipePath = Path.Combine(_basePath, _parentPrefixForAuFolder, DateTime.Now.ToString("yy"), aUName, "_snipes");
            if (!Directory.Exists(snipePath))
                Directory.CreateDirectory(snipePath);
        }

        public string GetAUFolder(string aUName)
        {
            var refval = new AUReference(aUName);
            string folderPath = Path.Combine(_basePath, _parentPrefixForAuFolder, refval.YearString, aUName);
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
