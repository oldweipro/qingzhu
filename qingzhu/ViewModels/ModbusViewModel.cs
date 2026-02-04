using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyModbus;
using Microsoft.UI.Dispatching;

namespace qingzhu.ViewModels;

public partial class ModbusViewModel : ViewModelBase, IDisposable
{
    private readonly DispatcherQueue _dispatcherQueue;
    private ModbusServer? _modbusServer;

    [ObservableProperty] private int baudRate = 9600;

    [ObservableProperty] private int dataBits = 8;

    [ObservableProperty] private int errorCount;

    [ObservableProperty] private bool isRunning;

    [ObservableProperty] private string parity = "None";

    [ObservableProperty] private int requestCount;

    [ObservableProperty] private string requestLog = string.Empty;

    [ObservableProperty] private string? selectedPort;

    [ObservableProperty] private int slaveId = 1;

    [ObservableProperty] private string status = "已停止";

    [ObservableProperty] private string stopBits = "One";

    public ModbusViewModel()
    {
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        RefreshSerialPorts();
        InitializeRegisters();
    }

    public ObservableCollection<string> AvailablePorts { get; } = new();

    public ObservableCollection<int> BaudRates { get; } = new()
    {
        4800, 9600, 19200, 38400, 57600, 115200
    };

    public ObservableCollection<string> ParityOptions { get; } = new()
    {
        "None", "Even", "Odd", "Mark", "Space"
    };

    public ObservableCollection<int> DataBitsOptions { get; } = new() { 7, 8 };

    public ObservableCollection<string> StopBitsOptions { get; } = new()
    {
        "One", "Two", "OnePointFive"
    };

    public ObservableCollection<RegisterData> HoldingRegisters { get; } = new();
    public ObservableCollection<RegisterData> InputRegisters { get; } = new();
    public ObservableCollection<CoilData> Coils { get; } = new();
    public ObservableCollection<CoilData> DiscreteInputs { get; } = new();

    public void Dispose()
    {
        if (IsRunning) StopSlave();
    }

    private void RefreshSerialPorts()
    {
        AvailablePorts.Clear();
        var ports = SerialPort.GetPortNames();
        foreach (var port in ports) AvailablePorts.Add(port);

        if (AvailablePorts.Count == 0)
        {
            AvailablePorts.Add("COM1");
            AvailablePorts.Add("COM2");
        }
    }

    private void InitializeRegisters()
    {
        for (var i = 0; i < 100; i++)
        {
            HoldingRegisters.Add(new RegisterData { Address = 40001 + i, Value = 0 });
            InputRegisters.Add(new RegisterData { Address = 30001 + i, Value = 0 });
            Coils.Add(new CoilData { Address = 1 + i, Value = false });
            DiscreteInputs.Add(new CoilData { Address = 10001 + i, Value = false });
        }
    }

    [RelayCommand]
    private void StartSlave()
    {
        try
        {
            if (string.IsNullOrEmpty(SelectedPort))
            {
                AddLog("错误: 请选择串口");
                ErrorCount++;
                return;
            }

            _modbusServer = new ModbusServer
            {
                SerialPort = SelectedPort,
                Baudrate = BaudRate,
                UnitIdentifier = (byte)SlaveId
            };

            _modbusServer.Parity = ParseParity(Parity);
            _modbusServer.StopBits = ParseStopBits(StopBits);

            _modbusServer.CoilsChanged += OnCoilsChanged;
            _modbusServer.HoldingRegistersChanged += OnHoldingRegistersChanged;

            _modbusServer.Listen();

            IsRunning = true;
            Status = "运行中";
            AddLog($"Modbus RTU 从站已启动 - {SelectedPort}, {BaudRate}, {Parity}, {StopBits}");
            AddLog($"从站ID: {SlaveId}");
        }
        catch (Exception ex)
        {
            Status = "启动失败";
            AddLog($"错误: {ex.Message}");
            ErrorCount++;
        }
    }

    [RelayCommand]
    private void StopSlave()
    {
        try
        {
            if (_modbusServer != null)
            {
                _modbusServer.CoilsChanged -= OnCoilsChanged;
                _modbusServer.HoldingRegistersChanged -= OnHoldingRegistersChanged;
                _modbusServer.StopListening();
                _modbusServer = null;
            }

            IsRunning = false;
            Status = "已停止";
            AddLog("Modbus RTU 从站已停止");
        }
        catch (Exception ex)
        {
            AddLog($"停止错误: {ex.Message}");
            ErrorCount++;
        }
    }

    [RelayCommand]
    private void RandomFill()
    {
        var random = new Random();
        foreach (var reg in HoldingRegisters)
        {
            reg.Value = (short)random.Next(-32768, 32767);
            if (_modbusServer != null) _modbusServer.holdingRegisters[reg.Address - 40000] = reg.Value;
        }

        foreach (var reg in InputRegisters)
        {
            reg.Value = (short)random.Next(0, 65535);
            if (_modbusServer != null) _modbusServer.inputRegisters[reg.Address - 30000] = reg.Value;
        }

        AddLog("寄存器已随机填充");
    }

    [RelayCommand]
    private void ClearRegisters()
    {
        foreach (var reg in HoldingRegisters)
        {
            reg.Value = 0;
            if (_modbusServer != null) _modbusServer.holdingRegisters[reg.Address - 40000] = 0;
        }

        foreach (var coil in Coils)
        {
            coil.Value = false;
            if (_modbusServer != null) _modbusServer.coils[coil.Address] = false;
        }

        AddLog("所有寄存器已清零");
    }

    [RelayCommand]
    private void ImportData()
    {
        // TODO: 实现数据导入功能
        AddLog("导入功能开发中...");
    }

    [RelayCommand]
    private void ExportData()
    {
        // TODO: 实现数据导出功能
        AddLog("导出功能开发中...");
    }

    private void OnCoilsChanged(int coil, int numberOfCoils)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            RequestCount++;
            AddLog($"线圈变更: 地址 {coil}, 数量 {numberOfCoils}");

            if (_modbusServer != null && coil < Coils.Count)
                for (var i = 0; i < numberOfCoils && coil + i < Coils.Count; i++)
                    Coils[coil + i].Value = _modbusServer.coils[coil + i + 1];
        });
    }

    private void OnHoldingRegistersChanged(int register, int numberOfRegisters)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            RequestCount++;
            AddLog($"保持寄存器变更: 地址 {register}, 数量 {numberOfRegisters}");

            if (_modbusServer != null && register < HoldingRegisters.Count)
                for (var i = 0; i < numberOfRegisters && register + i < HoldingRegisters.Count; i++)
                    HoldingRegisters[register + i].Value = _modbusServer.holdingRegisters[register + i + 1];
        });
    }

    private Parity ParseParity(string parity)
    {
        return parity switch
        {
            "Even" => System.IO.Ports.Parity.Even,
            "Odd" => System.IO.Ports.Parity.Odd,
            "Mark" => System.IO.Ports.Parity.Mark,
            "Space" => System.IO.Ports.Parity.Space,
            _ => System.IO.Ports.Parity.None
        };
    }

    private StopBits ParseStopBits(string stopBits)
    {
        return stopBits switch
        {
            "Two" => System.IO.Ports.StopBits.Two,
            "OnePointFive" => System.IO.Ports.StopBits.OnePointFive,
            _ => System.IO.Ports.StopBits.One
        };
    }

    private void AddLog(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        RequestLog += $"[{timestamp}] {message}\n";

        if (RequestLog.Length > 10000) RequestLog = RequestLog.Substring(RequestLog.Length - 8000);
    }
}