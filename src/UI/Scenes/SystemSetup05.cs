using Godot;
using System.Threading.Tasks;
using Core.Managers;
using UI.Components;
using Utils;

public partial class SystemSetup05 : SystemMenuController
{
	[Export] public ProfileNamesData ProfileNames { get; set; }
	[Export] private CustomButton RandomNameButton { get; set; }
	[Export] private CustomEntryField ProfileNameField  { get; set; }
	[Export] private CustomButton NextButton  { get; set; }

	public override void _Ready()
	{
		base._Ready();
		RandomNameButton.Pressed += _onRandomNameButtonPressed;
		ProfileNameField.TextChanged += _onProfileNameFieldUpdated;
		NextButton.FocusEntered += _onNextButtonDisabledPressed;
	}

	private void _onNextButtonDisabledPressed()
	{
		if (!NextButton.Disabled) return;
		SoundManager.Instance.PlaySound("Wrong");
		
		var notification = ObjectHelper.Create<Notification>("Notification", Vector2.Zero, GetTree().CurrentScene.GetPath());
		notification.Message = "Name must have more than 3 characters";
		notification._ConfirmButtonText = "OK";
		notification.OneButton = true;
		notification.UpdateUI();
	}
	
	private void _onProfileNameFieldUpdated(string text)
	{
		if (text.Length < 3)
		{
			NextButton.Disabled = true;
		}
		else
		{
			NextButton.Disabled = false;
		}
	}
	
	private void _onRandomNameButtonPressed()
	{
		var index = GD.RandRange(0, ProfileNames.Names.Count - 1);
		var name = ProfileNames.Names[index];
		ProfileNameField.Text = name;
		_onProfileNameFieldUpdated(ProfileNameField.Text);
	}
}
