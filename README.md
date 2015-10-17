# ClewareUSB.Net
An implementation of the Cleware USB Stoplight (USB-Ampel) in .NET

# USB-Ampel Protocols

## USB-TrafficLights S
### Identifiers
These may be the same for all Cleware Ampel devices but without the physical products I can not test this.

VID 0x0D50  
DID 0x0008

### Commands
The USB command structure consists of 4 bytes. The first two bytes are always 0, the third byte is the LED to turn on and the last is the state, 1 for on 0 for off.

*Red:* 0x010  
*Yellow:* 0x011  
*Green:* 0x012

The final command will look something like  
```csharp 
Device.Write(new byte[]{0,0,0x010,1}); 
```
