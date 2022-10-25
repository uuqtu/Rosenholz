﻿using System;

namespace ScreenshotManager.Hotkeys {
  [Flags]
  public enum ModifierKeys : uint {
    None = 0,
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8,
    NoRepeat = 400
  }
}
