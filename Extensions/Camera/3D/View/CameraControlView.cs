// using UnityEngine;
// using WillFrameworkPro.Core.Views;
// using WillFrameworkPro.Core.Views.Extensions;
// using WillFrameworkPro.Extension.Camera._3D.View.Command.Listen;
// using WillFrameworkPro.Extension.Camera._3D.View.Command.Report;
//
// namespace WillFrameworkPro.Extension.Camera._3D.View
// {
//     //todo 摄像机左右旋转的时候，玩家也应该跟随这个方向自旋
//     //todo 摄像机旋转的角度要做限制
//     public class CameraControlView : BaseView
//     {
//         [SerializeField] private float _horizontalRoundRotationSpeed;
//         [SerializeField] private float _verticalRoundRotationSpeed;
//         [SerializeField] private float _zommingSpeed;
//         
//         private Transform _cameraVerticalRotate; 
//         private Transform _cameraZooming; 
//         private void Start()
//         {
//             this.AddCommandListener<CameraMountPositionCommand>(OnMountPosition);
//             this.AddCommandListener<CameraRotationCommand>(OnHorizontalRotation);
//             this.AddCommandListener<CameraRotationCommand>(OnVerticalRotate);
//             
//             this.AddCommandListener<CameraZoomingCommand>(OnZooming);
//             _cameraVerticalRotate = transform.GetChild(0);
//             _cameraZooming = _cameraVerticalRotate.GetChild(0);
//         }
//         /// <summary>
//         /// 摄像机拉近拉远
//         /// </summary>
//         /// <param name="command"></param>
//         private void OnZooming(CameraZoomingCommand command)
//         {
//             _cameraZooming.transform.Translate(new Vector3(0, 0, command.DeltaValue * _zommingSpeed));
//         }
//
//         /// <summary>
//         /// 摄像机环绕锚点的旋转 - 上下
//         /// </summary>
//         /// <param name="command"></param>
//         private void OnVerticalRotate(CameraRotationCommand command)
//         {
//             _cameraVerticalRotate.Rotate(command.VerticalRoundRotationDirection * _verticalRoundRotationSpeed, 0, 0);
//             //将旋转角度限制在一定范围内
//             _cameraVerticalRotate.localEulerAngles = new Vector3(Mathf.Clamp(_cameraVerticalRotate.localEulerAngles.x, 5, 80), 0, 0);
//         }
//         /// <summary>
//         /// 摄像机环绕锚点的旋转 - 左右
//         /// </summary>
//         /// <param name="command"></param>
//         private void OnHorizontalRotation(CameraRotationCommand command)
//         {
//             float yAxisRotation = (command.HorizontalRoundRotationDirection) * _verticalRoundRotationSpeed;
//             transform.Rotate(0, yAxisRotation, 0);
//             this.InvokeCommand(new RotatePlayerCommand() {YAxisRotation = yAxisRotation});
//         }
//         /// <summary>
//         /// 实时更新摄像机锚点的位置
//         /// </summary>
//         /// <param name="command"></param>
//         private void OnMountPosition(CameraMountPositionCommand command)
//         {
//             transform.position = command.Position;
//         }
//
//         private void OnDrawGizmos()
//         {
//             //以摄像机骨架的锚点为圆心绘制的外圆，目的是方便观察旋转时的朝向
//             Gizmos.color = Color.red;
//             Gizmos.DrawWireSphere(transform.position, 15f);
//             //以摄像机骨架的锚点为圆心绘制的内圆，目的是方便观察旋转时的朝向
//             Gizmos.color = Color.blue;
//             Gizmos.DrawWireSphere(transform.position, 13f);
//         }
//     }
// }