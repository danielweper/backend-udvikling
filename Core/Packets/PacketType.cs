using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packets;

public enum PacketType : byte
{
    Unknown = 0,

    // Shared packets.
    Ping = 1,
    Acknowledged = 2,
    Accepted = 3,
    Denied = 4,
    InvalidRequest = 5,

    // Server packets.
    LobbyCreated = 11,
    LobbyInfo = 13,
    PlayerJoinedLobby = 15,
    PlayerLeftLobby = 17,
    AvailableLobbies = 19,
    GameStarting = 21,
    GameSettingsChanged = 23,
    PlayerChanged = 25,
    RoleChangeRequested = 27,
    RoleChanged = 29,
    ExecuteTurn = 31,
    BattleIsOver = 33,
    UserMessage = 35,
    SystemMessage = 37,
    RegisterPlayerTurn = 39,
    PlayerReadyStatus = 41,
    PlayerProfileCreated = 43,
    PlayerKicked = 45,

    // Client packets.
    CreateLobby = 12,
    JoinLobby = 14,
    ListAvailableLobbies = 16,
    DisconnectLobby = 18,
    KickPlayer = 20,
    StartGame = 22,
    ToggleReadyToStart = 24,
    ChangeGameSettings = 26,
    RequestPlayerUpdate = 28,
    RequestRoleChange = 30,
    SubmitTurn = 32,
    SendMessage = 34,
}
