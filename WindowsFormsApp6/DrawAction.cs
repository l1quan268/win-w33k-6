using System;
using System.Drawing;

[Serializable]
public class DrawAction
{
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }
    public Color PenColor { get; set; }
    public float PenWidth { get; set; }

    public bool IsHello { get; set; } = false; // Gói khởi tạo
    public bool IsImage { get; set; } = false; // Dùng cho chèn ảnh
    public byte[] ImageBytes { get; set; } // Ảnh nén dạng byte[]
    public Rectangle ImageBounds { get; set; } // Vị trí hiển thị ảnh
    public int ClientCount { get; set; } = -1; // Mặc định -1 nếu không phải gói cập nhật số lượng

}
