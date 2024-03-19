

## 前言
这篇文章（该说是文章嘛？）主要是为了放一些平日的积累，把这个放在远端更新会方便一些

## > 速查文档
- [Markdown 基本语法](https://markdown.com.cn/basic-syntax/) 

## > 参考书籍
- 图形学
  - [Real-Time Rendering](https://www.realtimerendering.com/)

## 课程
- 图形学
  - [GAMES101](https://sites.cs.ucsb.edu/~lingqi/teaching/games101.html)

## > 参考站点
- 图形学
  - [Candycat1992的博客](https://candycat1992.github.io/)
  - [SIGGRAPH](https://www.siggraph.org/)

## ↓ 知识点记录 ↓
> 怎么做 - 为什么这么做 - 为什么不这么做
---
### C#
#### LINQ
- [官方文档](https://learn.microsoft.com/zh-tw/dotnet/csharp/linq/)
- [（知乎文章）小白入门级](https://zhuanlan.zhihu.com/p/146747701)

#### 数组切片（Range）
- 似乎是后面的 C# 版本才有，目前的 Unity 不支持
- [官方文档](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-)
- [用法说明：Index型とRange型で快適に配列を扱う(Qiita)](https://qiita.com/Euglenach/items/c433afe78d72fc1a18fc)

## 求生指南系列
闲来无事在网上逛的时候偶然看见的分享贴，希望有朝一日能成为这些作者一般的大手子
![img](https://github.com/KamikazeHinata/Accumulation/blob/main/img/4.jpg "想成为他们！")

### 系列-1：水曜日鸡桑
1. [前期准备](https://zhuanlan.zhihu.com/p/306384460)  
2. [公司选择](https://zhuanlan.zhihu.com/p/306408924)  
3. [面试总结](https://zhuanlan.zhihu.com/p/306777683)

### 系列-2: Lilien-Gamer桑
1. [2022年总结](https://zhuanlan.zhihu.com/p/554193172)
2. [2023年总结](https://zhuanlan.zhihu.com/p/632083926)
3. [2024年总结](https://zhuanlan.zhihu.com/p/680356638)
4. [番外篇](https://zhuanlan.zhihu.com/p/557133446)

## ↓ 一些可能的知识点，之后查一查答案 ↓
- UGUI合批规则
- 打断合批的情况
- position会不会影响合批
- image控件修改color属性会触发什么（答主：Drawwcall or CanvasRebuild）
- modifiedmesh的应用（=>自制色盘？
- AB分包策略 加载方式
- 动画混合树 rootmotion 以及 playable
- playable 的优势
- 特效规范
- C# dictionary底层实现（entry结构体+hashtable？）
- C# list底层实现 add和remove怎么操作的
- C# 遍历list删除元素怎么做最安全
- C# 如何交换两个即将溢出的int（答主：第三个数/不用第三个数；但，考虑位运算的方法呢？）
- 后处理？（答主：后处理的毛玻璃效果，涉及卷积）

- UGUI优化方向（合批、text少用outline[why]，层级尽量简单[why]，减少canvas rebuild）
- C#的GC实现原理？
- Unity 是如何调用代码中的 Awake、Update 这些函数的？
- 红黑树、平衡二叉树的区别？
- 快速排序原理
- 设计模式？（答主：单例、工厂、抽象工厂、观察者）
- 单例在多线程下安全吗？（答主：饿汉安全、懒汉可能出现同时实例化多个的情况[how?]）

- 状态同步、帧同步
- 快速排序、堆排序、二分查找等排序/查找算法
- UI框架以及具体某功能如何实现
- 网络同步？如何优化弱网体验
- ECS概念？
- C#：GC、闭包、拆箱装箱、Dictionary实现
- lua：GC、闭包、元表、协程、弱引用、table、table取长度以及增删如何实现
- xLua/sLua如何实现C#与lua之间的交互？交互过程中会产生GC的点？如何优化？lua获取C#对象后如何管理GC？
- UGUI合批规则？项目打图集策略？如何减少drawcall？
- float转int会丢失精度吗？
- 大数的解决方案？
- lua加密算法？
- C++虚函数
