﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    [Header("Body Parts")]
    public BodyPart head;
    public BodyPart body;
    public BodyPart LeftArm;
    public BodyPart RightArm;
    public BodyPart LeftLeg;
    public BodyPart RightLeg;

    // for debugging
    public int forTest = 0;

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    // 시간이 흘렀을 때 캐릭터의 변화에 대한 함수이다
    public void DecayBody(int time)
    {

    }
}
