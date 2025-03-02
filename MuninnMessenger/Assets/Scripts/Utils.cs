using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static Vector2 GetSnapPosition(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        return result;
    }

    public static string GetTimeString(DateTime time)
    {
        if(time.Date == DateTime.Today)
        {
            return time.ToString("HH:mm");
        }

        if(time.Year == DateTime.Now.Year)
        {
            return time.ToString("d MMM HH:mm");
        }
        
        return time.ToString("d MMM yyyy HH:mm");
    }

    public static T GetRandom<T>(this ICollection<T> collection) => collection.ElementAt(UnityEngine.Random.Range(0, collection.Count));
}