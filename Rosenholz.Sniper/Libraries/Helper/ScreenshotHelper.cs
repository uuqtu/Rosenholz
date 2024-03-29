﻿
using Rosenholz.NotificationWindow;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using static Rosenholz.NotificationWindow.NotificationWindow;
using static Rosenholz.Sniper.Libraries.Helper.ConfigHelper;


namespace Rosenholz.Sniper.Libraries.Helper
{
    public static class ScreenshotHelper
    {
        public static async Task FinishScreenshot(Stream stream, string hwndName, string pathToSave)
        {
            try
            {
                //Set Stream Position to beginning
                stream.Position = 0;

                bool wasSaved = false;
                Action clickAction = null;

                //Config: Save Image locally?
                if (ConfigHelper.SaveImages)
                {
                    try
                    {
                        //Save File with unique name
                        long time = DateTime.Now.ToFileTimeUtc();
                        string extension = "." + ConfigHelper.ImageFormat.ToString().ToLower();
                        string fileName = $"Snipe_{time}{extension}";
                        string filePath = Path.Combine(pathToSave, "_snipes", fileName);
                        Clipboard.SetText($"![Bild](../_snipes/" + fileName + ")");
                        using (FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fstream);
                        }
                        wasSaved = true;
                        clickAction = () => { Process.Start(filePath); };

                        if (ConfigHelper.OpenFileAfterSnap)
                        {
                            //Open Explorer and Highlight Image
                            Process.Start("explorer.exe", $"/select, \"{filePath}\"");
                        }
                    }
                    catch
                    {
                        // could not save to file or start process
                    }
                }

                //Config: Upload Image to Imgur or Copy to Clipboard?
                switch (ConfigHelper.AfterSnipeAction)
                {
                    case AfterSnipe.CopyClipboard:
                        //Copy Binary Image to Clipboard
                        await ClipboardHelper.CopyImage(stream, wasSaved, false);
                        break;
                    case AfterSnipe.DoNothing:
                        //Do nothing (just save file e.g.)
                        if (wasSaved)
                            NotificationWindowShower.Show("", NotificationType.Success, true);
                        break;
                    case AfterSnipe.UploadImgur:
                        //Upload image to imgur and copy link
                        string link = await UploadImgur(stream, hwndName, false);
                        await ClipboardHelper.CopyLink(link, wasSaved);
                        break;
                }
            }
            catch (Exception ex)
            {
                await NotificationWindowShower.ShowAsync("", NotificationType.Error, ActionTroubleshoot);

                Helpers.WriteError(ex);
            }
        }

        public static async Task FinishGif(Stream stream, string hwndName, string pathToSave)
        {
            try
            {
                //Set Stream Position to beginning
                stream.Position = 0;

                bool wasSaved = false;
                Action clickAction = null;

                //Config: Save GIF locally?
                if (ConfigHelper.SaveImages)
                {
                    try
                    {
                        //Save File with unique name
                        long time = DateTime.Now.ToFileTimeUtc();
                        string filename = Path.Combine(pathToSave, $"Snipe_{time}.gif");
                        using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fstream);
                        }
                        wasSaved = true;
                        clickAction = () => { Process.Start(filename); };

                        if (ConfigHelper.OpenFileAfterSnap)
                        {
                            //Open Explorer and Highlight Image
                            Process.Start("explorer.exe", $"/select, \"{filename}\"");
                        }
                    }
                    catch
                    {
                        // could not start process
                    }
                }

                //Config: Upload GIF to Imgur or Copy to Clipboard?
                switch (ConfigHelper.AfterSnipeAction)
                {
                    case AfterSnipe.CopyClipboard:
                        //Copy Binary GIF to Clipboard
                        //TODO: Remove? GIFs cant be copied to clipboard
                        await ClipboardHelper.CopyImage(stream, wasSaved, true);
                        break;
                    case AfterSnipe.DoNothing:
                        //Do nothing (just save file e.g.)
                        if (wasSaved)
                            await NotificationWindowShower.ShowAsync("", NotificationType.Success, clickAction);
                        break;
                    case AfterSnipe.UploadImgur:
                        //Upload image to imgur and copy link
                        string link = await UploadImgur(stream, hwndName, true);
                        await ClipboardHelper.CopyLink(link, wasSaved);
                        break;
                }
            }
            catch (Exception ex)
            {
                await NotificationWindowShower.ShowAsync("", NotificationType.Error, ActionTroubleshoot);

                Helpers.WriteError(ex);
            }
        }



        private static async Task<string> UploadImgur(Stream stream, string hwndName, bool gif)
        {
            //            ImgurUploader imgur = await GetUploaderAsync();

            //#if DEBUG
            //            // ReSharper disable once ConvertToConstant.Local
            //            bool tooBig = false;
            //#else
            //            //10 MB = 10.485.760 Bytes => Imgur's max. File Size
            //            bool tooBig = stream.Length >= 10485760;
            //#endif
            //            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            //            if (tooBig)
            //            {
            //                //Could not upload to imgur
            //                await ShowNotificationAsync(gif ? strings.imgTooBigGif : strings.imgTooBig, NotificationType.Error, ActionTroubleshoot);
            //                return null;
            //            }

            //            //Progress Indicator
            //            string kb = $"{stream.Length / 1024d:0.#}";
            //            ShowNotification(string.Format(gif ? strings.uploadingGif : strings.uploading, kb), NotificationType.Progress, false);

            //            //Upload Binary
            //            return await imgur.Upload(stream, hwndName);

            return null;
        }
    }
}
