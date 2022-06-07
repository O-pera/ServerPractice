protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 

START ../../../Server/PacketGenerator/bin/Debug/net6.0/PacketGenerator.exe ./Protocol.proto

XCOPY /Y ServerPacketManager.cs "../../../Server/Server/Packet"
XCOPY /Y ServerPacketHandler.cs "../../../Server/Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../Server/Client/Packet"
XCOPY /Y ClientPacketHandler.cs "../../../Server/Client/Packet"

XCOPY /Y Protocol.cs "../../../Server/Server/Packet"
XCOPY /Y Protocol.cs "../../../Server/Client/Packet"