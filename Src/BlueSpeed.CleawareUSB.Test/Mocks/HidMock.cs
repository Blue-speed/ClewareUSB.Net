using BlueSpeed.ClewareUSB;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlueSpeed.CleawareUSB.Test.Mocks
{
  internal static class HidMock
  {
	internal static void WatchWrite(this List<Mock<IHidDevice>> devices)
	{
	  devices.ForEach(mock => mock.Setup(device => device.Write(It.IsAny<byte[]>())));
	}

	internal static void VerifyWrite(this List<Mock<IHidDevice>> devices, Expression<Func<byte[],bool>> condition, Times times)
	{
	  devices.ForEach(mock => mock.Verify(device => device.Write(It.Is(condition)), times));
	}
  }
}
