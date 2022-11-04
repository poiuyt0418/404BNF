public class Part
{
    string type;
    float durability;
    float speedChange = 5;
    enum Usage { step, usage, time };
    Usage durabilityLoss;

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
    public float speed
    {
        get
        {
            if (type == "leg")
            {
                return speedChange;
            }
            else
            {
                return 0;
            }
        }
        set { speedChange = value; }
    }
    public string usage
    {
        get { return durabilityLoss.ToString(); }
        //set { speedChange = float.Parse(value); }
    }
    public void SetUsage()
    {
        if (type == "leg")
        {
            durabilityLoss = Usage.step;
        }
        else if(type == "arm")
        {
            durabilityLoss = Usage.usage;
        }
        else if(type == "body")
        {
            durabilityLoss = Usage.time;
        }
    }
}