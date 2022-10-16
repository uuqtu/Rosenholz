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

        public string AppBaseLocation => AppDomain.CurrentDomain.BaseDirectory;

        public string Position => ini.IniReadValue("Admin", nameof(Position));
        public string StorageBaseLocation => ini.IniReadValue("Storage", nameof(StorageBaseLocation));
        public string F22SubLocation => ini.IniReadValue("FilePath", nameof(F22SubLocation));
        public string F16SubLocation => ini.IniReadValue("FilePath", nameof(F16SubLocation));
        public string TaskSubLocation => ini.IniReadValue("FilePath", nameof(TaskSubLocation));
        public string AUSubLocation => ini.IniReadValue("FilePath", nameof(AUSubLocation));
        public string F22FileName => ini.IniReadValue("FileName", nameof(F22FileName));
        public string F16FileName => ini.IniReadValue("FileName", nameof(F16FileName));
        public string TasksFileName => ini.IniReadValue("FileName", nameof(TasksFileName));
        public string TaskItemsFileName => ini.IniReadValue("FileName", nameof(TaskItemsFileName));
        public string TaskLinkFileName => ini.IniReadValue("FileName", nameof(TaskLinkFileName));
    }
}
