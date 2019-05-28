using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class video
{
    private int ID;
    private string title;
    private string description;
    private string section;
    private string date;
    private string Color;

    public video() { }

    public int ID1
    {   get{ return ID;}
        set{ ID = value;}
    }

    public string Title
    {
        get{return title;}
        set{title = value;}
    }

    public string Description
    {
        get{return description;}
        set{description = value;}
    }

    public string Section
    {
        get{return section;}
        set{section = value;}
    }

    public string Date
    {
        get{return date;}
        set{date = value;}
    }

    public string Color1
    {
        get{return Color;}
        set{Color = value;}
    }

    /// <summary>
    /// deprecated
    /// </summary>
    /*public video (string titulo, string descripcion, string seccion, string fecha, string color) {
        title = titulo;
        description = descripcion;
        section = seccion;
        date = fecha;
        Color = color;
    } 
    public string Title { get => title; set => title = value; }
    public string Description { get => description; set => description = value; }
    public string Section { get => section; set => section = value; }
    public string Date { get => date; set => date = value; }
    public string Color1 { get => Color; set => Color = value; }
    */


}
