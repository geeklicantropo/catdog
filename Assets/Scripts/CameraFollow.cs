using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float xMax = 10f;
    [SerializeField] private float yMax = 10f;
    [SerializeField] private float xMin = -10f;
    [SerializeField] private float yMin = -10f;

    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, xMin, xMax),
            Mathf.Clamp(target.position.y, yMin, yMax),
            transform.position.z);
    }
}
