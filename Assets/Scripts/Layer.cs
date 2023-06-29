public static class Layer
{
    //Leyer Player1 = 6 and Player2 = 7
    public static int SetLayerPlayer(Player player)
    {
        if (player.number == 1)
        {
            return 7;
        }
        else
        {
            return 6;
        }
    }

    //Leyer Fleet1 = 8 and Fleet2 = 9
    public static int SetLayerFleet(Player player)
    {
        if (player.number == 1)
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