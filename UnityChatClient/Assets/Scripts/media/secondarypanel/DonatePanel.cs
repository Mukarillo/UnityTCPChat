using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DonatePanel : MediaSecondaryPanelComponent
{   
	public Dropdown playersDropdown;
	public Text amount;

	private string mDonatingPlayer;
	private int mTotalAmount;
   
	public override void Initiate()
    {
		Debug.Log("INITIATING DONATE");
		ChatController.ME.onPlayerOnlineChanged.AddListener(OnPlayerAmountChanged);

		ChangeTotalAmount(0);
        
		var options = new List<Dropdown.OptionData>();
		foreach(var n in ChatController.ME.playersOnline)
		{
			if (n.ToLower() == "you")
				continue;
			options.Add(new Dropdown.OptionData(n));
		}

		playersDropdown.options = options;
		mDonatingPlayer = options[0].text;
		playersDropdown.onValueChanged.AddListener(DropdownValueChanged);
    }

	private void OnPlayerAmountChanged(int amount)
    {
        gameObject.SetActive(amount > 1);
    }

    public void AddDonate(int amount)
	{
		mTotalAmount = Mathf.Max(0, mTotalAmount + amount);
		ChangeTotalAmount(mTotalAmount);
	}

	void DropdownValueChanged(int dropdownIndex)
	{
		mDonatingPlayer = playersDropdown.options[dropdownIndex].text;
	}
 
    public void Donate()
	{
		if (mTotalAmount <= 0)
			return;
		
		MediaController.ME.SendDonation(mDonatingPlayer, mTotalAmount);
		MediaController.ME.ForceCloseMedia();
	}

    private void ChangeTotalAmount(int amount)
	{
		this.amount.text = string.Format("{0}$", amount);
	}

	public override void Dispose()
	{
        Debug.Log("DISPOSING DONATE");
		ChatController.ME.onPlayerOnlineChanged.RemoveListener(OnPlayerAmountChanged);
	}   
}
