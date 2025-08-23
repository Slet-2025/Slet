
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MoveOnEnable : UdonSharpBehaviour
{
    public float activeY = 0f;       // Позиция по Y при активации
    public float inactiveY = -5f;    // Позиция по Y при деактивации
    public float speed = 2f;         // Скорость движения

    private bool isMoving = false;
    private float targetY;

    private void Awake()
    {
        // Стартуем всегда с позиции inactiveY
        Vector3 pos = transform.localPosition;
        pos.y = inactiveY;
        transform.localPosition = pos;
    }

    private void OnEnable()
    {
        targetY = activeY;
        isMoving = true;
    }

    private void OnDisable()
    {
        isMoving = false;
        Vector3 pos = transform.localPosition;
        pos.y = inactiveY;
        transform.localPosition = pos;
    }

    private void Update()
    {
        if (!isMoving) return;

        Vector3 pos = transform.localPosition;
        pos.y = Mathf.MoveTowards(pos.y, targetY, speed * Time.deltaTime);
        transform.localPosition = pos;

        if (Mathf.Abs(pos.y - targetY) < 0.01f)
        {
            pos.y = targetY;
            transform.localPosition = pos;
            isMoving = false;
        }
    }
}
