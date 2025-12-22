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

    #region Private Field
    /// <summary>
    /// List này sẽ quản lý các ô target trên UI
    /// </summary>
    private List<TargetPrefapUI> listTargetUI = new List<TargetPrefapUI>();
    #endregion

    #region Public Field
    #endregion

    #region Function Logic
    public void SetData(LevelConfig data)
	{
		this.Clear();
		this.NumberRemain.text=data.numberInARound.ToString();
		foreach(Target target in data.targets)
		{
			TargetPrefapUI targetPrefap = Instantiate<TargetPrefapUI>(this.targetItem,this.transformGoals);
			targetPrefap.SetData((ETypeBlock)target.type, target.countNeed);
			targetPrefap.gameObject.SetActive(true);
            listTargetUI.Add(targetPrefap);

        }	
	}
	/// <summary>
	/// cập nhật lại điều kiện thắng bàn
	/// </summary>
    public void SetTargetItem(ETypeBlock type, int totalRemaintPoint)
    {
        for (int i = 0; i < listTargetUI.Count; i++)
        {
            var targetUI = listTargetUI[i];

            if (targetUI.Type == type)
            {
                if (totalRemaintPoint <= 0)
                {
                    Destroy(targetUI.gameObject);
                    listTargetUI.RemoveAt(i);
                }
                else
                {
                    targetUI.SetNumberRemainPoint(totalRemaintPoint);
                }
                break;
            }
        }

    }
    /// <summary>
    /// cập nhật lại số lượt còn lại
    /// </summary>
    public void SetNumberRemain(int totalNumberTurnRemaint)
    {
        this.NumberRemain.text = totalNumberTurnRemaint.ToString();	
    }
	public void Clear()
	{
		// Reset số lượt
		if (NumberRemain != null)
			NumberRemain.text = "0";

		// Destroy toàn bộ target UI đang tồn tại
		for (int i = listTargetUI.Count - 1; i >= 0; i--)
		{
			if (listTargetUI[i] != null)
			{
				Destroy(listTargetUI[i].gameObject);
			}
		}

		// Clear list quản lý
		listTargetUI.Clear();
	}
	#endregion
}
