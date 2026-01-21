using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportController : Singleton<SupportController>
{
	#region item sp
	#endregion
	private ItemDragAndClick _ItemDragAndClick;
    private bool _isDragging;
	// Update is called once per frame
	//   void Update()
	//   {
	//	//if (!_isDragging) return;
	//	//Vector2 localPoint;
	//	//RectTransformUtility.ScreenPointToLocalPointInRectangle(
	//	//	parentRect,
	//	//	Input.mousePosition,
	//	//	null,
	//	//	out localPoint
	//	//);
	//	//float clampedX = Mathf.Clamp(localPoint.x + dragOffset.x, this.minX, this.maxX);
	//	//frameRect.anchoredPosition = new Vector2(clampedX, frameRect.anchoredPosition.y);
	//}
	private void Start()
	{
		
	}
	public void Initialize()
	{

	}
	public void OnPointerDown(ItemDragAndClick item)
    {
        this._ItemDragAndClick = item;
        this._isDragging = true;
    }    
    private void OnPointerUp()
    {
        this._ItemDragAndClick = null;
		this._isDragging = false;
	}
    /// <summary>
    /// Chọn item sp
    /// </summary>
    /// <param name="item"></param>
    public void SetItemSpClick(ItemDragAndClick item)
    {
        this._ItemDragAndClick= item;
    }
	/// <summary>
	/// Thực thi item sp
    /// i , j là tọa độ của các item này 
	/// </summary>
	/// <param name="item"></param>
	public void ExecuteSp(int i,int j)
    {
        if (_ItemDragAndClick ==null)
            return;
        var instanceGrid = GameManager.Instance.GridManager;
        switch (this._ItemDragAndClick.TypeItemClickDrag)
        {
            case ETypeItemClickDrag.SHUFFLE:
                {
					UpdateTimesCanUseSP(ETypeItemClickDrag.SHUFFLE, -1);
					break;
                }
			case ETypeItemClickDrag.ROKET:
				{
                    instanceGrid.DoSpROCKET(i,j);
					UpdateTimesCanUseSP(ETypeItemClickDrag.ROKET, -1);
					break;
				}
			case ETypeItemClickDrag.BOWLING:
				{
					instanceGrid.DoSpBOWLING(i, j);
					UpdateTimesCanUseSP(ETypeItemClickDrag.BOWLING, -1);
					break;
				}
		}
        this._ItemDragAndClick = null;
    }
    /// <summary>
    /// Trả về thằng này đang có vật phẩm hỗ trợ nào không
    /// </summary>
    /// <returns></returns>
    public bool IsHavingSp()
    {
        return this._ItemDragAndClick==null?false:true;
    }
    /// <summary>
    /// Reset thằng này lại
    /// </summary>
    public void ResetItemSP()
    {
        if(this._ItemDragAndClick != null)
        {
			this._ItemDragAndClick = null;
		}
	}
    public int GetTimesCanUseSp(ETypeItemClickDrag type)
    {
        int times = 0;
        switch(type)
        {
            case ETypeItemClickDrag.SHUFFLE:
                {
					times = PlayerPrefs.GetInt(StaticControl.KEY_SHUFFLE, 0);
                    break;
                }
	        case ETypeItemClickDrag.ROKET:
		        {
					times = PlayerPrefs.GetInt(StaticControl.KEY_ROKET, 0);
					break;
		        }
	        case ETypeItemClickDrag.BOWLING:
		        {
					times = PlayerPrefs.GetInt(StaticControl.KEY_BOWLING, 0);
					break;
		        }
		}    
        return times;
    }
	public void UpdateTimesCanUseSP(ETypeItemClickDrag type, int countAppend)
	{
		int times = 0;
		switch (type)
		{
			case ETypeItemClickDrag.SHUFFLE:
				{
					times = PlayerPrefs.GetInt(StaticControl.KEY_SHUFFLE, 0);
					PlayerPrefs.SetInt(StaticControl.KEY_SHUFFLE, times + countAppend);
					break;
				}
			case ETypeItemClickDrag.ROKET:
				{
					times = PlayerPrefs.GetInt(StaticControl.KEY_ROKET, 0);
					PlayerPrefs.SetInt(StaticControl.KEY_ROKET, times + countAppend);
					break;
				}
			case ETypeItemClickDrag.BOWLING:
				{
					times = PlayerPrefs.GetInt(StaticControl.KEY_BOWLING, 0);
					PlayerPrefs.SetInt(StaticControl.KEY_BOWLING, times + countAppend);
					break;
				}
		}
	}
}
public enum ETypeItemClickDrag { 
    SHUFFLE,
    ROKET,
    BOWLING
}
