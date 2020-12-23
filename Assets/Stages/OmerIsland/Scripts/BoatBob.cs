using UnityEngine;

public class BoatBob : MonoBehaviour
{
    private float elapsedTime;
    private Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float y = Mathf.Sin(2 * elapsedTime + 0.7f);
        transform.position = new Vector3(pos.x, pos.y + 0.2f * y, pos.z);
    }    
}