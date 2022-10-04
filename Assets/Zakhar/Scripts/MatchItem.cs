using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MatchItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerUpHandler
{
    static MatchItem hoverItem;

    public GameObject WirePrefab;
    public string itemName;

    private GameObject line;

    public void OnPointerDown(PointerEventData eventData)
    {
        line = Instantiate(WirePrefab, transform.position, Quaternion.identity, transform.parent.parent);
        line.transform.SetAsFirstSibling();
        UpdateLine(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateLine(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData) // Checks if connected items were right
    {
        if (!this.Equals(hoverItem) && itemName.Equals(hoverItem.itemName))
        {
            UpdateLine(hoverItem.transform.position);
            MatchLogic.AddPoint();
            Destroy(hoverItem);
            Destroy(this);
        }
        else
        {
            Destroy(line);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverItem = this;
    }


    void UpdateLine(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        line.transform.right = direction;

        line.transform.localScale = new Vector3(direction.magnitude, 1, 1);
    }
}
