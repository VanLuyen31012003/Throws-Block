using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell :MonoBehaviour, IPointerClickHandler
{
	#region serilize field
	/// <summary>
	/// prefap của  square ô xanh
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapWaterMelon;

	/// <summary>
	/// prefap của  square ô đỏ
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapApple;

	/// <summary>
	/// prefap của  square ô vàng
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapMango;

	/// <summary>
	/// prefap của  square ô xanh dương
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapCherry;
	/// <summary>
	/// prefap của  square ô peach
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapPeach;

	/// <summary>
	/// fx của cell lúc bắn
	/// </summary>
	[SerializeField]
	private GameObject FxAnim;

	/// <summary>
	/// text tổng số square block ở trên cùng chung loại
	/// </summary>
	public TextMeshProUGUI TotalNumberSquareTopSameType;
	#endregion

	#region  Private Field
	/// <summary>
	/// _image renderer quản lý ảnh plus
	/// </summary>
	private Image _image;

	/// <summary>
	/// Quản lý độ dài của cái ô square này
	/// </summary>
	private float _lengthSquare;
	#endregion

	#region public field
	/// <summary>
	/// tọa độ của ô cell này
	/// </summary>
	public int x=-1,y=-1;

	/// <summary>
	/// các đối tượng square 
	/// </summary>
	public List<GameObject> lstBlock = new List<GameObject>();
	#endregion

	#region Function MonoBehaviour
	private void Awake()
	{
		this._lengthSquare = this.GetComponent<RectTransform>().rect.width;
		this._image= this.GetComponent<Image>();
	}
	#endregion

	#region function logic
	/// <summary>
	/// hàm này để thêm gạch vào cho ô này
	/// </summary>
	/// <param name="type"></param>
	public void SpawnBlock(ETypeBlock type,int count)
	{
		GameObject prefapInst = null;
		switch (type)
		{
			case ETypeBlock.APPLE:
				prefapInst = this.SquarePrefapApple;
				break;
			case ETypeBlock.WATERMELON:
				prefapInst = this.SquarePrefapWaterMelon;
				break;
			case ETypeBlock.MANGO:
				prefapInst = this.SquarePrefapMango;
				break;
			case ETypeBlock.CHERRY:	
				prefapInst = this.SquarePrefapCherry;
				break;
			case ETypeBlock.PEACH:
				prefapInst = this.SquarePrefapPeach;
				break;
			case ETypeBlock.NONE:
				break;

		}
		if(prefapInst!=null)
		{
			for (int i = 0; i < count; i++)
			{
				GameObject block = Instantiate(prefapInst, this.transform);
				float y = (float)(lstBlock.Count)*6;
				block.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
				block.SetActive(true);
				lstBlock.Add(block);
			}
		}	
		this.SetTextNumberTotalSameType();
	}
    /// <summary>
    /// hàm này dùng để spawn block vào cell cộng thêm khi ăn điểm lớn hơn 10
    /// cái listcount này để set thứ tự hiển thị cho đúng
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void SpawnBlockAnim(ETypeBlock type, int count,int listCount)
    {
        GameObject prefapInst = null;
        switch (type)
        {
            case ETypeBlock.APPLE:
                prefapInst = this.SquarePrefapApple;
                break;
            case ETypeBlock.WATERMELON:
                prefapInst = this.SquarePrefapWaterMelon;
                break;
            case ETypeBlock.MANGO:
                prefapInst = this.SquarePrefapMango;
                break;
            case ETypeBlock.CHERRY:
                prefapInst = this.SquarePrefapCherry;
                break;
            case ETypeBlock.NONE:
                break;

        }
        if (prefapInst != null)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject block = Instantiate(prefapInst, this.transform);
                float y = (float)(lstBlock.Count + 1)*StaticControl.VALUE_DEVIDE;
                block.transform.localPosition = new Vector2(0, y);
                block.SetActive(true);
                lstBlock.Add(block);
            }
        }
        this.TotalNumberSquareTopSameType.gameObject.SetActive(true);
        this.TotalNumberSquareTopSameType.text = this.GetTotalSuareSameTypeOntop().ToString();
        this.TotalNumberSquareTopSameType.gameObject.transform.position = lstBlock.Last().transform.position;
		this.TotalNumberSquareTopSameType.transform.SetAsLastSibling();


	}
    /// <summary>
    /// Tính toán lại tổng số ô trên cùng có same type
    /// </summary>
    public void SetTextNumberTotalSameType(bool isFirstCol=false)
	{
		//this.TotalNumberSquareTopSameType.text = this.GetTotalSuareSameTypeOntop().ToString();
		// tức là nó đang rỗng
		this.TotalNumberSquareTopSameType.gameObject.SetActive(true);
		if (lstBlock.Count<=0)
		{
			if(isFirstCol)
			{
				this.TotalNumberSquareTopSameType.text = "+";
				this._image.enabled = true;
			}
			else
			{
				this.TotalNumberSquareTopSameType.text = "";
				this._image.enabled = false;
			}
			this.TotalNumberSquareTopSameType.gameObject.transform.position = this.transform.position;
		}
		else
		{
			this.TotalNumberSquareTopSameType.text = this.GetTotalSuareSameTypeOntop().ToString();
			this.TotalNumberSquareTopSameType.gameObject.transform.position = lstBlock.Last().transform.position;
			this._image.enabled = false;
		}
		this.TotalNumberSquareTopSameType.transform.SetAsLastSibling();

	}
	public void SetVisibleTextNumberTotalSameType(bool value)
	{
		this.TotalNumberSquareTopSameType.gameObject.SetActive(value);	
	}

	/// <summary>
	///  thêm các ô vuông vào cell
	/// </summary>
	/// <param name="type"></param>
	/// <param name="block"></param>
	public void AddSquare(GameObject block)
	{
		block.transform.SetParent(this.transform);
		float y = (float)(lstBlock.Count) * StaticControl.VALUE_DEVIDE;
		block.transform.localPosition = new Vector2(0, y);
		lstBlock.Add(block);
		this.SetTextNumberTotalSameType();
	}

	/// <summary>
	/// Lấy ra loại ô cuối cùng trên bề mặt
	/// </summary>
	/// <returns></returns>
	public ETypeBlock GetLastSquareType()
	{ 
		if (this.lstBlock == null)
			return ETypeBlock.NONE	 ;
		if(this.lstBlock.Count <1)
		{
			return ETypeBlock.NONE; ;
		}	
		return this.lstBlock[lstBlock.Count-1].GetComponent<Square>().typeBlock;
	}
	
	/// <summary>
	/// Kiểm tra đủ điều kiện merge 2 cell không
	/// 1 là phải khác none
	/// 2 là phải cùng typeblock
	/// </summary>
	/// <returns></returns>
	public bool CheckMergeCondition(ETypeBlock typeBlock)
	{
		if(typeBlock==this.GetLastSquareType()&& typeBlock!=ETypeBlock.NONE)
		{
			return true;
		}	
		return false;
	}

	/// <summary>
	/// Lấy ra tất cả các suqare block có cùng kiểu ở đầu
	/// Lấy ra xong rồi cũng xóa nó luôn trong list của cell hiện tại nhé
	/// Còn nếu chỉ muốn lấy ra list thì set cái isGetCount về true;
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public List<GameObject> GetListSameTypeFirst(ETypeBlock type,bool isGetCount=false)
	{
		List<GameObject> list = new List<GameObject>();
		int indexRemove = 0;
		for(int i=this.lstBlock.Count-1;i>=0;i--)
		{
			if (this.lstBlock[i].GetComponent<Square>().typeBlock == type)
			{
				list.Add(lstBlock[i]);
				indexRemove = i;
			}
			// nếu k phải thì break luôn, k cần lấy các thằng sau nữa
			else
				break;
		}
		if(!isGetCount)
		{
			this.lstBlock.RemoveRange(indexRemove, list.Count);
			this.SetTextNumberTotalSameType();
		}		
		return list;
	}
	/// <summary>
	/// Lấy ra tổng số lượng các prefap ở đầu có cùng kiểu
	/// </summary>
	/// <param name="type"></param>
	/// <param name="isGetCount"></param>
	/// <returns></returns>
	public int GetTotalSuareSameTypeOntop()
	{
		List<GameObject> list = new List<GameObject>();
		ETypeBlock type = this.GetLastSquareType();

		for (int i = this.lstBlock.Count - 1; i >= 0; i--)
		{
			if (this.lstBlock[i].GetComponent<Square>().typeBlock == type)
			{
				list.Add(lstBlock[i]);
			}
			// nếu k phải thì break luôn, k cần lấy các thằng sau nữa
			else
				break;
		}

		return list.Count;
	}
	/// <summary>
	/// Lấy ra tổng số lượng các prefap trong cell cùng kiểu truyền vào
	/// </summary>
	/// <param name="type"></param>
	/// <param name="isGetCount"></param>
	/// <returns></returns>
	public int GetTotalSuareSameTypeInCell(ETypeBlock type)
	{
		int count = 0;

		for (int i = this.lstBlock.Count - 1; i >= 0; i--)
		{
			if (this.lstBlock[i].GetComponent<Square>().typeBlock == type)
			{
				count++;
			}
		}

		return count;
	}
	/// <summary>
	/// Clear dữ liệu trong list này đi đồng thời destroy gameobj luôn
	/// </summary>
	public void ClearAndDestroyListGameObj()
	{
		foreach(GameObject gameobj in this.lstBlock)
		{
			Destroy(gameobj);
		}
		this.ClearListGameObj();
		// đáng nhẽ sẽ clear list đi nhưng mà do cái này bị trỏ chung vùng nhớ
	}
	/// <summary>
	/// Clear dữ liệu list trong cell này đi
	/// </summary>
	public void ClearListGameObj()
	{
		this.lstBlock.Clear();
	}
	public void DestroyListGameObj()
	{
		foreach(var square in this.lstBlock)
		{
			Destroy(square);
		}	
		this.lstBlock.Clear();
		this.SetTextNumberTotalSameType();
	}
	/// <summary>
	/// Clear các square có cùng type ở đầu
	/// </summary>
	public void ClearListSquareHaveSameTypeOnTop()
	{
		ETypeBlock type = this.GetLastSquareType();

		for (int i = this.lstBlock.Count - 1; i >= 0; i--)
		{
			if (this.lstBlock[i].GetComponent<Square>().typeBlock == type)
			{
				/// xóa square đó
				Destroy(this.lstBlock[i]);
				/// sau đó xóa đối tượng square đó trong lstBlock
				lstBlock.RemoveAt(i);

			}
			else
				break;
		}
		this.SetTextNumberTotalSameType();
	}
	#endregion

    #region Set fx
	public void SetFxVisible(bool value)
	{
		this.FxAnim.SetActive(value);
	}
	public void SetImageVisible(bool value)
	{
		this._image.enabled = value;
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked UI Object");
		if (!SupportController.Instance.IsHavingSp())
		{
			return;
		}
		SupportController.Instance.ExecuteSp(this.x,this.y);
	}
	#endregion
}
public enum ETypeBlock
{
	NONE=0,
	APPLE=1,
	WATERMELON=2,
	CHERRY=3,
	MANGO=4,
	PEACH=5

}
