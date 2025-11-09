using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;

    private void Update()
    {
        transform.Translate(-1 * moveSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x < -26.7f)
        {
            transform.position = new Vector2(0, 0);
        }
    }
}
