using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItemTable
{
    Slot[][][][] table = new Slot[6][][][];
    Item[][] raceItem = new Item[6][];

    public BonusItemTable()
    {
        for(int i=0; i < raceItem.Length; i++)
        {
            raceItem[i] = new Item[2];
        }
        LoadRaceItemData();

        for (int i=0;i<table.Length;i++)
        {
            table[i] = new Slot[5][][];
            for(int j=0;j<table[i].Length;j++)
            {
                table[i][j] = new Slot[4][];
                for(int k=0; k<table[i][j].Length;k++)
                {
                    table[i][j][k] = new Slot[2];
                    table[i][j][k][0].slotItem = raceItem[i][0];
                    table[i][j][k][1].slotItem = raceItem[i][1];
                }
            }
        }
        LoadHumanItemNumberData();
        LoadGoblinItemNumberData();
        LoadElfItemNumberData();
        LoadOakItemNumberData();
        LoadMachineItemNumberData();
    }

    #region Initialize Methods

    private void LoadRaceItemData()
    {
        Item leather = Resources.Load("Items/Ingredient/가죽") as Item;
        Item gold = Resources.Load("Items/Ingredient/금가루") as Item;
        Item naturalEssence = Resources.Load("Items/Ingredient/자연의 정수") as Item;
        Item toughLeather = Resources.Load("Items/Ingredient/질긴 가죽") as Item;
        Item scrap = Resources.Load("Items/Ingredient/고철") as Item;
        Item part = Resources.Load("Items/Ingredient/기계 부품") as Item;
        raceItem[(int)Race.Human][0] = leather;
        raceItem[(int)Race.Goblin][0] = gold;
        raceItem[(int)Race.Goblin][1] = leather;
        raceItem[(int)Race.Elf][0] = naturalEssence;
        raceItem[(int)Race.Oak][0] = leather;
        raceItem[(int)Race.Oak][1] = toughLeather;
        raceItem[(int)Race.Machine][0] = scrap;
        raceItem[(int)Race.Machine][1] = part;
    }

    private void LoadHumanItemNumberData()
    {
        table[(int)Race.Human][(int)Grade.D][0][0].slotItemNumber = 1;
        table[(int)Race.Human][(int)Grade.D][0][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][1][0].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][1][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][2][0].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][2][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][3][0].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.D][3][1].slotItemNumber = 0;

        table[(int)Race.Human][(int)Grade.C][0][0].slotItemNumber = 1;
        table[(int)Race.Human][(int)Grade.C][0][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.C][1][0].slotItemNumber = 1;
        table[(int)Race.Human][(int)Grade.C][1][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.C][2][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.C][2][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.C][3][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.C][3][1].slotItemNumber = 0;

        table[(int)Race.Human][(int)Grade.B][0][0].slotItemNumber = 1;
        table[(int)Race.Human][(int)Grade.B][0][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.B][1][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.B][1][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.B][2][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.B][2][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.B][3][0].slotItemNumber = 3;
        table[(int)Race.Human][(int)Grade.B][3][1].slotItemNumber = 0;

        table[(int)Race.Human][(int)Grade.A][0][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.A][0][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.A][1][0].slotItemNumber = 2;
        table[(int)Race.Human][(int)Grade.A][1][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.A][2][0].slotItemNumber = 3;
        table[(int)Race.Human][(int)Grade.A][2][1].slotItemNumber = 0;
        table[(int)Race.Human][(int)Grade.A][3][0].slotItemNumber = 3;
        table[(int)Race.Human][(int)Grade.A][3][1].slotItemNumber = 0;
    }

    private void LoadGoblinItemNumberData()
    {
        table[(int)Race.Goblin][(int)Grade.D][0][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.D][0][1].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.D][1][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.D][1][1].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.D][2][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.D][2][1].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.D][3][0].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.D][3][1].slotItemNumber = 0;

        table[(int)Race.Goblin][(int)Grade.C][0][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.C][0][1].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.C][1][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.C][1][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.C][2][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.C][2][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.C][3][0].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.C][3][1].slotItemNumber = 1;

        table[(int)Race.Goblin][(int)Grade.B][0][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.B][0][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.B][1][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.B][1][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.B][2][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.B][2][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.B][3][0].slotItemNumber = 1;
        table[(int)Race.Goblin][(int)Grade.B][3][1].slotItemNumber = 2;

        table[(int)Race.Goblin][(int)Grade.A][0][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.A][0][1].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.A][1][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.A][1][1].slotItemNumber = 3;
        table[(int)Race.Goblin][(int)Grade.A][2][0].slotItemNumber = 0;
        table[(int)Race.Goblin][(int)Grade.A][2][1].slotItemNumber = 3;
        table[(int)Race.Goblin][(int)Grade.A][3][0].slotItemNumber = 2;
        table[(int)Race.Goblin][(int)Grade.A][3][1].slotItemNumber = 2;
    }

    private void LoadElfItemNumberData()
    {
        table[(int)Race.Elf][(int)Grade.D][0][0].slotItemNumber = 1;
        table[(int)Race.Elf][(int)Grade.D][0][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.D][1][0].slotItemNumber = 1;
        table[(int)Race.Elf][(int)Grade.D][1][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.D][2][0].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.D][2][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.D][3][0].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.D][3][1].slotItemNumber = 0;

        table[(int)Race.Elf][(int)Grade.C][0][0].slotItemNumber = 1;
        table[(int)Race.Elf][(int)Grade.C][0][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.C][1][0].slotItemNumber = 1;
        table[(int)Race.Elf][(int)Grade.C][1][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.C][2][0].slotItemNumber = 2;
        table[(int)Race.Elf][(int)Grade.C][2][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.C][3][0].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.C][3][1].slotItemNumber = 0;

        table[(int)Race.Elf][(int)Grade.B][0][0].slotItemNumber = 2;
        table[(int)Race.Elf][(int)Grade.B][0][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.B][1][0].slotItemNumber = 2;
        table[(int)Race.Elf][(int)Grade.B][1][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.B][2][0].slotItemNumber = 3;
        table[(int)Race.Elf][(int)Grade.B][2][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.B][3][0].slotItemNumber = 1;
        table[(int)Race.Elf][(int)Grade.B][3][1].slotItemNumber = 0;

        table[(int)Race.Elf][(int)Grade.A][0][0].slotItemNumber = 2;
        table[(int)Race.Elf][(int)Grade.A][0][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.A][1][0].slotItemNumber = 3;
        table[(int)Race.Elf][(int)Grade.A][1][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.A][2][0].slotItemNumber = 2;
        table[(int)Race.Elf][(int)Grade.A][2][1].slotItemNumber = 0;
        table[(int)Race.Elf][(int)Grade.A][3][0].slotItemNumber = 3;
        table[(int)Race.Elf][(int)Grade.A][3][1].slotItemNumber = 0;
    }

    private void LoadOakItemNumberData()
    {
        table[(int)Race.Oak][(int)Grade.D][0][0].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.D][0][1].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.D][1][0].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.D][1][1].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.D][2][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.D][2][1].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.D][3][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.D][3][1].slotItemNumber = 1;

        table[(int)Race.Oak][(int)Grade.C][0][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.C][0][1].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.C][1][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.C][1][1].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.C][2][0].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.C][2][1].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.C][3][0].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.C][3][1].slotItemNumber = 2;

        table[(int)Race.Oak][(int)Grade.B][0][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.B][0][1].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.B][1][0].slotItemNumber = 3;
        table[(int)Race.Oak][(int)Grade.B][1][1].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.B][2][0].slotItemNumber = 1;
        table[(int)Race.Oak][(int)Grade.B][2][1].slotItemNumber = 3;
        table[(int)Race.Oak][(int)Grade.B][3][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.B][3][1].slotItemNumber = 3;

        table[(int)Race.Oak][(int)Grade.A][0][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.A][0][1].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.A][1][0].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.A][1][1].slotItemNumber = 3;
        table[(int)Race.Oak][(int)Grade.A][2][0].slotItemNumber = 3;
        table[(int)Race.Oak][(int)Grade.A][2][1].slotItemNumber = 2;
        table[(int)Race.Oak][(int)Grade.A][3][0].slotItemNumber = 3;
        table[(int)Race.Oak][(int)Grade.A][3][1].slotItemNumber = 3;
    }

    private void LoadMachineItemNumberData()
    {
        table[(int)Race.Machine][(int)Grade.D][0][0].slotItemNumber = 3;
        table[(int)Race.Machine][(int)Grade.D][0][1].slotItemNumber = 0;
        table[(int)Race.Machine][(int)Grade.D][1][0].slotItemNumber = 3;
        table[(int)Race.Machine][(int)Grade.D][1][1].slotItemNumber = 0;
        table[(int)Race.Machine][(int)Grade.D][2][0].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.D][2][1].slotItemNumber = 1;
        table[(int)Race.Machine][(int)Grade.D][3][0].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.D][3][1].slotItemNumber = 2;

        table[(int)Race.Machine][(int)Grade.C][0][0].slotItemNumber = 3;
        table[(int)Race.Machine][(int)Grade.C][0][1].slotItemNumber = 1;
        table[(int)Race.Machine][(int)Grade.C][1][0].slotItemNumber = 4;
        table[(int)Race.Machine][(int)Grade.C][1][1].slotItemNumber = 1;
        table[(int)Race.Machine][(int)Grade.C][2][0].slotItemNumber = 4;
        table[(int)Race.Machine][(int)Grade.C][2][1].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.C][3][0].slotItemNumber = 5;
        table[(int)Race.Machine][(int)Grade.C][3][1].slotItemNumber = 2;

        table[(int)Race.Machine][(int)Grade.B][0][0].slotItemNumber = 4;
        table[(int)Race.Machine][(int)Grade.B][0][1].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.B][1][0].slotItemNumber = 5;
        table[(int)Race.Machine][(int)Grade.B][1][1].slotItemNumber = 3;
        table[(int)Race.Machine][(int)Grade.B][2][0].slotItemNumber = 5;
        table[(int)Race.Machine][(int)Grade.B][2][1].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.B][3][0].slotItemNumber = 5;
        table[(int)Race.Machine][(int)Grade.B][3][1].slotItemNumber = 1;

        table[(int)Race.Machine][(int)Grade.A][0][0].slotItemNumber = 5;
        table[(int)Race.Machine][(int)Grade.A][0][1].slotItemNumber = 3;
        table[(int)Race.Machine][(int)Grade.A][1][0].slotItemNumber = 6;
        table[(int)Race.Machine][(int)Grade.A][1][1].slotItemNumber = 2;
        table[(int)Race.Machine][(int)Grade.A][2][0].slotItemNumber = 6;
        table[(int)Race.Machine][(int)Grade.A][2][1].slotItemNumber = 4;
        table[(int)Race.Machine][(int)Grade.A][3][0].slotItemNumber = 6;
        table[(int)Race.Machine][(int)Grade.A][3][1].slotItemNumber = 5;
    }

    #endregion

    public Slot[] GetTableData(Race race, Grade grade, int probabilityIndex)
    {
        if(probabilityIndex < 0 || probabilityIndex > 3)
        {
            Debug.Log("Wrong Index!");
            probabilityIndex = 0;
        }
        if(grade == Grade.S)
        {
            grade = Grade.A;
        }
        return table[(int)race][(int)grade][probabilityIndex];
    }
}