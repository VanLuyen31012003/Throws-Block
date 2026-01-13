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
		this._lengthSquare = this.GetComponent<RectTransform>().rect.width;
	}
	#endregion

	#region function logic
	#endregion

}
