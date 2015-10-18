using System;

namespace BlueSpeed.ClewareUSB
{
  public abstract class ISwitchable
  {
	 public abstract void Clear();


	public abstract void SetState(Status status, Pulse pulse);

	#region enumerations
	public enum Status
	 {
	  Green,
	  Yellow,
	  Red,
	 }
	public enum Pulse
	{
	  Off,
	  Solid,
	  HalfSecond,
	  OneSecond,
	  TwoSeconds,
	  FourSeconds,
	  EightSeconds
	}
	#endregion
  }
}