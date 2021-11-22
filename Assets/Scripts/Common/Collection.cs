using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Collection {
    public static T RandomEnum<T>()
    {
        Array values = Enum.GetValues(typeof(T)); 
        return (T) values.GetValue(new Random().Next(0, values.Length)); 
    }
    
}
