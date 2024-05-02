using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCLI;

public enum Command : ushort
{
    ShowHelp = 'h',
    DisplayLobbies = 'l',
    JoinLobby = 'j',
    CreateLobby = 'c',
    DisconnectLobby = 'd',
    IsReady = 'y',
    IsNotReady = 'n',
    KickPlayer = 'k',
    StartGame = 's',
    ChangeGameSettings = 'g',
    RequestPlayerUpdate = 'r',
    RequestRoleChange = 'o',
    SendMessage = 'm',
    SubmitTurn = 't',
}
