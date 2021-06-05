using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    [SerializeField] private Color noDashColor;

    private Dash Dash { get; set; }
    private SpriteRenderer Renderer { get; set; }
    private Color BaseColor { get; set; }

    private void Start()
    {
        Dash = GetComponent<Dash>();
        Renderer = GetComponent<SpriteRenderer>();
        BaseColor = Renderer.color;
    }

    private void Update()
    {
        Renderer.color = Dash.CanDash ? BaseColor : noDashColor;
    }
}
