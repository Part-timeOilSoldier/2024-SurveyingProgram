<h1 align="center"><em>📓 2024-SurveyingProgram</em>: Personal Repository for the Surveying and Mapping Programming Design Competition</h1>

<div align="center">
  <p>
    <a href="https://github.com/Part-timeOilSoldier">Part-timeOilSoldier</a>
  </p>
</div>

<p align="center">
  <a href="https://github.com/casterbn/Geomatics-Program">
    <img src="https://img.shields.io/badge/测绘程序设计-Surveying and mapping-red" />
  </a>
  <a href="https://en.wikipedia.org/wiki/Windows_Forms">
    <img src="https://img.shields.io/badge/C%23-WinForms-blue" />
  </a>
</p>

<div align="center">
<a href="https://smt.whu.edu.cn/sshd/dxscxcyznds.htm">
  教育部高等学校测绘类专业教学指导委员会通知官网（比赛通知、结果发布地址）
</a>  
</div>

## 🍻 概述
**2024-SurveyingProgram** 是我参加2024年度“测绘程序设计竞赛”期间的代码合集，主要用于整理、复现与学习竞赛相关内容。
仓库按赛程进行分类，便于快速定位到对应题目与程序示例。  
目前包含赛题有：**2024年国赛、2024省赛、2024第一/二/三/四/五场国赛预赛**  

## 竞赛文件组成详解(以2024国赛为例)
2024 年国赛项目文件结构遵循测绘程序设计竞赛官方规范，覆盖数据输入、算法实现、结果输出和报告生成的全流程，以下为仓库主要组成部分说明。

### 一、总体目录结构📄

```text
2024_国赛/
│
├── 源码文件/                                  # 程序源码
│   ├── SpeaceAnalyse/                         # 解决方案文件夹
│   │   ├── SpeaceAnalyseApp1/                 # 主程序文件夹
│   │   │   ├── Lib/                           # ⭐核心计算文件夹
│   │   │   │   ├── Calculate.cs               # 空间分析与统计计算核心
│   │   │   │   ├── DataCenter.cs              # 数据中心类与结构体定义
│   │   │   │   └── FileHelp.cs                # 文件读写与报告生成
│   │   │   ├── Form1.cs                       # 主窗体逻辑（程序界面）
│   │   │   └── Program.cs                     # 程序主入口
│   │   └── SpeaceAnalyse.sln                  # VS解决方案文件
│   ├── SpeaceAnalyseApp1.exe                  # 可执行程序
│   └── result.txt                             # 程序运行结果输出
│
├── 报告文档.pdf                               # 竞赛报告文件（程序说明）
├── 空间数据探索性分析.pdf                      # 数据探索性分析报告（试题）
└── 正式数据.txt                               # 输入数据文件（原始测试数据）
```

### 二、核心代码模块说明

程序核心计算模块存储在Lib文件夹中，下面对核心代码进行简单解析

#### 🔧 1. `Calculate.cs`

实现全部的计算逻辑，是竞赛算法的核心部分。类的构造函数 `Calculate(DataCenter dataCenter)` 自动执行全部函数，实现“一键计算”。

| 函数名 | 功能描述 |
|--------|-----------|
| `getarea()` | 根据点的 `AreaCode` 将数据分配至七个区域 |
| `caavgcenter()` | 计算所有事件点的平均中心 |
| `cabiaozhuncha()` | 计算偏移量 a、b |
| `cafuzhu()` | 计算辅助量 A、B、C |
| `cacanshu()` | 计算椭圆旋转角与标准差椭圆长短轴（SDEx、SDEy） |
| `caareacenter()` | 计算各区域的平均中心 |
| `caquan()` | 计算区域间空间权重矩阵（基于平均中心距离） |
| `zhengli()` | 统计各区域事件数量，计算全区平均犯罪数 X |
| `caquanmo()` | 计算全局莫兰指数 I |
| `cajubumo()` | 计算各区域局部莫兰指数 Ii |
| `caZ()` | 计算局部莫兰指数的 Z 得分 |

---

#### ⚙️ 2. `DataCenter.cs`

定义了程序所需的数据结构

- `SAPoint`：存储单个事件点的 ID、X、Y、AreaCode、偏移量；
- `AreaSA`：存储单个区域的编号、点集、平均中心、局部指标；
- `Ell`：存储椭圆参数（A、B、C、旋转角、SDEx、SDEy）；
- `DataCenter`：全局数据类，包含：
  - 所有点集与区域；
  - 椭圆参数；
  - 权重矩阵；
  - 全局与局部莫兰指数结果；
  - 统计量（平均数 μ、标准差 σ 等）。

---

#### 📂 3. `FileHelp.cs`

实现对正式比赛数据的输入与计算结果的输出

| 函数 | 功能说明 |
|------|-----------|
| `readfile()` | 读取数据文件（如 `.txt`），解析点 ID、X、Y、区域号并加载到表格控件 |
| `savefile()` | 将计算报告另存为文本文件 |
| `updaterich()` | 将所有计算结果写入报告控件，包括椭圆参数、莫兰指数、权重矩阵、各区 Z 值等 |

> 仓库中代码计算结果不完全正确，如国赛计算结果部分数值存在问题，代码仅供参考
