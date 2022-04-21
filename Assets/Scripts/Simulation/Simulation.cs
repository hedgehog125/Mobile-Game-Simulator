using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simulation {
    private static string introScene = "Intro1";
    private static string resumeScene = "PhoneMenu";

    public static bool inGame;
    public static int time { get; private set; } = 27000;
    public static int day;

    public static bool preventClose;

    public static void Spend(int amount) {
        currentSave.spent += amount;
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
        currentSave.DNGSave.dailyLimitProgress = 0;
	}

    public static void StartPlaying() {
        if (currentSave.watched.intro) {
            SceneManager.LoadScene(resumeScene);
		}
        else {
            SceneManager.LoadScene(introScene);
        }
    }

    
    public static void Init(bool initSave) {
        if (initSave) InitSave();
    }
    public static void Init() {
        Init(true);
    }

    private static void InitSave() {
        if (currentSave == null) {
            currentSave = NewSave();
            Debug.LogWarning("Had to generate save, this should only happen if you're running from a different scene than the main menu.");

            // Set some values for easier development
            currentSave.watched.intro = true;
            currentSave.NFTMatchSave.plays = 1;
		}
	}

    public static List<Save> saves = new List<Save>();
    public static Save currentSave;
    public static Save NewSave() {
        Save save = new Save();
        saves.Add(save);
        return save;
	}
}
