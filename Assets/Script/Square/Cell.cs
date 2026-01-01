using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell :MonoBehaviour
{
	#region serilize field
	/// <summary>
	/// prefap của  square ô xanh
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapGreen;

	/// <summary>
	/// prefap của  square ô đỏ
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapRed;

	/// <summary>
	/// prefap của  square ô vàng
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapYellow;

	/// <summary>
	/// prefap của  square ô xanh dương
	/// </summary>
	[SerializeField]
	private GameObject SquarePrefapBlue;

	/// <summary>
	/// text tổng số square block ở trên cùng chung loại
	/// </summary>
	[SerializeField]
	private TextMesh TotalNumberSquareTopSameType;

	/// <summary>
	/// độ dài cộng thêm cho tia ray
	/// </summary>
	[SerializeField]
	private float length = 0.1f;
	#endregion

	#region  Private Field
	/// <summary>
	/// rigid quản lý tác động vật lý
	/// </summary>
	private Rigidbody2D rb;

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
		this.rb = GetComponent<Rigidbody2D>();
		this._lengthSquare = this.GetComponent<SpriteRenderer>().bounds.size.x;
	}
	private void Update()
	{
		if (this.rb.velocity.y > 0)
		{
			ShootRaycast();
		}
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
			case ETypeBlock.RED:
				prefapInst = this.SquarePrefapRed;
				break;
			case ETypeBlock.GREEN:
				prefapInst = this.SquarePrefapGreen;
				break;
			case ETypeBlock.YELLOW:
				prefapInst = this.SquarePrefapYellow;
				break;
			case ETypeBlock.BLUE:	
				prefapInst = this.SquarePrefapBlue;
				break;
			case ETypeBlock.NONE:
				break;

		}
		if(prefapInst!=null)
		{
			for (int i = 0; i < count; i++)
			{
				GameObject block = Instantiate(prefapInst, this.transform);
				float y = (float)(lstBlock.Count + 1) / 15f;
				block.transform.localPosition = new Vector2(0, y);
				//block.transform.localPosition = Vector2.zero;	

				//Debug.Log("x:"+block.transform.position.x);
				//Debug.Log("y:" + block.transform.position.y);
				// thêm thằng này đê nó hiển thị trên
				block.GetComponent<SpriteRenderer>().sortingOrder = lstBlock.Count + 1;
				block.SetActive(true);
				lstBlock.Add(block);
			}
		}	
		this.SetTextNumberTotalSameType();
	}
	/// <summary>
	/// Tính toán lại tổng số ô trên cùng có same type
	/// </summary>
	public void SetTextNumberTotalSameType()
	{
		//this.TotalNumberSquareTopSameType.text = this.GetTotalSuareSameTypeOntop().ToString();
		// tức là nó đang rỗng
		this.TotalNumberSquareTopSameType.gameObject.SetActive(true);
		if (lstBlock.Count<=0)
		{
			this.TotalNumberSquareTopSameType.text = "+";
			this.TotalNumberSquareTopSameType.gameObject.transform.position = this.transform.position;
		}
		else
		{
			this.TotalNumberSquareTopSameType.text = this.GetTotalSuareSameTypeOntop().ToString();
			this.TotalNumberSquareTopSameType.gameObject.transform.position = lstBlock.Last().transform.position;
		}
		this.TotalNumberSquareTopSameType.GetComponent<Renderer>().sortingOrder = lstBlock.Count;

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
		float y = (float)(lstBlock.Count + 1) / 15f;
		block.transform.localPosition = new Vector3(0, y);
		block.GetComponent<SpriteRenderer>().sortingOrder = lstBlock.Count + 1;
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

	#region Move and Collider
	public void Move(float speed)
	{
		Vector2 veloCache = rb.velocity;
		this.rb.velocity = new Vector2(veloCache.x, speed);
	}
	public void ShootRaycast()
	{
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.up, this._lengthSquare / 2 + this.length, GameManager.Instance.LayerMaskCell);
		Debug.DrawRay(this.transform.position, Vector2.up * (this._lengthSquare / 2 + this.length), Color.red, Time.deltaTime);
		if (hit.collider != null && hit.collider.CompareTag("Cell"))
		{
			if (hit.distance <= this._lengthSquare)
			{
				/// lấy ra thằng cell mà nó bắn ray cast trúng
				Cell cellCollison = hit.collider.gameObject.GetComponent<Cell>();
			
				// keiemr trả xem ô phía trên ô bắn tia ray trúng đang có dữ liệu không, nếu có thì gọi hàm merge, khôn thì bay tiếp
				if(GameManager.Instance.GridManager.CheckNone(cellCollison.x, cellCollison.y))
				{
					this.rb.velocity = Vector2.zero;
					GameManager.Instance.GridManager.MergeToNoneBlock(cellCollison.x, cellCollison.y, this);

				}
			}
		}
	}
	public void JumpSquareToCell(GameObject square, Cell targetCell, int order)
	{
		square.transform.SetParent(null); // tạm thời tách ra

		Vector3 startPos = square.transform.position;

		// vị trí đích dựa theo số block hiện có
		Vector3 targetPos = targetCell.transform.position +
			Vector3.up * ((targetCell.lstBlock.Count + 1) / 15f);

		float duration = 0.25f;
		float delay = order * 0.05f;

		square.transform
			.DOJump(targetPos, 0.6f, 1, duration)
			.SetDelay(delay)
			.SetEase(Ease.OutQuad)
			.OnComplete(() =>
			{
				// khi nhảy xong mới add vào cell
				targetCell.AddSquare(square);
			});
	}
	// trả về vị trí nên xếp
	public Vector3 GetNextSquareWorldPos()
	{
		float offsetY = 0.15f; // khoảng cách xếp chồng
		return transform.position + Vector3.up * lstBlock.Count * offsetY;
	}
	/// <summary>
	/// Lấy ra thứ tự sorting layer nên set
	/// </summary>
	/// <returns></returns>
	public int GetSlotSortingLayer( int indexSquare)
	{
		return this.lstBlock.Count+ indexSquare;
	}
	/// <summary>
	/// Trả về vị trí để merge anim
	/// </summary>
	/// <returns></returns>
	public Vector3 GetPostionMove(int indexSquare)
	{
		Vector3 pos = this.transform.position;
		pos.y = (float)(lstBlock.Count + indexSquare) / 15f;
		return pos;
	}
	#endregion
}
public enum ETypeBlock
{
	NONE=0,
	RED=1,
	GREEN=2,
	BLUE=3,
	YELLOW=4,

}
