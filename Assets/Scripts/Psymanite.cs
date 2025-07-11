using UnityEngine;

public class Psymanite : MonoBehaviour
{

    [SerializeField, Range(0, 1)] private float lookedAtSpeed = .5f;
    private static readonly int LookingPercentage = Shader.PropertyToID("_LookingPercentage");
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

    public void OnLookedAt()
    {
        if(m_finishedLooking)
            return;
        
        m_lookingPercentage += lookedAtSpeed * Time.deltaTime;
        m_material.SetFloat(LookingPercentage, m_lookingPercentage);

        m_finishedLooking = m_lookingPercentage >= 1;
    }
}
