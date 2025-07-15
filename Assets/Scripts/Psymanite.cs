using DG.Tweening;
using UnityEngine;

public class Psymanite : MonoBehaviour
{

    //[SerializeField, Range(0, 1)] private float lookedAtSpeed = .5f;
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Color lineStartColor;
    
    private static readonly string LookingPercentage = "_LookingPercentage";
    private Material m_material;
    private float m_lookingPercentage;
    private bool m_finishedLooking;

    private Sequence m_lineAppearing;
    
    
    private void Awake()
    {
        m_material = GetComponent<MeshRenderer>().material;
        line.gameObject.SetActive(false);

        m_lineAppearing = DOTween.Sequence()
            .Append(line.DOColor(new Color2(Color.clear, Color.clear), new Color2(lineStartColor, lineStartColor), transitionTime));
    }

    private void OnDestroy()
    {
        Destroy(m_material);
    }

    public bool OnLookedAt(bool shouldFinishLooking)
    {
        if(m_finishedLooking)
            return false;

        m_finishedLooking = shouldFinishLooking;

        if (m_finishedLooking)
        {
            m_material.DOFloat(1, LookingPercentage, transitionTime).Play();
            line.gameObject.SetActive(true);
            m_lineAppearing.Play();
            return true;
        }

        return false;

        // m_lookingPercentage += lookedAtSpeed * Time.deltaTime;
        // m_material.SetFloat(LookingPercentage, m_lookingPercentage);

        //m_finishedLooking = m_lookingPercentage >= 1;
    }
}
