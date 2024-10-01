using UnityEngine;
using WillFrameworkPro.Runtime.Command;

namespace WillFrameworkPro.Runtime.ToolKit.Camera._3D.View.Command.Listen
{
    public struct CameraMountPositionCommand : ICommand
    {
        public Vector3 Position { get; set; }
    }
}