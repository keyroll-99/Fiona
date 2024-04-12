using Fiona.IDE.Components.Layout.Menu;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;

namespace Fiona.IDE.Platforms.Windows
{
    public class FolderPicker : IFolderPicker
    {
        public async Task<string> PickFolder()
        {
            WindowsFolderPicker folderPicker = new();

            // Make it work for Windows 10
            folderPicker.FileTypeFilter.Add("*");
            // Get the current window's HWND by passing in the Window object
            IntPtr hwnd = (((MauiWinUIWindow)Application.Current?.Windows[0].Handler?.PlatformView!)!).WindowHandle;
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            StorageFolder? result = await folderPicker.PickSingleFolderAsync();

            return result.Path;
        }


    }
}