using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCircle : LineRendererCircle
{
    public Color lineColour;
    [Tooltip("(Optional) If defined the fill will have its width and height set to diameter.")]
    public RectTransform fill;
    [Tooltip("(Optional) If defined the fill renderer will have its colour set to the defined fill colour.")]
    public SpriteRenderer fillRender;
    public Color fillColour;
    [Tooltip("(Optional) If defined the label will be position to the right of the circle.")]
    public Transform label;
    [Tooltip("(Optional) If defined the text renderer will have its colour set to the defined text colour.")]
    public TextMesh labelRender;
    public Color labelColour;
    [Tooltip("Defines the offset from the right edge of the circle to place the label at.")]
    public Vector3 labelOffset = new Vector3(-20, 0, 0);

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    [ExposeMethodInEditor]
    public void Adjust()
    {
        SetCircle();

        line.startColor = line.endColor = lineColour;

        if (fill)
            fill.localScale = new Vector3(radius * 2, radius * 2, 0);

        if (fillRender)
            fillRender.color = fillColour;

        if (label)
            label.localPosition = new Vector3(-radius, 0, 0) + labelOffset;

        if (labelRender)
            labelRender.color = labelColour;
    }
}
