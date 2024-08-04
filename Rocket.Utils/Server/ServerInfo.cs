using SDG.Unturned;
using Steamworks;

namespace Feli.Rocket.Utils.Server;

public static class ServerInfo
{
    public static uint GetServerIP()
    {
        if (!Provider.configData.Server.Use_FakeIP)
        {
            var address = SteamGameServer.GetPublicIP().ToIPAddress();
            var ipBytes = address.GetAddressBytes();
            var ip = (uint)ipBytes[3] << 24;
            ip += (uint)ipBytes[2] << 16;
            ip += (uint)ipBytes[1] << 8;
            ip += (uint)ipBytes[0];

            return ip;
        }
        else
        {
            SteamGameServerNetworkingSockets.GetFakeIP(0, out var pInfo);
            if (pInfo.m_eResult == EResult.k_EResultOK)
            {
                return pInfo.m_unIP;
            }
        }

        return 0;
    }

    public static ushort GetServerPort()
    {
        if (!Provider.configData.Server.Use_FakeIP)
        {
            return Provider.port;
        }
        else
        {
            SteamGameServerNetworkingSockets.GetFakeIP(0, out var pInfo);
            if (pInfo.m_eResult == EResult.k_EResultOK)
            {
                return pInfo.m_unPorts[0];
            }
        }

        return 0;
    }
}
