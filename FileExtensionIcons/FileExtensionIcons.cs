using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileExtensionIcons
{
    internal static class FileExtensionIcons
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;


        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbSizeFileInfo,
            uint uFlags);


        /// <summary>
        /// Get the icon for a given extension
        /// </summary>
        /// <param name="fileExtension">eg "zip" or ".zip"</param>
        /// <param name="largeIcon">Controls the size of the resulting icon. Large => 32x32; Small => 16x16</param>
        /// <returns>An icon associated with the extension. Upon an error, null may be returned.</returns>
        internal static Bitmap? GetFileIcon(string fileExtension, bool largeIcon)
        {
            // Ensure the extension is of the formatted *.abc
            fileExtension = "*." + (fileExtension.StartsWith('.') ? fileExtension[1..] : fileExtension);

            uint flags = largeIcon ?
                SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES
                :
                SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;

            SHFILEINFO shinfo = new();
            IntPtr hImk = SHGetFileInfo(
                fileExtension,
                FILE_ATTRIBUTE_NORMAL,
                ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                flags);
            
            try
            {
                var icon = Icon.FromHandle(shinfo.hIcon);
                var bitmap = icon.ToBitmap();

                icon.Dispose();

                return bitmap;
            }
            catch
            {
                // This happened once during testing using: extension="*.zip". It was not reproducable.
                return null;
            }
        }
    }
}