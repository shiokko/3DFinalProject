using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowRage : MonoBehaviour
{
    [SerializeField]
    private List<GhostController> ghosts;
    [SerializeField]
    private GhostController TargetGhost;  
    [SerializeField]
    private MeshRenderer meshRenderer;  

    private float rageNormalized;

    void Start()
    {
        FindActiveGhost();  
    }

    void Update()
    {
        
        UpdateMaterialBaseMap();
    }

    void UpdateMaterialBaseMap()
    {
        // 
        rageNormalized = Mathf.Clamp(TargetGhost.GetRage() / 100f, 0f, 1f);
        Color baseColor = Color.white;

        if (TargetGhost.GetStatus() == GhostController.Status.Hunt)
        {
            baseColor = Color.Lerp(Color.white, Color.black, rageNormalized);
        }
        else if (TargetGhost.GetStatus() == GhostController.Status.YellAt)
        {
            baseColor = Color.Lerp(Color.white, Color.red, rageNormalized);  
        }
        else if(TargetGhost.GetStatus() == GhostController.Status.Scare)
        {
            baseColor = Color.Lerp(Color.white, Color.blue, rageNormalized);  
        }
        meshRenderer.material.color = baseColor;
    }
    void FindActiveGhost()
    {
        // Loop through the list and find the active ghost
        foreach (var ghost in ghosts)
        {
            if (ghost.gameObject.activeSelf) // Check if the ghost is active
            {
                TargetGhost = ghost;
                break;
            }
        }

        if (TargetGhost == null)
        {
            Debug.LogError("No active ghost found!");
        }
        else
        {
            Debug.Log("Active ghost found: " + TargetGhost.name);
        }
    }
}
