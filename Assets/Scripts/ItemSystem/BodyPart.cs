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

public enum Grade
{
    S,
    A,
    B,
    C,
    D
}

[CreateAssetMenu]
public class BodyPart : Tool
{
    [Header("Body Part Member")]
    public FloatVariable durability;
    public BodyPartType bodyPartType;
    public Grade grade;
    public Sprite bodyPartSprite;
}
