using WillFrameworkPro.Runtime.Command;

namespace WillFrameworkPro.Runtime.ToolKit.Camera._3D.View.Command.Listen
{
    public struct CameraRotationCommand : ICommand
    {
        //摄像机环绕锚点的纵向角度的旋转方向
        public float HorizontalRoundRotationDirection { get; set; }
        //摄像机纵向环绕锚点的旋转方向
        public float VerticalRoundRotationDirection { get; set; }
    }
}