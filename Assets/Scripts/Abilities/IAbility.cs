﻿using UnityEngine;
using System.Collections;

public interface IAbility
{
    void UseAbility();

    float Cooldown();
	
}
