# 知识点记录
## 游戏开发基本概念
### 帧同步、状态同步、帧缓冲
- [【网络同步】浅析帧同步和状态同步 - 知乎](https://zhuanlan.zhihu.com/p/357973435?utm_id=0)
- [游戏同步方案——帧同步_帧同步 帧缓冲-CSDN博客](https://blog.csdn.net/qq_41211553/article/details/106983481)
- [三种同步方式：状态同步、帧同步、状态帧同步 - 十月的大橘 - 博客园](https://www.cnblogs.com/October2018/p/16120681.html)
- 《网络同步在游戏历史中的发展变化》系列
  - [网络同步在游戏历史中的发展变化（一）—— 网络同步与网络架构](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491556&idx=1&sn=7101a907cb2d0df3d237ef0752638282&source=41&poc_token=HEBUBmaj_k-yiN0S53vC0z06rE0Iz1tPb30XLZJw)
  - [网络同步在游戏历史中的发展变化（二）—— Lockstep与帧同步](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491557&idx=1&sn=0f67e66538257d4d77ab0effe7e09fb4&chksm=c2b03bacf5c7b2ba9d3234ac13d6be453b1b65f642610ac7b55b6d9b27b1d6de02b677e84de9&cur_album_id=1742556256956923910&scene=189#wechat_redirect)
  - [网络同步在游戏历史中的发展变化（三）—— 状态同步的发展历程与基本原理（上）](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491567&idx=1&sn=7a61cdbb066b71e65a62b8a34411d50b&chksm=c2b03ba6f5c7b2b04330969a5a50c455e1d0f24465acc86a5d0d3e59b89189820dde384e96b8&cur_album_id=1742556256956923910&scene=189#wechat_redirect)
  - [网络同步在游戏历史中的发展变化（四）—— 状态同步的发展历程与基本原理（下）](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491569&idx=1&sn=043ed20e75d272a45fbb5813c3cca36b&chksm=c2b03bb8f5c7b2ae36caf98d75e10daa55668fd94d2c5f07f13793310dc23674752bb460f556&cur_album_id=1742556256956923910&scene=189#wechat_redirect)
  - [网络同步在游戏历史中的发展变化（五）—— 物理同步](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491580&idx=1&sn=8e188ed04f12dd23eef8656dd721fddc&chksm=c2b03bb5f5c7b2a3d7dfd7c75fa6e766eeb097c8d5dbb2998d35f52cc47a97459afa65436f1d&cur_album_id=1742556256956923910&scene=189#wechat_redirect)
  - [网络同步在游戏历史中的发展变化（六）—— 优化技术总结（完结篇）](https://mp.weixin.qq.com/s?__biz=MzkzNTIxMjMyNg==&mid=2247491582&idx=1&sn=7e65e449f4964b2d86deefd4b7759048&chksm=c2b03bb7f5c7b2a1a7862fc7eb544140f53410127771ed234dc33c3564428cf0132d3fe4da08&cur_album_id=1742556256956923910&scene=189#wechat_redirect)
### 3C
- [游戏开发与设计中的“3C”是指什么？](https://zhuanlan.zhihu.com/p/357621053)

## C#
### 编码规范
- [编写CSharp时最好遵守的规范](https://learn.microsoft.com/zh-cn/dotnet/csharp/fundamentals/coding-style/coding-conventions)， **每次写代码之前过来看一看**
- TODO：
  - 找个时间把上面文档里的规范看一下、精简后直接把内容总结在这儿

### LINQ
- [官方文档 (入门)](https://learn.microsoft.com/zh-tw/dotnet/csharp/linq/)
- [（知乎）小白入门级](https://zhuanlan.zhihu.com/p/146747701)

### 数组切片（Range）
> C#8 之后导入的新特性。C#8 以及之前版本的 C# 不支持  
> Unity 这边对 .NET 的支持度可以看下 [Unity的官方文档说明(2022.3)](https://docs.unity3d.com/Manual/dotnetProfileSupport.html)  
> 2020.2~2021.1 版本用的是 C#8，再往后才支持该特性
- [.NET 官方文档 ( 用作数组切片 LIKE charArr[1..3] )](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-)
- [.NET 官方文档 ( 用作分布运算符 LIKE [ .. charArr] )](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/collection-expressions#spread-element)
- [用法说明：Index型とRange型で快適に配列を扱う(Qiita)](https://qiita.com/Euglenach/items/c433afe78d72fc1a18fc)

## Unity
### 官方文档
- [Unity Docs](https://docs.unity.com/)
- [UGUI](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html)

## Lua
- [深入理解Lua虚拟机 - 可可西 - 博客园](https://www.cnblogs.com/kekec/p/11768935.html)

## 算法
### 密码学
#### AES
- [AES加密的详细过程是怎么样的？ - 知乎](https://www.zhihu.com/question/27307070)
### 数学
#### 贝塞尔(Bezier)曲线
- 实验
  - [Canvas贝塞尔曲线绘制工具](http://wx.karlew.com/canvas/bezier/)
  - [The Bézier Game：一个神奇的网站 - 知乎](https://zhuanlan.zhihu.com/p/21799678)
- 介绍
  - [贝塞尔曲线拟合原理_沈子恒的博客-CSDN博客](https://blog.csdn.net/shenziheng1/article/details/54410816)
#### 快速幂
- [快速幂 - OI Wiki](https://oi-wiki.org/math/binary-exponentiation/)
#### 矩阵
- []

## 杂项
### Windows CMD
- [给cmd命令行永久设置代理 - 知乎](https://zhuanlan.zhihu.com/p/478606447)




