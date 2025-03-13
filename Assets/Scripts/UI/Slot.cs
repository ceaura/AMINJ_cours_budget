using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<DraggableStat>().SwitchParent(eventData.pointerDrag.GetComponent<DraggableStat>().GetParent());
        }
        eventData.pointerDrag.GetComponent<DraggableStat>().SetParentAfterDrag(transform);
    }
}
