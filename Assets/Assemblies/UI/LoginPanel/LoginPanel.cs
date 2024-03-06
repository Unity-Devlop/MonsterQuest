using System;
using Proto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class LoginPanel : UIPanel
    {
        private TMP_InputField userIdInput;
        private TMP_InputField userPasswordInput;
        private Button loginButton;
        private Button registerButton;
        private RegisterPanel _registerPanel;

        private string userId => userIdInput.text;
        private string userPassword => userPasswordInput.text;

        private void Awake()
        {
            userIdInput = transform.Find("UserIdInput").GetComponent<TMP_InputField>();
            userPasswordInput = transform.Find("UserPasswordInput").GetComponent<TMP_InputField>();
            loginButton = transform.Find("LoginButton").GetComponent<Button>();
            registerButton = transform.Find("RegisterButton").GetComponent<Button>();
            _registerPanel = transform.Find("RegisterPanel").GetComponent<RegisterPanel>();
            
            
            loginButton.onClick.AddListener(OnLogin);
            registerButton.onClick.AddListener(OnRegister);
        }

        private void OnRegister()
        {
            
        }

        private void OnLogin()
        {
            // GrpcClient.GameService.LoginRequest(new UserLoginRequest()
        }
    }
}