﻿using AdvancedSharpAdbClient;
using APKInstaller.Pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Net;
using Windows.UI.Core;

namespace APKInstaller.Helpers
{
    internal static class ADBHelper
    {
        public static DeviceMonitor Monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdvancedAdbClient.AdbServerPort)));
        static ADBHelper()
        {
            Monitor.Start();
        }
    }

    internal static partial class UIHelper
    {
        public static bool IsDarkTheme(ElementTheme theme)
        {
            return theme == ElementTheme.Default ? Application.Current.RequestedTheme == ApplicationTheme.Dark : theme == ElementTheme.Dark;
        }

        public static void CheckTheme()
        {
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                bool IsDark = IsDarkTheme(SettingsHelper.Theme);

                if (IsDark)
                {
                    AppWindowTitleBar view = GetAppWindowForCurrentWindow().TitleBar;
                    view.ButtonBackgroundColor = view.InactiveBackgroundColor = view.ButtonInactiveBackgroundColor = Colors.Transparent;
                    view.ButtonForegroundColor = Colors.White;
                }
                else
                {
                    AppWindowTitleBar view = GetAppWindowForCurrentWindow().TitleBar;
                    view.ButtonBackgroundColor = view.InactiveBackgroundColor = view.ButtonInactiveBackgroundColor = Colors.Transparent;
                    view.ButtonForegroundColor = Colors.Black;
                }
            }
        }
    }

    internal static partial class UIHelper
    {
        public static MainPage MainPage;
        public static MainWindow MainWindow;

        public static void Navigate(Type pageType, NavigationTransitionInfo TransitionInfo, object e = null)
        {
            _ = (MainPage?.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _ = (MainPage?.CoreAppFrame.Navigate(pageType, e, TransitionInfo));
            }));
        }

        public static AppWindow GetAppWindowForCurrentWindow(this Window window)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }

        public static AppWindow GetAppWindowForCurrentWindow()
        {
            return MainWindow != null ? GetAppWindowForCurrentWindow(MainWindow) : null;
        }
    }

    internal static partial class UIHelper
    {
        public static string GetSizeString(this double size)
        {
            int index = 0;
            while (true)
            {
                index++;
                size /= 1024;
                if (size is > 0.7 and < 716.8) { break; }
                else if (size >= 716.8) { continue; }
                else if (size <= 0.7)
                {
                    size *= 1024;
                    index--;
                    break;
                }
            }
            string str = string.Empty;
            switch (index)
            {
                case 0: str = "B"; break;
                case 1: str = "KB"; break;
                case 2: str = "MB"; break;
                case 3: str = "GB"; break;
                case 4: str = "TB"; break;
                default:
                    break;
            }
            return $"{size:N2}{str}";
        }
    }
}
