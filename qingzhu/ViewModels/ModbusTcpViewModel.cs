using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyModbus;
using Microsoft.UI.Dispatching;

namespace qingzhu.ViewModels;

public partial class ModbusTcpViewModel : ViewModelBase, IDisposable
{
    private readonly DispatcherQueue _dispatcherQueue;

    private Timer? _connectionMonitorTimer;
    private ModbusServer? _modbusServer;

    [ObservableProperty] private int connectionCount;

    [ObservableProperty] private int errorCount;

    [ObservableProperty] private string ipAddress = "127.0.0.1";

    [ObservableProperty] private bool isRunning;

    [ObservableProperty] private int maxConnections = 10;

    [ObservableProperty] private int port = 502;

    [ObservableProperty] private int registerCount = 2000; // 添加可配置属性

    [ObservableProperty] private int requestCount;

    [ObservableProperty] private string requestLog = string.Empty;

    [ObservableProperty] private int slaveId = 1;

    [ObservableProperty] private string status = "已停止";

    public ModbusTcpViewModel()
    {
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        InitializeRegisters();
    }

    public ObservableCollection<ClientConnection> ConnectedClients { get; } = new();
    public ObservableCollection<RegisterData> HoldingRegisters { get; } = new();
    public ObservableCollection<RegisterData> InputRegisters { get; } = new();
    public ObservableCollection<CoilData> Coils { get; } = new();
    public ObservableCollection<CoilData> DiscreteInputs { get; } = new();

    public void Dispose()
    {
        _connectionMonitorTimer?.Dispose();

        if (IsRunning) StopServer();
    }

    // 当 RegisterCount 属性改变时重新初始化寄存器
    partial void OnRegisterCountChanged(int value)
    {
        // 只有在服务器未运行时才允许修改
        if (!IsRunning)
        {
            InitializeRegisters();
            AddLog($"寄存器数量已更新为: {value}");
        }
    }

    private void InitializeRegisters()
    {
        HoldingRegisters.Clear();
        InputRegisters.Clear();
        Coils.Clear();
        DiscreteInputs.Clear();

        for (var i = 0; i < RegisterCount; i++) // 使用可配置的值
        {
            HoldingRegisters.Add(new RegisterData { Address = 40001 + i, Value = 0 });
            InputRegisters.Add(new RegisterData { Address = 30001 + i, Value = 0 });
            Coils.Add(new CoilData { Address = 1 + i, Value = false });
            DiscreteInputs.Add(new CoilData { Address = 10001 + i, Value = false });
        }
    }

