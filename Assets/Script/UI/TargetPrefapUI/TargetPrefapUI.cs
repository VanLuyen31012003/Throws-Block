using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetPrefapUI : MonoBehaviour
{
	#region
	/// <summary>
	/// số ô cần
	/// </summary>
	[SerializeField]
	private TextMeshProUGUI UIText_NumberNeed;

	/// <summary>
	/// ảnh red
	/// </summary>
	[SerializeField]
	private Sprite Red;

	/// <summary>
	/// ảnh green
	/// </summary>
	[SerializeField]
	private Sprite Green;

	/// <summary>
	/// ảnh blue
	/// </summary>
	[SerializeField]
	private Sprite Blue;

	/// <summary>
	/// ảnh yellow
	/// </summary>
	[SerializeField]
	private Sprite Yellow;
	#endregion

	#region Public Field
	/// <summary>
	/// type của ô
	/// </summary>
	public ETypeBlock Type;
    #endregion

    #region Function MonoBehaviour
    //public void Start()
    //{
    //	this.GetComponent<Image>().sprite=null;
    //}
    #endregion

    #region Function Logic
    /// <summary>
    /// hàm cập nhật số lượng và hình ảnh
    /// </summary>
    /// <param name="type"></param>
    /// <param name="numberNeed"></param>
    public void SetData(ETypeBlock type, int numberNeed)
	{
		this.UIText_NumberNeed.text= numberNeed.ToString();
		switch (type)
		{
			case ETypeBlock.RED:
				this.GetComponent<Image>().sprite = this.Red;
				this.Type = ETypeBlock.RED;
                break;
			case ETypeBlock.GREEN:
				this.GetComponent<Image>().sprite = this.Green;
                this.Type = ETypeBlock.GREEN;
                break;
			case ETypeBlock.YELLOW:
				this.GetComponent<Image>().sprite = this.Yellow;
                this.Type = ETypeBlock.YELLOW;
                break;
			case ETypeBlock.BLUE:
				this.GetComponent<Image>().sprite = this.Blue;
                this.Type = ETypeBlock.BLUE;
                break;
		}
	}
	public void SetNumberRemainPoint(int totalRemainPoint)
	{
		this.UIText_NumberNeed.text = totalRemainPoint.ToString();
    }	
	#endregion
}
