using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CrystalDetector : MonoBehaviour
{
    [SerializeField] private LayerMask psymaniteLayerMask;
    
    [Header("Cursor")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Color baseColor, lookingColor;
    
    private Ray m_detectingRay;
    private RaycastHit[] m_hitsBuffer = new RaycastHit[5];

    private Camera m_camera;

    private static readonly Vector3 CameraCenter = new(.5f, .5f, 0);

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        m_detectingRay = m_camera.ViewportPointToRay(CameraCenter);

        int hits = Physics.RaycastNonAlloc(m_detectingRay, m_hitsBuffer, Mathf.Infinity, psymaniteLayerMask,
            QueryTriggerInteraction.Collide);

        for (int loop = 0; loop < hits; loop++)
        {
            if(m_hitsBuffer[loop].transform.TryGetComponent(out Psymanite psymanite))
                psymanite.OnLookedAt();
        }

        cursorImage.color = hits > 0 ? lookingColor : baseColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_detectingRay);
    }
}
