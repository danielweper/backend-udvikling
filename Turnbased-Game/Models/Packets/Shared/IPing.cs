namespace Turnbased_Game.Models.Packets.Shared;

public interface IPing: IPackage
{
    public byte PacketId => 1;
}