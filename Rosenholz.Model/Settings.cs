using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Settings
{

    //MVVM-Link: https://www.tutorialspoint.com/mvvm/mvvm_first_application.htm
    //MVVM-Link: https://www.c-sharpcorner.com/UploadFile/raj1979/simple-mvvm-pattern-in-wpf/
    public sealed class Settings
    {

        private static IniFile ini = null;
        private static readonly Settings instance = new Settings();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Settings()
        {
        }

        private Settings()
        {
        }

        public static Settings Instance
        {
            get
            {
                ini = new IniFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini"));
                return instance;
            }
        }

        #region Mail
        private string _position;
        public string Position
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("Position",
                                                                nameof(Position),
                                                                "2");
                _position = iniValue;
                return _position;
            }
            set
            {
                ini.IniWriteValue("Position", nameof(Position), value);
            }
        }

        private string _f22Location;
        public string F22Location
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(F22Location),
                                                                @"C:\Users\z0035hes\Desktop\MFS\ZVK\f22.db");
                _f22Location = iniValue;
                return _f22Location;
            }
            set
            {
                ini.IniWriteValue("FilePaths", nameof(F22Location), value);
            }
        }

        private string _f16Location;
        public string F16Location
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(F16Location),
                                                                @"C:\Users\z0035hes\Desktop\MFS\ZPK\f16.db");
                _f16Location = iniValue;
                return _f16Location;
            }
            set
            {
                ini.IniWriteValue("FilePaths", nameof(F16Location), value);
            }
        }

        private string _basePath;
        public string BasePath
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(BasePath),
                                                                @"C:\Users\z0035hes\Desktop\MFS");
                _basePath = iniValue;
                return _basePath;
            }
            set
            {
                ini.IniWriteValue("FilePaths", nameof(BasePath), value);
            }
        }

        private string _fileTypeFilters;
        public string FileTypeFilters
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(FileTypeFilters),
                                                                @"*.tex|*.txt|*.exe|*.*");
                _fileTypeFilters = iniValue;
                return _fileTypeFilters;
            }
            set
            {
                ini.IniWriteValue("Filters", nameof(FileTypeFilters), value);
            }
        }

        private string _taskLocation;

        public string TaskLocation
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(TaskLocation),
                                                                @"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db");
                _taskLocation = iniValue;
                return _taskLocation;
            }
            set
            {
                ini.IniWriteValue("FilePaths", nameof(TaskLocation), value);
            }
        }

        private string _taskItemLocation;

        public string TaskItemLocation
        {
            get
            {
                string iniValue = ini.IniReadOrPreallocateValue("FilePaths",
                                                                nameof(TaskItemLocation),
                                                                @"C:\Users\z0035hes\Desktop\MFS\ZTV\taskitems.db");
                _taskItemLocation = iniValue;
                return _taskItemLocation;
            }
            set
            {
                ini.IniWriteValue("FilePaths", nameof(TaskItemLocation), value);
            }
        }




        #endregion
    }
}
