using Firebase.Analytics;
using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager Instance;

    bool isFireLogin = false;

    private void Awake()
    {
        Instance = this;

        Add_Token();

    }

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();

                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app..
            }
            else
            {
            }
        });

       // Get_Editor_Gift();

    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        LogEvent("Start");
        FirebaseNullLogin();
    }

    public void FirebaseNullLogin()
    {

        if (!isFireLogin)
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    // User is now signed in.

                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    isFireLogin = true;


                }
                else
                {
                    Debug.Log("failed");
                }
            });
        }


    }

    public void LogEvent(string MainTitle)
    {

        // Log an event with no parameters.
        Firebase.Analytics.FirebaseAnalytics.LogEvent(MainTitle);


    }

    public void LogEvent(string MainTitle,string SubTitle ,int val)
    {

        // Log an event with no parameters.
        Firebase.Analytics.FirebaseAnalytics.LogEvent(MainTitle, SubTitle, val);


    }
    public void Add_Token()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        string gift_num = "";
        string value_num = "";

        int gift = -1;
        int value = -1;

        if (e.Message.Data.TryGetValue("Gift", out gift_num))
        {
            gift = int.Parse(gift_num);
        }

        if (e.Message.Data.TryGetValue("value", out value_num))
        {
            value = int.Parse(value_num);
        }

        if (gift != -1 && value != -1)
        {

            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/reward") as GameObject);
            obj.GetComponent<RewardPopup>().set_gift(gift, value);
            DialogManager.GetInstance().show(obj);
        }
    }

    public void Get_Editor_Gift()
    {

#if UNITY_EDITOR

        int gift = 0;
        int value = 50;

        if (gift != -1 && value != -1)
        {


            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/reward") as GameObject);
            obj.GetComponent<RewardPopup>().set_gift(gift,value);
            DialogManager.GetInstance().show(obj);

        }
#endif

    }

}
