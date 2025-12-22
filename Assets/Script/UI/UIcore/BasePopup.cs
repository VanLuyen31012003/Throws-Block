using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public virtual void ShowPopup()
    {
        this.gameObject.SetActive(true);
    }
	public virtual void HidePopup()
	{
		this.gameObject.SetActive(false);
	}
}

