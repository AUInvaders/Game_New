using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class ResetPassword : MonoBehaviour
{
    public GameObject token;
    public GameObject newpassword;
    public GameObject ForgotPasswordMenuUI;
    public GameObject ResetPasswordMenuUI;
    public GameObject LoginMenuUI;
    public GameObject Token_display;
    public GameObject NewPassword_display;

    private string Token;
    private string NewPassword;
    private bool _dataRecieved = false;
    private RecievedPasswordReset _rpr;


    public ResetPassword()
    {
        WebSocketClient.OnMessageEvent += this.OnMessage;
    }
    
    private void OnMessage(object sender, MessageEventArgs e)
    {
        print(e.Data);
        
        _rpr = new RecievedPasswordReset();
        _rpr = JsonUtility.FromJson<RecievedPasswordReset>(e.Data);
        _dataRecieved = true;
    }
    
    public bool ValidateToken(string Token)
    {
        if (Token == "")
        {
            Token_display.GetComponent<Text>().text = "Field is empty";
            Token_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        Token_display.GetComponent<Text>().text = "";
        return true;
    }

    public bool ValidateNewPassword(string NewPassword)
    {
        if (NewPassword == "")
        {
            NewPassword_display.GetComponent<Text>().text = "Field is empty";
            NewPassword_display.GetComponent<Text>().color = Color.red;
            return false;
        }

        NewPassword_display.GetComponent<Text>().text = "";
        return true;
    }

    public bool Recieved()
    {
        bool succes = false;

        do { } while (!_dataRecieved);

        switch (_rpr.Message)
        {
            case "OK":
            {
                print("Password reset");
                succes = true;
                break;
            }

            default:
            {
                print(_rpr.Message);
                newpassword.GetComponent<Text>().text = "Unknown error!";
                newpassword.GetComponent<Text>().color = Color.red;
                _dataRecieved = false;
                break;
            }
        }
        return succes;
    }

    public void SendNewPasswordButton()
    {
        bool tokenValidate = false;
        bool newpasswordValidate = false;
        var s = false;

        tokenValidate = ValidateToken(Token);
        newpasswordValidate = ValidateNewPassword(NewPassword);

        if (tokenValidate && newpasswordValidate)
        {
            ResetPasswordCommand _resetCommand = new ResetPasswordCommand(Token, NewPassword);
            WebSocketClient.Start();
            WebSocketClient.Send(JsonUtility.ToJson(_resetCommand));

            s = Recieved();
        }

        if(s)
        {
            ResetPasswordMenuUI.SetActive(false);
            LoginMenuUI.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Token != "" && NewPassword != "")
            {
                SendNewPasswordButton();
            }
        }
        Token = token.GetComponent<InputField>().text;
        NewPassword = newpassword.GetComponent<InputField>().text;
    }

    [Serializable]
    public class ResetPasswordCommand
    {
        public ResetPasswordCommand(string _t, string _p)
        {
            this.Command = "Reset password";
            this.Token = _t;
            this.Password = _p;
        }

        public string Command;
        public string Token;
        public string Password;
    }
}

[Serializable]
public class RecievedPasswordReset
{
    public string Token;
    public string Message;
    public string Code;
}
