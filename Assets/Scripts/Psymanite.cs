using System;
using UnityEngine;

public class Psymanite : MonoBehaviour
{
    private event EventHandler LookedAt;

    private void Awake()
    {
        LookedAt += OnLookedAt;
    }

    private void OnLookedAt(object sender, EventArgs e)
    {
        Debug.Log("looked at");
    }

    public void LookAt(object sender, EventArgs e)
    {
        LookedAt?.Invoke(this, EventArgs.Empty);
    }
}
