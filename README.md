# ClewareUSB.Net
An implementation of the Cleware USB Stoplight (USB-Ampel) in .NET

# Requirements

This package requires a .Net HID Libary found here:  
https://github.com/mikeobrien/HidLibrary

# Using
using the Ampel factory to get the first instance or all usb instances. Both methods will return an ISwitchable Object.  Using the interface set or clear the device state.

```csharp
ISwitchable device = AmpelFactory.GetAllDevices();
device.SetState(ISwitchable.Status.Red,ISwitchable.Pulse.Off);
device.Clear();
```

# USB-Ampel Protocols

## USB-TrafficLights S

Thanks to https://github.com/holgero/ClewareUSB for providing the java version and initial documention to get the light working.

### Identifiers
These may be the same for all Cleware Ampel devices but without the physical products I can not test this.

VID 0x0D50  
DID 0x0008

### Commands
The USB command structure consists of 4 bytes. The first two bytes are always 0, the third byte is the LED address and the last is the pulse duration.

#### LED Address
* *Red:* 0x010  
* *Yellow:* 0x011  
* *Green:* 0x012

#### Pulse duration
* *Off* 0x000  
* *Solid* 0x001
* *0.5 Seconds* 0x001
* *1 Second* 0x0011
* *1.5 Seconds* 0x012
* *2 Seconds* 0x013
* *2.5 Seconds* 0x014
* *3 Seconds* 0x015
* *3.5 Seconds* 0x016
* *4 Seconds* 0x017
* *4.5 Seconds* 0x018
* *5 Seconds* 0x019
* *5.5 Seconds* 0x01A
* *6 Seconds* 0x01B
* *6.5 Seconds* 0x01C
* *7 Seconds* 0x01D
* *7.5 Seconds* 0x01E
* *8 Seconds* 0x01F

The final command will look something like  
```csharp 
device.Write(new byte[]{0,0,0x010,1}); 
```
