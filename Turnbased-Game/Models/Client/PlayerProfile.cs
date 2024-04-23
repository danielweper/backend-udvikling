using System.Text;

namespace Turnbased_Game.Models.Client;

public class PlayerProfile : IPlayerProfile
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Color: {Color}");
        sb.AppendLine($"Name: {Name}");
        
        return sb.ToString();
    }
}