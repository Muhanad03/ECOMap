﻿namespace ECOMap;

using Android.Gms.Maps;
using ECOMap;
using Microsoft.Maui.Maps;

class MapCallbackHandler(CustomMapHandler mapHandler) : Java.Lang.Object, IOnMapReadyCallback
{
    public void OnMapReady(GoogleMap googleMap)
    {
        mapHandler.UpdateValue(nameof(IMap.Pins));
        mapHandler.Map?.SetOnMarkerClickListener(new CustomMarkerClickListener(mapHandler));
        mapHandler.Map?.SetOnInfoWindowClickListener(new CustomInfoWindowClickListener(mapHandler));
    }
}