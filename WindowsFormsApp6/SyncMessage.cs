using System.Collections.Generic;
using System;
using System.Drawing;
[Serializable]
public class SyncMessage
{
    public enum ActionType { FullSync, Draw, Delete, ClientCount, Image }
    public ActionType Type { get; set; }
    public Stroke StrokeData { get; set; }
    public List<Stroke> AllStrokes { get; set; }
    public Guid StrokeIdToDelete { get; set; }
    public int ClientCount { get; set; }
    public byte[] ImageBytes { get; set; }
    public Rectangle ImageBounds { get; set; }
    public Guid ImageId { get; set; }
}
