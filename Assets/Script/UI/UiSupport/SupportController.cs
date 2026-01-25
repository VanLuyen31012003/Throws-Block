using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportController : Singleton<SupportController>
{
	#region item sp
	[SerializeField]	
	private ItemDragAndClick ItemShuffle;

	[SerializeField]
	private ItemDragAndClick ItemRocket;

	[SerializeField]
	private ItemDragAndClick ItemBowling;
	#endregion

	#region private field
	private ItemDragAndClick _ItemDragAndClick;
	#endregion

	#region public fields
	/// <summary>
	/// biến này để biết là có đang sử dụng sp không
	/// </summary>
	public bool IsUsingSP = false;
	#endregion

	private void Start()
	{
		this.SetDataDefault();
		this.Initialize();
	}
	public void Initialize()
	{
		this.ItemShuffle.SetData(this.GetTimesCanUseSp(ETypeItemClickDrag.SHUFFLE));
		this.ItemRocket.SetData(this.GetTimesCanUseSp(ETypeItemClickDrag.ROKET));
		this.ItemBowling.SetData(this.GetTimesCanUseSp(ETypeItemClickDrag.BOWLING));

	}
	public void OnPointerDown(ItemDragAndClick item)
    {
        this._ItemDragAndClick = item;
        this.IsUsingSP = true;
    }    

    /// <summary>
    /// Chọn item sp
    /// </summary>
    /// <param name="item"></param>
    public void SetItemSpClick(ItemDragAndClick item)
    {
		if (this.IsUsingSP||!PlayerController.Instance.IsEndTurn||this.GetTimesCanUseSp(item.TypeItemClickDrag)<=0)
			return;
        this._ItemDragAndClick= item;
		if(item.TypeItemClickDrag== ETypeItemClickDrag.SHUFFLE)
		{
			this.ExecuteSp(1,1);
		}	
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
					instanceGrid.DoSHUFFLE();
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
			this.IsUsingSP=false;
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
					this.ItemShuffle.SetData(times + countAppend);
					break;
				}
			case ETypeItemClickDrag.ROKET:
				{
					times = PlayerPrefs.GetInt(StaticControl.KEY_ROKET, 0);
					PlayerPrefs.SetInt(StaticControl.KEY_ROKET, times + countAppend);
					this.ItemRocket.SetData(times + countAppend);
					break;
				}
			case ETypeItemClickDrag.BOWLING:
				{
					times = PlayerPrefs.GetInt(StaticControl.KEY_BOWLING, 0);
					PlayerPrefs.SetInt(StaticControl.KEY_BOWLING, times + countAppend);
					this.ItemBowling.SetData(times + countAppend);
					break;
				}
		}
	}
	public void Refresh()
	{
		this._ItemDragAndClick = null;
		this.IsUsingSP=false;
	}
	/// <summary>
	/// cái hàm này để set dữ liệu của sp về mặc định phục vụ cho test, sau mà lên bản chính phải bỏ đi
	/// </summary>
	public void SetDataDefault()
	{
		PlayerPrefs.SetInt(StaticControl.KEY_BOWLING, 3);
		PlayerPrefs.SetInt(StaticControl.KEY_ROKET, 2);
		PlayerPrefs.SetInt(StaticControl.KEY_SHUFFLE, 1);
	}
}
public enum ETypeItemClickDrag { 
    SHUFFLE,
    ROKET,
    BOWLING
}
