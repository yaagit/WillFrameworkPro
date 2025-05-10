using UnityEngine;
using WillFrameworkPro.Core.Command;

namespace WillFrameworkPro.Extension.Camera._3D.View.Command.Listen
{
    public struct CameraMountPositionCommand : ICommand
    {
        public Vector3 Position { get; set; }
    }
}