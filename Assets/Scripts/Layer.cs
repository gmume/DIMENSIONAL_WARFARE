using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layer
{
    //Leyer Player1 = 6 and Player2 = 7
    public static int SetLayerPlayer(PlayerScript playerScript)
    {
        if (playerScript.name == "Player1")
        {
            return 7;
        }
        else
        {
            return 6;
        }
    }

    //Leyer Fleet1 = 8 and Fleet2 = 9
    public static int SetLayerFleet(PlayerScript playerScript)
    {
        if (playerScript.name == "Player1")
        {
            return 8;
        }
        else
        {
            return 9;
        }
    }

    //Leyer VisibleShip = 10
    public static int SetShipVisible()
    {
        return 10;
    }
}