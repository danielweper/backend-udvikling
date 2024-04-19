namespace Turnbased_Game.Models.Packages.Client;

public class SendMessage: IPackage
{
    public byte senderId { get; set; }
    public string message { get; set; }
    
    public byte id { get;  } = 34;
    
}