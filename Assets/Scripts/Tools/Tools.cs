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

        
        public static string Username(int maxLength) {
            string username;

            do {
                bool useSecondAdj = UnityEngine.Random.Range(0, 6) == 0; // 1 in 5
                bool useNum = UnityEngine.Random.Range(0, 2) == 0; // 1 in 2
                bool useXX = UnityEngine.Random.Range(0, 21) == 0; // 1 in 20

                string adj2 = useSecondAdj? Item(username2ndAdjs) : "";
                string adj = Item(usernameAdjs);
                string noun = Item(usernameNouns);
                string num = useNum? UnityEngine.Random.Range(2, 100).ToString() : "";
                string xxStart = useXX? "XX_" : "";
                string xxEnd = useXX? "_XX" : "";

                username = $"{xxStart}{adj2}{adj}{noun}{num}{xxEnd}";
            } while (username.Length > maxLength);

            return username;
        }

        public static string Item(string[] array) {
            return array[UnityEngine.Random.Range(0, array.Length)];
		}
    }

    public static class English {
        public static string Th(int index) {
            string indexString = (index + 1).ToString(); // Start at 1
            char last = indexString[indexString.Length - 1];

            string suffix;
            if (last == '1') {
                suffix = "st";
			}
            else if (last == '2') {
                suffix = "nd";
			}
            else if (last == '3') {
                suffix = "rd";
			}
            else {
                suffix = "th";
			}

            if (indexString.Length != 1) {
                if (indexString[indexString.Length - 2] == '1') { // Numbers ending in 10-20 are all ths
                    suffix = "th";
				}
			}

            return indexString + suffix;
		}
	}
}
