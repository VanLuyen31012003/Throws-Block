using System.Collections;
using System.Collections.Generic;
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
	#endregion

	public List<GameObject> lstBlock = new List<GameObject>();

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
		}
		for (int i = 0; i < count; i++)
		{
			GameObject block = Instantiate(prefapInst, transform);
			float y = (float)(lstBlock.Count + 1) / 15f;
			block.transform.localPosition = new Vector2(0,y);
			//block.transform.localPosition = Vector2.zero;	
				
			//Debug.Log("x:"+block.transform.position.x);
			//Debug.Log("y:" + block.transform.position.y);
			// thêm thằng này đê nó hiển thị trên
			block.GetComponent<SpriteRenderer>().sortingOrder = lstBlock.Count+1;

			lstBlock.Add(block);
		}
	}
	#endregion
}
public enum ETypeBlock
{
	RED=0,
	GREEN=1,
	BLUE=2,
	YELLOW=3
}
