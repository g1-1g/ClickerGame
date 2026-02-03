using System.Collections;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class Dog 
{
    public Dog() { }

    [FirestoreProperty]
    public string Name { get; set; }
    [FirestoreProperty]
    public int Age { get; set; }

    public Dog(string name, int age)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentException("이름은 비어있을 수 없습니다");
        }

        if (age <= 0)
        {
            throw new System.ArgumentException("나이는 0보다 커야 합니다");
        }

        Name = name;
        Age = age;
    }
}