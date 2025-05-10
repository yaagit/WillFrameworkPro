using WillFrameworkPro.Core.Command;

namespace WillFrameworkPro.Extension.Camera._3D.View.Command.Report
{
    public struct RotatePlayerCommand : ICommand
    {
        public float YAxisRotation { get; set; }
    }
}