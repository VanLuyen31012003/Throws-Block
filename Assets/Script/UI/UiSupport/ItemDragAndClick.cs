using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragAndClick : MonoBehaviour
{
	#region Private Fields
	[SerializeField]
	ETypeItemClickDrag TypeItem;

	[SerializeField]
	private Button UIButton_Select;

	[SerializeField]
	private TextMeshProUGUI UIText_Times;
	#endregion

	#region Public Fields
	public ETypeItemClickDrag TypeItemClickDrag => TypeItem;
	#endregion

	#region function monobehaviour
	private void Start()
	{
		Initialize();
	}
    private void OnEnable()
    {
        SubscribeEventSystem();
    }
	private void OnDisable()
	{
		UnSubscribeEventSystem();
    }
    #endregion

    #region function logic
    private void Initialize()
	{
		UIHelper.AddButtonClickNormal(UIButton_Select, UIButton_Select_Clicked);
    }	
	private void UIButton_Select_Clicked()
	{
        SupportController.Instance.SetItemSpClick(this);
	}
	public void SetData(int count)
	{
        if (count<=0)
        {	
			UIText_Times.text = "+";	
			Utils.SetActiveButton(UIButton_Select, false);
        }
		else
			UIText_Times.text = count.ToString();
	}
	private void SubscribeEventSystem()
	{
		//EventSystem.Subscribe(StaticControl.ActionWhenStartPlay+ TypeItemClickDrag.ToString(), ()=> { Utils.SetActiveButton(UIButton_Select, false); });
  //      EventSystem.Subscribe(StaticControl.ActionWhenEndPlay + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, true); });

        EventSystem.Subscribe(StaticControl.ActionWhenStartUsingSp + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, false); });
        EventSystem.Subscribe(StaticControl.ActionWhenEndUsingSp + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, true); });

    }
    private void UnSubscribeEventSystem()
    {
        //EventSystem.Unsubscribe(StaticControl.ActionWhenStartPlay + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, false); });
        //EventSystem.Unsubscribe(StaticControl.ActionWhenEndPlay + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, true); });
        EventSystem.Unsubscribe(StaticControl.ActionWhenStartUsingSp + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, false); });
        EventSystem.Unsubscribe(StaticControl.ActionWhenEndUsingSp + TypeItemClickDrag.ToString(), () => { Utils.SetActiveButton(UIButton_Select, true); });
    }
    #endregion
}
