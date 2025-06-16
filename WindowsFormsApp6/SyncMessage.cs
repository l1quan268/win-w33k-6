using System.Collections.Generic;
using System;

[Serializable]
public class SyncMessage
{
    public enum ActionType { FullSync, Draw, Delete }
    public ActionType Type { get; set; }
    public Stroke StrokeData { get; set; }
    public List<Stroke> AllStrokes { get; set; }
    public Guid StrokeIdToDelete { get; set; }
}
