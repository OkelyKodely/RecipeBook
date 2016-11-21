using System;
using System.Collections.Generic;

[Serializable()]

public class Item : Attribute
{
    public string key = "";

    public string author = "";

    public string cat = "";

    public string value = "";

    public string src = "";

    public Item(string thekey, string thevalue, string thesrc, string author, string cat)
    {
        key = thekey;

        value = thevalue;

        src = thesrc;

        this.author = author;

        this.cat = cat;
    }
    
    public Item()
    {

    }
}

[Serializable()]
public class LList : List<Item>
{

}