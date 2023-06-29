using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour
{
    public int number;
    public Player opponent;

    public GameObject obj;
    public PlayerWorld world;
    public InputHandling inputHandling;
    public PlayerInput input;

    public GameObject eventSystemObj;
    public MultiplayerEventSystem eventSystem;
    public InputSystemUIInputModule inputSystem;

    public GameObject fleetMenuObj;
    public FleetMenuScript fleetMenu;

    public GameObject cameraVehicleObj;
    public VehicleBehavior vehicle;
    public GameObject cameraObj;
    public CameraBehavior cameraBehavior;

    public Dimensions dimensions; //Is initiated by PlayerWorld
    public Dimension ActiveDimension { get; set; }
    public Cell ActiveCell { get; set; }

    public Fleet fleet; //Is initiated by PlayerWorld
    public ShipButton CurrentShipButton { get; set; }
    public Ship ActiveShip { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    private void Awake()
    {
        number = int.Parse(name[^1].ToString());

        if (number == 1)
        {
            opponent = GameObject.Find("Player2").GetComponent<Player>();
        }
        else
        {
            opponent = GameObject.Find("Player1").GetComponent<Player>();
        }

        obj = GameObject.Find(name);
        world = obj.GetComponent<PlayerWorld>();
        inputHandling = obj.GetComponent<InputHandling>();
        input = obj.GetComponent<PlayerInput>();

        eventSystemObj = GameObject.Find("EventSystem" + number);
        eventSystem = eventSystemObj.GetComponent<MultiplayerEventSystem>();
        inputSystem = eventSystemObj.GetComponent<InputSystemUIInputModule>();

        fleetMenuObj = GameObject.Find("FleetMenu" + number);
        fleetMenu = fleetMenuObj.GetComponent<FleetMenuScript>();

        cameraVehicleObj = GameObject.Find("CameraVehicle" + number);
        vehicle = cameraVehicleObj.GetComponent<VehicleBehavior>();
        cameraObj = GameObject.Find("Camera" + number);
        cameraBehavior = GameObject.Find("Camera" + number).GetComponent<CameraBehavior>();
    }

    private void Start()
    {
        world.InitPlayerWorld(this);
        fleetMenu.InitFleetMenu(this);
        inputHandling.InitImputHandling(this);

        if(number == 1)
        {
            GameObject.Find("Overworld").GetComponent<Overworld>().debug.ShowCellCoords();
        }
    }
}
