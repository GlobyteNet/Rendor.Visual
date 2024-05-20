namespace Rendor.Visual.Drawing.Commands;

internal class DrawMeshCommand : DrawCommand
{
    public override DrawCommandType Type => DrawCommandType.Mesh;

    public List<ColorPoint> Meshes { get; set; } = new();
}
