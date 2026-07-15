using UnityEngine;

public class RotateOnSpawn : MonoBehaviour
{
    [Header("Rotación al aparecer")]
    public Vector3 rotation = new Vector3(0, 90, 0);

    void Awake()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}