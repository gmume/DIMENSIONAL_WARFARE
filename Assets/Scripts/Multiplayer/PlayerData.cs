using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerData : MonoBehaviour
{
    [HideInInspector] public int number;
                      public PlayerData opponent;
    [HideInInspector] public PlayerWorldManager world;

                      public GameObject audioManager;

    [HideInInspector] public InputHandler inputHandler;
    [HideInInspector] public PlayerInput input;
                      public MultiplayerEventSystem eventSystem;
                      public InputSystemUIInputModule inputSystem;

                      public HUD_Manager HUD;
                      public ShipButtonData CurrentShipButton { get; set; }

                      public VehicleManager vehicle;
                      public Camera playerCamera;

                      public DimensionsManager dimensions; //Is initiated by PlayerWorld
                      public DimensionManager ActiveDimension { get; set; }
    [HideInInspector] public CellData ActiveCell { get; set; }
    [HideInInspector] public FleetManager fleet; //Is initiated by PlayerWorld via InitDimension()
    [HideInInspector] public Color fleetColor;
                      public ShipManager ActiveShip { get; set; }
                      public int X { get; set; }
                      public int Y { get; set; }

    private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        world = GetComponent<PlayerWorldManager>();
        inputHandler = GetComponent<InputHandler>();
        input = GetComponent<PlayerInput>();

        fleetColor = (name == "Player1") ? new Color(0.3f, 0.12f, 0, 1) : new Color(0.3f, 0.3f, 0, 1); // brown or olive
    }

    private void Start()
    {
        world.Initialize();
        HUD.GetComponent<HUD_Initializer>().Initialize();
        inputHandler.Initialize();
        if (number == 1) GameObject.Find("Overworld").GetComponent<OptionsProvider>().debug.ShowCellCoords();
        Invoke("DebugShowShipsOwner", 0.1f);
    }

    private void DebugShowShipsOwner()
    {
        GameObject.Find("Overworld").GetComponent<OptionsProvider>().debug.ShowShipsOwner();
    }
}
