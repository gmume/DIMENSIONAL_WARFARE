using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour
{
    public int number;

    public GameObject obj;
    public PlayerWorld world;
    //public PlayerData data;
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

    public Dimension ActiveDimension { get; set; }
    public Cell ActiveCell { get; set; }
    public ShipButton CurrentShipButton { get; set; }
    public Ship ActiveShip { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    private void Awake()
    {
        number = int.Parse(this.name[this.name.Length - 1].ToString());
        obj = GameObject.Find(this.name);
        world = obj.GetComponent<PlayerWorld>();
        //data = world.playerData;
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
        
    }
}
