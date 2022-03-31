using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation {
    public static bool inGame;
    public static int time { get; private set; } = 27000;
    public static int day;

    public static void Spend(int amount) {
        currentSave.spent += amount;
	}

    public class dailyLimitProgress {
        public static int DNG;
    }

    public static void IncreaseTime(int amount) {
        int dayLength = 72000;

        time += amount;
        int dayIncrease = Mathf.FloorToInt(time / dayLength);
        if (dayIncrease != 0) {
            EndDay();

            day += dayIncrease;
            time %= dayLength;
		}

    }
    private static void EndDay() {
        dailyLimitProgress.DNG = 0;
	}

    public static List<Save> saves = new List<Save>();
    public static Save currentSave = NewSave();
    public static Save NewSave() {
        Save save = new Save();
        saves.Add(save);
        return save;
	}
}
