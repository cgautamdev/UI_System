using UnityEditor;
using System;
using System.Runtime.InteropServices;

[InitializeOnLoad]
public static class UnityAutoHideTitleBar
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int GWL_STYLE = -16;
    private const int WS_CAPTION = 0x00C00000;

    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_FRAMECHANGED = 0x0020;

    private static bool isTitleHidden = false;

    // This constructor runs automatically right when the Unity Editor loads the project
    static UnityAutoHideTitleBar()
    {
        EditorApplication.delayCall += InitialHide;
    }

    private static void InitialHide()
    {
        IntPtr hWnd = GetActiveWindow();
        if(hWnd == IntPtr.Zero) return;

        int style = GetWindowLong(hWnd, GWL_STYLE);
        style &= ~WS_CAPTION; // Strip away the title bar frame
        SetWindowLong(hWnd, GWL_STYLE, style);

        isTitleHidden = true;

        // Force Windows OS to refresh the layout frames
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }

    // Safety Hotkey: Just in case you ever need to temporarily minimize, move, or close the window
    [MenuItem("View/Toggle Top Title Bar _F10")]
    public static void ToggleTitleBar()
    {
        IntPtr hWnd = GetActiveWindow();
        if(hWnd == IntPtr.Zero) return;

        int style = GetWindowLong(hWnd, GWL_STYLE);

        if(!isTitleHidden) {
            style &= ~WS_CAPTION;
            SetWindowLong(hWnd, GWL_STYLE, style);
            isTitleHidden = true;
        }
        else {
            style |= WS_CAPTION;
            SetWindowLong(hWnd, GWL_STYLE, style);
            isTitleHidden = false;
        }

        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    }
}
