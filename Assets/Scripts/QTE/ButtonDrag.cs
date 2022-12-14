using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDrag : QTEButton
{
    Vector3 originalPos;
    RectTransform rect;
    Vector2 offset;
    Vector3 scaledTarget;
    GameObject target;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        //rect.eulerAngles = targetPos;
        originalPos = rect.localPosition;
        scaledTarget = new Vector2(rect.rect.width * targetPos.x, rect.rect.height * targetPos.y) + (Vector2)originalPos;
        target = new GameObject();
        target.transform.parent = transform.parent;
        tarImage = target.AddComponent<Image>();
        tarImage.sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        tarImage.type = Image.Type.Filled;
        tarImage.fillMethod = Image.FillMethod.Horizontal;
        tarImage.fillAmount = 0;
        target.GetComponent<RectTransform>().localPosition = new Vector3(scaledTarget.x-100,scaledTarget.y-100,scaledTarget.z);
        target.transform.localScale = new Vector3(8,6,1);
        Vector2 rotateTarget = Vector2.Scale(targetPos, rect.rect.size);
        Vector2 relative = transform.InverseTransformPoint(transform.position - (Vector3)rotateTarget);
        Vector2 fixedRelative = new Vector2(relative.y,-relative.x);
        float angle = Mathf.Atan2(fixedRelative.x, fixedRelative.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
        target.transform.rotation = transform.rotation;
        distance = Vector3.Distance(rect.localPosition,scaledTarget);
    }

    void OnMouseDown()
    {
        Cursor.visible = false;
        offset = -rect.localPosition + new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }

    void OnMouseUp()
    {
        Cursor.visible = true;
        rect.localPosition = originalPos;
    }

    void OnMouseDrag()
    {
        rect.localPosition = new Vector3(Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y,-1);
        tarImage.fillAmount = Mathf.Clamp(1-(Vector3.Distance(rect.localPosition, scaledTarget)/distance)/1.2f,0,1);
        //if(rect.rect.Contains(transform.TransformPoint(scaledTarget)))
        if (rect.localPosition.x <= scaledTarget.x  && scaledTarget.x <= rect.localPosition.x + rect.rect.width && rect.localPosition.y <= scaledTarget.y && scaledTarget.y <= rect.localPosition.y + rect.rect.height)
        {
            MusicControlScript.Instance.PlayQTE();
            Destroy(target);
            Succeed();
        }
            
    }
}