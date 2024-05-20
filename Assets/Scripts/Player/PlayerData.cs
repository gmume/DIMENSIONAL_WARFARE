using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerData : MonoBehaviour
{
    [HideInInspector] public int number;
    [Header("Player opponent")]
                      public PlayerData opponent;
    [HideInInspector] public PlayerWorldManager world;

    [Header("Audio")]
                      public AudioPlayer audioManager;

    [Header("Input")]
                      public InputHandler inputHandler;
                      public FleetSubmitter submitter;
                      public PlayerSwapper swapper;
                      public InputEnabler inputEnabler;

    [HideInInspector] public PlayerInput input;
                      public MultiplayerEventSystem eventSystem;
                      public InputSystemUIInputModule inputSystem;

    [Header("HUD")]
                      public HUD_Manager HUD;
                      public ShipButtonData CurrentShipButton { get; set; }

    [Header("Camera")]
                      public VehicleManager vehicle;
                      public Camera playerCamera;

    [Header("World")]
                      public DimensionsManager dimensions; //Is initiated by PlayerWorld
                      public DimensionManager ActiveDimension { get; set; }
    [HideInInspector] public CellData FocusedCell { get; set; }
    //[HideInInspector] public Material CellMaterial { get; set; }

    [Header("Fleet")]
                      public FleetManager fleet; //Is initiated by PlayerWorld via InitDimension()
    [HideInInspector] public Color fleetColor;
                      public ShipManager ActiveShip { get; set; }
    [HideInInspector] public ShipManager LastActiveShip { get; set; }

    [Header("Onboarding")]
                      public OnboardingManager onboarding;
    

   private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        world = GetComponent<PlayerWorldManager>();
        input = GetComponent<PlayerInput>();
        //CellMaterial = (number == 1) ? Materials.cellBlueMat : Materials.cellTurqoiseMat;
        fleetColor = (number == 1) ? Colors.fleet1 : Colors.fleet2;
    }

    private void Start()
    {
        world.Initialize(this);
        GetComponent<InputInitializer>().Initialize();

        //if (number == 1) Invoke("DebugShowCellOnCoords", 0.5f);
        //Invoke("DebugShowShipsOwner", 0.1f);
    }

    private void DebugShowCellOnCoords() => GameObject.Find("Overworld").GetComponent<OptionsProvider>().debug.ShowCellCoords();

    private void DebugShowShipsOwner() => GameObject.Find("Overworld").GetComponent<OptionsProvider>().debug.ShowShipsOwner();
}
