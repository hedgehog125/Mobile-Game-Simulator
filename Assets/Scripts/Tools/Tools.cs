using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools {
    public static GameObject GetNestedGameobject(List<string> names) {
        Transform currentTransform = GameObject.Find(names[0]).transform;
        for (int i = 1; i < names.Count; i++) {
            currentTransform = currentTransform.Find(names[i]);
            if (currentTransform == null) return null;
        }

        return currentTransform.gameObject;
    }

    public static class Random {
        private static string[] usernameNouns = {
            "notabot",
            "gamer",
            "player",
            "lion",
            "cheetah",
            "dog",
            "cat",
            "human"
        };
        private static string[] usernameAdjs = {
            "epic",
            "awesome",
            "cool",
            "badass",
            "skillful",
            "aggressive",
            "apex",
            "360",
            "noscope",
            "goated"
        };
        private static string[] username2ndAdjs = {
            "super",
            "very",
            "extremely",
            "evenmore"
        };

        public static string Username() {
            bool useSecondAdj = UnityEngine.Random.Range(0, 6) == 0; // 1 in 5
            bool useNum = UnityEngine.Random.Range(0, 2) == 0; // 1 in 2
            bool useXX = UnityEngine.Random.Range(0, 21) == 0; // 1 in 20

            string adj2 = useSecondAdj? Item(username2ndAdjs) : "";
            string adj = Item(usernameAdjs);
            string noun = Item(usernameNouns);
            string num = useNum? UnityEngine.Random.Range(2, 100).ToString() : "";
            string xxStart = useXX? "XX_" : "";
            string xxEnd = useXX ? "_XX" : "";

            return $"{xxStart}{adj2}{adj}{noun}{num}{xxEnd}";
        }

        public static string Item(string[] array) {
            return array[UnityEngine.Random.Range(0, array.Length)];
		}
    }
}
