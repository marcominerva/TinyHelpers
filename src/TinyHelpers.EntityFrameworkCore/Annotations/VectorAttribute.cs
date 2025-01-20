namespace System.ComponentModel.DataAnnotations.Schema;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class VectorAttribute : ColumnAttribute
{
    public VectorAttribute(int size = 1536)
    {
        TypeName = $"vector({size})";
    }

    public VectorAttribute(string name, int size = 1536) : base(name)
    {
        TypeName = $"vector({size})";
    }
}