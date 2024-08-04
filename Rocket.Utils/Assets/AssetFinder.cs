using Feli.Rocket.Utils.Strings;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feli.Rocket.Utils.Assets;

public static class AssetFinder
{
    public static Asset? GetAsset(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;

        if (Guid.TryParse(id, out var guid))
        {
            return GetAsset(guid);
        }

        if (ushort.TryParse(id, out ushort parsed))
        {
            return GetAsset(parsed);
        }

        List<Asset> possibilities = new List<Asset>();

        string lowered = id.ToLower();

        var assets = new List<Asset>();
        SDG.Unturned.Assets.find(assets);
        foreach (var asset in assets.Where(x => x.assetCategory == EAssetType.VEHICLE || x.assetCategory == EAssetType.ITEM))
        {
            if (string.IsNullOrWhiteSpace(asset.FriendlyName)) continue;

            if (asset.FriendlyName.ToLower().Contains(lowered))
            {
                possibilities.Add(asset);
            }
        }

        return possibilities
            .OrderBy(x => LevenshteinDistance.Calculate(x.FriendlyName.ToLower(), lowered))
            .FirstOrDefault();
    }

    public static Asset? GetAsset(ushort id)
    {
        var vehicleAsset = SDG.Unturned.Assets.find(EAssetType.VEHICLE, id);
        if (vehicleAsset != null)
            return GetAsset(vehicleAsset.GUID);

        var itemAsset = SDG.Unturned.Assets.find(EAssetType.ITEM, id);
        if (itemAsset != null)
            return GetAsset(itemAsset.GUID);

        return null;
    }

    public static Asset? GetAsset(Guid id)
    {
        var asset = SDG.Unturned.Assets.find(id);

        if (asset is RedirectorAsset redirectorAsset)
        {
            return GetAsset(redirectorAsset.TargetGuid);
        }

        if (asset is VehicleRedirectorAsset vehicleRedirectorAsset)
        {
            return vehicleRedirectorAsset.TargetVehicle.Find();
        }

        return asset;
    }
}
