## 编码规范
---
- [Microsoft .NET 常见 C# 代码约定](https://learn.microsoft.com/zh-cn/dotnet/csharp/fundamentals/coding-style/coding-conventions)

- [CSDN-呆呆敲代码的小Y  C#编码规范](https://xiaoy.blog.csdn.net/article/details/127565036)


## 内存相关
---
- [托管堆内存、非托管堆内存和栈内存](https://www.51cto.com/article/771791.html)

- [垃圾回收的基本知识](https://learn.microsoft.com/zh-cn/dotnet/standard/garbage-collection/fundamentals)


## LINQ
---
- [官方文档 (入门)](https://learn.microsoft.com/zh-tw/dotnet/csharp/linq/)


## 特性 (Attribute)
---
- [[CS 特性标签]]

# 正则表达式
---
- [零宽断言](https://blog.csdn.net/yeshang_lady/article/details/121756563)
	- 定义：零宽断言是一种零宽度的匹配，它匹配到的内容不会保存到匹配结果中去，最终匹配结果只是一个位置而已
	- 常用的 **断言元字符** ：`^` 和 `$` 和 `\b`
	- 常用的零宽断言：
		- (?=exp)  正向零宽先行断言: *目标字符出现的位置的**右边**必须匹配到exp这个表达式*
			- e.g.  
			- `pattern = "[a-zA-Z]+(?=\d{3})" // 匹配后面跟着三个数字的字母`
			- `str = "3ab_cd100ef1gh900" // OK, "cd"(4,6) matched`
			- `str = "abd111efg111" // OK, "abd"(0,3) matched`

		- (?!exp)  负向零宽先行断言: *目标字符出现的位置的**右边**不能匹配到exp这个表达式*
			- e.g.
			- `pattern = "[a-zA-Z]+(?!\d{3})" // 匹配后面没有三个数字的字母`
			- `str = "3ab_cd100ef1gh900" // OK, "ab"(1,3) matched`
			- `str = "abd111efg111" // OK, "ab"(0, 2) matched`
			
		- (?<=exp)  正向零宽后发断言: *目标字符出现的位置的**左边**必须匹配到exp这个表达式*
			- e.g.
			- `pattern = "(?<=\d{3})[a-zA-Z]+" // 目标字母左边必须有三个数字`
			- `str = "3ab_cd100ef1gh900" // OK, "ef"(9,11) matched`
			- `str = "abd111efg111" // OK, "efg"(6,9) matched`
			
		- (?<!exp)  负向零宽后发断言: *目标字符出现的位置的**左边**不能匹配到exp这个表达式*
			- e.g.
			- `pattern = "(?<!\d{3})[a-zA-Z]+" // 目标字母左边不能有三个数字`
			- `str = "3ab_cd100ef1gh900" // OK, "ab" "cd" "f" "gh" matched`
			- `str = "abd111efg111" // OK, "abd" "fg" matched`
		
		- ***在使用后发断言时，自定义的断言必须有固定的宽度，比如上例的\d{3}不可以是\d{2,}这种不确定宽度***
	
## 其他
---
##### 数组切片（Range）

> C#8 之后导入的新内容。C#8 以及之前版本的 C# 不支持。Unity 这边对 .NET 的支持度可以看下 [Unity的官方文档说明(2022.3)](https://docs.unity3d.com/Manual/dotnetProfileSupport.html) ，其中 Unity 2020.2~2021.1 版本用的是 C#8，再往后的 Unity 版本才支持该特性

- [.NET 官方文档 ( 用作数组切片 LIKE charArr[1..3] )](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-)

- [.NET 官方文档 ( 用作分布运算符 LIKE [ .. charArr] )](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/collection-expressions#spread-element)

- [用法说明：Index型とRange型で快適に配列を扱う(Qiita)](https://qiita.com/Euglenach/items/c433afe78d72fc1a18fc)


# 实践
---
- ## FileStream / MemoryStream
	- FileStream 读取文件后，将数据复制给 MemoryStream 后 Close 掉，看起来会比较安全
(```)
	FileStream fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
	MemoryStream ms = new MemoryStream();
	fs.CopyTo(ms);
	fs.Close();
(```)




