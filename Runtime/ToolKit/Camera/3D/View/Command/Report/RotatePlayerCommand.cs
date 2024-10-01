using WillFrameworkPro.Runtime.Command;

namespace WillFrameworkPro.Runtime.ToolKit.Camera._3D.View.Command.Report
{
    public struct RotatePlayerCommand : ICommand
    {
        public float YAxisRotation { get; set; }
    }
}