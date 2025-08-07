namespace Chipsoft.Assignments.EPD.BLL;

public class OperationResult
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = [];
}