using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QTE")]
public class QTE_ScriptableObject : ScriptableObject
{
    enum QTEType { press, drag, hold, random };
    [Tooltip("-List types of qte in the game")]
    [SerializeField]
    QTEType qteType;
    [SerializeField]
    GameObject backgroundImage, qteImage;
    [SerializeField]
    int numPress, qteDuration;
    [SerializeField]
    float holdDuration;
    [SerializeField]
    Vector2 xy;
    [SerializeField]
    Shape qteScreenSpace;
    
    public GameObject background
    {
        get { return backgroundImage; }
    }
    public GameObject image
    {
        get { return qteImage; }
    }
    public int amount
    {
        get { return numPress; }
    }
    public int duration
    {
        get { return qteDuration; }
    }
    public float holdFor
    {
        get { return holdDuration; }
    }
    public Shape space
    {
        get { return qteScreenSpace; }
    }
    public string type
    {
        get { return qteType.ToString(); }
    }
    public Vector2 direction
    {
        get { return xy; }
    }
}