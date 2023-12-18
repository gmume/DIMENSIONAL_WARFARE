using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerData : MonoBehaviour
{
    [HideInInspector] public int number;
                      public PlayerData opponent;
    [HideInInspector] public PlayerWorld world;

                      public GameObject audioManager;

    [HideInInspector] public InputHandling inputHandling;
    [HideInInspector] public PlayerInput input;
                      public MultiplayerEventSystem eventSystem;
                      public InputSystemUIInputModule inputSystem;

                      public HUD_Manager HUD;
                      public ShipButton CurrentShipButton { get; set; }

                      public VehicleBehavior vehicle;
                      public Camera playerCamera;

                      public Dimensions dimensions; //Is initiated by PlayerWorld
                      public Dimension ActiveDimension { get; set; }
    [HideInInspector] public Cell ActiveCell { get; set; }
    [HideInInspector] public Fleet fleet; //Is initiated by PlayerWorld via InitDimension()
                      public ShipManager ActiveShip { get; set; }
                      public int X { get; set; }
                      public int Y { get; set; }

    private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        world = GetComponent<PlayerWorld>();
        inputHandling = GetComponent<InputHandling>();
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        world.Initialize();
        HUD.Initialize();
        inputHandling.Initialize();
        if (number == 1) GameObject.Find("Overworld").GetComponent<Overworld>().debug.ShowCellCoords();
        Invoke("DebugShowShipsOwner", 0.1f);
    }

    private void DebugShowShipsOwner()
    {
        GameObject.Find("Overworld").GetComponent<Overworld>().debug.ShowShipsOwner();
    }
}
