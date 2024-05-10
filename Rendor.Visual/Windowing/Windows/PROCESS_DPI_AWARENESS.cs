namespace Rendor.Visual.Windowing.Windows;

/// <summary>
/// Mirrors the shellscalingapi.h PROCESS_DPI_AWARENESS enum.
/// </summary>
public enum PROCESS_DPI_AWARENESS
{
    ProcessDpiUnaware = 0,
    ProcessSystemDpiAware = 1,
    ProcessPerMonitorDpiAware = 2
}
