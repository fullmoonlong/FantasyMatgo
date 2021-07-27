using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Clicked");
            if (Physics.Raycast(ray, out hit))
            {
                string parentName = hit.collider.transform.parent.name;
                if (CardManager.instance.myHand.Contains(hit.collider.gameObject) || CardManager.instance.opponentHand.Contains(hit.collider.gameObject))
                {
                    //CardMatching.instance.FindMatch(hit.collider.tag);
                }
            }
        }
    }
}
