using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void Move(float size)
    {
        var localPosition = _transform.localPosition;

        var x = localPosition.x + size;
        x = Mathf.Clamp(x, -2.5f, 2.5f);

        localPosition.x = x;
        _transform.localPosition = localPosition;
    }
}