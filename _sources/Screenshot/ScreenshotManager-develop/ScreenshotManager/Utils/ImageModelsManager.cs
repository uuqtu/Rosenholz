﻿using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenshotManager.Utils {
  public static class ImageModelsManager {
    private static ObservableCollection<ImageModel> _models = new();
    public static ObservableCollection<ImageModel> Models {
      get => _models;
      private set {
        _models = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static bool _isModelsAvailable = false;
    public static bool IsModelsAvailable {
      get => _isModelsAvailable;
      private set {
        _isModelsAvailable = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static readonly string[] AcceptedImageExtensions = new []{ ".jpg", ".jpeg", ".png" };

    public static void Add(ImageModel model) => Models.Add(model);
    public static bool Remove(ImageModel model) => Models.Remove(model);
    public static bool Contains(ImageModel model) => Models.Contains(model);

    public static void AddAll(List<ImageModel> models) {
      foreach (var model in models) {
        Models.Add(model);
      }
    }

    public static void Initialize() {
      Directory.CreateDirectory(SettingsManager.ScreenshotFolder);
      UpdateImageModelsToLocalAsync();
    }

    // FIXME: Crash here when you close the app while this is running
    public async static void UpdateImageModelsToLocalAsync() {
      await Task.Run(async () => {
        IsModelsAvailable = false;
        var modelsFromJson = Load();
        var files = GetLocalImageFiles();
        foreach (var file in files) {
          await Task.Run(() => {
            var matchedModel = modelsFromJson.FirstOrDefault(model => model.AbsolutePath == file);
            if (matchedModel == null) {
              // newly added
              var model = new ImageModel(file);
              Application.Current.Dispatcher.Invoke(() => Add(model));
            } else {
              // already exists
              var model = new ImageModel(matchedModel);
              Application.Current.Dispatcher.Invoke(() => Add(model));
            }
          });
        }
        IsModelsAvailable = true;
        Save();
      });
    }

    public static IEnumerable<string> GetLocalImageFiles() {
      var result = Enumerable.Empty<string>();
      foreach (var extension in AcceptedImageExtensions) {
        result = result.Union(Directory.GetFiles(SettingsManager.ScreenshotFolder, $"*{extension}").Where(file => file.EndsWith(extension)));
      }
      return result.OrderBy(x => x);
    }

    // Note: ImageSource is dead, thus you need to re-instantiate it using AbsolutePath.
    private static IEnumerable<ImageModel> Load() {
      if (!File.Exists(SettingsManager.ImageModelsSettingFilePath)) {
        return Enumerable.Empty<ImageModel>();
      }
      using (var reader = new StreamReader(SettingsManager.ImageModelsSettingFilePath, Encoding.UTF8)) {
        var json = reader.ReadToEnd();
        var modelsFromJson = JsonConvert.DeserializeObject<IEnumerable<ImageModel>>(json);
        return modelsFromJson;
      }
    }

    public static void Save() {
      using (var writer = new StreamWriter(SettingsManager.ImageModelsSettingFilePath, false, Encoding.UTF8)) {
        var models = new List<ImageModel>(Models);
        var json = JsonConvert.SerializeObject(models, Formatting.Indented);
        writer.WriteLine(json);
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
