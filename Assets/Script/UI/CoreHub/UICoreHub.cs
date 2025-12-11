using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoreHub : MonoBehaviour
{
	#region Serilize Field
	/// <summary>
	/// text số lượt còn lại
	/// </summary>
	[SerializeField]
	private TextMeshProUGUI NumberRemain;

	/// <summary>
	/// transform của ô target
	/// </summary>
	[SerializeField]
	private Transform transformGoals;

	/// <summary>
	/// prefap ô target
	/// </summary>
	[SerializeField]
	private TargetPrefapUI  targetItem;
	#endregion

	#region Public Field
	#endregion

	#region Function Logic
	public void SetData(LevelConfig data)
	{
		this.NumberRemain.text=data.numberInARound.ToString();
		foreach(Target target in data.targets)
		{
			TargetPrefapUI targetPrefap = Instantiate<TargetPrefapUI>(this.targetItem,this.transformGoals);
			targetPrefap.SetData((ETypeBlock)target.type, target.countNeed);
			targetPrefap.gameObject.SetActive(true);
		}	
	}

	#endregion
}
