using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            Debug.Log("Clicked");
            if (Physics.Raycast(ray, out hit))
            {
                string parentName = hit.collider.transform.parent.name;
                if (parentName.Substring(parentName.Length - 4) == "Hand")
                {
                    CardMatching.instance.FindMatch(hit.collider.tag);
                }
            }
        }
    }
}
