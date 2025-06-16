using System.Collections.Generic;
using System.Drawing;
using System;

[Serializable]
public class Stroke
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Point> Points { get; set; } = new List<Point>();
    public Color Color { get; set; }
    public float Width { get; set; }
}
