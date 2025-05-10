using WillFrameworkPro.Core.Command;

namespace WillFrameworkPro.Extension.Camera._3D.View.Command.Listen
{
    public struct CameraZoomingCommand : ICommand
    {
        public float DeltaValue { get; set; }
    }
}