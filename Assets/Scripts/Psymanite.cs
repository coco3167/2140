using DG.Tweening;
using UnityEngine;

public class Psymanite : MonoBehaviour
{

    //[SerializeField, Range(0, 1)] private float lookedAtSpeed = .5f;
    [SerializeField] private float transitionTime = 2f;
    private static readonly string LookingPercentage = "_LookingPercentage";
    private Material m_material;
    private float m_lookingPercentage;

    private bool m_finishedLooking;
    
    
    private void Awake()
    {
        m_material = GetComponent<MeshRenderer>().material;
        Debug.Log(m_material.GetFloat(LookingPercentage));
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
            return true;
        }

        return false;

        // m_lookingPercentage += lookedAtSpeed * Time.deltaTime;
        // m_material.SetFloat(LookingPercentage, m_lookingPercentage);

        //m_finishedLooking = m_lookingPercentage >= 1;
    }
}
