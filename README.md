# Will Framework
### 简介
这是一个用于 Unity 应用开发的 C# 语言类极简 MVC 架构 ── WillFrameworkPro。
WillFrameworkPro 体量小巧， 支持 IOC 单例自动注册、单例依赖注入以及注入权限管理，拥有各种开箱即用的 Attributes。
WillFramework 的底层也实现了一个事件容器，实现了注册，调用和注销的整套流程，用户无需操心事件没有及时回收带来的内存泄露问题。
### 使用方式
#### 开始使用
用户需要创建一个 C# 脚本，定义一个类型，例如：GameApplication，让其继承框架的 BaseApplication。
```
    public class GameApplication : BaseApplication
    {
        
    }
```
BaseApplication 继承了 Unity 的 MonoBehavior，因此需要将其挂靠在 Unity 的一个 gameObject 作为脚本组件。然后启动 Unity，当在控制台看到以下这种提示时，说明 WillFrameworkPro 已成功接入：

`WillFrameworkPro Context 执行完毕, 用时:  ms`
#### 单例注册
WillFrameworkPro 提供了几种用在类型上面的 Attribute，分别是：General，Model，Service，Controller。这几种 Attribute 本质上毫无区别，都能被框架识别，只是为了强语义化而在名称上做了区分而已。若无特殊要求，用户直接使用 General 即可。
```
    [General] //标注 General，意味着 MyObject 接受框架管理
    public class MyObject
    {
        
    }
```
#### 依赖注入
依赖注入仅支持受框架托管的类型。用户无需关心对象的创建以及是不是单例的问题，框架会自动创建对象，且默认是一个单例。
```
    [General]
    public class MyObject
    {
    
    }
    [General]
    public class MyOtherObject
    {
        [Inject] // 通过添加 Inject Attribute 在字段上实现依赖注入
        private MyObject myObject; 
    }
```
#### 事件定义与事件参数
事件参数类型的定义非常重要，框架底层依靠类型的定义来区别具体某一个事件订阅。用户需要创建一个类型来继承框架提供的 ICommand 接口，强烈推荐使用 struct 类型，同时可以提供事件的传递参数：
```
    public enum Gender {Male,Female}
    
    public struct Employee : ICommand { // 必须继承 ICommand 接口
        public string Name {get;set;}
        public Gender Gender {get;set;}
        public int Age {get;set;}
    }
```
#### 事件调用与自动事件注销
BaseView 是一个 MonoBehavior 扩展类型，底层除了能注入依赖，还接入了事件调用和自动事件注销的功能。因此，用户为了获得这些能力，需要用 BaseView 来取代 Unity 的 MonoBehavior。
```
    public class MyUnityScript : BaseView
    {
        [Inject]
        private MyObject myObject;
        
        private void RegisterEmployee() {
            this.InvokeCommand(new Employee() {Name="Mike",Gender=Gender.Male,Age=30}); //使用 this.InvokeCommand 调用某一事件
        }
    }
```
#### 事件监听
框架提供了用在方法上面的 Listener Attribute，能够监听到事件调用者的调用以及传递过来参数：
```
    public class MyUnityScript01 : BaseView
    {
        [Listener]
        private void PrintEmployee(Employee e) { // 参数类型，必须提供
            Debug.Log(e.Name);
            Debug.Log(e.Gender);
            Debug.Log(e.Age);
        } 
    }
    [General]
    public class MyObject
    {
        [Listener]
        private void PrintEmployeeInOneLine(Employee e) { // 参数类型，必须提供
            Debug.Log(e.Name + e.Gender + e.Age);
        } 
    }
```
#### CommandManager
CommandManager 是一个框架中受 Ioc 容器托管的内置对象，作为工具类被使用。这是为了适配用户自定义受托管类型（不继承 BaseView，属于非 Unity 对象）的情况下无法使用`this.InvokeCommand`方法来进行事件调用的情况：
 ```
    [General]
    public class MyObject
    {
        [Inject]
        private CommandManager commandManager; //引入 CommandManager
        
        private void RegisterEmployee() {
            commandManager.InvokeCommand(new Employee() {Name="Mike",Gender=Gender.Male,Age=30}); //使用 commandManager.InvokeCommand 调用某一事件
        }
    }
```   
