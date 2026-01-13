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
	private Sprite Apple;

	/// <summary>
	/// ảnh green
	/// </summary>
	[SerializeField]
	private Sprite WaterMelon;

	/// <summary>
	/// ảnh blue
	/// </summary>
	[SerializeField]
	private Sprite Cherry;

	/// <summary>
	/// ảnh yellow
	/// </summary>
	[SerializeField]
	private Sprite Mango;
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
    //	this.GetComponent<Image>()._image=null;
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
			case ETypeBlock.APPLE:
				this.GetComponent<Image>().sprite = this.Apple;
				this.Type = ETypeBlock.APPLE;
                break;
			case ETypeBlock.WATERMELON:
				this.GetComponent<Image>().sprite = this.WaterMelon;
                this.Type = ETypeBlock.WATERMELON;
                break;
			case ETypeBlock.MANGO:
				this.GetComponent<Image>().sprite = this.Mango;
                this.Type = ETypeBlock.MANGO;
                break;
			case ETypeBlock.CHERRY:
				this.GetComponent<Image>().sprite = this.Cherry;
                this.Type = ETypeBlock.CHERRY;
                break;
		}
	}
	public void SetNumberRemainPoint(int totalRemainPoint)
	{
		this.UIText_NumberNeed.text = totalRemainPoint.ToString();
    }	
	#endregion
}
