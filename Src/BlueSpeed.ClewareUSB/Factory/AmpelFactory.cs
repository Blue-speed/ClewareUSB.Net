using BlueSpeed.ClewareUSB.Devices;

namespace BlueSpeed.ClewareUSB.Factory
{
  public static class AmpelFactory
  {
	public static ISwitchable GetAllDevices()
	{
	  return new ClearwareAmpel(UsbDevice.FindAll(ClearwareAmpel.VendorId, ClearwareAmpel.DeviceId));
	}

	public static ISwitchable GetFirstDevice()
	{
	  return new ClearwareAmpel(UsbDevice.Find(ClearwareAmpel.VendorId, ClearwareAmpel.DeviceId));
	}
  }
}
