using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string cal;
    public int val;
}

[System.Serializable]
public class Questions
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public Question[] questions;
}