# 坑 & 遇到的难点
- spine 导入
  - 一般给到的文件是：.json, .atlas, .png 这三个
  - .json 有可能是 .skel.bytes, 导入时是这种情况 Unity 的 Spine 库会给报错提示，改后缀名即可
  - .atlas 在导入前后缀名最好改成 .atlas.txt 这样，不然导入有问题