using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {
  
  private Canvas canvas = null;

  private void Awake() {
    canvas = GetComponent<Canvas>();
  }

  public void Setup() {
    Hide();
  }

  public void Show() {
    canvas.enabled = true;
  }

  public void Hide() {
    canvas.enabled = false;
  }
}
