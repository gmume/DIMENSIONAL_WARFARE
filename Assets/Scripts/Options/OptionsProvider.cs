using UnityEngine;

public class OptionsProvider : MonoBehaviour
{
    [Range(1,5)] [SerializeField] private int dimensionsCount;
    [Range(5, 19)] [SerializeField] private float dimensionSize; //Should be uneven!
    [Range(1, 5)] [SerializeField]  private int fleetSize;

    public Debugger debug;

    private void Awake()
    {
        OverworldData.DimensionsCount = dimensionsCount;
        OverworldData.DimensionSize = (int)dimensionSize;
        OverworldData.DimensionDiagonal = dimensionSize * Mathf.Sqrt(2);
        OverworldData.MiddleCoordNo = Mathf.FloorToInt(dimensionSize / 2);
        OverworldData.FleetSize = fleetSize;
        OverworldData.PlayerTurn = 0;
        OverworldData.Player1SubmittedFleet = false;
        OverworldData.Player2SubmittedFleet = false;
        OverworldData.Player1 = GameObject.Find("Player1").GetComponent<PlayerData>();
        OverworldData.Player2 = GameObject.Find("Player2").GetComponent<PlayerData>();
    }

    private void Start()
    {
        debug = GameObject.Find("Debugging").GetComponent<Debugger>();
        debug.Initialize();
    }

    public void OnValidate() => dimensionSize = 1 + (((int)(dimensionSize + 1.0f) - 1) & 0xFFFFFFFE);
}
