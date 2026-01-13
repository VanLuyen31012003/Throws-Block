using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellBackGround : MonoBehaviour
{
	/// <summary>
	/// ảnh red
	/// </summary>
	[SerializeField]
	private Sprite BgOdd;

	/// <summary>
	/// ảnh green
	/// </summary>
	[SerializeField]
	private Sprite BgEven;

	public void SetData(int type)
	{
		this.GetComponent<Image>().sprite =type%2==0? this.BgEven:this.BgOdd;
    }
}
