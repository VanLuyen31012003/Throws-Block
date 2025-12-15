using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Square : MonoBehaviour
{
	#region serilize field
	/// <summary>
	/// độ dài cộng thêm cho tia ray
	/// </summary>
	[SerializeField]	
	private float length = 0.1f;
	#endregion

	#region public and private field
	/// <summary>
	/// loại block
	/// </summary>
	public ETypeBlock typeBlock;

	/// <summary>
	/// rigid quản lý tác động vật lý
	/// </summary>
	private Rigidbody2D rb;

	/// <summary>
	/// Quản lý độ dài của cái ô square này
	/// </summary>
	private float _lengthSquare;
	#endregion

	#region function monobehaviour
	private void Awake()
	{
		this.rb = GetComponent<Rigidbody2D>();	
		this._lengthSquare = this.GetComponent<SpriteRenderer>().bounds.size.x;
	}

	private void Update()
	{
		//if (this.rb.velocity.y>0)
		//{
		//	ShootRaycast();
		//}
	}
	#endregion

	#region function logic
	public void Move(float speed)
	{
		Vector2 veloCache = rb.velocity;
		this.rb.velocity = new Vector2(veloCache.x, speed);
	}
	public void ShootRaycast()
	{
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.up,this._lengthSquare/2 +this.length, GameManager.Instance.LayerMaskCell);
		Debug.DrawRay(this.transform.position, Vector2.up*(this._lengthSquare/2 + this.length) , Color.red, Time.deltaTime);
		if (hit.collider != null&& hit.collider.CompareTag("Cell"))
		{
			if (hit.distance<=this._lengthSquare)
			{
				/// lấy ra thằng cell mà nó bắn ray cast trúng
				Cell cellCollison  =hit.collider.gameObject.GetComponent<Cell>();
				this.rb.velocity = Vector2.zero;
				// check xem có merge được với ô chúng kia không
				//if(GameManager.Instance.GridManager.CanMerge(cellCollison.x, cellCollison.y, this.gameObject))
				//{
				//	GameManager.Instance.GridManager.SnapMerge(cellCollison.x, cellCollison.y, this.gameObject);
				//}

			}		
		}
	}
	#endregion

}
