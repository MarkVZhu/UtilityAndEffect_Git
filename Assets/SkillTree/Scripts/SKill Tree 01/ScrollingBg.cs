using UnityEngine;

public class ScrollingBg : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float bound;
   
    private void Update()
    {
        transform.Translate(-1 * scrollSpeed * Time.deltaTime, 0, 0);

        if(transform.position.x < bound)
        {
            transform.position = new Vector2(0,0);
        }
    }
}
