using Turnbased_Game.Models.Packages;

namespace Turnbased_Game.Models.Client;

public class TestClient : Client
{
    public event Action<IPackage> PackageSent;
    public event Action<IPackage> PackageReceived;

    public override async void SendPackage(IPackage package)
    {
        PackageSent?.Invoke(package);
        lastPackage = package;
    }
    public override async void ReceivePackage(IPackage package)
    {
        PackageReceived?.Invoke(package);
    }
}