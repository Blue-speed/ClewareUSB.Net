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

	  _trafficLight.SetState(ISwitchable.Status.Green, ISwitchable.Pulse.Solid);

	  _mockDevices.VerifyWrite(LightHasState(LedAddress.Green,StateBytes.Solid), Times.Exactly(1));
	}

	[TestMethod]
	public void SetStateWillClearBeforeSettingLight()
	{
	  _mockDevices.WatchWrite();

	  _trafficLight.SetState(ISwitchable.Status.Red, ISwitchable.Pulse.Solid);

	  _mockDevices.VerifyWrite(AllLightsOff, Times.Exactly(3));
	  _mockDevices.VerifyWrite(LightHasState(LedAddress.Red, StateBytes.Solid), Times.Exactly(1));
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
	  (bytes) => bytes[3] == 0 && bytes[2] >= LedAddress.Red && bytes[2] <= LedAddress.Green;

	private Expression<Func<byte[], bool>> LightHasState(byte ledAddress,byte state)
	{
	  return (bytes) => bytes[3] == state && bytes[2] == ledAddress;
	}

	#region struct
	private struct LedAddress
	{
	  public static readonly byte Red = 0x010;
	  public static readonly byte Yellow = 0x011;
	  public static readonly byte Green = 0x012;
    }
	private struct StateBytes
	{
	  public static readonly byte Off = 0x000;
	  public static readonly byte Solid = 0x001;
	  public static readonly byte HalfSecond = 0x010;
	  public static readonly byte OneSecond = 0x011;
	}
	#endregion
	#endregion

  }
}
