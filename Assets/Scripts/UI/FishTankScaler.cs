using UnityEngine;


[RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
public class FishTankMeshScaler : MonoBehaviour
{
    [SerializeField] private float tankHeightWorldUnits = 2f;
    [SerializeField] private float bottomMargin = 0.1f;

    private Camera mainCam;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    public void Init()
    {
        mainCam = Camera.main;
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        PositionAndScaleQuad();
    }

    public MeshCollider GetCollider()
    {
        return meshCollider;
    }

    private void PositionAndScaleQuad()
    {
        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth = camHeight * mainCam.aspect;

        float quadCenterY = mainCam.transform.position.y - mainCam.orthographicSize + tankHeightWorldUnits / 2f + bottomMargin;

        transform.position = new Vector3(mainCam.transform.position.x, quadCenterY, 0f);
        transform.localScale = new Vector3(camWidth, tankHeightWorldUnits, 1f);
        // Forzar actualización del MeshCollider
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
}



