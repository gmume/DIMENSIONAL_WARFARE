public static class LayerSetter
{
    //Leyer Player1 = 6 and Player2 = 7
    public static int SetLayerPlayer(PlayerData player)
    {
        return (player.number == 1) ? 7 : 6;
    }

    public static int SetLayerDimensions(PlayerData player)
    {
        return (player.number == 1) ? 14 : 15;
    }

    //Leyer Fleet1 = 8 and Fleet2 = 9
    public static int SetLayerFleet(PlayerData player)
    {
        return (player.number == 1) ? 8 : 9;
    }

    public static int SetShipVisible(PlayerData player)
    {
        return (player.number == 1) ? 10 : 11;
    }
}