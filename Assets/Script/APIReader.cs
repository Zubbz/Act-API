using Models;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class APIReader : MonoBehaviour
{
    private readonly string basePath = "https://retoolapi.dev/BTTfpi/data";
    public UserData[] users;

    public UserData userData;
    [Header("Sign Up")]
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField conPassword;
    public TMP_InputField classChosen;
    [Header("Log-In")]
    public TMP_InputField logInInp;
    public TMP_InputField logInPass;
    [Header("Forget Password")]
    public TMP_InputField userID;
    public TMP_InputField userUsername;
    public TMP_InputField newPass;
    public TMP_InputField newConfirmPass;

    public void Start()
    {
        Get();

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Post();
        }
        

    }

    public void Get()
    { 
        RestClient.Get(basePath).Then(response =>
        {
            try
            {
                string jsonResponse = response.Text;
                users = JsonHelper.ArrayFromJson<UserData>(jsonResponse);   

                if(users!= null) {
                    Debug.Log("Number of users : " + users.Length);
                }
                
            }
            catch (Exception ex)
            {
                Debug.Log(ex + "User array is null");
            }

        }).Catch(error => { 
            Debug.Log(error.Message);
        }

        );
    }

    public void Post()
    {
        userData.Username = username.text;
        userData.Password = password.text;
        userData.confirmPassword = conPassword.text;
        userData.classChosen = classChosen.text;
        if (conPassword.text == password.text)
        {
            Debug.Log("passowrd matches");
            RestClient.Post(basePath, userData).Then(response =>
            {
                try
                {
                    if (response != null)
                    {
                        Debug.Log("Successful");

                    }
                    else
                    {
                        Debug.Log("No Response");
                    }

                }
                catch (Exception ex)
                {
                    Debug.Log(ex + "Error posting UserData");
                }

            }).Catch(error => {
                Debug.Log(error.Message);
            }

);
        }
        else
        {
            Debug.Log("Password don't match!");
        }

    }

    public void DeleteUser(int userId)
    {
        RestClient.Delete(basePath + "/"+ userId).Then(response =>
        {
            try
            {
                if (response != null)
                {
                    Debug.Log("Deleted Successfully");
                }
                else
                {
                    Debug.Log("No Response");
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex + "Error posting UserData");
            }

        }).Catch(error => {
            Debug.Log(error.Message);
        }

           );
    }

    public void PatchUser()
    {
        RestClient.Patch(basePath + "/" + userData.id, userData).Then(response =>
        {
            try
            {
               if (response != null)
               {
                   Debug.Log("Patched Successfully");
               }
               else
               {
                   Debug.Log("No Response");
               }

            }
            catch (Exception ex)
            {
               Debug.Log(ex + "Error Patching UserData");
            }

            }).Catch(error =>
            {
                Debug.Log(error.Message);
            }

       );
    }
    public void LogInChecker()
    {
        if (InputChecker())
        {
            Debug.Log("Log in success");
        }
        else
        {
            Debug.Log("Log in unsuccessful");
        }
    }
    public bool InputChecker()
    {
        foreach (UserData user in users)
        {
            if (user.Username == logInInp.text)
            {
                if (user.Password == logInPass.text) 
                {
                    return true;
                }
                return false;
            }
        }
        return false;
        
    }
    public void UpdatePassword()
    {
        IDChecker();
        if (userData != null)
        {
            Debug.Log($"{userData.id} this is user ID");


            userData.Password = newPass.text;
            userData.confirmPassword = newConfirmPass.text;

            if (newPass.text == newConfirmPass.text)
            {
                RestClient.Patch(basePath + "/" + userData.id, userData).Then(response =>
                {
                    Debug.Log(userData.id);
                    try
                    {
                        if (response != null)
                        {
                            Debug.Log("Patched Successfully");
                        }
                        else
                        {
                            Debug.Log("No Response");
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex + "Error Patching UserData");
                    }

                }).Catch(error =>
                {
                    Debug.Log(error.Message);
                }

           );
            }
            else
            {
                Debug.Log("Passwords don't match, unable to change password!");
            }
            Debug.Log("Successfully changed password!");
        }
        else
        {
            Debug.Log("Password change failed!");
        }
    }

    public void IDChecker() 
    { 
        foreach (UserData user in users)
        {
            if (user.id.ToString() == userID.text)
            {
                Debug.Log("user ID found! Changing password!");
                userData = user;
            }
        }
        Debug.Log("user ID found! unable to change password.");
        
    }
}
