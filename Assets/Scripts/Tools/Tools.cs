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
}
