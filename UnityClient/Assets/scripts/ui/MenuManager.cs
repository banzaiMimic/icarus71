using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

  [SerializeField]
  private Panel currentPanel = null;
  private List<Panel> panelHistory = new List<Panel>();

  private void Start() {
    SetupPanels();
  }

  private void SetupPanels() {
    Panel[] panels = GetComponentsInChildren<Panel>();

    foreach (Panel panel in panels) {
      panel.Setup(this);
    }

    this.currentPanel.Show();
  }

}