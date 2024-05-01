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
                      public FleetManager fleet; //Is initiated by PlayerWorld via InitDimension()
                      public Color fleetColor; // Player 1: Color(0.3f, 0.12f, 0, 1) brown, Player 2: Color(0.3f, 0.3f, 0, 1); olive
                      public ShipManager ActiveShip { get; set; }
                      public int X { get; set; }
                      public int Y { get; set; }

    [Header("Onboarding")]
                      public OnboardingManager onboarding;
    

   private void Awake()
    {
        number = int.Parse(name[^1].ToString());
        world = GetComponent<PlayerWorldManager>();
        input = GetComponent<PlayerInput>();
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
