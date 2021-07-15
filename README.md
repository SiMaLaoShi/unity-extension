# unity-extension
添加平时加的一些Unity扩展，基本上都是很简单的一些东西，主要是积累和方便以后存取。

[TOC]

## 1，Untiy扩展Svn使用

如果我们的项目是svn管理的，就基本上可以使用，方便扩展其他的命令，我这里是git，不太好展示。

![Svn工具](Gif/Svn%E5%B7%A5%E5%85%B7.gif)

### [代码路径](Assets/Lib/Editor/SVNTools)

## 2，打开方式

![快捷打开方式](Gif/%E5%BF%AB%E6%8D%B7%E6%89%93%E5%BC%80%E6%96%B9%E5%BC%8F.gif)

这个功能有时候不想用vs，就简单修改个个配置就很方便。

### [代码路径](Assets/Lib/Editor/OpenWay)

## 3，UGUI扩展（拼图专用）

![拼图扩展](Gif/%E6%8B%BC%E5%9B%BE%E6%89%A9%E5%B1%95.gif)

这个功能扩展了用 I， J， K， L，控制物体的小范围移动，还有可以覆盖UGUI自带的快捷键，创建一些列UI，我这里是重新搞了一个快捷键，没有覆盖他的

### [代码路径](Assets/Lib/Editor/UGUI)

## 4，动态设置GameView

![](Gif/%E5%8A%A8%E6%80%81%E8%AE%BE%E7%BD%AE%E5%88%86%E8%BE%A8%E7%8E%87.gif)

接口示例

```c#
GameExtension.SetGameView(1000, 1000, "测试分辨率");
```

## 5，动态设置闪屏

![](Gif/%E5%8A%A8%E6%80%81%E8%AE%BE%E7%BD%AE%E9%97%AA%E5%B1%8F.gif)

接口示例

```c#
GameExtension.SetSplashScreen("Assets/Res/Splash.png");
```