    [RelayCommand]
    private void StartServer()
    {
        try
        {
            _modbusServer = new ModbusServer
            {
                Port = Port
            };

            _modbusServer.CoilsChanged += OnCoilsChanged;
            _modbusServer.HoldingRegistersChanged += OnHoldingRegistersChanged;

            _modbusServer.Listen();

            IsRunning = true;
            Status = "运行中";
            AddLog($"Modbus TCP 服务器已启动 - {IpAddress}:{Port}");

            // 启动连接监控定时器
            _connectionMonitorTimer = new Timer(
                UpdateConnectionStatus,
                null,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2)
            );
        }
        catch (Exception ex)
        {
            Status = "启动失败";
            AddLog($"错误: {ex.Message}");
            ErrorCount++;
        }
    }

    [RelayCommand]
    private void StopServer()
    {
        try
        {
            // 停止监控定时器
            _connectionMonitorTimer?.Dispose();
            _connectionMonitorTimer = null;

            if (_modbusServer != null)
            {
                _modbusServer.CoilsChanged -= OnCoilsChanged;
                _modbusServer.HoldingRegistersChanged -= OnHoldingRegistersChanged;
                _modbusServer.StopListening();
                _modbusServer = null;
            }

            IsRunning = false;
            Status = "已停止";
            ConnectionCount = 0;
            ConnectedClients.Clear();
            AddLog("Modbus TCP 服务器已停止");
        }
        catch (Exception ex)
        {
            AddLog($"停止错误: {ex.Message}");
            ErrorCount++;
        }
    }

    [RelayCommand]
    private void ClearLog()
    {
        RequestLog = string.Empty;
    }

    [RelayCommand]
    private void ExportLog()
    {
        // TODO: 实现日志导出功能
        AddLog("导出功能开发中...");
    }

    [RelayCommand]
    private void RandomFillRegisters()
    {
        var random = new Random();
        foreach (var reg in HoldingRegisters) reg.Value = (short)random.Next(-32768, 32767);
        AddLog("保持寄存器已随机填充");
    }

    [RelayCommand]
    private void ClearRegisters()
    {
        foreach (var reg in HoldingRegisters) reg.Value = 0;
        foreach (var coil in Coils) coil.Value = false;
        AddLog("所有寄存器已清零");
    }

    private void OnCoilsChanged(int coil, int numberOfCoils)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            RequestCount++;

            // 读取变更的线圈值
            var coilValues = new List<bool>();
            if (_modbusServer != null && coil < Coils.Count)
                for (var i = 0; i < numberOfCoils && coil + i < Coils.Count; i++)
                {
                    var value = _modbusServer.coils[coil + i + 1];
                    coilValues.Add(value);
                    Coils[coil + i].Value = value;
                }

            // 打印详细信息
            var valuesStr = string.Join(", ", coilValues.Select(v => v ? "1" : "0"));
            AddLog($"[写入线圈] 起始地址: {coil}, 数量: {numberOfCoils}, 值: [{valuesStr}]");

            // 更新连接数
            if (_modbusServer != null) ConnectionCount = _modbusServer.NumberOfConnections;
        });
    }

    private void OnHoldingRegistersChanged(int register, int numberOfRegisters)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            RequestCount++;

            // 读取变更的寄存器值
            var registerValues = new List<short>();

            // 调试信息：显示原始参数
            AddLog($"[调试] register={register}, numberOfRegisters={numberOfRegisters}");

            if (_modbusServer != null && register - 1 < HoldingRegisters.Count)
                for (var i = 0; i < numberOfRegisters && register - 1 + i < HoldingRegisters.Count; i++)
                {
                    // EasyModbusTCP 的 holdingRegisters 数组从索引 1 开始
                    // register 参数已经是从 1 开始的，所以直接使用 register + i
                    var arrayIndex = register + i;
                    var value = _modbusServer.holdingRegisters[arrayIndex];

                    AddLog($"[调试] i={i}, arrayIndex={arrayIndex}, value={value}");

                    registerValues.Add(value);
                    // 更新 UI 集合时需要 -1，因为 UI 集合是从 0 开始的
                    HoldingRegisters[register - 1 + i].Value = value;
                }

            // 打印详细信息
            var valuesStr = string.Join(", ", registerValues.Select(v => $"{v} (0x{v:X4})"));
            // Modbus 地址 = 40000 + register，因为 register 已经是从 1 开始
            AddLog($"[写入保持寄存器] 起始地址: {40000 + register}, 数量: {numberOfRegisters}, 值: [{valuesStr}]");

            // 更新连接数
            if (_modbusServer != null) ConnectionCount = _modbusServer.NumberOfConnections;
        });
    }

    private void AddLog(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        RequestLog += $"[{timestamp}] {message}\n";

        if (RequestLog.Length > 10000) RequestLog = RequestLog.Substring(RequestLog.Length - 8000);
    }

    private void UpdateConnectionStatus(object? state)
    {
        if (_modbusServer == null || !IsRunning)
            return;

        _dispatcherQueue.TryEnqueue(() =>
        {
            try
            {
                var currentConnections = _modbusServer.NumberOfConnections;

                if (ConnectionCount != currentConnections)
                {
                    ConnectionCount = currentConnections;

                    // 更新客户端连接列表（EasyModbusTCP 不提供详细连接信息，显示连接数）
                    UpdateClientList(currentConnections);
                }
            }
            catch
            {
                // 忽略定时器执行中的异常
            }
        });
    }

    private void UpdateClientList(int connectionCount)
    {
        // 由于 EasyModbusTCP 不提供详细的客户端信息，我们只能显示连接数量
        // 根据连接数调整列表项
        while (ConnectedClients.Count > connectionCount) ConnectedClients.RemoveAt(ConnectedClients.Count - 1);

        while (ConnectedClients.Count < connectionCount)
        {
            var connectionIndex = ConnectedClients.Count + 1;
            ConnectedClients.Add(new ClientConnection
            {
                IpAddress = $"客户端 #{connectionIndex}",
                Port = Port,
                ConnectedTime = DateTime.Now
            });
        }
    }
}

public partial class ClientConnection : ObservableObject
{
    [ObservableProperty] private DateTime connectedTime = DateTime.Now;

    [ObservableProperty] private string ipAddress = string.Empty;

    [ObservableProperty] private int port;
}

public partial class RegisterData : ObservableObject
{
    [ObservableProperty] private int address;

    [ObservableProperty] private short value;
}

public partial class CoilData : ObservableObject
{
    [ObservableProperty] private int address;

    [ObservableProperty] private bool value;
}