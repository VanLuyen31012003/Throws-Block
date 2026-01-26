using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAspectRation : MonoBehaviour
{
    [SerializeField]
    private CanvasScaler canvas;

	private void Awake()
	{
		this.ApplyMatch();
	}
	/// <summary>
	/// hàm tính toán match aspect cho màn hình 
	/// </summary>
	private void ApplyMatch()
	{
		float aspect = (float)Screen.width / Screen.height;

		// Ví dụ rule
		if (aspect >= 1.5f)
		{
			// Màn hình rất dài (18:9, 19.5:9)
			canvas.matchWidthOrHeight = 1f;
		}
		else
		{
			canvas.matchWidthOrHeight = 0f;
		}

	}	
}
