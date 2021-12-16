using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading.Tasks;
using System.Threading;


public class RegisterMenu : MonoBehaviour
{
    public GameObject email;
    public GameObject username;
    public GameObject password;
    public GameObject confirmpassword;
    public GameObject wrongemail_display;
    public GameObject wronguser_display;
    public GameObject wrongpass_display;
    public GameObject wrongconfpass_display;

    private string Email;
    private string Username;
    private string Password;
    private string ConfPassword;

    private  bool _dataRecieved = false;
    private RecievedData _rd;

    public RegisterMenu()
    {
        WebSocketClient.OnMessageEvent += this.OnMessage;
    }
    private void OnMessage(object sender, MessageEventArgs e)
    {
        _rd = new RecievedData();
        _rd = JsonUtility.FromJson<RecievedData>(e.Data);
        _dataRecieved = true;
    }

    [Serializable]
    public class RecievedData { public string Message; public int Code; }

    public bool ValidateEmail(string Email)
    {
        if (Email == "")
        {
            wrongemail_display.GetComponent<Text>().text = "Field is empty";
            wrongemail_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        wrongemail_display.GetComponent<Text>().text = "";
        return true;
    }

    //Validering af Username
    public bool ValidateUsername(string Username)
    {
        if (Username == "")
        {
            wronguser_display.GetComponent<Text>().text = "Field is empty";
            wronguser_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        wronguser_display.GetComponent<Text>().text = "";
        return true;
    }

    //Validering af Password
    public bool ValidatePassword(string Password)
    {
        if (Password == "")
        {
            wrongpass_display.GetComponent<Text>().text = "Field is empty";
            wrongpass_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        if (Password.Length < 7)
        {
            wrongpass_display.GetComponent<Text>().text = "Atleast 7 Characters long";
            wrongpass_display.GetComponent<Text>().color = Color.red;
            return false;
        }
        return true;
    }

    //Validering af Confirmation Password
    public bool ValidateConfPassword(string Conf)
    {
        if(Conf == ""){
            wrongconfpass_display.GetComponent<Text>().text = "Field is empty";
            wrongconfpass_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        if(Conf != Password){
            wrongconfpass_display.GetComponent<Text>().text = "Passwords Don't match";
            wrongconfpass_display.GetComponent<Text>().color = Color.red;
            return false;
        }
        
        return true;
    }

    public bool awaitReg(){
        bool succes = false;

        do{

        } while(!_dataRecieved);

        switch (_rd.Message)
        {
            case "Username is already in use":
                {
                    print("Registration failed: User already exist");
                    wronguser_display.GetComponent<Text>().text = "User already exists";
                    wronguser_display.GetComponent<Text>().color = Color.red;
                    
                    _dataRecieved = false;
                    break;
                }

            case "OK": //Besked skal opdates, mangler svar fra lasse
                {
                    print("User is created");

                    username.GetComponent<InputField>().text = "";
                    password.GetComponent<InputField>().text = "";
                    confirmpassword.GetComponent<InputField>().text = "";

                    wronguser_display.GetComponent<Text>().text = "";
                    wrongpass_display.GetComponent<Text>().text = "";
                    wrongconfpass_display.GetComponent<Text>().text = "Registration Complete";
                    wrongconfpass_display.GetComponent<Text>().color = Color.green;

                    _dataRecieved = false;
                    succes = true;
                    break;
                }
            case "Email is already in use":
            {
                print("Email is already in use");
                
                wrongemail_display.GetComponent<Text>().text = "Email is already in use";
                wrongemail_display.GetComponent<Text>().color = Color.red;


                _dataRecieved = false;
                break;
            }
            default:
                {
                    print(_rd.Message);
                    wrongconfpass_display.GetComponent<Text>().text = "Unknown error!";
                    wrongconfpass_display.GetComponent<Text>().color = Color.red;
                    _dataRecieved = false;
                    break;
                }
        }

        return succes;
    }
    public void RegisterButton()
    {
        bool emailValidate = false;
        bool unameValidate = false;
        bool paswValidate = false;
        bool confPaswValidate = false;
        var s = false;

        emailValidate = ValidateEmail(Email);
        unameValidate = ValidateUsername(Username);
        paswValidate = ValidatePassword(Password);
        confPaswValidate = ValidateConfPassword(ConfPassword);

        if ((emailValidate && unameValidate && paswValidate && confPaswValidate))
        {
            ServerCommand Sc = new ServerCommand("Register", Email, Username, Password);

            WebSocketClient.Start();
            WebSocketClient.Send(JsonUtility.ToJson(Sc));
            
            s = awaitReg();
        }

        if (s)
        {
            SceneManager.LoadScene(1);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                confirmpassword.GetComponent<InputField>().Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(Email!="" && Password != "" && Username != "" && ConfPassword != "")
            {
                RegisterButton();
            }
        }
        Email = email.GetComponent<InputField>().text;
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confirmpassword.GetComponent<InputField>().text;
    }

    public void GoBackToLogin()
    {
        SceneManager.LoadScene(1);
    }
}

[Serializable]
public class ServerCommand : MonoBehaviour
{
    public string Command;
    public string Email;
    public string Username;
    public string Password;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public ServerCommand(string command,string email, string user, string pass)
    {
        Command = command;
        Email = email;
        Username = user;
        Password = pass;
    }
}

[Serializable]
public class ServerCommandLogIn : MonoBehaviour
{
    public string Command;
    public string UsernameOrEmail;
    public string Password;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public ServerCommandLogIn(string command, string user, string pass)
    {
        Command = command;
        UsernameOrEmail = user;
        Password = pass;
    }
}