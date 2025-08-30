// THIS FILE IS PART OF Xunet.WinFormium PROJECT
// THE Xunet.WinFormium PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.WinFormium

namespace Xunet.WinFormium.Core;

using System.Runtime.InteropServices;

/// <summary>
/// 屏幕缩放DPI
/// </summary>
public partial class DpiHelper
{
    [LibraryImport("user32.dll")]
    private static partial IntPtr GetDC(nint hWnd);

    [LibraryImport("user32.dll")]
    private static partial int ReleaseDC(nint hWnd, nint hDC);

    [LibraryImport("gdi32.dll")]
    private static partial int GetDeviceCaps(nint hDC, int nIndex);

    private const int LOGPIXELSX = 88; // 水平DPI
    private const int LOGPIXELSY = 90; // 垂直DPI

    /// <summary>
    /// 获取系统DPI缩放比例(基于96DPI)
    /// </summary>
    public static float GetDpiScalingFactor()
    {
        IntPtr hDC = GetDC(IntPtr.Zero);
        int dpiX = GetDeviceCaps(hDC, LOGPIXELSX);
        _ = ReleaseDC(IntPtr.Zero, hDC);

        return dpiX / 96.0f;
    }

    /// <summary>
    /// 获取屏幕的物理DPI值
    /// </summary>
    public static (int dpiX, int dpiY) GetScreenDpi()
    {
        IntPtr hDC = GetDC(IntPtr.Zero);
        int dpiX = GetDeviceCaps(hDC, LOGPIXELSX);
        int dpiY = GetDeviceCaps(hDC, LOGPIXELSY);
        _ = ReleaseDC(IntPtr.Zero, hDC);

        return (dpiX, dpiY);
    }
}
