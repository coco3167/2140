using System;
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
    [SerializeField] private Gradient baseColor;
    [SerializeField] private float minDistance, maxDistance;
    [SerializeField] private float losangeTime, scaleTime;
    [SerializeField] private float maxScale;

    [Header("Time before discovery")]
    [SerializeField] private TextMeshProUGUI timeText;
    
    
    
    private Ray m_detectingRay;
    private RaycastHit[] m_hitsBuffer = new RaycastHit[5];

    private Psymanite[] m_allPsymanite;

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
            //.Join(cursorImage.DOColor(lookingColor, losangeTime))
            .SetAutoKill(false);

        m_lockedSequence = DOTween.Sequence();
        m_lockedSequence
            .Append(cursorImage.rectTransform.DOScale(Vector3.one * maxScale, scaleTime))
            .SetAutoKill(false);
    }

    private void Start()
    {
        m_allPsymanite = FindObjectsByType<Psymanite>(FindObjectsSortMode.None);
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
        cursorImage.color = baseColor.Evaluate(PsymaniteDistance(m_hitsBuffer[0].point));

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

    private float PsymaniteDistance(Vector3 point)
    {
        float minPsymaniteDistance = float.PositiveInfinity;
        foreach (Psymanite psymanite in m_allPsymanite)
        {
            if(psymanite.FinishedLooking)
                continue;
            
            float distance = Vector3.Distance(psymanite.transform.position, point);
            if (distance < minPsymaniteDistance)
            {
                minPsymaniteDistance = distance;
            }
        }

        return Math.Clamp((minPsymaniteDistance - minDistance)/maxDistance, 0, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_detectingRay);
    }
}