using System;
using System.Drawing;

[Serializable]
public class DrawAction
{
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }
    public Color PenColor { get; set; }
    public float PenWidth { get; set; }

    public bool IsHello { get; set; } = false; 
    public bool IsImage { get; set; } = false; 
    public byte[] ImageBytes { get; set; } 
    public Rectangle ImageBounds { get; set; } 
    public int ClientCount { get; set; } = -1;

}
