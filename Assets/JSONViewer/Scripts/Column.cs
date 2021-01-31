using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> Column, toda la magia del sur. </summary>
[Serializable]
public class Column
{
    public RectTransform container;
    public RectTransform trColumn;
    public TMP_Text lblHeader;
    public List<Cell> cells;

    public Column(RectTransform container, string title)
    {
        cells = new List<Cell>();
        lblHeader = JsonTable.CreateInstance(container);
        lblHeader.fontStyle = FontStyles.Bold;
        lblHeader.text = title;
        lblHeader.name = $"Column - {title}";
        trColumn = lblHeader.GetComponent<RectTransform>();
    }

    private Cell AddCell(int rowCount, string content = "")
    {
        Cell cell = new Cell(trColumn, JsonTable.CreateInstance(trColumn), rowCount, content);
        cells.Add(cell);
        return cell;
    }

    public void SetCell(int rowCount, string content)
    {
        Cell cell;
        for (int i = cells.Count - 1; i >= 0; i--)
        {
            cell = cells[i];
            if (cell.rowCount == rowCount)
            {
                cell.SetCell(rowCount, content);
                return;
            }
        }

        AddCell(rowCount, content);
    }

    public void Switch(bool @switch) => trColumn.gameObject.SetActive(@switch);
    public void Enable() => Switch(true);
    public void Disable()
    {
        Switch(false);
        for (int i = cells.Count - 1; i >= 0; i--)
            cells[i].Disable();
    }
}

[Serializable]
public class Cell
{
    public RectTransform column;
    public RectTransform trCell;
    public TMP_Text label;
    public int rowCount;

    public Cell(RectTransform column, TMP_Text label, int rowCount, string content)
    {
        this.column = column;
        this.label = label;
        this.rowCount = rowCount;
        trCell = this.label.GetComponent<RectTransform>();
        SetCell(rowCount, content);
    }

    public void SetCell(int rowCount, string content)
    {
        label.text = content;
        trCell.localPosition = new Vector3(0, rowCount * -JsonTable.CellHeight, 0);
        Enable();
    }

    public void Switch(bool @switch) => trCell.gameObject.SetActive(@switch);
    public void Enable() => Switch(true);
    public void Disable() => Switch(false);
}
