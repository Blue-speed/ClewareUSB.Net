using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueSpeed.ClewareUSB.Devices
{
  public class UsbDevice : IHidDevice, IDisposable
  {
	private HidLibrary.IHidDevice _device;

	public static UsbDevice Find(int vendorId, int productId)
	{
	  return new UsbDevice(HidLibrary.HidDevices.Enumerate(vendorId, productId).First());
	}

	public static IEnumerable<UsbDevice> FindAll(int vendorId, params int[] productIds)
	{
		return HidLibrary.HidDevices.Enumerate(vendorId, productIds).Select(device=> new UsbDevice(device));
    }

	private UsbDevice(HidLibrary.HidDevice device)
	{
	  _device = device;
	  _device.OpenDevice();
	  while(!_device.IsOpen || !_device.IsConnected) { }
	}

	public void Write(byte[] data)
	{
	  _device.Write(data);
	}

	public void Dispose()
	{
	  _device.CloseDevice();
	}
  }
}
