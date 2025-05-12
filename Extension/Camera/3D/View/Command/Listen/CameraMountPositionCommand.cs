using UnityEngine;
using WillFrameworkPro.Core.CommandManager;

namespace WillFrameworkPro.Extension.Camera._3D.View.Command.Listen
{
    public struct CameraMountPositionCommand : ICommand
    {
        public Vector3 Position { get; set; }
    }
}