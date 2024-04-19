using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCLI;

public enum Command : ushort
{
    Exit = 'x',
    DisplayLobbies = 'l',
    JoinLobby = 'j',
    DisconnectLobby = 'd',
    IsReady = 'y',
    IsNotReady = 'n',
}
