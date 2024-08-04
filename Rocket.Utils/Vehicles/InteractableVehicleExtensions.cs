using SDG.Unturned;
using System.Linq;

namespace Feli.Rocket.Utils.Vehicles;

public static class InteractableVehicleExtensions
{
    public static void DestroyFull(this InteractableVehicle vehicle)
    {
        ThreadUtil.assertIsGameThread();

        vehicle.forceRemoveAllPlayers();

        foreach (var drop in BarricadeManager.getRegionFromVehicle(vehicle).drops)
        {
            if (drop.interactable is InteractableStorage interactableStorage)
                interactableStorage.items.clear();
            else if (drop.interactable is InteractableMannequin interactableMannequin)
                interactableMannequin.clearClothes();
        }

        if (vehicle.trunkItems is not null)
        {
            foreach (var item in vehicle.trunkItems.items.ToList())
            {
                vehicle.trunkItems.removeItem(vehicle.trunkItems.getIndex(item.x, item.y));
            }
        }

        VehicleManager.askVehicleDestroy(vehicle);
    }
}
