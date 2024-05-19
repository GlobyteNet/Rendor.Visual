using System.Diagnostics;

namespace Rendor.Visual.Rendering.OpenGL;

internal class VertexArray : IGLObject
{
    public uint Id { get; private init; }
    public GLObjectType Type => GLObjectType.VertexArray;

    private readonly List<VertexAttribute> Attributes = new();
    private bool Built = false;

    public VertexArray()
    {
        Id = GL.GenVertexArray();
    }

    public void AddPoint1f(GLBuffer buffer, uint index, int stride = 0, int offset = 0, int divisor = -1)
    {
        Attributes.Add(new VertexAttribute
        {
            Buffer = buffer,
            Index = index,
            Size = 1,
            Type = DataType.Float,
            Stride = stride,
            Offset = offset,
            Divisor = divisor
        });
    }

    public void AddPoint2f(GLBuffer buffer, uint index, int stride = 0, int offset = 0, int divisor = -1)
    {
        Attributes.Add(new VertexAttribute
        {
            Buffer = buffer,
            Index = index,
            Size = 2,
            Type = DataType.Float,
            Stride = stride,
            Offset = offset,
            Divisor = divisor
        });
    }

    public void AddPoint3f(GLBuffer buffer, uint index, int stride = 0, int offset = 0, int divisor = -1)
    {
        Attributes.Add(new VertexAttribute
        {
            Buffer = buffer,
            Index = index,
            Size = 3,
            Type = DataType.Float,
            Stride = stride,
            Offset = offset,
            Divisor = divisor
        });
    }

    public void AddPoint4f(GLBuffer buffer, uint index, int stride = 0, int offset = 0, int divisor = -1)
    {
        Attributes.Add(new VertexAttribute
        {
            Buffer = buffer,
            Index = index,
            Size = 4,
            Type = DataType.Float,
            Stride = stride,
            Offset = offset,
            Divisor = divisor
        });
    }

    public void Build()
    {
        GL.BindVertexArray(Id);

        foreach (var attribute in Attributes)
        {
            var (buffer, index, size, type, stride, offset, divisor) = attribute;

            buffer.Bind(BufferTarget.ArrayBuffer);
            GL.VertexAttribPointer(index, size, type, false, stride, offset);
            GL.EnableVertexAttribArray(index);

            if (divisor != -1)
            {
                GL.VertexAttribDivisor(index, (uint)divisor);
            }
        }

        GL.BindVertexArray(0);

        Built = true;
    }

    public void Bind()
    {
        Debug.Assert(Built, "VertexArray not built");
        GL.BindVertexArray(Id);
    }

    public void Unbind()
    {
        Debug.Assert(Built, "VertexArray not built");
        GL.BindVertexArray(0);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(Id);
    }

    private struct VertexAttribute
    {
        public VertexAttribute(GLBuffer buffer, uint index, int size, DataType type, int stride, int offset)
        {
            Buffer = buffer;
            Index = index;
            Size = size;
            Type = type;
            Stride = stride;
            Offset = offset;
        }

        public GLBuffer Buffer { get; set; }

        public uint Index { get; set; }

        public int Size { get; set; }

        public DataType Type { get; set; }

        public int Stride { get; set; }

        public int Offset { get; set; }

        public int Divisor { get; set; } = 1;

        //deconstruct

        public void Deconstruct(out GLBuffer buffer, out uint index, out int size, out DataType type, out int stride, out int offset, out int divisor)
        {
            buffer = Buffer;
            index = Index;
            size = Size;
            type = Type;
            stride = Stride;
            offset = Offset;
            divisor = Divisor;
        }
    }
}
