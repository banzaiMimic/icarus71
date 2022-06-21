using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {
  
  private Canvas canvas = null;
  private MenuManager menuManager = null;

  private void Awake() {
    this.canvas = GetComponent<Canvas>();
  }

  public void Setup(MenuManager menuManager) {
    this.menuManager = menuManager;
    Hide();
  }

  public void Show() {
    this.canvas.enabled = true;
  }

  public void Hide() {
    this.canvas.enabled = false;
  }
}
