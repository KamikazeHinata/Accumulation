# Unity 知识点记录

## 官方文档
---
- [Unity Docs](https://docs.unity.com/)

- [UGUI](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html)

- [Unity 进阶建议（涵盖了C#建议规范、TA练习demo等）](https://docs.unity3d.com/cn/2022.3/Manual/best-practice-guides.html)


## 学习建议
---
- [CSDN-呆呆敲代码的小Y # Unity学习路线 | 知识汇总](https://xiaoy.blog.csdn.net/article/details/131460926)


## AssetBundle 相关
---
- [资产依赖](https://docs.unity3d.com/Packages/com.unity.addressables@2.0/manual/AssetDependencies.html)

- [内存管理](https://docs.unity3d.com/Packages/com.unity.addressables@1.18/manual/MemoryManagement.html)


# Unity Localization 学习
---
## 基本概念：

- **Locale**: 一个 Locale 对应一个语种
- **String Table**: 用于管理存放语种字符串内容
- **Asset Table**: 用于管理存放语种资产（音频、图片等）资源

## 资源管理

- 基于 Addressable Asset System 进行资源管理
	- 建立 Group Resolver 进行分组规则管理，如果需要默认规则外的更复杂规则，需自行新建 Group Resolver 类







