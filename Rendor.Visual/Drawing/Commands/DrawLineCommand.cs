namespace Rendor.Visual.Drawing.Commands
{
    internal class DrawLineCommand : DrawCommand
    {
        public override DrawCommandType Type => DrawCommandType.Line;

        public List<Line> Lines { get; set; } = new();
    }
}
