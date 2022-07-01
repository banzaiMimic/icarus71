using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;
using TMPro;
using DarkRift.Client.Unity;
using icarus.gg;

// @Recall
/*
  should split in 3 ... 
  -VrMenuManager
  -MouseMenuManager
  -shared data / ui screens etc 
*/
public class VrMenuManager : MonoBehaviour {

  static VrMenuManager _instance;
  public GameObject go;
  public static VrMenuManager INSTANCE {
    get { 
      if (_instance == null) {
        GameObject go = new GameObject("VrMenuManager");
        _instance = go.AddComponent<VrMenuManager>();
        Vector3 position = new Vector3(0,0,0);
        go = Instantiate(Resources.Load("VrMenuManager"), position, Quaternion.identity) as GameObject;
      }
      return _instance;
    }
  }

  private const string USERNAME_PLACEHOLDER = "enter username";
  private SteamVR_LaserPointer leftLaserPointer { get; set; }
  private SteamVR_LaserPointer rightLaserPointer { get; set; }
  private Panel currentPanel { get; set; }
  private TextMeshProUGUI username { get; set; }
  private UnityClient drClient;
  private List<Panel> panelHistory = new List<Panel>();
  private Panel[] panels = new Panel[2];

  public void Init(
    SteamVR_LaserPointer leftLaserPointer,
    SteamVR_LaserPointer rightLaserPointer,
    TextMeshProUGUI username,
    Panel panelMain,
    Panel panelLogin,
    Vector3 position
  ) {

    this.leftLaserPointer = leftLaserPointer;
    this.rightLaserPointer = rightLaserPointer;
    this.username = username;
    panelMain.transform.position = position;
    panelLogin.transform.position = position;
    panels[0] = panelMain;
    panels[1] = panelLogin;

    username.text = USERNAME_PLACEHOLDER;
    leftLaserPointer.PointerClick += PointerClick;
    rightLaserPointer.PointerClick += PointerClick;

    SetupPanels();
  }

  public void PointerClick(object sender, PointerEventArgs e) {
    string clickedName = e.target.name;
    switch (clickedName) {
      case "btn_login":
        SetCurrentWithHistory(panels[1]);
      break;
      case "btn_submit":
        SubmitLogin();
      break;
      case "btn_devTest":
        ConnectToServer();
      break;
      default:
        if (username.text == USERNAME_PLACEHOLDER) {
          username.text = "";
        }
        string key = clickedName.Substring(4, 1);
        Debug.Log($"key = {key}");
        username.text += key;
      break;
    }
  }

  private void SubmitLogin() {
    Debug.Log($"[SubmitLogin] with username {username.text}");
  }

  private void SetupPanels() {
    panels[1].Hide();
    currentPanel = panels[0];
    currentPanel.Show();
  }

  private void ConnectToServer() {
    //@TODO 
    // - host & port input ui
    // - list (hardcode for now) of 'our' core servers
    Dispatcher.INSTANCE.connectToServer("localhost", 4296);
  }

  void Update() {
    // if primary hand trigger is down -> go to previous (might not add this)
  }

  public void GoToPrevious() {
    if (panelHistory.Count == 0) {
      // may need a ui confirm quit ? 
      return;
    }
    int lastIndex = panelHistory.Count - 1;
    SetCurrent(panelHistory[lastIndex]);
    panelHistory.RemoveAt(lastIndex);
  }

  public void SetCurrentWithHistory(Panel panel) {
    panelHistory.Add(currentPanel);
    SetCurrent(panel);
  }

  private void SetCurrent(Panel panel) {
    currentPanel.Hide();
    currentPanel = panel;
    currentPanel.Show();
  }

}
