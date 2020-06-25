using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class JsonHelper
{

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.item;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.item = array;

        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.item = array;

        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [Serializable]
    public class Wrapper<T>
    {
        public T[] item;
    }
}
