using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simulation {
    public const string phoneScene = "PhoneMenu";
    public static CutsceneTextController textBox;


    public static Difficulty difficulty;
    public class Difficulty {
        public int gameTimeLimit;

        public Difficulty(int level) {
            if (level == 0) {

			}
            else if (level == 1) {
                gameTimeLimit = (5 * 60) * 50;
			}
		}
    }

    public static bool inGame;
    public static string gameName;

    public static bool preventClose;

    public static void Spend(int amount) {
        currentSave.spent += amount;
	}

    public static void IncreaseTime(int amount) {
        currentSave.timeLeft -= amount;
        if (currentSave.timeLeft < 0) {
            TimesUp();
        }
    }

    private static void TimesUp() {
        currentSave.timeLeft = difficulty.gameTimeLimit;
        currentSave.gamesUnlocked++;

        SceneManager.LoadScene(phoneScene);
    }

    public static void StartPlaying() {
        SceneManager.LoadScene(phoneScene);
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

            currentSave.DNGSave.plays = 1;
            currentSave.NFTMatchSave.plays = 1;

            currentSave.gamesUnlocked = 2;
		}
	}

    public static List<Save> saves = new List<Save>();
    public static Save currentSave;
    public static Save NewSave() {
        Save save = new Save();
        saves.Add(save);
        difficulty = new Difficulty(save.difficultyLevel);

        return save;
	}
}
