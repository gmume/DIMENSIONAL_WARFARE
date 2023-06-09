using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dimension : MonoBehaviour
{
    private Player player;
    private GameObject[][] cells;
    private readonly List<GameObject> ships = new();
    
    public int DimensionNr { get; private set; }

    public void InitDimension(Player player, int nr, GameObject cellPrefab, ArrayList fleet)
    {
        this.player = player;
        DimensionNr = nr;
        CreateCells(cellPrefab);

        if(DimensionNr == 0)
        {
            player.ActiveDimension = GetComponent<Dimension>();
             
            for (int i = 0; i < fleet.Count; i++)
            {
                GameObject shipObj = (GameObject)fleet[i];
                Ship ship = shipObj.GetComponent<Ship>();
                ship.Dimension = this;
                ship.OccupyCell();

                //if(player.number == 1 && DimensionNr == 0)
                //{
                //    Debug.Log("ship: " + ship.X + ", " + ship.Z);
                //}
                
            }

            AddShips(fleet);
        }

        //if (player.number == 1 && DimensionNr == 0)
        //{
        //    Cell cell;
        //    foreach (var cellObj in cells[0])
        //    {
        //        cell = cellObj.GetComponent<Cell>();
        //        Debug.Log("occupied? " + cell.X + ", " + cell.Y + ": " + cell.Occupied);
        //    }
        //}
    }

    public void CreateCells(GameObject cellPrefab)
    {
        cells = new GameObject[OverworldData.DimensionSize][];

        for (int i = 0; i < OverworldData.DimensionSize; i++)
        {
            cells[i] = new GameObject[OverworldData.DimensionSize];

            for (int j = 0; j < OverworldData.DimensionSize; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(i, OverworldData.DimensionSize * DimensionNr * 2, j), Quaternion.identity);

                cell.layer = Layer.SetLayerPlayer(player);
                cell.transform.parent = transform;
                cell.GetComponent<Cell>().X = j;
                cell.GetComponent<Cell>().Y = i;
                cell.GetComponent<Cell>().Activated = false;
                cell.GetComponent<Cell>().Occupied = false;
                cell.GetComponent<Cell>().Hitted = false;
                cells[i][j] = cell;

                //////////
                GameObject canvasObj;
                GameObject myText;
                Canvas myCanvas;
                Text text;

                canvasObj = new GameObject("TestCanvas", typeof(Canvas), typeof(GraphicRaycaster));
                canvasObj.transform.SetParent(cell.transform, false);
                RectTransform trans = canvasObj.GetComponent<RectTransform>();
                trans.Rotate(new Vector3(90, 0, 0));

                Transform transParent = canvasObj.GetComponent<Transform>();
                trans.position = new Vector3(transParent.position.x, 0.51f, transParent.position.z);
                trans.sizeDelta = new Vector3(1, 1, 0);

                myCanvas = canvasObj.GetComponent<Canvas>();
                myCanvas.renderMode = RenderMode.WorldSpace;

                myText = new GameObject("cellText", typeof(Text));
                myText.transform.SetParent(myCanvas.transform, false);
                myText.GetComponent<RectTransform>().localScale = new Vector3(0.003f, 0.003f, 1);

                text = myText.GetComponent<Text>();
                text.font = (Font)Resources.Load("arial");
                text.text = i+","+j;
                text.fontSize = 150;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleCenter;
                ///////////////////
            }
        }

        if(player.number == 1 && DimensionNr == 1)
        {
            string cellsRow0 = "cells: ";
            foreach (var cellObj in cells[0])
            {
                Cell cell = cellObj.GetComponent<Cell>();
                cellsRow0 += "("+cell.X +", "+cell.Y+")";
            }
            string cellsRow1 = "cells: ";
            foreach (var cellObj in cells[1])
            {
                Cell cell = cellObj.GetComponent<Cell>();
                cellsRow1 += "(" + cell.X + ", " + cell.Y + ")";
            }

            Debug.Log(cellsRow1);
            Debug.Log(cellsRow0);
        }
        
    }

    public GameObject GetCell(int x, int y)
    {
        return cells[x][y];
    }

    public void AddShips(ArrayList newShips)
    {
        foreach(GameObject ship in newShips)
        {
            if (ship.GetComponent<Ship>().Dimension == this)
            {
                ship.transform.SetParent(GetComponent<Transform>().transform, true);
                ships.Add(ship);
            }
        }
    }

    public GameObject GetShipOnCell(int x, int y)
    {
        bool found = false;
        int i = 0;

        try
        {
            while (!found && i < ships.Count)
            {
                GameObject shipObj = ships[i];
                Ship ship = shipObj.GetComponent<Ship>();
                if (ship.X == x && ship.Z == y)
                {
                    found = true;
                    return shipObj;
                }
                else
                {
                    i++;
                }
            }
        }
        catch (System.Exception)
        {
            Debug.Log("No ship found!");
            throw;
        }

        return null;
    }
}
