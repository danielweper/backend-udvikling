namespace Core.Packets.Client;

public class ToggleReadyToStartPacket : IPacket
{
    public PacketType type => PacketType.ToggleReadyToStart;

    public bool newStatus;
}