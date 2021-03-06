﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum sides
    {
        Top,
        Right,
        Left,
        Bottom
    }

    public string Name = "Air";
    public bool TopInput = true;
    public bool RightInput = false;
    public bool LeftInput = false;
    public bool RightOutput = false;
    public bool LeftOutput = false;
    public bool BottomOutput = true;

    public float MaxWaterAmount = 1.0f;
    public float MaxWaterFlow = 0.2f;

    public GameObject WaterPrefab;
    public WaterChanger WaterObj;

    public float WaterAmount { get { return waterAmount;} }


    protected float waterAmount = 0.0f;

    // Use this for initialization
    protected virtual void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (waterAmount >= MaxWaterAmount / 2.0f)
        {
            UpdateSides();
        }
    }

    void AddWater(sides sideInput, float amount)
    {
        if (CheckInput(sideInput))
        {
            if (WaterAmount + amount > MaxWaterAmount)
            {
                waterAmount = MaxWaterAmount;
            }
            else
            {
                waterAmount += amount;
            }

            WaterObj.ChangeMesh(WaterAmount / MaxWaterAmount,sideInput);

            OnChangeWater();
        }
    }

    bool CheckInput(sides side)
    {
        switch (side)
        {
            case sides.Top:
                if (TopInput) return true;
                break;
            case sides.Right:
                if (RightInput) return true;
                break;
            case sides.Left:
                if (LeftInput) return true;
                break;
        }
        return false;
    }

    protected void UpdateSides()
    {
        int amountOfOutputs = 0;
        if (LeftOutput) amountOfOutputs++;
        if (RightOutput) amountOfOutputs++;
        if (BottomOutput) amountOfOutputs++;

        if (amountOfOutputs == 0 || WaterAmount <= 0) return;
        float amountToOutput = MaxWaterFlow / amountOfOutputs;

        if (LeftOutput) {
            Block block = World.Instance.GetBlock((int)(transform.position.x) - 1, (int)(transform.position.y), (int)(transform.position.z));
            if (block != null) block.AddWater(sides.Right, amountToOutput);
        }
        if (RightOutput)
        {
            Block block = World.Instance.GetBlock((int)(transform.position.x), (int)(transform.position.y), (int)(transform.position.z) - 1);
            if (block != null) block.AddWater(sides.Left, amountToOutput);
        }
        if (BottomOutput)
        {
            Block block = World.Instance.GetBlock((int)(transform.position.x), (int)(transform.position.y)-1, (int)(transform.position.z));
            if (block != null) block.AddWater(sides.Top, amountToOutput);
        }

        waterAmount -= MaxWaterFlow;
    }

    protected virtual void OnChangeWater() { }
}
