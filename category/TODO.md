## 项目相关
- 为什么用 lua 做热更？比起其他热更新方案而言，lua 有什么优缺点？
  - [例子里](https://zhuanlan.zhihu.com/p/306384460) 是提到了 ILRuntime 方案与 lua 方案
- 用的具体是什么方案？xlua？toLua？还是其他方案？还是自己写的？
  - 我们项目用的是 [sLua](https://github.com/pangweiwei/slua/tree/master)
- lua 和 C# 之间是怎么交互的？
- 使用 lua 的时候需要注意什么？

## 语言基础（C#）
- 引用类型 和 值类型 的却别
- GC 算法
- 字典(Dictionary)的实现原理
- Lua 与 C# 的交互原理 （《Lua程序设计》24章-28章）
- 为什么要拆箱、装箱？
- 堆 与 栈

## 计算机组成原理（大厂爱考）
- 虚拟内存 和 物理内存
- 动态链接库 和 静态链接库
- 内存对齐
- 浮点数表示
- 多线程

## 数据结构与算法
- TopK 问题
- 排序
- **红黑树** 和 **B树** 与 **二叉搜索树**
- **数组** 和 **链表** 的复杂度分析
- 用 **栈** 实现 **队列**
- 堆排序、快速排序

## 图形学
- 渲染管线
- 空间变化矩阵
- shader 优化

## 网络协议（TCP/UDP 几乎必考）
- 比较 TCP 与 UDP
- 为什么 TCP 要三次握手、四次挥手？
- 如何实现可靠有序的 UDP

## 可参考书籍
- 《CLR via C#》
- 《Lua 程序设计》
- 《计算机组成原理》
- 《剑指offer》
- 《DirectX 9.0 3D游戏开发编程基础》
- 《Unity Shader入门精要》

---

# 一些可能的知识点，之后查一查答案
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

