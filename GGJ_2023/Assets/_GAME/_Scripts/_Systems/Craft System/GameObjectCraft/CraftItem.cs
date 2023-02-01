using System;
public class CraftItem : ICloneable
{
    public string Name { get; }

    public CraftItem(string name) {
        Name = name;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
