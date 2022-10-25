﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenshotManager.Hotkeys {
  public sealed class KeyboardHook : IDisposable {
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private class Window : NativeWindow, IDisposable {
      private static int WM_HOTKEY = 0x0312;

      public Window() {
        CreateHandle(new CreateParams());
      }

      protected override void WndProc(ref Message m) {
        base.WndProc(ref m);
        if (m.Msg == WM_HOTKEY) {
          Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
          ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);
          KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
        }
      }

      public event EventHandler<KeyPressedEventArgs> KeyPressed;

      public void Dispose() {
        DestroyHandle();
      }
    }

    private Window _window = new Window();
    private int _currentId;

    public KeyboardHook() {
      _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args) {
        KeyPressed?.Invoke(this, args);
      };
    }

    public void RegisterHotkey(ModifierKeys modifier, Keys key) {
      _currentId += 1;
      if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key)) {
        throw new InvalidOperationException("Couldn't register the hotkey.");
      }
    }

    public event EventHandler<KeyPressedEventArgs> KeyPressed;

    public void Dispose() {
      for (int i = _currentId; i > 0; i--) {
        UnregisterHotKey(_window.Handle, i);
      }
      _window.Dispose();
    }
  }
}
