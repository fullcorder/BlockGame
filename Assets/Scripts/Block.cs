using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int LineNumber { get; set; }

    public Action<Block> OnDestroy;


    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ball"))
        {
            OnDestroy(this);
            Destroy(gameObject);
        }
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(LineNumber)}: {LineNumber}";
    }
}