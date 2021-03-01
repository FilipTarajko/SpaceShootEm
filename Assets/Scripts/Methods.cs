using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Methods
{
    public static void BgScale(Transform transform)
    {
        transform.localScale *= Screen.height / 2400f;
    }

    public static bool IntToBool(int value)
    {
        if (value == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static int BoolToInt(bool value)
    {
        if (value == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
