using Microsoft.AspNetCore.Http.HttpResults;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Client;

public interface IParticipant
{
    public byte id { get; }
    public event Func<string> JoinedLobby;
    public event Func<byte> LeftLobby; // check
    public event Func<byte, IPlayerProfile> PlayerJoined;
    public event Func<byte> PlayerLeft;
    public event Func<ulong> GameStarting; // maybe DateTime instead of ulong
    public event Func<IGameSettings> GameSettingsChanged;
    public event Func<byte, IPlayerProfile> PlayerChangedProfile;
    public event Func<byte, IRole> PlayerChangedRole;
    
}