using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rosenholz.Sniper.Libraries.Helper
{
    public static class ConfigHelper
    {
        #region Properties
        //Value whether ImgurSniper should strech over all screens or not
        public static bool AllMonitors => JsonConfig.AllMonitors;

        //The Image Format for normal Images
        public static ImageFormat ImageFormat => JsonConfig.ImageFormat;

        //Value whether ImgurSniper should open the uploaded Image in Browser after upload
        public static bool OpenBrowserAfterUpload => JsonConfig.OpenBrowserAfterUpload;

        //Value whether ImgurSniper should open the saved Image in Windows Explorer after upload
        public static bool OpenFileAfterSnap => JsonConfig.OpenFileAfterSnap;

        //Key for ImgurSniper Image Shortcut
        public static Key ShortcutImgKey => JsonConfig.ShortcutImgKey;

        //Key for ImgurSniper GIF Shortcut
        public static Key ShortcutGifKey => JsonConfig.ShortcutGifKey;

        //Use PrintKey for ImgurSniper Shortcut?
        public static bool UsePrint => JsonConfig.UsePrint;

        //Value wether Images should be saved or not
        public static bool SaveImages => JsonConfig.SaveImages;

        //Value wether upload Images to Imgur or copy to Clipboard
        public static AfterSnipe AfterSnipeAction => JsonConfig.AfterSnipeAction;

        //Last Time, ImgurSniper checked for Updates
        public static DateTime LastChecked => JsonConfig.LastChecked;

        //Text Language
        public static string Language => JsonConfig.Language;

        //Frames per Second of GIF Capture
        public static int GifFps => JsonConfig.GifFps;

        //Maximum GIF Length in Milliseconds
        public static int GifLength => JsonConfig.GifLength;

        //Show Mouse Cursor on Screenshot
        public static bool ShowMouse => JsonConfig.ShowMouse;

        //Freeze Screen on Image Capture
        public static bool FreezeScreen => JsonConfig.FreezeScreen;

        //Value whether ImgurSniper should automatically search for Updates
        public static bool AutoUpdate => JsonConfig.AutoUpdate;

        //Quality of Image (1 being lowest, 100 being highest)
        public static long Quality => JsonConfig.Quality;
        #endregion

        #region Imgur Account

        //Does Imgur Refresh Token exist?
        public static bool TokenExists => File.Exists(TokenPath);

        //Path to Imgur User Refresh Token
        public static string TokenPath
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ImgurSniper",
                "refreshtoken.imgurtoken");


        #endregion

        public static Settings JsonConfig;

        public static string DocumentsDirectory
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Rosenholz");

        public static string ConfigFile
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Rosenholz",
                "config.json");

        //Path to Installation Folder
        public static string InstallDir => AppDomain.CurrentDomain.BaseDirectory;

        //Salt for Cipher Encryption
        private const string PassPhrase = "ImgurSniper User-Login File_PassPhrase :)";

        public static void Exists()
        {
            if (!Directory.Exists(DocumentsDirectory))
            {
                Directory.CreateDirectory(DocumentsDirectory);
            }

            if (!File.Exists(ConfigFile))
            {
                File.WriteAllText(ConfigFile, "{}");
            }
        }

        //Check for Write Access to Directory
        public static bool CanWrite(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectorySecurity acl = di.GetAccessControl(AccessControlSections.All);
            AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(NTAccount));

            //Go through the rules returned from the DirectorySecurity
            foreach (AuthorizationRule rule in rules)
            {
                //If we find one that matches the identity we are looking for
                if (rule.IdentityReference.Value.Equals(Environment.UserName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var filesystemAccessRule = (FileSystemAccessRule)rule;

                    //Cast to a FileSystemAccessRule to check for access rights
                    if ((filesystemAccessRule.FileSystemRights & FileSystemRights.WriteData) > 0 && filesystemAccessRule.AccessControlType != AccessControlType.Deny)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Action after Sniping
        /// </summary>
        public enum AfterSnipe
        {
            UploadImgur,
            CopyClipboard,
            DoNothing
        }

        public class Settings
        {
            public bool AllMonitors = true;
            public bool AutoUpdate = true;
            public bool ShowMouse = true;
            public bool FreezeScreen = false;
            public bool UsePrint = false;
            public bool OpenBrowserAfterUpload = true;
            public bool OpenFileAfterSnap = false;
            public bool SaveImages = true;

            public int GifFps = 10;
            public int GifLength = 12000;

            public long Quality = 90;

            public string Language = null;

            public AfterSnipe AfterSnipeAction = AfterSnipe.DoNothing;

            public Key ShortcutGifKey = Key.G;
            public Key ShortcutImgKey = Key.X;

            public DateTime LastChecked = DateTime.Now;

            public ImageFormat ImageFormat = ImageFormat.Png;
        }
    }
}
