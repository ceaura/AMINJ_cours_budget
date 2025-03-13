using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableStat : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI[] m_TextMeshProUGUIs;
    [SerializeField] Image[] images;
    private Transform _draggableParent, parentAfterDrag;

    private void Start()
    {
        _draggableParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        for (int i = 0; i < m_TextMeshProUGUIs.Length; i++)
        {
            m_TextMeshProUGUIs[i].raycastTarget = false;
        }
        for (int i = 0; i < images.Length; i++)
        {
            images[i].raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SwitchParent(parentAfterDrag);
        for (int i = 0; i < m_TextMeshProUGUIs.Length; i++)
        {
            m_TextMeshProUGUIs[i].raycastTarget = true;
        }
        for (int i = 0; i < images.Length; i++)
        {
            images[i].raycastTarget = true;
        }
    }

    public void SetParentAfterDrag(Transform parentAfterDrg)
    {
        parentAfterDrag = parentAfterDrg;
    }
    public Transform GetParent()
    {
        return _draggableParent;
    }
    public void SwitchParent(Transform parent)
    {
        transform.SetParent(parent);
        _draggableParent = parent;
    }
}
