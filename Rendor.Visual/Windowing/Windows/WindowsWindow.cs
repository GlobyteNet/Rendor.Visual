using Rendor.Visual.Drawing;
﻿using Rendor.Visual.Rendering;
using Rendor.Visual.Rendering.OpenGL;
using System.Runtime.InteropServices;

namespace Rendor.Visual.Windowing.Windows;

public partial class WindowsWindow : NativeWindow
{
    private WndProc _wndProc;

    static WindowsWindow()
    {
        Win32.SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitorDpiAware);
    }

    public WindowsWindow()
    {
        // create delegate and store it in a field to prevent it from being garbage collected
        _wndProc = WindowProc;

        WNDCLASS wc = new WNDCLASS
        {
            style = CS_HREDRAW | CS_VREDRAW,
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProc),
            hInstance = Marshal.GetHINSTANCE(typeof(Program).Module),
            hbrBackground = nint.Zero,
            lpszClassName = "OpenGLWindowClass"
        };

        Win32.RegisterClass(ref wc);

        hwnd = Win32.CreateWindowEx(
            WS_EX_APPWINDOW, wc.lpszClassName, "OpenGL Window",
            WS_OVERLAPPEDWINDOW | WS_VISIBLE | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            100, 100, 800, 600,
            nint.Zero, nint.Zero, wc.hInstance, nint.Zero
        );

        hdc = Win32.GetDC(hwnd);

        PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR
        {
            nSize = (ushort)Marshal.SizeOf(typeof(PIXELFORMATDESCRIPTOR)),
            nVersion = 1,
            dwFlags = (uint)(DWFlags.PFD_DRAW_TO_WINDOW | DWFlags.PFD_SUPPORT_OPENGL | DWFlags.PFD_DOUBLEBUFFER),
            iPixelType = (byte)IPixelType.PFD_TYPE_RGBA,
            cColorBits = 32,
            cDepthBits = 24,
            cStencilBits = 8
        };

        int pixelFormat = Win32.ChoosePixelFormat(hdc, ref pfd);
        Win32.SetPixelFormat(hdc, pixelFormat, ref pfd);

        Win32.GetClientRect(hwnd, out RECT rect);
        Width = rect.Right;
        Height = rect.Bottom;
    }

    public override void Show()
    {
        Win32.ShowWindow(hwnd, SW_SHOWNORMAL);
        Win32.UpdateWindow(hwnd);
    }

    public override bool PollEvents()
    {
        bool hasEvents = false;

        while (Win32.PeekMessage(out MSG msg, nint.Zero, 0, 0, 1))
        {
            hasEvents = true;
            Console.WriteLine(msg.message);
            Win32.TranslateMessage(ref msg);
            Win32.DispatchMessage(ref msg);
        }

        return hasEvents;
    }

    public override void SwapBuffers()
    {
        GraphicsDevice.Render(Surface);
        Win32.SwapBuffers(hdc);
    }

    public override void Dispose()
    {
        Win32.ReleaseDC(hwnd, hdc);
        GraphicsDevice.Dispose();
    }

    public override void Invalidate()
    {
        Win32.InvalidateRect(hwnd, IntPtr.Zero, false);
    }

    private nint hwnd;
    internal nint hdc;

    nint WindowProc(nint hWnd, uint uMsg, nint wParam, nint lParam)
    {
        switch (uMsg)
        {
            case WM_CLOSE:
                PostQuitMessage(0);
                return nint.Zero;
            case WM_PAINT:
                Win32.BeginPaint(hWnd, out PAINTSTRUCT ps);
                OnPaint?.Invoke();
                Win32.EndPaint(hWnd, ref ps);
                return IntPtr.Zero;
            default:
                return Win32.DefWindowProc(hWnd, uMsg, wParam, lParam);
        }
    }

    void PostQuitMessage(int exitCode)
    {
        Win32.DestroyWindow(hwnd);
        _isVisible = false;
    }

    public override bool IsVisible => _isVisible;

    public override GraphicsDevice GraphicsDevice
    {
        get => graphicsdevice;
        protected set
        {
            graphicsdevice = value;
            graphicsdevice.ScreenResolution = new Vector2f(Width, Height);
        }
    }
    private GraphicsDevice graphicsdevice;

    private bool _isVisible = true;

    const uint CS_HREDRAW = 0x0002;
    const uint CS_VREDRAW = 0x0001;
    const uint WM_CLOSE = 0x0010;
    const uint WM_PAINT = 0x000F;
    const uint WS_OVERLAPPEDWINDOW = 0x00CF;
    const uint WS_VISIBLE = 0x10000000;
    const uint WS_SYSMENU = 0x00080000;
    const uint WS_MINIMIZEBOX = 0x00020000;
    const uint WS_MAXIMIZEBOX = 0x00010000;
    const uint WS_CAPTION = 0x00C00000;
    const uint WS_VSCROLL = 0x00200000;
    const uint WS_EX_APPWINDOW = 0x40000;
    const int SW_SHOWNORMAL = 1;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate nint WndProc(nint hWnd, uint uMsg, nint wParam, nint lParam);
}
