using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPartType
{
    Head,
    Body,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg,
    None = -1
}

[CreateAssetMenu]
public class BodyPart : Tool
{
    [Header("Body Part Member")]
    public BodyPartType bodyPartType;
    public Sprite bodyPartSprite;
}
