using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyModbus;
using Microsoft.UI.Dispatching;

namespace qingzhu.ViewModels;

public partial class ModbusTcpClientViewModel : ViewModelBase, IDisposable
{
    private readonly DispatcherQueue _dispatcherQueue;
    private ModbusClient? _modbusClient;

    [ObservableProperty] private string ipAddress = "127.0.0.1";

    [ObservableProperty] private bool isConnected;

    [ObservableProperty] private string operationLog = string.Empty;

    [ObservableProperty] private int port = 502;

    [ObservableProperty] private int quantity = 10;

    [ObservableProperty] private string readResult = string.Empty;

    [ObservableProperty] private string selectedFunction = "读保持寄存器 (FC03)";

    [ObservableProperty] private int slaveId = 1;

    [ObservableProperty] private int startAddress = 40001;

    [ObservableProperty] private string status = "未连接";

    [ObservableProperty] private string writeValue = "0";

    public ModbusTcpClientViewModel()
    {
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    public ObservableCollection<string> FunctionCodes { get; } = new()
    {
        "读线圈 (FC01)",
        "读离散输入 (FC02)",
        "读保持寄存器 (FC03)",
        "读输入寄存器 (FC04)",
        "写单个线圈 (FC05)",
        "写单个寄存器 (FC06)",
        "写多个线圈 (FC15)",
        "写多个寄存器 (FC16)"
    };

    public void Dispose()
    {
        if (IsConnected) Disconnect();
    }

    [RelayCommand]
    private void Connect()
    {
        try
        {
            _modbusClient = new ModbusClient(IpAddress, Port);
            // _modbusClient.UnitIdentifier = (byte)SlaveId;  // 设置从站地址
            _modbusClient.Connect();

            IsConnected = true;
            Status = "已连接";
            AddLog($"已连接到 {IpAddress}:{Port}");
        }
        catch (Exception ex)
        {
            Status = "连接失败";
            AddLog($"连接错误: {ex.Message}");
        }
    }

    [RelayCommand]
    private void Disconnect()
    {
        try
        {
            _modbusClient?.Disconnect();
            _modbusClient = null;

            IsConnected = false;
            Status = "未连接";
            ReadResult = string.Empty;
            AddLog("已断开连接");
        }
        catch (Exception ex)
        {
            AddLog($"断开连接错误: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ExecuteFunction()
    {
        if (_modbusClient == null || !IsConnected)
        {
            AddLog("错误: 未连接到服务器");
            return;
        }

        try
        {
            var address = StartAddress >= 40000 ? StartAddress - 40001 :
                StartAddress >= 30000 ? StartAddress - 30001 :
                StartAddress >= 10000 ? StartAddress - 10001 : StartAddress - 1;

            switch (SelectedFunction)
            {
                case "读线圈 (FC01)":
                    ReadCoils(address);
                    break;
                case "读离散输入 (FC02)":
                    ReadDiscreteInputs(address);
                    break;
                case "读保持寄存器 (FC03)":
                    ReadHoldingRegisters(address);
                    break;
                case "读输入寄存器 (FC04)":
                    ReadInputRegisters(address);
                    break;
                case "写单个线圈 (FC05)":
                    WriteSingleCoil(address);
                    break;
                case "写单个寄存器 (FC06)":
                    WriteSingleRegister(address);
                    break;
                case "写多个线圈 (FC15)":
                    WriteMultipleCoils(address);
                    break;
                case "写多个寄存器 (FC16)":
                    WriteMultipleRegisters(address);
                    break;
            }
        }
        catch (Exception ex)
        {
            AddLog($"操作错误: {ex.Message}");
        }
    }

    private void ReadCoils(int address)
    {
        var values = _modbusClient!.ReadCoils(address, Quantity);
        var result = string.Join(", ", values.Select(v => v ? "1" : "0"));
        ReadResult = $"[{result}]";
        AddLog($"[FC01 读线圈] 地址: {StartAddress}, 数量: {Quantity}, 结果: [{result}]");
    }

    private void ReadDiscreteInputs(int address)
    {
        var values = _modbusClient!.ReadDiscreteInputs(address, Quantity);
        var result = string.Join(", ", values.Select(v => v ? "1" : "0"));
        ReadResult = $"[{result}]";
        AddLog($"[FC02 读离散输入] 地址: {StartAddress}, 数量: {Quantity}, 结果: [{result}]");
    }

    private void ReadHoldingRegisters(int address)
    {
        var values = _modbusClient!.ReadHoldingRegisters(address, Quantity);
        var result = string.Join(", ", values.Select(v => $"{v} (0x{v:X4})"));
        ReadResult = $"[{result}]";
        AddLog($"[FC03 读保持寄存器] 地址: {StartAddress}, 数量: {Quantity}, 结果: [{result}]");
    }

    private void ReadInputRegisters(int address)
    {
        var values = _modbusClient!.ReadInputRegisters(address, Quantity);
        var result = string.Join(", ", values.Select(v => $"{v} (0x{v:X4})"));
        ReadResult = $"[{result}]";
        AddLog($"[FC04 读输入寄存器] 地址: {StartAddress}, 数量: {Quantity}, 结果: [{result}]");
    }

    private void WriteSingleCoil(int address)
    {
        var value = WriteValue == "1" || WriteValue.ToLower() == "true";
        _modbusClient!.WriteSingleCoil(address, value);
        ReadResult = $"写入成功: {(value ? "1" : "0")}";
        AddLog($"[FC05 写单个线圈] 地址: {StartAddress}, 值: {(value ? "1" : "0")}");
    }

    private void WriteSingleRegister(int address)
    {
        var value = int.Parse(WriteValue);
        _modbusClient!.WriteSingleRegister(address, value);
        ReadResult = $"写入成功: {value} (0x{value:X4})";
        AddLog($"[FC06 写单个寄存器] 地址: {StartAddress}, 值: {value} (0x{value:X4})");
    }

    private void WriteMultipleCoils(int address)
    {
        var values = WriteValue.Split(',')
            .Select(v => v.Trim() == "1" || v.Trim().ToLower() == "true")
            .ToArray();
        _modbusClient!.WriteMultipleCoils(address, values);
        var result = string.Join(", ", values.Select(v => v ? "1" : "0"));
        ReadResult = $"写入成功: [{result}]";
        AddLog($"[FC15 写多个线圈] 地址: {StartAddress}, 数量: {values.Length}, 值: [{result}]");
    }

    private void WriteMultipleRegisters(int address)
    {
        var values = WriteValue.Split(',')
            .Select(v => int.Parse(v.Trim()))
            .ToArray();
        _modbusClient!.WriteMultipleRegisters(address, values);
        var result = string.Join(", ", values.Select(v => $"{v} (0x{v:X4})"));
        ReadResult = $"写入成功: [{result}]";
        AddLog($"[FC16 写多个寄存器] 地址: {StartAddress}, 数量: {values.Length}, 值: [{result}]");
    }

    [RelayCommand]
    private void ClearLog()
    {
        OperationLog = string.Empty;
    }

    private void AddLog(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        OperationLog += $"[{timestamp}] {message}\n";

        if (OperationLog.Length > 10000) OperationLog = OperationLog.Substring(OperationLog.Length - 8000);
    }
}