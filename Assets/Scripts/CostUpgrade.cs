using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CostUpgrade
{
    public static int[] keyLvlCost { get; private set; } = { 100, 300, 600, 900, 1400, 2000, 2500, 3500 };
    public static int[] keySpeedCost { get; private set; } = { 100, 300, 600, 900, 1400 };
    public static int[] keyReloadCost { get; private set; } = { 100, 300, 600, 900, 1400 };
    public static int[] roundDurationCost { get; private set; } = { 100, 300, 600, 900, 1400 };
}
