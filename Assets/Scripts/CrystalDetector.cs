using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CrystalDetector : MonoBehaviour
{
    [SerializeField] private LayerMask psymaniteLayerMask;
    
    [Header("Cursor")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Color baseColor, lookingColor;
    [SerializeField] private float transitionTime;
    
    private Ray m_detectingRay;
    private RaycastHit[] m_hitsBuffer = new RaycastHit[5];

    private Sequence m_lookedAtSequence;

    private Camera m_camera;

    private static readonly Vector3 CameraCenter = new(.5f, .5f, 0);

    private void Awake()
    {
        m_camera = GetComponent<Camera>();

        m_lookedAtSequence = DOTween.Sequence();
        m_lookedAtSequence.Append(cursorImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 45), transitionTime));
        m_lookedAtSequence.Join(cursorImage.DOColor(lookingColor, transitionTime));
        m_lookedAtSequence.SetAutoKill(false);
    }

    private void FixedUpdate()
    {
        bool hasTouchedPsymanite = false;
        
        m_detectingRay = m_camera.ViewportPointToRay(CameraCenter);

        int hits = Physics.RaycastNonAlloc(m_detectingRay, m_hitsBuffer, Mathf.Infinity, psymaniteLayerMask,
            QueryTriggerInteraction.Collide);

        for (int loop = 0; loop < hits; loop++)
        {
            if (m_hitsBuffer[loop].transform.TryGetComponent(out Psymanite psymanite))
            {
                hasTouchedPsymanite = true;
                psymanite.OnLookedAt();
            }
        }

        if (hasTouchedPsymanite)
        {
            m_lookedAtSequence.PlayForward();
        }
        else
        {
            m_lookedAtSequence.PlayBackwards();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_detectingRay);
    }
}
