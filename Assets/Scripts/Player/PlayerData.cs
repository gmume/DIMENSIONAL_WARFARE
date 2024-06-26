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
    public Submitter submitter;
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
    public Pointer Pointer;
    public CellData FocusedCell { get; set; }
    public Color CellColor { get; set; }
    public Material CellMaterial { get; set; }
    public FadeEffect Fade;

    [Header("Fleet")]
    public FleetManager fleet;
    public ShipManager ActiveShip { get; set; }
    public ShipManager LastActiveShip { get; set; }
    [HideInInspector] public Color fleetColor;

    [Header("Onboarding")]
    public OnboardingManager onboarding;

    [Header("Options")]
    public OptionsHandler options;

    private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        world = GetComponent<PlayerWorldManager>();
        input = GetComponent<PlayerInput>();
        CellColor = (number == 1) ? Colors.cellTurqoise : Colors.cellBlue;
        CellMaterial = (number == 1) ? Materials.cellBlueMat : Materials.cellTurqoiseMat;
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
