using System.Linq;
using Core.Managers;
using Godot;

namespace UI.Components;

[GlobalClass]
public partial class CustomOptionButton : OptionButton
{
	private ButtonAnimator _animator;
	private PopupAnimator _PopupAnimator;

	[Export] private string ClickSound { get; set; } = "Click";

	private PopupMenu _popup;
	private PanelContainer _PopupContainer;
	
	public override void _Ready()
	{
		_animator = new ButtonAnimator(this);

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		Pressed += OnPressed;
		ItemSelected += OnItemSelected;
		
		_popup = GetPopup();
		_popup.HideOnCheckableItemSelection = false;
		
		_PopupContainer = _popup.GetChild<PanelContainer>(0, true);
		_PopupContainer.PivotOffset = _popup.Size / 2;
		
		_PopupAnimator = new PopupAnimator(_PopupContainer, _popup);
	}

	public override void _ExitTree()
	{
		MouseEntered -= OnMouseEntered;
		MouseExited -= OnMouseExited;
		Pressed -= OnPressed;

		base._ExitTree();
	}

	private void OnMouseEntered()
	{
		SoundManager.Instance.PlaySound("ButtonHover");
		_animator.PlayAnim(ButtonAnimator.Animations.Hover);
	}

	private void OnMouseExited()
	{
		_animator.PlayAnim(ButtonAnimator.Animations.HoverOut);
	}

	private void OnPressed()
	{
		_animator.PlayAnim(ButtonAnimator.Animations.Click);	
		_PopupAnimator.PlayAnim(PopupAnimator.Animations.In);
		
		SoundManager.Instance.PlaySound(ClickSound);
	}

	private void OnItemSelected(long id)
	{
		_PopupAnimator.PlayAnim(PopupAnimator.Animations.Out);
	}
}
