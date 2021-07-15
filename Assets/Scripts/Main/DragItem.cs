using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Image))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private Vector2 draggingOffset = new Vector2(0f, 40f);
    private GameObject draggingObject;
    private RectTransform canvasRectTransform;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start");
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draging");
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        // throw new System.NotImplementedException();
    }

}
