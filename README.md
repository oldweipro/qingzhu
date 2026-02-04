<p align="center">
  <a href="https://qingzhu.oldwei.com"><img src="qingzhu/Assets/logo.png" alt="Exy mascot" width="248" /><br /></a>
</p>

# 青竹工业协议模拟器 (Qingzhu Industrial Protocol Simulator)

一款基于 WinUI 3 开发的现代化工业协议模拟与测试工具，支持多种工业通信协议的模拟、测试和调试。

## ✨ 特性

- 🎨 **现代化 UI** - 采用 WinUI 3 + Fluent Design，支持 Mica 材质效果
- 🔌 **多协议支持** - 覆盖串口、工业总线、网络通信等多种协议
- 📊 **实时监控** - 实时显示通信数据和状态信息
- 🛠️ **易于使用** - 直观的用户界面，快速配置和测试
- 💾 **配置管理** - 支持保存和加载协议配置
- 🌙 **主题切换** - 支持浅色/深色/系统主题

## 🚀 支持的协议

### 串口协议
- **串口通信** - RS232/RS485 串口模拟器

### 工业协议
- **Modbus RTU** - Modbus 串口协议，支持主站/从站模式
- **Modbus TCP** - Modbus 网络协议，支持多客户端连接

### 网络协议
- **TCP/IP** - TCP 服务器/客户端模拟
- **HTTP/REST** - HTTP 服务器与 RESTful API 模拟
- **MQTT** - MQTT 客户端/服务器模拟

### 其他协议
- **OPC UA** - OPC Unified Architecture 服务器模拟

## 🛠️ 技术栈

- **.NET 8.0** - 最新的 .NET 框架
- **WinUI 3** - Windows App SDK 1.8.260101001
- **MVVM** - CommunityToolkit.Mvvm 8.4.0
- **目标平台** - Windows 10 (19041) 及以上
- **架构支持** - x86, x64, ARM64

## 📋 系统要求

- **操作系统**: Windows 10 (版本 1903/19H1/Build 19041) 或更高版本
- **运行时**: .NET 8.0 Runtime
- **最低版本**: Windows 10 (Build 17763)

## 🎯 快速开始

### 安装与运行

1. **克隆仓库**
   ```bash
   git clone https://github.com/yourusername/qingzhu.git
   cd qingzhu
   ```

2. **使用 Visual Studio 2022**
   - 打开 `qingzhu.slnx` 解决方案文件
   - 确保已安装 "Windows 应用程序开发" 工作负载
   - 按 F5 运行项目

3. **使用 .NET CLI**
   ```bash
   cd qingzhu
   dotnet restore
   dotnet build
   dotnet run
   ```

### 发布应用

```bash
# 发布为自包含应用 (x64)
dotnet publish -c Release -r win-x64 --self-contained

# 发布为依赖框架应用
dotnet publish -c Release -r win-x64 --no-self-contained
```

## 📖 使用指南

### 串口模拟器
1. 在左侧导航栏选择 "串口模拟器"
2. 配置串口参数（端口号、波特率等）
3. 点击 "启动" 开始模拟
4. 在数据区域查看收发数据

### Modbus 协议
1. 选择 Modbus RTU 或 Modbus TCP
2. 配置从站地址和寄存器参数
3. 启动模拟器
4. 使用 Modbus 客户端工具进行连接测试

### MQTT 协议
1. 进入 MQTT 页面
2. 配置 Broker 地址和端口
3. 设置主题和消息格式
4. 连接并开始发布/订阅消息

### OPC UA 协议
1. 打开 OPC UA 页面
2. 配置服务器端点和安全策略
3. 启动 OPC UA 服务器
4. 使用 OPC UA 客户端工具连接测试

## 📂 项目结构

```
qingzhu/
├── Converters/          # XAML 值转换器
├── Models/              # 数据模型
├── ViewModels/          # MVVM 视图模型
│   ├── HomeViewModel.cs
│   ├── SerialPortViewModel.cs
│   ├── ModbusViewModel.cs
│   ├── MqttViewModel.cs
│   └── ...
├── Views/               # XAML 视图页面
│   ├── HomePage.xaml
│   ├── SerialPortPage.xaml
│   ├── ModbusPage.xaml
│   └── ...
├── MainWindow.xaml      # 主窗口
└── App.xaml             # 应用程序入口
```

## 🔧 开发说明

### 添加新协议模拟器

1. 在 `Models/` 下创建协议数据模型
2. 在 `ViewModels/` 下创建对应的 ViewModel
3. 在 `Views/` 下创建 XAML 页面
4. 在 `MainWindow.xaml` 添加导航菜单项
5. 在 `MainWindow.xaml.cs` 添加导航逻辑

### MVVM 模式

项目使用 CommunityToolkit.Mvvm 简化 MVVM 开发：
- 使用 `[ObservableProperty]` 生成属性
- 使用 `[RelayCommand]` 生成命令
- 继承 `ViewModelBase` 获取通用功能

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

## 📝 已知问题

- ARM64 平台 XAML 编译问题已修复（需移除多平台配置）
- 自定义标题栏双击最大化问题已修复

## 📄 许可证

[MIT License](LICENSE)

## 👨‍💻 作者

- 项目维护者：Qingzhu Team

## 🙏 致谢

- [WinUI 3](https://github.com/microsoft/microsoft-ui-xaml)
- [CommunityToolkit](https://github.com/CommunityToolkit)
- [.NET](https://github.com/dotnet)

---

⭐ 如果这个项目对你有帮助，欢迎 Star！
