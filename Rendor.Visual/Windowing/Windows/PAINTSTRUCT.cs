namespace Rendor.Visual.Windowing.Windows;

unsafe struct PAINTSTRUCT
{
    public nint hdc;
    public bool fErase;
    public RECT rcPaint;
    public bool fRestore;
    public bool fIncUpdate;
    public fixed byte rgbReserved[32];
}
