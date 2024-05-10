using System.Runtime.InteropServices;

namespace Rendor.Visual.GUI;

internal class D2D1
{
    static D2D1()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            nint d2d1 = NativeLibrary.Load("d2d1.dll");

            CreateFactory = Marshal.GetDelegateForFunctionPointer<CreateFactoryDelegate>
                (NativeLibrary.GetExport(d2d1, "D2D1CreateFactory"));
        }
        else
        {
            throw new PlatformNotSupportedException("Direct2D is only supported on Windows.");
        }
    }

    static nint RenderTarget;
    static nint Factory;

    static bool Init(nint hwnd)
    {
        CreateFactory(D2D1_FACTORY_TYPE.SINGLE_THREADED, ref Factory);
        return true;
    }

    unsafe void Draw()
    {
    }

    delegate void BeginDrawDelegate();
    delegate void EndDrawDelegate();

    delegate nint CreateFactoryDelegate(D2D1_FACTORY_TYPE factoryType, ref nint riid);
    static CreateFactoryDelegate CreateFactory;

    //delegate void CreateHwndRenderTargetDelegate(nint factory, ref D2D1_RENDER_TARGET_PROPERTIES renderTargetProperties, ref D2D1_HWND_RENDER_TARGET_PROPERTIES hwndRenderTargetProperties, ref nint hwndRenderTarget);
}

enum D2D1_FACTORY_TYPE
{
    SINGLE_THREADED = 0,
    MULTI_THREADED = 1,
}
