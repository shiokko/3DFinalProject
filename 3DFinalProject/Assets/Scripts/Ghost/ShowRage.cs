using Unity.VisualScripting;
using UnityEngine;

public class ShowRage : MonoBehaviour
{
    [SerializeField]
    private GhostController ghost;  
    [SerializeField]
    private MeshRenderer meshRenderer;  

    private float rageNormalized;

    void Start()
    {
 
    }

    void Update()
    {
        
        UpdateMaterialBaseMap();
    }

    void UpdateMaterialBaseMap()
    {
        // 
        rageNormalized = Mathf.Clamp(ghost.GetRage() / 100f, 0f, 1f);

        // 
        Color baseColor = Color.Lerp(Color.white, Color.red, rageNormalized);
        meshRenderer.material.color = baseColor;
    }
}
