using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [field: SerializeField] public int Team { get; private set; } = 1;  // Quin's preference - enemies on team 1, player/allies on team 0
    [field: SerializeField] public bool IsTargetable { get; set; } = true;
}