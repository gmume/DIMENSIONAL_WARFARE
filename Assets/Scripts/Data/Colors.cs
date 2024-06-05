using UnityEngine;

static class Colors
{
    [Header("Ships")]
    public readonly static Color fleet1 = new(0.3f, 0.3f, 0f);    // Hex 4C4C00 olive
    public readonly static Color fleet2 = new(0.3f, 0.18f, 0.1f);   // Hex 4C2E1A brown
    public readonly static Color deltaActivShip  = new(0.3f, 0.3f, 0.3f);
    public readonly static Color intactPart = new(0.35f, 0.95f, 0.68f); // Hex 59F2AD HUD_green bright
    public readonly static Color damagedPart = new(0.5f, 0f, 0f);

    [Header("Dimensions")]
    //public readonly static Color oceanBlue = new(0.3f, 0.4f, 1f);     // Hex 4C66FF blue         
    //public readonly static Color oceanTurqoise = new(0.15f, 0.8f, 1f); // Hex 26CCFF turquoise

    public readonly static Color cellBlue = new(0.13f, 0.24f, 0.9f, 1f);     // Hex 213DE6 blue (oceanBlue +20%)      
    public readonly static Color cellTurqoise = new(0.1f, 0.56f, 0.7f, 1f); // Hex 1B8FB2 turquoise
    public readonly static Color deltaActiveCell = new(0.1f, 0.1f, 0.1f, 0f);
    public readonly static Color hitCell  = Color.red;

    [Header("HUD")]
    public readonly static Color HUD_green = new(0f, 0.43f, 0.25f); // Hex 006E40
    public readonly static Color deltaActiveHUDDimension = new(0.2f, 0.2f, 0.2f, 0f);
}