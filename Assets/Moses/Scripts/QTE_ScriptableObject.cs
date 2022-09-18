using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QTE")]
public class QTE_ScriptableObject : ScriptableObject
{
    [SerializeField]
    [Tooltip("-List types of qte in the game")]
    public string qteType;
    public GameObject backgroundImage;

    public int numPress;
    public int qteDuration;
    public Shape qteScreenSpace;
}