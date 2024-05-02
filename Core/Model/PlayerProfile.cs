using System.Text;

namespace Core.Model;

public class PlayerProfile(Color color, string name/*, string connectionId*/) : IPlayerProfile
{
    /*public string ConnectionId { get; init; } = connectionId;*/
    public Color Color { get; private set; } = color;
    public string Name { get; private set; } = name;


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Color: {Color}");
        sb.AppendLine($"Name: {Name}");

        return sb.ToString();
    }
}