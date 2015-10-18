using System;

namespace BlueSpeed.ClewareUSB
{
  public interface IHidDevice
  {
	void Write(byte[] data);
  }
}