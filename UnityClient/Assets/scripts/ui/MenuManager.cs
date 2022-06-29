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
public class MenuManager : MonoBehaviour {

  private const string USERNAME_PLACEHOLDER = "enter username";
  [SerializeField]
  private SteamVR_LaserPointer leftLaserPointer;
  [SerializeField]
  private SteamVR_LaserPointer rightLaserPointer;
  [SerializeField]
  private Panel currentPanel = null;
  [SerializeField]
  private TextMeshProUGUI username;
  [SerializeField]
  private UnityClient drClient;

  private List<Panel> panelHistory = new List<Panel>();
  private Panel[] panels = new Panel[2];

  //@Recall need to instantiate or call this after NetworkManager.INSTANCE's Awake call.... isParrelClone has not been updated yet
  private void Awake() {
    if (!NetworkManager.INSTANCE.isParrelClone) {
      username.text = USERNAME_PLACEHOLDER;
      leftLaserPointer.PointerClick += PointerClick;
      rightLaserPointer.PointerClick += PointerClick;
    }
  }

  private void Start() {
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
    panels = GetComponentsInChildren<Panel>();
    foreach (Panel panel in panels) {
      panel.Setup(this);
    }
    currentPanel.Show();
  }

  private void ConnectToServer() {
    //@TODO 
    // - host & port input ui
    // - list (hardcode for now) of 'our' core servers
    Dispatcher.INSTANCE.connectToServer("localhost", 4296);
  }

  private void Update() {
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
