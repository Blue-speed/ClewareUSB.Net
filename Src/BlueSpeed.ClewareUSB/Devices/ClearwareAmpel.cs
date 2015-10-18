using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueSpeed.ClewareUSB.Devices
{
  public class ClearwareAmpel : ISwitchable
  {
	private IEnumerable<IHidDevice> _devices;
	private readonly Dictionary<Status, byte> _status;
	private readonly Dictionary<Pulse, byte> _pulse;

	public static readonly int VendorId = 0x0D50;
	public static readonly int DeviceId = 0x0008;

	#region Contructors

	public ClearwareAmpel(IHidDevice device) : this(new [] { device}) { }

	public ClearwareAmpel(IEnumerable<IHidDevice> devices) : this()
	{
	  if(devices.Count() == 0)
	  {
		throw new MissingDeviceException();
	  }
	  _devices = devices;
	}

	public ClearwareAmpel()
	{
	  _status = new Dictionary<Status, byte>
		{ {Status.Red,0x010 }, { Status.Yellow, 0x011 }, { Status.Green, 0x012 } };
	  _pulse = new Dictionary<Pulse, byte>
		{ {Pulse.Off, 0 }, {Pulse.Solid,1 },
		{ Pulse.HalfSecond,0x010 },{Pulse.OneSecond,0x011 },{Pulse.TwoSeconds,0x013 },
		{ Pulse.FourSeconds,0x017 },{Pulse.EightSeconds,0x01F }, };
	}
	
	#endregion

	public override void Clear()
	{
	  RunCommandOnAllDevices(GenerateCommand(Status.Red, Pulse.Off));
	  RunCommandOnAllDevices(GenerateCommand(Status.Yellow, Pulse.Off));
	  RunCommandOnAllDevices(GenerateCommand(Status.Green, Pulse.Off));
	}

	public override void SetState(Status status, Pulse pulse)
	{
	  Clear();
	  RunCommandOnAllDevices(GenerateCommand(status, pulse));
	}

	#region private methods
	
	private void RunCommandOnAllDevices(byte[] command)
	{
	  foreach (var device in _devices)
	  {
		device.Write(command);
	  }
	}

	private byte[] GenerateCommand(Status status, Pulse pulse)
	{
	  return GenerateCommand(_status[status],_pulse[pulse]);
	}

	private byte[] GenerateCommand(byte status, byte pulse)
	{
	  return new byte[] { 0, 0, status, pulse };
	}

	#endregion

	#region InnerClasses
	#region Exceptions
	public class MissingDeviceException : Exception	{}
	#endregion
	#endregion
  }
}