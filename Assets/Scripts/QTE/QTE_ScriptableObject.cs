using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QTE")]
public class QTE_ScriptableObject : ScriptableObject
{
    [Tooltip("-List types of qte in the game")]
    [SerializeField]
    string qteType;
    [SerializeField]
    GameObject backgroundImage, qteImage;
    [SerializeField]
    int numPress, qteDuration;
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
    public Shape space
    {
        get { return qteScreenSpace; }
    }
}