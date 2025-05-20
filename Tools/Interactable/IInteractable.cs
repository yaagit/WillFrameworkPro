namespace WillFrameworkPro.Tools.Interactable
{
    /// <summary>
    /// player 与环境交互的接口。同样的接口，不同的具体实现（行为）。
    /// 使用接口的好处在于：替换同样实现了 IInteractable 接口的脚本 component 的时候，可以不用修改其他类的源码。
    /// </summary>
    public interface IInteractable
    {
        public void Interact();

        public void Activate(bool isActivated);
    }
}