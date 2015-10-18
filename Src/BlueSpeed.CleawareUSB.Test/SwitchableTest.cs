using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueSpeed.ClewareUSB;
using BlueSpeed.ClewareUSB.Devices;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BlueSpeed.CleawareUSB.Test.Mocks;
using System.Linq.Expressions;

namespace BlueSpeed.CleawareUSB.Test
{
  [TestClass]
  public class SwitchableTest
  {
	private ISwitchable _trafficLight;
	private List<Mock<IHidDevice>> _mockDevices;

	[TestInitialize]
	public void TestInitialize()
	{
	  _mockDevices = new List<Mock<IHidDevice>>();
	  _mockDevices.Add(new Mock<IHidDevice>());
	  _mockDevices.Add(new Mock<IHidDevice>());

	  _trafficLight = new ClearwareAmpel(_mockDevices.Select(m => m.Object));
	}

	[TestMethod]
	public void CanInstantiateWithSingleObject()
	{
	  var device = new Mock<IHidDevice>();
	  ISwitchable TrafficLight = new ClearwareAmpel(device.Object);
	}

	[TestMethod]
	[ExpectedException(typeof(ClearwareAmpel.MissingDeviceException))]
	public void InstantiateWithEmptyListRaisesMissingDeviceException()
	{
	  var emptyDeviceList = new List<IHidDevice>();
	  ISwitchable TrafficLight = new ClearwareAmpel(emptyDeviceList);
	}

	[TestMethod]
	public void SetStateWillCallHidWithFourBytes()
	{
	  _mockDevices.WatchWrite();
	  foreach (ISwitchable.Status status in Enum.GetValues(typeof(ISwitchable.Status)))
	  {
		_trafficLight.SetState(status, ISwitchable.Pulse.Solid);

		_mockDevices.VerifyWrite(LightHasState(LedAddress[status], StateBytes[ISwitchable.Pulse.Solid]), Times.Exactly(1));
	  }
	}
	
	[TestMethod]
	public void ClearingCausesAllDevicesToClear()
	{
	  _mockDevices.WatchWrite();

	  _trafficLight.Clear();

	  _mockDevices.VerifyWrite(AllLightsOff, Times.Exactly(3));
	}

	#region Helpers
	private readonly Expression<Func<byte[],bool>> AllLightsOff = 
	  (bytes) => bytes[3] == 0 && bytes[2] >= LedAddress[ISwitchable.Status.Red] && bytes[2] <= LedAddress[ISwitchable.Status.Green];

	private Expression<Func<byte[], bool>> LightHasState(byte ledAddress,byte state)
	{
	  return (bytes) => bytes[3] == state && bytes[2] == ledAddress;
	}

	#region static
	private static Dictionary<ISwitchable.Status, byte> LedAddress = new Dictionary<ISwitchable.Status, byte>
	{
	  {ISwitchable.Status.Red,0x010},
	  {ISwitchable.Status.Yellow,0x011},
	  {ISwitchable.Status.Green,0x012},
	};
	private static Dictionary<ISwitchable.Pulse, byte> StateBytes = new Dictionary<ISwitchable.Pulse, byte>
	{
	  {ISwitchable.Pulse.Off,0x000},
	  {ISwitchable.Pulse.Solid,0x001},
	  {ISwitchable.Pulse.HalfSecond,0x010},
	  {ISwitchable.Pulse.OneSecond,0x011},
	};
	#endregion
	#endregion

  }
}
