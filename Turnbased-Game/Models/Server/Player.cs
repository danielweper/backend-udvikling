namespace Turnbased_Game.Models.Server;

public struct Player(byte id)
{
    public byte id { get; private set; } = id;
}