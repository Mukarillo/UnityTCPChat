using UnityEngine;

public class DonationButton : MediaButton
{
	protected override string buttonText => "Donate";   
	protected override string buttonSpriteName => "donation";

	protected override void Awake()
	{
		base.Awake();
  
		ChatController.ME.onPlayerOnlineChanged.AddListener(OnPlayerAmountChanged);
    }

	private void OnPlayerAmountChanged(int amount)
    {
		gameObject.SetActive(amount > 1);
    }
       
	protected override void OnClick()
	{
		MediaController.ME.OpenSecondaryPanel<DonatePanel>("DonatePanel");
	}
}
