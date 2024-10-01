using WillFrameworkPro.Runtime.Command;

namespace WillFrameworkPro.Runtime.ToolKit.Camera._3D.View.Command.Listen
{
    public struct CameraZoomingCommand : ICommand
    {
        public float DeltaValue { get; set; }
    }
}