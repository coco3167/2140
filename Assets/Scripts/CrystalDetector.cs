using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CrystalDetector : MonoBehaviour
{
    [SerializeField] private LayerMask psymaniteLayerMask;
    
    [Header("Cursor")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Color baseColor, lookingColor;
    [SerializeField] private float losangeTime, scaleTime;
    [SerializeField] private float maxScale;

    [Header("Time before discovery")]
    [SerializeField] private TextMeshProUGUI timeText;
    
    
    
    private Ray m_detectingRay;
    private RaycastHit[] m_hitsBuffer = new RaycastHit[5];

    private Sequence m_lookedAtSequence, m_lockedSequence;
    
    private Camera m_camera;

    private static readonly Vector3 CameraCenter = new(.5f, .5f, 0);

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        timeText.text = "";

        m_lookedAtSequence = DOTween.Sequence();
        m_lookedAtSequence
            .Append(cursorImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 45), losangeTime))
            .Join(cursorImage.DOColor(lookingColor, losangeTime))
            .SetAutoKill(false);

        m_lockedSequence = DOTween.Sequence();
        m_lockedSequence
            .Append(cursorImage.rectTransform.DOScale(Vector3.one * maxScale, scaleTime))
            .SetAutoKill(false);
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
                
                if (psymanite.OnLookedAt(m_lockedSequence.IsComplete()))
                {
                    timeText.text = $"Filon trouvé en moins de {Mathf.Round(Time.realtimeSinceStartup)}s";
                }
            }
        }

        if (hasTouchedPsymanite)
        {
            m_lookedAtSequence.PlayForward();

            m_lockedSequence.timeScale = 1;
            m_lockedSequence.PlayForward();
        }
        else
        {
            m_lookedAtSequence.PlayBackwards();
            
            m_lockedSequence.timeScale = 5;
            m_lockedSequence.PlayBackwards();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_detectingRay);
    }
}
