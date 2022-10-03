public class Part
{
    string type;
    float durability;

    public string name
    {
        get { return type; }
        set { type = value; }
    }
    public float dur
    {
        get { return durability; }
        set { durability = value; }
    }
}