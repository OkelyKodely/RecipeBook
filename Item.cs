using System;
using System.Collections.Generic;

[Serializable()]
public class Item : Attribute
{

    public string key = "";
    public string value = "";

    public Item(string thekey, string thevalue)
    {
        key = thekey;
        value = thevalue;
    }
    public Item() { }
}

[Serializable()]
public class LList : List<Item>
{

}