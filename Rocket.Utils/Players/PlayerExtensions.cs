using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace Feli.Rocket.Utils.Players;

public static class PlayerExtensions
{
    public static InteractableVehicle? GetLookingVehicle(this UnturnedPlayer player)
    {
        return player.Player.GetLookingVehicle();
    }

    public static InteractableVehicle? GetLookingVehicle(this Player player)
    {
        if (Physics.Raycast(player.look.aim.position, player.look.aim.forward,
            out RaycastHit hitinfo,
            player.look.perspective == EPlayerPerspective.THIRD ? 6 : 4,
            RayMasks.VEHICLE) && hitinfo.transform != null)
        {
            return DamageTool.getVehicle(hitinfo.transform);
        }

        return null;
    }
}
