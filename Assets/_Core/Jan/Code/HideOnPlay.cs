using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
