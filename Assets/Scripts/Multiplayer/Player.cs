using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour
{
    public GameObject opponentObj, eventSystemObj, HUD_Obj, cameraVehicleObj, dimensionsObj, audioManager;

    [HideInInspector] public int number;
    [HideInInspector] public Player opponent;
    [HideInInspector] public PlayerWorld world;
    [HideInInspector] public InputHandling inputHandling;
    [HideInInspector] public PlayerInput input;
    [HideInInspector] public MultiplayerEventSystem eventSystem;
    [HideInInspector] public InputSystemUIInputModule inputSystem;
    [HideInInspector] public HUD_Manager HUD;
    [HideInInspector] public VehicleBehavior vehicle;
    [HideInInspector] public CameraBehavior cameraBehavior;
    [HideInInspector] public Dimensions dimensions; //Is initiated by PlayerWorld
                      public Dimension ActiveDimension { get; set; }
    [HideInInspector] public Cell ActiveCell { get; set; }
    [HideInInspector] public Fleet fleet; //Is initiated by PlayerWorld via InitDimension()

                      public ShipButton CurrentShipButton { get; set; }
                      public Ship ActiveShip { get; set; }
                      public int X { get; set; }
                      public int Y { get; set; }

    private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        opponent = opponentObj.GetComponent<Player>();
        world = GetComponent<PlayerWorld>();
        inputHandling = GetComponent<InputHandling>();
        input = GetComponent<PlayerInput>();
        eventSystem = eventSystemObj.GetComponent<MultiplayerEventSystem>();
        inputSystem = eventSystemObj.GetComponent<InputSystemUIInputModule>();
        HUD = HUD_Obj.GetComponent<HUD_Manager>();
        vehicle = cameraVehicleObj.GetComponent<VehicleBehavior>();

        for (int i = 0; i < cameraVehicleObj.transform.childCount; i++)
        {
            if (cameraVehicleObj.transform.GetChild(i).CompareTag("Camera"))
            {
                cameraBehavior = cameraVehicleObj.transform.GetChild(i).gameObject.GetComponent<CameraBehavior>();
                break;
            }
        }
    }

    private void Start()
    {
        world.InitPlayerWorld();
        HUD.InitHUD();
        inputHandling.InitImputHandling();

        if (number == 1)
        {
            GameObject.Find("Overworld").GetComponent<Overworld>().debug.ShowCellCoords();
        }

        Invoke("DebugShowShipsOwner", 0.1f);
    }

    private void DebugShowShipsOwner()
    {
        GameObject.Find("Overworld").GetComponent<Overworld>().debug.ShowShipsOwner();
    }
}
