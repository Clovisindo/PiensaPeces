using UnityEngine;


[RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
public class FishTankMeshScaler : MonoBehaviour
{
    [SerializeField] private float tankHeightWorldUnits = 2f;
    [SerializeField] private float taskbarHeightPixels = 40f; // Ajusta según necesites
    [SerializeField] private BoxCollider2D colliderWindow; // Asigna este en el inspector

    private Camera mainCam;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    public void Init()
    {
        mainCam = Camera.main;
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        PositionAndScaleQuad();
        AdjustCollider2D();
    }

    public MeshCollider GetCollider()
    {
        return meshCollider;
    }

    private void PositionAndScaleQuad()
    {
        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth = camHeight * mainCam.aspect;

        // Obtenemos altura de pantalla visible, restando la barra de tareas
        float bottomYInPixels = taskbarHeightPixels;

        // Convertimos ese punto a coordenadas del mundo
        Vector3 worldBottom = mainCam.ScreenToWorldPoint(new Vector3(0, bottomYInPixels, Mathf.Abs(mainCam.transform.position.z)));

        float quadCenterY = worldBottom.y + tankHeightWorldUnits / 2f;

        transform.position = new Vector3(mainCam.transform.position.x, quadCenterY, 0f);
        transform.localScale = new Vector3(camWidth, tankHeightWorldUnits, 1f);

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
}

    private void AdjustCollider2D()
    {
        if (colliderWindow == null)
        {
            Debug.LogWarning("No BoxCollider2D assigned to FishTankMeshScaler.");
            return;
        }
        // El collider es hijo, así que solo ajustamos tamaño y offset, *NO la posición*
        colliderWindow.offset = Vector2.zero;
        colliderWindow.size = new Vector2(1f, 1f); // LocalScale del padre * este size = tamaño final
    }
}
