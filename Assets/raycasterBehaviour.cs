using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycasterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check for a touch or click
        if (Input.GetMouseButtonDown(0))
        {

            // Cast a ray from the camera to the clicked position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Layer mask to ignore the UI layer
            int layerMask = 1 << LayerMask.NameToLayer("UI");
            layerMask = ~layerMask; // Invert the mask to ignore the UI layer

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // Check if the hit object has the PrefabClickAction component
                towerSpawnCellBehaviour clickAction = hit.collider.GetComponent<towerSpawnCellBehaviour>();
                if (clickAction != null)
                {
                    clickAction.onRaycast();
                }
            }
        }
    }
}
