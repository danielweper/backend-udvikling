namespace ClientLogic;

[Flags]
public enum ClientStates
{
    None = 0,
    IsConnected = 1 << 0,
    IsInLobby = 1 << 1,
    IsHost = 1 << 2,
    IsFighter = 1 << 3,
    IsInGame = 1 << 4,
}
