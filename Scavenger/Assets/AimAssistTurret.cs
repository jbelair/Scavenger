using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Guides/Aiming/Turret")]
public class AimAssistTurret : MonoBehaviour
{
    public Statistics player;
    public string statisticAiming = "Aiming";
    public string statisticAimingSpeed = "Aiming Speed";
    public string statisticColour = "Guide Colour";
    public string statisticRadiusStart = "Projectile Radius Start";
    public string statisticRadiusEnd = "Projectile Radius End";
    public string statisticRange = "Projectile Range";

    public LineRenderer aimingLine;
    public LineRenderer widthGlow;
    public LineRenderer widthLeftAssist;
    public LineRenderer widthRightAssist;
    public LineRenderer radius;

    private Statistic aiming,
        aimingSpeed,
        guideColour,
        radiusStart,
        radiusEnd,
        range;

	// Use this for initialization
	void Start ()
    {
        aiming = player[statisticAiming];
        aimingSpeed = player[statisticAimingSpeed];
        guideColour = player[statisticColour];
        radiusStart = player[statisticRadiusStart];
        radiusEnd = player[statisticRadiusEnd];
        range = player[statisticRange];
    }

    public int updates = 2;
    private int update;
	// Update is called once per frame
	void FixedUpdate ()
    {
        update++;

        if (update == updates)
            update -= updates;
        else
            return;

        Vector3 rangeV = new Vector3(0, range.Get<float>());
        aimingLine.startColor = widthGlow.startColor = widthLeftAssist.startColor = widthRightAssist.startColor = radius.startColor = radius.endColor = guideColour;
        aimingLine.SetPosition(1, rangeV);
        widthGlow.SetPosition(1, rangeV);
        widthGlow.startWidth = widthGlow.endWidth = radiusStart.Get<float>() * 2;
        widthGlow.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 1) });

        widthLeftAssist.SetPosition(0, new Vector3(radiusStart.Get<float>(), 0));
        widthRightAssist.SetPosition(0, new Vector3(-radiusStart.Get<float>(), 0));
        widthLeftAssist.SetPosition(1, new Vector3(radiusEnd.Get<float>(), range.Get<float>()));
        widthRightAssist.SetPosition(1, new Vector3(-radiusEnd.Get<float>(), range.Get<float>()));
        radius.transform.localScale = Vector3.one * radiusStart.Get<float>();

        if (aiming.type == Statistic.ValueType.Vector2)
        {
            Vector3 aim = aiming.Get<Vector2>();
            aim = new Vector3(-aim.x, aim.y);
            transform.up = Vector3.RotateTowards(transform.up, aim, aimingSpeed.Get<float>() * Time.deltaTime, aimingSpeed.Get<float>() * Time.deltaTime);
        }
        else if (aiming.type == Statistic.ValueType.Vector3)
        {
            Vector3 aim = aiming.Get<Vector3>() - player.transform.position;
            transform.up = Vector3.RotateTowards(transform.up, aim, aimingSpeed.Get<float>() * Time.deltaTime, aimingSpeed.Get<float>() * Time.deltaTime);
        }
    }
}
